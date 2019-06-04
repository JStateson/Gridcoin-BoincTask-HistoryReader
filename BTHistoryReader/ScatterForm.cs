﻿using System;
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
        private int CurrentNumberSeriesDisplayed = 0;
        private int dot_1_offset_y = 34;    // where marker of first system name is
        private int dot_1_offset_x = 516;    // how far across the 640x320 chart
        private int dot_y_space = 15;           // this needs to be 15 so use kluge1

        private List<Point> SeriesMarkers;

        // if you resize the chart you must re-calculate the above number
        private void WasHereLastTime(int nSystems)
        {
            SeriesMarkers = new List<Point>();
            for(int i = 0; i < nSystems;i++)
            {
                Point p = new Point();
                p.Y = dot_1_offset_y + i * dot_y_space;
                p.X = dot_1_offset_x;
                SeriesMarkers.Add(p);
            }
        }

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
            public Color rgb;
        }

        List<cColoredLegends> MyLegendNames;

        private void FillInSeriesLegends()
        {
            int n = ThisSeriesData.Count;
            MyLegendNames = new List<cColoredLegends>();
            cColoredLegends cl = new cColoredLegends();
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
                MyLegendNames.Add(cl);
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
            string strOut = " secs";
            dOut = d;
            if (d < 120.0) return strOut;
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
            CurrentNumberSeriesDisplayed = ThisSeriesData.Count;
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
        }

        // click anywhere and if any are disabled then enable them (make visibls)
        // click near the series marker then hide all the others series.
        // this was no good when legend text wrapped
        private void ChartScatter_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            //int j = AreWeClose(e.Location); // set breakpoint here and made a note of x,y to adjust the "kluge's"
            
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
            }
            DrawShowingText(i);
            ShowHideSeries(i);
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