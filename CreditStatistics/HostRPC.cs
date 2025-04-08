using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace CreditStatistics
{
    internal class HostRPC
    {
        public bool GetHostInfo(string IP, ref string sOut)
        {
            var ip = IPAddress.Loopback;
            var port = 31416;
            string StatisticsRequest = "<boinc_gui_rpc_request>\n" +
                "<get_project_status/>\n" +
                "</boinc_gui_rpc_request>\n\u0003";//+ "\x01";
            string sBuff = string.Empty;
            int n = 0;
            string r = Environment.NewLine;
            string[] StartTok = { "<project_name>", "<hostid>" };
            string[] StopTok = { "</project_name>", "</hostid>" };

            Socket client = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(new IPEndPoint(ip, port));
                Console.WriteLine("Connected to server.");

                byte[] data = Encoding.UTF8.GetBytes(StatisticsRequest);
                client.Send(data);
                Console.WriteLine($"Sent: {StatisticsRequest}");

                var buffer = new byte[256];
                while (true)
                {
                    n = client.Receive(buffer); 
                    if(n>0)
                    {
                        string s = Encoding.UTF8.GetString(buffer, 0, n);
                        if(s.Contains("</projects>")) break;
                        sBuff += s;
                        if (n < 256) break;
                    }
                }
                n = 0;
                int i,j, nRec = 0;
                sOut += IP + r;
                i = 0;
                while (n < sBuff.Length)
                {
                    i = sBuff.IndexOf(StartTok[0],i);
                    if (i < 0) return nRec > 0;
                    j = sBuff.IndexOf(StopTok[0],i);
                    Debug.Assert(j > 0);
                    string sPROJ = sBuff.Substring(i + StartTok[0].Length, j - i - StartTok[0].Length);
                    i += StopTok[0].Length;
                    i = sBuff.IndexOf(StartTok[1],i);
                    if (i < 0) return nRec > 0;
                    j = sBuff.IndexOf(StopTok[1], i);
                    Debug.Assert(j > 0);
                    string sProjID = sBuff.Substring(i + StartTok[1].Length, j - i - StartTok[1].Length);
                    i += StopTok[1].Length;
                    sOut += sPROJ + "," + sProjID + r;
                }
                return true;
            }
            finally
            {
                client.Dispose();
            }
            return false;
        }
    }
}
