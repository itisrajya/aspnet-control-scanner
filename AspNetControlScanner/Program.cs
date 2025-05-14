using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetControlScanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string projectDir;

            // Get project directory from user
            while (true)
            {
                Console.Write("Enter the full path to the project directory to scan: ");
                projectDir = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(projectDir))
                {
                    Console.WriteLine("Project directory cannot be empty. Please try again.");
                }
                else if (!Directory.Exists(projectDir))
                {
                    Console.WriteLine($"The directory '{projectDir}' does not exist. Please enter a valid path.");
                }
                else
                {
                    break; // Valid directory entered
                }
                Console.WriteLine(); // Add a blank line for readability before next prompt
            }

            Console.WriteLine($"Scanning directory: {projectDir}...");

            // Original list of specific controls
            string[] controls = {
                "asp:GridView", "asp:Label", "asp:ListBox", "asp:RadioButton",
                "asp:TextBox", "asp:DataList", "asp:DropDownList", "asp:Repeater",
                "asp:Button", "asp:LinkButton", "asp:ImageButton", "asp:CheckBox",
                "asp:CheckBoxList", "asp:RadioButtonList", "asp:Literal", "asp:Panel",
                "asp:PlaceHolder", "asp:HiddenField", "asp:Table", "asp:Image",
                "asp:Calendar", "asp:AdRotator", "asp:FileUpload", "asp:Wizard",
                "asp:MultiView", "asp:View", "asp:Login", "asp:LoginView",
                "asp:LoginStatus", "asp:LoginName", "asp:CreateUserWizard",
                "asp:PasswordRecovery", "asp:ChangePassword"
                // Add more controls as needed
            };

            // To scan for any <asp: tag (like your second script), you can use:
            // string[] controls = { "asp:" };

            string[] fileExtensions = { ".aspx", ".ascx", ".cshtml" }; // .cshtml for Razor pages that might embed ASP.NET controls in some scenarios
            var output = new List<(string filePath, string controlFound)>();

            try
            {
                foreach (string file in Directory.EnumerateFiles(projectDir, "*.*", SearchOption.AllDirectories))
                {
                    if (fileExtensions.Contains(Path.GetExtension(file).ToLowerInvariant())) // Use ToLowerInvariant for case-insensitive extension comparison
                    {
                        string content;
                        try
                        {
                            content = File.ReadAllText(file);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Warning: Could not read file '{file}'. Error: {ex.Message}. Skipping.");
                            continue; // Skip unreadable files
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine($"Warning: No access to file '{file}'. Error: {ex.Message}. Skipping.");
                            continue; // Skip files without access
                        }


                        foreach (string control in controls)
                        {
                            if (content.IndexOf("<" + control, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                output.Add((file, control));
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error: Could not access parts of the directory '{projectDir}'. Error: {ex.Message}.");
                Console.WriteLine("Please ensure the application has necessary permissions.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred during file scanning: {ex.Message}");
                return;
            }


            if (output.Any())
            {
                // Write results to CSV
                string csvFileName = "aspnet_control_inventory.csv";
                string csvPath = Path.Combine(projectDir, csvFileName);

                try
                {
                    using (var writer = new StreamWriter(csvPath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("File,Control");
                        foreach (var entry in output.OrderBy(o => o.filePath).ThenBy(o => o.controlFound))
                        {
                            writer.WriteLine($"\"{entry.filePath}\",\"{entry.controlFound}\"");
                        }
                    }
                    Console.WriteLine($"Scan complete. Results saved to: {csvPath}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error writing CSV file to '{csvPath}'. Error: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Error: No permission to write CSV file to '{csvPath}'. Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Scan complete. No matching ASP.NET controls found in the specified file types.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}