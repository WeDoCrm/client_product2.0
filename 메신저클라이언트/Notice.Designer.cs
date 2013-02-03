namespace Client
{
    partial class Notice
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notice));
            this.label_title = new System.Windows.Forms.Label();
            this.label_sender = new System.Windows.Forms.Label();
            this.label_noticetitle = new System.Windows.Forms.Label();
            this.label_notice_sender = new System.Windows.Forms.Label();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.richTextBoxContent = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CutCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_title.Location = new System.Drawing.Point(6, 8);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(51, 12);
            this.label_title.TabIndex = 14;
            this.label_title.Text = "제  목 :";
            // 
            // label_sender
            // 
            this.label_sender.AutoSize = true;
            this.label_sender.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_sender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_sender.Location = new System.Drawing.Point(4, 28);
            this.label_sender.Name = "label_sender";
            this.label_sender.Size = new System.Drawing.Size(54, 12);
            this.label_sender.TabIndex = 15;
            this.label_sender.Text = "게시자 :";
            // 
            // label_noticetitle
            // 
            this.label_noticetitle.AutoSize = true;
            this.label_noticetitle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_noticetitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_noticetitle.Location = new System.Drawing.Point(57, 8);
            this.label_noticetitle.Name = "label_noticetitle";
            this.label_noticetitle.Size = new System.Drawing.Size(53, 12);
            this.label_noticetitle.TabIndex = 16;
            this.label_noticetitle.Text = "공지사항";
            // 
            // label_notice_sender
            // 
            this.label_notice_sender.AutoSize = true;
            this.label_notice_sender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_notice_sender.Location = new System.Drawing.Point(58, 28);
            this.label_notice_sender.Name = "label_notice_sender";
            this.label_notice_sender.Size = new System.Drawing.Size(41, 12);
            this.label_notice_sender.TabIndex = 17;
            this.label_notice_sender.Text = "관리자";
            // 
            // btn_confirm
            // 
            this.btn_confirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_confirm.Location = new System.Drawing.Point(172, 248);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_confirm.TabIndex = 18;
            this.btn_confirm.Text = "확인";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // richTextBoxContent
            // 
            this.richTextBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxContent.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBoxContent.Location = new System.Drawing.Point(-1, 43);
            this.richTextBoxContent.Name = "richTextBoxContent";
            this.richTextBoxContent.ReadOnly = true;
            this.richTextBoxContent.Size = new System.Drawing.Size(418, 199);
            this.richTextBoxContent.TabIndex = 19;
            this.richTextBoxContent.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CutCtrlCToolStripMenuItem1,
            this.CopyCtrlCToolStripMenuItem1,
            this.PasteCtrlCToolStripMenuItem1,
            this.toolStripSeparator1,
            this.SelectAllCtrlCToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(136, 98);
            // 
            // CutCtrlCToolStripMenuItem1
            // 
            this.CutCtrlCToolStripMenuItem1.Enabled = false;
            this.CutCtrlCToolStripMenuItem1.Image = global::Client.Properties.Resources.ID_EDIT_CUT_SMALL;
            this.CutCtrlCToolStripMenuItem1.Name = "CutCtrlCToolStripMenuItem1";
            this.CutCtrlCToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.CutCtrlCToolStripMenuItem1.ShowShortcutKeys = false;
            this.CutCtrlCToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.CutCtrlCToolStripMenuItem1.Text = "잘라내기(T)";
            // 
            // CopyCtrlCToolStripMenuItem1
            // 
            this.CopyCtrlCToolStripMenuItem1.Image = global::Client.Properties.Resources.ID_EDIT_COPY_SMALL;
            this.CopyCtrlCToolStripMenuItem1.Name = "CopyCtrlCToolStripMenuItem1";
            this.CopyCtrlCToolStripMenuItem1.ShortcutKeyDisplayString = "";
            this.CopyCtrlCToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyCtrlCToolStripMenuItem1.ShowShortcutKeys = false;
            this.CopyCtrlCToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.CopyCtrlCToolStripMenuItem1.Text = "복사(C)";
            this.CopyCtrlCToolStripMenuItem1.Click += new System.EventHandler(this.CopyCtrlCToolStripMenuItem1_Click);
            // 
            // PasteCtrlCToolStripMenuItem1
            // 
            this.PasteCtrlCToolStripMenuItem1.Enabled = false;
            this.PasteCtrlCToolStripMenuItem1.Image = global::Client.Properties.Resources.pastesmall;
            this.PasteCtrlCToolStripMenuItem1.Name = "PasteCtrlCToolStripMenuItem1";
            this.PasteCtrlCToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.PasteCtrlCToolStripMenuItem1.ShowShortcutKeys = false;
            this.PasteCtrlCToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.PasteCtrlCToolStripMenuItem1.Text = "붙여넣기(P)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(132, 6);
            // 
            // SelectAllCtrlCToolStripMenuItem1
            // 
            this.SelectAllCtrlCToolStripMenuItem1.Image = global::Client.Properties.Resources.AutomaticColor;
            this.SelectAllCtrlCToolStripMenuItem1.Name = "SelectAllCtrlCToolStripMenuItem1";
            this.SelectAllCtrlCToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SelectAllCtrlCToolStripMenuItem1.ShowShortcutKeys = false;
            this.SelectAllCtrlCToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.SelectAllCtrlCToolStripMenuItem1.Text = "모두 선택(A)";
            this.SelectAllCtrlCToolStripMenuItem1.Click += new System.EventHandler(this.SelectAllCtrlCToolStripMenuItem1_Click);
            // 
            // Notice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 275);
            this.Controls.Add(this.richTextBoxContent);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.label_notice_sender);
            this.Controls.Add(this.label_noticetitle);
            this.Controls.Add(this.label_sender);
            this.Controls.Add(this.label_title);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(433, 313);
            this.Name = "Notice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "공지사항";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label_title;
        public System.Windows.Forms.Label label_sender;
        public System.Windows.Forms.Label label_noticetitle;
        public System.Windows.Forms.Label label_notice_sender;
        public System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.RichTextBox richTextBoxContent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CutCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CopyCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem PasteCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem SelectAllCtrlCToolStripMenuItem1;
    }
}