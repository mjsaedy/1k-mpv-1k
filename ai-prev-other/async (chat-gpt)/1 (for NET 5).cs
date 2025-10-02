// In C#, write a console program that accepts a file path as a command line argument. It should launch the path and wait the process finishes asynchronously.
/*
The Main method first checks if a file path was provided as a command line argument. If not, it displays a message and returns.

If a file path is provided, it checks if the file exists. If not, it displays a message and returns.

If the file exists, it creates a new Process object with the provided file path and starts it. It then waits for the process to exit asynchronously using the WaitForExitAsync method, which returns a Task that completes when the process exits. When the Task completes, the ContinueWith method is called with a lambda expression that displays a message indicating the process ID and exit code.

If an error occurs, it displays an error message.
*/

using System;
using System.Diagnostics;

namespace ConsoleProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command line argument.");
                return;
            }

            string filePath = args[0];

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("The file {0} does not exist.", filePath);
                return;
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true,
                        CreateNoWindow = true,
                    }
                };

                process.Start();

                process.WaitForExitAsync().ContinueWith(task =>
                {
                    Console.WriteLine("Process {0} has exited with code {1}.", process.Id, process.ExitCode);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
            }
        }
    }
}


