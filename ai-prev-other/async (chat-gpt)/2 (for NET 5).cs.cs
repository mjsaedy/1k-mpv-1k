// can you modify it to make the Main function async
/*
In this version, the Main method is declared as async Task. The WaitForExitAsync method is awaited using the await keyword instead of using ContinueWith. After the process exits, the Console.WriteLine statement is executed to display the process ID and exit code.

Note that because the Main method is async, the program will exit once the Main method completes, even if the process launched by the program is still running. To keep the program running until the launched process completes, you can add a call to Console.ReadLine at the end of the Main method.
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command line argument.");
                return;
            }

            string filePath = args[0];

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"The file {filePath} does not exist.");
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

                await process.WaitForExitAsync();

                Console.WriteLine($"Process {process.Id} has exited with code {process.ExitCode}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
