# DarkMode.cs
A C# Class for .NET Framework 2.0 - 4.8.1 (Windows 98 - Windows 11) to enable Dark Mode on Windows Forms.

By default only .NET 9.0 (no Framework) and higher supports Dark Mode natively, which is an optional download for Windows 10/11 only.

README: https://wiki.danit.nl/index.php?title=DarkMode.cs

Example: Default Light Mode / Dark Mode Enabled

![Preview](https://wiki.danit.nl/images/8/87/DarkMode_Example.png)

# Usage
 ![Preview](https://wiki.danit.nl/images/d/d4/DarkMode_Solution.png)
 
1. Add DarkMode.cs to the project solution.
2. Add `DarkMode.Enable(form);` to every Form that needs Dark Mode.
   
Example: On each Form Initialize Event
```csharp
public Form1()
{
    InitializeComponent();
    DarkMode.Enable(this);
}
```

Example: When creating a Form instance
```csharp
Form1 form = new Form1();
DarkMode.Enable(form);
form.Show();
```

# Inherit
You can also inherit the current Windows User Dark/Light Mode preference to this Form.

Usage: `DarkMode.Inherit(form);`

Can only Inherit on Windows Operating Systems that support Dark Mode (Window 10/11), otherwise Inherit will automatically default to standard Light Mode (change this with `DarkMode.UserDefault`).
