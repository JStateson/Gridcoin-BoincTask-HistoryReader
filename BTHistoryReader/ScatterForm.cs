using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTHistoryReader
{
    public partial class ScatterForm : Form
    {

        List<cSeriesData> ThisSeriesData;

        private string SeriesName = "";
        private double dBig = -1;
        double dSmall = 1e6;
        private string strSeries = "Elapsed Time in ";
        private string strUnits = "secs";

        public ScatterForm(ref List<cSeriesData> refSD)
        {
            InitializeComponent();
            ThisSeriesData = refSD;
            ShowScatter();
        }

        private double GetBestScaleingBottom(double a)
        {
            if (a < 100) return 0;
            if (a < 1000) return 100;
            return 1000;
        }

        private double GetBestScaleingUpper(double a)
        {
            int iSig = (int) nudXscale.Value;
            double r = 1.0 / (1 + iSig);
            if (a < 10) return Math.Max(a, 10 * r);
            if (a < 100) return Math.Max(a, 100 * r);
            if (a < 1000) return Math.Max(a, 1000 * r);
            return a;
        }

        // use (dOut / d) to scale
        private string BestTimeUnits(double d, ref double dOut)
        {
            string strOut = " secs";
            dOut = d;
            if (d < 60.0) return strOut;
            strOut = " mins";
            dOut /= 60.0;
            if (dOut < 60) return  strOut;
            strOut = " hours";
            dOut /= 60.0;
            if (dOut < 24) return  strOut;
            dOut /= 24.0;
            return " days";
        }

        /*
         * if elapsed time is over 60 seconds, scale time to minutes
         * same for minutes and hours
        */

        private string SetMinMax(ref double f)
        {
            double d = 0;
            dSmall = 1e6;
            dBig = -1;
            foreach(cSeriesData sd in ThisSeriesData)
            {
                if (sd.dSmall < dSmall) dSmall = sd.dSmall;
                if (sd.dBig > dBig) dBig = sd.dBig;                
            }
            string strUnits = BestTimeUnits(dBig, ref d);
            f = d / dBig;
            dSmall *= f;
            dBig = d;
            return strUnits;
        }

        // draw points vertical one over the other from 1 to n but normalzed to 0.0 to 1.0
        // the x axis position is the elapsed time.
        private void ShowScatter()
        {
            double d=0;
            double f = 0;
            string strUnits = SetMinMax(ref f);
            foreach (cSeriesData sd in ThisSeriesData)
            {
                int n = sd.dValues.Count;
                List<double> yAxis = new List<double>(n);
                List<double> xAxis = new List<double>(n);
                for (int i = 0; i < n; i++)
                {
                    d = Convert.ToDouble(i) / n;
                    yAxis.Add(d);
                    d = sd.dValues[i];
                    xAxis.Add(d * f);
                }

                string seriesname = sd.bIsShowingApp ? sd.strAppName : sd.strSystemName;
                SeriesName = seriesname;
                ChartScatter.Series.Add(seriesname);
                ChartScatter.Series[seriesname].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                ChartScatter.Series[seriesname].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
            }

            ChartScatter.Legends["Legend1"].Title = strSeries + strUnits;
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Minimum = GetBestScaleingBottom(dSmall);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "#.#";
        }

        private void nudXscale_ValueChanged(object sender, EventArgs e)
        {
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
            return;
        }
    }
}