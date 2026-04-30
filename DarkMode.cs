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
        if (darkmode)
        {
            form.BackColor = DarkBack1;
            form.ForeColor = Color.White;
        }
        else
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
                if (darkmode)
                {
                    tc.Appearance = TabAppearance.Normal;
                }
                break;*/

            case "TabPage":

                TabPage tp = (TabPage)control;
                if (darkmode)
                {
                    tp.BackColor = DarkBack2;
                }
                else
                {
                    tp.BackColor = SystemColors.Window;
                }
                break;

            case "Panel":

                Panel p = (Panel)control;
                if (darkmode)
                {
                    p.BackColor = DarkBack2;
                }
                else
                {
                    p.BackColor = SystemColors.ControlLight;
                }
                break;

            case "GroupBox":

                GroupBox gb = (GroupBox)control;
                if (darkmode)
                {
                    gb.BackColor = DarkBack2;
                    gb.ForeColor = DarkText;
                }
                else
                {
                    gb.BackColor = SystemColors.Control;
                    gb.ForeColor = LightText;
                }
                break;

            case "Label":

                Label lbl = (Label)control;
                if (darkmode)
                {
                    lbl.BackColor = Color.Transparent;
                }
                else
                {
                    lbl.BackColor = Color.Transparent;
                }
                break;

            case "LinkLabel":

                LinkLabel llbl = (LinkLabel)control;
                if (darkmode)
                {
                    llbl.BackColor = Color.Transparent;
                    llbl.LinkColor = Color.LightBlue;
                }
                else
                {
                    llbl.BackColor = Color.Transparent;
                    llbl.LinkColor = Color.FromArgb(0, 0, 255);
                }
                break;

            case "Button":

                Button btn = (Button)control;
                if (darkmode)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = DarkBack3;
                    btn.ForeColor = DarkText;
                }
                else
                {
                    btn.FlatStyle = FlatStyle.System;
                    btn.BackColor = SystemColors.Control;
                    btn.ForeColor = LightText;
                }
                break;

            case "TextBox":

                TextBox tb = (TextBox)control;
                if (darkmode)
                {
                    tb.BackColor = DarkBack3;
                    tb.ForeColor = DarkText;
                    if (tb.Enabled == false || tb.ReadOnly == true) { 
                        tb.BackColor = DarkBack1; tb.ForeColor = Color.LightGray; }
                }
                else
                {
                    tb.BackColor = SystemColors.Window;
                    tb.ForeColor = LightText;
                    if (tb.Enabled == false || tb.ReadOnly == true) {
                        tb.BackColor = SystemColors.Control; tb.ForeColor = Color.DimGray; }
                }
                break;

            case "MaskedTextBox":

                MaskedTextBox mtb = (MaskedTextBox)control;
                if (darkmode)
                {
                    mtb.BackColor = DarkBack3;
                    mtb.ForeColor = DarkText;
                    if (mtb.Enabled == false || mtb.ReadOnly == true) { 
                        mtb.BackColor = DarkBack1; mtb.ForeColor = Color.LightGray; }
                }
                else
                {
                    mtb.BackColor = SystemColors.Window;
                    mtb.ForeColor = LightText;
                    if (mtb.Enabled == false || mtb.ReadOnly == true) { 
                        mtb.BackColor = SystemColors.Control; mtb.ForeColor = Color.DimGray; }
                }
                break;

            case "RichTextBox":

                RichTextBox rtb = (RichTextBox)control;
                if (darkmode)
                {
                    rtb.BackColor = DarkBack3;
                    rtb.ForeColor = DarkText;
                    if (rtb.Enabled == false || rtb.ReadOnly == true) { 
                        rtb.BackColor = DarkBack1; rtb.ForeColor = Color.LightGray; }
                }
                else
                {
                    rtb.BackColor = SystemColors.Window;
                    rtb.ForeColor = LightText;
                    if (rtb.Enabled == false || rtb.ReadOnly == true) { 
                        rtb.BackColor = SystemColors.Control; rtb.ForeColor = Color.DimGray; }
                }
                break;

            case "NumericUpDown":

                NumericUpDown nud = (NumericUpDown)control;
                if (darkmode)
                {
                    nud.BackColor = DarkBack3;
                    nud.ForeColor = DarkText;
                    if (nud.Enabled == false || nud.ReadOnly == true) {
                        nud.BackColor = DarkBack1; nud.ForeColor = Color.LightGray; }
                }
                else
                {
                    nud.BackColor = SystemColors.Window;
                    nud.ForeColor = LightText;
                    if (nud.Enabled == false || nud.ReadOnly == true) {
                        nud.BackColor = SystemColors.Control; nud.ForeColor = Color.DimGray; }
                }
                break;

            case "ComboBox":

                ComboBox cb = (ComboBox)control;
                if (darkmode)
                {
                    cb.BackColor = DarkBack3;
                    cb.ForeColor = DarkText;
                }
                else
                {
                    cb.BackColor = SystemColors.Window;
                    cb.ForeColor = LightText;
                }
                break;

            case "ListBox":

                ListBox lb = (ListBox)control;
                if (darkmode)
                {
                    lb.BackColor = DarkBack3;
                    lb.ForeColor = DarkText;
                }
                else
                {
                    lb.BackColor = SystemColors.Window;
                    lb.ForeColor = LightText;
                }
                break;

            case "CheckedListBox":

                CheckedListBox clb = (CheckedListBox)control;
                if (darkmode)
                {
                    clb.BackColor = DarkBack2;
                    clb.ForeColor = DarkText;
                }
                else
                {
                    clb.BackColor = SystemColors.Window;
                    clb.ForeColor = LightText;
                }
                break;

            case "ListView":

                ListView lv = (ListView)control;
                if (darkmode)
                {
                    lv.BackColor = DarkBack2;
                    lv.ForeColor = DarkText;
                }
                else
                {
                    lv.BackColor = SystemColors.Window;
                    lv.ForeColor = LightText;
                }
                break;

            case "TreeView":

                TreeView tv = (TreeView)control;
                if (darkmode)
                {
                    tv.BackColor = DarkBack3;
                    tv.ForeColor = DarkText;
                }
                else
                {
                    tv.BackColor = SystemColors.Window;
                    tv.ForeColor = LightText;
                }
                break;

            case "StatusStrip":

                StatusStrip ss = (StatusStrip)control;
                if (darkmode)
                {
                    ss.BackColor = DarkToolStrip;
                    ss.RenderMode = ToolStripRenderMode.Professional;
                    ss.Renderer = new ToolStripDarkRenderer();
                }
                else
                {
                    ss.BackColor = LightToolStrip;
                    ss.RenderMode = ToolStripRenderMode.Professional;
                    ss.Renderer = new ToolStripLightRenderer();
                }

                ToolStripItems(ss.Items, darkmode);
                break;

            case "MenuStrip":

                MenuStrip ms = (MenuStrip)control;
                if (darkmode)
                {
                    ms.BackColor = DarkToolStrip;
                    ms.Renderer = new ToolStripDarkRenderer();
                }
                else
                {
                    ms.BackColor = LightToolStrip;
                    ms.Renderer = new ToolStripLightRenderer();
                }

                ToolStripItems(ms.Items, darkmode);
                break;

            case "ContextMenuStrip":

                ContextMenuStrip cms = (ContextMenuStrip)control;
                if (darkmode)
                {
                    cms.BackColor = DarkToolStrip;
                    cms.Renderer = new ToolStripDarkRenderer();
                }
                else
                {
                    cms.BackColor = LightToolStrip;
                    cms.Renderer = new ToolStripLightRenderer();
                }

                ToolStripItems(cms.Items, darkmode);
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

                if (darkmode)
                {
                    cms.BackColor = DarkToolStrip;
                    cms.Renderer = new ToolStripDarkRenderer();
                }
                else
                {
                    cms.BackColor = LightToolStrip;
                    cms.Renderer = new ToolStripLightRenderer();
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

                if (darkmode)
                {
                    tsmi.BackColor = DarkToolStrip;
                    tsmi.ForeColor = DarkText;
                }
                else
                {
                    tsmi.BackColor = LightToolStrip;
                    tsmi.ForeColor = LightText;
                }
            }

            if (item is ToolStripTextBox)
            {
                ToolStripTextBox tstb = (ToolStripTextBox)item;
                tstb.BorderStyle = BorderStyle.FixedSingle;

                if (darkmode)
                {
                    tstb.BackColor = DarkBack1;
                    tstb.ForeColor = DarkText;
                }
                else
                {
                    tstb.BackColor = SystemColors.Control;
                    tstb.ForeColor = LightText;
                }
            }

            if (item is ToolStripComboBox)
            {
                ToolStripComboBox tscb = (ToolStripComboBox)item;

                if (darkmode)
                {
                    tscb.ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    tscb.ComboBox.BackColor = DarkBack1;
                    tscb.ForeColor = DarkText;
                }
                else
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

    class ToolStripDarkRenderer : ToolStripProfessionalRenderer
    {
        public ToolStripDarkRenderer() : base(new ToolStripDark()) { }

        class ToolStripDark : ProfessionalColorTable
        {
            // [MenuStrip]
            public override Color MenuItemSelectedGradientBegin
            {
                get { return SystemColors.HotTrack; }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return SystemColors.HotTrack; }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return DarkBack3; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return DarkBack3; }
            }
            
            // [StatusStrip]
            public override Color ButtonSelectedBorder
            {
                get { return SystemColors.ActiveCaption; }
            }
            public override Color ButtonSelectedGradientBegin
            {
                get { return SystemColors.HotTrack; }
            }
            public override Color ButtonSelectedGradientMiddle
            {
                get { return SystemColors.HotTrack; }
            }
            public override Color ButtonSelectedGradientEnd
            {
                get { return SystemColors.HotTrack; }
            }

            // [ToolStripItems]
            public override Color ToolStripDropDownBackground
            {
                get { return DarkToolStrip; }
            }

            public override Color MenuItemBorder
            {
                get { return SystemColors.ActiveCaption; }
            }

            public override Color MenuItemSelected
            {
                get { return SystemColors.HotTrack; }
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

    class ToolStripLightRenderer : ToolStripProfessionalRenderer
    {
        public ToolStripLightRenderer() : base(new ToolStripLight()) { }

        public class ToolStripLight : ProfessionalColorTable
        {
            // For ToolStripItems:
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