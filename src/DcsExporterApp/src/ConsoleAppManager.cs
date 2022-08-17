using System.Reflection;
using DcsExportLib.Models;

namespace DCSExporterApp
{
    internal class ConsoleAppManager
    {
        // TODO: remove when not needed
        private readonly string[] _notWorkingModules =
        {
            //"M-2000C", 
            //"MiG-21bis", 
            //"Mirage F1", 
           //"NS430"
        };

        public DcsModuleInfo PromptSelectModule(ICollection<DcsModuleInfo> modules, string searchedPath)
        {
            Console.WriteLine("Below is the list of your detected installed modules.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"* DCS path was set to: {searchedPath}");
            Console.WriteLine("* List contains only modules with exportable data");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            
            ListInstalledModules(modules);

            Console.WriteLine();

            int optionInt = PromptUserNumberEntry("Type the number of module you would like to export and press enter:", 1, modules.Count);

            return modules.ElementAt(optionInt - 1);
        }

        public void DisplayExportStartedMessage(DcsModuleInfo exportedModule)
        {
            Console.Clear();
            Console.WriteLine($"Exporting module: { exportedModule.Name} ...");
        }

        public void SetConsoleTitle()
        {
            string versionInfo = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            Console.Title = $"DCS Button IDs Export Tool by Miroslav Janousek (version { versionInfo})";
        }

        private void ListInstalledModules(ICollection<DcsModuleInfo> modules)
        {
            if (modules.Count== 0)
            {
                return;
            }

            int counter = 1;

            foreach (var dcsModuleInfo in modules)
            {
                Console.Write($"{counter}\t- {dcsModuleInfo.Name}");
                //string lineStr = $"{counter}\t- {dcsModuleInfo.Name}";

                if (_notWorkingModules.Contains(dcsModuleInfo.Name))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" (not working yet)");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine();
                counter++;
            }
        }

        private int PromptUserNumberEntry(string infoStr, int min, int max)
        {
            while (true)
            {
                Console.Write(infoStr);
                string? userEntry = Console.ReadLine();

                if (int.TryParse(userEntry, out int optionInt))
                {
                    if (optionInt <= max && optionInt >= min) return optionInt;
                }

                Console.WriteLine("Incorrect entry");
            }
        }

        public void ShowNoExportedModuleMessage()
        {
            Console.WriteLine("Sorry! Module was not exported.");
        }

        public void NotifyException(string message, Exception? exception = null)
        {
            Console.Clear();
            Console.WriteLine($"Error encountered: {message}");

            if(exception != null)
                Console.WriteLine($"Exception message: {exception.Message}");
        }

        public void NotifyWrongSettings(ICollection<string> settingsErrors)
        {
            Console.Clear();
            Console.WriteLine("Some application settings are missing or incorrect.");

            if(settingsErrors.Count > 0)
                Console.WriteLine();

            foreach (var error in settingsErrors)
            {
                Console.WriteLine($"\t- {error}");   
            }

            if (settingsErrors.Count > 0)
                Console.WriteLine();

            Console.WriteLine("Check the appsettings.json file.");
        }

        public void PromptExitConfirmation(bool clearConsole = false)
        {
            if(clearConsole)
                Console.Clear();

            Console.WriteLine("Press enter to exit the application");
            Console.ReadLine();
        }

        public void NotifyExportDone()
        {
            Console.WriteLine("Export was successful!");
        }
    }
}
