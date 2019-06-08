using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int CurrentNumberSeriesDisplayable = 0; // number available to view
        private int CurrentSeriesDisplayed = -1;        // the one being shown IFF only one series is shown else -1
        private bool bScatteringApps;
        private bool bShowSystemData;         // scattering systems
        double fScaleMultiplier;
        string strScaleUnits;

        private class cSaveOutlier
        {
            public string seriesname;
            public int iWhereRemoved;   // where point on screen was declared an outlier
            public int iWhereData;      // where data was declared an outlier
            public double fmpy;         // the restore multiplier
            public Color c;             // save original color
        }
        private Stack<cSaveOutlier> UsedOutliers;

        private double DistanceTo(Point point1, Point point2)
        {
            var a = (double)(point2.X - point1.X);
            var b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }



        // if showing only project data only the project changes. 
        // the application is the same for all project so it is homogeneous
        // and there is no need to refresh the default colors so
        // i can avoid that strange side effect of changing point colors
        // side effect discussed here 
        // https://stackoverflow.com/questions/56484451/unexpected-side-effect-setting-point-colors-in-chart-xaxis
        // solution was found:  use color.empty in addition to point.isempty
        public ScatterForm(ref List<cSeriesData> refSD, bool bShowingSystems)
        {
            InitializeComponent();
            bShowSystemData = bShowingSystems;
            //gboxOutlier.Visible = bShowSystemData;          // FIXME eliminate side effect on colors to fix problem [DONE!]
            ThisSeriesData = refSD;
            ShowScatter();
            GetLegendInfo.Enabled=true;
        }

        class cColoredLegends
        {  
            public string strName;
            public string strSubItems;  // for now, we are only showing projects
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

        // how many valid points in series
        private int CountOnlyValids(int iSeries)
        {
            int n = 0;
            var stuff = ThisSeriesData[iSeries];
            {
               foreach(bool b in stuff.bIsValid)
                {
                    if(b)n++;
                }
            }
            return n;
        }

        private int CountWhatsShowing()
        {
            int n = 0;
            if(CurrentSeriesDisplayed >=0)
            {
                return CountOnlyValids(CurrentSeriesDisplayed);
            }
            for(int i = 0; i < CurrentNumberSeriesDisplayable;i++)
            {
                n += CountOnlyValids(i);
            }
            return n;
        }

        private void DrawShowingText(int i)
        {
            int iCountWhatsShowing = CountWhatsShowing();
            tboxShowing.Text = MyLegendNames[i].strName + " (" + iCountWhatsShowing.ToString() + ")";
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
        // the input to this must be in minutes
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

        private double cvtScale2Double(string str)
        {
            if (str == " mins") return 1.0;
            if (str == " hours") return 60.0;
            return 60*24;
        }

        // this from idea at stackoverflow
        private void HidePoint(DataPoint p)
        {
            p.IsEmpty = true;
            p.Color = Color.Empty;
        }
        private void UnHidePoint(DataPoint p, Color c)
        {
            p.IsEmpty = false;
            p.Color = c;
        }

        private bool CalcMinMax(int iSeries)
        {
            double dSmall = 1e6;
            double dBig = -1;
            bool bValid = false;
            cSeriesData sd = ThisSeriesData[iSeries];
            int n = sd.dValues.Count;
            double d;
            for(int i = 0; i < n; i++)
            {
                if (!sd.bIsValid[i]) continue;
                d = sd.dValues[i];
                dSmall = Math.Min(dSmall, d);
                dBig = Math.Max(dBig, d);
                bValid = true;
            }
            sd.dSmall = dSmall;
            sd.dBig = dBig;
            return bValid;
        }

        /*
         * if elapsed time is over 60 seconds, scale time to minutes
         * same for minutes and hours
        */

        private string SetMinMax(ref double f)
        {
            double d = 0;
            int n = ThisSeriesData.Count;
            dSmall = 1e6;
            dBig = -1;           
            bool bValid;
            cSeriesData sd;
            for (int i = 0; i < n;i++)
            {
                sd = ThisSeriesData[i];
                bValid = CalcMinMax(i);
                if (sd.dSmall < dSmall) dSmall = sd.dSmall;
                if (sd.dBig > dBig) dBig = sd.dBig;               
            }
            string strUnits = BestTimeUnits(dBig, ref d);
            f = d / dBig;
            dSmall *= f;
            dBig = d;
            return strUnits;
        }

        private void SetScale()
        {
            fScaleMultiplier = 0;
            strScaleUnits = SetMinMax(ref fScaleMultiplier);
        }


        // draw points vertical one over the other from 1 to n but normalzed to 0.0 to 1.0
        // the x axis position is the elapsed time.
        private void ShowScatter()
        {
            double d=0;
            int j = 0;
            CurrentNumberSeriesDisplayable = ThisSeriesData.Count;
            SetScale();
            bScatteringApps = ThisSeriesData[0].bIsShowingApp;
            lviewSubSeries.Visible = bScatteringApps;
            
            if (!bScatteringApps)
            {
                lviewSubSeries.Items.Add(ThisSeriesData[0].strAppName);
            }
            foreach (cSeriesData sd in ThisSeriesData)
            {
                int n = sd.dValues.Count;
                List<double> yAxis = new List<double>();
                List<double> xAxis = new List<double>();
                for (int i = 0; i < n; i++)
                {
                    //if (!sd.bIsValid[i]) continue;    // hmm.  not known at this time and this routine runs once
                    d = Convert.ToDouble(i) / n;
                    yAxis.Add(d);
                    d = sd.dValues[i];
                    xAxis.Add(d * fScaleMultiplier);
                }
                string seriesname = sd.bIsShowingApp ? sd.strAppName : sd.strSystemName;
                SeriesName = seriesname;
                ChartScatter.Series.Add(seriesname);
                ChartScatter.Series[seriesname].EmptyPointStyle.Color = Color.Transparent;
                // seems not needed but left in to remind of what I tried
                ChartScatter.Series[seriesname].ChartType = SeriesChartType.Point;
                ChartScatter.Series[seriesname].Points.DataBindXY(xAxis.ToArray(), yAxis.ToArray());
            }
            UsedOutliers = new Stack<cSaveOutlier>();
            ChartScatter.Legends["Legend1"].Title = strSeries + strScaleUnits;

            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = GetBestScaleingUpper(dBig);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Minimum = GetBestScaleingBottom(dSmall);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "#.#";

        }

        // we removed an outlier got to rescale
        private void DoRescaleXaxis(double f)
        {
            if (Math.Abs(f - 1.0) < .01) return;    // within rounding
            for(int i = 0; i < ThisSeriesData.Count;i++)
            {
                foreach(DataPoint p in ChartScatter.Series[i].Points)
                {
                    p.XValue *= f;
                }
            }
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
        // not necessary for scatterning projects, only if scattering data
        private void ShowSystemNames(int s)
        {
            lviewSubSeries.Items.Clear();
            if(bShowSystemData)
            {

            }
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

        // this still does not work even after recommendation from stackoverflow
        private void RestoreDefaultColors()
        {
            int n = ThisSeriesData.Count;
            if (bShowSystemData) return;
            for(int i = 0; i <n; i ++)
            {
                Color c = SavedColoredPoints[i].Color;
                foreach (DataPoint p in ChartScatter.Series[i].Points)
                {
                    p.Color = c;
                    p.IsEmpty = false;
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
                CurrentSeriesDisplayed = -1;
                for (int i = 0; i < n; i++)
                {
                    ChartScatter.Series[i].Enabled = true;
                }
                gboxOutlier.Visible = true;// & bShowSystemData;
            }
            else 
            {
                j--;
                for (int i = 0; i < n; i++)
                {
                    if (i == j)
                    {
                        CurrentSeriesDisplayed = i; // showing only this series of "ThisSeriesData[]"
                        ChartScatter.Series[i].Enabled = true;
                    }
                    else
                        ChartScatter.Series[i].Enabled = false;
                }
                gboxOutlier.Visible = false;
            }
        }

        // show each series individually or all
        //
        private void nudShowOnly_ValueChanged(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(nudShowOnly.Value);
            int j = MyLegendNames.Count;
            if (i == j)
            {
                i = 0;  // wrap back to 0
                //RestoreDefaultColors();
                // the above works here but is not needed
                nudShowOnly.Value = 0;
                lviewSubSeries.Items.Clear();
            }
            ShowHideSeries(i);
            DrawShowingText(i); // ShowHide must be done first
            if (i == 0  | !bScatteringApps)
            {
                //RestoreDefaultColors();
                // THERE IS STILL A BUG AS CANNOT RESTORE DEFAULT COLORS
                // EVEN WITH THAT TRICK FROM STACKOVERFLOW so the above does not work
                lviewSubSeries.Items.Clear();
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

        // data is not sorted so we will just traverse and get biggest
        private bool bGetLastOutlier(int iSeries, ref int iLoc, ref double xValue, ref Color OriginalColor)
        {
            double dBig = -1;
            int iWhereBig = -1;
            bool bAny = false;
            DataPoint p;
            int j = (CurrentSeriesDisplayed == -1) ? 0 : CurrentSeriesDisplayed;
            Color c = SavedColoredPoints[j].Color;  // just need something here
            int n = ChartScatter.Series[iSeries].Points.Count;
            for(int i = 0; i < n; i++)
            {
                p = ChartScatter.Series[iSeries].Points[i];
                if (p.IsEmpty) continue;
                if(p.XValue > dBig)
                {
                    dBig = p.XValue;
                    iWhereBig = i;
                    c = p.Color;
                    bAny = true;
                }
            }
            xValue = dBig;
            iLoc = iWhereBig;
            OriginalColor = c;
            return bAny;
        }

        private double FindOutlier(ref cSaveOutlier sO, ref int iWhereSeries)
        {
            double xValue = -1.0, dBig = -1;
            int iWherePoint = -1;
            int iLoc = -1;
            bool bAny = false;
            if(CurrentSeriesDisplayed >= 0)
            {
                bAny = bGetLastOutlier(CurrentSeriesDisplayed, ref iLoc, ref xValue, ref sO.c);
                return xValue;
            }
            for (int i = 0; i < CurrentNumberSeriesDisplayable; i++)
            {
                bAny = bGetLastOutlier(i, ref iLoc, ref xValue, ref sO.c);
                if (!bAny)
                {
                    continue;
                }
                if (dBig < xValue)
                {
                    dBig = xValue;
                    iWherePoint = iLoc;
                    iWhereSeries = i;
                }
            }
            if (iWhereSeries < 0 || iWherePoint < 0) return 0;    // all removed
            sO.seriesname = ChartScatter.Series[iWhereSeries].Name;
            sO.iWhereRemoved = iWherePoint; 
            return dBig;
        }

        // remove the outlier then rescale to next largest
        // outlier value from chart series is scaled, may not be in minutes
        private void RemoveOutlier()
        {
            double x1Value,x2Value, x2original;   // original means from actual data,not the graphed point values
            double dNewMax=0; // if we remove a maximum there is a new one unless series is empty
            int iNewMax = 0;  // where it is
            bool bAny;      // if no new max
            double fCurrentScale;   // multiplier that was last used
            int iWhereSeries = -1;
            int iWhereData = -1;    // where int the original data 
            cSaveOutlier sO = new cSaveOutlier();
            cSaveOutlier sO2;
            x1Value = FindOutlier(ref sO, ref iWhereSeries );
            if (iWhereSeries == -1) return; // nothing to remove
            HidePoint(ChartScatter.Series[iWhereSeries].Points[sO.iWhereRemoved]);

            if (CurrentSeriesDisplayed < 0)
                iWhereData = iWhereSeries;  // they are one and the same
            else iWhereData = CurrentSeriesDisplayed;
            sO.iWhereData = iWhereData;
            sO.fmpy = 1.0;  // this changes
            ThisSeriesData[iWhereData].bIsValid[sO.iWhereRemoved] = false;
            UsedOutliers.Push(sO);
            sO2 = new cSaveOutlier();
            // need to recalc the maximum to be consistent
            bAny = bGetLastOutlier(iWhereSeries, ref iNewMax, ref dNewMax, ref sO2.c);
            if(bAny)
            {
                ThisSeriesData[iWhereData].dBig = dNewMax;
                if (dNewMax > dBig)
                    dBig = dNewMax; // this will change with scaling change
            }
            iWhereSeries = -1;
            x2Value = FindOutlier(ref sO2, ref iWhereSeries);
            if (iWhereSeries < 0) return;
            fCurrentScale = cvtScale2Double(strScaleUnits);
            //SetScale();
            x2original = ThisSeriesData[iWhereSeries].dValues[sO2.iWhereRemoved];
            if(x2Value != x2original)
            {
                double fRescale = x2original / x2Value;
                double OriginalValueThere = x1Value * fCurrentScale;
                ChangeScale(x2original, fCurrentScale, ref sO.fmpy);
            }
        }

        private void nudHideXoutliers_ValueChanged(object sender, EventArgs e)
        {
            int n = (int) nudHideXoutliers.Value;
            double x1;
            if(UsedOutliers.Count < n)
            {
                RemoveOutlier();
            }
            else
            {
                cSaveOutlier sO = UsedOutliers.Pop();
                //ChartScatter.Series[sO.seriesname].Points[sO.iWhereRemoved].IsEmpty = false;
                UnHidePoint(ChartScatter.Series[sO.seriesname].Points[sO.iWhereRemoved],sO.c);
                x1 = ThisSeriesData[sO.iWhereData].dValues[sO.iWhereRemoved];
                ThisSeriesData[sO.iWhereData].bIsValid[sO.iWhereRemoved] = true;
                // x1 was biggest at the time it was removed
                Debug.Assert(x1 >= ThisSeriesData[sO.iWhereData].dBig);   // got to be true in same series
                ThisSeriesData[sO.iWhereData].dBig = x1;
                RestoreScale(x1, sO.fmpy);
            }
        }

        // xValue here is original data not fron graph
        private void RestoreScale(double xValue, double fMpy)
        {
            double a,f,d = 0;
            strScaleUnits = BestTimeUnits(xValue, ref d);
            f = d / xValue;
            a = 1.0 / fMpy;
            DoRescaleXaxis(a);   

            dSmall *= f;
            dBig = GetBestScaleingUpper(d);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = dBig;

            ChartScatter.Legends["Legend1"].Title = strSeries + strScaleUnits;
        }

        // fMpy is current scale applied to original data
        // xAxis points unlikely in minutes
        private void ChangeScale(double xValue, double fMpy, ref double aUsed)
        {
            double a, f, d = 0;
            strScaleUnits = BestTimeUnits(xValue, ref d);
            f = d / xValue;
            a = fMpy * f;
            DoRescaleXaxis(a);       // chart data was scaled already

            dSmall *= a;
            dBig = GetBestScaleingUpper(d);
            ChartScatter.ChartAreas["ChartArea1"].AxisX.Maximum = dBig;
            ChartScatter.Legends["Legend1"].Title = strSeries + strScaleUnits;
            aUsed = a;
        }
    }
}