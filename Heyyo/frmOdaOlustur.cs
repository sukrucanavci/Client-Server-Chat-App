using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace Heyyo
{
    public partial class frmOdaOlustur : Form
    {
        public string odaAdi = null;
        public string odaSifresi = null;

        public frmOdaOlustur()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }

        private void btnOdaOlustur_Click(object sender, EventArgs e)
        {
            if (txtOdaAdi.TextLength < 3) { return; }
            //if (odaAdi.Substring(0, 3).Contains(' ')) { return; }

            odaAdi = txtOdaAdi.Text;
            odaSifresi = txtOdaSifresi.Text;

            txtOdaAdi.Clear();
            txtOdaSifresi.Clear();

            this.Hide();
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            odaAdi = null;
            odaSifresi = null;

            this.Hide();
        }
    }
}
