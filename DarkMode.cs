using System;
using System.Drawing; // Color
using System.Windows.Forms; // Form
using System.ComponentModel; // IContainer
using System.Runtime.InteropServices; // DllImport
using Microsoft.Win32; // Registry

public static class DarkMode
{
    public static bool UserDefault = false;
    public static bool FlatStyle = true;
    public static Color DarkText = Color.White;
    public static Color LightText = Color.Black;
    public static Color GrayScale1 = ColorGradientGray(31);
    public static Color GrayScale2 = ColorGradientGray(47);
    public static Color GrayScale3 = ColorGradientGray(63);

    private static Color ColorGradientGray(int gradient) { return Color.FromArgb(gradient, gradient, gradient); }

    public static void Inherit(Form form)
    {
        if (Environment.OSVersion.Version.Major >= 6) // Windows Vista or higher
        {
            RegistryKey rkUserDefault = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", false);
            UserDefault = !Convert.ToBoolean(rkUserDefault.GetValue("AppsUseLightTheme", true));
        }

        if (UserDefault == true)
        {
            form.Invalidate();

            TitleBar(form, true);
            Form(form, true);
            RefreshForm(form);
        }
        /*else if (UserDefault == false)
        {
            form.Invalidate();
            
            TitleBar(form, false);
            Form(form, false);
            RefreshForm(form);
        }*/
    }

    public static void Enable(Form form)
    {
        form.Invalidate();

        TitleBar(form, true);
        Form(form, true);
        RefreshForm(form);
    }

    public static void Disable(Form form)
    {
        form.Invalidate();

        TitleBar(form, false);
        Form(form, false);
        RefreshForm(form);
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref bool attrValue, int attrSize);

    private static void TitleBar(Form form, bool darkmode)
    {
        if (Environment.OSVersion.Version.Major >= 6) // Windows Vista or higher
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkmode, Marshal.SizeOf(darkmode)); // Windows 10 + 11
        }
    }

    private static void RefreshForm(Form form)
    {
        FormWindowState fwsOriginal = form.WindowState;
        form.WindowState = FormWindowState.Minimized;
        System.Threading.Thread.Sleep(100); // Wait
        form.WindowState = fwsOriginal;
    }

    private static void Form(Form form, bool darkmode)
    {
        if (darkmode == true)
        {
            form.BackColor = GrayScale1;
            form.ForeColor = Color.White;
        }
        if (darkmode == false)
        {
            form.BackColor = SystemColors.Control;
            form.ForeColor = SystemColors.ControlText;
        }

        Controls(form, darkmode);
        DesignerControls(form, darkmode);
    }

    private static void Controls(Control control, bool darkmode)
    {
        switch (control.GetType().Name)
        {
            case "TabControl":
                TabControl TabControl = (TabControl)control;
                if (darkmode == true)
                {
                    TabControl.Appearance = TabAppearance.Normal;
                }
                break;

            case "TabPage":
                TabPage TabPage = (TabPage)control;
                if (darkmode == true)
                {
                    TabPage.BackColor = GrayScale2;
                }
                else if (darkmode == false)
                {
                    TabPage.BackColor = SystemColors.Window;
                }
                break;

            case "GroupBox":
                GroupBox GroupBox = (GroupBox)control;
                if (darkmode == true)
                {
                    GroupBox.BackColor = GrayScale2;
                    GroupBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    GroupBox.BackColor = SystemColors.ControlLight;
                    GroupBox.ForeColor = LightText;
                }
                break;

            case "Panel":
                Panel Panel = (Panel)control;
                if (darkmode == true)
                {
                    Panel.BackColor = GrayScale2;
                }
                else if (darkmode == false)
                {
                    Panel.BackColor = SystemColors.ControlLight;
                }
                break;

            case "Label":
                Label Label = (Label)control;
                if (darkmode == true)
                {
                    Label.BackColor = Color.Transparent;
                }
                else if (darkmode == false)
                {
                    Label.BackColor = Color.Transparent;
                }
                break;

            case "LinkLabel":
                LinkLabel LinkLabel = (LinkLabel)control;
                if (darkmode == true)
                {
                    LinkLabel.BackColor = Color.Transparent;
                    LinkLabel.LinkColor = Color.LightBlue;
                }
                else if (darkmode == false)
                {
                    LinkLabel.BackColor = Color.Transparent;
                    LinkLabel.LinkColor = Color.FromArgb(0, 0, 255);
                }
                break;

            case "Button":
                Button Button = (Button)control;

                if (FlatStyle == true) Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                //else if (FlatStyle == false) Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;

                if (darkmode == true)
                {
                    Button.BackColor = GrayScale3;
                }
                if (darkmode == false)
                {
                    //Button.BackColor = SystemColors.ControlLight;
                    Button.BackColor = Color.Transparent;
                }
                break;

            case "TextBox":
                TextBox TextBox = (TextBox)control;
                if (darkmode == true)
                {
                    TextBox.BackColor = GrayScale3;
                    TextBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    TextBox.BackColor = SystemColors.Window;
                    TextBox.ForeColor = LightText;
                }
                break;

            case "MaskedTextBox":
                MaskedTextBox MaskedTextBox = (MaskedTextBox)control;
                if (darkmode == true)
                {
                    MaskedTextBox.BackColor = GrayScale3;
                    MaskedTextBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    MaskedTextBox.BackColor = SystemColors.Window;
                    MaskedTextBox.ForeColor = LightText;
                }
                break;

            case "RichTextBox":
                RichTextBox RichTextBox = (RichTextBox)control;
                if (darkmode == true)
                {
                    RichTextBox.BackColor = GrayScale3;
                    RichTextBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    RichTextBox.BackColor = SystemColors.Window;
                    RichTextBox.ForeColor = LightText;
                }
                break;

            case "ComboBox":
                ComboBox ComboBox = (ComboBox)control;
                if (darkmode == true)
                {
                    ComboBox.BackColor = GrayScale3;
                    ComboBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    ComboBox.BackColor = SystemColors.Window;
                    ComboBox.ForeColor = LightText;
                }
                break;

            case "NumericUpDown":
                NumericUpDown NumericUpDown = (NumericUpDown)control;
                if (darkmode == true)
                {
                    NumericUpDown.BackColor = GrayScale3;
                    NumericUpDown.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    NumericUpDown.BackColor = SystemColors.Window;
                    NumericUpDown.ForeColor = LightText;
                }
                break;

            case "ListBox":
                ListBox ListBox = (ListBox)control;
                if (darkmode == true)
                {
                    ListBox.BackColor = GrayScale3;
                    ListBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    ListBox.BackColor = SystemColors.Window;
                    ListBox.ForeColor = LightText;
                }
                break;

            case "CheckedListBox":
                CheckedListBox CheckedListBox = (CheckedListBox)control;
                if (darkmode == true)
                {
                    CheckedListBox.BackColor = GrayScale2;
                    CheckedListBox.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    CheckedListBox.BackColor = SystemColors.Window;
                    CheckedListBox.ForeColor = LightText;
                }
                break;

            case "ListView":
                ListView ListView = (ListView)control;
                if (darkmode == true)
                {
                    ListView.BackColor = GrayScale2;
                    ListView.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    ListView.BackColor = SystemColors.Window;
                    ListView.ForeColor = LightText;
                }
                break;

            case "TreeView":
                TreeView TreeView = (TreeView)control;
                if (darkmode == true)
                {
                    TreeView.BackColor = GrayScale3;
                    TreeView.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    TreeView.BackColor = SystemColors.Window;
                    TreeView.ForeColor = LightText;
                }
                break;

            case "MenuStrip":
                MenuStrip MenuStrip = (MenuStrip)control;
                if (darkmode == true)
                {
                    MenuStrip.BackColor = GrayScale1;
                    MenuStrip.Renderer = new ToolStripProfessionalRenderer(new ToolStripDark());
                    
                }
                if (darkmode == false)
                {
                    MenuStrip.BackColor = SystemColors.Control;
                    MenuStrip.Renderer = new ToolStripProfessionalRenderer(new ToolStripLight());
                }

                ToolStripItems(MenuStrip.Items, darkmode);
                break;
        }

        foreach (Control child in control.Controls)
        {
            Controls(child, darkmode);
        }
    }

    private static void DesignerControls(Form form, bool darkmode)
    {
        IContainer components = (IContainer)form.GetType().GetField("components", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(form);
        if (components == null) { return; }

        foreach (IComponent component in components.Components)
        {
            if (component is ContextMenuStrip)
            {
                if (darkmode == true) ((ContextMenuStrip)component).Renderer = new ToolStripProfessionalRenderer(new ToolStripDark());
                if (darkmode == false) ((ContextMenuStrip)component).Renderer = new ToolStripProfessionalRenderer(new ToolStripLight());

                ToolStripItems(((ContextMenuStrip)component).Items, darkmode);
            }
        }
    }

    private static void ToolStripItems(ToolStripItemCollection items, bool darkmode)
    {
        foreach (ToolStripItem item in items)
        {
            if (darkmode == true)
            {
                item.BackColor = GrayScale1;
                item.ForeColor = DarkText;
            }
            if (darkmode == false)
            {
                item.BackColor = SystemColors.Control;
                item.ForeColor = SystemColors.ControlText;
            }

            if (item is ToolStripDropDownItem)
            {
                ToolStripItems(((ToolStripDropDownItem)item).DropDownItems, darkmode);
            }
        }
    }

    public class ToolStripDark : ProfessionalColorTable
    {
        public override Color MenuItemBorder
        {
            get { return SystemColors.MenuHighlight; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return SystemColors.Highlight; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return SystemColors.Highlight; }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return GrayScale3; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return GrayScale3; }
        }

        public override Color MenuItemSelected
        {
            get { return SystemColors.Highlight; }
        }
    }

    public class ToolStripLight : ProfessionalColorTable
    {

    }
}