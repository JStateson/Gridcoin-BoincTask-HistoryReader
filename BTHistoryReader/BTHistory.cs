
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace BTHistoryReader
{

    public partial class BTHistory : Form
    {
        public const int MAXGPUS = 64;
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

        cGpuReassigned GpuReassignment = new cGpuReassigned();

        cAdvFilter MyAdvFilter = new cAdvFilter();

        public class cGpuFilter
        {
            public string nGpu { get; set; }
            public bool bGpu { get; set; }
            public string sStats { get; set; }
        }

        private List<cGpuFilter> GpuFilters;

        private void FillGPU()
        {
            dgvOF.DataSource = GpuFilters;
            dgvOF.AutoGenerateColumns = false;
            dgvOF.Columns[0].HeaderText = "gpu";
            dgvOF.Columns[0].Width = 32;
            dgvOF.Columns[1].HeaderText = "filter";
            dgvOF.Columns[1].Width = 32;
            dgvOF.Columns[2].HeaderText = "statistics";
            dgvOF.Columns[0].ReadOnly = true;
            dgvOF.Columns[2].ReadOnly = true;
        }

        public BTHistory()
        {
            InitializeComponent();
            GpuFilters = new List<cGpuFilter>(); 
            try
            {
                // DEFAULT IS TO USE CVS1  note that the 3 files slowly change in real time as they are updated
                bool b = Properties.Settings.Default.TypeCVS;
                if (!b) rbUseCVS.Checked = true;
                else rbUseCVS1.Checked = true;
                cboxStopLoad.Checked = Properties.Settings.Default.UseLimit;
                tboxLimit.Text = Properties.Settings.Default.RecLimit;
                lbLastFiles.Text = Properties.Settings.Default.LastFiles;
                cbFilterSTD.SelectedIndexChanged -= cbFilterSTD_SelectedIndexChanged;
                cbFilterSTD.SelectedIndex = 3;  // no filtering
                cbFilterSTD.SelectedIndexChanged += cbFilterSTD_SelectedIndexChanged;
                GpuReassignment.init();
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
            MyAdvFilter.bContains = true;   // must match default in design view of AdvFilter
            MyAdvFilter.strPhrase = ""; // for now dont bother with any properties or setting can add save feature later if at all
            GpuReassignment.ReassignedGPU = -1;
            GpuReassignment.NumGPUs = 1;    // if no GPUs then CPU must have at least 1
        }

        private int iLocMaxDiff;
        public string[] AllHistories;

        static string str_PathToHistory;
        public string[] LinesHistory;
        static string ReqVer = "1.79";
        static string ReqID = "BoincTasks History";
        static double dAvgCreditPerUnit;
        static int iPadSize;
        static int[] iSortIndex; //  the items in the InfoTable are ordered by this index when put into the select box
        static int[] SortToInfo; // index created by sort so that the items in the select box can easily locate the actual items in
                                 // the info table (backwards to info table)
        static int NumInSTI;     // number items in above int array

        static int LastKnownProject = 0;
        public int NumberBadWorkUnits;
        static int ExpectedLengthLine = 100;

        const int LKUP_NOT_FOUND = -1;      // cannot find project- forgot it or new one
        const int LKUP_TOBE_IGNORED = -2;   // do not use this project
        const int LKUP_INVALID_LINE = -3;   // line in history is invalid 

        private List<cOutFilter> lNAS = new List<cOutFilter>();

        public string CurrentSystem;    // computer name
        public string CurrentProject;   // project being looked at
        private cDataName CurrentDataset;
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
        private cAppName CurrentApp; // the current one selected


        // put some projects into the table but unknown (to this program) also get added
        // there is no limit on projects nor apps.  Usefull to initialize so as to
        // see if there are new or really old apps being run.
        private void InitLookupTable()
        {
            cKnownProjApps kpa;
            KnownProjApps = new List<cKnownProjApps>();
            OneSplitLine = new cSplitHistoryValues();
            kpa = new cKnownProjApps();
            kpa.AddName("Milkyway@Home",228.0);
            kpa.AddApp("Milkyway@home Separation", "opencl_ati_101",228.0);
            kpa.AddApp("Milkyway@home Separation", "opencl_nvidia_101",228.0);
            kpa.AddApp("Milkyway@home", "opencl_ati_101",228.0);
            kpa.AddApp("Milkyway@home Separation", "",228.0);
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
            kpa.AddName("Amicable Numbers",6836.19);
            kpa.AddApp("Amicable Numbers up to 10^20", "opencl_amd");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("LHC@home");
            kpa.AddApp("SixTrack", "",3.0);  // estimate as a lot are less than 1.0
            kpa.AddApp("CMS Simulation", "");
            kpa.AddApp("Theory Simulation", "", 50.0); // average
            kpa.AddApp("Atlas Simulation", "",1300.0); // average as really large swings
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("World Community Grid",110.0); // looks like 110 is average for mcm and covid
            kpa.AddApp("Mapping Cancer Markers","", 97.0);
            kpa.AddApp("FightAIDS@Home - Phase 1", "");
            kpa.AddApp("FightAIDS@Home - Phase 2", "");
            kpa.AddApp("OpenZika", "");
            kpa.AddApp("Microbiome Immunity Project", "");
            kpa.AddApp("OpenPandemics - COVID 19", "", 85.0);
            kpa.AddApp("OpenPandemics - COVID-19 - GPU [opencl_ati_102]","", 1350.0);
            kpa.AddApp("OpenPandemics - COVID-19 - GPU [opencl_nvidia_102]", "", 1350.0);
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("GPUGRID");
            kpa.AddApp("Short runs (2-3 hours on fastest card)", "cuda80");
            kpa.AddApp("Long runs (8-12 hours on fastest card)", "cuda80");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NFS@Home",44.0);
            kpa.AddApp("14e Lattice Sieve", "",36.0);
            kpa.AddApp("14e Lattice Sieve", "notphenomiix6",36.0);
            kpa.AddApp("15e Lattice Sieve", "notphenomiix6", 44.0);
            kpa.AddApp("15e Lattice Sieve", "",44.0);
            kpa.AddApp("16e Lattice Sieve V5", "",130.0);
            kpa.AddApp("15e Lattice Sieve for smaller numbers", "",44.0);
            kpa.AddApp("16e Lattice Sieve for smaller numbers", "",50.0);
            kpa.AddApp("16e Lattice Sieve", "notphenomiix6");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Cosmology@Home",375.0);  // averages about 375
            kpa.AddApp("camb_legacy", "");
            KnownProjApps.Add(kpa);


            kpa = new cKnownProjApps();
            kpa.AddName("Rosetta@home",400.0);  // this is an average
            kpa.AddApp("Rosetta", "");
            kpa.AddApp("Rosetta Mini", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("TN-Grid Platform",130.0); // another average
            kpa.AddApp("gene@home PC-IM", "sse2");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("NumberFields@home",190.0);
            kpa.AddApp("Get Decic Fields", "default");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Enigma@Home");
            kpa.AddApp("Enigma GPU", "");
            kpa.AddApp("Enigma GPU", "cuda_fermi");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("PrimeGrid",3371.0);
            kpa.AddApp("PPS (Sieve)", "cudaPPSsieve");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Einstein@Home",3333.0);
            kpa.AddApp("Gravitational Wave Engineering run on LIGO O1 Open Data", "");
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-ati",3465.0);
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl1K-nvidia",3465.0);
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl-nvidia",3465.0);
            kpa.AddApp("Binary Radio Pulsar Search (MeerKAT)", "BRP7-cuda55",3333.0);
            kpa.AddApp("Binary Radio Pulsar Search (MeerKAT)", "BRP7-opencl-nvidia", 3333.0);
            kpa.AddApp("Binary Radio Pulsar Search (MeerKAT)", "BRP7-opencl-ati", 3333.0);
            kpa.AddApp("Gamma-ray pulsar binary search #1 on GPUs", "FGRPopencl2Pup-nvidia", 3465.0);
            KnownProjApps.Add(kpa);


            // this project ignored
            kpa = new cKnownProjApps();
            kpa.AddName("WUProp@Home");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("latinsquares",11.0);
            kpa.AddApp("odlk3@home", "");
            kpa.AddApp("odlkmax@home", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("yoyo@home",835.73);
            kpa.AddApp("Siever", "");
            kpa.AddApp("Cruncher ogr", "");
            kpa.AddApp("ecm", "");
            KnownProjApps.Add(kpa);

            kpa = new cKnownProjApps();
            kpa.AddName("Asteroids@home",60.0);
            kpa.AddApp("Period Search Application", "cuda118_linux");
            kpa.AddApp("Period Search Application", "cuda118_win10");
            KnownProjApps.Add(kpa);


            kpa = new cKnownProjApps();
            kpa.AddName("SiDock@home", 960.0); // average of 54 results
            kpa.AddApp("CurieMarieDock 0.2.0 long tasks", "");
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
            kpa.AddName("Universe@Home",666.67);
            kpa.AddApp("Universe BHspin v2", "");
            KnownProjApps.Add(kpa);


            //lb_NumKnown.Text = "Known Projects: " + KnownProjApps.Count.ToString();
            LastKnownProject = KnownProjApps.Count;
        }

        // remove old stuff
        public void ClearInfoTables()
        {
            //tb_AvgCredit.Text = "0";
            //tb_Info.Text = "";
            tb_Results.Text = "";
        }

        // more old stuff to remove
        private void ClearForNewProject()
        {
            lb_SelWorkUnits.Items.Clear();
            ClearInfoTables();
        }

        // ditto on old stuff
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
                lb_nApps.Text = cb_AppNames.Items.Count.ToString() + " App" + (cb_AppNames.Items.Count > 1 ? "s" : "");
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
            groupBox10.Enabled = b;
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
            if (AllHistories.Length > 1) // more than one history has been selected
            {
                lbLastFiles.Text = "";
                foreach (string strHisFile in AllHistories)
                {
                    lbLastFiles.Text += strHisFile + "\r\n";
                }
                SaveLast();
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
            lbLastFiles.Text = ofd_history.FileName;
            SaveLast();
            if (File.Exists(lb_history_loc.Text))
            {
                str_PathToHistory = lb_history_loc.Text;
                if (ValidateHistory(str_PathToHistory, ref ThisSystem) >= 0)
                {
                    ClearPreviousHistory();
                    CurrentSystem = LinesHistory[1];
                    if (BTHistory.ActiveForm != null)    // can occur during debugging
                        BTHistory.ActiveForm.Text = CurrentSystem; ;   // this is name of the computer
                    ProcessHistoryFile(); // provide statistics on this history file
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
            lb_nProj.Text = bAny ? cb_SelProj.Items.Count.ToString() + " Project" + (cb_SelProj.Items.Count > 1 ? "s" :"") : "";
            ShowNumberApps();
            return true;
        }


        private void ShowGPUcount(ref cAppName AppName)
        {
            tb_Results.Text = "";
            GpuFilters.Clear();
            dgvOF.DataSource = null;
            for (int i = 0; i < AppName.GpuReassignment.NumGPUs;  i++)
            {
                string sName = AppName.GpuReassignment.idGPUused(i).ToString();
                cGpuFilter cGF = new cGpuFilter();
                cGF.nGpu = (i + 1).ToString();
                cGF.bGpu = false;
                cGF.sStats = "";
                GpuFilters.Add(cGF);
                tb_Results.Text += "GPU-" + (i+1).ToString() + " " + sName + "\r\n";
            }
            if(AppName.GpuReassignment.NumberGPUsUnknown > 0)
            {
                string sName = AppName.GpuReassignment.NumberGPUsUnknown.ToString();
                tb_Results.Text += "Number unknown GPUs " + sName + "\r\n";
            }
            if (GpuFilters.Count > 0)
            {
                FillGPU();
            }
        }

        private void btn_OpenHistory_Click(object sender, EventArgs e)
        {
            tb_Info.Text = "";
            nudConCurrent.Value = 1;
            GpuReassignment.NumGPUs = 0;
            GpuReassignment.ReassignedGPU = -1;
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
            int jStart, jStop; // 12/22/2022
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                foreach (cAppName AppName in kpa.KnownApps)
                {
                    if (AppName.bIsValid.Count > 0)
                    {
                        int FirstValid = -1;
                        int LastValid = -1;
                        bool bAny = AppName.DoAverages(ref FirstValid, ref LastValid);
                        if(bAny)
                        {
                            //jStart = FirstValid + 4; 
                            jStart = AppName.LineLoc[FirstValid];
                            string[] strSymbols = LinesHistory[jStart].Split('\t');
                            long lStart = Convert.ToInt64(strSymbols[11]);
                            lStart -= Convert.ToInt64(AppName.dElapsedTime[FirstValid]);
                            // jStop = LastValid + 4;
                            jStop = AppName.LineLoc[LastValid];
                            strSymbols = LinesHistory[jStop].Split('\t');
                            long lStop = Convert.ToInt64(strSymbols[11]);
                            AppName.TotalTimeSecs = lStop - lStart;
                            if (AppName.bIsValid.Count > 1)
                            {
                                AppName.CreditPerDay = AppName.AppCredit * AppName.bIsValid.Count * 86400.0 / AppName.TotalTimeSecs;
                                if(AppName.CreditPerDay < 0)
                                {
                                    // 12/24/2022 was caused by wuprop.  wuprop is an ignored project, needs to be marked as invalid
                                    // ok:  turned out that only "ignored" apps caused the problem.  The indexes were offset by 1 due to 
                                    // the ignored app and the value calcualateds was correct to to 0 being set for credit
                                    // all calculations were correct whether LineLoc was used or not
                                    AppName.CreditPerDay = 0.0; // 2-9-2020 did see a huge negative number here but could not reproduce it
                                    Debug.Assert(false);
                                }
                            }
                            else AppName.CreditPerDay = 0.0;
                        }
                    }
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
            string strClass = "";
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
                else kpa = KnownProjApps[RtnCode];
                bAnyData = true;
                strClass = OneSplitLine.PlanClass;
                if (strClass != "") strClass = " [" + strClass + "]";
                AppName = kpa.SymbolInsert(OneSplitLine.Application + strClass, 3 + iLine);  // first real data is in 5th line (0..4)
                AppName.AddUse(OneSplitLine.use);   
                iGrp = AppName.DataName.NameInsert(OneSplitLine.Name, OneSplitLine.Project);
                iLocDevice = OneSplitLine.use.IndexOf("device "); //1234567  note that sometimes the device is missing, if so, then use 0 as
                    // possible there was only 1 device and no number was assigned
                if (iLocDevice > 0)
                {
                    jloc = OneSplitLine.use.LastIndexOf(")");
                    iLocDevice += 7;
                    jloc -= iLocDevice;
                    OneSplitLine.iDeviceUsed = Convert.ToInt32(OneSplitLine.use.Substring(iLocDevice, jloc));
                }
                else OneSplitLine.iDeviceUsed = 0;  // device is not shown if only one gpu so use 0   
                AppName.AddETinfo(OneSplitLine.dElapsedTimeCpu, iGrp, OneSplitLine.iDeviceUsed);
                // the above iGrp needs to go into ThisProjectInfo which unfortunately does not exist here
                // and I do not want to rewrite this code at this time.
                // going to append that value to the current history line
                LinesHistory[iLine + 3] += "WTFaTODO_" + iGrp.ToString() + "|"; // + "WTFbTODO_" + OneSplitLine.iDeviceUsed.ToString();
                bTemp = (OneSplitLine.ExitStatus == 0); // this is not a complete test as it seem if CVS is used then the exit "state"
                // can be 2.  Not sure what that represents but it is a problem as shown at bottom of this source in a comment
                // looks like the fix is to check for 0 in reported and completed time.  I think the problem is CVS is a temp file and CVS1
                // is the one that is more "complete" but cvs has the most newest stuff and cvs1 and 3 like several hours behine
                if (OneSplitLine.State == 1) bTemp = false;     // this is always the case
                if (OneSplitLine.ReportedTime == 0) bTemp = false;  // jys 9-6-2019 maybe this fixes the problem with bad values
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
            {
                PerformCalcAverages();
            }
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
            tb_Results.Text = "";
            ClearForNewProject();
            cb_SelProj.Text = cb_SelProj.Items[i].ToString();
            i = LookupProject(cb_SelProj.Text);
            FillAppBox(i);
            DisallowAppCallbacks(false);
            DisplayHistory();
            ShowNumberApps();
            if (lb_SelWorkUnits.Items.Count == 0)
                tb_Results.Text = "No good data for this app\r\n";
            cb_AppNames_SelectedIndexChanged(null, null);   // need to propagate the change
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
                if (!ThisProjectInfo[j].bState)
                {
                    continue;
                }
                sTemp = ThisProjectInfo[j].strOutput;
                SortToInfo[n] = j; 
                n++;
                // do not show D64
                sTemp = sTemp.Replace("D64", "");
                lb_SelWorkUnits.Items.Add(sTemp);
            }
            NumInSTI = n;
            pbarLoading.Visible = false;
            lb_SelWorkUnits.UseWaitCursor = false;
            CanChangeProjApp(true);
        }

        // put hours minuts secs in a nice concise format
        static string fmtDHMS(long seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"d\:hh\:mm\:ss").Trim();
        }

        // put hours minuts secs in a nice concise format
        static string fmtHMS(long seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"d\:hh\:mm\:ss");
        }





        // this fills in the "ThisProjectInfo" structure with stuff from each single line in the history files of "the app"
        public int FillProjectInfo(cAppName AppName)
        {
            int nGPUnum = 0; // need to find the number of gpus, if any
            string[] strSymbols;
            string sTemp;
            System.DateTime dt_1970 = new System.DateTime(1970, 1, 1);
            System.DateTime dt_this;
            string strWTF = "";
            int j = 0;
            int iWTF = 0, jWTF;
            bool bState;
            long n, nElapsedTime;
            bool bStopReading = cboxStopLoad.Checked;
            int nLimit = Convert.ToInt32(tboxLimit.Text);
            int i,GPUid, iStart, iTraverse, iCount;
            pbarLoading.Value = 0;
            MyAdvFilter.NumExcluded = 0;
            if (AppName.LineLoc.Count == 0) return 0;
            
            AppName.GpuReassignment = new cGpuReassigned();
            AppName.GpuReassignment.init();
            AppName.bHasDevices = false;
            AppName.bHasGPU = false;
            // this could be rewritten better but WTF, it was done before I got that splitlinestuff to work
            // 8-1-2019 if truncating then use only last part of this table     

            iCount = AppName.LineLoc.Count;
            iStart = 0;
            if (bStopReading)
            {
                if (iCount > nLimit)
                {
                    iStart = iCount - nLimit;
                    iCount = nLimit;
                    tb_Info.Text += " skipping to get last " + tboxLimit.Text + " out of " + AppName.LineLoc.Count.ToString() + " records\r\n";
                }
                AppName.SkipToStart = iStart;
            }
            for (iTraverse = 0; iTraverse < iCount; iTraverse++)
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
                if(iTraverse == 0) // the very first data line of this application see note 1/1/2023 below
                {
                    AppName.bHasGPU = LinesHistory[i].Contains(" GPU");
                }
                if (iWTF > 0)
                {
                    jWTF = LinesHistory[i].IndexOf("|");
                    Debug.Assert(jWTF > 0);
                    iWTF += 9;
                    strWTF = LinesHistory[i].Substring(iWTF, (jWTF - iWTF));
                    ThisProjectInfo[j].DatasetGroup = Convert.ToInt32(strWTF);
                }
                iWTF = LinesHistory[i].IndexOf("device ");
                if (iWTF > 0)
                {
                    AppName.bHasDevices = true;
                    iWTF += 7;
                    jWTF = LinesHistory[i].Substring(iWTF).IndexOf(")");
                    Debug.Assert(jWTF > 0); ;
                    strWTF = LinesHistory[i].Substring(iWTF, (jWTF));
                    GPUid = Convert.ToInt32(strWTF);
                    ThisProjectInfo[j].iDeviceUsed = AppName.iLastGpuUsed = GPUid;
                    AppName.GpuReassignment.AddGpu(GPUid);
                    ThisProjectInfo[j].bDeviceUnk = false;
                    nGPUnum = Math.Max(nGPUnum, AppName.iLastGpuUsed);
                    AppName.GpuReassignment.NumGPUs = 1 + nGPUnum; // note that the filter may have fewer GPUs
                }
                else // either GPU is missing or it is a CPU app
                {
/*
 Problem 1/1/2023  Need to identify if a GPU is unknown (no device id) or if there is only one gpu 
 and no unknown ones.  Assigne GPU #64 to any unknown GPU and if all gpus are 64 then change it to gpu 0
*/
                    if (AppName.bHasGPU)
                    {
                        ThisProjectInfo[j].iDeviceUsed = 64; // jys 1/1/2023 signal possible missing or just 1 gpu
                        AppName.GpuReassignment.AddGpu(64);
                        ThisProjectInfo[j].bDeviceUnk = true; //AppName.bHasDevices;
                        if (cbAssignGPU.Checked && AppName.bHasDevices)
                        {
                            ThisProjectInfo[j].iDeviceUsed = AppName.iLastGpuUsed;
                            AppName.GpuReassignment.AddBadGpu(AppName.iLastGpuUsed); // may  not be bad 
                            ThisProjectInfo[j].iSaveDeviceUsed = AppName.iLastGpuUsed;
                            AppName.GpuReassignment.NumberGPUsUnknown++;
                        }
                    }
                    else
                    {
                        ThisProjectInfo[j].iDeviceUsed = 0; // cpu 
                    }

                }
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
                bState = strSymbols[(int)eHindex.ExitStatus].ToString() == "0";
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
                ThisProjectInfo[j].strName = strSymbols[(int)eHindex.Name];
                if (cbUseAdvFilter.Checked)
                {
                    if (MyAdvFilter.bContains)
                    {
                        ThisProjectInfo[j].bExclude = !(ThisProjectInfo[j].strName.Contains(MyAdvFilter.strPhrase));
                    }
                    else
                    {
                        ThisProjectInfo[j].bExclude = (ThisProjectInfo[j].strName.Contains(MyAdvFilter.strPhrase));
                    }
                }
                else ThisProjectInfo[j].bExclude = false;
                if (ThisProjectInfo[j].bExclude)
                {
                    MyAdvFilter.NumExcluded++;
                    ThisProjectInfo[j].bState = false;  // 9=21=2019 jys expedient to just mark them as bad for now and rewrite later
                }
                j++;
            }
            SortTimeIncreasing(j);
            if (cbUseAdvFilter.Checked)
            {
                tb_Info.Text += "Seems " + (MyAdvFilter.NumExcluded).ToString() + " items were excluded by the advanced filter\r\n";
            }
            return j;        }


        // calculates amount of credit the system can do
        // 10/14/2021 elapsed time can be bigger than seconds per workunit iff concurrent tasks
        // are being done
        private void PerformThruput()
        {

            long t_start, t_stop, t_diff;
            int i, j, k;
            int i1, i2; // used to access iSort..
            double dSeconds = 0;
            int nItems, nDevices;
            double dUnitsPerSecond;
            double WorkunitsPerDay; 
            int NumUnits = lb_SelWorkUnits.SelectedItems.Count;
            string sTemp, s1, s2;
            double WUelapsed = 60.0*PerformStats(false);
            double WattsThisSystem = Convert.ToDouble(tbWPB.Text);
            double sdc; // system daily credits
            if (NumUnits != 2 || WUelapsed < 0.0)
            {
                tb_Results.Text = "you must select exactly two items or stats error\r\n";
                return;
            }
            iStart = i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            iStop = j = lb_SelWorkUnits.SelectedIndices[1];
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
            WorkunitsPerDay = dUnitsPerSecond * 24.0 * 3600.0;
            tb_Results.Text += "Elapsed secs(includes down time): " + dSeconds.ToString("###,##0\r\n");
            if (lbTimeContinunity.Text != "")
                tb_Results.Text += lbTimeContinunity.Text + "\r\n";
            tb_Results.Text += "Number Work Units: " + nItems + "\r\n";
            tb_Results.Text += "Work units per second(system): " + dUnitsPerSecond.ToString("###,##0.0000\r\n");
            tb_Results.Text += "Calc secs per work unit per device: " + (nDevices / dUnitsPerSecond).ToString("###,##0\r\n");
            //tb_Results.Text += "Measured avg secs elapsed per WU: " + WUelapsed.ToString("###,##0\r\n");
            // above is WTF??  Elapsed time is concurrent with other units.  Above has no meaning here!!!
            tb_Results.Text += "Secs per work unit this system: " + (1.0 / dUnitsPerSecond).ToString("###,##0\r\n");
            dAvgCreditPerUnit = Convert.ToDouble(tb_AvgCredit.Text);
            tb_Results.Text += "Credits/sec (one device): " + (dUnitsPerSecond * dAvgCreditPerUnit / nDevices).ToString("##0.00\r\n");
            tb_Results.Text += "Credits/sec (system): " + (dUnitsPerSecond * dAvgCreditPerUnit).ToString("#,##0.00\r\n");
            sdc = 86400.0 * dUnitsPerSecond * dAvgCreditPerUnit;
            tb_Results.Text += "System Daily Avg: " + sdc.ToString("#,###,##0.0\r\n");
            tb_Results.Text += "Avg work units per day:" + WorkunitsPerDay.ToString("###,##0.0\r\n");
            if(WattsThisSystem > 0.0)
            {
                tb_Results.Text += "Credits per watt:" + (sdc / (WattsThisSystem)).ToString("###,##0.0");
            }
        }

        // using the selected items, take an average and the std and display
        // 10/12/2021 want to show completion time instead of the redundent elapsed time
        // 10/14/2021 want to calculate but not alway show results
        private double PerformStats(bool bShow)
        {
            int i, j, k, n;
            double Avg = 0.0;
            double Std = 0.0;
            double d;
            long l, l_First=0;
            string strOut = "";
            int NumCurrent = 1;
            if (cbUseWUs.Checked) NumCurrent = Convert.ToInt32(nudConCurrent.Value);

            int NumUnits = lb_SelWorkUnits.SelectedItems.Count;
            if (NumUnits != 2)
            {
                tb_Results.Text = "you must select exactly two items\r\n";
                return -1.0;
            }
            iStart = i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            iStop = j = lb_SelWorkUnits.SelectedIndices[1]; // 8-9-2019 but these must be used to locate the actual valuea
            n = 1 + j - i;  // number of items to average
            if (n < 2)
            {
                tb_Results.Text += "Need at least 2 items\r\n";
                return -1.0;
            }
            n = 0;
            for (int k1 = i; k1 <= j; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState)
                {
                    continue;
                }
                d = ThisProjectInfo[k].dElapsedTime / NumCurrent;
                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data was finally fixed.  Problem was bitcoin utopia had 0 for cpu
                }                                                        // so putting in "0.1" for cpu but not if gpu is 0 also
                //11/12/21 l = Convert.ToInt64(d);
                l = ThisProjectInfo[k].time_t_Completed;
                if (k1 == i) l_First = l;
                l-= l_First;
                d /= 60.0;
                n++;
                Avg += d;
                //11/12/21     strOut += d.ToString("###,##0.00") + "\t" + fmtHMS(l) + " D" +ThisProjectInfo[k].iDeviceUsed.ToString() + "\r\n";
                //
                if(bShow && !cbShowTotals.Checked)
                    strOut += d.ToString("###,##0.00") + " D" +ThisProjectInfo[k].iDeviceUsed.ToString() + "\t" + fmtHMS(l) + "\r\n";
            }
            if (n == 0) return -1.0;
            Avg /= n;
            l = Convert.ToInt64(Avg * 60.0);
            Std = 0;
            for (int k1 = i; k1 <= j; k1++)
            {
                k = SortToInfo[k1];
                if (!ThisProjectInfo[k].bState) continue;
                d = ThisProjectInfo[k].dElapsedTime / NumCurrent;
                if (d == 0.0)
                {
                    Debug.Assert(false);        // 
                    continue;
                }
                d = d / 60.0 - Avg;
                Std += d * d;
            }
            Std = Math.Sqrt(Std / n);
            if(bShow)
            {
                tb_Results.Text = strOut;
                tb_Results.Text += "Number of selections " + n.ToString("#,##0") + "\r\n";
                tb_Results.Text += "AVG elapsed (minutes) " + Avg.ToString("###,##0.00") + "\t" + fmtHMS(l) + "\r\n";
                tb_Results.Text += "STD of elapsed time " + Std.ToString("###,##0.00") + "\r\n";
                if (cbUseWUs.Checked && NumCurrent > 1)
                    tb_Results.Text += "Statistics divided by number of concurrent tasks:" + NumCurrent.ToString() + "\r\n";
            }
            return Avg;
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
            CurrentApp = AppName; // jys 12/25/2022 want to keep track of unknown GPUs
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
            ShowGPUcount(ref AppName);
        }

        private (List<double>, List<int>) RemoveOutliersWithIndexes(ref List<double> data, double threshold = 2)
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


        private List<double> RemoveOutliers(ref List<double> data, double threshold)
        {
            double mean = data.Average();
            double stdDev = Math.Sqrt(data.Average(v => Math.Pow(v - mean, 2)));
            return data.Where(x => Math.Abs(x - mean) <= threshold * stdDev).ToList();
        }
        private (List<double>, List<int>) GetOneStatList(int iDev, int iStart, int iStop, ref int nOriginal)
        {
            List<double> data = new List<double>();
            int n = 0, nU = 0;
            StdU = 0.0;
            AvgU = 0.0;
            double t = GetOutlierFilter(iDev);
            int NumCurrent = 1;
            if (cbUseWUs.Checked) NumCurrent = Convert.ToInt32(nudConCurrent.Value);
            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                int k = SortToInfo[k1];
                double d = ThisProjectInfo[k].dElapsedTime / NumCurrent;
                d /= 60.0;
                if (ThisProjectInfo[k].bDeviceUnk)
                {
                    nU++;
                    AvgU += d;
                    if (!cbAssignGPU.Checked) continue; // if not assigning the unknown GPU then do not include it in gpu stats
                }
                if (!ThisProjectInfo[k].bState || iDev != ThisProjectInfo[k].iDeviceUsed) continue;

                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data was finally fixed.  Problem was bitcoin utopia had 0 for cpu
                }
                data.Add(d);
                n++;
            }
            if (n == 0)
            {
                return (null,null); // strResult + " has no valid data\r\n"; do not show anything
            }
            nOriginal = data.Count;
            if (t == 0) return (data,null);
            AvgU /= nU;
            return RemoveOutliersWithIndexes(ref data, t);
        }

        private List<double> GetOneStatListx(int iDev, int iStart, int iStop, ref int nOriginal)
        {
            List<double> data = new List<double>();            
            int n = 0, nU = 0;
            StdU = 0.0;
            AvgU = 0.0;
            double t = GetOutlierFilter(iDev);
            int NumCurrent = 1;
            if (cbUseWUs.Checked) NumCurrent = Convert.ToInt32(nudConCurrent.Value);
            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                int k = SortToInfo[k1];
                double d = ThisProjectInfo[k].dElapsedTime / NumCurrent;
                d /= 60.0;
                if (ThisProjectInfo[k].bDeviceUnk)
                {
                    nU++;
                    AvgU += d;
                    if (!cbAssignGPU.Checked) continue; // if not assigning the unknown GPU then do not include it in gpu stats
                }
                if (!ThisProjectInfo[k].bState || iDev != ThisProjectInfo[k].iDeviceUsed) continue;

                if (d == 0.0)
                {
                    Debug.Assert(false);
                    continue; // bad or missing data was finally fixed.  Problem was bitcoin utopia had 0 for cpu
                }
                data.Add(d);
                n++;
            }
            if (n == 0)
            {
                return null; // strResult + " has no valid data\r\n"; do not show anything
            }
            nOriginal = data.Count;
            if (t == 0) return data;
            AvgU /= nU;
            return RemoveOutliers(ref data, t);
        }

        private cOutFilter CalcOneStat(int iDev, int iStart, int iStop, ref int nRemoved)
        {
            int nOriginal = 0;
            int NumCurrent = 1;
            int k;
            long lS=0, lE=0;
            cOutFilter cNAS = new cOutFilter();
            if (cbUseWUs.Checked) NumCurrent = Convert.ToInt32(nudConCurrent.Value);
            (cNAS.data,cNAS.outlierIndexes) = GetOneStatList(iDev, iStart, iStop, ref nOriginal);
            cNAS.time_t = new List<long>();
            for(int i = 0; i < cNAS.data.Count; i++)
            {
                k = i;
                if(cNAS.outlierIndexes != null)
                {
                    k = cNAS.outlierIndexes[i];
                }
                if (i == 0)
                    lS = ThisProjectInfo[k].time_t_Completed;
                if (i == (cNAS.data.Count - 1))
                    lE = ThisProjectInfo[k].time_t_Completed;
                cNAS.time_t.Add(ThisProjectInfo[k].time_t_Completed);
            }
            cNAS.nWidth = lE - lS;

            for (int k1 = iStart; k1 <= iStop; k1++)
            {
                k = SortToInfo[k1];
                double d = ThisProjectInfo[k].dElapsedTime / NumCurrent;
                if (ThisProjectInfo[k].bDeviceUnk)
                {
                    d = d / 60.0 - AvgU;
                    StdU += d * d;
                    continue;
                }
            }
            if(nU > 0)StdU = Math.Sqrt(StdU / nU);
            if (cNAS.data == null) return null;
            if (cNAS.data.Count == 0) return null;
            nRemoved = nOriginal - cNAS.data.Count;
            double Avg = cNAS.data.Average();
            double Std  = Math.Sqrt(cNAS.data.Average(v => Math.Pow(v - Avg, 2)));
            int n = cNAS.data.Count;
            cNAS.n = n;
            cNAS.avg = Avg;
            cNAS.std = Std;
            return cNAS;
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
                return ""; 
            }
            val = n * d / TimeIntervalMinutes;
            return strResult + "(" + val.ToString("#,##0.00)");
        }

        private int nU;
        private double StdU;
        private double AvgU;

        string tbTemp = "";
        private string CalcGPUstats(int nDevices,int iStart, int iStop)
        {
            int nRemoved = 0;
            string strResults = "There are " + nDevices.ToString() + (CurrentApp.bHasGPU ? " GPUs" : " CPUs") + ", units are minutes\r\n";
            strResults += "Dev# WU count  Avg and Std of avg\r\n";
            nU = 0;
            StdU = 0.0;
            AvgU = 0.0;
            tbTemp = "";
            lNAS.Clear();
            int[] dc = {0,0,0};
            for (int i = 0; i < nDevices;i++)
            {                
                cOutFilter cnas = CalcOneStat(i, iStart, iStop, ref nRemoved);
                if (cnas == null) continue;
                lNAS.Add(cnas);
                dc[0] = Math.Max(dc[0], lNAS[i].n.ToString().Length);
                dc[1] = Math.Max(dc[1], lNAS[i].avg.ToString("#.00").Length); // 0.00 has length 4
                dc[2] = Math.Max(dc[2], lNAS[i].std.ToString("#.00").Length);
                string sN = "None removed";
                if(nRemoved == 1)
                {
                    sN = "1 was removed";
                }
                if(nRemoved > 1)
                {
                    sN = nRemoved.ToString() + " were removed";
                }
                tbTemp += "GPU" + (i + 1).ToString() + ": " + sN;
                GpuFilters[i].nGpu = (i + 1).ToString();
                GpuFilters[i].sStats = sN;
                if (i < nDevices - 1) tbTemp += "\r\n";
            }
            if(GpuFilters.Count > 0)
            {
                dgvOF.Invalidate();
            }
            int iDev = 1;
            foreach(cOutFilter cnas in lNAS)
            {
                strResults+= (CurrentApp.bHasGPU ? "GPU" : "CPU") + iDev.ToString() + " WUs:";
                strResults += cnas.n.ToString().PadLeft(dc[0],' ') + " -Stats- Avg:" + cnas.avg.ToString("0.00").PadLeft(dc[1],' ') +
                    "(" + cnas.std.ToString("0.00").PadLeft(dc[2],' ') + ")\r\n";
                iDev++;
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
                string strTemp = CalcOnecredit(i, iStart, iStop, ref d) + "".PadRight(CostSizes[i]);
                AvgAll += d;
                double x = r * d / e;
                int j = Convert.ToInt32(x) ;
                strResults += strTemp + " |++++++++++++++++++++++++++++++++++++++++++++".Substring(0,j) + "\r\n";
            }
            strResults += "System averages " + AvgAll.ToString("#,##0.0") + " credits per minute\r\n";
            return strResults + "System daily average " + (AvgAll * 1440.0).ToString("#,###,##0") + " credits\r\n";
        }

        // history may include fewer gpu if board was removed or failed
        // want to count the gpus in the filter
        private bool CountGPUs()
        {
            int iLastDevice = 0; // use this if the device id is unkno
            // get start, stop and number of devices
            MaxDeviceCount = -1;
            GpuReassignment.clear();
            int i, j, k, n;
            int NumExcluded = 0;
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
            GpuReassignment.NumGPUs = CurrentApp.nDevices;
            if (!CurrentApp.bHasGPU)
            {
                // is a cpu
                MaxDeviceCount = 0;  //sorry - not a count, just the ordinal
                CurrentApp.nDevices = 1;
                return true;
            }
            if(!CurrentApp.bHasDevices || CurrentApp.bHasGPU) // have a GPU but only one so backfix the 64
            {
                for (int k1 = i; k1 <= j; k1++)
                {
                    k = SortToInfo[k1];
                    n = ThisProjectInfo[k].iDeviceUsed;
                    if (n == 64)  // either unknown or is really gpu 0 for single gpu systems
                    {
                        ThisProjectInfo[k].iDeviceUsed = 0;
                        ThisProjectInfo[k].bDeviceUnk = false;
                        CurrentApp.nDevices = 1;
                        CurrentApp.GpuReassignment.NumGPUs = 1;
                        MaxDeviceCount = 0;
                    }
                }
            }
            // want to count the number of assigned or excluded GPUs
            for (int k1 = i; k1 <= j; k1++)
            {
                
                k = SortToInfo[k1];
                n = ThisProjectInfo[k].iDeviceUsed;
                if(n ==  64)  // either unknown or is really gpu 0 for single gpu systems
                {
                    if(cbAssignGPU.Checked)
                    {
                        int n1 = GpuReassignment.ReassignedGPU;                        
                        if(n1 < 0) // want to use the last one but may not be known
                        {
                            if (k1 == i) ThisProjectInfo[k].iDeviceUsed = 0;  // use 0 if no data available (first gpu was missing)
                            else ThisProjectInfo[k].iDeviceUsed = iLastDevice;
                            n = iLastDevice; // not using 64 anymore
                            ThisProjectInfo[k].iSaveDeviceUsed = n;
                        }
                    }
                }
                iLastDevice = n;
                if (!ThisProjectInfo[k].bState)
                {
                    GpuReassignment.AddBadGpu(n);
                    continue;
                }
                if (ThisProjectInfo[k].bDeviceUnk)
                {
                    CurrentApp.iAssignedGPUs++;
                    NumExcluded++;
                    continue;
                }
                MaxDeviceCount = Math.Max(MaxDeviceCount, n);
                GpuReassignment.AddGpu(n);
            }
            if (MaxDeviceCount == -1)
            {
                tb_Results.Text += "Either only CPUs or there are no unknown GPUs\r\n";
                return false;
            }
            GpuReassignment.NumGPUs = MaxDeviceCount + 1;
            CountGpuIdles(GpuReassignment.NumGPUs);
            return true;
        }

        private bool FilterUsingGPUs(ref int iStart, ref int iStop)
        {
            // get start, stop 
            int i, j, k, n;
            int NumExcluded = 0;
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


            if (rbElapsed.Checked)
            {
                tb_Results.Text +=  CalcGPUstats(MaxDeviceCount + 1, i, j);
                if (nU > 0)  // if(NumExcluded > 0 && nU > 0)
                {
                    tb_Results.Text += "UNK?"  + " WUs:" + nU.ToString("##,##0") + " -Stats- Avg:" + AvgU.ToString("###,##0.00") + "(" + StdU.ToString("#,##0.00") + ")\r\n";
                }
                return true;
            }
            else if(rbThroughput.Checked)
            {
                double dLeftOver = Convert.ToDouble(tb_AvgCredit.Text);
                if (tb_AvgCredit.Text == "0")
                {
                    tb_Results.Text = "need to specify a credit value\r\nClick on Lookup Credit or just use 100\r\n";
                    return false;
                }
                tb_Results.Text += CalcGPUcredits(MaxDeviceCount + 1, i, j);
                NumExcluded = Math.Max(NumExcluded, CurrentApp.iAssignedGPUs);
                dLeftOver *= NumExcluded;
                tb_Results.Text += "Num ( " + NumExcluded.ToString() + ") Unknown credits: " + dLeftOver.ToString() + "\r\n";
            }
            return true;
        }

        private void RunFilter()
        {
            int NumCurrent = Convert.ToInt32(nudConCurrent.Value);
            double d = RunContinunityCheck();
            btnGTime.Enabled = cbGPUcompare.Checked;
            tb_Results.Text = "";
            btnScatGpu.Enabled = true;
            CurrentApp.iAssignedGPUs = 0;   // this is recalculated based on filter and is the number reassigned
            if (!CountGPUs()) return;
            if (d > 0.0)
                tb_Results.Text += lbTimeContinunity.Text + "\r\n";
            if (cbGPUcompare.Checked)
            {
                iStart = -1;
                iStop = -1;
                FilterUsingGPUs(ref iStart, ref iStop);
                AddUnknownStats();
                if (cbUseWUs.Checked && NumCurrent > 1) tb_Results.Text += "Statistics divided by number of concurrent tasks:" + NumCurrent.ToString() + "\r\n";
                return;
            }
            if (rbElapsed.Checked) PerformStats(true);
            if (rbThroughput.Checked) PerformThruput();
            if (rbIdle.Checked)
            {
                if (PerformIdleAnalysis())
                    ShowIdleInfo();
                else tb_Info.Text = "Probably no data for idle analysis\r\n";
            }
            AddUnknownStats();
        }

        // the first number shown in the selection box is line number in the history file, not the index to the project info table
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            RunFilter();
        }



        private void AddUnknownStats()
        {
            if (nU == 0) return; // no unknown GPUs
            if(CurrentApp.iAssignedGPUs > 0) // no need to show number of unknowns as that was displayed in statistics earlier
                tb_Results.Text += "Number Unknown GPUs:" + CurrentApp.iAssignedGPUs.ToString() + "\r\n";
            tb_Results.Text += "Work Units Attempted: " + (CurrentApp.LineLoc.Count-CurrentApp.SkipToStart).ToString() + "\r\n";
            tb_Results.Text += "Work Units Completed: " + lb_SelWorkUnits.Items.Count.ToString() + "\r\n";
            if(cbGPUcompare.Checked && rbElapsed.Checked) // only applicable if comparing GPU for elapsed time
            {
                if (cbAssignGPU.Checked) // used to have CurrentApp.iAssignedGPUs
                {
                    tb_Results.Text += "Above unknown GPUs were re-assigned and are included in statistics\r\n";
                }
                else
                {
                    tb_Results.Text += "Above unknown GPUs are not included in statistics\r\n";
                }
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
            if (lb_SelWorkUnits.Items.Count == 0)
                tb_Results.Text = "No good data for this app\r\n";
            CountGpuIdles(0);
            tb_AvgCredit.Text = CurrentApp.AppCredit.ToString();
        }


        private void CountGpuIdles(int nGpu)
        {
            cbIdleGpu.Items.Clear();
            cbIdleGpu.Items.Add("All GPUs");
            for (int i = 0; i < nGpu; i++)
                cbIdleGpu.Items.Add(i.ToString());
            cbIdleGpu.SelectedIndex = 0;
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
        private double RunContinunityCheck()
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
                return 0.0;
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
            return MaxDiff;
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

        private void btnBug_Click(object sender, EventArgs e)
        {
            if(ofd_history.InitialDirectory == "")
            {
                int nFiles = LocatePathToHistory();
                if(nFiles ==0)
                {
                    tb_Info.Text += "Warning: cannot find any history files\r\n";
                    ofd_history.InitialDirectory = "";
                }
            }
            reportbug MyReport = new reportbug(lbLastFiles.Text, ofd_history.InitialDirectory);
            MyReport.ShowDialog();
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
            //btnPlot.Enabled = bShow;
            //btnPlotET.Enabled = bShow;
            groupBox3.Enabled = bShow;
            groupBox5.Enabled = bShow;
            groupBox10.Enabled = bShow;
            btnCheckNext.Enabled = bShow;
            btnCheckPrev.Enabled = bShow;
        }

        string RemoveCnt(string strIn)
        {
            int i = strIn.TrimStart().IndexOf(' ');
            if (i < 0) return strIn;
            return strIn.Substring(i + 1).Trim();
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
                btnScatGpu.Enabled = false; // do not allow gpu scatter unless filtering is done first
                lbTimeContinunity.Text = "not calculated yet";
                if (n == 0)
                    return 0;
                i = 0;
                j = iSortIndex.Last();  // j is the index to the oldest time completed ie: the last one in the ld_selworkunits table
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
            tbStartStopDate.Text = RemoveCnt(ThisProjectInfo[SortToInfo[i]].strCompletedTime) + "\r\n";  // jys cnt is of form "xx yyy" key on space
            tbStartStopDate.Text += RemoveCnt(ThisProjectInfo[SortToInfo[j]].strCompletedTime) + "\r\n";
            tbStartStopDate.Text += fmtDHMS(Convert.ToInt64(TimeIntervalMinutes * 60.0)) + "\r\n" ;
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

        private void SelectLast(long iSeconds)
        {
            int j,i = lb_SelWorkUnits.Items.Count;
            long tEndComplete, tStartComplete, tDiff;   // not true "start" time as not adding the elapsed time
            btnGTime.Enabled = false;
            btnScatGpu.Enabled = false;
            if (i == 0) return;
            i--;
            lb_SelWorkUnits.ClearSelected();
            lb_SelWorkUnits.SetSelected(i, true);
            tEndComplete = ThisProjectInfo[SortToInfo[i]].time_t_Completed;
            i--;
            for(j = i; j >= 0; j--)
            {
                tStartComplete = ThisProjectInfo[SortToInfo[j]].time_t_Completed;
                tDiff = tEndComplete - tStartComplete;
                if (tDiff >= iSeconds) break;
            }
            if (j < 0) j = 0;   // dont remmber how for loop ends in "C"
            lb_SelWorkUnits.SetSelected(j, true);
            CountSelected();
            RunContinunityCheck();
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
        /// <summary>
        // 9/19/2019 plotting wrong direction.  0 should be present time and 0....5 the 5 would be 5 hours back.  I got it reversed
        /// </summary>
        /// <returns></returns>
        private bool PerformIdleAnalysis()
        {
            int i, j, n;
            long l;
            double d,dd;
            int iRequestedGPU = (cbIdleGpu.SelectedIndex - 1);  // index 0 is "Use all gpu"
            bool bJust1GPU = (iRequestedGPU >= 0);             // index 1 is gpu 0

            if (lb_SelWorkUnits.Items.Count < 2) return false;
            if (lb_SelWorkUnits.SelectedIndices.Count != 2) return false;
            iStart = i = lb_SelWorkUnits.SelectedIndices[0]; // difference between this shows the selection
            iStop = j = lb_SelWorkUnits.SelectedIndices[1];
            n = j - i;
            if (n < 3) return false;  // need to show two segments at least
            CompletionTimes = new List<double>(n + 1);
            IdleGap = new List<double>(n);
            AvgGap = 0;
            // need to make sure the first one valid
            // 1/6/2023 and select only the GPUs wanted
            do
            {
                int k = SortToInfo[i]; 
                if (ThisProjectInfo[k].bState) break;
                if(bJust1GPU)
                {
                    if (ThisProjectInfo[k].iDeviceUsed != iRequestedGPU) continue;
                }
                i++;
                if (i >= j) return false;
            }
            while (true);

            for (n = i; n <= j; n++)
            {
                int k = iSortIndex[n];
                if (!ThisProjectInfo[k].bState) continue;
                if (bJust1GPU)
                {
                    if (ThisProjectInfo[k].iDeviceUsed != iRequestedGPU) continue;
                }
                l = ThisProjectInfo[k].time_t_Completed;
                if (CompletionTimes.Count > 0) //(n > i) jys 9-12-2019 bug if first record is bad
                {
                    dd = l - CompletionTimes.Last();
                    IdleGap.Add(dd);
                    AvgGap += dd;
                }
                CompletionTimes.Add(l);
            }
            if (IdleGap.Count == 0) return false;
            AvgGap /= IdleGap.Count;
            if (AvgGap == 0)
            {
                tb_Info.Text = "All are zero:  no gaps, nothing to plot\r\n";
                return false;
            }
            StdGap = CalcStd(AvgGap, ref IdleGap);
            //the below is NFG as it is the plotting that needs to be fixd
            //IdleGap.Reverse();
            //CompletionTimes.Reverse();
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
            int iRequestedGPU = (cbIdleGpu.SelectedIndex - 1);  // index 0 is "Use all gpu"
            bool bJust1GPU = (iRequestedGPU >= 0);
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
                if (bJust1GPU)
                {
                    if (ThisProjectInfo[k].iDeviceUsed != iRequestedGPU) continue;
                }
                d = ThisProjectInfo[k].dElapsedTime / 60.0;
                IdleGap.Add(d);
                //GrpList.Add(ThisProjectInfo[k].DatasetGroup);
                AvgGap += d;
            }
            if (IdleGap.Count == 0) return false;
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
            else tb_Info.Text = "Probably no data for idle analysis\r\n";
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
            else tb_Info.Text = "Probably no data for ET analysis\r\n";
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


        private void ShowScatter(string strFilter)
        {
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData, "Datasets", cbShowError.Checked, strFilter,0.0);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private void ShowGPUScatter(string strFilter,double dOffset)
        {
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData, "GPUs", cbShowError.Checked, strFilter, dOffset);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }


        private List<cSeriesData> MySeriesData;
        bool FormSeriesFromSets()
        {
            MySeriesData = new List<cSeriesData>();
            int nCon = Convert.ToInt32(nudConCurrent.Value);
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
                            sa.dValues.Add(d / (nCon*60.0));   // probably should just use minutes to start with
                        else
                        {
                            if (bGood)
                                sa.dValues.Add(d / (nCon*60.0));
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
                    sa.nConcurrent = nCon;

                    MySeriesData.Add(sa);
                }
            }
            return bAny;
        }

        // this does not use the select table as all data is used
        bool FormSeriesFromGPUs(long lStart, long lStop, ref double dOffset)
        {
            bool bAny = false;
            int iGpu = 0;
            int nSeries = 0;
            double dMinutes;
            int[] GPUtoSeries = new int[MAXGPUS];
            MySeriesData = new List<cSeriesData>();
            List<int> iGPUsUsed = new List<int>();   // these will be series names
                                                     // need to count devices
            int nCon = Convert.ToInt32(nudConCurrent.Value);
            cSeriesData sa;
            // need to count series first
            foreach (cProjectInfo pi in ThisProjectInfo)
            {
                if(pi.bState || cbShowError.Checked)
                {
                    if (pi.bDeviceUnk && !cbAssignGPU.Checked) continue;
                    if (iGPUsUsed.Contains(pi.iDeviceUsed)) continue;   // already counted it
                    if (pi.time_t_Started >= lStop) continue;
                    if (pi.time_t_Completed <= lStart) continue;
                    iGpu = pi.iDeviceUsed;
                    iGPUsUsed.Add(iGpu);
                    sa = new cSeriesData();
                    sa.strSeriesName = "D" + iGpu.ToString();
                    GPUtoSeries[iGpu] = nSeries;
                    nSeries++;
                    bAny = true;
                    sa.strSysName = CurrentSystem;      // only 1 system as we are not comparing systems
                    sa.strAppName = cb_AppNames.Text;       // usually more than one app this must be first in listview
                    sa.strProjName = CurrentProject;    //only doing one project use this for title info
                    sa.dValues = new List<double>();
                    sa.ShowType = eShowType.DoingGPUs;
                    sa.bIsValid = new List<bool>();
                    sa.dAvgs = 0.0;
                    sa.nConcurrent = nCon;
                    sa.TheseSystems = new List<string>();
                    sa.iTheseSystem = new List<int>();
                    sa.iTheseSystem.Add(0);
                    MySeriesData.Add(sa);
                }
            }
            // now get data
            foreach (cProjectInfo pi in ThisProjectInfo)
            {
                if (pi.bState || cbShowError.Checked)
                {
                    if (pi.time_t_Started >= lStop) continue;
                    if (pi.time_t_Completed <= lStart) continue;
                    if (pi.bDeviceUnk && !cbAssignGPU.Checked) continue;
                    iGpu = pi.iDeviceUsed;
                    //sa = MySeriesData[iGpu];    // true only if all GPUs are in table else not true
                    sa = MySeriesData[GPUtoSeries[iGpu]];

                    dMinutes = pi.dElapsedTime / (nCon * 60.0);
                    sa.dValues.Add(dMinutes);
                    sa.dAvgs += dMinutes;
                    sa.bIsValid.Add(pi.bState);
                }
            }
            dOffset = 0.0;
            foreach(cSeriesData sd in MySeriesData)
            {
                sd.dAvgs /= sd.dValues.Count;
                dOffset = Math.Max(dOffset, sd.dAvgs);
            }
            return bAny;
        }

        private void btnScatSets_Click(object sender, EventArgs e)
        {
            if (FormSeriesFromSets())
                ShowScatter(GetAdvFilter());
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
Amicable
Asteroids@Home
boinc@tacc
climate prediction
collatz
cosmology(g)home
cosmology@home(http)
DENIS@Home
einstein
gaia@home
Gerasim
gpugrid
ithenaComputational
ithenaMeasurements
lhc@home
lhc@home-dev
LODA
milkyway@home
MindModeling@Home
Minecraft@home
MLC@Home
MoolWrapper
nanoHUB@Home
NFS@Home
NumberFields@home
ODLK
ODLK1
PrimeGrid
PrivateGFNServer
QCN
QuChemPedlA@home
Radioactive@Home
RakeSearch
Ralph@Home
Ramanujan Machine
RNAWorld
Rosetta@Home
SiDock
srBase
T.Brada
TN-Grid
Universe(a)Home
WEP-M+2
WUProp@Home
Yafu
yoyo@home


        https://grafana.kiska.pw/d/boinc/boinc?refresh=5m&var-project=milkyway@home
        https://grafana.kiska.pw/d/boinc/boinc?var-project=Asteroids@Home
        https://grafana.kiska.pw/d/boinc/boinc?orgId=1&var-project=Asteroids@Home
        */
        // this really needs to come from all_projects_list.xml
        private void btnLkCr_Click(object sender, EventArgs e)
        {
            string[] strProjNames =  {
                "ilkyway","ommunity","SETI","Rosetta",
                "GPUGRID","Einstein","LHC@home","Asteroids","NumberFields",
                "latinsquares","TN-Grid Platform","ollatz",
                "PrimeGrid","Universe","Bitcoin Utopia", "nfs@home", "enigma",
                "Amicable","SiDock"};
            string[] strGraf = {"milkyway@home", "","","Rosetta@home",
                "gpugrid", "einstein","lhc@home","Asteroids@home", "NumberFields@home",
                "ODLK1", "TN-Grid", "collatz", 
                "PrimeGrid","Universe@Home", "", "NFS@LHome", "",
                "Amicable", "SiDock"
            };
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
                "http://www.enigmaathome.net",
                "https://sech.me/boinc/Amicable",
                "https://www.sidock.si/sidock"
            };

            if (cb_SelProj.Items.Count == 0) return;
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
                    strUrl = strGraf[iIndex];
                    if(strUrl != "")
                    {
                        strUrl = "https://grafana.kiska.pw/d/boinc/boinc?orgId=1&var-project=" + strUrl;
                        GoToSite(strUrl);
                    }
                    bFound = true;
                    break;
                }
                iIndex++;
            }
            if(!bFound)
            {
                tb_Info.Text += "missing url to " + strProj + "\r\n";
            }
            else GoToSite("www.stateson.net/hostprojectstats");
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
            bool bNoPoints = true;
            foreach(cOutFilter cnas in lNAS)
            {
                if(cnas.n > 1)
                {
                    bNoPoints = false;
                    break;
                }
            }
            if (bNoPoints) return;
            double dMax = -1.0;
            int nCon = Convert.ToInt32(nudConCurrent.Value);
            for (int i=iStart; i<=iStop; i++)
            {
                int j = SortToInfo[i];
                cProjectInfo pi = ThisProjectInfo[j];
                if (pi.bState)  // do not use error checked box as times series is not useful with bad numbers
                {
                    dMax = Math.Max(dMax, pi.dElapsedTime);
                }
                else continue;
            }
            timegraph DeviceGraph = new timegraph (ref lNAS, ref ThisProjectInfo, 1+MaxDeviceCount, iStart, iStop,dMax/nCon, ref SortToInfo, nCon);
            DeviceGraph.ShowDialog();
            DeviceGraph.Dispose();
        }

        private void btnClrInfo_Click(object sender, EventArgs e)
        {
            tb_Info.Text = "";
        }

        private string GetAdvFilter()
        {
            if(cbUseAdvFilter.Checked)
            {
                return lblFilterString.Text;
            }
            return "";
        }

        private void btnScatGpu_Click(object sender, EventArgs e)
        {
            long tEnd = ThisProjectInfo[SortToInfo[iStop]].time_t_Completed;
            long tStart = ThisProjectInfo[SortToInfo[iStart]].time_t_Started;
            double dOffset = 0.0;
            if (FormSeriesFromGPUs(tStart, tEnd, ref dOffset))
                ShowGPUScatter(GetAdvFilter(), dOffset);
        }

        private void SaveLast()
        {
            Properties.Settings.Default.LastFiles = lbLastFiles.Text ;
        }

        private void btnLastHour_Click(object sender, EventArgs e)
        {
            SelectLast(3600);
        }

        private void btnLastDay_Click(object sender, EventArgs e)
        {
            SelectLast(24 * 3600);
        }

        private void btn2hr_Click(object sender, EventArgs e)
        {
            SelectLast(3600 * 2);
        }

        private void btn4hr_Click(object sender, EventArgs e)
        {
            SelectLast(3600 * 4);
        }

        private void btn8hr_Click(object sender, EventArgs e)
        {
            SelectLast(3600 * 8);
        }

        // go through the data base and change all unknown gpus to the new value
        private void ReassignAll_GPUs()
        {
            foreach(cProjectInfo pi in ThisProjectInfo)
            {
                if(pi.bDeviceUnk)
                {
                    if (GpuReassignment.ReassignedGPU >= 0)
                    {
                        pi.iDeviceUsed = GpuReassignment.ReassignedGPU;
                    }
                    else pi.iDeviceUsed = pi.iSaveDeviceUsed;
                }
            }
        }

        private void btnAdvFilter_Click(object sender, EventArgs e)
        {
            int GPUreassignd = GpuReassignment.ReassignedGPU; // make a note of the one last used
            AdvFilter adfForm = new AdvFilter(ref MyAdvFilter, ref GpuReassignment);
            adfForm.ShowDialog();
            adfForm.Dispose();
            if(MyAdvFilter.strPhrase != "")
            {
                string strTemp = "select datasets where data name " + (MyAdvFilter.bContains ? "contains " : " does not contain") + " the phrase " + MyAdvFilter.strPhrase;
                lblFilterString.Text = strTemp;
                cbUseAdvFilter.Enabled = true;
            }
            else
            {
                cbUseAdvFilter.Checked = false;
                cbUseAdvFilter.Enabled = false;
            }
            cbUseAdvFilter.Checked |= MyAdvFilter.bOKreturn;
            if(GPUreassignd != GpuReassignment.ReassignedGPU)
            {
                ReassignAll_GPUs();
            }
            tb_Results.Text = "";
        }
        
        private double GetOutlierFilter(int iGpu)
        {
            int n = cbFilterSTD.SelectedIndex;
            double[] Vals = { 0.25, 0.333, 0.5, 0.0, 1.0, 2.0, 3.0 };
            if (dgvOF.RowCount == 0) return 0.0;
            if(dgvOF[1, iGpu].Value == null)return 0.0;
            if ((bool)dgvOF[1,iGpu].EditedFormattedValue)
            {
                return Vals[n];
            }
            
            return 0.0;
        }

        private void cbFilterSTD_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunFilter();
        }
    }    
}
/*Project	Application	Version Number	Name	PlanClass	Elapsed Time Cpu	Elapsed Time Gpu	State	ExitStatus	Reported time	Completed time	Use	Received	VMem	Mem
 * -------------------------------------------------------------------------------------------------ES---Reported---Completed---use-Received
482	World Community Grid	Mapping Cancer Markers	743	MCM1_0153882_1609_0		22122	22115	5	0	1567809502	1567807942		1567531363.832855	78569472.000000	37664686.658060	-1x		-1x		-1x		-1x		-1x	
483	World Community Grid	Mapping Cancer Markers	743	MCM1_0153918_9343_1		23901	23877	5	0	0	1567810582		1567531363.832855	80019456.000000	39214027.476951	-1x		-1x		-1x		-1x		-1x	
484	World Community Grid	Mapping Cancer Markers	743	MCM1_0153919_8085_1		23976	23959	5	0	0	1567811362		1567531363.832855	80003072.000000	40076038.203909	-1x		-1x		-1x		-1x		-1x	
485	World Community Grid	Mapping Cancer Markers	743	MCM1_0153926_6242_1		23535	23519	2	0	0	0		1567531363.832855	80228352.000000	39406757.418836	-1x		-1x		-1x		-1x		-1x	
487	World Community Grid	Mapping Cancer Markers	743	MCM1_0153924_2059_0		23350	23328	2	0	0	0		1567531363.832855	80228352.000000	39444388.646863	-1x		-1x		-1x		-1x		-1x	
489	World Community Grid	Mapping Cancer Markers	743	MCM1_0153882_1551_1		21823	21801	5	0	0	1567810582		1567531363.832855	78565376.000000	37634012.999719	-1x		-1x		-1x		-1x		-1x	
482 is good, 3,4 bad as 0 is not a valid start time 5 and 7 are bad as start and stop are both 0 and 9 is also bad
not that the exit status is all 0 or "good" 
https://www.epochconverter.com/timezones?q=1569287308&tz=America%2FChicago
*/
