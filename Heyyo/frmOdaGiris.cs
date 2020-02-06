using System;
using System.Windows.Forms;

namespace Heyyo
{
    public partial class frmOdaGiris : Form
    {
        public string odaAdi;
        public string odaSifresi;

        public frmOdaGiris(string odaAdi)
        {
            InitializeComponent();
            this.odaAdi = odaAdi;
            lblBilgi.Text += odaAdi;
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            odaSifresi = txtOdaSifresi.Text;
            this.Close();
        }
    }
}
