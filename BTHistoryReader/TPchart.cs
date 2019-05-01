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
        private double AvgGap;
        private double StdGap;
        private double SigGap;
        private int iSig;
        private Series sCT = new Series("CompletionTime");
        private Series sET = new Series("ElapsedTime");
        private bool bDoingHist = false;

        public TPchart(ref List<long> refCT, ref List<double> refIT, double rAvgGap, double rStdGap, string strProject)
        {
            int i;
            InitializeComponent();
            AvgGap = rAvgGap;
            StdGap = rStdGap;
            ct = refCT;
            it = refIT;
            lbl_sysname.Text = "System: " + strProject;
            bDoingHist = (AvgGap == 0);
            if (AvgGap != 0.0)
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
            DrawHist();
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
            n = ct.Count;
            for(i = 0; i < n; i++)
            {
                xAxis.Add(ct[i]);
                yAxis.Add(it[i]);
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


            // go back 24 hours maximum
            tMax = 24 * 3600;
            tSecs = ct[n - 1];
            for(iStartIndex=0;iStartIndex<n;iStartIndex++)
            {
                if ((tSecs - ct[iStartIndex]) <= tMax) break;
            }


            //chart1.Series["CompletionTime"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            tSecs = ct[n] - ct[iStartIndex];
            tMinutes = (tSecs / 60.0);
            tHours = tSecs / 3600;
            SigGap = AvgGap + (iSig * StdGap);
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

        private void DetailFilter_ValueChanged(object sender, EventArgs e)
        {
            iSig = Convert.ToInt32(DetailFilter.Value);
            if(bDoingHist)
            {
                chart1.Series.Remove(sET);
                DrawHist();
                return;
            }
            chart1.Series.Remove(sCT);
            DrawStuff();
        }

        private void TPchart_FormClosing(object sender, FormClosingEventArgs e)
        {
            chart1.Series.Remove(sCT);
            chart1.Series.Remove(sET);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
