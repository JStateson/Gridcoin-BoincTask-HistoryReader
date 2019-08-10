using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int iDeviceUsed; //if -1 then CPU or not known  only applies to GPUs
        public string strReceived;
        public string strVMem;
        public string strMem;
        public string strOutput;
        public int iSystem;    // index to system data came from
        public int DatasetGroup;
    }

    public class cNameValue 
    {
        public string DataName;
        // todo dont need following if marking group id to value in points plotted
        public int SizeGroup;
        public void init(string sName)
        {
            SizeGroup=1;
            DataName = sName;
        }
    }

    // one set of names for each project name, not by app so putting "All" into sAppName
    // but might want make change later so not removing that field
    public class cDataName
    {
        int i;
        public string CurrentProject;   // needed by chart program for title
        public string sAppName;
        public List<cNameValue> DataNameInfo;
        public void Init(string sApp)
        {
            DataNameInfo = new List<cNameValue>();
            sAppName = sApp;
        }

        // lookup the dataset name and see if it can be subclassed
        // to allow differenct classes of data to be identified by color 
        private string GetAppName(string sFullName, string sProj)
        {
            int iTerm;
            string sOut = "";
            int nUnder = 0;
            int n = sFullName.Length;


            // rosetta does not have identifiable names
            // collatz and latin squares do not differentiate names, only apps 

            // gpugrid PABLO_DIST2_UCB_goal_KIX_CMY use "pablo" and take first 2 after the underscore
            //e84s1_e48s72p0f52-PABLO_v3O60885_MOR_31_IDP-1-2-RND3124_1 
            // web page tracks PABLO under personal bests
            if(sProj.Contains("GPUGRID"))
            {
                //-------------------------0123456789012
                iTerm = sFullName.IndexOf("_PABLO_");
                if (iTerm < 0) return "";
                sOut = sFullName.Substring(iTerm + 1, 8);
                return sOut;
            }

            //146733_Hs_T105121-KCNJ15_wu-113_1539963765552_
            // for TN-Grid use the Hs_ (second underscore)
            if (sProj.Contains("TN-Grid"))
            {
                for (int i = 0; i < n; i++)
                {
                    if (sFullName[i] == '_')
                    {
                        nUnder++;
                        if (nUnder == 2)
                        {
                            return sFullName.Substring(0, i);
                        }
                    }
                }
                return "";
            }

            //ZIKA_000439482_x5hmw_DENV3_NS5pol_s1_0460_0
            //MCM1_0151633_8911_2
            if (sProj.Contains("World C"))
            {                  //go with first 5 chars after the first underscore
                iTerm = sFullName.IndexOf('_');
                if (iTerm < 0) return "";
                return sFullName.Substring(0, iTerm + 5);
            }

            //universe_bh2_180328_261_3607081393_20000_1-999999_85100_0
            // go with digits after the second underscore
            if(sProj.Contains("Universe"))
            {
                for(int i = 0; i < n; i++)
                {
                    if(sFullName[i] == '_')
                    {
                        nUnder++;
                        if(nUnder == 4)
                        {
                            return sFullName.Substring(0, i);
                        }
                    }
                }
                return "";
            }

            if (sProj.Contains("Numberfields"))
            {                  //wu_sf6_DS-7x10_Grp78095of160000_0 use group as id
                iTerm = sFullName.IndexOf("_Grp");
                if (iTerm < 0) return "";
                return sFullName.Substring(0, iTerm);
            }


            if (sProj.Contains("Asteroids"))
            {                  //ps_190601_input_13257_5_0  go with _input_ and all before
                iTerm = sFullName.IndexOf("_input_");  
                if (iTerm < 0) return "";
                return sFullName.Substring(0, i);
            }

            if (sProj.Contains("SETI"))
            {
                // use decimal point as first guess  01jl11aa.10070.
                iTerm = sFullName.IndexOf('.');
                if (iTerm < 0) return "";
                sOut = sFullName.Substring(0, iTerm);
                iTerm = sOut.IndexOf("_");
                if (iTerm < 0) return sOut;
                // blc25_2bit_guppi_58406_2
                return sOut.Substring(0, iTerm);
            }
            if (sProj.Contains("Einstein"))
            {
                // use decimal point LATeah1049ZF_108.0_0_0.0_10547677_0
                iTerm = sFullName.IndexOf('.');
                if (iTerm < 0) return "";
                sOut = sFullName.Substring(0, iTerm);
                return sOut;
            }
            if (sProj.Contains("LHC@home"))
            {
                //workspace2_lhc2016_c12_o20_N1.1_e2.3__54__s__64.28_59.31__0_2__6__60_1_sixvf_boinc5255_0
                //workspace1_hl10_MCBRD_ranID_87_oct_0_B1__12__s__62.31_60.32__10_12__5__7.5_1_sixvf_boinc881_2
                //w-c0_30.000_job.B1inj.rf_c0_30.000.3012__5__s__64.28_59.31__2.1_6.1__6__27_1_sixvf_boinc4871_4
                //wfcchtc_job_arctripdipoctu_geb3_inj__5__s__109.28_107.31__3_4__5__4.5_1_sixvf_boinc8069_2 
                if (sFullName.Contains("lhc201"))
                {
                    iTerm = sFullName.IndexOf('.');
                    if (iTerm < 0) return "";
                    sOut = sFullName.Substring(0, iTerm);
                    iTerm = sOut.LastIndexOf('_');
                    if (iTerm < 0) return sOut;
                    return sOut.Substring(0, iTerm);
                }
                iTerm = sFullName.IndexOf("_job");
                if(iTerm > 0)
                {
                    int i = sFullName.IndexOf('-');
                    if(i < iTerm)
                    {
                        // want to return only the w-c0 and not the _30.000_job
                        i = sFullName.IndexOf('_');
                        if (i < 0) return "";
                        return sFullName.Substring(0, i);
                    }
                    return sFullName.Substring(0, iTerm);
                }
                iTerm = sFullName.IndexOf('_');
                if (iTerm < 0) return "";
                int j = sFullName.Substring(iTerm + 1).IndexOf('_');
                sOut = sFullName.Substring(0, iTerm + j + 1);
                return sOut;
            }
            if (sProj.Contains("Milky"))
            {
                // traverse right to left looking for first alpha char
                // then move right to first underscore
                //de_modfit_82_bundle5_3s_NoContraintsWithDisk200_6_1553189073_1614443_1
                for (int i = n - 1; i >= 0; i--)
                {
                    char aCH = sFullName[i];
                    if (aCH == '_') continue;
                    if (aCH < '0' || aCH > '9')
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            aCH = sFullName[j];
                            if (aCH == '_')
                            {
                                return sFullName.Substring(0, j);
                            }
                        }
                    }
                }
                return "";
            }
            return "";
        }


        public void Sort()
        {
            // todo
        }

        // sNameFull is the dataset name
        // sname is the abbreviated datasaet (if any) that could be a subclass of the data
        public int NameInsert(string sNameFull, string sProj)
        {
            int n = DataNameInfo.Count;
            cNameValue nv;
            string sName = GetAppName(sNameFull, sProj);
            if (sName == "")
            {
                sName = "ID Unknown";
            }
            if(n == 0)
            {
                nv = new cNameValue();
                nv.init(sName);
                DataNameInfo.Add(nv);
                return 0;
            }
            for(i = 0; i < n; i++)
            {
                if (sName == DataNameInfo[i].DataName)
                {
                    DataNameInfo[i].SizeGroup++;
                    return i;
                }
            }
            nv = new cNameValue();
            nv.init(sName);
            DataNameInfo.Add(nv);
            return n;
        }
    }

    public enum eShowType
    {
        DoingApps = 0,
        DoingSystems = 1,
        DoingSets = 2
    }


public class cSeriesData
    {
        public string strAppName;
        public string strProjName;
        public string strSysName;    // if gathering all data for a specific app then this field may contain multiple system names (scatter apps)
        public List<double> dValues;    // if scattering systems then only one app can be selected so this collection can only be homogenous
        public List<int> iSystem;       // would like to know which values above belong to which system  (scattering apps) -or-
                                        // this value will correspond with the elapsed time in the series.  for example
                                        // 1111222211   the first 2 indicates that system 2 ownes the 5 point in the et array
        public List<bool> bIsValid;         // normally true but set to false if suspected outlier (gpu stuck low speed)
        public eShowType ShowType;      // 0 for scatter apps, 1 for scatter systems, 2 for scatter datasets
        public int nConcurrent;
        public double dSmall;
        public double dBig;
        public List<string> TheseSystems;   // those systems that contributed to the elapsed times in the scatter plot
                                            // or which datasets (the name) that contributed
        public List<int> iTheseSystem;      // need to know the index of the system that is associated with iSystem above
                                            // which values belong to dataset when scattering datasets.  This is a small list
                                            // and each number is unique unless the same dataset is used by another app
                                            // for scattering datasets this is the group number that matches the dataset name
                                            // and does not correspond to a point
                                
        public string GetNameToShow(eShowType eST)
        {
            switch (eST)
            {
                case eShowType.DoingApps: return strAppName;
                case eShowType.DoingSystems: return strSysName;
                case eShowType.DoingSets: return strAppName;
            }
            return "";
        }
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
        State = 8,              // if 3 or 6 then aborted (6 for user abort)
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


        // read a line from history file and recored info
        // if 0 returned then all is ok
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
            //RtnCod |= (ElapsedTimeGpu == 0) ? (int)eHistoryError.MissingGPUtime : 0;
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
        public int iDeviceUsed; // if gpu we want id else =1
        public double Received;
        public double vMem;
        public double Mem;
        
    }

    // AppName could be "Milkyway@home Separation
    public class cAppName
    {
        public string Name;
        public cKnownProjApps ptrKPA;
        public cDataName DataName;
        public string strPlanClass;
        public string strName;
        public bool bUseThisAppInStatsListBox;
        public List<int> LineLoc;
        public List<double> dElapsedTime;
        public List<int> DataSetGroup;
        public List<bool> bIsValid;
        //  device id not useful in tree struct but could be shown
        public void AddETinfo(double d, int n, int j)
        {
            dElapsedTime.Add(d);
            DataSetGroup.Add(n);
            //DeviceID.Add(j);
        }
        public string GetInfo
        {
            get { return ptrKPA.ProjName + "\\" + Name; }
        }
        public int NumberBadWorkUnits;
        public double AvgRunTime;
        public double StdRunTime;
        public bool bNoResults = false;
        public string strAvgStd = "";
        public int nDevices;
        /*
        public List<int> DeviceID;
        public string GetDeviceStats(int idev)
        {
            double d, dArt=0.0, dSrt=0.0;
            int  nU = 0;
            for (int i = 0; i < bIsValid.Count; i++)
            {
                if (bIsValid[i] && idev == DeviceID[i])
                {
                    d = dElapsedTime[i] / 60.0; // want minutes
                    dArt += d;
                    nU++;
                }
            }
            if (nU == 0)
            {
                bNoResults = true;
                return "Device " + idev.ToString() + "not present in this sample";
            }
            AvgRunTime /= nU;
            for (int i = 0; i < bIsValid.Count; i++)
            {
                if (bIsValid[i] && idev == DeviceID[i])
                {
                    d = dElapsedTime[i] / 60.0;
                    d = d - dArt;
                    d = d * d;
                    dSrt += d;
                }
            }
            dSrt /= nU;
            dSrt = Math.Sqrt(StdRunTime);
            return "GPU" + idev.ToString() + ":"  + nU.ToString("##,##0") + "-" + dArt.ToString("###,##0.0") + "(" + dSrt.ToString("#,##0.0") + ")\r\n";        
        }
        */
        public bool DoAverages()            // return true if avg was calculated
        {
            int nUsed = 0;
            double d;
            nDevices = 0;
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
                    //nDevices = Math.Max(nDevices, DeviceID[i]); // GPUs are numbered 0..n-1 NOT USEFUL IN TREE STRUCT
                }
            }
            StdRunTime /= nUsed;
            StdRunTime = Math.Sqrt(StdRunTime);
            strAvgStd = nUsed.ToString("##,##0") + "-" +  AvgRunTime.ToString("###,##0.0") + "(" + StdRunTime.ToString("#,##0.0") + ")";
            return true;
        }

        public int nAppEntries
        {
            get { return LineLoc.Count; }
        }
        // the following is not being used it seems
        public void init(string sApp, string strProj, bool bIsUnk)
        {
            Name = sApp;
            DataName = new cDataName();
            DataName.Init(sApp);
            DataName.CurrentProject = strProj;
            LineLoc = new List<int>();
            dElapsedTime = new List<double>();
            DataSetGroup = new List<int>();
            bIsValid = new List<bool>();
            bIsUnknown = bIsUnk;
            //bUseThisAppInStatsListBox = true;   // unless specified otherwise in listbox
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

        public cAppName AddApp(string strName, string strPC)
        {
            cAppName AppName = new cAppName();
            AppName.init(strName, ProjName,false);
            AppName.ptrKPA = this;
            AppName.strPlanClass = strPC;
            AppName.Name = strName + " [" + strPC + "]";
            KnownApps.Add(AppName);
            bIgnore = false;  
            bIsUnknown = false;
            return AppName;
        }
        public cAppName AddUnkApp(string strIn)
        {
            cAppName AppName = new cAppName();
            AppName.init(strIn, ProjName,true);
            AppName.ptrKPA = this;
            KnownApps.Add(AppName);
            bIgnore = false;   
            bContainsUnknownApps = true;
            return AppName;
        }

        // look for known apps but if unknown found then insert it
        public cAppName SymbolInsert(string strAppName, int iLoc)
        {
            cAppName UnkAppName;

            foreach (cAppName AppName in KnownApps)
            {
                if (strAppName == AppName.Name)
                {
                    AppName.LineLoc.Add(iLoc);
                    if (AppName.LineLoc.Count == 1)
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
