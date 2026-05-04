using System;
using System.Drawing; // Color
using System.Windows.Forms; // Form
using System.ComponentModel; // IContainer
using System.Runtime.InteropServices; // DllImport
using Microsoft.Win32; // Registry

public class DarkMode
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

    public static void TitleBar(Form form, bool darkmode)
    {
        if (OS.StartsWith("Windows 10") || OS.StartsWith("Windows 11"))
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkmode, Marshal.SizeOf(darkmode));
        }
    }

    public static void Form(Form form, bool darkmode)
    {
        if (darkmode)
        {
            form.BackColor = DarkBack1;
            form.ForeColor = DarkText;
        }
        else
        {
            form.BackColor = SystemColors.Control;
            form.ForeColor = SystemColors.ControlText;
        }

        FormControls(form, darkmode);
        DesignerControls(form, darkmode);
    }

    private static void FormControls(Control control, bool darkmode)
    {
        System.Reflection.MethodInfo method = typeof(DarkMode.Controls).GetMethod(control.GetType().Name);
        if (method != null) method.Invoke(null, new object[] { control, darkmode });

        foreach (Control childcontrol in control.Controls)
        {
            FormControls(childcontrol, darkmode);
        }
    }

    private static void DesignerControls(Form form, bool darkmode)
    {
        IContainer components = (IContainer)form.GetType().GetField("components", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(form);
        if (components == null) { return; }

        foreach (IComponent component in components.Components)
        {
            if (component is ContextMenuStrip) DarkMode.Controls.ContextMenuStrip((ContextMenuStrip)component, darkmode);
        }
    }

    private static void ToolStripItems(ToolStripItemCollection items, bool darkmode)
    {
        foreach (ToolStripItem item in items)
        {
            if (item is ToolStripMenuItem)
            {
                DarkMode.Controls.ToolStripMenuItem((ToolStripMenuItem)item, darkmode);
            }

            if (item is ToolStripTextBox)
            {
                DarkMode.Controls.ToolStripTextBox((ToolStripTextBox)item, darkmode);
            }

            if (item is ToolStripComboBox)
            {
                DarkMode.Controls.ToolStripComboBox((ToolStripComboBox)item, darkmode);
            }

            if (item is ToolStripDropDownItem)
            {
                ToolStripItems(((ToolStripDropDownItem)item).DropDownItems, darkmode);
            }
        }
    }

    public class Controls
    {
        public static void TabControl(TabControl tabcontrol, bool darkmode)
        {
            if (darkmode)
            {
                tabcontrol.DrawMode = TabDrawMode.OwnerDrawFixed;

                StringFormat format = new StringFormat(); format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Center;
                tabcontrol.DrawItem += delegate(object sender, DrawItemEventArgs e)
                {
                    e.Graphics.FillRectangle(new SolidBrush(DarkMode.DarkBack1), new Rectangle(0, 0, (int)e.Graphics.ClipBounds.Width, 20));

                    Rectangle rectangle = tabcontrol.GetTabRect(tabcontrol.SelectedIndex);
                    e.Graphics.FillRectangle(new SolidBrush(DarkMode.DarkBack3), new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));

                    for (int i = 0; i < tabcontrol.TabPages.Count; i++) { e.Graphics.DrawString(tabcontrol.TabPages[i].Text, e.Font, new SolidBrush(DarkMode.DarkText), tabcontrol.GetTabRect(i), format); }
                };
            }
            else
            {
                tabcontrol.DrawMode = TabDrawMode.Normal;
            }
        }

        public static void TabPage(TabPage tabpage, bool darkmode)
        {
            if (darkmode)
            {
                tabpage.BackColor = DarkMode.DarkBack3;
            }
            else
            {
                tabpage.BackColor = SystemColors.Window;
            }
        }

        public static void Panel(Panel panel, bool darkmode)
        {
            if (darkmode)
            {
                panel.BackColor = DarkBack2;
            }
            else
            {
                panel.BackColor = SystemColors.ControlLight;
            }
        }

        public static void GroupBox(GroupBox groupbox, bool darkmode)
        {
            if (darkmode)
            {
                groupbox.BackColor = DarkBack2;
                groupbox.ForeColor = DarkText;
            }
            else
            {
                groupbox.BackColor = SystemColors.Control;
                groupbox.ForeColor = LightText;
            }
        }

        public static void LinkLabel(LinkLabel linklabel, bool darkmode)
        {
            if (darkmode)
            {
                linklabel.LinkColor = Color.LightBlue;
            }
            else
            {
                linklabel.LinkColor = Color.Blue;
            }
        }
        
        public static void Button(Button button, bool darkmode)
        {
            if (darkmode)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = DarkBack3;
                button.ForeColor = DarkText;
            }
            else
            {
                button.FlatStyle = FlatStyle.System;
                button.BackColor = SystemColors.Control;
                button.ForeColor = LightText;
            }
        }

        public static void TextBox(TextBox textbox, bool darkmode)
        {
            if (darkmode)
            {
                textbox.BackColor = DarkBack3;
                textbox.ForeColor = DarkText;
                if (textbox.Enabled == false || textbox.ReadOnly == true)
                {
                    textbox.BackColor = DarkBack1; textbox.ForeColor = Color.LightGray;
                }
            }
            else
            {
                textbox.BackColor = SystemColors.Window;
                textbox.ForeColor = LightText;
                if (textbox.Enabled == false || textbox.ReadOnly == true)
                {
                    textbox.BackColor = SystemColors.Control; textbox.ForeColor = Color.DimGray;
                }
            }
        }

        public static void MaskedTextBox(MaskedTextBox maskedtextbox, bool darkmode)
        {
            if (darkmode)
            {
                maskedtextbox.BackColor = DarkBack3;
                maskedtextbox.ForeColor = DarkText;
                if (maskedtextbox.Enabled == false || maskedtextbox.ReadOnly == true)
                {
                    maskedtextbox.BackColor = DarkBack1; maskedtextbox.ForeColor = Color.LightGray;
                }
            }
            else
            {
                maskedtextbox.BackColor = SystemColors.Window;
                maskedtextbox.ForeColor = LightText;
                if (maskedtextbox.Enabled == false || maskedtextbox.ReadOnly == true)
                {
                    maskedtextbox.BackColor = SystemColors.Control; maskedtextbox.ForeColor = Color.DimGray;
                }
            }
        }

        public static void RichTextBox(RichTextBox richtextbox, bool darkmode)
        {
            if (darkmode)
            {
                richtextbox.BackColor = DarkBack3;
                richtextbox.ForeColor = DarkText;
                if (richtextbox.Enabled == false || richtextbox.ReadOnly == true)
                {
                    richtextbox.BackColor = DarkBack1; richtextbox.ForeColor = Color.LightGray;
                }
            }
            else
            {
                richtextbox.BackColor = SystemColors.Window;
                richtextbox.ForeColor = LightText;
                if (richtextbox.Enabled == false || richtextbox.ReadOnly == true)
                {
                    richtextbox.BackColor = SystemColors.Control; richtextbox.ForeColor = Color.DimGray;
                }
            }
        }

        public static void NumericUpDown(NumericUpDown numericupdown, bool darkmode)
        {
            if (darkmode)
            {
                numericupdown.BackColor = DarkBack3;
                numericupdown.ForeColor = DarkText;
                if (numericupdown.Enabled == false || numericupdown.ReadOnly == true)
                {
                    numericupdown.BackColor = DarkBack1; numericupdown.ForeColor = Color.LightGray;
                }
            }
            else
            {
                numericupdown.BackColor = SystemColors.Window;
                numericupdown.ForeColor = LightText;
                if (numericupdown.Enabled == false || numericupdown.ReadOnly == true)
                {
                    numericupdown.BackColor = SystemColors.Control; numericupdown.ForeColor = Color.DimGray;
                }
            }
        }

        public static void ComboBox(ComboBox combobox, bool darkmode)
        {
            if (darkmode)
            {
                combobox.BackColor = DarkBack3;
                combobox.ForeColor = DarkText;
            }
            else
            {
                combobox.BackColor = SystemColors.Window;
                combobox.ForeColor = LightText;
            }
        }

        public static void ListBox(ListBox listbox, bool darkmode)
        {
            if (darkmode)
            {
                listbox.BackColor = DarkBack3;
                listbox.ForeColor = DarkText;
            }
            else
            {
                listbox.BackColor = SystemColors.Window;
                listbox.ForeColor = LightText;
            }
        }

        public static void CheckedListBox(CheckedListBox checkedlistbox, bool darkmode)
        {
            if (darkmode)
            {
                checkedlistbox.BackColor = DarkBack3;
                checkedlistbox.ForeColor = DarkText;
            }
            else
            {
                checkedlistbox.BackColor = SystemColors.Window;
                checkedlistbox.ForeColor = LightText;
            }
        }

        public static void ListView(ListView listview, bool darkmode)
        {
            if (darkmode)
            {
                listview.BackColor = DarkBack3;
                listview.ForeColor = DarkText;
            }
            else
            {
                listview.BackColor = SystemColors.Window;
                listview.ForeColor = LightText;
            }
        }

        public static void TreeView(TreeView treeview, bool darkmode)
        {
            if (darkmode)
            {
                treeview.BackColor = DarkBack3;
                treeview.ForeColor = DarkText;
            }
            else
            {
                treeview.BackColor = SystemColors.Window;
                treeview.ForeColor = LightText;
            }
        }

        public static void StatusStrip(StatusStrip statusstrip, bool darkmode)
        {
            if (darkmode)
            {
                statusstrip.BackColor = DarkToolStrip;
                statusstrip.Renderer = new ToolStripDarkRenderer();
            }
            else
            {
                statusstrip.BackColor = LightToolStrip;
                statusstrip.Renderer = new ToolStripLightRenderer();
            }
            ToolStripItems(statusstrip.Items, darkmode);
        }

        public static void MenuStrip(MenuStrip menustrip, bool darkmode)
        {
            if (darkmode)
            {
                menustrip.BackColor = DarkToolStrip;
                menustrip.Renderer = new ToolStripDarkRenderer();
            }
            else
            {
                menustrip.BackColor = LightToolStrip;
                menustrip.Renderer = new ToolStripLightRenderer();
            }
            ToolStripItems(menustrip.Items, darkmode);
        }

        public static void ContextMenuStrip(ContextMenuStrip contextnenustrip, bool darkmode)
        {
            if (darkmode)
            {
                contextnenustrip.BackColor = DarkToolStrip;
                contextnenustrip.Renderer = new ToolStripDarkRenderer();
            }
            else
            {
                contextnenustrip.BackColor = LightToolStrip;
                contextnenustrip.Renderer = new ToolStripLightRenderer();
            }
            ToolStripItems(contextnenustrip.Items, darkmode);
        }

        public static void DataGridView(DataGridView datagridview, bool darkmode)
        {
            if (darkmode)
            {
                datagridview.BackgroundColor = GradientGray(23);
                datagridview.ForeColor = DarkText;

                datagridview.GridColor = GradientGray(40);
                datagridview.DefaultCellStyle.BackColor = GradientGray(50);
                //datagridview.AlternatingRowsDefaultCellStyle.BackColor = ;

                datagridview.EnableHeadersVisualStyles = false;
                datagridview.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                datagridview.ColumnHeadersDefaultCellStyle.BackColor = GradientGray(32);
                datagridview.ColumnHeadersDefaultCellStyle.ForeColor = DarkText;

                datagridview.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                datagridview.RowHeadersDefaultCellStyle.BackColor = GradientGray(32);
                datagridview.RowHeadersDefaultCellStyle.ForeColor = DarkText;
                datagridview.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(188, 220, 244);
                datagridview.RowHeadersDefaultCellStyle.SelectionForeColor = LightText;
            }
            else
            {
                datagridview.BackgroundColor = DarkMode.GradientGray(171);
                datagridview.ForeColor = LightText;

                datagridview.GridColor = DarkMode.GradientGray(160);
                datagridview.DefaultCellStyle.BackColor = Color.White;
                //datagridview.AlternatingRowsDefaultCellStyle.BackColor = ;

                datagridview.EnableHeadersVisualStyles = true;
                datagridview.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                datagridview.ColumnHeadersDefaultCellStyle.ForeColor = LightText;

                datagridview.RowHeadersDefaultCellStyle.BackColor = Color.White;
                datagridview.RowHeadersDefaultCellStyle.ForeColor = LightText;
            }
        }

        public static void ToolStripMenuItem(ToolStripMenuItem toolstripmenuitem, bool darkmode)
        {
            if (darkmode)
            {
                toolstripmenuitem.BackColor = DarkToolStrip;
                toolstripmenuitem.ForeColor = DarkText;
            }
            else
            {
                toolstripmenuitem.BackColor = LightToolStrip;
                toolstripmenuitem.ForeColor = LightText;
            }
        }

        public static void ToolStripTextBox(ToolStripTextBox toolstriptextbox, bool darkmode)
        {
            toolstriptextbox.BorderStyle = BorderStyle.FixedSingle;
            if (darkmode)
            {
                toolstriptextbox.BackColor = DarkBack1;
                toolstriptextbox.ForeColor = DarkText;
            }
            else
            {
                toolstriptextbox.BackColor = SystemColors.Control;
                toolstriptextbox.ForeColor = LightText;
            }
        }

        public static void ToolStripComboBox(ToolStripComboBox toolstripcombobox, bool darkmode)
        {
            if (darkmode)
            {
                toolstripcombobox.ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                toolstripcombobox.ComboBox.BackColor = DarkBack1;
                toolstripcombobox.ForeColor = DarkText;
            }
            else
            {
                toolstripcombobox.ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
                toolstripcombobox.ComboBox.BackColor = SystemColors.Control;
                toolstripcombobox.ForeColor = LightText;
            }
        }

        class ToolStripDarkRenderer : ToolStripProfessionalRenderer
        {
            public ToolStripDarkRenderer() : base(new ToolStripDark()) { }

            class ToolStripDark : ProfessionalColorTable
            {
                // [StatusStrip]
                public override Color ButtonSelectedBorder
                {
                    get { return Color.FromArgb(153, 180, 209); }
                }
                public override Color ButtonSelectedGradientBegin
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }
                public override Color ButtonSelectedGradientMiddle
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }
                public override Color ButtonSelectedGradientEnd
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }

                // [MenuStrip]
                public override Color MenuItemSelectedGradientBegin
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }

                public override Color MenuItemSelectedGradientEnd
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }

                public override Color MenuItemPressedGradientBegin
                {
                    get { return DarkBack3; }
                }

                public override Color MenuItemPressedGradientEnd
                {
                    get { return DarkBack3; }
                }

                // [ToolStripItem]
                public override Color ToolStripDropDownBackground
                {
                    get { return DarkToolStrip; }
                }

                public override Color MenuItemBorder
                {
                    get { return Color.FromArgb(153, 180, 209); }
                }

                public override Color MenuItemSelected
                {
                    get { return Color.FromArgb(0, 102, 204); }
                }

                public override Color ImageMarginGradientBegin
                {
                    //get { return SystemColors.Control; }
                    get { return DarkToolStrip; }
                }

                public override Color ImageMarginGradientMiddle
                {
                    //get { return SystemColors.Control; }
                    get { return DarkToolStrip; }
                }

                public override Color ImageMarginGradientEnd
                {
                    //get { return SystemColors.Control; }
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
                // [ToolStripItem]
                public override Color ToolStripDropDownBackground
                {
                    get { return LightToolStrip; }
                }

                public override Color ImageMarginGradientBegin
                {
                    //get { return GradientGray(252); } // Default
                    get { return LightToolStrip; }
                }

                public override Color ImageMarginGradientMiddle
                {
                    //get { return GradientGray(247); } // Default
                    get { return LightToolStrip; }
                }

                public override Color ImageMarginGradientEnd
                {
                    //get { return GradientGray(241); } // Default
                    get { return LightToolStrip; }
                }
            }
        }
    }
}