using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class SetExtensionForm : Form
    {
        public SetExtensionForm()
        {
            InitializeComponent();
        }

        private void SetExtensionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void tbx_extension_TextChanged(object sender, EventArgs e)
        {
            Int64 val;
            if (tbx_extension.Text != "" && !Int64.TryParse(tbx_extension.Text, out val))
            {
                MessageBox.Show(this, "숫자를 입력하세요.", "오류", MessageBoxButtons.OK);
                tbx_extension.Text = "";
            }
        }
    }
}
