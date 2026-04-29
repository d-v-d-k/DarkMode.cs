# DarkMode.cs
A C# Class file for .NET Framework 2.0 - 4.8.1 (Windows 98 - Windows 11) to enable Dark Mode on Windows Forms.

By default only .NET 9.0 (no Framework) and higher supports Dark Mode natively, which is an optional download for Windows 10/11 only.

README: https://wiki.danit.nl/index.php?title=DarkMode.cs

_Example:_ Default Light Mode / Dark Mode

![Preview](https://wiki.danit.nl/images/8/87/DarkMode_Example.png)

# Usage
 ![Preview](https://wiki.danit.nl/images/d/d4/DarkMode_Solution.png)
 
Add DarkMode.cs to the project solution.

## Enable
Add `DarkMode.Enable(form);` to every Form that needs Dark Mode.

Place your custom style changes after this.

_Example:_ On each Form Initialize Event
```csharp
public Form1()
{
    InitializeComponent();
    DarkMode.Enable(this);
}
```

_Example:_ When creating a Form instance
```csharp
Form1 form = new Form1();
DarkMode.Enable(form);
form.Show();
```

_Example:_ When a Button gets clicked
```csharp
private void buttonDarkMode_Click(object sender, EventArgs e)
{
    DarkMode.Enable(this);
}
```

## Inherit
You can also instead inherit the current Windows User Dark/Light Mode preference to this Form.

Usage: `DarkMode.Inherit(form);`

Can only Inherit on Windows versions that support Dark Mode (Window 10/11), otherwise Inherit will automatically default to standard Light Mode (change this with `DarkMode.UserDefault`).

## Disable
Revert this Form back to standard 'Light Mode' style defaults.

Usage: `DarkMode.Disable(form);`
