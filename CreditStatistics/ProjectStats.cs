using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using static CreditStatistics.CreditStatistics;
using System.Globalization;
using System.Security.Policy;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Net;
using System.Xml.Linq;


namespace CreditStatistics
{
    public class cPSlist
    {
        public bool UseDefault;   // use tasks/4/0 and omit ?appid=       

        public string name;
        public string sURL;
        public string sHid;
        public string sValid;
        public string sStudy;
        public string sStudyV;
        public string sPage;
        public string sCountValids;

        public List<string> Hosts = new List<string>(); // the ID of the pc
        public List<string> HostNames = new List<string>();
        public void AddHosts(string sHostIDs)
        {
            if (sHostIDs == "") return;
            string[] s = sHostIDs.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s1 in s)
            {
                if (s1 == "") continue;
                Hosts.Add(s1);
            }
        }
    }
    public class cProjectStats
    {
        public List<cPSlist> ProjectList = new List<cPSlist>();
        public static readonly string[] KnownProjects = @"
sidock
https://www.sidock.si/sidock/results.php?
hostid=
&state=4
&appid=
5
&offset=
 Valid () .

denis
https://denis.usj.es/denisathome/results.php?
hostid=
&state=4
&appid=
17
&offset=
 Valid () .

gpugrid
https://gpugrid.net/gpugrid/results.php?
hostid=
&state=4
&appid=
41
&offset=
 Valid () .

rosetta
https://boinc.bakerlab.org/rosetta/results.php?
hostid=
&state=4
&appid=
3
&offset=
 Valid () .

asteroids
https://asteroidsathome.net/boinc/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

milkyway
https://milkyway.cs.rpi.edu/milkyway/results.php?
hostid=
&state=4
&appid=
4
&offset=
 Valid () .

einstein
https://einsteinathome.org
/host/
/tasks/4
/
0
?page=
>Valid ()</span>.

lhc
https://lhcathome.cern.ch/lhcathome/results.php?
hostid=
&state=4
&appid=
13
&offset=
 Valid () .

amicable
https://sech.me/boinc/Amicable/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

numberfields
https://numberfields.asu.edu/NumberFields/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

odlk progger
https://boinc.progger.info/odlk/results.php?
hostid=
&state=4
&appid=
3
&offset=
 Valid () .

odlk1 latinsquares
https://boinc.multi-pool.info/latinsquares/results.php?
hostid=
&state=4
&appid=
1
&offset=
 Valid () .

moowrap moo
https://moowrap.net/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

nfs escatter
https://escatter11.fullerton.edu/nfs/results.php?
hostid=
&state=4
&appid=
9
&offset=
 Valid () .

PrimeGrid
https://www.primegrid.com/results.php?
hostid=
&state=4
&appid=
8
&offset=
<b>Valid</b> 0 |.

gerasim
https://gerasim.boinc.ru/users/viewHostResults.aspx?
hostid=
&opt=2
null
null
null
>Valid 0 </a>.

srbase
https://srbase.my-firewall.org/sr5/results.php?
hostid=
&state=4
&appid=
15
&offset=
 Valid () .

rakesearch
https://rake.boincfast.ru/rakesearch/results.php?
hostid=
&state=4
&appid=
8
&offset=
 Valid () .

rnma ramanujanmachine
https://rnma.xyz/boinc/results.php?
hostid=
&state=4
&appid=
6
&offset=
 Valid () .

loda
https://boinc.loda-lang.org/loda/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

yafu
https://yafu.myfirewall.org/yafu/results.php?
hostid=
&state=4
&appid=
15
&offset=
 Valid () .

cpdn ClimatePrediction
https://main.cpdn.org/results.php?
hostid=
&state=4
null
null
&offset=
 Valid () .

yoyo
https://www.rechenkraft.net/yoyo/results.php?
hostid=
null
null
null
&offset=
null

WCG WORLDCOMMUNITYGRID WORLD
https://www.worldcommunitygrid.org/contribution
/device?id=
&type=B
?projectId=
124
null
null

radioactive
http://radioactiveathome.org/boinc/results.php?
hostid=
&state=3
null
null
&offset=
null

gene tn-grid
https://gene.disi.unitn.it/test/results.php?
hostid=
&state=4
null
null
&offset=
null
"
        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // lookup project

        public void SelectComputer(string sPC) // name of computer is sPC such as "shire2"
        {
            LocalHosts.Clear();
            foreach (cPSlist p in ProjectList)
            {
                int i = p.HostNames.IndexOf(sPC);
                if (i >= 0)
                {
                    cLHe cLHe = new cLHe();
                    cLHe.ComputerID = sPC;
                    cLHe.ProjectName = p.name;
                    cLHe.ProjectsHostID = p.Hosts[i];
                    int nID = GetNameIndex(p.name);
                    cLHe.IndexToProjectList = nID;
                    LocalHosts.Add(cLHe);
                }
            }
        }

        public void Init()
        {
            int i, j, n = KnownProjects.Length;
            List<string>UnsortedNames = new List<string>();

            for (j = 0; j < n; j += 8)
            {
                UnsortedNames.Add(KnownProjects[j].ToLower());
            }
            n = UnsortedNames.Count;
            List<int> indices = Enumerable.Range(0, n).ToList();
            indices.Sort((i1, i2) => UnsortedNames[i1].CompareTo(UnsortedNames[i2]));
            for (j = 0; j < n; j++)
            {
                i = indices[j]*8;
                ProjectList.Add(new cPSlist()
                {
                    name = KnownProjects[i++].ToLower(),
                    sURL = KnownProjects[i++],
                    sHid = KnownProjects[i++],
                    sValid = KnownProjects[i++],
                    sStudy = KnownProjects[i++],
                    sStudyV = KnownProjects[i++],
                    sPage = KnownProjects[i++],
                    sCountValids = KnownProjects[i]
                });
            }
            for (j = 0; j < n; j++)
            {
                ProjectList[j].UseDefault = true;
            }
        }

        public void AddDemo(string sUrl)
        {
            int pLoc = GetNameIndex(sUrl);
            string SelectedProject = ProjectList[pLoc].name;
            string sLoc = ProjectList[pLoc].sPage;
        }
        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (var p in ProjectList)
            {
                names.Add(p.name);
            }
            return names;
        }

        private string GetBaseURL(int iLoc, string sID, string sPage)
        {
            cPSlist p = ProjectList[iLoc];
            string s = p.sURL + p.sHid + sID + p.sValid + ((p.sStudy == "null") ? "" : p.sStudy + p.sStudyV);
            BaseUrl = s;
            CannotIncrement = (sPage == "");
            if (CannotIncrement) return s;
            return s + p.sPage + sPage;
        }

        public bool GetBaseInfo(int ProjectID, ref string sURL, ref string sHid,
                ref string sValid, ref string sPage, ref string sStudy, ref string sCountValids)
        {
            if (ProjectID < 0 || ProjectID >= ProjectList.Count) return false;
            cPSlist p = ProjectList[ProjectID];
            sURL = p.sURL;
            sHid = p.sHid;
            sValid = p.sValid;
            sPage = p.sPage;
            if (p.UseDefault)
            {
                if (p.name == "einstein")
                {
                    sStudy = p.sStudy + "0";
                }
                else sStudy = "null";
            }
            else
            {
                sStudy = p.sStudy + p.sStudyV;
            }
            sCountValids = p.sCountValids;
            return true;
        }

        public string GetStudy(string s)
        {
            foreach (cPSlist c in ProjectList)
            {
                string[] st = c.name.Split(' ');
                foreach (string s1 in st)
                {
                    if (s.Contains(s1))
                    {
                        string t = c.sStudy;
                        if(t == "null") return "";
                        bool b = (t == "/");
                        if(b)return "/tasks/4/XX";
                        return t + "XX";
                    }
                }                
            }
            return "";
        }
        public string ShortName(int i)
        {
            string[] s = ProjectList[i].name.Split(' ');
            return s[0];
        }

        public string GetURL0(string name, string sID, ref bool HasValids)
        {
            HasValids = true;   // unless otherwise
            string sE = ""; // for einstein
            string sNE = "";// for non-einstein 
            foreach (var p in ProjectList)
            {
                if (p.name.Contains(name))
                {
                    string s = p.sURL + p.sHid;
                    if(p.UseDefault)
                    {
                        if(name == "einstein")
                        {
                            sE = "/tasks/4/0";
                        }
                        else
                        {
                            sNE = "";
                        }
                    }
                    else
                    {
                        if (name == "einstein")
                        {
                            sE = p.sValid + p.sStudy + p.sStudyV;
                        }
                        else
                        {
                            sNE = "";
                            if(p.sValid != "null")
                            {
                                sNE = p.sValid;
                                if(p.sStudy != "null")
                                {
                                    sNE += p.sStudy + p.sStudyV;
                                }
                            }                            
                        }
                    }
                    s = s + sID;
                    if (p.sValid == "&state=4" || p.sValid == "&state=3")
                    {
                        s += sNE;
                        s += p.sPage + "0"; //jys cannot be 0
                        return s;
                    }

                    if (p.sValid == "/tasks/4")
                    {
                        s += sE;
                        s += p.sPage + "0"; // jys cannot be 0
                        return s;
                    }
                    if (p.sValid == "&opt=2")
                    {
                        s += sNE;
                        HasValids = false;
                        return s;
                    }
                    if (p.sValid == "&type=B")
                    {
                        s += sNE;
                        Debug.Assert(p.sPage == "null");
                        HasValids = false;
                        return s;
                    }
                    if(p.sValid == "null")
                    {
                        s += sNE;
                        HasValids = false;
                        return s;
                    }
                }
            }
            return "";
        }

        public class cLHe
        {
            public string ComputerID;   // name of the PC such as "shire2"
            public string ProjectName;  // cpdn, lhc, etc
            public string ProjectsHostID;   // the ID given by the project to the PC
            public int IndexToProjectList;
        }
        public List<cLHe> LocalHosts = new List<cLHe>();

        public bool DoesPCexist(string pcName)
        {
            foreach (cLHe cle in LocalHosts)
            {
                if (cle.ComputerID == pcName) return true;
            }
            return false;
        }
        private bool ProjectExists (string s)
        {
            foreach (cPSlist c in ProjectList)
            {
                string[] st = c.name.Split(' ');
                foreach (string s1 in st)
                {
                    if (s.Contains(s1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasProjectID(string projectID, int iLoc)
        {
            foreach (cLHe cle in LocalHosts)
            {
                if (cle.ProjectsHostID == projectID && cle.IndexToProjectList == iLoc) return true;
            }
            return false;
        }
        public int GetNameIndex(string s)
        {
            int i = 0;
            string[] sTemp;
            foreach (cPSlist c in ProjectList)
            {
                sTemp = c.name.Split(' ');
                foreach (string s1 in sTemp)
                {
                    if (s.Contains(s1))
                    {
                        return i;
                    }
                }
                i++;
            }
            return -1;
        }

        public class cOrganizedRaw
        {
            public string ProjNameFull;
            public List<string> PCnameHostID = new List<string>();
        }
        public class cBoincRaw
        {
            public string PCname;
            public List<string> Proj = new List<string>();
            public List<string> HostID = new List<string>();
        }
        public class cOldraw
        {   public List<string> RawOut = new List<string>();
            public List<cOrganizedRaw> OrgRaw = new List<cOrganizedRaw>();
            public int AddProjName(string s)
            {
                if (OrgRaw.Count == 0)
                {
                    cOrganizedRaw OR = new cOrganizedRaw();
                    OR.ProjNameFull = s;
                    OrgRaw.Add(OR);
                    return OrgRaw.Count - 1;
                }
                int i = 0;
                foreach(cOrganizedRaw OR1 in OrgRaw)
                {
                    if (s == OR1.ProjNameFull) return i;
                    i++;
                }
                cOrganizedRaw OR2 = new cOrganizedRaw();
                OR2.ProjNameFull = s;
                OrgRaw.Add(OR2);
                return OrgRaw.Count - 1;
            }
            public void AddTriple(string sPCname, string ProjName, string sPCid)
            {
                int i = AddProjName(ProjName);
                OrgRaw[i].PCnameHostID.Add(sPCname + " " + sPCid);
            }
        }

        public bool GetHostsFile(string sBoincLoc)
        {
            if (File.Exists(sBoincLoc))
            {
                return GetHosts(File.ReadAllText(sBoincLoc).ToLower());
            }
            return false;
        }


        public string WhereEXE = "";
        public bool GetHosts(string sBoincInfo)
        {
            string sPCname;
            string ProjHost;
            string sProj;
            string sHost;
            bool b;
            int j;
#if DEBUGNO
            string sDout =  WhereEXE + "/ClientList_out.txt";
            string sDin = WhereEXE + "/ClientList_in.txt";
            File.WriteAllText(sDout, sBoincInfo);
            MessageBox.Show("Just wrote your client list to: " + Environment.NewLine +
                sDout + Environment.NewLine + "Going to read and use: " + sDin);
            sBoincInfo = File.ReadAllText(sDin).ToLower();
#endif

            List <cBoincRaw> LocalHostsRaw = new List<cBoincRaw>();
            cBoincRaw br;
            cOldraw cOl = new cOldraw();
            string UnknownProjects  = "";
            sBoincInfo = sBoincInfo.Replace("@home", "");
            string[] content = sBoincInfo.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            while (true)
            {
                if (i >= content.Length)
                {
                    break;
                }

                do
                {
                    sPCname = content[i].Trim();
                    i++;
                    if (sPCname.Contains(",") || sPCname == "")
                    {
                        MessageBox.Show("expected hostname found: " + sPCname);
                        b = true;
                    }
                    else b = false;
                } while (b);

                br = new cBoincRaw();
                br.PCname = sPCname;
                LocalHostsRaw.Add(br);

                do
                {
                    if(i >= content.Length)
                    {
                        break;  // todo this is an error as there should be data for this PC
                    }
                    ProjHost = content[i].Trim();
                    i++;
                    j = ProjHost.IndexOf(",");
                    if (j < 0)
                    {
                        i--;
                        break;
                    }
                    else
                    {
                        if (j >= (ProjHost.Length - 1))
                        {
                            break;
                        }
                        sProj = ProjHost.Substring(0, j).Trim();
                        sHost = ProjHost.Substring(j + 1).Trim();
                        LocalHostsRaw.Last().Proj.Add(sProj);
                        LocalHostsRaw.Last().HostID.Add(sHost);
                    }
                    b = (i < content.Length);
                } while (b);
            }

            foreach (cBoincRaw abr in LocalHostsRaw)
            {
                int k = 0;
                for (i = 0; i < abr.Proj.Count; i++)
                {
                    if (ProjectExists(abr.Proj[i]))
                    {
                        cOl.AddTriple(abr.PCname, abr.Proj[i], abr.HostID[i]);
                    }
                    else
                    {
                        string u = abr.Proj[i];
                        if (!UnknownProjects.Contains(u))
                            UnknownProjects += u + " ";
                        k++;
                        if (k == 4)
                        {
                            k = 0;
                            UnknownProjects += Environment.NewLine;
                        }
                    }
                }
            }
            foreach (cOrganizedRaw OR in cOl.OrgRaw)
            {
                string sOut = OR.ProjNameFull + ": ";
                for (j = 0; j < OR.PCnameHostID.Count(); j++)
                    sOut += OR.PCnameHostID[j] + ",";
                sOut = sOut.Substring(0,sOut.Length - 1);   // remove trailing comma
                cOl.RawOut.Add(sOut);
            }
            Properties.Settings.Default.HostList = cOl.RawOut.ToArray();
            if (UnknownProjects != "")
            {
                MessageBox.Show("Unknown or old projects:" + Environment.NewLine + UnknownProjects);
            }

            return Properties.Settings.Default.HostList.Length > 0;
        }

        public string GetIDfromName(string name)
        {
            foreach (cLHe c in LocalHosts)
            {
                if (c.ProjectName == name)
                {
                    return c.ProjectsHostID;
                }
            }
            return "";
        }

        public string sTaskType = "";    // one of HDR or BODY
        public int NumValid;       // number of valids found in header
        public bool TaskBusy;
        public bool TaskDone;
        public bool StopTask;
        public bool TaskError;
        private string PageCommand; // ?offset= etc or empty
        public string RawPage;
        public string RawTable;
        private string TaskName;       // project name
        public string TaskUrl;      // base url plus the offset
        public string BaseUrl;      // just the url no offset
        public bool CannotIncrement;
        private int TaskOffset;   // could be page 0,1,etc or 20,40, etc
        public string sTaskOffset;   // could be page 0,1,etc or 20,40, etc
        private int nTaskOffset;        
        private string sTaskHost;
        private int TaskIncrement;// 1 or 20
        private bool HasOffset;  // if false , then no offset
        private int iTask;      // 0..number projects-1
        private int TasksWanted;
        public string sCountValids;
        public int NumberRecordsRead;
        private int nTotalSamples;
        private int NumberConcurrent;
        public string ResultsBoxText;
        public List<cCreditInfo> LCreditInfo = new List<cCreditInfo>();
        public List<cCreditInfo> UnsortedLCI = new List<cCreditInfo>();
        public List<DateTime> UnsortedDT = new List<DateTime>();
        private string[] RawLines;
        private string[] formats = { "d MMM yyyy | H:mm:ss UTC", "dd MMM yyyy | H:mm:ss UTC", "d MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, h:mm:ss tt UTC" };
        private string[] format2 = { "d MMM yyyy, H:mm:ss", "dd MMM yyyy, H:mm:ss" };

        public List<double> mCPU = new List<double>();
        public List<double> mELA = new List<double>();

        public bool IncrementReader()
        {
            if (CannotIncrement) return false;
            nTaskOffset += TaskIncrement;
            sTaskOffset = nTaskOffset.ToString();            
            TaskUrl = BaseUrl + PageCommand + sTaskOffset;
            return true;
        }
        public void ConfigureTask(int jTask, string sHost, string sOffset, string sType)
        {
            if (jTask < 0) return;
            UnsortedLCI.Clear();
            UnsortedDT.Clear();
            PageCommand = "";
            iTask = jTask;
            NumValid = 0;
            sTaskType = sType;
            sTaskHost = sHost;
            StopTask = false;
            TaskBusy = false;
            TaskDone = false;
            cPSlist p = ProjectList[jTask];
            TaskName = p.name;
            // jys url is required and can be different from the one created by the previous
            // parse. this need to be cleaned up eventually
            TaskUrl = GetBaseURL(jTask, sHost, sOffset);            
            sTaskOffset = sOffset;
            TaskOffset = 0;
            TaskIncrement = 0;
            HasOffset = true;
            TaskError = false;
            sCountValids = p.sCountValids;
            NumberConcurrent = 1;
            nTotalSamples = 0;// in case we need to count them if not listed in header
            ResultsBoxText = "";
            if (sTaskOffset.Length > 0)
            {
                nTaskOffset = Convert.ToInt32(sTaskOffset);
                if (p.sPage.Contains("page")) TaskIncrement = 1;
                else if (p.sPage.Contains("offset")) TaskIncrement = 20;                
                else HasOffset = false;
                PageCommand = p.sPage;
            }
        }

        public int GetTableFromRaw()
        {
            int iStart, iEnd, i, j, k;
            string t;
            NumberRecordsRead = 0;
            switch (TaskName)
            {
                case "einstein":
                    iStart = RawPage.IndexOf("<tbody>");
                    iEnd = RawPage.IndexOf("</tbody>");
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd))
                    {
                        ResultsBoxText = "Bad einstein url:  cannot find TABLE nor TBODY or maybe no results\n";
                        return -3;
                    }
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    BuildEinsteinStatsTable();
                    break;
                case "odlk1 latinsquares":
                case "lhc":
                case "amicable":
                case "rosetta":
                case "milkyway":
                case "numberfields":
                case "cpdn climateprediction":
                case "moowrap":
                case "nfs escatter":
                case "srbase":
                case "yafu":
                case "loda":
                case "rakesearch":
                case "sidock":
                case "rnma":
                case "radioactive":
                case "odlk progger":
                case "gpugrid":
                    string strH = "workunit.php?";
                    iStart = RawPage.IndexOf(strH);
                    if (iStart < 0)
                    {
                        ResultsBoxText = "error: missing 'a href=' or maybe no results\n";  // had to remove <
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable();
                case "asteroids":
                    iStart = RawPage.IndexOf("<table class");
                    iEnd = RawPage.IndexOf("</table>");
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable();
                case "primegrid":
                    iStart = RawPage.IndexOf("<tr class=row0>");
                    if (iStart < 0)
                    {
                        ResultsBoxText = "error: no data or missing 'tr class=row0'\n"; // had to remove < and >
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");   // need skip over any earlier tables in the header
                    if (iStart < 0 || iEnd < 0)
                    {
                        int ERR = 0;
                    }
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildPrimeTable();
               case "gerasim":
                    iStart = RawPage.IndexOf("<table class=\"gridTable\"");
                    iEnd = RawPage.IndexOf("</table>",iStart);
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    string[] OuterTable = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool bNext = false;
                    i = 0;
                    while(i < OuterTable.Length)
                    {
                        string s = OuterTable[i++];
                        j = s.IndexOf("<span id=");
                        if (j < 0) continue;
                        k = s.IndexOf("</span>", j);
                        if (j < 0) continue;
                        j = s.LastIndexOf(">", k);
                        t = s.Substring(j + 1, k - j - 1);
                        CreditStatistics.cCreditInfo ci = new cCreditInfo();
                        if (DateTime.TryParseExact(t, format2, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                        {
                            ci.tCompleted = dateTime1;
                        }
                        else
                        {
                            return 0;
                        }
                        s = OuterTable[i++];
                        iStart = s.IndexOf("</td><td align=\"center\">cpu</td><td align=\"right\">");
                        if (iStart < 0) return 0;
                        iEnd = s.LastIndexOf(">", iStart);
                        t = s.Substring(iEnd + 1, iStart - iEnd - 1);
                        ci.ElapsedSecs = Convert.ToDouble(t);
                        ci.CPUtimeSecs = ci.ElapsedSecs;
                        j = s.LastIndexOf("</td>", s.Length - 1);
                        if (j < 0) return 0;
                        k = s.LastIndexOf(">", j);
                        if (k < 0) return 0;
                        t = s.Substring(k+1, j - k - 1);
                        ci.Credits = Convert.ToDouble(t);
                        ci.mELA = ci.Credits / ci.ElapsedSecs;
                        mELA.Add(ci.mELA);
                        if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                        ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                        mCPU.Add(ci.mCPU);
                        ci.bValid = true;
                        LCreditInfo.Add(ci);
                        continue;
                    }
                    break;
                default:
                    iStart = RawPage.IndexOf("<tr class=row0>");
                    if (iStart < 0)
                    {
                        ResultsBoxText = "error: no data or missing 'tr class=row0'\n"; // had to remove < and >
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");   // need skip over any earlier tables in the header
                    if (iStart < 0 || iEnd < 0)
                    {
                        int ERR = 0;
                    }
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable();
            }
            return 0;
        }

        private int BuildPrimeTable()
        {
            string s;
            string[] OuterTable = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string sLine in OuterTable)
            {
                CreditStatistics.cCreditInfo ci = new cCreditInfo();
                RawLines = sLine.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                s = RawLines[4];
                if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                {
                    ci.tCompleted = dateTime1;
                }
                else
                {
                    return 0;
                }
                int i = RawLines[6].LastIndexOf(">");
                s = RawLines[6].Substring(i + 1);
                ci.ElapsedSecs = Convert.ToDouble(s);
                s = RawLines[7].Substring(i + 1);
                ci.CPUtimeSecs = Convert.ToDouble(s);
                s = RawLines[8].Substring(i + 1);
                ci.Credits = Convert.ToDouble(s);
                ci.mELA = ci.Credits / ci.ElapsedSecs;
                mELA.Add(ci.mELA);
                if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                mCPU.Add(ci.mCPU);
                ci.bValid = true;   // do not really know if it is valid yet
                LCreditInfo.Add(ci);
            }
            return 0;
        }
        private int BuildStatsTable()
        {
            int i;
            RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (i = 0; i < 21; i++)    // first line of data must be within first 21 or so lines
            {
                if (RawLines[i].Contains("right>"))
                {
                    FinishTable(i); // this gets rest of table which might be less then 20 lines
                    return 0;
                }
            }
            ResultsBoxText = "Error - could not find first value on the page\n";
            return -2;
        }

        private void FinishTable(int iFirst)
        {
            //for (int i = 0; i < NumberToCollect; i++)
            while (true)
            {
                if (ExtractTriplet(iFirst))
                {
                    iFirst += 10;
                    NumberRecordsRead++;
                }
                else return;
            }
        }

        private DateTime GetValueT(int iLoc, int iOffset)
        {
            string lSide;
            if ((iLoc + iOffset) >= RawLines.Length)
                return DateTime.MinValue;
            string sTemp = RawLines[iLoc + iOffset];            
            int iRight = sTemp.IndexOf("<td>");
            if (iRight < 0)
            {
                ResultsBoxText = "Error expected <td> not found in line " + sTemp;
                return DateTime.MinValue;
            }
            lSide = sTemp.Substring(iRight + 4);
            iRight = lSide.IndexOf("</td>");
            if (iRight < 0)
            {
                ResultsBoxText = "Error expected /td not found in line " + sTemp;
                return DateTime.MinValue;
            }
            string s = lSide.Substring(0, iRight);
            if (s == "")
            {
                return DateTime.MinValue;
            }

            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
            {
                return dateTime1;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private double GetValueD(int iLoc, int iOffset)
        {
            string sTemp = RawLines[iLoc + iOffset];
            string lSide;
            int iRight = sTemp.IndexOf("right>");
            if (iRight < 0)
            {
                ResultsBoxText = "Error expected right greaterthansign  not found in line " + sTemp;
                return -1;
            }
            lSide = sTemp.Substring(iRight + 6);
            iRight = lSide.IndexOf("</td>");
            if (iRight < 0)
            {
                ResultsBoxText = "Error expected /td not found in line " + sTemp;
                return -1;
            }
            string s = lSide.Substring(0, iRight);
            double aValue = Convert.ToDouble(s);
            return aValue;
        }
        private bool ExtractTriplet(int iLocation)
        {
            CreditStatistics.cCreditInfo ci = new cCreditInfo();
            ci.tCompleted = GetValueT(iLocation, -2);
            if(ci.tCompleted == DateTime.MinValue)
            {
                return false;
            }

            ci.ElapsedSecs = GetValueD(iLocation, 0) / NumberConcurrent;
            ci.CPUtimeSecs = GetValueD(iLocation, 1);
            ci.Credits = GetValueD(iLocation, 2);            
            ci.mELA = ci.Credits / ci.ElapsedSecs;
            mELA.Add(ci.mELA);
            if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
            ci.mCPU = ci.Credits / ci.CPUtimeSecs;
            mCPU.Add(ci.mCPU);
            ci.bValid = true;   // do not really know if it is valid yet
            LCreditInfo.Add(ci);
            return true;
        }

        int NumberToCollect;
        private int BuildEinsteinStatsTable()
        {
            int i, j, k;
            string sDataKey = "</td><td>Completed and validated</td><td>";
            double t;
            string[] RawLineValues;
            string[] eDTformats = { "d MMM yyyy H:mm:ss UTC", "dd MMM yyyy H:mm:ss UTC", };           
            RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            NumberToCollect = RawLines.Length - 1;
            NumberRecordsRead = 0;
            if (NumberToCollect <= 0) return -1;

            for (i = 1; i < (1 + NumberToCollect); i++) // first data line starts as row 1, not 0
            {
                string s = RawLines[i];
                j = s.IndexOf(sDataKey); // to the left is a date and the right is data
                if (j > 0)
                {
                    CreditStatistics.cCreditInfo ci = new cCreditInfo();
                    k = s.LastIndexOf("</td><td>", j);
                    k += 9;

                    RawLineValues = s.Substring(k).Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                    string sDT = RawLineValues[0];
                    if (DateTime.TryParseExact(sDT, eDTformats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                    {
                        ci.tCompleted = dateTime1;
                        UnsortedDT.Add(dateTime1);
                    }
                    else
                    {
                        ResultsBoxText = "Error - bad date time string";
                        RunDTsort();
                        return -1;
                    }

                    t = Convert.ToDouble(RawLineValues[2]);
                    t /= NumberConcurrent;
                    ci.ElapsedSecs = t;                    
                    
                    t = Convert.ToDouble(RawLineValues[3]);
                    t /= NumberConcurrent;
                    ci.CPUtimeSecs = t;                    

                    t = Convert.ToDouble(RawLineValues[4]);
                    t /= NumberConcurrent;
                    ci.Credits = t;
                    ci.mELA = ci.Credits / ci.ElapsedSecs;
                    mELA.Add(ci.mELA);
                    if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                    ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                    mCPU.Add(ci.mCPU);
                    ci.bValid = true;   // do not really know if it is valid yet
                    UnsortedLCI.Add(ci);
                    NumberRecordsRead++;
                }
                else
                {
                    ResultsBoxText = "Error - could not find first value on the page";
                    RunDTsort();
                    return -1;
                }
            }
            RunDTsort();
            return 0;
        }

        private void RunDTsort()
        {
            List<int> indices = Enumerable.Range(0, UnsortedDT.Count).ToList();
            indices.Sort((i1, i2) => UnsortedDT[i2].CompareTo(UnsortedDT[i1]));
            LCreditInfo = indices.Select(i => UnsortedLCI[i]).ToList();
        }

    }

}
