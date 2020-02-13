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
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            this.odaAdi = odaAdi;
            lblBilgi.Text += odaAdi;
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (txtOdaSifresi.TextLength == 0) { return; }
            odaSifresi = txtOdaSifresi.Text;
            this.Close();
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            odaSifresi = null;
            this.Close();
        }
    }
}
