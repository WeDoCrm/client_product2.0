using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.PopUp;

namespace Client
{
    public partial class SendNoticeForm : TopMostForm
    {

        public event EventHandler<CustomEventArgs> NoticeRegisterRequested;

        public SendNoticeForm()
        {
            InitializeComponent();
        }


        private void BtnSend_Click(object sender, EventArgs e)
        {

            if (TextBoxTitle.Text.Length == 0)
            {
                MessageBox.Show("공지 제목을 적어 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TextBoxTitle.Focus();
                return;
            }
            if (TextBoxContent.Text.Length != 0)
            {
                String NoticeTime = DateTime.Now.ToString();
                NoticeInfo info = new NoticeInfo();
                info.IsEmergency = ToggleButtonNormalNotice.Pressed;
                info.Title = TextBoxTitle.Text;
                info.Content = TextBoxContent.Text;
                NoticeRegisterRequested(this, new CustomEventArgs(info));
                Close();
            }
            else
            {
                MessageBox.Show("공지할 내용을 적어 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TextBoxContent.Focus();
            }
        }

        private void ToggleButtonNormalNotice_PressedChanged(object sender, EventArgs e)
        {
            string tag = "[긴급]";
            if (ToggleButtonNormalNotice.Pressed)
            {
                ToggleButtonNormalNotice.ScreenTip.Text = "긴급공지";
                if (TextBoxTitle.Text.Length >= 4 && TextBoxTitle.Text.Substring(0, 4) == tag)
                {
                    TextBoxTitle.Text = TextBoxTitle.Text;
                }
                else
                {
                    TextBoxTitle.Text = tag + TextBoxTitle.Text;
                }

            }
            else
            {
                ToggleButtonNormalNotice.ScreenTip.Text = "일반공지";
                if (TextBoxTitle.Text.Length >= 4 && TextBoxTitle.Text.Substring(0, 4) == tag)
                {
                    TextBoxTitle.Text = TextBoxTitle.Text.Substring(4);
                }
            }

        }
    }
}