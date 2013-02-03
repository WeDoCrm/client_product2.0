using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class SendFileForm : Form
    {
        public SendFileForm()
        {
            InitializeComponent();
        }

        private void SendFileForm_Activated(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void SendFileForm_Deactivate(object sender, EventArgs e)
        {
            TopMost = false;
        }

        public void setProgress(int index)
        {
            if (index < 0)
            {
                progressBarSendFile.Visible = false;
            }
            else
            {
                if (!progressBarSendFile.Visible)
                {
                    progressBarSendFile.Visible = true;
                }
                if (index >= 0)
                    progressBarSendFile.Value = index;
                if (index == 100)
                    progressBarSendFile.Visible = false;
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_selectfile.Enabled = false;
        }

        public void DisplayInit()
        {
        }

        public void DisplayRun()
        {
        }

        public void DisplayError()
        {
        }

        public void DisplaySuccess()
        {
        }

        public void DisplayCancel()
        {
        }
    }
}