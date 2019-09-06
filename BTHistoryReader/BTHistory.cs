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
        public int MaxDeviceCount;
        int iStart = -1;
        int iStop = -1;
        public int AnalysisType;
        public string ThisSystem = "";
        public cSplitHistoryValues OneSplitLine;    // use this for processing each line in history file;

        private List<double> CompletionTimes;       // better to use minutes instead of seconds so change from long
        private List<double> IdleGap;
        private List<int> GrpList;
        private double AvgGap;
        private double StdGap;
        //private bool bDoNotLoadA = true; // this is a kluge until I figure out why I cannot remove those events!!!
        //private bool bDoNotLoadP = true;

        public BTHistory()
        {
            InitializeComponent();
            try
            {
                // DEFALT IS TO USE CVS1  note that the 3 files slowly change in real time as they are updated
                bool b = Properties.Settings.Default.TypeCVS;
                if (!b) rbUseCVS.Checked = true;
                else rbUseCVS1.Checked = true;
                cboxStopLoad.Checked = Properties.Settings.Default.UseLimit;
                tboxLimit.Text = Properties.Settings.Default.RecLimit;
            }
            catch
            {
                try
                {
                    // something may be happening here, just guessing
                    Properties.Settings.Default.TypeCVS = true;
                    Properties.Settings.Default.RecLimit = "40000";
                    Properties.Settings.Default.UseLimit = true;
                }
                catch (Exception e)
                {
                    tb_Info.Text = "exception: " + e.Message + "\r\n";
                }
            }

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
        static int[] SortToInfo; // index created by sort so that the intems int he select box eacn easily located the actual items in
                                      // the info table

        static int LastKnownProject = 0;
        public int NumberBadWorkUnits;
        static int ExpectedLengthLine = 100;

        const int LKUP_NOT_FOUND = -1;      // cannot find project- forgot it or new one
        const int LKUP_TOBE_IGNORED = -2;   // do not use this project
        const int LKUP_INVALID_LINE = -3;   // line in history is invalid 


        public string CurrentSystem;    // computer name
        public string CurrentProject;   // project being looked at
        cDataName CurrentDataset;
        public List<cProjectInfo> ThisProjectInfo;

        public int LinesToReadThenIncrement = 0;   // after reading this many, increment the progress bar
        public int LinesWeRead = 0;

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

        // this also sets the filter in the file open dialog box
        private string GetHistoryExtension()
        {
            string strExt = rbUseCVS1.Checked ? "*.cvs1" : "*.cvs";
            string strTest = "RecentHistory(" + strExt + ")|" + strExt + "|OldHistory(_long_)|*_long_*" + strExt;
            ofd_history.Filter = strTest;
            return strExt;
        }

        // lookup application in the knonwn project app table where loc is index to selected project
        // if app not there return an error else return index to app
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

        // look up project and return index into the known project app table
        // must be in table else return error as not building table at this time
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

        // lookup project in table ignore certain projects. we are building the table 
        // at this sequence in the program
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

        // use appropriate units based on the number of seconds
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



        // put some projects into the table but unknown (to this program) also get added
        // there is no limit on projects nor apps.  Usefull to initialize so as to
        // see if there are new or really old apps being run.
        private void InitLookupTable()
        {
            cKnownProjApps kpa;
            KnownProjApps = new List<cKnownProjApps>();
            OneSplitLine = new cSplitHistoryValues();
            kpa = new cKnownProjApps();
            kpa.AddName("Milkyway@Home");
            kpa.AddApp("Milkyway@home Separation", "opencl_ati_101");
            kpa.AddApp("Milkyway@home Separation", "opencl_nvidia_101");
            kpa.AddApp("Milkyway@home", "opencl_ati_101");
            kpa.AddApp("Milkyway@home Separation", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("SETI@home");
            kpa.AddApp("SETI@home v8", "opencl_ati5_SoG_nocal");
            kpa.AddApp("SETI@home v8", "opencl_nvidia_SoG");
            kpa.AddApp("SETI@home v8", "opencl_atiapu_sah");
            kpa.AddApp("SETI@home v8", "opencl_ati5_nocal");
            kpa.AddApp("SETI@home v8", "opencl_ati_nocal");
            kpa.AddApp("SETI@home v8", "");
            kpa.AddApp("AstroPulse v7", "opencl_ati_100");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("collatz");
            kpa.AddApp("Collatz Sieve", "");
            kpa.AddApp("Collatz Sieve", "opencl_nvidia");
            KnownProjApps.Add(kpa);


            kpa = new cKnownProjApps();
            kpa.AddName("Bitcoin Utopia");
            kpa.AddApp("cgminer (Campaign #7) (for 1.6+GH/s ASICs)", "");
            kpa.AddApp("cgminer (Campaign #7) (for 1.6+GH/s ASICs)", "miner_asic");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Collatz Conjecture");
            kpa.AddApp("Collatz Sieve", "");
            kpa.AddApp("Collatz Sieve", "opencl_intel_gpu");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Amicable Numbers");
            kpa.AddApp("Amicable Numbers up to 10^20", "opencl_amd");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("LHC@home");
            kpa.AddApp("SixTrack", "");
            kpa.AddApp("SixTrack", "avx");
            kpa.AddApp("SixTrack", "sse2");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("World Community Grid");
            kpa.AddApp("Mapping Cancer Markers", "");
            kpa.AddApp("FightAIDS@Home - Phase 1", "");
            kpa.AddApp("FightAIDS@Home - Phase 2", "");
            kpa.AddApp("OpenZika", "");
            kpa.AddApp("Microbiome Immunity Project", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("GPUGRID");
            kpa.AddApp("Short runs (2-3 hours on fastest card)", "cuda80");
            kpa.AddApp("Long runs (8-12 hours on fastest card)", "cuda80");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NFS@Home");
            kpa.AddApp("15e Lattice Sieve", "");
            kpa.AddApp("15e Lattice Sieve", "notphenomiix6");
            kpa.AddApp("16e Lattice Sieve V5", "");
            kpa.AddApp("14e Lattice Sieve", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Cosmology@Home");
            kpa.AddApp("camb_legacy", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Rosetta@home");
            kpa.AddApp("Rosetta", "");
            kpa.AddApp("Rosetta Mini", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("TN-Grid Platform");
            kpa.AddApp("gene@home PC-IM", "sse2");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NumberFields@home");
            kpa.AddApp("Get Decic Fields", "default");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Enigma@Home");
            kpa.AddApp("Enigma GPU", "");
            kpa.AddApp("Enigma GPU", "cuda_fermi");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("PrimeGrid");
            kpa.AddApp("PPS (Sieve)", "cudaPPSsieve");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Einstein@Home");
            kpa.AddApp("Gravitational Wave Engineering run on LIGO O1 Open Data", "");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-ati");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-nvidia");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl-nvidia");
            KnownProjApps.Add(kpa);


            // this project ignored
            kpa = new cKnownProjApps();
            kpa.AddName("WUProp@Home");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("latinsquares");
            kpa.AddApp("odlk3@home", "");
            kpa.AddApp("odlkmax@home", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("yoyo@home");
            kpa.AddApp("Siever", "");
            kpa.AddApp("Cruncher ogr", "");
            kpa.AddApp("ecm", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Asteroids@home");
            kpa.AddApp("Period Search Application", "cuda55");
            kpa.AddApp("Period Search Application", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Moo! Wrapper");
            kpa.AddApp("Distributed.net Client", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Gerasim@home");
            kpa.AddApp("spstarter", "");
            kpa.AddApp("Test separator", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Universe@Home");
            kpa.AddApp("Universe BHspin v2", "");
            KnownProjApps.Add(kpa);


            //lb_NumKnown.Text = "Known Projects: " + KnownProjApps.Count.ToString();
            LastKnownProject = KnownProjApps.Count;
        }

        // remove old stuff
        public void ClearInfoTables()
        {
            tb_AvgCredit.Text = "0";
            //tb_Info.Text = "";
            tb_Results.Text = "";
        }

        // more old stuff to remove
        private void ClearForNewProject()
        {
            lb_SelWorkUnits.Items.Clear();
            ClearInfoTables();
        }

        // ditto onold stuff
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

        // show how many apps are associated with the project
        private void ShowNumberApps()
        {
            lb_nApps.Visible = cb_AppNames.Items.Count > 0;
            if (lb_nApps.Visible)
            {
                lb_nApps.Text = cb_AppNames.Items.Count.ToString();
            }
        }

        // looking for the history files.  "long" are the files that are "saved" when that option in BoincTasks is enabled.
        private int LocatePathToHistory()
        {
            string str_WantedDirectory = "\\EFmer\\BoincTasks\\history";
            string str_LookHere = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int nFiles = 0;

            ofd_history.DefaultExt = GetHistoryExtension();
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
            foreach (string sFile in Directory.GetFiles(str_PathToHistory, GetHistoryExtension()))
                nFiles++;

            ofd_history.InitialDirectory = str_PathToHistory;
            return nFiles;
        }

        // launch the program that compares multiple files
        private void PerformSelectCompare()
        {
            CompareHistories MyHistories = new CompareHistories(this);
            MyHistories.ShowDialog();
            ClearPreviousHistory();
        }

        public int GetPBARmax()
        {
            return pbarLoading.Maximum;
        }
        public void SetPBARcnt(int n)
        {
            LinesToReadThenIncrement = n;
        }

        public void IncrementPBAR()
        {
            pbarLoading.PerformStep();
            pbarLoading.Update();
            pbarLoading.Refresh();
            Application.DoEvents();
        }

        // following stops unwanted side effects if user clicks on certain widgits while the progress bar is moving
        private void CanChangeProjApp(bool b)
        {
            cb_AppNames.Enabled = b;
            cb_SelProj.Enabled = b;
            btnScatSets.Enabled = b;
            btn_OpenHistory.Enabled = b;
            gboxOPFsettings.Enabled = b;
            lb_SelWorkUnits.Enabled = b;
        }

        // user clicked open files,this program does the reading of single files or hands it off if multiple
        private bool FetchHistory()
        {
            int nFiles = LocatePathToHistory();
            bool bAny = nFiles > 0;

            lb_nProj.Visible = false;
            lb_nApps.Visible = false;
            bt_all.Enabled = false;

            if (DialogResult.OK != ofd_history.ShowDialog())
                return false;
            AllHistories = ofd_history.FileNames;
            if (AllHistories.Length > 1)
            {
                pbarLoading.Visible = true;
                BTHistory.ActiveForm.Enabled = false;
                LinesWeRead = 0;                    // used by progress bar
                PerformSelectCompare();
                pbarLoading.Visible = false;
                BTHistory.ActiveForm.Enabled = true;
                pbarLoading.Value = 0;
                btnScatSets.Enabled = false;
                return false;
            }
            lb_history_loc.Text = ofd_history.FileName;
            if (File.Exists(lb_history_loc.Text))
            {
                str_PathToHistory = lb_history_loc.Text;
                if (ValidateHistory(str_PathToHistory, ref ThisSystem) >= 0)
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
            tb_Info.Text = "";
            InitLookupTable();
            DisallowCallbacks(true);
            LinesToReadThenIncrement = 0;
            btnGTime.Enabled = false;
            btnScatGpu.Enabled = false;
            if (FetchHistory())
            {
                DisallowCallbacks(false);
                cb_AppNames_SelectedIndexChanged(null, null);
                return;
            }
            ClearPreviousHistory();
            ShowContinunities(false);
            ShowSelectable(false);

        }

        // see if we have a real history file and not some junk file accidently opened.
        public int ValidateHistory(string strFile, ref string SysName)
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
                SysName = LinesHistory[1];
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
            int i = 0, n = 0;
            bool bAny = false;

            cb_AppNames.Items.Clear();
            foreach (cAppName appName in KnownProjApps[iLoc].KnownApps)
            {
                n = appName.nAppEntries;
                if (n > 0)
                {
                    bAny = true;
                    cb_AppNames.Items.Add(appName.Name + "  (" + n.ToString() + ")");
                }
                i++;
            }
            cb_AppNames.Text = cb_AppNames.Items[0].ToString();
            cb_AppNames.Tag = i;    // use tag to restore any edits to the combo box as I cant make it readonly
            btnScatSets.Enabled = bAny;
        }

        // for all applications over all projects, compute the avg and std of elapsed time
        public void PerformCalcAverages()
        {
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                foreach (cAppName AppName in kpa.KnownApps)
                {
                    AppName.DoAverages();
                }
            }
        }

        // get list of projects and their apps
        // save all information in the KnownProjApp table
        public int ProcessHistoryFile()
        {
            bool bAnyData = false, bTemp;
            int iLine = -4;  // if > 4 then 
            int RtnCode = 0;
            int eInvalid = 0;   // invalid line in history (not complete or whatever)
            cAppName AppName;
            cKnownProjApps kpa;
            int iLocDevice, jloc, iGrp;   // which dataset group the record is in (dataset is the "name" of the data

            bool bInformOnlyOnce = true;
            // find and identify any project in the file
            foreach (string s in LinesHistory)
            {
                iLine++;
                LinesWeRead++;
                if (LinesWeRead > LinesToReadThenIncrement & pbarLoading.Visible)
                {
                    LinesWeRead = 0;
                    IncrementPBAR();
                }
                if (iLine < 1) continue;    // skip past header
                // possible sanity check here: iLine is 1 and first token of "s" is also 1
                eInvalid = OneSplitLine.StoreLineOfHistory(s, ExpectedLengthLine);
                if (eInvalid != 0)
                {
                    if (eInvalid == (int)eHistoryError.EndHistory)
                        continue;
                    //tb_Info.Text += "PHF: bad line " + iLine.ToString() + "\r\n";
                    //continue;    // do not put 0 elapsed time into our results
                }
                RtnCode = LookupProj(OneSplitLine.Project);
                if (RtnCode < 0)
                {
                    if (RtnCode == LKUP_NOT_FOUND)
                    {
                        // if line has 763	Updating...	Updating, please wait	--- then rpc call did not complete good
                        if (OneSplitLine.Project.Contains("pdating..."))
                        {
                            tb_Info.Text += "bad line:" + s.Substring(0, 38) + "\r\n";
                            continue;
                        }
                        tb_Info.Text += "Cannot find project: " + OneSplitLine.Project + " adding to database\r\n";
                        kpa = new cKnownProjApps();
                        kpa.AddUnkProj(OneSplitLine.Project);
                        KnownProjApps.Add(kpa);
                        RtnCode = KnownProjApps.Count - 1;  // put unknown project here
                    }
                    else continue;
                }
                bAnyData = true;
                // if the app is found then point to the line containing the app's info
                // and put all info also 
                // jys adding plan class info
                AppName = KnownProjApps[RtnCode].SymbolInsert(OneSplitLine.Application + " [" + OneSplitLine.PlanClass + "]", 3 + iLine);  // first real data is in 5th line (0..4)
                AppName.AddUse(OneSplitLine.use);   
                iGrp = AppName.DataName.NameInsert(OneSplitLine.Name, OneSplitLine.Project);
                iLocDevice = OneSplitLine.use.IndexOf("device "); //1234567  note that sometimes the device is missing, if so, then use 0 as
                    // possible there was only 1 device and no number was assigned
                if (iLocDevice > 0)
                {
                    jloc = OneSplitLine.use.LastIndexOf(")");
                    iLocDevice += 7;
                    jloc -= iLocDevice;
                    //string strDebug = OneSplitLine.use.Substring(iLocDevice, jloc);
                    OneSplitLine.iDeviceUsed = Convert.ToInt32(OneSplitLine.use.Substring(iLocDevice, jloc));
                }
                else OneSplitLine.iDeviceUsed = 0;  // device is not shown if only one gpu so use 0   
                // need to backfit gpu id to 
                AppName.AddETinfo(OneSplitLine.dElapsedTimeCpu, iGrp, OneSplitLine.iDeviceUsed);
                // the above iGrp needs to go into ThisProjectInfo which unfortunately does not exist here
                // and I do not want to rewrite this code at this time.
                // going to append that value to the current history line
                // the -1 on below subscripting was a big mistake
                // note:  the WTFb was added for debugging as the device number was a problem and is not needed any more
                LinesHistory[iLine + 3] += "WTFaTODO_" + iGrp.ToString() + "|"; // + "WTFbTODO_" + OneSplitLine.iDeviceUsed.ToString();
                //AppName.bIsValid.Add(OneSplitLine.State != 3);        //(eInvalid == (int)eHistoryError.SeemsOK);
                // State cannot be used as just not 3
                bTemp = (OneSplitLine.ExitStatus == 0);
                //if (OneSplitLine.State == 6) bTemp = false;
                if (OneSplitLine.dElapsedTimeGpu == 0 && OneSplitLine.dElapsedTimeCpu == 0) bTemp = false;// exception is bitcoin utopia
                if ((AppName.nUsesGPU>=0) && OneSplitLine.dElapsedTimeGpu == 0) bTemp = false;
                AppName.bIsValid.Add(bTemp);
                if (AppName.bIsUnknown & bInformOnlyOnce)
                {
                    tb_Info.Text += "Unk App " + AppName.GetInfo + " added to database\r\n";
                    bInformOnlyOnce = false;    // if project is unknown all apps are unknown
                }
            }

            if (bAnyData)
                PerformCalcAverages();
            else
                tb_Info.Text += "Project has no data or all data is illegal\r\n";
            return 0;
        }




        // fill in the project selection combo box
        // use semaphore to signal not to unnecessary fill in listbox as I cant seem to remove the event handler
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

        // if true stop callbacks from happening
        // if false then OK to fall through and trigger other events
        private void DisallowCallbacks(bool bValue)
        {
            if (bValue)
            {
                this.cb_SelProj.SelectedIndexChanged -= new System.EventHandler(this.cb_SelProj_SelectedIndexChanged);
                this.cb_AppNames.SelectedIndexChanged -= new System.EventHandler(this.cb_AppNames_SelectedIndexChanged);
            }
            else
            {
                this.cb_SelProj.SelectedIndexChanged += new System.EventHandler(this.cb_SelProj_SelectedIndexChanged);
                this.cb_AppNames.SelectedIndexChanged += new System.EventHandler(this.cb_AppNames_SelectedIndexChanged);
            }
        }

        private void DisallowAppCallbacks(bool bValue)
        {
            if (bValue)
            {
                this.cb_AppNames.SelectedIndexChanged -= new System.EventHandler(this.cb_AppNames_SelectedIndexChanged);
            }
            else
            {
                this.cb_AppNames.SelectedIndexChanged += new System.EventHandler(this.cb_AppNames_SelectedIndexChanged);
            }
        }


        // if project changed then fill in the associated app box
        private void cb_SelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cb_SelProj.SelectedIndex;
            DisallowAppCallbacks(true);
            ClearForNewProject();
            cb_SelProj.Text = cb_SelProj.Items[i].ToString();
            i = LookupProject(cb_SelProj.Text);
            FillAppBox(i);
            DisallowAppCallbacks(false);
            DisplayHistory();
            ShowNumberApps();
        }

        // standard bubble sort with exchange on index
        // could have used yourList.sort() but progress bar was needed
        // due to size of array
        // 8-1-2019 when truncating, use only newest data
        public void SortTimeIncreasing(int nSort)
        {
            iSortIndex = new int[nSort];
            SortToInfo = new int[nSort];

            int i, j, k;
            int j1, j2;
            string sTemp;
            int n = Convert.ToInt32(tboxLimit.Text);
            CanChangeProjApp(false);    // cannot allow changes while sorting due to side effects
            tb_Info.Text += "sorting " + nSort.ToString() + " items please wait......\r\n";
            if (nSort > n / 2)
            {
                pbarLoading.Visible = true;
                SetPBARcnt(nSort / GetPBARmax());
                lb_SelWorkUnits.UseWaitCursor = true;
            }
            k = nSort - 1;
            tb_Info.Refresh();
            for (i = 0; i < nSort; i++)
                iSortIndex[i] = i;
            for (i = 0; i < k; i++)
            {
                LinesWeRead++;
                if (LinesWeRead > LinesToReadThenIncrement & pbarLoading.Visible)
                {
                    LinesWeRead = 0;
                    IncrementPBAR();
                }
                for (j = 0; j < k; j++)
                {
                    j1 = iSortIndex[j];
                    j2 = iSortIndex[j + 1];
                    if (ThisProjectInfo[j1].time_t_Completed > ThisProjectInfo[j2].time_t_Completed)
                    {
                        iSortIndex[j] = j2;
                        iSortIndex[j + 1] = j1;
                    }
                }
            }
            if (pbarLoading.Visible)
            {
                pbarLoading.Value = 0;
                LinesWeRead = 0;
                tb_Info.Text += "Displaying those items wait moment longer...\r\n";
            }
            n = 0;
            for (i = 0; i < nSort; i++)
            {
                LinesWeRead++;
                if (LinesWeRead > LinesToReadThenIncrement & pbarLoading.Visible)
                {
                    LinesWeRead = 0;
                    IncrementPBAR();
                }
                j = iSortIndex[i];
                if (!ThisProjectInfo[j].bState) continue;
                sTemp = ThisProjectInfo[j].strOutput;
                SortToInfo[n] = j;    //Convert.ToInt32(sTemp.Substring(0, iPadSize));
                n++; 
                lb_SelWorkUnits.Items.Add(sTemp);
            }
            pbarLoading.Visible = false;
            lb_SelWorkUnits.UseWaitCursor = false;
            CanChangeProjApp(true);
        }

        // put hours minuts secs in a nice concise format
        static string fmtHMS(long seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }

        // this fills in the "ThisProjectInfo" structure with stuff from each single line in the history files of "the app"
        public int FillProjectInfo(cAppName AppName)
        {
            string[] strSymbols;
            string sTemp;
            System.DateTime dt_1970 = new System.DateTime(1970, 1, 1);
            System.DateTime dt_this;
            string strWTF = "";
            int j = 0;
            int iWTF = 0, jWTF ;
            bool bState, bState1;
            long n, nElapsedTime;
            bool bStopReading = cboxStopLoad.Checked;
            int nLimit = Convert.ToInt32(tboxLimit.Text);
            int i, iStart, iTraverse, iCount;
            pbarLoading.Value = 0;
            if (AppName.LineLoc.Count == 0) return 0;

            // this could be rewritten better but WTF, it was done before I got that splitlinestuff to work
            // 8-1-2019 if truncating then use only last part of this table      
            //foreach (int i in AppName.LineLoc) 

            iCount = AppName.LineLoc.Count;
            iStart = 0;
            if(bStopReading)
            {
                if(iCount > nLimit)
                {
                    iStart = iCount - nLimit;
                    iCount = nLimit;
                    tb_Info.Text += " skipping to get last " + tboxLimit.Text + " out of " + AppName.LineLoc.Count.ToString() + " records\r\n";
                }
            }
            for(iTraverse=0; iTraverse < iCount;  iTraverse++)
            {
                i = AppName.LineLoc[iTraverse + iStart];
                bState = LinesHistory[i].Length > ExpectedLengthLine;
                if (!bState)
                {
                    break;
                }
                //if (bStopReading && j >= nLimit)
                //{
                //    tb_Info.Text += " stopping after reading " + tboxLimit.Text + " out of " + AppName.LineLoc.Count.ToString() + " records\r\n";
                //    break;
                //}
                iWTF = LinesHistory[i].IndexOf("WTFaTODO_");
                if (iWTF > 0)
                {
                    jWTF = LinesHistory[i].IndexOf("|");
                    Debug.Assert(jWTF > 0);
                    iWTF += 9;
                    strWTF = LinesHistory[i].Substring(iWTF,(jWTF-iWTF));
                    ThisProjectInfo[j].DatasetGroup = Convert.ToInt32(strWTF);
                }
                iWTF = LinesHistory[i].IndexOf("device ");
                if (iWTF > 0)
                {
                    iWTF += 7;
                    jWTF = LinesHistory[i].Substring(iWTF).IndexOf(")");
                    Debug.Assert(jWTF > 0);;
                    strWTF = LinesHistory[i].Substring(iWTF,(jWTF));
                    ThisProjectInfo[j].iDeviceUsed = Convert.ToInt32(strWTF);
                }
                else
                    ThisProjectInfo[j].iDeviceUsed = 0; // device 0 or cpu

                /*  this was used for debugging
                iWTF = LinesHistory[i].IndexOf("WTFbTODO_");
                if (iWTF > 0)
                {
                    jWTF = LinesHistory[i].LastIndexOf("|");
                    Debug.Assert(jWTF > 0);
                    iWTF += 9;
                    strWTF = LinesHistory[i].Substring(iWTF);
                    ThisProjectInfo[j].iDeviceUsed = Convert.ToInt32(strWTF);
                }
                */
                strSymbols = LinesHistory[i].Split('\t');
                ThisProjectInfo[j].strLineNum = strSymbols[(int)eHindex.Run];
                //RunNumber = Convert.ToInt32(ThisProjectInfo[j].strLineNum);
                sTemp = strSymbols[(int)eHindex.CompletedTime];                             // this is completed time in seconds based on 1970    
                n = Convert.ToInt64(sTemp);                         // want to convert to time stamp
                ThisProjectInfo[j].time_t_Completed = n;
                if (n <= 0)   // is 0 if not calculated yet
                {
                    tb_Info.Text += "No completion time " + AppName.GetInfo + " at line " + i.ToString() + "\r\n";
                    // may not have finished yet ???
                    continue;
                }
                dt_this = DateTime.SpecifyKind(dt_1970.AddSeconds(n), DateTimeKind.Utc);
                sTemp = fmtLineNumber(strSymbols[(int)eHindex.Run]) + dt_this.ToLocalTime().ToString();
                ThisProjectInfo[j].strCompletedTime = sTemp;        // save in readable format
                nElapsedTime = Convert.ToInt64(strSymbols[(int)eHindex.ElapsedTimeCpu].ToString()); // this is actually elapsed time
                // the below is actually CPU time as it appears headers in history are reversed for these two items
                ThisProjectInfo[j].dElapsedCPU = Convert.ToDouble(strSymbols[(int)eHindex.ElapsedTimeGpu].ToString());
                ThisProjectInfo[j].dElapsedTime = nElapsedTime;

                // try to find which history entries are bad and mark them out of statistical calculations for "avg" 
                // they still count in thruput

                // wrong test if (strSymbols[(int)eHindex.State].ToString() == "3") bState = false;
                // wrong test if (strSymbols[(int)eHindex.State].ToString() == "6") bState = false;
                bState = strSymbols[(int)eHindex.State].ToString() != "0";
                // problem:  bitcoin utopia has 0 cpu time but we want to show it
                // if there is gpu time then set cpu time to 1 second
                if (nElapsedTime == 0) bState = false;  // possibly incomplete written out data has 0 but state still good?
                else
                {
                    if (ThisProjectInfo[j].dElapsedCPU == 0.0)
                        ThisProjectInfo[j].dElapsedCPU = 1;
                }
                ThisProjectInfo[j].bState = bState;
                if (!bState)
                {
                    AppName.NumberBadWorkUnits++;
                    tb_Info.Text += "Bad exit status line " + ThisProjectInfo[j].strLineNum + "\r\n";
                    //                    continue;   // 7-july-2019 ignore bad data: had not effect as not used in calculationn anyway and
                    // if plotted might be useful as one can click on the bad point and possibly find the dataset name
                }
                n -= nElapsedTime;                                  // get the correct start time as best as we can
                ThisProjectInfo[j].time_t_Started = n;              // needed to calculate throughput
                ThisProjectInfo[j].strElapsedTimeCpu = fmtHMS(nElapsedTime);
                ThisProjectInfo[j].strElapsedTimeGpu = fmtHMS(Convert.ToInt64(strSymbols[(int)eHindex.ElapsedTimeGpu].ToString()));
                sTemp += " " + ThisProjectInfo[j].strElapsedTimeCpu;
                //    "(" + ThisProjectInfo[j].strElapsedTimeGpu + ")";
                ThisProjectInfo[j].strOutput = sTemp + " D" + ThisProjectInfo[j].iDeviceUsed.ToString();               // eventually put into our text box to allow selections
                j++;
            }
            SortTimeIncreasing(j);
            return j;
        }


        // calculates amount of credit the system can do
        private void PerformThruput()
        {

            long t_start, t_stop, t_diff;
            int i, j, k;
            int i1, i2; // used to access iSort..
            double dSeconds = 0;
            int nItems, nDevices;
            double dUnitsPerSecond;
            double ExpectedAvgCredit; // per 24 hous=rs
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

            t_start = ThisProjectInfo[SortToInfo[i]].time_t_Started;
            t_stop  = ThisProjectInfo[SortToInfo[j]].time_t_Completed;

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
            tb_Results.Text += "Credits/sec (one device): " + (dUnitsPerSecond * dAvgCreditPerUnit / nDevices).ToString("##0.00\r\n");
            tb_Results.Text += "Credits/sec (system): " + (dUnitsPerSecond * dAvgCreditPerUnit).ToString("#,##0.00\r\n");
            tb_Results.Text += "System Daily Avg: " + (86400.0 * dUnitsPerSecond * dAvgCreditPerUnit).ToString("#,###,##0.0\r\n");

        }

        // using the selected items, take an average and the std and display
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
            j = lb_SelWorkUnits.SelectedIndices[1]; // 8-9-2019 but these must be used to locate the actual valuea
            n = 1 + j - i;  // number of items to average
            if (n < 2)
            {
                tb_Results.Text += "Need at least 2 items\r\n";
                return;
            }
            n = 0;
            for (int k1 = i; k1 <= j; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState)
                {
                    continue;
                }
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data was finally fixed.  Problem was bitcoin utopia had 0 for cpu
                }                                                        // so putting in "0.1" for cpu but not if gpu is 0 also
                l = Convert.ToInt64(d);
                d /= 60.0;
                n++;
                Avg += d;

                strOut += d.ToString("###,##0.00") + "\t" + fmtHMS(l) + " D" +ThisProjectInfo[k].iDeviceUsed.ToString() + "\r\n";
            }
            if (n == 0) return;
            Avg /= n;
            l = Convert.ToInt64(Avg * 60.0);
            Std = 0;
            for (int k1 = i; k1 <= j; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);        // 
                    continue;
                }
                d = d / 60.0 - Avg;
                Std += d * d;
            }
            Std = Math.Sqrt(Std / n);
            tb_Results.Text = strOut;
            tb_Results.Text += "Number of selections " + n.ToString("#,##0") + "\r\n";
            tb_Results.Text += "AVG elapsed (minutes) " + Avg.ToString("###,##0.00") + "\t" + fmtHMS(l) + "\r\n";
            tb_Results.Text += "STD of elapsed time " + Std.ToString("###,##0.00") + "\r\n";
        }


        // no need to read history using a button, it is fetched when the app is selected.
        private void btnFetchHistory_Click(object sender, EventArgs e)
        {

        }


        // once the project and app are selected then this displays the data
        public void DisplayHistory()
        {

            int iProject, iApp, i;
            cAppName AppName;
            string strProjName, strAppName;

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
            CurrentProject = strProjName;   //replaced by CurrentDataset for use in chart
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
            AppName = KnownProjApps[iProject].FindApp(strAppName);
            CurrentDataset = AppName.DataName;
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


        private string CalcOneStat(int iDev, int iStart, int iStop)
        {
            string strResult = "GPU" + iDev.ToString() + " WUs:";
            double d;
            double Avg = 0.0;
            double Std = 0.0;
            long l;
            int k, n = 0;
            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState || iDev != ThisProjectInfo[k].iDeviceUsed) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data was finally fixed.  Problem was bitcoin utopia had 0 for cpu
                }                                                        // so putting in "0.1" for cpu but not if gpu is 0 also
                l = Convert.ToInt64(d);
                d /= 60.0;
                n++;
                Avg += d;
            }
            if(n == 0)
            {
                return strResult + " has no valid data\r\n";
            }
            Avg /= n;
            l = Convert.ToInt64(Avg * 60.0);
            Std = 0;
            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState || iDev != ThisProjectInfo[k].iDeviceUsed) continue;
                d = ThisProjectInfo[k].dElapsedTime;
                if (d == 0.0)
                {
                    Debug.Assert(false);        // 
                    continue;
                }
                d = d / 60.0 - Avg;
                Std += d * d;
            }
            Std = Math.Sqrt(Std / n);

            return strResult + n.ToString("##,##0") + " -Stats- Avg:" + Avg.ToString("###,##0.0") + "(" + Std.ToString("#,##0.00") + ")\r\n";
        }

        private string CalcOnecredit(int iDev, int iStart, int iStop, ref double val)
        {
            double d = Convert.ToDouble(tb_AvgCredit.Text);
            string strResult = "GPU" + iDev.ToString() + " ";
            int n=0, k;
            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState || iDev != ThisProjectInfo[k].iDeviceUsed) continue;
                n++;
            }
            if (n == 0)
            {
                return strResult + " has no valid data\r\n";
            }
            val = n * d / TimeIntervalMinutes;
            return strResult + "(" + val.ToString("#,##0.00)");
        }


        private string CalcGPUstats(int nDevices,int iStart, int iStop)
        {
            string strResults = "There are " + nDevices.ToString() + " GPUs, units are minutes\r\n";
            strResults += "Dev#   WU count  Avg and Std of avg\r\n";
            for (int i = 0; i < nDevices;i++)
            {
                strResults += CalcOneStat(i, iStart, iStop);
            }
            return strResults;
        }

        private string CalcGPUcredits(int nDevices, int iStart, int iStop)
        {
            //1234567890123456789012345678901234567890123456
            //         1         2         3         4
            // |++++++++++++++++++++++++++++++++++++++++++++
            // 46 chars in output area
            int[] CostSizes = new int[nDevices];
            int n = -1;
            double d = 0, AvgAll = 0;
            double e = -1;
            int r; // the remainder of line
            
            for (int i = 0; i < nDevices; i++)
            {
                CostSizes[i] = CalcOnecredit(i, iStart, iStop, ref d).Length;
                n = Math.Max(n, CostSizes[i]);
                if (e < d) e = d;
            }
            r = 45 - n; // need this many + signs for the maximum 

            for(int i = 0; i < nDevices;i++)
            {
                CostSizes[i] = n - CostSizes[i];
                // above is how many spaces to pad the text
            }
            string strResults = "Using " + tb_AvgCredit.Text + " for average credit\r\nCredit per minute for " + nDevices.ToString() + " GPUs is\r\n";
            for (int i = 0; i < nDevices; i++)
            {
                string strTemp = CalcOnecredit(i, iStart, iStop, ref d) + "        ".Substring(0, CostSizes[i]);
                AvgAll += d;
                double x = r * d / e;
                int j = Convert.ToInt32(x) ;
                strResults += strTemp + " |++++++++++++++++++++++++++++++++++++++++++++".Substring(0,j) + "\r\n";
            }
            return strResults + "System averages " + AvgAll.ToString("#,##0.0") + " credits per minute\r\n";
        }

        private bool FilterUsingGPUs(ref int DeviceMax, ref int iStart, ref int iStop)
        {
            // get start, stop and number of devices
            DeviceMax = -1;
            int i, j, k, n;
            long l;
            int NumUnits = lb_SelWorkUnits.SelectedItems.Count;
            if (NumUnits != 2)
            {
                tb_Results.Text = "you must select exactly two items\r\n";
                return false;
            }
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = 1 + j - i;  // number of items to average
            if (n < 2)
            {
                tb_Results.Text += "Need at least 2 items\r\n";
                return false;
            }
            n = 0;
            iStart = i;
            iStop = j;
            for (int k1 = i; k1 <= j; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState) continue;
                DeviceMax = Math.Max(DeviceMax, ThisProjectInfo[k].iDeviceUsed);
            }
            if(DeviceMax == -1)
            {
                    tb_Results.Text += "App does not use GPUs\r\n";
                    return false;
            }

            if (rbElapsed.Checked)
            {
                tb_Results.Text +=  CalcGPUstats(DeviceMax + 1, i, j);
                return true;
            }
            else if(rbThroughput.Checked)
            {
                if (tb_AvgCredit.Text == "0")
                {
                    tb_Results.Text = "need to specify a credit value\r\nClick on Lookup Credit or just use 100\r\n";
                    return false;
                }
                tb_Results.Text += CalcGPUcredits(DeviceMax + 1, i, j);
            }
            return true;
        }

        // the first number shown in the selection box is line number in the history file, not the index to the project info table
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            btnGTime.Enabled = cbGPUcompare.Checked;
            tb_Results.Text = "";
            btnScatGpu.Enabled = true;
            if (cbGPUcompare.Checked)
            {
                MaxDeviceCount = -1;
                iStart = -1;
                iStop = -1;
                FilterUsingGPUs(ref MaxDeviceCount, ref iStart, ref iStop);
                return;
            }
            if (rbElapsed.Checked) PerformStats();
            if (rbThroughput.Checked) PerformThruput();
            if (rbIdle.Checked)
            {
                if (PerformIdleAnalysis())
                    ShowIdleInfo();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lb_SelWorkUnits.SelectedIndices.Clear();
            ShowSelectable(false);
            btnGTime.Enabled = false;
            tb_Results.Text = "";
        }

        private void cb_AppNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = cb_AppNames.Text;
            if (cb_AppNames.Text == "") return;
            ClearInfoTables();
            DisplayHistory();
        }


        //frm2.ShowDialog(); //shows form as a modal dialog
        //frm2.Show();    //shows form as a non modal dialog
        //frm2.Dispose();   
        private void btnShowProjectTree_Click(object sender, EventArgs e)
        {
            if (KnownProjApps == null)
            {
                tb_Info.Text += "You must first open a history file\r\n";
                return;
            }
            InfoForm MyInfo = new InfoForm(this);
            MyInfo.ShowDialog();
            MyInfo.Dispose();
        }

        // find the biggest gap in the difference between completion time
        // only is idle if the elapsed time is less then the gap
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
                if (iHoursAgo == 0)
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

        // the following two routines make certain object visible depending on whether
        // the data (if any) allows features to be utilized.
        private void ShowContinunities(bool bShow)
        {
            btnCheckNext.Enabled = bShow;
            btnCheckPrev.Enabled = bShow;
            lb_history_loc.Visible = bShow;
        }

        private void ShowSelectable(bool bShow)
        {
            btnContinunity.Enabled = bShow;
            btn_Filter.Enabled = bShow; ;
            lb_NumSel.Visible = bShow;
            lb_LocMax.Visible = bShow;
            btnPlot.Enabled = bShow;
            btnPlotET.Enabled = bShow;
            btnCheckNext.Enabled = bShow;
            btnCheckPrev.Enabled = bShow;
        }

        // see how many items the user selected in the elapsed time list
        // only allowed to select 2 items, a start and a stop 
        double TimeIntervalMinutes = -1;
        private int CountSelected()
        {
            int i, j, n = lb_SelWorkUnits.SelectedIndices.Count;
            string strTimeDiff;
            long tStart, tEnd;
            TimeIntervalMinutes = -1;
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
                tStart = ThisProjectInfo[SortToInfo[i]].time_t_Completed - Convert.ToInt64(ThisProjectInfo[iSortIndex[i]].dElapsedTime);
                tEnd = ThisProjectInfo[SortToInfo[j]].time_t_Completed;
                strTimeDiff = BestTimeUnits(tEnd - tStart);
                lbSeriesTime.Text = "total series time(only valid shown): " + strTimeDiff;
                return 0;
            }
            ShowSelectable(true);
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            tStart = ThisProjectInfo[SortToInfo[i]].time_t_Completed - Convert.ToInt64(ThisProjectInfo[SortToInfo[i]].dElapsedTime);
            j = lb_SelWorkUnits.SelectedIndices[1];
            tEnd = ThisProjectInfo[SortToInfo[j]].time_t_Completed;
            strTimeDiff = BestTimeUnits(tEnd - tStart);
            lbSeriesTime.Text = "Selected series time(only valid shown): " + strTimeDiff;
            n = 1 + j - i;
            lb_NumSel.Text = "Selected: " + n.ToString();
            TimeIntervalMinutes = (tEnd - tStart) / 60.0;
            return n;
        }

        // quick selection of all items
        private void bt_all_Click(object sender, EventArgs e)
        {
            int i = lb_SelWorkUnits.Items.Count;
            btnGTime.Enabled = false ;
            btnScatGpu.Enabled = false;
            if (i == 0) return;
            lb_SelWorkUnits.ClearSelected();
            lb_SelWorkUnits.SetSelected(0, true);
            lb_SelWorkUnits.SetSelected(i - 1, true);
            CountSelected();
        }

        private void lb_SelWorkUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountSelected();
        }


        // we calculated the average somewhere else, but now we need the standard deviation
        public double CalcStd(double avg, ref List<double> Values)
        {
            double dd;
            double std = 0, rms = 0;
            foreach (double d in Values)
            {
                dd = d - avg;
                rms += (dd * dd);
            }
            std = Math.Sqrt(rms / Values.Count);
            return std;
        }

        // user want a plot of the gaps in completion
        // this forms the data that is eventually plotted
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
            CompletionTimes = new List<double>(n + 1);
            IdleGap = new List<double>(n);
            AvgGap = 0;
            // need to make sure the first one valid
            do
            {
                int k = SortToInfo[i]; //iSortIndex[i];
                if (ThisProjectInfo[k].bState) break;
                i++;
                if (i >= j) return false;
            }
            while (true);

            for (n = i; n <= j; n++)
            {
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
            if (AvgGap == 0)
            {
                tb_Info.Text = "All are zero:  no gaps, nothing to plot\r\n";
                return false;
            }
            StdGap = CalcStd(AvgGap, ref IdleGap);
            return true;
        }

        // show results of the continunity test
        private void ShowIdleInfo()
        {
            tb_Results.Text = "Number of idle gaps " + IdleGap.Count.ToString() + "\r\n";
            tb_Results.Text += "Average Gap Size " + BestTimeUnits(Convert.ToInt64(AvgGap)) + "\r\n";
            tb_Results.Text += "Standard Deviation of Gap " + BestTimeUnits(Convert.ToInt64(StdGap));
        }

        // use that neat histogram feature
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

        // user want a plot of the elapsed time in completion
        // this forms the data that is eventually plotted
        private bool CalculateETdistribution()
        {
            int i, j, n, h, lPtr;
            long l;
            double d, d1;
            int k;

            if (lb_SelWorkUnits.Items.Count < 2) return false;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return false;
            i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 3) return false;  // need to show two segments at least    
            IdleGap = new List<double>(n);
            //GrpList = new List<int>(n);
            AvgGap = 0;
            for (n = i; n <= j; n++)
            {
                k = iSortIndex[n];
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime / 60.0;
                IdleGap.Add(d);
                //GrpList.Add(ThisProjectInfo[k].DatasetGroup);
                AvgGap += d;
            }
            AvgGap /= IdleGap.Count;
            StdGap = CalcStd(AvgGap, ref IdleGap);

            // the histogram stuff needs to be moved to the chart program if we are to do a subset using groups

            Histogram<double> hist = new Histogram<double>();
            h = 0;
            foreach (double dd in IdleGap)
            {
                hist.IncrementCount(dd);
            }
            k = hist.Count; // Convert.ToInt32(d);
            CompletionTimes = new List<double>(k);
            for (i = 0; i < k; i++) CompletionTimes.Add(0);
            IdleGap = new List<double>(k);
            for (i = 0; i < k; i++) IdleGap.Add(0);
            d = hist.Count / (k - 1);
            d = Math.Ceiling(d);
            j = Convert.ToInt32(d);
            lPtr = 0;
            h = 0;
            foreach (KeyValuePair<double, uint> histEntry in hist.AsEnumerable())
            {
                IdleGap[lPtr] = histEntry.Value;
                CompletionTimes[lPtr] = histEntry.Key;
                lPtr++;
            }
            return true;
        }

        // idle plot
        private void btnPlot_Click(object sender, EventArgs e)
        {
            if (PerformIdleAnalysis())
            {
                TPchart DrawThruput = new TPchart(ref CompletionTimes, ref IdleGap, AvgGap, StdGap, CurrentProject);
                DrawThruput.ShowDialog();
                DrawThruput.Dispose();
            }
        }

        // elapsed time is identified using the -1 arguments, the vars are misnamed but I prefer reused instead.
        private void btnPlotET_Click(object sender, EventArgs e) // this is the histogram
        {
            if (CalculateETdistribution())
            {
                TPchart DrawThruput = new TPchart(ref CompletionTimes, ref IdleGap, -1, -1, CurrentProject);
                DrawThruput.ShowDialog();
                DrawThruput.Dispose();
            }
        }

        // the next group in ascending order of time
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

        // go backwards in time looking for another group.
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

        // want to save setting for whether to use CVS or CVS1.  Fred says to use CVS1, not sure why exactly
        private void BTHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.TypeCVS = rbUseCVS1.Checked;
            Properties.Settings.Default.RecLimit = tboxLimit.Text;
            Properties.Settings.Default.UseLimit = cboxStopLoad.Checked;
            Properties.Settings.Default.Save();
        }

        private string GetSimpleDate(string sDT)
        {
            //Sun 06/09/2019 23:33:53.18 
            int i = sDT.IndexOf(' ');
            i++;
            int j = sDT.LastIndexOf('.');
            return sDT.Substring(i, j - i);
        }

        private void TimerShowBuild_Tick(object sender, EventArgs e)
        {
            TimerShowBuild.Enabled = false;
            // possibly this form is not active ???
            //BTHistory.ActiveForm.Text = "Build Date:" + Properties.Resources.BuildDate;
            lblBuildDate.Text = GetSimpleDate(Properties.Resources.BuildDate) + " (v) 1.0";
        }


        private void ShowScatter()
        {
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData, "Datasets", cbShowError.Checked);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private void ShowGPUScatter()
        {
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData, "GPUs", cbShowError.Checked);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }


        private List<cSeriesData> MySeriesData;
        bool FormSeriesFromSets()
        {
            MySeriesData = new List<cSeriesData>();
            int iLoc = LookupProject(cb_SelProj.Text); //cb_SelProj.SelectedIndex;
            int jLoc;   // traverse pointer to see if data is bad or not
            bool bGood;
            bool bAny = false;
            if (iLoc < 0) return false; // canceled out of open history but tried a plot
            foreach (cAppName appName in KnownProjApps[iLoc].KnownApps)
            {
                int n = appName.nAppEntries;
                if (n > 0)
                {
                    if (appName.bNoResults && (!cbShowError.Checked))
                        continue;
                    bAny = true;
                    cSeriesData sa = new cSeriesData();
                    sa.strSysName = CurrentSystem;      // only 1 system as we are not comparing systems
                    sa.strAppName = appName.Name;       // usually more than one app this must be first in listview
                    sa.strProjName = CurrentProject;    //only doing one project use this for title info
                    sa.dValues = new List<double>();
                    jLoc = 0;
                    sa.iGpuDevice = new List<int>();
                    foreach (double d in appName.dElapsedTime)
                    {
                        bGood = appName.bIsValid[jLoc];
                        sa.iGpuDevice.Add(appName.DeviceID[jLoc]);
                        jLoc++;
                        if(cbShowError.Checked)
                            sa.dValues.Add(d / 60.0);   // probably should just use minutes to start with
                        else
                        {
                            if (bGood)
                                sa.dValues.Add(d / 60.0);
                            else continue;
                        }
                    }
                    sa.iSystem = appName.DataSetGroup; //  new List<int>();

                    sa.TheseSystems = new List<string>();   // these numbers are the index into the dataset name
                                                            // each app has list of names which is a duplicate problem maybe

                    foreach (cNameValue nv in appName.DataName.DataNameInfo)
                    {
                        //string strUse = appName.nUsesGPU < 0 ? "[CPU]" : "[GPU" + appName.nUsesGPU.ToString() + "]";
                        sa.TheseSystems.Add(nv.DataName + " (" + nv.SizeGroup.ToString() + ")");  
                        // strUse not useful here as this is not homogeneous group (gpus are different)
                    }
                    sa.iTheseSystem = new List<int>();  // this needs to match the name of the dataset
                    for (int i = 0; i < sa.TheseSystems.Count; i++)
                    {

                        sa.iTheseSystem.Add(i); // there is no single repository of group numbers, each app
                                                // has its own copy so they are all numbered sequential
                    }
                    sa.ShowType = eShowType.DoingSets;
                    sa.bIsValid = appName.bIsValid; //new List<bool>();
                    sa.nConcurrent = 1;

                    MySeriesData.Add(sa);
                }
            }
            return bAny;
        }


        bool FormSeriesFromGPUs()
        {
            bool bAny = false;
            int iGpu = 0;
            MySeriesData = new List<cSeriesData>();
            List<int> iGPUsUsed = new List<int>();   // these will be series names
                                                     // need to count devices
            cSeriesData sa;
            foreach (cProjectInfo pi in ThisProjectInfo)
            {
                if(pi.bState || cbShowError.Checked)
                {
                    if (iGPUsUsed.Contains(pi.iDeviceUsed)) continue;
                    iGpu = pi.iDeviceUsed;
                    iGPUsUsed.Add(iGpu);
                    sa = new cSeriesData();
                    sa.strSeriesName = "D" + iGpu.ToString();
                    bAny = true;
                    sa.strSysName = CurrentSystem;      // only 1 system as we are not comparing systems
                    sa.strAppName = cb_AppNames.Text;       // usually more than one app this must be first in listview
                    sa.strProjName = CurrentProject;    //only doing one project use this for title info
                    sa.dValues = new List<double>();
                    sa.ShowType = eShowType.DoingGPUs;
                    sa.bIsValid = new List<bool>();
                    sa.nConcurrent = 1;
                    sa.TheseSystems = new List<string>();
                    sa.iTheseSystem = new List<int>();
                    sa.iTheseSystem.Add(0);
                    MySeriesData.Add(sa);
                }
            }
            foreach (cProjectInfo pi in ThisProjectInfo)
            {
                if (pi.bState || cbShowError.Checked)
                {
                    iGpu = pi.iDeviceUsed;
                    sa = MySeriesData[iGpu];
                    sa.dValues.Add(pi.dElapsedTime / 60.0);
                    sa.bIsValid.Add(pi.bState);
                }
            }
            return bAny;
        }

        private void btnScatSets_Click(object sender, EventArgs e)
        {
            if (FormSeriesFromSets())
                ShowScatter();
        }

        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        /*
                 "Milkyway@Home","World Community Grid","SETI@home","Rosetta@home",
                "GPUGRID","Einstein@Home","LHC@home","Asteroids@home","NumberFields@home",
                "latinsquares","TN-Grid Platform","collatz","Collatz Conjecture",
                "PrimeGrid","Universe@Home","Bitcoin Utopia", "nfs@home", "enigma",
                "Amicable"}; 
        */

        private void btnLkCr_Click(object sender, EventArgs e)
        {
            string[] strProjNames =  {
                "ilkyway","ommunity","SETI","Rosetta",
                "GPUGRID","Einstein","LHC@home","Asteroids","NumberFields",
                "latinsquares","TN-Grid Platform","ollatz",
                "PrimeGrid","Universe","Bitcoin Utopia", "nfs@home", "enigma",
                "Amicable"};
            string[] ProjUrls = {
                "https://milkyway.cs.rpi.edu/milkyway",
                "https://www.worldcommunitygrid.org/",
                "https://setiathome.berkeley.edu",
                "https://boinc.bakerlab.org/",
                //
                "http://www.gpugrid.net",
                "https://einsteinathome.org",
                "https://lhcathome.cern.ch/lhcathome",
                "http://asteroidsathome.net",
                "https://numberfields.asu.edu/NumberFields/",
                //
                "https://boinc.multi-pool.info/latinsquares",
                "http://gene.disi.unitn.it/test/",
                "https://boinc.thesonntags.com/collatz",
                //
                "https://www.primegrid.com/",
                "https://universeathome.pl/universe/",
                "", // project info no longer exists
                "https://escatter11.fullerton.edu/nfs/",
                "http://www.enigmaathome.net"
            };
            GoToSite("www.stateson.net/hostprojectstats");
            string strProj = cb_SelProj.SelectedItem.ToString().ToLower();
            int iIndex= 0;
            bool bFound = false;
            foreach(string strName in strProjNames)
            {
                string strLower = strName.ToLower();
                if(strProj.Contains(strLower))
                {
                    string strUrl = ProjUrls[iIndex];
                    if (strUrl == "")
                    {
                        break;
                    }
                    GoToSite(strUrl);
                    bFound = true;
                    break;
                }
                iIndex++;
            }
            if(!bFound)
            {
                tb_Info.Text += "missing url to " + strProj + "\r\n";
            }
        }

        private void cbGPUcompare_CheckedChanged(object sender, EventArgs e)
        {
            if(cbGPUcompare.Checked)
            {
                rbIdle.Enabled = false;
                if (rbIdle.Checked)
                    rbElapsed.Checked = true;
            }
            else
            {
                rbIdle.Enabled = true;
            }
        }

        private void btnGTime_Click(object sender, EventArgs e)
        {
            if (iStart < 0 || iStop < 0) return;
            double dMax = -1.0;
            for(int i=iStart; i<=iStop; i++)
            {
                int j = SortToInfo[i];
                cProjectInfo pi = ThisProjectInfo[j];
                if (pi.bState)  // do not use error checked box as times series is not useful with bad numbers
                {
                    dMax = Math.Max(dMax, pi.dElapsedTime);
                }
                else continue;
            }
            timegraph DeviceGraph = new timegraph (ref ThisProjectInfo, 1+MaxDeviceCount, iStart, iStop,dMax, ref SortToInfo);
            DeviceGraph.ShowDialog();
            DeviceGraph.Dispose();
        }

        private void gb_filter_Enter(object sender, EventArgs e)
        {

        }

        private void btnClrInfo_Click(object sender, EventArgs e)
        {
            tb_Info.Text = "";
        }

        private void btnScatGpu_Click(object sender, EventArgs e)
        {
            if (FormSeriesFromGPUs())
                ShowGPUScatter();
        }
    }
}
