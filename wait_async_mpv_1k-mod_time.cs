using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class LaunchAndWait {

    private Process myProcess;
    private TaskCompletionSource<bool> eventHandled;

    public async Task LaunchMPV(string fileName) {
        eventHandled = new TaskCompletionSource<bool>();
        using (myProcess = new Process()) {
            try {
                // Start a process and raise an event when done.
                myProcess.StartInfo.Arguments = String.Format("\"{0}\"", fileName);;
                myProcess.StartInfo.FileName = @"mpv.exe"; //mpv.exe should be on the PATH
                myProcess.StartInfo.Verb = "Open";
                //myProcess.StartInfo.CreateNoWindow = true;
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += new EventHandler(myProcess_Exited);
                myProcess.Start();
            } catch (Exception ex) {
                //Console.WriteLine("An error occurred trying to print \""  + fileName + "\":\n" + ex.Message);
                return;
            }
            // Wait for Exited event, but no more than 30 seconds.
            //await Task.WhenAny(eventHandled.Task, Task.Delay(30000));
            //mdsy: this seems unnecessary
            //from MS-Learn: "When this method returns the first completed task, the other tasks will continue running until completion."
            await Task.WhenAny(eventHandled.Task);
        }
    }

    // Handle Exited event
    private void myProcess_Exited(object sender, System.EventArgs e) {
        /*
        Console.WriteLine(
            String.Format("Exit time    : {0}\n", myProcess.ExitTime) +
            String.Format("Exit code    : {0}\n", myProcess.ExitCode) +
            String.Format("Elapsed time : {0}\n", Math.Round((myProcess.ExitTime - myProcess.StartTime).TotalMilliseconds))
        );
        */
        eventHandled.TrySetResult(true);
    }

    public static async Task Do_1k_mpv_1k(string filename) {
        
        //store original mod-time
        DateTime originalWriteTime;
        originalWriteTime = File.GetLastWriteTime(filename); //save mod time

        //1k
        SimpleHexViewer.Toggle1k.Do1k(filename);

        //launch mpv
        LaunchAndWait myProcess = new LaunchAndWait();
        await myProcess.LaunchMPV(filename);

        //1k
        Thread.Sleep(200); //wait, just in case
        SimpleHexViewer.Toggle1k.Do1k(filename);
        
        //restore original mod-time
        Thread.Sleep(100); //wait, just in case
         File.SetLastWriteTime(filename, originalWriteTime); //restore original mod time
    }

    [STAThread]
    static void Main(string[] args) {
        if (args.Length == 0) {
            //Console.WriteLine("Provide a file name.");
            return;
        }
        string filename = args[0];
        if (filename == string.Empty || !File.Exists(filename) ){
            MessageBox.Show("File not found!");
            return;
        }
        long length = new System.IO.FileInfo(filename).Length;
        if ( length < (1024*1024) ){ // 1MB, just an arbitrary value
            MessageBox.Show("File too small!");
            return;
        }
        Task.Run(async () => {
            //async execution
            await Do_1k_mpv_1k(filename);
        }).GetAwaiter().GetResult();
    }

}