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
    public partial class ScatterForm : Form
    {

        List<cSeriesData> ThisSeriesData;

        private string SeriesName = "";
        private double dBig = -1;
        double dSmall = 1e6;
        private string strSeries = "Elapsed Time in ";
        // note to myself, elspased time is always in minutes
        private int CurrentNumberSeriesDisplayed = 0;
        private bool bScatteringApps;
  

        private double DistanceTo(Point point1, Point point2)
        {
            var a = (double)(point2.X - point1.X);
            var b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }
       
        public ScatterForm(ref List<cSeriesData> refSD)
        {
            InitializeComponent();
            ThisSeriesData = refSD;
            ShowScatter();
            GetLegendInfo.Enabled=true;
        }

        class cColoredLegends
        {
            public string strName;
            public string strSubItems;  // for now, we are only showing projects, not apps
            public Color rgb;
        }

        List<cColoredLegends> MyLegendNames;
        List<DataPoint> SavedColoredPoints;
        private void FillInSeriesLegends()
        {
            int n = ThisSeriesData.Count;
            MyLegendNames = new List<cColoredLegends>();
            cColoredLegends cl = new cColoredLegends();
            SavedColoredPoints = new List<DataPoint>();
            ChartScatter.ApplyPaletteColors();
            cl.strName = "All Series";
            cl.rgb = Color.Black;
            MyLegendNames.Add(cl);
            foreach(cSeriesData sd in ThisSeriesData)
            {
                string seriesname = sd.bIsShowingApp ? sd.strAppName : sd.strSystemName;
                cl = new cColoredLegends();
                cl.strName = seriesname;
                cl.rgb = ChartScatter.Series[seriesname].Color;
                if(sd.bIsShowingApp)
                {
                    // reserve for possibly subsystems when scattering apps
                }
                else
                {
                    // append all systems here (used lbox instead)
                }
                MyLegendNames.Add(cl);
            }
            for(int i = 0; i < ThisSeriesData.Count; i++)
            {
                DataPoint p = new DataPoint();
                p.Color = ChartScatter.Series[i].Points[0].Color;
                SavedColoredPoints.Add(p);  // want to restore this color
            }
            // allow 1 more than actual so we can wrap back to 0
            nudShowOnly.Maximum = n + 1;
            DrawShowingText(0);
        }


        private void DrawShowingText(int i)
        {
            tboxShowing.Text = MyLegendNames[i].strName;
            tboxShowing.ForeColor = MyLegendNames[i].rgb;
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
            string strOut = " mins";
            dOut = d;
            if (d < 60.0) return strOut;
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
            CurrentNumberSeriesDisplayed = ThisSeriesData.Count;
            bScatteringApps = ThisSeriesData[0].bIsShowingApp;
            if(!bScatteringApps)
            {
                lviewSubSeries.Items.Add(ThisSeriesData[0].strAppName);
            }
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
                ChartScatter.Series[seriesname].ChartType = SeriesChartType.Point;
                ChartScatter.Series[seriesname].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
            }

            ChartScatter.Legends["Legend1"].Title = strSeries + strUnits;

            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Minimum = GetBestScaleingBottom(dSmall);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "#.#";
        }

        // setup the expected colors for systems for the selcted app


        List<Color> MyColors = new List<Color>();
        List<int> ExpectedOffset = new List<int>();
        private void SetSysColors(int s)
        {
            int n = ThisSeriesData[s].TheseSystems.Count;
            Random rnd = new Random();
            MyColors.Clear();
            ExpectedOffset.Clear();
            for (int i = 0; i < n; i++)
            {
                ListViewItem itm = new ListViewItem();
                itm.Text = ThisSeriesData[s].TheseSystems[i];
                itm.ForeColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                lviewSubSeries.Items.Add(itm);
                MyColors.Add(itm.ForeColor);
                ExpectedOffset.Add(ThisSeriesData[s].iTheseSystem[i]);
            }
        }

        // for the series index s put the names of the systems into the list box
        // and arrange to show the different colors (if any)
        private void ShowSystemNames(int s)
        {
            lviewSubSeries.Items.Clear();
            SetSysColors(s);
            int n = ExpectedOffset.Count;
            if(n == 1)
            {
                // no need to get random color, reuse the default
                return;
            }
            for (int i = 0; i < ThisSeriesData[s].dValues.Count; i++)
            {
                int x = ThisSeriesData[s].iSystem[i];   // index to system for this point
                DataPoint p = ChartScatter.Series[s].Points[i];
                for(int j = 0; j < n; j++)
                {
                    if( ExpectedOffset[j] == x)
                    {
                        p.Color = MyColors[j];
                        break;
                    }
                }
            }
        }

        private void RestoreDefaultColors()
        {
            int n = ThisSeriesData.Count;
            for(int i = 0; i <n; i ++)
            {
                Color c = SavedColoredPoints[i].Color;; 
                foreach (DataPoint p in ChartScatter.Series[i].Points)
                {
                    p.Color = c;
                }
                ChartScatter.Series[i].Color = c;
            }
        }

        private void nudXscale_ValueChanged(object sender, EventArgs e)
        {
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
        }
 

        private void GetLegendInfo_Tick(object sender, EventArgs e)
        {
            GetLegendInfo.Enabled = false;
            //FindLegend();
            if(ThisSeriesData.Count == 1)
            {
                labelShowSeries.Visible = false;
                tboxShowing.Visible = false;
                nudShowOnly.Visible = false;
                return;
            }
            FillInSeriesLegends();
        }
    
        private void ShowHideSeries(int j)
        {
            int n = ThisSeriesData.Count;

            if (j == 0 )
            {
                for (int i = 0; i < n; i++)
                {
                    ChartScatter.Series[i].Enabled = true;
                }
            }
            else 
            {
                j--;
                for (int i = 0; i < n; i++)
                {
                    ChartScatter.Series[i].Enabled = (i == j);
                }
            }
        }

        private void nudShowOnly_ValueChanged(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(nudShowOnly.Value);
            int j = MyLegendNames.Count;
            if (i == j)
            {
                i = 0;  // wrap back to 0
                nudShowOnly.Value = 0;
                if(bScatteringApps)
                    lviewSubSeries.Items.Clear();    // do not clear if scatttering projects
            }
            DrawShowingText(i);
            ShowHideSeries(i);
            if (i == 0  | !bScatteringApps)
            {
                RestoreDefaultColors();
                return; // not showing individual series nor scattering apps
            }
            ShowSystemNames(i-1);
        }

        private void nudXscale_ValueChanged_1(object sender, EventArgs e)
        {
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
        }

        private void cboxUseLog_CheckedChanged(object sender, EventArgs e)
        {
            double d = GetBestScaleingBottom(dSmall);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Minimum = cboxUseLog.Checked ? Math.Max(0.1, d) : d ;
            ChartScatter.ChartAreas["ChartArea1"].AxisX.IsLogarithmic = cboxUseLog.Checked;
            if(!cboxUseLog.Checked)
            {
                ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
            }
        }
    }
}