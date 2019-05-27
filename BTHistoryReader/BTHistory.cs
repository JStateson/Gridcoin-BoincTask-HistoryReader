using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace BTHistoryReader
{
    public partial class BTHistory : Form
    {

        public int AnalysisType;
        public string ThisSystem;
        public cSplitHistoryValues OneSplitLine;    // use this for processing each line in history file;

        private List<long> CompletionTimes;
        private List<double> IdleGap;
        private double AvgGap;
        private double StdGap;

        public BTHistory()
        {
            InitializeComponent();
        }

        private int iLocMaxDiff;
        public string[] AllHistories;

        static string str_PathToHistory;
        public string[] LinesHistory;
        static string ReqVer = "1.79";
        static string ReqID = "BoincTasks History";
        static double dAvgCreditPerUnit;
        static int iPadSize;
        static int[] iSortIndex;
        static int LastKnownProject = 0;
        public int NumberBadWorkUnits;
        static int ExpectedLengthLine = 100;

        const int LKUP_NOT_FOUND = -1;      // cannot find project- forgot it or new one
        const int LKUP_TOBE_IGNORED = -2;   // do not use this project
        const int LKUP_INVALID_LINE = -3;   // line in history is invalid 


        public string CurrentSystem;    // computer name
        public string CurrentProject;   // project being looked at
        public List<cProjectInfo> ThisProjectInfo;

        // pad right side with spaces to fill
        public static string Rpadto(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return strIn + "                              ".Substring(0, i);
        }

        // pad left side with spaces to fill
        public static string Lpadto(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return "                              ".Substring(0, i) + strIn;
        }

        // output  is 1, 2 or 3 digit number formatted with space  after (example for "3") shown
        //  eg:  xxxxx  1 xxxx
        //       xxxxx123 xxx
        public static string fmtLineNumber(string strVal)
        {
            return Lpadto(strVal, iPadSize) + " ";
        }


        public int LookupApp(string strIn, int iLoc)
        {
            int n = 0;
            foreach (cAppName AppName in KnownProjApps[iLoc].KnownApps)
            {
                if (strIn == AppName.Name)
                {
                    return n;
                }
                n++;
            }
            return -1;
        }

        public int LookupProject(string strName)
        {
            int i = 0;
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                if (strName == kpa.ProjName) return i;
                i++;
            }
            return LKUP_NOT_FOUND;
        }

        public string BestTimeUnits(long t)
        {
            double d = Convert.ToDouble(t);
            string strOut = " secs";
            if (t < 60) return t.ToString() + strOut;
            strOut = " mins";
            d /= 60.0;
            if (d < 60) return d.ToString("#0.0") + strOut;
            strOut = " hours";
            d /= 60;
            if (d < 24) return d.ToString("#0.00") + strOut;
            d /= 24;
            return d.ToString("##0.0") + " days";
        }


        // this is our lookup table
        public List<cKnownProjApps> KnownProjApps;

        // lookup name of project in table
        public int LookupProj(string strProjName)
        {
            int iIndex = 0;
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                if (strProjName == kpa.ProjName)
                {
                    if (kpa.bIgnore) return LKUP_TOBE_IGNORED;
                    return iIndex;
                }
                iIndex++;
            }
            return LKUP_NOT_FOUND;
        }

        // put some projects into the table but unknown (to this program) also get added
        // there is no limit on projects nor apps
        private void InitLookupTable()
        {
            cKnownProjApps kpa;
            KnownProjApps = new List<cKnownProjApps>();
            OneSplitLine = new cSplitHistoryValues();
            kpa = new cKnownProjApps();
            kpa.AddName("Milkyway@Home");
            kpa.AddApp("Milkyway@home Separation", "opencl_ati_101");
            kpa.AddApp("Milkyway@home Separation", "opencl_nvidia_101");
            kpa.AddApp("Milkyway@home Separation", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("SETI@home");
            kpa.AddApp("SETI@home v8", "opencl_ati5_SoG_nocal");
            kpa.AddApp("SETI@home v8", "opencl_atiapu_sah");
            kpa.AddApp("SETI@home v8", "opencl_ati5_nocal");
            kpa.AddApp("SETI@home v8", "");
            kpa.AddApp("AstroPulse v7", "opencl_ati_100");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("collatz");
            kpa.AddApp("Collatz Sieve","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Amicable Numbers");
            kpa.AddApp("Amicable Numbers up to 10^20","opencl_amd");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("LHC@home");
            kpa.AddApp("SixTrack","");
            kpa.AddApp("SixTrack", "avx");
            kpa.AddApp("SixTrack", "sse2");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("World Community Grid");
            kpa.AddApp("Mapping Cancer Markers","");
            kpa.AddApp("FightAIDS@Home - Phase 1","");
            kpa.AddApp("FightAIDS@Home - Phase 2","");
            kpa.AddApp("OpenZika","");
            kpa.AddApp("Microbiome Immunity Project","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("GPUGRID");
            kpa.AddApp("Short runs (2-3 hours on fastest card)","cuda80");
            kpa.AddApp("Long runs (8-12 hours on fastest card)","cuda80");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NFS@Home");
            kpa.AddApp("15e Lattice Sieve","");
            kpa.AddApp("15e Lattice Sieve", "notphenomiix6");
            kpa.AddApp("16e Lattice Sieve V5","");
            kpa.AddApp("14e Lattice Sieve","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Cosmology@Home");
            kpa.AddApp("camb_legacy","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Rosetta@home");
            kpa.AddApp("Rosetta","");
            kpa.AddApp("Rosetta Mini","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("TN-Grid Platform");
            kpa.AddApp("gene@home PC-IM","sse2");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NumberFields@home");
            kpa.AddApp("Get Decic Fields","default");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Enigma@Home");
            kpa.AddApp("Enigma GPU","");
            kpa.AddApp("Enigma GPU", "cuda_fermi");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("PrimeGrid");
            kpa.AddApp("PPS (Sieve)","cudaPPSsieve");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Einstein@Home");
            kpa.AddApp("Gravitational Wave Engineering run on LIGO O1 Open Data","");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-ati");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-nvidia");
            KnownProjApps.Add(kpa);


            // this project ignored
            kpa = new cKnownProjApps();
            kpa.AddName("WUProp@Home");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("latinsquares");
            kpa.AddApp("odlk3@home","");
            kpa.AddApp("odlkmax@home","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("yoyo@home");
            kpa.AddApp("Siever","");
            kpa.AddApp("Cruncher ogr","");
            kpa.AddApp("ecm","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Asteroids@home");
            kpa.AddApp("Period Search Application","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Moo! Wrapper");
            kpa.AddApp("Distributed.net Client","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Gerasim@home");
            kpa.AddApp("spstarter","");
            kpa.AddApp("Test separator","");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Universe@Home");
            kpa.AddApp("Universe BHspin v2","");
            KnownProjApps.Add(kpa);


            //lb_NumKnown.Text = "Known Projects: " + KnownProjApps.Count.ToString();
            LastKnownProject = KnownProjApps.Count;
        }

        public void ClearInfoTables()
        {
            tb_AvgCredit.Text = "0";
            tb_Info.Text = "";
            tb_Results.Text = "";
        }

        public void ClearPreviousHistory()
        {
            if (ThisProjectInfo != null)
                ThisProjectInfo.Clear();
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                kpa.EraseAppInfo();
            }
            lb_SelWorkUnits.Items.Clear();
            cb_AppNames.Items.Clear();
            cb_SelProj.Items.Clear();
            cb_SelProj.Text = "";
            cb_AppNames.Text = "";
            lbSeriesTime.Text = "";
            ClearInfoTables();
        }

        private void ShowNumberApps()
        {
            lb_nApps.Visible = cb_AppNames.Items.Count > 0;
            btnFetchHistory.Enabled = lb_nApps.Visible;
            if (lb_nApps.Visible)
            {
                lb_nApps.Text = cb_AppNames.Items.Count.ToString();
            }
        }

        private int LocatePathToHistory()
        {
            string str_WantedDirectory = "\\EFmer\\BoincTasks\\history";
            string str_LookHere = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int nFiles = 0;

            ofd_history.DefaultExt = ".cvs?";
            str_PathToHistory = str_LookHere + str_WantedDirectory;
            tb_Results.Text = "";
            if (!Directory.Exists(str_PathToHistory))
            {
                tb_Info.Text = "Cannot find " + str_PathToHistory + "\r\n---Trying\r\n";
                // look where executable is located
                str_PathToHistory = Directory.GetCurrentDirectory();
                str_LookHere = str_PathToHistory + "\r\n";
                tb_Info.Text += str_LookHere;
            }
            foreach (string sFile in Directory.GetFiles(str_PathToHistory, "*.cvs"))
                nFiles++;

            ofd_history.InitialDirectory = str_PathToHistory;
            return nFiles;
        }

        private void PerformSelectCompare()
        {
            CompareHistories MyHistories = new CompareHistories(this, CBoxLONG.Checked);
            MyHistories.ShowDialog();
            ClearPreviousHistory();
        }

        private bool FetchHistory()
        {
            int nFiles = LocatePathToHistory();
            bool bAny = nFiles > 0;

            lb_nProj.Visible = false;
            lb_nApps.Visible = false;
            bt_all.Enabled = false;

            ofd_history.ShowDialog();
            AllHistories = ofd_history.FileNames;
            if (AllHistories.Length > 1 )
            {
                PerformSelectCompare();
                return true;
            }
            lb_history_loc.Text = ofd_history.FileName;
            if (File.Exists(lb_history_loc.Text))
            {
                str_PathToHistory = lb_history_loc.Text;
                if (ValidateHistory(str_PathToHistory) >= 0)
                {
                    ClearPreviousHistory();
                    CurrentSystem = LinesHistory[1];
                    if (BTHistory.ActiveForm != null)    // can occur during debugging
                        BTHistory.ActiveForm.Text = CurrentSystem; ;   // this is name of the computer
                    ProcessHistoryFile();
                    FillSelectBoxes();
                }
                else
                {
                    tb_Info.Text += "problem with history file\r\n";
                    return false;
                }
            }
            else
            {
                tb_Info.Text += "file does not exist\r\n";
                str_PathToHistory = "";
                return false;
            }
            bAny = cb_SelProj.Items.Count > 0;
            lb_nProj.Visible = bAny;
            lb_nProj.Text = bAny ? cb_SelProj.Items.Count.ToString() : "";
            ShowNumberApps();
            return true;
        }

        private void btn_OpenHistory_Click(object sender, EventArgs e)
        {
            InitLookupTable();
            if (FetchHistory())return;
            ClearPreviousHistory();
            ShowContinunities(false);
            ShowSelectable(false);
        }

        public int ValidateHistory(string strFile)
        {
            int i = 0;

            try
            {
                LinesHistory = File.ReadAllLines(strFile);
                do
                {
                    if (LinesHistory.Length == 0) break;
                    i = LinesHistory.Last().Length;
                    if (i < ExpectedLengthLine)
                    {
                        Array.Resize(ref LinesHistory, LinesHistory.Length - 1);
                    }
                    else break;
                } while (true);

            }
            catch (Exception e)
            {
                tb_Info.Text += (string)e.Data["MSG"] + "\r\n";
                return -1;
            }
            if (LinesHistory[2] == ReqID && LinesHistory[1] != "")  // no longer check for version number
            {
                iPadSize = Convert.ToInt32(Math.Ceiling(Math.Log10(LinesHistory.Length)));
                // want to know how many digits to format data in combobox view
                //OneSplitLine.StoreLineOfHistory(LinesHistory[3]);    // this was used to look at the header items
                ThisSystem = LinesHistory[1];
                return 0;
            }
            else
            {
                tb_Info.Text += "cannot find " + ReqID + " or maybe empty systemn name\r\n";
                return -2;
            }
        }

        // iLoc is index to project table and we need list of apps to show
        void FillAppBox(int iLoc)
        {
            int i=0, n = 0;
            bool bAny = false;

            cb_AppNames.Items.Clear();
            foreach (cAppName appName in KnownProjApps[iLoc].KnownApps)
            {
                n = appName.nAppEntries;
                if(n > 0)
                {
                    bAny = true;
                    cb_AppNames.Items.Add(appName.Name + "  (" + n.ToString() + ")");
                }
                i++;
            }
            Debug.Assert(bAny);
            cb_AppNames.Text = cb_AppNames.Items[0].ToString();
            cb_AppNames.Tag = i;    // use tag to restore any edits to the combo box as I cant make it readonly
        }

        public void PerformCalcAverages()
        {
            foreach(cKnownProjApps kpa in KnownProjApps)
            {
                foreach(cAppName AppName in kpa.KnownApps)
                {
                    AppName.DoAverages();
                }
            }
        }

        // get list of projects and their apps
        // save all information in the KnownProjApp table
        public int ProcessHistoryFile()
        {
            int iLine = -4;  // if > 4 then 
            int RtnCode=0;
            int eInvalid=0;   // invalid line in history (not complete or whatever)
            cAppName AppName;
            cKnownProjApps kpa;

            // find and identify any project in the file
            foreach (string s in LinesHistory)
            {
                iLine++;
                if (iLine < 1) continue;    // skip past header
                // possible sanity check here: iLine is 1 and first token of "s" is also 1
                eInvalid = OneSplitLine.StoreLineOfHistory(s, ExpectedLengthLine);
                RtnCode = LookupProj(OneSplitLine.Project);
                if (RtnCode < 0)
                {
                    if(RtnCode == LKUP_NOT_FOUND)
                    {
                        tb_Info.Text += "Cannot find project: " + OneSplitLine.Project + " adding to database\r\n";
                        kpa = new cKnownProjApps();
                        kpa.AddUnkProj(OneSplitLine.Project);
                        KnownProjApps.Add(kpa);
                        RtnCode = KnownProjApps.Count-1;  // put unknown project here
                    }
                    else continue;
                }
                // if the app is found then point to the line containing the app's info
                // and put all info also 
                // jys adding plan class info
                AppName = KnownProjApps[RtnCode].SymbolInsert(OneSplitLine.Application + " [" + OneSplitLine.PlanClass + "]", 3+iLine);  // first real data is in 5th line (0..4)
                AppName.dElapsedTime.Add(OneSplitLine.dElapsedTimeCpu); // want to calculate average time this app
                AppName.bIsValid.Add(eInvalid == (int)eHistoryError.SeemsOK);
                if (AppName.bIsUnknown)
                {
                    tb_Info.Text += "Unk App " + AppName.GetInfo + " added to database\r\n";
                }
            }
            PerformCalcAverages();
            return 0;
        }




        // fill in the project selection combo box
        void FillSelectBoxes()
        {
            int n;
            string strProjName;

            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                n = kpa.nAppsUsed;
                if (n > 0 && !kpa.bIgnore)
                {
                    cb_SelProj.Items.Add(kpa.ProjName);
                }
            }
            if (cb_SelProj.Items.Count == 0) return;
            strProjName = cb_SelProj.Items[0].ToString();
            n = LookupProject(strProjName);
            cb_SelProj.Text = strProjName;
            cb_SelProj.Tag = n;                     // tag select project box with current project#
            FillAppBox(n);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cb_SelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cb_SelProj.SelectedIndex;
            cb_SelProj.Text = cb_SelProj.Items[i].ToString();
            i = LookupProject(cb_SelProj.Text);
            FillAppBox(i);
        }

        // standard bubble sort with exchange on index
        public void SortTimeIncreasing(int nSort)
        {
            iSortIndex = new int[nSort];
            int i, j, k;
            int j1, j2;
            string sTemp;
            k = nSort - 2;
            for (i = 0; i < nSort; i++)
                iSortIndex[i] = i;
            for(i=0; i < k; i++)
            {
                for(j = 0; j < k; j++)
                {
                    j1 = iSortIndex[j];
                    j2 = iSortIndex[j + 1];
                    if(ThisProjectInfo[j1].time_t_Completed > ThisProjectInfo[j2].time_t_Completed)
                    {
                        iSortIndex[j] = j2;
                        iSortIndex[j+1] = j1;
                    }
                }
            }
            for(i=0; i<nSort; i++)
            {
                j = iSortIndex[i];
                if (!ThisProjectInfo[j].bState) continue;
                sTemp = ThisProjectInfo[j].strOutput;
                lb_SelWorkUnits.Items.Add(sTemp);
            }
        }

        static string fmtHMS(long seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }

        public int FillProjectInfo(cAppName AppName)
        {
            string[] strSymbols;
            string sTemp;
            System.DateTime dt_1970 = new System.DateTime(1970, 1, 1);
            System.DateTime dt_this;
            int j = 0;
            bool bState;
            long n, nElapsedTime;
            int RunNumber;  // the line number in the history

            if (AppName.LineLoc.Count == 0) return 0;

            foreach (int i in AppName.LineLoc)  // this needs to be re-written to use the SplitLine stuff
            {
                bState = LinesHistory[i].Length > ExpectedLengthLine;
                if(!bState)
                {
                    break;
                }
                strSymbols = LinesHistory[i].Split('\t');
                ThisProjectInfo[j].strLineNum = strSymbols[(int)eHindex.Run];
                //RunNumber = Convert.ToInt32(ThisProjectInfo[j].strLineNum);
                sTemp = strSymbols[(int)eHindex.CompletedTime];                             // this is completed time in seconds based on 1970    
                n = Convert.ToInt64(sTemp);                         // want to convert to time stamp
                ThisProjectInfo[j].time_t_Completed = n;
                if (n <= 0)   // is 0 if not calculated yet
                {
                    tb_Info.Text += "No completion time " + AppName.GetInfo + " at line " + i.ToString() + "\r\n" ;
                    // may not have finished yet ???
                    continue;  
                }
                dt_this = DateTime.SpecifyKind(dt_1970.AddSeconds(n),DateTimeKind.Utc);
                sTemp = fmtLineNumber(strSymbols[(int)eHindex.Run]) + dt_this.ToLocalTime().ToString();
                ThisProjectInfo[j].strCompletedTime = sTemp;        // save in readable format
                nElapsedTime = Convert.ToInt64(strSymbols[(int)eHindex.ElapsedTimeCpu].ToString()); // this is actually elapsed time
                // the below is actually CPU time as it appears headers in history are reversed for these two items
                ThisProjectInfo[j].dElapsedCPU = Convert.ToDouble(strSymbols[(int)eHindex.ElapsedTimeGpu].ToString());
                ThisProjectInfo[j].dElapsedTime = nElapsedTime;

                // try to find which history entries are bad and mark them out of statistical calculations for "avg" 
                // they still count in thruput

                if (strSymbols[(int)eHindex.State].ToString() == "3") bState = false;
                if (strSymbols[(int)eHindex.State].ToString() == "6") bState = false;
                if(nElapsedTime == 0)bState = false;
                if(ThisProjectInfo[j].dElapsedCPU == 0.0)bState = false;
                ThisProjectInfo[j].bState = bState;
                if(!bState)
                {
                    AppName.NumberBadWorkUnits++;
                    tb_Info.Text += "Bad exit status line " + ThisProjectInfo[j].strLineNum + "\r\n";
                }
                n -= nElapsedTime;                                  // get the correct start time as best as we can
                ThisProjectInfo[j].time_t_Started = n;              // needed to calculate throughput
                ThisProjectInfo[j].strElapsedTimeCpu = fmtHMS(nElapsedTime);
                ThisProjectInfo[j].strElapsedTimeGpu = fmtHMS(Convert.ToInt64(strSymbols[(int)eHindex.ElapsedTimeGpu].ToString()));
                sTemp += " " + ThisProjectInfo[j].strElapsedTimeCpu;
                //    "(" + ThisProjectInfo[j].strElapsedTimeGpu + ")";
                ThisProjectInfo[j].strOutput = sTemp;               // eventually put into our text box to allow selections
                j++;
            }
            SortTimeIncreasing(j);
            return j;
        }

        private void PerformThruput()
        {

            long t_start, t_stop, t_diff;
            int i, j, k;
            int i1, i2; // used to access iSort..
            double dSeconds = 0;
            int nItems, nDevices;
            double dUnitsPerSecond;
            int NumUnits = lb_SelWorkUnits.SelectedItems.Count;
            string sTemp, s1, s2;

            if (NumUnits != 2)
            {
                tb_Results.Text = "you must select exactly two items\r\n";
                return;
            }
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            nDevices = Convert.ToInt32(tbNDevices.Text);
            sTemp = lb_SelWorkUnits.SelectedItems[0].ToString();
            //i1 = Convert.ToInt32(sTemp.Substring(0, iPadSize)) - 1;  // origin is 0 not 1
            s1 = sTemp.Substring(iPadSize + 1); // remove digits and space
            k = s1.IndexOf("M ");
            s1 = s1.Substring(0, k + 1);

            sTemp = lb_SelWorkUnits.SelectedItems[1].ToString();
            //i2 = Convert.ToInt32(sTemp.Substring(0, iPadSize)) - 1;
            s2 = sTemp.Substring(iPadSize + 1); // remove digits and space
            k = s2.IndexOf("M ");
            s2 = s2.Substring(0, k + 1);

            t_start = ThisProjectInfo[iSortIndex[i]].time_t_Started;
            t_stop = ThisProjectInfo[iSortIndex[j]].time_t_Completed;

            tb_Results.Text = "Start time " + s1 + "\r\n";
            tb_Results.Text += "Stop  time " + s2 + "\r\n";
            t_diff = t_stop - t_start;  // seconds
            dSeconds = (double)t_diff;
            nItems = 1 + j - i;
            dUnitsPerSecond = nItems / dSeconds;
            tb_Results.Text += "Elapsed seconds: " + dSeconds.ToString("###,##0\r\n");
            tb_Results.Text += "Number Work Units: " + nItems + "\r\n";
            tb_Results.Text += "Units per second(system): " + dUnitsPerSecond.ToString("###,##0.0000\r\n");
            tb_Results.Text += "Secs per work unit per devices: " + (nDevices / dUnitsPerSecond).ToString("###,##0\r\n");
            tb_Results.Text += "Secs per work unit this system: " + (1.0 / dUnitsPerSecond).ToString("###,##0\r\n");
            dAvgCreditPerUnit = Convert.ToDouble(tb_AvgCredit.Text);
            tb_Results.Text += "Credits/sec (system): " + (dUnitsPerSecond * dAvgCreditPerUnit).ToString("#,##0.00\r\n");

            tb_Results.Text += "Credits/sec (one device): " + (dUnitsPerSecond * dAvgCreditPerUnit / nDevices).ToString("##0.00\r\n");
        }

        private void PerformStats()
        {
            int i, j, k, n;
            double Avg = 0.0;
            double Std = 0.0;
            double d;
            long l;
            string strOut = "";

            int NumUnits = lb_SelWorkUnits.SelectedItems.Count;
            if (NumUnits != 2)
            {
                tb_Results.Text = "you must select exactly two items\r\n";
                return;
            }
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = 1 + j - i;  // number of items to average
            if(n < 2)return;
            n = 0;
            for(k=i; k <= j; k++)
            {
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data
                }
                l = Convert.ToInt64(d);
                d /= 60.0;
                n++;
                Avg += d;

                strOut += d.ToString("###,##0.00") + "\t" + fmtHMS(l) + "\r\n";
            }
            if (n == 0) return;
            Avg /= n;
            l = Convert.ToInt64(Avg * 60.0);
            Std = 0;
            for (k = i; k <= j; k++)
            {
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue;
                }
                d = d/60.0 - Avg;
                Std += d*d;
            }
            Std = Math.Sqrt(Std / n);
            tb_Results.Text = strOut;
            tb_Results.Text += "Number of selections " + n.ToString("#,##0") + "\r\n";
            tb_Results.Text += "AVG elapsed (minutes) " + Avg.ToString("###,##0.00") + "\t" + fmtHMS(l) +  "\r\n";
            tb_Results.Text += "STD of elapsed time " + Std.ToString("###,##0.00") + "\r\n";
        }

        private void btnFetchHistory_Click(object sender, EventArgs e)
        {

            int iProject, iApp, i;
            cAppName AppName;
            string strProjName, strAppName;

            ClearInfoTables();
            i = cb_SelProj.SelectedIndex;
            if (i < 0)   // invalid selection. restore original project name using "tag"
            {
                tb_Info.Text = "cannot find project: " + cb_SelProj.Text + " \r\n Restoreing";
                if (cb_SelProj.Tag == null) return;
                strProjName = KnownProjApps[(int)cb_SelProj.Tag].ProjName;
                cb_SelProj.Text = strProjName;
                cb_SelProj.Text = strProjName;
                return;
            }
            strProjName = cb_SelProj.Items[i].ToString();
            CurrentProject = strProjName;
            iProject = LookupProject(strProjName);
            Debug.Assert(iProject >= 0);
            iApp = cb_AppNames.SelectedIndex;
            strAppName = cb_AppNames.Items[iApp].ToString();    // contains line count
            i = strAppName.LastIndexOf(" (");
            if (i > 0) strAppName = strAppName.Substring(0, i).TrimEnd();
            if (iProject < 0 || iApp < 0)
            {
                ShowContinunities(false);
                return;
            }
            //iApp = KnownProjApps[iProject].FindApp(strAppName);
            //AppName = KnownProjApps[iProject].KnownApps[iApp];
            AppName = KnownProjApps[iProject].FindApp(strAppName);
            ThisProjectInfo = new List<cProjectInfo>(AppName.nAppEntries);
            lb_SelWorkUnits.Items.Clear();
            for (i = 0; i < AppName.nAppEntries; i++)
            {
                cProjectInfo cpi = new cProjectInfo();
                ThisProjectInfo.Add(cpi);
            }
            FillProjectInfo(AppName);
            CountSelected();
        }



        // the first number shown in the selection box is line number in the history file, not the index to the project info table
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            if (rbElapsed.Checked) PerformStats();
            if (rbThroughput.Checked) PerformThruput();
            if (rbIdle.Checked)
            {
                if(PerformIdleAnalysis())
                    ShowIdleInfo();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lb_SelWorkUnits.SelectedIndices.Clear();
            ShowSelectable(false);
        }

        private void cb_AppNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = cb_AppNames.Text;
            ShowNumberApps();
        }


      //frm2.ShowDialog(); //shows form as a modal dialog
      //frm2.Show();    //shows form as a non modal dialog
      //frm2.Dispose();   
        private void btnShowProjectTree_Click(object sender, EventArgs e)
        {
            InfoForm MyInfo = new InfoForm(this);
            MyInfo.ShowDialog();
        }

        private void RunContinunityCheck()
        {

            int NumUnits = lb_SelWorkUnits.SelectedItems.Count; ;
            int i, j, k, nItems;
            long tStop, tStart, iHoursAgo;
            double a, b, c, MaxDiff = 0.0;
            string strUnits = "";

            iLocMaxDiff = 0;
            lb_LocMax.Text = "";
            lbTimeContinunity.Text = "";

            if (CountSelected() == 0)
            {
                tb_Results.Text = "you must select exactly two items\r\n";
                ShowContinunities(false);
                return;
            }
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            nItems = 1 + j - i;
            for (k = 0; k < nItems - 1; k++)
            {
                // do not discard bad data as it was processed and time consumed
                a = ThisProjectInfo[iSortIndex[i + k]].time_t_Completed;
                b = ThisProjectInfo[iSortIndex[i + k + 1]].time_t_Completed;
                c = b - a;
                if (c > MaxDiff)
                {
                    MaxDiff = c;
                    iLocMaxDiff = k + i;
                }
            }
            MaxDiff /= 60.0;    // to minutes
            lbTimeContinunity.Text = "Most minutes between tasks: " + MaxDiff.ToString("###,##0.00");
            lb_LocMax.Visible = (MaxDiff > 0.0);
            if (lb_LocMax.Visible)
            {
                string strLine = lb_SelWorkUnits.Items[iLocMaxDiff].ToString().TrimStart();
                tStop = ThisProjectInfo[iSortIndex[j]].time_t_Completed;
                tStart = ThisProjectInfo[iSortIndex[iLocMaxDiff]].time_t_Completed;
                tStart -= Convert.ToInt64(ThisProjectInfo[iSortIndex[iLocMaxDiff]].dElapsedTime);
                iHoursAgo = (tStop - tStart) / 3600;
                if(iHoursAgo == 0)
                {
                    iHoursAgo = (tStop - tStart) / 60;
                    strUnits = " (" + iHoursAgo.ToString() + " minutes ago)";

                }
                else
                {
                    strUnits = " (" + iHoursAgo.ToString() + " hours ago)";
                }
                i = strLine.IndexOf(' ');
                lb_LocMax.Text = "Near # " + strLine.Substring(0, i) + strUnits;
            }
            ShowContinunities(true);
            
        }

        private void btnContinunity_Click(object sender, EventArgs e)
        {
            RunContinunityCheck();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MyAbout myAbout = new MyAbout();
            myAbout.ShowDialog();
        }


        private void rbElapsed_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void rbThroughput_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ShowContinunities(bool bShow)
        {
            btnCheckNext.Enabled = bShow;
            btnCheckPrev.Enabled = bShow;
            lb_history_loc.Visible = bShow;
        }

        private void ShowSelectable(bool bShow)
        {
            btnContinunity.Enabled = bShow;
            btn_Filter.Enabled = bShow;;
            lb_NumSel.Visible = bShow;
            lb_LocMax.Visible = bShow ;
            btnPlot.Enabled = bShow;
            btnPlotET.Enabled = bShow;
            btnCheckNext.Enabled = bShow;
            btnCheckPrev.Enabled = bShow;
        }

        private int CountSelected()
        {
            int i, j, n = lb_SelWorkUnits.SelectedIndices.Count;
            string strTimeDiff;
            long tStart, tEnd;

            lb_NumSel.Text = "None Selected";
            bt_all.Enabled = (lb_SelWorkUnits.Items.Count > 0); 
            if (n != 2)
            {
                ShowSelectable(false);
                lbTimeContinunity.Text = "not calculated yet";
                if (n == 0)
                    return 0;
                i = 0;
                j = iSortIndex.Last();
                tStart = ThisProjectInfo[iSortIndex[i]].time_t_Completed - Convert.ToInt64(ThisProjectInfo[iSortIndex[i]].dElapsedTime);
                tEnd = ThisProjectInfo[iSortIndex[j]].time_t_Completed;
                strTimeDiff = BestTimeUnits(tEnd - tStart);
                lbSeriesTime.Text = "total series time: " + strTimeDiff;
                return 0;
            }
            ShowSelectable(true);
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            tStart = ThisProjectInfo[iSortIndex[i]].time_t_Completed - Convert.ToInt64(ThisProjectInfo[iSortIndex[i]].dElapsedTime);
            j = lb_SelWorkUnits.SelectedIndices[1];
            tEnd = ThisProjectInfo[iSortIndex[j]].time_t_Completed;
            strTimeDiff = BestTimeUnits(tEnd - tStart);
            lbSeriesTime.Text = "Selected series time: " + strTimeDiff;
            n = 1 + j - i;
            lb_NumSel.Text = "Selected: " + n.ToString();
            
            return n;
        }


        private void bt_all_Click(object sender, EventArgs e)
        {
            int i = lb_SelWorkUnits.Items.Count;
            if (i == 0) return;
            lb_SelWorkUnits.ClearSelected();
            lb_SelWorkUnits.SetSelected(0, true);
            lb_SelWorkUnits.SetSelected(i-1, true);
            CountSelected();

        }

        private void lb_SelWorkUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountSelected();
        }

        public double CalcStd(double avg, ref List<double>Values)
        {
            double  dd;
            double std=0, rms=0;
            foreach(double d in Values)
            {
                dd = d - avg;
                rms += (dd * dd);
            }
            std = Math.Sqrt(rms / Values.Count);
            return std;
        }

        private bool PerformIdleAnalysis()
        {
            int i, j, n;
            long l;
            double d;

            if (lb_SelWorkUnits.Items.Count < 2) return false;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return false;
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 3) return false;  // need to show two segments at least
            CompletionTimes = new List<long>(n + 1);
            IdleGap = new List<double>(n);
            AvgGap = 0;
            // need to make sure the first one valid
            do
            {
                int k = iSortIndex[i];
                if (ThisProjectInfo[k].bState) break;
                i++;
                if (i >= j) return false;
            }
            while (true);


            for (n = i; n <= j; n++)
            {
                //todo need to evaluate if data was bad ???
                int k = iSortIndex[n];
                if (!ThisProjectInfo[k].bState) continue;
                l = ThisProjectInfo[k].time_t_Completed;
                if (n > i)
                {
                    double dd = l - CompletionTimes.Last();
                    IdleGap.Add(dd);
                    AvgGap += dd;
                }
                CompletionTimes.Add(l);
            }
            AvgGap /= IdleGap.Count;
            StdGap = CalcStd(AvgGap, ref IdleGap);
            return true;
        }

        private void ShowIdleInfo()
        {

            tb_Results.Text = "Number of idle gaps " + IdleGap.Count.ToString() + "\r\n";
            tb_Results.Text += "Average Gap Size " + BestTimeUnits(Convert.ToInt64(AvgGap)) + "\r\n";
            tb_Results.Text += "Standard Deviation of Gap " + BestTimeUnits(Convert.ToInt64(StdGap));
        }

        public class Histogram<TVal> : SortedDictionary<TVal, uint>
        {
            public void IncrementCount(TVal binToIncrement)
            {
                if (ContainsKey(binToIncrement))
                {
                    this[binToIncrement]++;
                }
                else
                {
                    Add(binToIncrement, 1);
                }
            }
        }

        // calculate the distribution of elapsed time
        private bool CalculateETdistribution()
        {
            int i, j, n, h, lPtr;
            long l;
            double d, d1;
            int k; // Seturge's rule for histograms

            if (lb_SelWorkUnits.Items.Count < 2) return false;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return false;
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 3) return false;  // need to show two segments at least    
            IdleGap = new List<double>(n);
            AvgGap = 0;
            for (n = i; n <= j; n++)
            {
                k = iSortIndex[n];
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                IdleGap.Add(d);
                AvgGap += d;
            }
            AvgGap /= IdleGap.Count;
            StdGap = CalcStd(AvgGap, ref IdleGap);

            Histogram<double> hist = new Histogram<double>();
            h = 0;
            foreach(double dd in IdleGap)
            {
                hist.IncrementCount(dd);
            }
            d = 1.0 + 3.22 * Math.Log10(hist.Count);
            d1 = 1.0 + 3.22 * Math.Log10(IdleGap.Count);
            k = hist.Count; // Convert.ToInt32(d);
            CompletionTimes = new List<long>(k);
            for (i = 0; i < k; i++) CompletionTimes.Add(0);
            IdleGap = new List<double>(k);
            for (i = 0; i < k; i++) IdleGap.Add(0);
            d = hist.Count / (k-1);
            d = Math.Ceiling(d);
            j = Convert.ToInt32(d);
            lPtr = 0;
            h = 0;
            /*
                        foreach (KeyValuePair<double, uint> histEntry in hist.AsEnumerable())
                        {
                            IdleGap[lPtr]+= histEntry.Key;
                            CompletionTimes[lPtr] += histEntry.Value;
                            h++;
                            if(h == j)
                            {
                                IdleGap[lPtr] /= h;
                                CompletionTimes[lPtr] /= h;
                                lPtr++;
                                h = 0;
                            }
                        }

            */

            foreach (KeyValuePair<double, uint> histEntry in hist.AsEnumerable())
            {
                IdleGap[lPtr] = histEntry.Value;
                CompletionTimes[lPtr] = Convert.ToInt64( histEntry.Key);
                lPtr++;
            }

            return true;
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            if(PerformIdleAnalysis())
            {
                TPchart DrawThruput = new TPchart(ref CompletionTimes, ref IdleGap, AvgGap, StdGap,CurrentProject);
                DrawThruput.ShowDialog();
                DrawThruput.Dispose();
            }
        }

        private void btnPlotET_Click(object sender, EventArgs e) // this is the histogram
        {
            if (CalculateETdistribution())
            {
                TPchart DrawThruput = new TPchart(ref CompletionTimes, ref IdleGap, -1, -1, CurrentProject);
                DrawThruput.ShowDialog();
                DrawThruput.Dispose();
            }
        }

        private void btnCheckNext_Click(object sender, EventArgs e)
        {
            int i, j, n;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return;
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 2) return;
            n = iLocMaxDiff + 1;
            if ((j - n) < 2) return;
            lb_SelWorkUnits.SetSelected(i, false);
            lb_SelWorkUnits.SetSelected(n, true);
            CountSelected();
            RunContinunityCheck();
        }

        private void btnCheckPrev_Click(object sender, EventArgs e)
        {
            int i, j, n;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return;
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 2) return;
            n = iLocMaxDiff - 1;
            if (n < 0) n = 0;
            lb_SelWorkUnits.SetSelected(j, false);
            lb_SelWorkUnits.SetSelected(n, true);
            CountSelected();
            RunContinunityCheck();
        }
    }
}
