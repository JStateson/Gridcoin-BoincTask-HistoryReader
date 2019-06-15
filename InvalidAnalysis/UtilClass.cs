using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*  cProjPage reads following (may be missing the <tr> depending on implementation
<tr><td><a href="result.php?resultid=243747096">243747096</a></td>
<td><a href="workunit.php?wuid=1769142142">1769142142</a></td>

        <td>15 Jun 2019, 2:37:53 UTC</td>
        <td>15 Jun 2019, 4:43:07 UTC</td>
        <td>Validate error</td>
        <td align=right>150.66</td>
        <td align=right>13.84</td>
        <td align=right>---</td>
        <td>Milkyway@home Separation v1.46 (opencl_ati_101)<br>windows_x86_64</td>
        </tr>
need the following outputs
https://milkyway.cs.rpi.edu/milkyway/workunit.php?wuid=1769183969
https://milkyway.cs.rpi.edu/milkyway/show_host_detail.php?hostid=772622 (get this from main page header only)
Milkyway@home Separation v1.46 (opencl_ati_101)<br>windows_x86_64

*/


namespace InvalidAnalysis
{

    public class cProjPage 
    {
        //Task	Work unit	Computer	Sent	Time reported	Status	Run time CPU time	Credit	Application
        public string strWorkUnit;
        public string strComputer;
        public string strApplication;
        public string strDevice;
        public string strOs;
        public string strProject;
        public List<cWorkUnit> Wingmen;

        public bool SetData(string strKey, string sComputer, ref string strIn)
        {
            strWorkUnit = GetNumericTarget(ref strIn, "?wuid=");
            strComputer = sComputer;
            int i = strIn.IndexOf(strKey);  // key should be "Milkyway@home"
            if (i < 0) return false;
            string strOut = strIn.Substring(i);
            strOut = strOut.Replace("<br>", " ");
            i = strOut.IndexOf('<');
            if (i < 0) return false;
            strOut = strOut.Substring(0, i);
            i = strOut.IndexOf('(');
            if (i < 0) return false;
            strApplication = strOut.Substring(0, i - 1); 
            strOut = strOut.Substring(i+1);
            i = strOut.IndexOf(')');
            if (i < 0) return false;
            strDevice = strOut.Substring(0, i);
            strOs = strOut.Substring(i + 1);
            return true;
        }
        public string urlWorkUnit
        {
            get { return strProject + "/workunit.php?wuid=" + strWorkUnit; }
        }
        public string urlComputer
        {
            get { return strProject + "/show_host_detail.php?hostid=" + strComputer; }
        }

        // strKey: " tasks for computer "
        // strProjectName: must be in form: https://milkyway.cs.rpi.edu/milkyway
        public string InitProject(string strProjectName, ref string strIn, string strKey)    
        {
            strProject = strProjectName;
            Wingmen = new List<cWorkUnit>();
            return GetNumericTarget(ref strIn, strKey); // this is the computer id
        }

        public string GetNumericTarget(ref string InString, string LookingFor)
        {
            string strTemp;
            int i = InString.IndexOf(LookingFor);
            int j = LookingFor.Length;
            if (i < 0) return "";
            i += j;
            strTemp = InString.Substring(i);
            for(j = 0; j < 12; j++) // should find terminator within 10 but who knows
            {
                char aChar = strTemp[j];
                if(aChar >= '0' && aChar <= '9')
                    continue;
                return strTemp.Substring(0, j);
            }
            return "";
        }
    }



    /*
[3]	"style=\"padding-left:12px\" >de_modfit_85_bundle4_4s_south4s_1_1556550902_16369827"
[31]	"style=\"padding-left:12px\" >2, 9, 6"	string
2,4,6 etc for :"<td><a href=\"result.php?resultid=243774122\">243774122</a></td>\n<td><a href=\"show_host_detail.php?hostid=806834\">806834</a></td>\n\n        <td>15 Jun 2019, 3:28:12 UTC</td>\n        <td>15 Jun 2019, 4:47:31 UTC</td>\n        <td>Completed and validated</td>\n        <td align=right>43.34</td>\n        <td align=right>8.77</td>\n        <td align=right>227.53</td>\n        <td>Milkyway@home Separation v1.46 (opencl_ati_101)<br>windows_x86_64</td>\n        "	string
00000000011111111112222
12345678901234567890123
Completed and validated
     */
    public class cWorkUnit
    {
        public string name;    //de_modfit_83_bundle4_4s_south4s_1_1556550902_16315217
        public string ets;     // max # of error/total/success tasks	2, 9, 6
                        //Task	Computer	Sent	Time reported	Status	Run time	CPU time	Credit	Application
        public string strComputer;
        public string strStatus;
        public string Application;

        public bool HeaderInit(ref string s3, ref string s31)
        {
            int i = s3.IndexOf('>');
            if (i < 0) return false;
            name = s3.Substring(i+1);
            i = s31.LastIndexOf('>');
            if (i < 0) return false;
            ets = s31.Substring(i+1);
            return true;
        }
        public bool AddData(ref string s)
        { 
            strComputer = GetNumericTarget(ref  s, "?hostid=");
            string[] sTmp = s.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
            strStatus = sTmp[8].PadRight(23);
            Application = sTmp[13].Replace("<br>", " ");
            return true;
        }
        public string GetNumericTarget(ref string InString, string LookingFor)
        {
            string strTemp;
            int i = InString.IndexOf(LookingFor);
            int j = LookingFor.Length;
            if (i < 0) return "";
            i += j;
            strTemp = InString.Substring(i);
            for (j = 0; j < 12; j++) // should find terminator within 10 but who knows
            {
                char aChar = strTemp[j];
                if (aChar >= '0' && aChar <= '9')
                    continue;
                return strTemp.Substring(0, j);
            }
            return "";
        }
    }


}
