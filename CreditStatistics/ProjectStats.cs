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

/*
 ---------- ALL_PROJECTS_LIST.XML

        https://www.rnaworld.de/rnaworld/
        https://root.ithena.net/usr/

        https://boinc.berkeley.edu/central/

*/


namespace CreditStatistics
{


    public class cPSlist
    {
        public string name;
        public string sURL;
        public string sHid; 
        public string sValid;
        public string sPage;
        public string sCountValids;
    }
    public class cProjectStats
    {
        public List<cPSlist> ProjectList = new List<cPSlist>();
        public static readonly string[] KnownProjects = @"
sidock
https://www.sidock.si/sidock/results.php?
hostid=
&state=4
&offset=
 Valid () .

denis
https://denis.usj.es/denisathome/results.php?
hostid=
&state=4
&offset=
 Valid () .

gpugrid
https://gpugrid.net/gpugrid/results.php?
hostid=
&state=4
&offset=
 Valid () .

rosetta
https://boinc.bakerlab.org/rosetta/results.php?
hostid=
&state=4
&offset=
 Valid () .

asteroids
https://asteroidsathome.net/boinc/results.php?
hostid=
&state=4
&offset=
 Valid () .

milkyway
https://milkyway.cs.rpi.edu/milkyway/results.php?
hostid=
&state=4
&offset=
 Valid () .

einstein
https://einsteinathome.org
/host/
/tasks/4/0
?page=
>Valid ()</span>.

lhc
https://lhcathome.cern.ch/lhcathome/results.php?
hostid=
&state=4
&offset=
 Valid () .

amicable
https://sech.me/boinc/Amicable/results.php?
hostid=
&state=4
&offset=
 Valid () .

numberfields
https://numberfields.asu.edu/NumberFields/results.php?
hostid=
&state=4
&offset=
 Valid () .

odlk progger
https://boinc.progger.info/odlk/results.php?
hostid=
&state=4
&offset=
 Valid () .

odlk1 latinsquares
https://boinc.multi-pool.info/latinsquares/results.php?
hostid=
&state=4
&offset=
 Valid () .

moowrap
https://moowrap.net/results.php?
hostid=
&state=4
&offset=
 Valid () .

nfs escatter
https://escatter11.fullerton.edu/nfs/results.php?
hostid=
&state=4
&offset=
 Valid () .

PrimeGrid
https://www.primegrid.com/results.php?
hostid=
&state=4
&offset=
<b>Valid</b> 0 |.

gerasim
https://gerasim.boinc.ru/users/viewHostResults.aspx?
hostid=
&opt=2
null
>Valid 0 </a>.

srbase
https://srbase.my-firewall.org/sr5/results.php?
hostid=
&state=4
&offset=
 Valid () .

rakesearch
https://rake.boincfast.ru/rakesearch/results.php?
hostid=
&state=4
&offset=
 Valid () .

rnma
https://rnma.xyz/boinc/results.php?
hostid=
&state=4
&offset=
 Valid () .

loda
https://boinc.loda-lang.org/loda/results.php?
hostid=
&state=4
&offset=
 Valid () .

yafu
https://yafu.myfirewall.org/yafu/results.php?
hostid=
&state=4
&offset=
 Valid () .

cpdn ClimatePrediction
https://main.cpdn.org/results.php?
hostid=
&state=4
&offset=
 Valid () .

yoyo
https://www.rechenkraft.net/yoyo/results.php?
hostid=
null
&offset=
null

WCG WORLDCOMMUNITYGRID
https://www.worldcommunitygrid.org/contribution
/device?id=
&type=B
null
null

radioactive
http://radioactiveathome.org/boinc/results.php?
hostid=
&state=3
&offset=
null
"
        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        public void Init()
        {
            int i, j, n = KnownProjects.Length;
            for (j = 0; j < n; j += 6)
            {
                i = j;
                ProjectList.Add(new cPSlist()
                {
                    name = KnownProjects[i++].ToLower(),
                    sURL = KnownProjects[i++],
                    sHid = KnownProjects[i++],
                    sValid = KnownProjects[i++],
                    sPage = KnownProjects[i++],
                    sCountValids = KnownProjects[i]
                });
            }
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

        private string GetBaseURL(int iTask, string sID, string sPage)
        {
            cPSlist p = ProjectList[iTask];
            string s = p.sURL + p.sHid + sID + p.sValid;
            if (sPage == "") return s;
            return s + p.sPage + sPage;
        }

        public string GetURL0(string name, string sID, ref bool HasValids)
        {
            HasValids = true;   // unless otherwise
            foreach (var p in ProjectList)
            {
                if (p.name == name)
                {
                    string s = p.sURL + p.sHid;
                    s = s + sID;
                    if (p.sValid == "&state=4")
                    {
                        s += p.sValid;
                        s += p.sPage + "0";
                        //Debug.Assert(p.sCountValids == " Valid () .");
                        //sTermDelim = p.sCountValids;
                        return s;
                    }
                    if (p.sValid == "&state=3")
                    {
                        s += p.sValid;
                        s += p.sPage + "0";
                        //Debug.Assert(p.sCountValids == "null");
                        //sTermDelim = p.sCountValids;
                        return s;
                    }
                    if (p.sValid == "/tasks/4/0")
                    {
                        s += p.sValid;
                        s += p.sPage + "0";
                        //Debug.Assert(p.sCountValids == ">Valid ()</span>.");
                        //sTermDelim = p.sCountValids;
                        return s;
                    }
                    if (p.sValid == "&opt=2")
                    {
                        s += p.sValid;
                        //Debug.Assert(p.sCountValids == ">Valid 0 </a>.");
                        //sTermDelim = p.sCountValids;
                        HasValids = false;
                        return s;
                    }
                    if (p.sValid == "&type=B")
                    {
                        s += p.sValid;
                        //Debug.Assert(p.sCountValids == "null");
                        Debug.Assert(p.sPage == "null");
                        //sTermDelim = p.sCountValids;
                        HasValids = false;
                        return s;
                    }
                }
            }
            return "";
        }

        public class cLHe
        {
            public string name;
            public string HostID;
        }
        public List<cLHe> LocalHosts = new List<cLHe>();

        private string GetName(string s)
        {
            foreach (cPSlist c in ProjectList)
            {
                string[] st = c.name.Split(' ');
                foreach (string s1 in st)
                {
                    if (s.Contains(s1))
                    {
                        return c.name;
                    }
                }
            }
            return "";
        }

        public int GetNameIndex(string s)
        {
            int i = 0;
            foreach (cPSlist c in ProjectList)
            {
                string[] st = c.name.Split(' ');
                foreach (string s1 in st)
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
        public void GetHosts(string sBoincLoc)
        {
            LocalHosts.Clear();

            string[] LookFiles = { "SCHED_REP*", "SCHED_REQ*" };
            if (Directory.Exists(sBoincLoc))
            {
                for (int k = 0; k < 2; k++)
                {
                    string[] files = Directory.GetFiles(sBoincLoc, LookFiles[k]);
                    foreach (string filepath in files)
                    {
                        string sName = GetName(filepath.ToLower());
                        if (sName == "") continue;
                        string content = File.ReadAllText(filepath);
                        int i = content.IndexOf("<hostid>");
                        if (i < 0) continue;
                        i += 8;
                        int j = content.IndexOf("</hostid>", i);
                        if (j < 0) continue;
                        string sID = content.Substring(i, j - i);
                        if (sID == "0") continue;
                        cLHe cLHe = new cLHe();
                        bool bFound = false;
                        foreach (cLHe c in LocalHosts)
                        {
                            if (c.name == sName)
                            {
                                bFound = true;
                                break;
                            }
                        }
                        if (bFound) continue;
                        cLHe.name = sName;
                        cLHe.HostID = sID;
                        LocalHosts.Add(cLHe);
                    }
                }

            }

        }

        public string GetIDfromName(string name)
        {
            foreach (cLHe c in LocalHosts)
            {
                if (c.name == name)
                {
                    return c.HostID;
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
        private int TaskProgress;    // progress or busy
        public string RawPage;
        public string RawTable;
        private string TaskName;       // project name
        public string TaskUrl;      // base url, all except offset
        private int TaskOffset;   // could be page 0,1,etc or 20,40, etc
        private string sTaskOffset;   // could be page 0,1,etc or 20,40, etc
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
        public List<cCreditInfo> LCreditInfo =     new List<cCreditInfo>();
        private string[] RawLines;
        public List<double> mCPU = new List<double>();
        public List<double> mELA = new List<double>();
        public void ConfigureTask(int jTask, string sHost, string sOffset, int iWanted, string sType)
        {
            if (jTask < 0) return;
            iTask = jTask;
            NumValid = 0;
            sTaskType = sType;
            sTaskHost = sHost;
            TasksWanted = iWanted;
            TaskProgress = 0;
            StopTask = false;
            TaskBusy = false;
            TaskDone = false;
            cPSlist p = ProjectList[jTask];
            TaskName = p.name;
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
                TaskOffset = Convert.ToInt32(sTaskOffset);
                if (p.sPage.Contains("page")) TaskIncrement = 1;
                else if (p.sPage.Contains("offset")) TaskIncrement = 20;
                else HasOffset = false;
            }
        }
        private bool URLincrementOffset()
        {
            if (HasOffset)
            {
                TaskOffset += TaskIncrement;
            }
            else return false;
            sTaskOffset = TaskOffset.ToString();
            TaskUrl = GetBaseURL(iTask, sTaskHost, sTaskOffset);
            return true;
        }

        public int GetTableFromRaw()
        {
            int iStart, iEnd;
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

            string[] formats = { "d MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, h:mm:ss tt UTC" };
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

        int NumberToCollect = 20;
        private int BuildEinsteinStatsTable()
        {
            int i;
            int j;
            int iIndex = nTotalSamples;
            double t;
            string[] RawLineValues;

            RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            NumberToCollect = Math.Min(NumberToCollect, RawLines.Length - 1);
            if (RawLines.Count() < NumberToCollect)
            {
                ResultsBoxText = "Error - must have at least " + NumberToCollect.ToString() + " values on a page";
                return -1;
            }
            for (i = 1; i < (1 + NumberToCollect); i++) // first data line starts as row 1, not 0
            {
                j = RawLines[i].IndexOf("Completed and validated");
                if (j > 0)
                {
                    CreditStatistics.cCreditInfo ci = new cCreditInfo();
                    j += 32;    // at first value: "2,224</td><td>369</td><td>3,465</td>"
                    RawLineValues = RawLines[i].Substring(j, 60).Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                    t = Convert.ToDouble(RawLineValues[0]);
                    t /= NumberConcurrent;
                    ci.ElapsedSecs = t;                    
                    
                    t = Convert.ToDouble(RawLineValues[1]);
                    t /= NumberConcurrent;
                    ci.CPUtimeSecs = t;                    

                    t = Convert.ToDouble(RawLineValues[2]);
                    t /= NumberConcurrent;
                    ci.Credits = t;
                    ci.mELA = ci.Credits / ci.ElapsedSecs;
                    mELA.Add(ci.mELA);
                    if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                    ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                    mCPU.Add(ci.mCPU);
                    ci.bValid = true;   // do not really know if it is valid yet
                    LCreditInfo.Add(ci);
                }
                else
                {
                    ResultsBoxText = "Error - could not find first value on the page";
                    return -1;
                }
                iIndex++;
            }
            nTotalSamples = iIndex;
            return 0;
        }

    }

}
