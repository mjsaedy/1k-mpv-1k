// In C#, write a console program that accepts a file path as a command line argument. It should launch the path and wait the process finishes asynchronously. Target the .NET Framework; this means you cannot use WaitForExitAsync()

/*
This program first checks if there is at least one command line argument provided, which is assumed to be the file path of the process to launch. If no argument is provided, the program will display an error message and exit.

Then, a ProcessStartInfo object is created with the specified file path and UseShellExecute set to true, which means the file will be opened using the default shell associated with the file type. This is necessary for some file types, such as .txt files, to be opened properly.

The Process object is created by calling Process.Start() with the ProcessStartInfo object as an argument. Then, WaitForExitAsync() is called with the Process object as an argument. This method returns a Task that will complete when the process has exited.

The WaitForExitAsync() method creates a TaskCompletionSource<bool> object, which is a way to create a Task that can be completed manually. Then, it sets the EnableRaisingEvents property of the Process object to true, which means the Exited event will be raised when the process has exited. Finally, it registers an event handler for the Exited event that completes the TaskCompletionSource<bool> object by calling TrySetResult(true).

The Main() method uses await to wait for the Task returned by WaitForExitAsync() to complete. When it does, the program prints a message indicating that the process has finished.
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{

    /* asyn Task Main() requires Roslyn compiler */
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a file path as a command line argument.");
            return;
        }

        string filePath = args[0];

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        };

        Process process = Process.Start(startInfo);

        await WaitForExitAsync(process);

        Console.WriteLine($"Process {process.Id} has finished.");
    }

    static Task WaitForExitAsync(Process process)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        process.EnableRaisingEvents = true;
        process.Exited += (sender, args) => tcs.TrySetResult(true);

        return tcs.Task;
    }
}


