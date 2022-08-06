using System.Reflection;
using DcsExportLib.Models;

namespace DCSExporterApp
{
    internal class ConsoleAppManager
    {
        public DcsModuleInfo PromptSelectModule(ICollection<DcsModuleInfo> modules)
        {
            Console.WriteLine("Choose one of the modules below:");
            
            ListInstalledModules(modules);

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
                Console.WriteLine($"{counter}\t- {dcsModuleInfo.Name}");
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
    }
}
