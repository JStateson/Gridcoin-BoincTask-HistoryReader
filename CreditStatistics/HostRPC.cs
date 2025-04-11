using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;

namespace CreditStatistics
{
    internal class HostRPC
    {

        private static string r = Environment.NewLine;
        private static string[] StartTok = { "<project_name>", "<hostid>" };
        private static string[] StopTok = { "</project_name>", "</hostid>" };
        public static string sOut = "";
        public static bool bDone = true;
        public static int sErr = 0;
        public static bool bInScheduler = false;
        public static string sBuff;
        public void InitScheduler()
        {
            sOut = "";
            bInScheduler = true;
        }

        public void StopScheduler()
        {
            bInScheduler = false;
        }

        public string GetSchedulerResults()
        {
            bInScheduler = false;
            return sOut;
        }
        public bool InScheduler(){return bInScheduler;}
        public  bool SchedulerDone() { return bDone; }
        public int SchedulerError() { return sErr; }


        public int ReadStream(string hostname)
        {
            var ipx = IPAddress.Loopback;
            var port = 31416;
            string StatisticsRequest = "<boinc_gui_rpc_request>\n" +
                "<get_project_status/>\n" +
                "</boinc_gui_rpc_request>\n\u0003\u0001";//+ "\u0001";
            sBuff = string.Empty;
            int n = 0;
            int Numfound = 0;

            const int blockSize = 256;
            byte[] buffer = new byte[blockSize];

            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);

                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
                    {
                        ipx = ip;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            Socket client = new Socket(ipx.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(ipx, port));
            byte[] data = Encoding.UTF8.GetBytes(StatisticsRequest);
            client.ReceiveTimeout = 1000;
            client.Send(data);

            try
            {
                while (true)
                {
                                        
                    
                    try
                    {
                        n = client.Receive(buffer);
                       
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        break;
                    }
                    catch (SocketException ex)
                    {
                        break;
                    }



                    if (n > 0)
                    {
                        string s = Encoding.UTF8.GetString(buffer, 0, n);
                        sBuff += s;
                        //if (n < blockSize) break;
                        continue;
                    }
                    else break;
                }
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                n = 0;
                int i,j, nRec = 0;
                sOut += r + hostname + r;
                i = 0;
                while (i < sBuff.Length)
                {
                    int k = sBuff.IndexOf(StartTok[0],i);
                    if (k < 0)
                    {
                        return Numfound;
                    }
                    i = k;
                    j = sBuff.IndexOf(StopTok[0],i);
                    Debug.Assert(j > 0);
                    string sPROJ = sBuff.Substring(i + StartTok[0].Length, j - i - StartTok[0].Length);
                    i += StopTok[0].Length;
                    k = sBuff.IndexOf(StartTok[1],i);
                    if (k < 0) 
                    {
                        return Numfound;
                    }
                    i = k;
                    j = sBuff.IndexOf(StopTok[1], i);
                    Debug.Assert(j > 0);
                    string sProjID = sBuff.Substring(i + StartTok[1].Length, j - i - StartTok[1].Length);
                    i += StopTok[1].Length;
                    sOut += sPROJ + "," + sProjID + r;
                    Numfound++;
                }
                return Numfound;
            }
            finally
            {
                sErr = 0;
                bDone = true;
                //ProcessOUT(hostname, ref sBuff);
                client.Dispose();
            }
            return Numfound;
        }



        public static int ProcessOUT(string hostname, ref string sBuff)
        {
            int Numfound = 0;
            int i, j;
            sOut += r + hostname + r;
            i = 0;
            
            while (i < sBuff.Length)
            {
                int k = sBuff.IndexOf(StartTok[0], i);
                if (k < 0)
                {
                    return Numfound;
                }
                i = k;
                j = sBuff.IndexOf(StopTok[0], i);
                Debug.Assert(j > 0);
                string sPROJ = sBuff.Substring(i + StartTok[0].Length, j - i - StartTok[0].Length);
                i += StopTok[0].Length;
                k = sBuff.IndexOf(StartTok[1], i);
                if (k < 0)
                {
                    return Numfound;
                }
                i = k;
                j = sBuff.IndexOf(StopTok[1], i);
                Debug.Assert(j > 0);
                string sProjID = sBuff.Substring(i + StartTok[1].Length, j - i - StartTok[1].Length);
                i += StopTok[1].Length;
                sOut += sPROJ + "," + sProjID + r;
                Numfound++;
            }
            return Numfound;
        }



        // Replace 'using declarations' with explicit using statements to ensure compatibility with C# 7.3.  
        public static async Task GetHostInfo(string hostname)
        {
            string StatisticsRequest = "<boinc_gui_rpc_request>\n" +
    "<get_project_status/>\n" +
    "</boinc_gui_rpc_request>\n\u0003";//+ "\x01";
            string sBuff = string.Empty;
            int n = 0;
            bool bTrue = true;
            string r = Environment.NewLine;
            string[] StartTok = { "<project_name>", "<hostid>" };
            string[] StopTok = { "</project_name>", "</hostid>" };
            int Numfound = 0;
            var ipx = IPAddress.Loopback;
            string sTemp = "";
            bDone = false;
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);

                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
                    {
                        ipx = ip;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                sErr = 1;
                bDone = true;
                bTrue = false;
                return;
            }

            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(ipx, 31416);                

                NetworkStream stream = client.GetStream();
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(StatisticsRequest);
                    await stream.WriteAsync(data, 0, data.Length);
                    var buffer = new byte[1024*64];
                    //while (bTrue)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if(bytesRead == 0)
                        {
                            bTrue = false;
                        }
                        //sTemp = Encoding.UTF8.GetString(buffer);
                        //ProcessOUT(hostname, ref sTemp);
                        else sBuff += Encoding.UTF8.GetString(buffer);
                        //Application.DoEvents();
                    }

                }
                finally
                {
                    bTrue = false;
                    stream.Dispose();
                }

            }
            finally
            {
                client.Dispose();
                bTrue = false;
            }
            ProcessOUT(hostname, ref sBuff);
            sErr = 0;
            bDone = true;
        }

    }
}
