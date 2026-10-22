namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
/// <summary>Reusable typed input dialog. The host and action buttons remain editable in Designer.</summary>
public partial class frmEntry : Form
{
    public frmEntry()
    {
        InitializeComponent();
        Shown += (_, _) => Ui.Fit(this);
    }

    private void Field(string title, Control editor, int height = 36)
    {
        var panel = new Panel
        {
            Width = 510,
            Height = height + 30,
            Margin = new Padding(0, 0, 0, 8)
        };
        panel.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = 26 });
        editor.SetBounds(0, 28, 490, height);
        panel.Controls.Add(editor);
        fields.Controls.Add(panel);
    }

    public TextBox TextField(string label, string value = "", bool multiline = false)
    {
        var box = new TextBox
        {
            Text = value,
            Multiline = multiline,
            ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None
        };
        Field(label, box, multiline ? 105 : 30);
        return box;
    }

    public NumericUpDown NumberField(string label, decimal value = 0, decimal max = 1_000_000_000m, int decimals = 0)
    {
        var box = new NumericUpDown
        {
            Minimum = 0,
            Maximum = max,
            DecimalPlaces = decimals,
            ThousandsSeparator = true,
            Value = Math.Min(max, Math.Max(0, value))
        };
        Field(label, box);
        return box;
    }

    public CheckBox BoolField(string label, bool value)
    {
        var box = new CheckBox
        {
            Text = label,
            Checked = value
        };
        Field("Trạng thái", box);
        return box;
    }

    public ComboBox ChoiceField(string label, IEnumerable<Choice> choices, string? selected = null)
    {
        var box = new ComboBox();
        Ui.Combo(box, choices);
        Ui.Select(box, selected);
        Field(label, box);
        return box;
    }

    public DateTimePicker DateField(string label, DateTime value)
    {
        var box = new DateTimePicker
        {
            Value = value,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy HH:mm"
        };
        Field(label, box);
        return box;
    }

    public TextBox ImageField(string initial)
    {
        var box = TextField("Đường dẫn ảnh món ăn (có thể bỏ trống)", initial);
        var preview = new PictureBox
        {
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.White
        };
        Field("Ảnh xem trước", preview, 150);
        void LoadImage()
        {
            preview.Image?.Dispose();
            preview.Image = null;
            if (File.Exists(box.Text))
            {
                try
                {
                    using var source = Image.FromFile(box.Text);
                    preview.Image = new Bitmap(source);
                }
                catch (ArgumentException)
                {
                }
            }
        }

        var pick = new Button
        {
            Text = "Chọn ảnh...",
            Width = 130,
            Height = 36
        };
        fields.Controls.Add(pick);
        pick.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                box.Text = dialog.FileName;
                LoadImage();
            }
        };
        FormClosed += (_, _) => preview.Image?.Dispose();
        LoadImage();
        return box;
    }

    public bool Edit(IWin32Window owner, string title, Action save)
    {
        Text = title;
        btnSave.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            Validate();
            save();
            DialogResult = DialogResult.OK;
            Close();
        }
        );
        return ShowDialog(owner) == DialogResult.OK;
    }
}
