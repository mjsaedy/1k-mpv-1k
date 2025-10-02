using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

class LaunchAndWait {

    private Process myProcess;
    private TaskCompletionSource<bool> eventHandled;

    public async Task LaunchMPV(string fileName) {
        eventHandled = new TaskCompletionSource<bool>();

        using (myProcess = new Process()) {
            try {
                myProcess.StartInfo.FileName = "mpv.exe"; // must be in PATH
                myProcess.StartInfo.Arguments = $"\"{fileName}\"";
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += (sender, e) => eventHandled.TrySetResult(true);

                myProcess.Start();
            } catch (Exception ex) {
                MessageBox.Show($"Failed to launch mpv:\n{ex.Message}");
                return;
            }

            // Wait for process to exit
            await eventHandled.Task;
        }
    }

    public static async Task Do1kMpv1kAsync(string filename) {
        DateTime originalWriteTime = File.GetLastWriteTime(filename);

        try {
            // First 1k modification
            SimpleHexViewer.Toggle1k.Do1k(filename);

            // Launch mpv and wait
            LaunchAndWait launcher = new LaunchAndWait();
            await launcher.LaunchMPV(filename);

            // Wait a bit, then revert first 1k
            await Task.Delay(200);
            SimpleHexViewer.Toggle1k.Do1k(filename);
        } finally {
            // Restore original modification time
            await Task.Delay(100);
            File.SetLastWriteTime(filename, originalWriteTime);
        }
    }

    [STAThread]
    static void Main(string[] args) {
        if (args.Length == 0) return;

        string filename = args[0];
        if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename)) {
            MessageBox.Show("File not found!");
            return;
        }

        long length = new FileInfo(filename).Length;
        if (length < 1024 * 1024) { // arbitrary 1MB check
            MessageBox.Show("File too small!");
            return;
        }

        try {
            Task.Run(async () => await Do1kMpv1kAsync(filename))
                .GetAwaiter()
                .GetResult();
        } catch (Exception ex) {
            MessageBox.Show($"An error occurred:\n{ex.Message}");
        }
    }
}
