# ASP.NET Control Scanner

ASP.NET Control Scanner is a simple command-line tool written in C# to scan a specified project directory for various ASP.NET web forms controls (e.g., `<asp:GridView>`, `<asp:TextBox>`) within `.aspx`, `.ascx`, and `.cshtml` files. It then generates a CSV inventory of these controls and their file locations.

## Features

* Scans a user-specified directory recursively.
* Filters for files with `.aspx`, `.ascx`, and `.cshtml` extensions.
* Identifies a predefined list of common ASP.NET server controls.
    * The list of controls to search for can be easily modified in the source code (`Program.cs`).
* Outputs the findings (file path and control type) to a CSV file named `aspnet_control_inventory.csv` in the scanned directory.
* Provides console feedback during the scanning process and error handling for unreadable files or directories.

## Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) (Version 6.0 or later recommended, as the project template defaults to recent versions).

## How to Build

1.  Clone the repository:
    ```bash
    git clone [https://github.com/your-username/aspnet-control-scanner.git](https://github.com/your-username/aspnet-control-scanner.git)
    cd aspnet-control-scanner
    ```
2.  Build the project:
    ```bash
    dotnet build --configuration Release
    ```

## How to Run

After building, you can run the tool from the project's root directory:

```bash
dotnet run --project AspNetControlScanner/AspNetControlScanner.csproj
```

## Example CSV Output (aspnet_control_inventory.csv)
```bash
File,Control
"C:\path\to\your\webapp\Default.aspx","asp:Label"
"C:\path\to\your\webapp\UserControls\MyControl.ascx","asp:TextBox"
"C:\path\to\your\webapp\Admin\Settings.aspx","asp:GridView"
```