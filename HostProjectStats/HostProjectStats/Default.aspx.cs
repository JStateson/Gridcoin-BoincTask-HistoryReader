//#define TenCheck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;



// Joseph BEEMER BIKER Stateson  copyright May 1, 2018

namespace HostProjectStats
{
    public partial class Default : System.Web.UI.Page
    {

        WebClient client;
        int NumberToCollect = 20;
        int NumberWatts = 0;
        int IdleWatts = 0;
        int NumberConcurrent = 1;
        int NumberBoards = 1;
        const int NumberMaxPages = 10;
        const int MaxNumSamples = NumberMaxPages * 20;
        int nTotalSamples;
        string RawPage;
        string RawTable;
        string[] RawLines;
        double[] Rt, Ct, Cr;    // run time, cpu time, credit
        double avgRt, avgCt, avgCr;
        int nPagesToRead;
        string StatsOut;
        // escatter is nfs at home
        enum eProjectID { einstein, milkyway, collatz, gpugrid, amicable, asteroids, cosmology, drugdiscovery, enigmaathome, latinsquares, lhcathome, escatter, theskynet, setiathome, rosetta, commgrid  };
        string[] strProjNames =  // damn -- casesensitive i forgot about that
            {"einstein", "milkyway", "collatz", "gpugrid", "Amicable", "asteroids", "cosmology",
            "drugdiscovery", "enigmaathome", "latinsquares", "lhcathome", "escatter", "theskynet", "setiathome","rosetta", "communitygrid"};
        int iProjectID;
        //return bWasValid ? strTemp : strToday;
        bool bDoNorm = false;
        

        int ProjectLookup(string strUrl)
        {
            int i, n = strProjNames.Count() ;
            for(i = 0; i < n; i++)
            {
                if(strUrl.Contains(strProjNames[i]))
                {
                    iProjectID = i; // enum must start at 0
                    lblProjName.Text = strProjNames[i];
                    if ((eProjectID)iProjectID == eProjectID.commgrid)
                    {
                        int j = Convert.ToInt32(tb_num2read.Text);
                        if (j > 15)
                        {
                            tb_num2read.Text = "15";    // wcg shows 15 per page
                            NumberToCollect = 15;
                        }
                    }
                    return 0;
                }
            }
            ResultsBox.Text = " project not recognized\n";
            return -1;
        }

        void ZeroStuff()    // could consider accumulating results from additional pages
        {
            RawTable = "";
            avgRt = 0.0;
            avgCt = 0.0;
            avgCr = 0.0;
            nTotalSamples = 0;
            for (int i = 0; i < MaxNumSamples; i++)
            {
                Rt[i] = Ct[i] = Cr[i] = 0.0;
            }
            StatsOut = "";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            //CookieContainer cc = new CookieContainer();
            client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
            // the below were used for accedssing WCG but didnt work
            //client.UseDefaultCredentials = true;
            //client.Credentials = new NetworkCredential("username", "password");
            Rt = new double[MaxNumSamples];
            Ct = new double[MaxNumSamples];
            Cr = new double[MaxNumSamples];
            ZeroStuff();
            if (!IsPostBack)
            {
                ProjUrl.Text = ddlTest.SelectedValue;
            }
            else
            {
                i++;
            }
        }

        double GetSTD(ref double[] dIn, double dAvg)
        {
            double sqr;
            double sqrSum = 0.0;
            int i;
            for (i = 0; i < nTotalSamples; i++)
            {
                double dDif = dIn[i] - dAvg;
                sqr = dDif * dDif;
                sqrSum += sqr;
            }
            sqrSum = sqrSum / NumberToCollect;
            return Math.Sqrt(sqrSum);
        }


        double GetValue(int iLoc, int iOffset)
        {
            string sTemp = RawLines[iLoc + iOffset];
            string lSide;
            int iRight = sTemp.IndexOf("right>");
            if (iRight < 0)
            {
                ResultsBox.Text = "Error expected right> not found in line " + sTemp;
                Environment.Exit(0);
            }
            lSide = sTemp.Substring(iRight + 6);
            iRight = lSide.IndexOf("</td>");
            if (iRight < 0)
            {
                ResultsBox.Text = "Error expected </td> not found in line " + sTemp;
                Environment.Exit(0);
            }
            double aValue = Convert.ToDouble(lSide.Substring(0, iRight));
            return aValue;
        }

        void ExtractTriplet(int iLocation, int iIndex)
        {
            Rt[iIndex] = GetValue(iLocation, 0) / NumberConcurrent;
            Ct[iIndex] = GetValue(iLocation, 1);    // jys should not have reduced this as CPU time was correct
            Cr[iIndex] = GetValue(iLocation, 2);
            if(bDoNorm)
            {
                double d = Cr[iIndex];
                Cr[iIndex] = 1.0;
                Rt[iIndex] /= d;
                Ct[iIndex] /= d;
            }
        }

        void ShowData()
        {
            double t;
            avgCr = 0;
            avgCt = 0;
            avgRt = 0;
            string outStr = "";
            for(int i = 0; i < nTotalSamples;i++)
            {
                t = Rt[i];
                avgRt += t;
                outStr += t.ToString("0.0").PadLeft(16);
                t = Ct[i];
                avgCt += t;
                outStr += t.ToString("0.0").PadLeft(14);
                t = Cr[i];
                avgCr += t;
                outStr += t.ToString("0.0\n").PadLeft(14); 
            }
            StatsOut += outStr;
        }


        private int PerformCalculate(string strUrl)
        {
            int iStart, iEnd;
            Uri myUri;


            iStart = 0; // for breakpoint purpose
            try
            {
                myUri = new Uri(strUrl);
            }
            catch
            {
                ResultsBox.Text = "cannot access site as the url is not recognzized\n";
                ResultsBox.Text += strUrl + "\n";
                ResultsBox.Text += "This program tested only with milkyway and einstein\n";
                ResultsBox.Text += "may be missing https:// and must have offset= or page= in the url\n";
                return -1;
            }
            try
            {
                //client.Credentials.GetCredential(myUri,"Basic");
                RawPage = client.DownloadString(myUri);
            }
            catch
            {
                ResultsBox.Text = "no internet connection or project down\n";
                return -2;
            }
            switch ((eProjectID)iProjectID)
            {
                case eProjectID.einstein:
                    iStart = RawPage.IndexOf("<tbody>");
                    iEnd = RawPage.IndexOf("</tbody>");
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd))
                    {
                        ResultsBox.Text = "Bad einstein url:  cannot find TABLE nor TBODY or maybe no results\n";
                        return -3;
                    }
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    BuildEinsteinStatsTable();
                    break;
                case eProjectID.commgrid:
                    string strGranted = "Granted BOINC Credit</td>";
                    iStart = RawPage.IndexOf(strGranted);
                    break;
                case eProjectID.collatz:
                case eProjectID.setiathome:
                case eProjectID.latinsquares:
                case eProjectID.enigmaathome:
                case eProjectID.lhcathome:
                case eProjectID.amicable:
                case eProjectID.rosetta:
                case eProjectID.milkyway:
                    string strH = "<td><a href=" + "\"" + "workunit.php";
                    iStart = RawPage.IndexOf(strH);
                    if(iStart < 0)
                    {
                        ResultsBox.Text = "error: missing '<a href=' or maybe no results\n";
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable();
                    break;
                default:
                    iStart = RawPage.IndexOf("<tr class=row0>");
                    if(iStart < 0)
                    {
                        ResultsBox.Text = "error: no data or missing '<tr class=row0>'\n";
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");   // need skip over any earlier tables in the header
                    if (iStart < 0 || iEnd < 0 )
                    {
                        int ERR = 0;
                    }
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable();
                    break;

            }
            return 0;
        }
        /*
        https://setiathome.berkeley.edu/results.php?hostid=8699104&offset=0&show_names=0&state=4&appid=
        https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=766466&offset=0&show_names=0&state=4&appid=
        https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=799122&offset=0&show_names=0&state=4&appid=
        */
        private string ValidateUrl(string strIN)
        {
            string strTmp = "";
            if(strIN.Contains("userid"))
            {
                ResultsBox.Text = "error: url cannot have userid\n";
                return "";
            }
            if (strIN.Substring(0, 4) == "www.")
            {
                strTmp = "https://" + strIN;
            }
            else strTmp = strIN;
            if(strTmp.Contains("&offset") || strTmp.Contains("&page="))return strTmp;
            if(strTmp.Contains("?offset") || strTmp.Contains("?page=")) return strTmp;
            if(strTmp.Contains("&pageNum="))
            {
                // wcg does not "always" put pageNum at end of url where we need it 
                //strTmp.Replace("&pageNum=", "");
                // need to get rid of the digits jys do later
            }
            // got to add offset or page phrase
            switch ((eProjectID) iProjectID )
            {
                case eProjectID.einstein:
                    strTmp += "?page=0";
                    break;
                case eProjectID.gpugrid:
                    strTmp += "&offset=0&show_names=0&state=3&appid=";
                    break;
                case eProjectID.collatz:
                case eProjectID.latinsquares:
                case eProjectID.theskynet:
                case eProjectID.amicable:
                case eProjectID.setiathome:
                case eProjectID.milkyway:
                    strTmp += "&offset=0&show_names=0&state=4&appid=";
                    break;
                case eProjectID.commgrid:
                    //strTmp += "&pageNum=1";
                    break;
                default:
                    ResultsBox.Text = "problem with url:  missing phrase offset or page\n";
                    strTmp = "";
                    break;

             
            }
            return strTmp;
        }


        protected void btnCalc_Click(object sender, EventArgs e)
        {
            string nexturl;
            string strPrefix = "", strSuffix = "";
            int iStart=0, iEnd = -1;
            int iOffset = 0;
            int j;
            string strProjUrl = ProjUrl.Text;

            bDoNorm = CBoxNorm.Checked;
            NumberToCollect = Convert.ToInt32(tb_num2read.Text);
            if (tb_ntasks.Text == "") tb_ntasks.Text = "1";
            NumberConcurrent = Convert.ToInt32(tb_ntasks.Text);
#if TenCheck
            tb_ngpu.Text = "3";
            tb_watts.Text = "420";
            tb_idle.Text = "120";
#endif
            if (tb_ngpu.Text == "") tb_ngpu.Text = "1";
            if(tb_watts.Text=="")tb_watts.Text = "0";
            if(tb_idle.Text== "")tb_idle.Text = "0";
            NumberBoards = Convert.ToInt32(tb_ngpu.Text);
            NumberWatts = Convert.ToInt32(tb_watts.Text);
            IdleWatts = Convert.ToInt32(tb_idle.Text);
            if(IdleWatts > NumberWatts)
            {
                ResultsBox.Text += "Idle wattage must be less than load wattage";
                return;
            }
            if (ProjectLookup(strProjUrl) < 0)
            {
                ResultsBox.Text = "problem with project lookup\n";
                return;
            }
            strProjUrl = ValidateUrl(ProjUrl.Text);
            if(strProjUrl =="")
            {
                ResultsBox.Text += "url is bad";
                return;
            }
            StatsOut = "";
            // be able to find the page or offset
            switch ((eProjectID) iProjectID)
            {
                default:
                    iStart = strProjUrl.IndexOf("offset=");
                    iStart += 7;
                    iEnd = strProjUrl.IndexOf("&show_names");
                    iOffset = Convert.ToInt32(strProjUrl.Substring(iStart, iEnd - iStart));
                    break;
                case eProjectID.einstein:
                    iStart = strProjUrl.IndexOf("page=");
                    iStart += 5;
                    strPrefix = strProjUrl.Substring(0, iStart);
                    strSuffix = strProjUrl.Substring(iStart);
                    break;
                case eProjectID.commgrid:
                    iStart = strProjUrl.IndexOf("pageNum=");
                    iStart += 8;
                    strPrefix = strProjUrl.Substring(0, iStart);
                    strSuffix = strProjUrl.Substring(iStart);
                    break;
            }


            nPagesToRead = Convert.ToInt32(ddlNumPages.Text); // Convert.ToInt32(txtPagesToRead.Text);
//            if (nPagesToRead < 0) nPagesToRead = 1;
//            if (nPagesToRead > NumberMaxPages) nPagesToRead = NumberMaxPages;
//            txtPagesToRead.Text = nPagesToRead.ToString();
            IssueTitle();
            for (int i = 0; i < nPagesToRead;i++)
            {

                if (
                    ((eProjectID) iProjectID == eProjectID.einstein) ||
                    ((eProjectID) iProjectID == eProjectID.commgrid)
                   )
                {
                    nexturl = strPrefix + strSuffix;
                    j = Convert.ToInt32(strSuffix);
                    j++;
                    strSuffix = j.ToString();
                }
                else
                {
                    nexturl = strProjUrl.Substring(0, iStart) + iOffset.ToString() + strProjUrl.Substring(iEnd);
                    iOffset += 20;
                }

                ProjUrl.Text = nexturl;
                if (PerformCalculate(nexturl) < 0)
                {
                    ResultsBox.Text += "calculation was bad probably no data\n";
                    return;
                }
            }
            ShowData();
            if (bDoNorm)
            {
                StatsOut += "\nnormalizing can show problems with credit calculations\n";
                StatsOut += "or which gpu devices are faster than others\n";
            }
            else
                FormStats();

            ResultsBox.Text = StatsOut;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ZeroStuff();
            ResultsBox.Text = "";
            ProjUrl.Text = "";
            tb_num2read.Text = "20";
            tb_ntasks.Text   = "1";
            tb_idle.Text = "0";
            tb_watts.Text = "0";
            tb_ngpu.Text = "1";
        }



        void IssueTitle()
        {
            StatsOut += "        Run Time     CPU Time     Credit\n";
            StatsOut += "         (sec)         (sec)\n";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/About.aspx");
        }

       

        protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjUrl.Text = ddlTest.SelectedValue;
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            Response.Redirect(ProjUrl.Text);
        }

        protected void btn_help_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/help.aspx");
        }





        /*
         * finr first "right" and the next is exactly 10 rows down
         * first one should be within 21 lines of top of buffer
        <td align=right>211.42</td>
        <td align=right>19.19</td>
        <td align=right>227.26</td>
        */

        void FinishTable(int iFirst)
        {
            for (int i = 0; i < NumberToCollect; i++)
            {
                ExtractTriplet(iFirst, nTotalSamples);
                nTotalSamples++;
                iFirst += 10;
            }
        }

        void FormStats()
        {
            double t;
            double spc; //seconds per credit one gpu one work unit
            double cps; //credits per second
            double cph; //credits per hour
            double Total_watts, Each_watts, GPU_watts;
            double kwh;
            string outStr = "";
            StatsOut += "         ----------------------------------\n";
            avgCr = avgCr / nTotalSamples;
            avgRt = avgRt / nTotalSamples;
            avgCt = avgCt / nTotalSamples;
            outStr = "\nAvg:";
            outStr += avgRt.ToString("0.0").PadLeft(12);    // average runtime
            outStr += avgCt.ToString("0.0").PadLeft(14);    // average cpu time
            outStr += avgCr.ToString("0.0").PadLeft(13);    // average credit
            outStr += "\nSTD:";
            outStr += GetSTD(ref Rt, avgRt).ToString("0.0").PadLeft(12);
            outStr += GetSTD(ref Ct, avgCt).ToString("0.0").PadLeft(14);
            outStr += GetSTD(ref Cr, avgCr).ToString("0.0\n\n").PadLeft(15);
            spc = avgRt / avgCr;
#if TenCheck
            spc = 10.0;
#endif
            cps = 1.0 / spc;
            outStr += spc.ToString("#,##0.00 seconds per credit from above info one device\n");
            outStr += cps.ToString("##0.0000 Credits per second for one device\n");
            if (NumberConcurrent > 0)
            {
                outStr += "Times shown above were divided by number of concurrent tasks(" + NumberConcurrent.ToString() + ")\n";
            }
            cph = cps * 3600 * NumberBoards * NumberConcurrent;
            outStr += cph.ToString("###,##0. number of credits in an hour, this system\n");
            if (NumberWatts > 0)
            {
                GPU_watts = (NumberWatts - IdleWatts) / NumberBoards;
                outStr += GPU_watts.ToString("#,##0. total watts used by a single producing device (avg each work unit)\n");
                cph = cps * 3600 * NumberConcurrent;    // per each board or cpu thread this time.
                outStr += cph.ToString("###,##0") + " credits per hour for exactly one device(" + NumberConcurrent.ToString() + " tasks)\n"; 
                // convert credits for a full kilowatt hour (not just GPU hours) 
                cph /= NumberBoards;
                kwh = cph * 1000 / GPU_watts;
                outStr += kwh.ToString("A kilowatt hour will theoretically  produce maximum of ###,##0. credits each device this PC\n");
                outStr += "Use the above KWH credits to compare this device with any other device\n\tas the overhead (idle) has been removed\n";
                outStr += "Actual credit product is less because the GPU has idle time between tasks\n";
            }
            StatsOut += outStr;
        }

        // TIME TO COMPLETE ONE CREDIT


        private int BuildStatsTable()
        {
            int i;
            RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (RawLines.Count() < 10 * NumberToCollect)
            {
                ResultsBox.Text = "Error - must have at least " + NumberToCollect.ToString() + " values on a page\n";
                return -1;
            }
            for (i = 0; i < 21; i++)    // first line of data must be within first 21 or so lines
            {
                if (RawLines[i].Contains("right>"))
                {
                    FinishTable(i); // this gets rest of table which might be less then 20 lines
                    return 0;
                }
            }
            ResultsBox.Text = "Error - could not find first value on the page\n";
            return -2;
        }

        // Completed and validated</td><td>2211</td><td>366</td><td>3465</td><td class="task-app">Gamma-ray pulsar binary search #1 on GPUs
        // <tbody> and </tbody> are better

        void BuildEinsteinStatsTable()
        {
            int i;
            int j;
            int iIndex = nTotalSamples;
            double t;
            string[] RawLineValues;

            RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            NumberToCollect = Math.Min(NumberToCollect, RawLines.Length - 1);
            if (RawLines.Count() < NumberToCollect)
            {
                ResultsBox.Text = "Error - must have at least " + NumberToCollect.ToString() + " values on a page";
                Environment.Exit(0);
            }
            for (i = 1; i < (1+NumberToCollect); i++) // first data line starts as row 1, not 0
            {
                j = RawLines[i].IndexOf("Completed and validated");
                if (j > 0)
                {
                    j += 32;    // at first value: "2,224</td><td>369</td><td>3,465</td>"
                    RawLineValues = RawLines[i].Substring(j, 60).Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                    t = Convert.ToDouble(RawLineValues[0]);
                    t /= NumberConcurrent;
                    Rt[iIndex] = t;
                    avgRt += t;

                    t = Convert.ToDouble(RawLineValues[1]);
                    t /= NumberConcurrent;
                    Ct[iIndex] = t;
                    avgCt += t;

                    t = Convert.ToDouble(RawLineValues[2]);
                    t /= NumberConcurrent;
                    Cr[iIndex] = t;
                    avgCr += t;
                }
                else
                {
                    ResultsBox.Text = "Error - could not find first value on the page";
                    Environment.Exit(0);
                }
                iIndex++;
            }
            nTotalSamples = iIndex;
        }


    }
}