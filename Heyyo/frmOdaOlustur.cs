﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Heyyo
{
    public partial class frmOdaOlustur : Form
    {
        private string olusturan;
        public frmOdaOlustur(string olusturan)
        {
            InitializeComponent();
            this.olusturan = olusturan;
        }

        public string komut;

        private void btnOdaOlustur_Click(object sender, EventArgs e)
        {
            string odaAdi = txtOdaAdi.Text;
            string sifre = txtOdaSifresi.Text;

            komut = "odaEkle:" + odaAdi + ":" + sifre + ":" + olusturan;
            this.Close();
        }

        private void chkSifre_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkSifre.Checked)
            {
                txtOdaSifresi.Clear();
            }
            txtOdaSifresi.Enabled = chkSifre.Checked ? true : false ;
        }
    }
}
