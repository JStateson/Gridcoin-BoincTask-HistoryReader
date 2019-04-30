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
        private List<long> ct;    // completion times
        private List<double> it;    // idle times
        private double AvgGap;
        private double StdGap;
        private double SigGap;
        private int iSig;
        private Series sCT = new Series("CompletionTime");

        public TPchart(ref List<long> refCT, ref List<double> refIT, double rAvgGap, double rStdGap)
        {
            InitializeComponent();
            ct = refCT;
            it = refIT;
            AvgGap = rAvgGap;
            StdGap = rStdGap;
            iSig = Convert.ToInt32(DetailFilter.Value);
            DrawStuff();
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
            // chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = true;
            for ( i = iStartIndex; i < n; i ++)
            {
                d = tStop - ct[i];
                d /= 3600;
                xAxis.Add(d);
                d = it[i];
                if (d < SigGap) d = 0;
                yAxis.Add(d / 60.0);
            }

            chart1.Series["CompletionTime"].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
        }

        private void DetailFilter_ValueChanged(object sender, EventArgs e)
        {
            iSig = Convert.ToInt32(DetailFilter.Value);
            chart1.Series.Remove(sCT);
            DrawStuff();
        }
    }
}
