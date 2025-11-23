using System;
using System.Diagnostics;


public static class Basic
{
    public static void BasicWinCmds(string cmd)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c \"{cmd}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine("\n--- Command Output ---");
        Console.Write(output);
        Console.Write(error);
        Console.WriteLine("----------------------");


        if (process.ExitCode != 0)
        {
            throw new Exception($"command '{cmd}' failed with error: {process.ExitCode}");
        }
    }
}
public class Program
{

    private static bool PostExecutionMenu()
    {
        Console.WriteLine("\nWhat would you like to do next?");
        Console.WriteLine("1) Go back to the main CMD list");
        Console.WriteLine("0) Exit the program");
        Console.Write("Enter your choice (0 or 1): ");
        
        string choice = Console.ReadLine().Trim();

        if (choice == "0")
        {
            Console.WriteLine("\nExiting application. Goodbye! 👋");
            return false; 
        }
        else if (choice == "1")
        {
            return true; 
        }
        else
        {
            Console.WriteLine("\nInvalid choice. Returning to the main CMD list by default.");
            return true; 
        }
    }

    public static void Main(string[] args)
    {
        bool isRunning = true;
        try
        {
            do
            {
                try
                {
                    Console.WriteLine("\n=== Welcome to Basic CMD Commands ===");

                    string list = @"1) Ping
2) Ipconfig
3) dir
4) Network statistics
5) Running Processes
6) Terminate Process
7) Systeminfo

0) exit";
                    Console.WriteLine(list);
                    Console.WriteLine("Enter a number to choose a command to execute: \n");
                    string choice = Console.ReadLine();

                    switch (choice.Trim())
                    {
                        case "1":
                            Console.Write("Enter a host/IP to ping (e.g., google.com): ");
                            string target = Console.ReadLine();
                            Basic.BasicWinCmds("Ping " + target);
                            isRunning = PostExecutionMenu();
                            break;
                        case "2":
                            Basic.BasicWinCmds("ipconfig");
                            isRunning = PostExecutionMenu();
                            break;
                        case "3":
                            Console.WriteLine("Enter a path: \n");
                            string path = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                Console.WriteLine("Path cannot be empty. Skipping command.");
                            }
                            else
                            {
                                Basic.BasicWinCmds("dir " + path);
                            }
                            isRunning = PostExecutionMenu();
                            break;
                        case "4":
                            Basic.BasicWinCmds("netstat -ano");
                            isRunning = PostExecutionMenu();
                            break;
                        case "5":
                            Basic.BasicWinCmds("tasklist /svc");
                            isRunning = PostExecutionMenu();
                            break;
                        case "6":
                            Console.WriteLine("Enter a Process ID: \n");
                            string id = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(id))
                            {
                                Console.WriteLine("Process ID cannot be empty. Skipping command.");
                            }
                            else
                            {
                                Basic.BasicWinCmds("taskkill /F /PID " + id);
                            }
                            isRunning = PostExecutionMenu();
                            break;
                        case "7":
                            Basic.BasicWinCmds("systeminfo");
                            isRunning = PostExecutionMenu();
                            break;
                        case "0":
                            isRunning = false;
                            Console.WriteLine("\nExiting application. Goodbye! 👋");
                            break;
                        default:
                            Console.WriteLine("\nInvalid choice. Returning to the main menu.");
                            break;
                    }
                }
                catch (Exception cmdEx)
                {
                    Console.Error.WriteLine($"\nCOMMAND EXECUTION FAILED: {cmdEx.Message}");
                    Console.WriteLine("Press any key to continue to the menu...");
                    Console.ReadKey(true);
                }

            } while (isRunning);


        }
        catch (Exception appEx)
        {
            Console.Error.WriteLine($"\nCRITICAL APPLICATION ERROR: {appEx.Message}");
            Environment.Exit(1);
        }

    }

}