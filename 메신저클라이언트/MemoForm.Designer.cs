namespace Client
{
    partial class MemoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoForm));
            this.MemoRe = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CutCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllCtrlCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CutCtrlCToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyCtrlCToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteCtrlCToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllCtrlCToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new Elegant.Ui.SplitContainer();
            this.richTextBoxMemo = new System.Windows.Forms.RichTextBox();
            this.richTextBoxReply = new System.Windows.Forms.RichTextBox();
            this.Memobtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MemoRe
            // 
            this.MemoRe.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MemoRe.Location = new System.Drawing.Point(152, 31);
            this.MemoRe.Multiline = true;
            this.MemoRe.Name = "MemoRe";
            this.MemoRe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MemoRe.Size = new System.Drawing.Size(265, 64);
            this.MemoRe.TabIndex = 0;
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
            this.CutCtrlCToolStripMenuItem1.Click += new System.EventHandler(this.CutCtrlCToolStripMenuItem1_Click);
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
            this.PasteCtrlCToolStripMenuItem1.Click += new System.EventHandler(this.PasteCtrlCToolStripMenuItem1_Click);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.MemoRe);
            this.panel1.Location = new System.Drawing.Point(241, 178);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 39);
            this.panel1.TabIndex = 8;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CutCtrlCToolStripMenuItem2,
            this.CopyCtrlCToolStripMenuItem2,
            this.PasteCtrlCToolStripMenuItem2,
            this.toolStripSeparator2,
            this.SelectAllCtrlCToolStripMenuItem2});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(136, 98);
            // 
            // CutCtrlCToolStripMenuItem2
            // 
            this.CutCtrlCToolStripMenuItem2.Image = global::Client.Properties.Resources.ID_EDIT_CUT_SMALL;
            this.CutCtrlCToolStripMenuItem2.Name = "CutCtrlCToolStripMenuItem2";
            this.CutCtrlCToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.CutCtrlCToolStripMenuItem2.ShowShortcutKeys = false;
            this.CutCtrlCToolStripMenuItem2.Size = new System.Drawing.Size(135, 22);
            this.CutCtrlCToolStripMenuItem2.Text = "잘라내기(T)";
            this.CutCtrlCToolStripMenuItem2.Click += new System.EventHandler(this.CutCtrlCToolStripMenuItem2_Click);
            // 
            // CopyCtrlCToolStripMenuItem2
            // 
            this.CopyCtrlCToolStripMenuItem2.Image = global::Client.Properties.Resources.ID_EDIT_COPY_SMALL;
            this.CopyCtrlCToolStripMenuItem2.Name = "CopyCtrlCToolStripMenuItem2";
            this.CopyCtrlCToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyCtrlCToolStripMenuItem2.ShowShortcutKeys = false;
            this.CopyCtrlCToolStripMenuItem2.Size = new System.Drawing.Size(135, 22);
            this.CopyCtrlCToolStripMenuItem2.Text = "복사(C)";
            this.CopyCtrlCToolStripMenuItem2.Click += new System.EventHandler(this.CopyCtrlCToolStripMenuItem2_Click);
            // 
            // PasteCtrlCToolStripMenuItem2
            // 
            this.PasteCtrlCToolStripMenuItem2.Image = global::Client.Properties.Resources.pastesmall;
            this.PasteCtrlCToolStripMenuItem2.Name = "PasteCtrlCToolStripMenuItem2";
            this.PasteCtrlCToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.PasteCtrlCToolStripMenuItem2.ShowShortcutKeys = false;
            this.PasteCtrlCToolStripMenuItem2.Size = new System.Drawing.Size(135, 22);
            this.PasteCtrlCToolStripMenuItem2.Text = "붙여넣기(P)";
            this.PasteCtrlCToolStripMenuItem2.Click += new System.EventHandler(this.PasteCtrlCToolStripMenuItem2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(132, 6);
            // 
            // SelectAllCtrlCToolStripMenuItem2
            // 
            this.SelectAllCtrlCToolStripMenuItem2.Image = global::Client.Properties.Resources.AutomaticColor;
            this.SelectAllCtrlCToolStripMenuItem2.Name = "SelectAllCtrlCToolStripMenuItem2";
            this.SelectAllCtrlCToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SelectAllCtrlCToolStripMenuItem2.ShowShortcutKeys = false;
            this.SelectAllCtrlCToolStripMenuItem2.Size = new System.Drawing.Size(135, 22);
            this.SelectAllCtrlCToolStripMenuItem2.Text = "모두 선택(A)";
            this.SelectAllCtrlCToolStripMenuItem2.Click += new System.EventHandler(this.SelectAllCtrlCToolStripMenuItem2_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Id = "f7389220-eda9-4af8-aa30-1373d178b64a";
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxMemo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxReply);
            this.splitContainer1.Panel2.Controls.Add(this.Memobtn);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(334, 293);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 9;
            // 
            // richTextBoxMemo
            // 
            this.richTextBoxMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBoxMemo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMemo.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBoxMemo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMemo.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMemo.Name = "richTextBoxMemo";
            this.richTextBoxMemo.ReadOnly = true;
            this.richTextBoxMemo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxMemo.Size = new System.Drawing.Size(334, 239);
            this.richTextBoxMemo.TabIndex = 8;
            this.richTextBoxMemo.Text = "";
            // 
            // richTextBoxReply
            // 
            this.richTextBoxReply.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxReply.ContextMenuStrip = this.contextMenuStrip2;
            this.richTextBoxReply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxReply.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxReply.Name = "richTextBoxReply";
            this.richTextBoxReply.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxReply.Size = new System.Drawing.Size(272, 50);
            this.richTextBoxReply.TabIndex = 11;
            this.richTextBoxReply.Text = "";
            // 
            // Memobtn
            // 
            this.Memobtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.Memobtn.Location = new System.Drawing.Point(272, 0);
            this.Memobtn.Name = "Memobtn";
            this.Memobtn.Size = new System.Drawing.Size(62, 50);
            this.Memobtn.TabIndex = 10;
            this.Memobtn.Text = "보내기";
            this.Memobtn.UseVisualStyleBackColor = true;
            // 
            // MemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 293);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 193);
            this.Name = "MemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "쪽지쓰기";
            this.Activated += new System.EventHandler(this.MemoForm_Activated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox MemoRe;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CopyCtrlCToolStripMenuItem1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem CutCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem PasteCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SelectAllCtrlCToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem CopyCtrlCToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem CutCtrlCToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem PasteCtrlCToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem SelectAllCtrlCToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Elegant.Ui.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBoxMemo;
        private System.Windows.Forms.RichTextBox richTextBoxReply;
        public System.Windows.Forms.Button Memobtn;
    }
}