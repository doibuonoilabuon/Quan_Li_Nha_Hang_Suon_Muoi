using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
public partial class ucStatCard : UserControl
{
    public ucStatCard()
    {
        InitializeComponent();
    }

    public void SetValues(string title, string value)
    {
        lblTitle.Text = title;
        lblValue.Text = value;
    }
}
