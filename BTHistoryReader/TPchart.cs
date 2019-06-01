using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BTHistoryReader
{
    public partial class TPchart : Form
    {
        private List<long> ct;      // completion times in seconds since 1970
        private List<double> it;    // idle time in seconds
        private long WORKctStart;   // start time of WORKct in event it is normalized to 0 
        private int iLastIndex;     // index of last time that is <= the lookback limit
        private long[] WORKct;      // completion times in seconds since 1970
        private double[] WORKit;    // idle time in seconds
        private int ValidWork;
        private double AvgGap;
        private double StdGap;
        private double SigGap;
        private int iSig;
        private Series sCT = new Series("CompletionTime");
        private Series sET = new Series("ElapsedTime");
        static bool bDoingHist = false;
        private long LKBhours = 24;
        private int iBinCnt = 2;   // 2 (show in spinner box) raised to power of 1 (spinner value) 
        private string strStartDT;  // date and time started (x axis origin)
        private double dFirst, dLast;
        List<double> xAxis;
        List<double> yAxis;

        private void ShowOrHideHist(bool bValue)
        {
            lbBinSize.Visible = bValue;
            tbSpinBinValue.Visible = bValue;
            SpinBin.Visible = bValue;
            bDoingHist = bValue;
            nudConcur.Visible = bValue;
            labConcur.Visible = bValue;
            gboxLKBACK.Visible = !bValue;
        }

        // ctStart if > 0 then normalizing
        private void SaveWorking(int n, long ctStart)
        {
            int i;
            WORKit = new double[n];
            WORKct = new long[n];
            WORKctStart = ct[0];
            for (i = 0; i < n; i++)
            {
                WORKct[i] = ct[i] - ctStart;
                WORKit[i] = it[i];
            }
            ValidWork = n;
        }

        // used by idle plot and returns time in seconds
        private long SetLastTimeDisplayed()
        {
            long tMax = LKBhours * 3600;
            int i = 0;
            for (i = 0; i < ValidWork; i++)
            {
                if (WORKct[i] >= tMax) break;
            }
            if (i >= ValidWork)
                i = ValidWork - 1;
            iLastIndex = i;
            return WORKct[i];
        }

        // bin original data then put into working area
        // iBinCnt is the number of bins
        private int NewBins()
        {
            int iPtr=0;   // traverse the entire array
            int iCnt=0;   // number in the current transversal (0..iBinCnt-1)
            int jBinCnt = 0;   //traversal for the working bin
            int nInBin=0; // actual number in bin
            int nExpected = 1 +( ct.Count / iBinCnt);
            if (nExpected < 2) nExpected = 2;   // put this many in each bin
            WORKct = new long[iBinCnt+1];
            WORKit = new double[iBinCnt+1];
            do
            {
                if (iCnt == 0)
                {
                    WORKct[jBinCnt] = 0;
                    WORKit[jBinCnt] = 0;
                    nInBin = 0;  // need to count actual since last bin may not be full;
                }
                WORKct[jBinCnt]+= ct[iPtr];
                WORKit[jBinCnt]+= it[iPtr];
                iPtr++;
                iCnt++;
                nInBin++;
                if(iCnt == nExpected || iPtr == ct.Count)
                {
                    WORKct[jBinCnt] /= nInBin;
                    iCnt = 0;   // start next bin
                    jBinCnt++;
                    nInBin = 0;
                    if (iPtr == ct.Count) break;
                }
            } while (true);
            ValidWork = WORKct.Length;
            for(int i = WORKct.Length-1; i >= 0;i--)
            {
                if (WORKct[i] == 0)
                    ValidWork--;
            }
            // could have a 0 in the last bin due to my expedient algorithm
            return ValidWork;
        }

        public TPchart(ref List<long> refCT, ref List<double> refIT, double rAvgGap, double rStdGap, string strProject)
        {
            int i;
            InitializeComponent();
            AvgGap = rAvgGap;
            StdGap = rStdGap;
            ct = refCT;
            it = refIT;
            lbl_sysname.Text = "System: " + strProject;
            ShowOrHideHist(AvgGap == -1);
            long iDT = 0;

            labStartTime.Visible = (AvgGap != -1);  // only for plot of idle time

            if (AvgGap != -1)
            {   // plotting idle time
                SaveWorking(ct.Count-1, ct[0]); // since it is difference between ct then there is always one less item
                gboxFilter.Text = "Detail Filter (0 is all)";
                this.Text = "Idle Plot";
                i = Convert.ToInt32(StdGap / AvgGap);
                DetailFilter.ValueChanged -= new System.EventHandler(this.DetailFilter_ValueChanged);
                if (i < 1) DetailFilter.Value = 0;
                else if (i < 2) DetailFilter.Value = 1;
                DetailFilter.ValueChanged += new System.EventHandler(this.DetailFilter_ValueChanged);
                iSig = Convert.ToInt32(DetailFilter.Value);
                SetStartTime(WORKctStart);
                DrawStuff();
                return;
            }
            // plotting elapsed time
            this.Text = "Elapsed Time";
            gboxFilter.Text = "Visual Scaling";
            SaveWorking(ct.Count,0);
            toolTip1.SetToolTip(DetailFilter, "Change x-axis scale");
            lbSpinFilter.Text = "adj xAxis scale";
            DrawHist();
        }

        private void SetStartTime(long n)
        {
            System.DateTime dt_1970 = new System.DateTime(1970, 1, 1);
            System.DateTime dt_this = DateTime.SpecifyKind(dt_1970.AddSeconds(n), DateTimeKind.Utc);
            labStartTime.Text = "Start: " +  dt_this.ToLocalTime().ToString();
        }

        private void GetLKHours()
        {
            LKBhours = Convert.ToInt64(cbHours.Text);
            if (LKBhours < 1) LKBhours = 1;
        }

        // used for x axis only
        private double GetBestScaleingBottom(double a)
        {
            if (a < 100) return 0;
            if (a < 1000) return 100;
            return 1000;
        }

        // used for x axis only
        private double GetBestScaleingUpper(double a)
        {
            double r = 1.0 / (1 + iSig);
            if (a < 10) return Math.Max(a, 10*r);
            if (a < 100) return Math.Max(a,100*r);
            if (a < 1000) return Math.Max(a,1000*r);
            return a;
        }

        // used by idle to truncate outliers when project off line a long time
        private double GetBestYscale(double a)
        {
            double r = 1.0;
            if (a < 10) return Math.Max(a, 10 * r);
            if (a < 100) return Math.Max(a, 100 * r);
            if (a < 1000) return Math.Max(a, 1000 * r);
            if (a < 10000) return Math.Max(a, 10000 * r);
            return a;
        }

        // resize time by number of concurrent tasks
        double dConcurrent = 1;
        long iScaleTime(long l)
        {
            double d = Convert.ToDouble(l) / dConcurrent;
            return Convert.ToInt64( Math.Round(d));
        }

        private void DrawHist()
        {
            int i, n;
            double d;
            dConcurrent = Convert.ToDouble(nudConcur.Value);
            xAxis = new List<double>();
            yAxis = new List<double>();
            chart1.Series.Add(sET);
            chart1.Series["ElapsedTime"].AxisLabel = "";
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Elapsed Time(sec)";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Number Samples";
            lbChart.Text = "Distribution of elapsed time";
            n = ValidWork; // WORKct.Length ;
            for(i = 0; i < n; i++)
            {
                xAxis.Add(iScaleTime(WORKct[i]));
                yAxis.Add(WORKit[i]);
            }
            dLast = GetBestScaleingUpper(xAxis.Last());
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dLast;
            dFirst = GetBestScaleingBottom(xAxis.First());
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dFirst;
            d = dLast - dFirst;
            d = Math.Floor(Math.Log10(d));
            n = Convert.ToInt32(Math.Pow(10, d));
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = n; 
            //chart1.ChartAreas["ChartArea1"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.Series["ElapsedTime"].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
        }


        private double GetSigGap()
        {
            if (iSig == 0) return 0;
            return AvgGap + ((iSig-1) * StdGap);
        }


        // this is a plot (histogram) of the gap in completion time
        // if the elapsed time is smaller than the gap, then the gap is project idle time
        //1-jun-2019: simplflying and bug fix as outliers can hide all useful data when project down for hours.
        private void DrawStuff()
        {
            xAxis = new List<double>();
            yAxis = new List<double>();
            double d;
            double tMinutes;    // total minutes
            double tHours;
            long tSecs, tMax;
            double dSum = 0;
            int iStartIndex;
            long iStartValue;
            int i, n = ct.Count -1;

            chart1.Series.Add(sCT);
            chart1.Series["CompletionTime"].LegendText = "Idle Time (minutes)";

            //normalized(already) and stop after 24 or specified hours.  probably could use binary search here
            tSecs = SetLastTimeDisplayed();
            tMinutes = (tSecs / 60.0);
            tHours = tSecs / 3600;
            SigGap = GetSigGap();
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = (tHours == 0) ? 1 : tHours;
            TryTruncate();

            //chart1.Series["CompletionTime"].MarkerSize = 1;
            // chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = true;


            for ( i = 0; i < iLastIndex; i++)
            {
                d = Convert.ToDouble(WORKct[i]);
                d /= 3600;  // xAxis to be hours behind us
                xAxis.Add(d);
                d = WORKit[i];  // seconds between completions "idle " time 
                if (d < SigGap) d = 0; 
                d = d / 60.0;
                // log scale is screwed with 0, not worth fixing
                yAxis.Add(d);    // want minutes for y axis
            }

            chart1.Series["CompletionTime"].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
        }

        private void DrawIdleChanged()
        {
            chart1.Series.Remove(sCT);
            DrawStuff();
        }

        private void DrawHistChanged()
        {
            chart1.Series.Remove(sET);
            if (NewBins() > 0)
                DrawHist();
        }

        private void DetailFilter_ValueChanged(object sender, EventArgs e)
        {
            double d;
            int n;
            iSig = Convert.ToInt32(DetailFilter.Value);
            if (bDoingHist)
            {
                dLast = GetBestScaleingUpper(xAxis.Last());
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dLast;
                dFirst = GetBestScaleingBottom(xAxis.First());
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dFirst;
                d = dLast - dFirst;
                d = Math.Floor(Math.Log10(d));
                n = Convert.ToInt32(Math.Pow(10, d));
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = n;
                return;
            }
            //TODO: the following needs to be change to avoid a total recalculate
            DrawIdleChanged();
        }

        private void TPchart_FormClosing(object sender, FormClosingEventArgs e)
        {
            chart1.Series.Remove(sCT);
            chart1.Series.Remove(sET);
        }

        private void cbHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLKHours();
            if (bDoingHist)
            {
                DrawHistChanged();
            }
            else
            {
                DrawIdleChanged();
            }
        }

        private void SpinBin_ValueChanged(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(SpinBin.Value);
            int j = Convert.ToInt32( Math.Pow(2.0, i));
            tbSpinBinValue.Text = j.ToString();
            iBinCnt = j;
            if (bDoingHist) DrawHistChanged();
            else DrawIdleChanged();
        }

        private void cboxLOG_CheckedChanged(object sender, EventArgs e)
        {
            //chart1.ChartAreas["ChartArea1"].AxisY.IsLogarithmic = cboxTruncate.Text
        }


        private double FindLargest()
        {
            SetLastTimeDisplayed();    // search no further than this
            double dBig = -1;
            for(int i = 0; i < iLastIndex; i++)
            {
                dBig = Math.Max(dBig, WORKit[i]);
            }
            return dBig;
        }

        private void TryTruncate()
        {
            if (cboxTruncate.Checked)
            {
                double dMinutesTrunc = Convert.ToDouble(tBoxTruncValue.Text);
                double dBiggie = FindLargest();
                if (dBiggie > 60 * dMinutesTrunc)
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(dMinutesTrunc);
                else
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = double.NaN;
            }
            else
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = double.NaN;
        }

        private void cboxTruncate_CheckedChanged(object sender, EventArgs e)
        {
            //double dlast = chart1.ChartAreas["ChartArea1"].AxisY.Maximum;
            TryTruncate();
        }

        private void nudConcur_ValueChanged(object sender, EventArgs e)
        {
            if (bDoingHist) DrawHistChanged();
            else DrawIdleChanged();
        }
    }
}
