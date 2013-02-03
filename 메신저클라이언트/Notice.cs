using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Client.PopUp;

namespace Client
{
    public partial class Notice : FlashWindowForm
    {
        public event EventHandler<EventArgs> NoticeAlreadyRead;

        public Notice()
        {
            InitializeComponent();
        }

        public void SetNoticeInfo(string title, string content, string writerId, string writerName, string tagKey)
        {
            richTextBoxContent.Text = content;
            label_noticetitle.Tag = writerId;
            label_notice_sender.Text = writerName;
            label_noticetitle.Text = title;
            this.Tag = tagKey;
        }

        //private void Notice_SizeChanged(object sender, EventArgs e)
        //{
        //    int rightgap = this.Width - 433;

        //    textBox.Width = rightgap + 414;
            

        //    int heightgap = this.Height - 273;
        //    textBox.Height = heightgap + 153;

        //    label_noticetitle.SetBounds(97 + (rightgap / 2), label_noticetitle.Top, label_noticetitle.Width, label_noticetitle.Height);
        //    btn_confirm.SetBounds(168 + (rightgap / 2), btn_confirm.Top, btn_confirm.Width, btn_confirm.Height);
        //}

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (NoticeAlreadyRead != null)
            {
                NoticeAlreadyRead(this, e);
            }
            Close();
        }

        private void CopyCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBoxContent.Copy();
        }

        private void SelectAllCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBoxContent.SelectAll();
        }

    }
}