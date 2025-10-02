using System;
using System.Diagnostics;
using System.Windows.Forms;

using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MdsyApps {

    class MdsyConsoleApp {
        
        [STAThread]
        public static void Main(string[] args) {
            /*if (args.Length > 1) {
                Console.WriteLine("...");
            } else {
                Console.WriteLine("\n\tUsage:");
            }*/

            //How to Wait for a Shelled Process to Finish
            //Get the path to the system folder.
            //string sysFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
            
            //Create a new process info structure.
            ProcessStartInfo pInfo = new ProcessStartInfo();
            
            //Set the file name member of the process info structure.
            pInfo.FileName = @"C:\Program Files\mpv\mpv.exe";
            pInfo.Arguments = String.Format("\"{0}\"", args[0]);
            
            //Start the process.
            Process p = Process.Start(pInfo);
            
            //Wait for the window to finish loading.
            p.WaitForInputIdle();
            //MessageBox.Show("windows ready...");  //didn't work
            
            //Wait for the process to end.
            p.WaitForExit();
            //MessageBox.Show("Code continuing...");
            Console.WriteLine("Code continuing...");



            //Console.WriteLine("\n\npress any key...\n");
            //Console.ReadKey();
        }


    } //class
} //namespace