using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

// Joseph "Beemer Biker" Stateson. All rights reserved  copyright May 1, 2018


namespace GRCearned
{
    public partial class Default : System.Web.UI.Page
    {
        private class cProjTable
        {
            public string strProjname;
            public int nHosts;
            public long iLAvgRAC;
            public double fEstMAG;
            public double fGRC;
            public double fPCT;
            public double fEffic;
        }
        private cProjTable[] ProjTable;
        private double fSumPCT;
        private int nEntries;
        private int nSizeName;
        private string strHeaderOut;
        private WebClient client;
        private int[] iSort;

        private void SignalError(string strMsg)
        {
            errbox.Text = strMsg;
        }


        private int ParseInputs()
        {
            string[] strINs = ResultsBox.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            double fVal;
            long iLRac;
            double fLargestMag = -1.0, fLargestRac = -1.0;

            int iLocLargestV = 0;
            int iLocLargestR = 0;
            int n;
            int iStart = 0;
            nSizeName = -1;
            fSumPCT = 0;
            nEntries = 0;
            if (strINs[0].Contains("Project")) iStart = 1;
            try
            {
                for (int i = iStart; i < strINs.Length; i++)
                {
                    string[] strLines = strINs[i].Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                    ProjTable[nEntries].strProjname = strLines[0];
                    n = ProjTable[nEntries].strProjname.Length;
                    if (n > nSizeName) nSizeName = n;
                    ProjTable[nEntries].nHosts = Convert.ToInt32(strLines[1]);
                    decimal dVal = Convert.ToDecimal(strLines[2]);
                    ProjTable[nEntries].iLAvgRAC = iLRac = Convert.ToInt32(dVal);
                    ProjTable[nEntries].fEstMAG = fVal = Convert.ToDouble(strLines[3]);
                    if (iLRac == 0) iLRac = 1; // still gives 0 below but displays as 0 also
                    ProjTable[nEntries].fEffic = fVal / iLRac;
                    if (ProjTable[nEntries].fEffic > fLargestRac)
                    {
                        fLargestRac = ProjTable[nEntries].fEffic;
                        iLocLargestR = nEntries;
                    }
                    if (fVal > fLargestMag)
                    {
                        iLocLargestV = nEntries;
                        fLargestMag = fVal;
                    }
                    nEntries++;
                }
                // normalize values so when we multipoly by 1000 we get the grc distribution or close to it
                for (int i = 0; i < nEntries; i++)
                {
                    if (fLargestMag > 0.0)
                    {
                        ProjTable[i].fPCT = fVal = ProjTable[i].fEstMAG / fLargestMag;
                        fSumPCT += fVal;
                        ProjTable[i].fEffic = 100.0 * ProjTable[i].fEffic / fLargestRac;
                    }
                    else
                    {
                        ProjTable[i].fPCT = 0;
                        ProjTable[i].fEffic = 0;
                    }
                }
                for (int i = 0; i < nEntries; i++) ProjTable[i].fPCT /= fSumPCT;
                return 0;
            }
            catch
            {
                SignalError("Problem with values pasted into the box at line " + (1 + nEntries));
                return -1;
            }
        }

        // did not put much thought into below code. note that "Project" has a lot of space so the -3 was used
        string PerformPad(string strIN, int padVal, string strHdr)
        {
            int p = padVal;
            string strTemp;
            if (p < 0)
            {
                p = -p;
                strTemp = strIN.PadRight(p);
                strHeaderOut += strHdr.PadRight(strTemp.Length - 3);
                return strTemp;
            }
            else
            {
                strTemp = strIN.PadLeft(p);
                strHeaderOut += strHdr.PadLeft(strTemp.Length);
                return strTemp;
            }

        }


        private void SortRAC()
        {
            int n;
            for (int i = 0; i < nEntries; i++)
            {
                for (int j = i + 1; j < nEntries; j++)
                {
                    if (ProjTable[iSort[i]].iLAvgRAC < ProjTable[iSort[j]].iLAvgRAC)
                    {
                        n = iSort[i];
                        iSort[i] = iSort[j];
                        iSort[j] = n;
                    }
                }
            }
        }

        // "Project  nHosts   RAC     MAG   $GRC   EFf%";  these have to be smaller than the values or the above pad program fails.
        private void ShowStuff()
        {
            string strOut = "\n";
            int i;
            double fVal;
            SortRAC();
            for (int j = 0; j < nEntries; j++)
            {
                i = iSort[j];
                strHeaderOut = ""; // this needs to be cleaned up
                strOut += PerformPad(ProjTable[i].strProjname, -1 - nSizeName, "Project");
                strOut += PerformPad(ProjTable[i].nHosts.ToString(), 3, "nHosts");
                strOut += PerformPad(ProjTable[i].iLAvgRAC.ToString(), 10, "RAC");
                strOut += PerformPad(ProjTable[i].fEstMAG.ToString(), 10, "MAG");
                fVal = Convert.ToDouble(LastKnown.Text);
                ProjTable[i].fGRC = fVal * ProjTable[i].fPCT;
                strOut += PerformPad(ProjTable[i].fGRC.ToString("N2"), 10, "$GRC");
                strOut += PerformPad(ProjTable[i].fEffic.ToString("N2"), 7, "EFF%");
                strOut += "\n";
            }

            ResultsBox.Text = strHeaderOut + strOut;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
            iSort = new int[48];
            ProjTable = new cProjTable[48];
            for (int i = 0; i < 48; i++)
            {
                ProjTable[i] = new cProjTable();
                iSort[i] = i;
            }
            // ResultsBox.Font.Name = "Courier";      
            //ResultsBox.Width = 600;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResultsBox.Text = "";
            cpidValue.Text = "";
        }

        protected void btnCalc_Click(object sender, EventArgs e)
        {
            if (ParseInputs() < 0) return;
            ShowStuff();
        }


        private string GetRight(string strIN)
        {
            int j, i = strIN.IndexOf("right'>");
            if (i < 0) return "";
            i += 7;
            return strIN.Substring(i);
        }

        /*

               table of interest is between a pair of <tbody></tbody>
               view&id=numberfields@home'>  
        */
        protected void btnLKUPcpid_Click(object sender, EventArgs e)
        {

            string RawPage;
            string RawTable;
            string[] RawProjects;
            string[] RawLines;
            int i,n;
            string S;
            string strOut = "";
            string ProjUrl = "https://gridcoinstats.eu/cpid/" + cpidValue.Text;
            int iStart, iEnd;
            Uri myUri;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                myUri = new Uri(ProjUrl);
                RawPage = client.DownloadString(myUri);
            }
            catch
            {
                SignalError("unable to access web site or download from url");
                return;
            }

            iStart = RawPage.IndexOf("<tbody>");
            iEnd = RawPage.IndexOf("</tbody>");
            if (iStart < 0 || iEnd < 0 || (iStart >= iEnd))
            {
                SignalError("cannot find magnitude table at " + ProjUrl);
                return;
            }
            RawTable = RawPage.Substring(iStart, iEnd - iStart);
            RawProjects = RawTable.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
            n = RawProjects.Length;
            i = 0;
            do
            {
                iStart = RawProjects[i].IndexOf("view&id=");
                if (iStart >= 0)
                {
                    iStart += 8;
                    iEnd = RawProjects[i].Substring(iStart).IndexOf("'>");
                    if(iEnd > 0)
                    {
                        // found a project name, rneed to remove spaces in project name
                        S = RawProjects[i].Substring(iStart, iEnd);
                        strOut += Regex.Replace(S, @"\s+", "");
                        strOut += "\t";
                        i += 2; // POINT TO NHOSTS
                        strOut += RawProjects[i] + "\t";
                        i++;
                        strOut += GetRight(RawProjects[i]) + "\t";
                        i++;
                        strOut += GetRight(RawProjects[i]) + "\n";
                        i++;
                    }
                }
                i++;
            } while (i < n);
            ResultsBox.Text = strOut;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/About.aspx");
        }
    }
}