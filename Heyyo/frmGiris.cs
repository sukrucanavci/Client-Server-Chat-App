using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heyyo
{
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }

        public string kullaniciAdi;
        public IPEndPoint lep;

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            if (txtIP.Text.Length == 0 || txtPort.Text.Length == 0)
            {
                MessageBox.Show("Lütfen bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            kullaniciAdi = txtKullaniciAdi.Text;

            string ip = txtIP.Text;
            int port = Convert.ToInt32(txtPort.Text);
            lep = new IPEndPoint(IPAddress.Parse(ip), port);

            this.Hide();

            foreach (frmAna frm in Application.OpenForms.OfType<frmAna>())
                frm.Baglan();
        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            string ka = txtKullaniciAdi.Text;

            if (ka.Length < 3 || ka.Substring(0, 3).Contains(" "))
            {
                btnBaglan.Enabled = false;
            }
            else
            {
                btnBaglan.Enabled = true;
            }
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
