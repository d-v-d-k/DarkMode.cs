using System;
using System.Drawing; // Color
using System.Windows.Forms; // Form
using System.ComponentModel; // IContainer
using System.Runtime.InteropServices; // DllImport
using Microsoft.Win32; // Registry

public static class DarkMode
{
    public static bool UserDefault = false;
    public static Color DarkBack1 = GradientGray(31);
    public static Color DarkBack2 = GradientGray(47);
    public static Color DarkBack3 = GradientGray(63);
    public static Color DarkText = Color.White;
    public static Color LightText = Color.Black;
    public static Color DarkToolStrip = GradientGray(43);
    public static Color LightToolStrip = Color.White;

    private static Color GradientGray(int gradient) { return Color.FromArgb(gradient, gradient, gradient); }
    private static string OS = Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", null));

    public static void Inherit(Form form)
    {
        if (OS.StartsWith("Windows 10") || OS.StartsWith("Windows 11"))
        {
            RegistryKey rkUserDefault = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", false);
            UserDefault = !Convert.ToBoolean(rkUserDefault.GetValue("AppsUseLightTheme", true));
        }

        if (UserDefault == true)
        {
            Apply(form, true);
        }
        /*if (UserDefault == false)
        {
            Apply(form, false);
        }*/
    }

    public static void Enable(Form form)
    {
        Apply(form, true);
    }

    public static void Disable(Form form)
    {
        Apply(form, false);
    }

    private static void Apply(Form form, bool darkmode)
    {
        form.SuspendLayout();

        TitleBar(form, darkmode);
        Form(form, darkmode);

        form.ResumeLayout(true);

        if (OS.StartsWith("Windows 10") || OS.StartsWith("Windows 11")) // Title Bar graphics glitch fix
        {
            FormWindowState fwsOriginal = form.WindowState;
            form.WindowState = FormWindowState.Minimized;
            System.Threading.Thread.Sleep(100); // Wait
            form.WindowState = fwsOriginal;
        }

        form.Invalidate(true);
        form.Update();
        form.Activate();
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref bool attrValue, int attrSize);

    private static void TitleBar(Form form, bool darkmode)
    {
        if (OS.StartsWith("Windows 10") || OS.StartsWith("Windows 11"))
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkmode, Marshal.SizeOf(darkmode));
        }
    }

    private static void Form(Form form, bool darkmode)
    {
        if (darkmode == true)
        {
            form.BackColor = DarkBack1;
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
            /*case "TabControl":
                TabControl tc = (TabControl)control;
                if (darkmode == true)
                {
                    tc.Appearance = TabAppearance.Normal;
                }
                break;*/

            case "TabPage":
                TabPage tp = (TabPage)control;
                if (darkmode == true)
                {
                    tp.BackColor = DarkBack2;
                }
                else if (darkmode == false)
                {
                    tp.BackColor = SystemColors.Window;
                }
                break;

            case "GroupBox":
                GroupBox gb = (GroupBox)control;
                if (darkmode == true)
                {
                    gb.BackColor = DarkBack2;
                    gb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    //gb.BackColor = SystemColors.ControlLight;
                    gb.BackColor = SystemColors.Control;
                    gb.ForeColor = LightText;
                }
                break;

            case "Panel":
                Panel p = (Panel)control;
                if (darkmode == true)
                {
                    p.BackColor = DarkBack2;
                }
                else if (darkmode == false)
                {
                    p.BackColor = SystemColors.ControlLight;
                }
                break;

            case "Label":
                Label lbl = (Label)control;
                if (darkmode == true)
                {
                    lbl.BackColor = Color.Transparent;
                }
                else if (darkmode == false)
                {
                    lbl.BackColor = Color.Transparent;
                }
                break;

            case "LinkLabel":
                LinkLabel llbl = (LinkLabel)control;
                if (darkmode == true)
                {
                    llbl.BackColor = Color.Transparent;
                    llbl.LinkColor = Color.LightBlue;
                }
                else if (darkmode == false)
                {
                    llbl.BackColor = Color.Transparent;
                    llbl.LinkColor = Color.FromArgb(0, 0, 255);
                }
                break;

            case "Button":
                Button btn = (Button)control;

                if (darkmode == true)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = DarkBack3;
                    btn.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    btn.FlatStyle = FlatStyle.System;
                    btn.BackColor = SystemColors.Control;
                    btn.ForeColor = LightText;
                }
                break;

            case "TextBox":
                TextBox tb = (TextBox)control;
                if (darkmode == true)
                {
                    tb.BackColor = DarkBack3;
                    tb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    tb.BackColor = SystemColors.Window;
                    tb.ForeColor = LightText;
                }
                break;

            case "MaskedTextBox":
                MaskedTextBox mtb = (MaskedTextBox)control;
                if (darkmode == true)
                {
                    mtb.BackColor = DarkBack3;
                    mtb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    mtb.BackColor = SystemColors.Window;
                    mtb.ForeColor = LightText;
                }
                break;

            case "RichTextBox":
                RichTextBox rtb = (RichTextBox)control;
                if (darkmode == true)
                {
                    rtb.BackColor = DarkBack3;
                    rtb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    rtb.BackColor = SystemColors.Window;
                    rtb.ForeColor = LightText;
                }
                break;

            case "ComboBox":
                ComboBox cb = (ComboBox)control;
                if (darkmode == true)
                {
                    cb.BackColor = DarkBack3;
                    cb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    cb.BackColor = SystemColors.Window;
                    cb.ForeColor = LightText;
                }
                break;

            case "NumericUpDown":
                NumericUpDown nud = (NumericUpDown)control;
                if (darkmode == true)
                {
                    nud.BackColor = DarkBack3;
                    nud.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    nud.BackColor = SystemColors.Window;
                    nud.ForeColor = LightText;
                }
                break;

            case "ListBox":
                ListBox lb = (ListBox)control;
                if (darkmode == true)
                {
                    lb.BackColor = DarkBack3;
                    lb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    lb.BackColor = SystemColors.Window;
                    lb.ForeColor = LightText;
                }
                break;

            case "CheckedListBox":
                CheckedListBox clb = (CheckedListBox)control;
                if (darkmode == true)
                {
                    clb.BackColor = DarkBack2;
                    clb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    clb.BackColor = SystemColors.Window;
                    clb.ForeColor = LightText;
                }
                break;

            case "ListView":
                ListView lv = (ListView)control;
                if (darkmode == true)
                {
                    lv.BackColor = DarkBack2;
                    lv.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    lv.BackColor = SystemColors.Window;
                    lv.ForeColor = LightText;
                }
                break;

            case "TreeView":
                TreeView tv = (TreeView)control;
                if (darkmode == true)
                {
                    tv.BackColor = DarkBack3;
                    tv.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    tv.BackColor = SystemColors.Window;
                    tv.ForeColor = LightText;
                }
                break;

            case "MenuStrip":
                MenuStrip ms = (MenuStrip)control;
                if (darkmode == true)
                {
                    ms.BackColor = DarkToolStrip;
                    ms.Renderer = new DarkToolStripRenderer();

                }
                if (darkmode == false)
                {
                    ms.BackColor = LightToolStrip;
                    ms.Renderer = new LightToolStripRenderer();
                }

                ToolStripItems(ms.Items, darkmode);
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
                ContextMenuStrip cms = (ContextMenuStrip)component;

                if (darkmode == true)
                {
                    cms.BackColor = DarkToolStrip;
                    cms.Renderer = new DarkToolStripRenderer();
                }
                if (darkmode == false) 
                {
                    cms.BackColor = LightToolStrip;
                    cms.Renderer = new LightToolStripRenderer();
                }

                ToolStripItems(cms.Items, darkmode);
            }
        }
    }

    private static void ToolStripItems(ToolStripItemCollection items, bool darkmode)
    {
        foreach (ToolStripItem item in items)
        {
            if (item is ToolStripMenuItem)
            {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;

                if (darkmode == true)
                {
                    tsmi.BackColor = DarkToolStrip;
                    tsmi.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    tsmi.BackColor = LightToolStrip;
                    tsmi.ForeColor = LightText;
                }
            }

            if (item is ToolStripTextBox)
            {
                ToolStripTextBox tstb = (ToolStripTextBox)item;
                tstb.BorderStyle = BorderStyle.FixedSingle;

                if (darkmode == true)
                {
                    tstb.BackColor = DarkBack1;
                    tstb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    tstb.BackColor = SystemColors.Control;
                    tstb.ForeColor = LightText;
                }
            }

            if (item is ToolStripComboBox)
            {
                ToolStripComboBox tscb = (ToolStripComboBox)item;

                if (darkmode == true)
                {
                    tscb.ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    tscb.ComboBox.BackColor = DarkBack1;
                    tscb.ForeColor = DarkText;
                }
                if (darkmode == false)
                {
                    tscb.ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
                    tscb.ComboBox.BackColor = SystemColors.Control;
                    tscb.ForeColor = LightText;
                }
            }

            if (item is ToolStripDropDownItem)
            {
                ToolStripItems(((ToolStripDropDownItem)item).DropDownItems, darkmode);
            }
        }
    }

    class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new ToolStripDark()) { }

        class ToolStripDark : ProfessionalColorTable
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
                get { return DarkBack3; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return DarkBack3; }
            }

            public override Color MenuItemSelected
            {
                get { return SystemColors.Highlight; }
            }

            public override Color ToolStripDropDownBackground
            {
                get { return DarkToolStrip; }
            }

            public override Color ImageMarginGradientBegin
            {
                get { return DarkToolStrip; }
            }

            public override Color ImageMarginGradientMiddle
            {
                get { return DarkToolStrip; }
            }

            public override Color ImageMarginGradientEnd
            {
                get { return DarkToolStrip; }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(DarkToolStrip), 31, 0, e.Item.Width, e.Item.Height);
            e.Graphics.DrawLine(new Pen(GradientGray(189)), 31, e.Item.Height / 2, e.Item.Width, e.Item.Height / 2);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
    }

    class LightToolStripRenderer : ToolStripProfessionalRenderer
    {
        public LightToolStripRenderer() : base(new ToolStripLight()) { }

        public class ToolStripLight : ProfessionalColorTable
        {

            public override Color ToolStripDropDownBackground
            {
                get { return LightToolStrip; }
            }

            public override Color ImageMarginGradientBegin
            {
                //get { return GradientGray(252); }
                get { return LightToolStrip; }
            }

            public override Color ImageMarginGradientMiddle
            {
                //get { return GradientGray(247); }
                get { return LightToolStrip; }
            }

            public override Color ImageMarginGradientEnd
            {
                //get { return GradientGray(241); }
                get { return LightToolStrip; }
            }
        }
    }
}