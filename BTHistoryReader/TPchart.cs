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
        private List<long> ct;    // completion times in seconds since 1970
        private List<double> it;  // idle time in seconds
        private long[] WORKct;    // completion times in seconds since 1970
        private double[] WORKit;  // idle time in seconds
        private int ValidWork;
        private double AvgGap;
        private double StdGap;
        private double SigGap;
        private int iSig;
        private Series sCT = new Series("CompletionTime");
        private Series sET = new Series("ElapsedTime");
        private bool bDoingHist = false;
        private long LKBhours = 24;
        private int iBinCnt = 2;   // 2 (show in spinner box) raised to power of 1 (spinner value) 

        private void ShowOrHideHist(bool bValue)
        {
            lbBinSize.Visible = bValue;
            tbSpinBinValue.Visible = bValue;
            SpinBin.Visible = bValue;
            bDoingHist = bValue;
            cbHours.Enabled = !bValue;
        }

        private void SaveWorking(int n)
        {
            int i;
            WORKit = new double[n];
            WORKct = new long[n];
            for (i = 0; i < n; i++)
            {
                WORKct[i] = ct[i];
                WORKit[i] = it[i];
            }
            ValidWork = n;
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
            ShowOrHideHist(AvgGap == 0);
            
            if (AvgGap != -1)
            {
                i = Convert.ToInt32(StdGap / AvgGap);
                DetailFilter.ValueChanged -= new System.EventHandler(this.DetailFilter_ValueChanged);
                if (i < 1) DetailFilter.Value = 0;
                else if (i < 2) DetailFilter.Value = 1;
                DetailFilter.ValueChanged += new System.EventHandler(this.DetailFilter_ValueChanged);
                iSig = Convert.ToInt32(DetailFilter.Value);
                DrawStuff();
                return;
            }
            SaveWorking(ct.Count);
            toolTip1.SetToolTip(DetailFilter, "Change x-axis scale");
            lbSpinFilter.Text = "adj xAxis scale";
            DrawHist();
        }

        private void GetLKHours()
        {
            LKBhours = Convert.ToInt64(cbHours.Text);
            if (LKBhours < 1) LKBhours = 1;
        }

        private double GetBestScaleingBottom(double a)
        {
            if (a < 100) return 0;
            if (a < 1000) return 100;
            return 1000;
        }

        private double GetBestScaleingUpper(double a)
        {
            double r = 1.0 / (1 + iSig);
            if (a < 10) return Math.Max(a, 10*r);
            if (a < 100) return Math.Max(a,100*r);
            if (a < 1000) return Math.Max(a,1000*r);
            return a;
        }

        private void DrawHist()
        {
            int i, n;
            double d, dFirst, dLast ;
            List<double> xAxis = new List<double>();
            List<double> yAxis = new List<double>();
            chart1.Series.Add(sET);
            chart1.Series["ElapsedTime"].AxisLabel = "";
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Elapsed Time(sec)";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Number Samples";
            lbChart.Text = "Distribution of elapsed time";
            n = ValidWork; // WORKct.Length ;
            for(i = 0; i < n; i++)
            {
                xAxis.Add(WORKct[i]);
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
        private void DrawStuff()
        {
            List<double> xAxis = new List<double>();
            List<double> yAxis = new List<double>();
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
            double tStart = ct[0];
            double tStop = ct[n];


            tMax = LKBhours * 3600;
            tSecs = ct[n - 1];
            for(iStartIndex=0;iStartIndex<n;iStartIndex++)
            {
                if ((tSecs - ct[iStartIndex]) <= tMax) break;
            }


            //chart1.Series["CompletionTime"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            tSecs = ct[n] - ct[iStartIndex];
            tMinutes = (tSecs / 60.0);
            tHours = tSecs / 3600;
            SigGap = GetSigGap();
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = (tHours == 0) ? 1 : tHours;
            //chart1.Series["CompletionTime"].MarkerSize = 1;
            // chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = true;

            for ( i = iStartIndex; i < n; i ++)
            {
                d = tStop - ct[i];
                d /= 3600;  // xAxis to be hours behind us
                xAxis.Add(d);
                d = it[i];  // seconds between completions "idle " time 
                if (d < SigGap) d = 0;
                d = d / 60.0;
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
            if(NewBins() > 0)
                DrawHist();
        }

        private void DetailFilter_ValueChanged(object sender, EventArgs e)
        {
            iSig = Convert.ToInt32(DetailFilter.Value);
            if(bDoingHist)
            {
                DrawHistChanged();
                return;
            }
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
            DrawIdleChanged();
        }

        private void SpinBin_ValueChanged(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(SpinBin.Value);
            int j = Convert.ToInt32( Math.Pow(2.0, i));
            tbSpinBinValue.Text = j.ToString();
            iBinCnt = j;
            DrawHistChanged();
        }
    }
}
