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
    public partial class MemoForm : FlashWindowForm
    {
        public event EventHandler<CustomEventArgs> MemoReplyDone;
        private string senderId;

        public string getSenderId()
        {
            return senderId;
        }

        public MemoForm()
        {
            InitializeComponent();
            this.Enter += new EventHandler(MemoForm_Enter);
            this.MemoRe.Enter += new EventHandler(MemoForm_Enter);
        }

        void MemoForm_Enter(object sender, EventArgs e)
        {
            base.StopFlash();
        }

        public void setMemoInfo(string senderId, string senderName, string memoContent)
        {
            this.senderId = senderId;
            this.Text = senderName + "님의 쪽지";
            this.richTextBoxMemo.Text =memoContent;//.Split(new string[] { Environment.NewLine }, StringSplitOptions.None); //줄바꿈 지원 2012.9.1
        }
        //private void MemoForm_SizeChanged(object sender, EventArgs e)
        //{
        //    int rightgap = this.Width - 357;
        //    MemoCont.Width = rightgap + 341;
        //    MemoRe.Width = rightgap + 265;

        //    int heightgap = this.Height - 193;
        //    MemoCont.Height = heightgap + 107;

        //    MemoRe.SetBounds(MemoRe.Left, 108 + heightgap, MemoRe.Width, MemoRe.Height);

        //    Memobtn.SetBounds(Memobtn.Left, 109 + heightgap, Memobtn.Width, Memobtn.Height);
        //    Memobtn.SetBounds(271+rightgap, Memobtn.Top, Memobtn.Width, Memobtn.Height);
        //}
        
        private void richTextBoxMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.ControlKey)
            {
                switch (e.KeyData) {
                    case Keys.C:
                        richTextBoxMemo.Copy();
                        break;
                    //case Keys.P:
                    //    richTextBoxMemo.Paste();
                    //    break;
                    //case Keys.x:
                    //    richTextBoxMemo.Cut();
                    //    break;
                    case Keys.A:
                        richTextBoxMemo.SelectAll();
                        break;
                }
            }
        }

        private void richTextBoxReply_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.ControlKey)
            {
                switch (e.KeyData)
                {
                    case Keys.C:
                        richTextBoxReply.Copy();
                        break;
                    case Keys.P:
                        richTextBoxReply.Paste();
                        break;
                    case Keys.X:
                        richTextBoxReply.Cut();
                        break;
                    case Keys.A:
                        richTextBoxReply.SelectAll();
                        break;
                }
            }
        }

        private void Memobtn_Click(object sender, EventArgs e)
        {
            if (richTextBoxReply.Text.Trim().Length == 0)
            {
                richTextBoxReply.Focus();
            }
            else
            {
                if (MemoReplyDone != null)
                {
                    MemoReplyDone(this, new CustomEventArgs(richTextBoxReply.Text));
                }
                Close();
            }
        }

        private void CopyCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(richTextBoxMemo.Text);
            richTextBoxMemo.Copy();
        }

        private void CutCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBoxMemo.Cut();
        }

        private void PasteCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBoxMemo.Paste();
        }

        private void SelectAllCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBoxMemo.SelectAll();
        }

        private void CopyCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBoxReply.Copy();
        }

        private void CutCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBoxReply.Cut();
        }

        private void PasteCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBoxReply.Paste();
        }

        private void SelectAllCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBoxReply.SelectAll();
        }

        private void MemoForm_Activated(object sender, EventArgs e)
        {
            richTextBoxReply.Focus();
        }
    }
}