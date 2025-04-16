using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace CreditStatistics
{
    public class cCreditInfo
    {
        public DateTime tCompleted;
        public int nCnt;
        public bool bValid;
        public double ElapsedSecs;
        public double CPUtimeSecs;
        public double Credits;
        public double mELA;
        public double mCPU;

        public void Init()
        {
            Credits = 0;
            ElapsedSecs = 0;
            CPUtimeSecs = 0;
            mELA = 0;
            mCPU = 0;
            nCnt = 0;
        }
    }

    internal class globals
    {
        public static class Utils
        {
            // pad right side with spaces to fill
            public static string Rp(string strIn, int cnt)
            {
                int i = cnt - strIn.Length;
                if (i < 0) return strIn.Substring(0, cnt);
                return strIn + "                              ".Substring(0, i);
            }

            // pad left side with spaces to fill
            public static string Lp(string strIn, int cnt)
            {
                int i = cnt - strIn.Length;
                if (i < 0) return strIn.Substring(0, cnt);
                return "                                               ".Substring(0, i) + strIn;
            }

            public static string[] FindHdr = { "All (", "Valid (", "Invalid (", "Error (" };
            public static string[] FindHdrA = { "All</a> (", "Valid</a> (", "Invalid</a> (", "Error</a> (" };
            public static string[] FindHdrB = { "All</a> ", "Valid</b> ", "Invalid</font> ", "Error</a> " };
            public static string[] FindBTrm = { "|", "|", "|", "<" };
            public static string[] FindHdrC = { ">All ", ">Valid ", ">Invalid ", ">To late " };
            public static string[] FindCTrm = { "</a>", "</a>", "</a>", "</a>" };

            private static string WhereEXE()
            {
                return Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            }
            public static string GetSimpleDate(string sDT)
            {
                //Sun 06/09/2019 23:33:53.18 
                int i = sDT.IndexOf(' ');
                i++;
                int j = sDT.LastIndexOf('.');
                return sDT.Substring(i, j - i);
            }

            public static (List<double>, List<int>) RemoveOutliersWithIndexes(ref List<double> data, double threshold = 2)
            {
                double mean = data.Average();
                double stdDev = Math.Sqrt(data.Average(v => Math.Pow(v - mean, 2)));

                // Track indexes of removed outliers
                List<int> outlierIndexes = new List<int>();

                // Filter data while keeping track of original indexes
                List<double> filteredData = data
                    .Select((value, index) => new { value, index }) // Store original index
                    .Where(item =>
                    {
                        bool isOutlier = Math.Abs(item.value - mean) > threshold * stdDev;
                        if (!isOutlier) outlierIndexes.Add(item.index);
                        return !isOutlier;
                    })
                    .Select(item => item.value)
                    .ToList();

                return (filteredData, outlierIndexes);
            }

            public static bool IsInteger(string input)
            {
                return int.TryParse(input, out _);
            }

            public static int FindFirstNonNumericFromEnd(string str)
            {
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    if (!char.IsDigit(str[i]))
                    {
                        return i;
                    }
                }
                return -1; // All characters are digits
            }


            public static int FirstNonInteger(string s, int iOffset)
            {
                int i = iOffset;
                while (i < s.Length)
                {
                    if (s[i] < '0' || s[i] > '9') return i;
                    i++;
                }
                return s.Length;
            }

            // check url for errors and extract info from url s
            public static bool ParseUrl(ref cProjectStats ProjectStats, string s, ref string sOut, ref int ProjectIndex,
                ref string sHost, ref string sSelectedProject, ref string sPage)
            {
                string sPageOffset = "";    // page: 0,1,2 or offset: 0,20,40 etc value + increment of 20
                                            // includes the directive (?offset=) or (page)

                string tURL = "";
                string tHid = "";
                string tValid = "";
                string tPage = "";
                string tCountValids = "";
                string tStudy = "";
                string t;
                int i, j;
                sPage = "0";
                s = s.Trim();
                if (s == "") return false;
                string sUrl = s.ToLower();

                i = sUrl.IndexOf("http");
                if (i < 0)
                {
                    MessageBox.Show("badly formed url: missing http");
                    return false;
                }
                sUrl = sUrl.Substring(i);

                ProjectIndex = ProjectStats.GetNameIndex(sUrl);
                if (ProjectIndex < 0)
                {
                    MessageBox.Show("Project not found in url");
                    return false;
                }

                // determine if the project uses an application ID and if not then flag default
                if (ProjectStats.ProjectList[ProjectIndex].sStudy == "null")
                {
                    tStudy = "null";
                    ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
                }
                else
                {
                    string ProjectName = ProjectStats.ProjectList[ProjectIndex].name;
                    if (ProjectName == "einstein")
                    {
                        i = sUrl.IndexOf("/tasks/4/");
                        if (i < 0)
                        {
                            MessageBox.Show("Task ID not found in url");
                            return false;
                        }
                        j = FirstNonInteger(sUrl, i + 9);
                        t = sUrl.Substring(i + 9, j - i - 9);
                        if (t == "0")
                        {
                            ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
                        }
                        else
                            ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                        ProjectStats.ProjectList[ProjectIndex].sStudyV = t;
                    }
                    else
                    {
                        i = sUrl.IndexOf("?appid=");
                        if (i > 0)
                        {
                            j = FirstNonInteger(sUrl, i + 7);
                            t = sUrl.Substring(i + 7, j - i - 7);
                            if (t == "0")
                            {
                                ProjectStats.ProjectList[ProjectIndex].sStudyV = t;
                                ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
                            }
                            else ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                        }
                        else
                        {
                            ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                        }
                    }
                }

                ProjectStats.GetBaseInfo(ProjectIndex, ref tURL, ref tHid, ref tValid, ref tPage, ref tStudy, ref tCountValids);

                //need to get the value assigned to tHid and to tPage
                i = sUrl.IndexOf(tHid);
                if (i < 0)
                {
                    MessageBox.Show(tHid + " not found in url");
                    return false;
                }

                j = FirstNonInteger(sUrl, i + tHid.Length);
                sHost = s.Substring(i + tHid.Length, j - i - tHid.Length);

                //need to get the page offset indexing into the table if it exists
                if (tPage != "null")
                {
                    i = sUrl.IndexOf(tPage);
                    if (i < 0)
                    {
                        sPageOffset = tPage + "0";
                    }
                    else
                    {
                        j = FirstNonInteger(s, i + tPage.Length);
                        sPage = s.Substring(i + tPage.Length, j - i - tPage.Length);
                        sPageOffset = tPage + sPage;
                    }
                }
                else sPageOffset = "";
                ProjectStats.sTaskOffset = sPage;
                sOut = tURL + tHid + sHost + tValid + ((tStudy == "null") ? "" : tStudy) + sPageOffset;
                //sOut = tURL + tHid + sHost + tValid + tStudy + sPageOffset;
                sSelectedProject = ProjectStats.ProjectList[ProjectIndex].name;
                return true;
                if (tStudy != "null")
                {
                    i = sUrl.IndexOf(tStudy); // the /0 or /56 of the /4/xxg or ?appid=xx the x part
                    if (i < 0)
                    {
                        sOut = tURL + tHid + sHost + tValid + tStudy + sPageOffset;
                    }
                    else
                    {
                        j = FirstNonInteger(s, i + tStudy.Length);
                        string sStudyV = s.Substring(i + tStudy.Length, j - i - tStudy.Length);
                        // need to put sStudyV where the default study value is
                        // default is like &appid=13 or like /0
                        int k = FindFirstNonNumericFromEnd(tStudy);
                        Debug.Assert(k > 0);
                        string sStudy = tStudy.Substring(0, k) + sStudyV;
                        sOut = tURL + tHid + sHost + tValid + sStudy + sPageOffset;
                    }
                }
                else
                {
                    sOut = tURL + tHid + sHost + tValid + sPageOffset;
                }
                sSelectedProject = ProjectStats.ProjectList[ProjectIndex].name;
                return true;
            }

            public static int ReadHostsSets(ref cProjectStats ProjectStats)
            {

                bool bFirstValid = true;
                int ShowPos = 0;
                List<string> MyList = new List<string>();
                ReadStringsIntoList(ref MyList);
                if (MyList.Count == 0) return 0;
                for (int i = 0; i < ProjectStats.ProjectList.Count(); i++)
                {
                    ProjectStats.ProjectList[i].Hosts.Clear();
                    ProjectStats.ProjectList[i].HostNames.Clear();
                }
                foreach (string line in MyList)
                {
                    string[] parts = line.Split(new[] { ':', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        string projectName = parts[0].Trim().ToLower();
                        int iLoc = ProjectStats.GetNameIndex(projectName);
                        if (iLoc < 0)
                        {
                            MessageBox.Show("Project name not found: " + projectName);
                            continue;
                        }
                        if (bFirstValid)
                        {
                            bFirstValid = false;
                            ShowPos = iLoc;
                        }
                        for (int i = 1; i < parts.Length - 1; i += 2)
                        {

                            ProjectStats.ProjectList[iLoc].Hosts.Add(parts[i + 1]);
                            ProjectStats.ProjectList[iLoc].HostNames.Add(parts[i]);
                        }
                    }
                }
                return ShowPos;
            }

             public static bool IsPortOpen(string host)
            {
                int port = 31416;
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        var result = client.BeginConnect(host, port, null, null);
                        bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                        return success && client.Connected;
                    }
                }
                catch
                {
                    return false;
                }
            }

            public static void ReadStringsIntoList(ref List<string> sList)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    openFileDialog.Title = "Open Text File";
                    openFileDialog.InitialDirectory = WhereEXE();

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(openFileDialog.FileName);
                            Properties.Settings.Default.HostList = lines;
                            Properties.Settings.Default.Save();
                            sList.Clear(); // Clear current items
                            sList.AddRange(lines); // Add all lines
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error reading file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }


            public static string GetKnownSystems(ref cProjectStats ProjectStats, int n)
            {
                string sout = "";
                string s;
                int i, j;
                string r = Environment.NewLine;
                foreach (cPSlist ps in ProjectStats.ProjectList)
                {
                    sout += ps.name + r;    // project name
                    s = "";
                    j = 0;
                    for (i = 0; i < ps.HostNames.Count; i++)
                    {
                        if (j + 1 == ps.HostNames[i].Length)
                        {
                            s += "\t" + ps.HostNames[i] + " " + ps.Hosts[i] + r;
                        }
                        else s += "\t" + ps.HostNames[i] + " " + ps.Hosts[i] + ", ";
                        j++;
                        if (j == n)
                        {
                            j = 0;
                            s += r;
                        }
                    }
                    sout += s + r + r;
                }
                return sout;
            }


            // just determines if the project ID exists or not
            // since PCname is unknown if the ID is not registered
            public static bool GetPCnameFromURL(ref cProjectStats ProjectStats, string sUrl, ref string sHost)
            {
                string tURL = "";
                string tHid = "";
                string tValid = "";
                string tPage = "";
                string tStudy = "";
                string tCountValids = "";
                int ProjectIndex = ProjectStats.GetNameIndex(sUrl);

                ProjectStats.GetBaseInfo(ProjectIndex, ref tURL, ref tHid, ref tValid, ref tPage, ref tStudy, ref tCountValids);

                //need to get the value assigned to tHid and to tPage
                int i = sUrl.IndexOf(tHid);
                if (i < 0)
                {
                    MessageBox.Show(tHid + " not found in url");
                    return false;
                }

                int j = FirstNonInteger(sUrl, i + tHid.Length);
                sHost = sUrl.Substring(i + tHid.Length, j - i - tHid.Length);
                return ProjectStats.HasProjectID(sHost, ProjectIndex);
            }

            public static int ProcessHDR(ref string RawPage, ref string sOut)
            {
                //RawPage = ProjectStats.RawPage;
                if (RawPage == null) return 0;
                int NumValid = 0;
                for (int k = 0; k < FindHdr.Length; k++)
                {
                    string sDFT = ")";  // default terminator

                    string sB = FindHdrB[k];
                    string tB = FindBTrm[k];
                    string sC = FindHdrC[k];
                    string tC = FindCTrm[k];


                    string sA = FindHdrA[k];
                    string s = FindHdr[k];
                    int i = RawPage.IndexOf(s);
                    int n = s.Length;
                    if (i < 0)
                    {
                        i = RawPage.IndexOf(sA);
                        n = sA.Length;
                    }
                    if (i < 0)
                    {
                        i = RawPage.IndexOf(sB);
                        n = sB.Length;
                        sDFT = tB;
                    }
                    if (i < 0)
                    {
                        i = RawPage.IndexOf(sC);
                        n = sC.Length;
                        sDFT = tC;
                    }
                    if (i < 0) return 0;

                    int j = RawPage.IndexOf(sDFT, i + n);
                    if (j < 0) return 0;

                    string t = RawPage.Substring(i + n, j - i - n).Trim();
                    if (t == "") t = "0";
                    if (k == 1) NumValid = Convert.ToInt32(t);
                    sOut += FindHdr[k] + t + ")\r\n";
                }
                return NumValid;
            }

            public static void AskToGetAppList(ref cProjectStats ProjectStats)
            {
                if (Properties.Settings.Default.AskForStudy)
                {
                    string s = "If you have BOINC on this PC I can possibly obtain app (study) IDs\r\n" +
                        "Cancel allows this question to be asked again\r\n" +
                        "Select Yes and the results will be saved for future use";
                    DialogResult Res = MessageBox.Show(s, "You can always make changes in the config menu", MessageBoxButtons.YesNoCancel);
                    if (Res == DialogResult.Yes)
                    {
                        int n = ProjectStats.GetProjectAPPIDs();
                        Properties.Settings.Default.AskForStudy = false;
                        Properties.Settings.Default.Save();
                        if (n > 0)
                        {
                            MessageBox.Show("Found " + n.ToString() + " app IDs. View them in the  config menu");
                        }
                    }
                    else if (Res == DialogResult.No)
                    {
                        Properties.Settings.Default.AskForStudy = false;
                        Properties.Settings.Default.Save();
                    }
                }
                GetSavedAppStudy(ref ProjectStats);         
            }

            public static bool GetSavedAppStudy(ref cProjectStats ProjectStats)
            {
                string[] AppStrings = Properties.Settings.Default.AppList;
                if (AppStrings != null)
                {
                    ProjectStats.ParseAppsStrings(ref AppStrings);
                    return true;
                }
                return false;
            }

            public static bool AskToGetClientList(string MyComputerID)
            {
                if (Properties.Settings.Default.AskForBoinctasks)
                {
                    string sQ =
                        "Your host ID is " + MyComputerID + Environment.NewLine +
                        "If you have BOINC then I may be able to obtain IDs for your projects\r\n" +
                        "If you have BOINCTASKS then I may be able to obtain IDs for remote systems with no passwords\r\n" +
                        "Select Yes and I will try asking BOINC on remote systems to provide project IDs\r\n" +
                        "Select No and I will not try and will not ask again\r\n" +
                        "Select Cancel and I will ask this the next time you start this program.";
                    DialogResult Res = MessageBox.Show(sQ, "You can always make changes in the config menu", MessageBoxButtons.YesNoCancel);
                    if (Res == DialogResult.No)
                    {
                        Properties.Settings.Default.AskForBoinctasks = false;
                        Properties.Settings.Default.Save();
                        return false;
                    }
                    if (Res == DialogResult.Cancel) return false;
                    return true;
                }
                return false;
            }

            public static bool ReadStrings(ref string[] StringsIN, string sTgt)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    dialog.Title = "Select a file like " + sTgt;
                    dialog.InitialDirectory = WhereEXE();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = dialog.FileName;
                        if (path == null) return false;
                        try
                        {
                            StringsIN = File.ReadLines(path).ToArray();
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Error reading file: " + ex.Message);
                            return false;
                        }
                    }
                }
                return true;
            }

            // "Save AppIDs " for study or "DemoHostList.txt" for client list
            public static bool WriteStrings(ref string[] StringsOut, string sTgt)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.Title = sTgt + " ";
                    saveFileDialog.InitialDirectory = WhereEXE();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                            {
                                foreach (string line in StringsOut)
                                {
                                    writer.WriteLine(line);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error saving list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                return true;
            }

            public static bool GetHostsSet(ref cProjectStats ProjectStats)
            {
                int i;
                string[] SavedHostList = Properties.Settings.Default.HostList;
                List<string> Proj_PC_ID = new List<string>();   // project name, pc name and pc id "computer id"
                if (SavedHostList == null || SavedHostList.Length == 0)
                {
                    return false;
                }
                Proj_PC_ID.AddRange(SavedHostList);
                if (Proj_PC_ID.Count == 0) return false;
                return FormHostList(ref Proj_PC_ID, ref ProjectStats);
            }

            public static bool FormHostList(ref List<string> Proj_PC_ID, ref cProjectStats ProjectStats)
            {
                if (Proj_PC_ID.Count == 0) return false;
                int i, n = 0;
                ProjectStats.ComputerList.Clear();
                for(i = 0; i < ProjectStats.ProjectList.Count;i++)
                {
                    ProjectStats.ProjectList[i].Hosts.Clear();
                    ProjectStats.ProjectList[i].HostNames.Clear();
                }

                foreach (string s in Proj_PC_ID)
                {
                    i = s.IndexOf(":");
                    if (i < 0)
                    {
                        MessageBox.Show("Expected format of project name: id1 id2 .. not found!");
                        continue;
                    }
                    string sName = s.Substring(0, i).Trim();
                    if (i >= s.Length) continue;

                    string sRem = s.Substring(i + 1).Trim();
                    int iLoc = ProjectStats.GetNameIndex(sName);
                    if (iLoc < 0)
                    {
                        MessageBox.Show("Unknown project found: ", sName);
                        continue;
                    }

                    string[] sPairHosts = sRem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (i = 0; i < sPairHosts.Length; i++)
                    {
                        string u = sPairHosts[i];
                        int i1 = u.IndexOf("--passwd ");
                        if (i1 > 0)
                        {
                            int i2 = u.LastIndexOf(" ");
                            Debug.Assert(i2 > 0);
                            string v = u.Substring(0, i1).Trim();
                            string w = u.Substring(i2 + 1).Trim();
                            u = v + " " + w;
                            sPairHosts[i] = u;
                        }
                        string[] sPair = sPairHosts[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sPair.Length != 2)
                        {
                            MessageBox.Show("Expected format of 'hostname hostid'");
                            continue;
                        }
                        string sPC = sPair[0].Trim();
                        ProjectStats.AddLocalPC(sPC);
                        ProjectStats.ProjectList[iLoc].HostNames.Add(sPC);
                        ProjectStats.ProjectList[iLoc].Hosts.Add(sPair[1].Trim());
                        n++;
                    }
                }
                return n > 0;
            }

            public static void UseDemoData()
            {
                Properties.Settings.Default.HostList = JysDemoClient.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Properties.Settings.Default.AppList = JYSDemoStudy.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Properties.Settings.Default.RemoteHosts = JYSDemoStudy.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Properties.Settings.Default.Save();
            }

            public static string JysDemoClient = @"
amicable: AsusX299 250099,H110btc 247724,JYSOmen 249983,JYSX299 247720
asteroids: AsusX299 757796,H110btc 801421,JYS-RTX-3070 801550,JYSEVGA3 801930,JYSOmen 785798,JYSX299 739274,Shire2 798739
cpdn climateprediction: AsusX299 1556725
denis: AsusX299 223521,H110btc 223524,JYSOmen 223525
einstein: AsusX299 13165236,H110btc 13220820,JYS-RTX-3070 13145961,JYSEVGA3 13218819,JYSOmen 13180083,JYSX299 13177545,Shire2 13205645
gerasim: 
gpugrid: AsusX299 621577,H110btc 636833,JYSEVGA3 636834,JYSOmen 604468,JYSX299 622167,Shire2 636835
lhc: AsusX299 10825078,H110btc 10824337,JYSOmen 10825081
loda: 
milkyway: AsusX299 1046337,H110btc 1046347,JYS-RTX-3070 1046345,JYSEVGA3 1047480,JYSOmen 989898,JYSX299 1046341,Shire2 1046342
moowrap: H110btc 1323852
nfs escatter: AsusX299 7205907,H110btc 7205910,JYSOmen 7205913,JYSX299 7229360
numberfields: AsusX299 2883430,H110btc 2848215,JYSOmen 2848216,JYSX299 2883421
odlk progger: 
odlk1 latinsquares: AsusX299 128066,H110btc 128063,JYSOmen 128062
primegrid: H110btc 1304227
radioactive: AsusX299 48545
rakesearch: 
rnma: AsusX299 25716
rosetta: AsusX299 6323416,H110btc 6237587
sidock: AsusX299 45921,H110btc 67646,JYSEVGA3 70715,JYSOmen 56719,JYSX299 55910
srbase: AsusX299 223945,H110btc 236401,JYSOmen 223941,JYSX299 236324
wcg worldcommunitygrid: AsusX299 8788717,H110btc 8866993,JYS-RTX-3070 8866995,JYSEVGA3 8867440,JYSOmen 8866994,JYSX299 8800933,Shire2 8862947
yafu: 
yoyo: AsusX299 517504,H110btc 517506,JYSOmen 517505
";
            public static string JYSDemoStudy = @"
denis:17
einstein:0 57 40 58 56 25 60
gpugrid:41 32 40 38
lhc:13 1 10 14
milkyway:4
nfs:9
odlk:3
odlk1:1
primegrid:8
rakesearch:8
rnma:6
rosetta:3
sidock:5
srbase:15
wcg:124 92 94 74
yafu:15
";
            public static string DemoRemote = @"
shire2
AsusX299
jysx299
jysomen
jysevga3
jys-rtx-3070
h110btc
dualx5675
dual-linux
jysevga
";
        }
    }
}
