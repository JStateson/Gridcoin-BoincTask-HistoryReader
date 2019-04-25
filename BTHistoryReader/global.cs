using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics;

namespace BTHistoryReader
{
    // fillowing struct is mostly filled in as history is parsed
    public class cProjectInfo
    {
        public string strLineNum;
        public string strProject;
        public string strApplication;
        public string strVersionNumber;
        public string strName;
        public string strPlanClass;
        public string strElapsedTimeCpu;    // actually this is the "elapsed time"
        public double dElapsedTime;
        public string strElapsedTimeGpu;    // actually this is the cpu time that is in parens ie: "00:02:45(00:00:17)"
        public double dElapsedCPU;  // this seems to really be CPU time
        public string strState;     // seems a '3' here is aborted or bad
        public bool bState; // if true then state is valid can can be counted in average & std computations
        public string strExitstatus;
        public string strReportedTime;
        public string strCompletedTime;
        public long time_t_Started;
        public long time_t_Completed;
        public long time_t_Diff_C_S;
        public string strUse;
        public string strReceived;
        public string strVMem;
        public string strMem;
        public string strOutput;
        public int iSystem;    // index to system data came from
    }



    public enum eHindex
    {
        Run = 0,
        Project = 1,
        Application = 2,
        VersionNumber = 3,
        Name = 4,
        PlanClass = 5,
        ElapsedTimeCpu = 6,    // if 0 here then dont use
        ElapsedTimeGpu = 7,    // if 0 here then dont use
        State = 8,              // if 3 then aborted
        ExitStatus = 9,
        ReportedTime = 10,
        CompletedTime = 11,
        Use = 12,
        Received = 14,
        VMem = 15,
        Mem = 16
    }
    
    public enum eHistoryError
    {
        SeemsOK = 0,
        EndHistory = -1,
        NoCompletionTime = 1,
        NoStartTime = 2,
        MissingCPUtime = 4,
        MissingGPUtime = 8,
        StateIs_3 = 16, // this is bad state?
        ConversionError = 32
    }

    public class cSplitHistoryValues
    {
        private string[] SplitHistoryLine;
        private string fmtHMS(long seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }
        public int RtnCod;
        public string strConversionError = "";
        public int StoreLineOfHistory(string OneHistoryLine, int ExpectedLengthLine)
        {
            if (OneHistoryLine.Length < ExpectedLengthLine) return (int)eHistoryError.EndHistory;  // cant be valid, lines are huge
            SplitHistoryLine = OneHistoryLine.Split('\t');
            try
            {
                return ProcessLine();
            }
            catch (Exception e)
            {
                RtnCod |= (int)eHistoryError.ConversionError;    // an unknown conversion
                strConversionError = (string)e.Message;
                return (int)eHistoryError.ConversionError;
            }
        }

        private int ProcessLine()
        {
            RtnCod = (int)eHistoryError.SeemsOK;
            Run = Convert.ToInt32(SplitHistoryLine[(int)eHindex.Run]);
            Project = SplitHistoryLine[(int)eHindex.Project];
            Application = SplitHistoryLine[(int)eHindex.Application];
            VersionNumber = Convert.ToInt32(SplitHistoryLine[(int)eHindex.VersionNumber]);
            Name = SplitHistoryLine[(int)eHindex.Name];
            PlanClass = SplitHistoryLine[(int)eHindex.PlanClass];
            try
            {
                ElapsedTimeCpu = Convert.ToInt64(SplitHistoryLine[(int)eHindex.ElapsedTimeCpu]);
            }
            catch
            {
                ElapsedTimeCpu = 0;
            }
            dElapsedTimeCpu = ElapsedTimeCpu;
            try
            {
                ElapsedTimeGpu = Convert.ToInt64(SplitHistoryLine[(int)eHindex.ElapsedTimeGpu]);
            }
            catch
            {
                ElapsedTimeGpu = 0;
            }
            dElapsedTimeGpu = ElapsedTimeGpu;
            State = Convert.ToInt32(SplitHistoryLine[(int)eHindex.State]);
            ExitStatus = Convert.ToInt32(SplitHistoryLine[(int)eHindex.ExitStatus]);
            try
            {
                ReportedTime = Convert.ToInt64(SplitHistoryLine[(int)eHindex.ReportedTime]);
                strReportedTime = fmtHMS(ReportedTime);
            }
            catch   //  this error is not critical as of now
            {
                ReportedTime = 0;
                strCompletedTime = "";
            }
            try
            {
                CompletedTime = Convert.ToInt64(SplitHistoryLine[(int)eHindex.CompletedTime]);
                strCompletedTime = fmtHMS(CompletedTime);
            }
            catch
            {
                CompletedTime = 0;
                strCompletedTime = "";
            }
            use = SplitHistoryLine[(int)eHindex.Use];
            // not useing the following
            //Received = Convert.ToDouble(SplitHistoryLine[(int)eHindex.Received]);
            //vMem = Convert.ToDouble(SplitHistoryLine[(int)eHindex.VMem]);
            //Mem = Convert.ToDouble(SplitHistoryLine[(int)eHindex.Mem]);
            RtnCod |= (ElapsedTimeCpu == 0) ? (int)eHistoryError.MissingCPUtime : 0;
            RtnCod |= (ElapsedTimeGpu == 0) ? (int)eHistoryError.MissingGPUtime : 0;
            RtnCod |= (ExitStatus == 3) ? (int)eHistoryError.StateIs_3 : 0;
            RtnCod |= (CompletedTime == 0) ? (int)eHistoryError.NoCompletionTime : 0;
            return RtnCod;
        }

        public int Run;
        public string Project;
        public string Application;
        public int VersionNumber;
        public string Name;
        public string PlanClass;
        public long ElapsedTimeCpu;
        public double dElapsedTimeCpu;
        public long ElapsedTimeGpu;
        public double dElapsedTimeGpu;
        public int State;
        public int ExitStatus;
        public long ReportedTime;
        public string strReportedTime;
        public long CompletedTime;
        public string strCompletedTime;
        public string use;
        public double Received;
        public double vMem;
        public double Mem;
        
    }


    public class cAppName
    {
        public string Name;
        public cKnownProjApps ptrKPA;
        public string GetInfo
        {
            get { return ptrKPA.ProjName + "\\" + Name; }
        }
        public int NumberBadWorkUnits;
        public double AvgRunTime;
        public double StdRunTime;
        public bool bNoResults = false;
        public string strAvgStd = "";
        public bool DoAverages()            // return true if avg was calculated
        {
            int nUsed = 0;
            double d;

            bNoResults = false;
            AvgRunTime = 0.0;
            StdRunTime = 0.0;
            for(int i = 0; i < bIsValid.Count;i++)
            {
                if(bIsValid[i])
                {
                    d = dElapsedTime[i] / 60.0; // want minutes
                    AvgRunTime += d;
                    nUsed++;
                }
            }
            if(nUsed == 0)
            {
                bNoResults = true;
                return false;
            }
            AvgRunTime /= nUsed;
            for (int i = 0; i < bIsValid.Count; i++)
            {
                if (bIsValid[i])
                {
                    d = dElapsedTime[i] / 60.0;
                    d = d - AvgRunTime;
                    d = d * d;
                    StdRunTime += d;
                }
            }
            StdRunTime /= nUsed;
            StdRunTime = Math.Sqrt(StdRunTime);
            strAvgStd = nUsed.ToString("##,##0") + "-" +  AvgRunTime.ToString("###,##0.0") + "(" + StdRunTime.ToString("#,##0.0") + ")";
            return true;
        }
        public List<int> LineLoc;
        public List<double> dElapsedTime;
        public List<bool> bIsValid;
        public int nAppEntries
        {
            get { return LineLoc.Count; }
        }
        public void init()
        {
            LineLoc.Clear();
            AvgRunTime = 0.0;
            StdRunTime = 0.0;
        }
        public bool bIsUnknown;
    }

    // this structure is the "symbol table" used to lookup apps found in history
    // it may be missing apps as new ones are created by the projects
    public class cKnownProjApps
    {
        public string ProjName;
        public bool bIsUnknown;
        public bool bContainsUnknownApps;
        public int NumberBadWorkUnits;
        public List<cAppName> KnownApps;
        public cAppName FindApp(string strName)
        {
            foreach(cAppName AppName in KnownApps)
            {
                if (AppName.Name == strName)
                    return AppName;
            }
            Debug.Assert(false);
            return null;
        }
        private int CountActualEntries()
        {
            int n = 0;
            foreach (cAppName AppName in KnownApps)
            {
                if (AppName.nAppEntries > 0) n++;
            }
            return n;
        }
        public int NumDValues()
        {
            int n = 0;
            foreach (cAppName AppName in KnownApps)
            {
                n += AppName.dElapsedTime.Count;
            }
            return n;
        }
        public int nAppsDefined         // there are this many in the lookup table
        {
            get { return KnownApps.Count; }
        }
        public int nAppsUsed         // there are this many referenced in the history file
        {
            get { return CountActualEntries(); }
        }
        public bool bIgnore;    // wuprop project is ignored or any that have no apps
        public void AddName(string strIn)
        {
            ProjName = strIn;
            KnownApps = new List<cAppName>();
            bIgnore = true; // assume no apps for this project
            bContainsUnknownApps = false;   // assume apps are known
            bIsUnknown = false;
        }
        public void AddUnkProj(string strIn)
        {
            ProjName = strIn;
            KnownApps = new List<cAppName>();
            bIgnore = true; // assume no apps for this project
            bIsUnknown = true;
        }

        public void AddApp(string strIn)
        {
            cAppName AppName = new cAppName();
            AppName.ptrKPA = this;
            AppName.Name = strIn;
            AppName.LineLoc = new List<int>();
            AppName.dElapsedTime = new List<double>();
            AppName.bIsValid = new List<bool>();
            KnownApps.Add(AppName);
            bIgnore = false;  
            bIsUnknown = false;
        }
        public cAppName AddUnkApp(string strIn)
        {
            cAppName AppName = new cAppName();
            AppName.Name = strIn;
            AppName.ptrKPA = this;
            AppName.LineLoc = new List<int>();
            AppName.dElapsedTime = new List<double>();
            AppName.bIsValid = new List<bool>();
            KnownApps.Add(AppName);
            bIgnore = false;   
            AppName.bIsUnknown = true;
            bContainsUnknownApps = true;
            return AppName;
        }

        // look for known apps but if unknown found then insert it
        public cAppName SymbolInsert(string strAppName, int iLoc)
        {
            cAppName UnkAppName;
            //if(strAppName.Contains( "camb_"))
            //{
            //    int i = 0;
            //}
            foreach (cAppName AppName in KnownApps)
            {
                if (strAppName == AppName.Name)
                {
                    AppName.LineLoc.Add(iLoc);
                    AppName.bIsUnknown = false;
                    return AppName;    // was a known app or was in database
                }
            }
            UnkAppName = AddUnkApp(strAppName);
            UnkAppName.LineLoc.Add(iLoc);
            UnkAppName.bIsUnknown = true;
            return UnkAppName; // was an unknown app
        }

        // remove all traces of app results
        public void EraseAppInfo()
        {
            NumberBadWorkUnits = 0;
            foreach(cAppName AppName in KnownApps)
            {
                AppName.LineLoc.Clear();
                AppName.dElapsedTime.Clear();
                AppName.bIsValid.Clear();
                AppName.NumberBadWorkUnits = 0;
            }
        }
    }

}
