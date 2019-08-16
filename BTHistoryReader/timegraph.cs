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
    public partial class timegraph : Form
    {
        static int HoursSpanMin = 6;
        static int MaxHours = 7 * 24;
        double dOffline;
        double mLargestTimeSpan;    // longest time start to stop of selected records in minutes
        private List<double> Offline;
        private List<double> TimeOffline;
        private double x_max = MaxHours;
        private double x_interval = HoursSpanMin;
        private List<cProjectInfo> ThisProjectInfo;
        private int DeviceCount;
        private List<cGraphInfo> GraphInfo;
        private double mFilter  = 15.0  ; //15 minutes (note raw data is in seconds)
        private System.DateTime dt_1970 = new System.DateTime(1970, 1, 1);
        private long lSmallest; // need to make sure all series start at offset to first time point
        private double dSmallOffset;    // make up for difference
        private double dElapsedOffset;  // for y axis
        private int iElapsedCount;
        public DateTime dtStart;
        private int[] iSortToInfo;
        int iStart;
        int iStop;
        int nDevices;

        public timegraph(ref List<cProjectInfo> rThisProjectInfo, int rnDevices, int iiStart, int iiStop, ref int[] SortToInfo)
        {
            InitializeComponent();
            nDevices = rnDevices;
            ThisProjectInfo = rThisProjectInfo;
            DeviceCount = nDevices;
            iSortToInfo = SortToInfo;
            iStart = iiStart;
            iStop = iiStop;
            PerformGraph();
        }
        
        private void PerformGraph()
        {
            GraphInfo = new List<cGraphInfo>(nDevices);
            Offline = new List<double>();
            TimeOffline = new List<double>();
            for (int i = 0; i < nDevices; i++)
            {
                cGraphInfo gi = new cGraphInfo();
                gi.RawDevice = new List<cDeviceGraphInfo>();
                GraphInfo.Add(gi);
                if (i >= 0)
                {
                    Series gpus = new Series("GPU" + i.ToString());
                    gpus.ChartType = SeriesChartType.Line;
                    tgraph.Series.Add(gpus);
                }
            }
            tgraph.ChartAreas[0].AxisX.Minimum = 0;
            tgraph.ChartAreas[0].AxisX.Interval = x_interval;
            dtStart = DateTime.SpecifyKind(dt_1970.AddSeconds(ThisProjectInfo[iSortToInfo[iStart]].time_t_Completed), DateTimeKind.Utc);
            for (int i1 = iStart; i1 < iStop; i1++)
            {
                int i = iSortToInfo[i1];
                if (!ThisProjectInfo[i].bState)
                    continue;
                int k = ThisProjectInfo[i].iDeviceUsed;
                for (int j = 0; j < nDevices; j++)
                {
                    if (k == j)
                    {
                        cDeviceGraphInfo dgi = new cDeviceGraphInfo();
                        dgi.dElapsed = ThisProjectInfo[i].dElapsedCPU;
                        dgi.time_t = ThisProjectInfo[i].time_t_Completed;
                        GraphInfo[j].RawDevice.Add(dgi);
                        break;
                    }
                }
            }
            dElapsedOffset = 0;
            iElapsedCount = 0;
            mFilter = Convert.ToInt32(nudAvg.Text);
            for (int i = 0; i < nDevices; i++)
            {
                GraphInfo[i].AvgMinutes = mFilter;
                if (i == 0) lSmallest = GraphInfo[i].lStart;
                lSmallest = Math.Min(lSmallest, GraphInfo[i].lStart);
                iElapsedCount += GraphInfo[i].RawDevice.Count;
                dElapsedOffset += GraphInfo[i].sAverageElapsed;
            }
            dElapsedOffset /= iElapsedCount;

            //  would like to know if system was offline:  use 3 * elapsed offset to determine if offline
            dOffline = 3 * dElapsedOffset;
            for (int i1 = iStart; i1 < iStop - 1; i1++)
            {
                int i = iSortToInfo[i1];
                // even bad points count as system still being on line
                int j = iSortToInfo[i1 + 1];
                double dij = (ThisProjectInfo[j].time_t_Completed - ThisProjectInfo[i].time_t_Completed) / 60.0;
                if (dij > dOffline)
                {
                    // x axis is in hours not minutes
                    Offline.Add((ThisProjectInfo[i].time_t_Completed - ThisProjectInfo[iSortToInfo[iStart]].time_t_Completed) / 3600.0);
                    TimeOffline.Add(dij / 60.0);
                }
            }

            if (Offline.Count > 0)
            {
                Series gpus = new Series("Offline?");
                int NumMarkers = 10; 
                gpus.ChartType = SeriesChartType.Point;
                for (int i = 0; i < Offline.Count; i++)
                {
                    // add some points between x and x + the span
                    NumMarkers = GetBestNumMarkers(TimeOffline[i]);
                    double dSpan = TimeOffline[i] / NumMarkers;
                    for (int j = 0; j < NumMarkers; j++)
                    {
                        gpus.Points.AddXY(Offline[i] + j * dSpan, 10);
                    }
                }
                tgraph.Series.Add(gpus);
            }
            mLargestTimeSpan = -1;
            for (int i = 0; i < nDevices; i++)
            {
                dSmallOffset = (GraphInfo[i].lStart - lSmallest) / 60.0;
                mLargestTimeSpan = Math.Max(mLargestTimeSpan, GraphInfo[i].mTimeSpan);
                for (int j = 0; j < GraphInfo[i].NumEntries; j++)
                {
                    double dHours = (GraphInfo[i].time_m[j] + dSmallOffset) / 60.0;
                    tgraph.Series[i].Points.AddXY(dHours, (GraphInfo[i].dElapsed[j] + dElapsedOffset * i));
                }
                if (GraphInfo[i].NumEntries < 2)
                {
                    tgraph.Series[i].ChartType = SeriesChartType.Point;
                }
                else tgraph.Series[i].ChartType = SeriesChartType.Line;
            }
            tgraph.ChartAreas[0].AxisX.Maximum = BestXmax(mLargestTimeSpan);
            lbxaxis.Text = "xAxis time since " + dtStart.ToLocalTime() + " in hours";
            lbyaxis.Text = "yAxis elapsed time (seconds) offset by " + dElapsedOffset.ToString("##.0 seconds");
            //tgraph.Series[0].Points.DataBindXY(GraphInfo[0].time_t, GraphInfo[0].dElapsed);
            tgraph.Invalidate();
        }

        // span is in hours
        private int GetBestNumMarkers(double span)
        {
            if (span < 1.0) return 10;
            if (span < 10) return 50;
            return 100;
        }

        private double BestXmax(double mMinutes)
        {
            double dScale = HoursSpanMin;
            double h = mMinutes / 60.0;            
            for (int i = 0; i < MaxHours; i += HoursSpanMin)
            {
                if (h < (i + HoursSpanMin)) return dScale;
                dScale += HoursSpanMin;
            }
            return dScale;
        }

        private void btnDoGraph_Click(object sender, EventArgs e)
        {
            int n = tgraph.Series.Count;
            for (int i = n - 1; i >= 0; i--)
                tgraph.Series.RemoveAt(i);
            PerformGraph();
        }
    }
}
