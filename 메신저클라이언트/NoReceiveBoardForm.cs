using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class NoReceiveBoardForm : Form
    {
        Color labelColorInit;
        Color panelColorInit;

        Color labelColorSelected = Color.Black;
        Color panelColorSelected = Color.LightGray;

        public NoReceiveBoardForm()
        {
            InitializeComponent();
            labelColorInit = label_notice.ForeColor;
            panelColorInit = panel_file.BackColor;
        }

        private void NoReceiveBoardForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void panel_notice_MouseEnter(object sender, EventArgs e)
        {
            panel_notice.BackColor = panelColorSelected;
            label_notice.ForeColor = labelColorSelected;
        }

        private void panel_notice_MouseLeave(object sender, EventArgs e)
        {
            panel_notice.BackColor = panelColorInit;
            label_notice.ForeColor = labelColorInit;
        }

        private void panel_memo_MouseEnter(object sender, EventArgs e)
        {
            panel_memo.BackColor = panelColorSelected;
            label_memo.ForeColor = labelColorSelected;
        }

        private void panel_memo_MouseLeave(object sender, EventArgs e)
        {
            panel_memo.BackColor = panelColorInit;
            label_memo.ForeColor = labelColorInit;
        }

        private void panel_file_MouseEnter(object sender, EventArgs e)
        {
            panel_file.BackColor = panelColorSelected;
            label_file.ForeColor = labelColorSelected;
        }

        private void panel_file_MouseLeave(object sender, EventArgs e)
        {
            panel_file.BackColor = panelColorInit;
            label_file.ForeColor = labelColorInit;
        }

        private void panel_trans_MouseEnter(object sender, EventArgs e)
        {
            panel_trans.BackColor = panelColorSelected;
            label_trans.ForeColor = labelColorSelected;
        }

        private void panel_trans_MouseLeave(object sender, EventArgs e)
        {
            panel_trans.BackColor = panelColorInit;
            label_trans.ForeColor = labelColorInit;
        }

        private void panel_notice_MouseClick(object sender, MouseEventArgs e)
        {
            dgv_file.Visible = false;
            dgv_memo.Visible = false;
            dgv_transfer.Visible = false;
            dgv_notice.Visible = true;
        }

        private void panel_memo_MouseClick(object sender, MouseEventArgs e)
        {
            dgv_file.Visible = false;
            dgv_memo.Visible = true;
            dgv_transfer.Visible = false;
            dgv_notice.Visible = false;
        }

        private void panel_file_MouseClick(object sender, MouseEventArgs e)
        {
            dgv_file.Visible = true;
            dgv_memo.Visible = false;
            dgv_transfer.Visible = false;
            dgv_notice.Visible = false;
        }

        private void panel_trans_MouseClick(object sender, MouseEventArgs e)
        {
            dgv_file.Visible = false;
            dgv_memo.Visible = false;
            dgv_transfer.Visible = true;
            dgv_notice.Visible = false;
        }
    }
}
