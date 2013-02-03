using Client.PopUp;
namespace Client
{
    partial class SendNoticeForm : TopMostForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendNoticeForm));
            this.TextBoxContent = new System.Windows.Forms.TextBox();
            this.formkey = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxTitle = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.ToggleButtonNormalNotice = new Elegant.Ui.ToggleButton();
            this.SuspendLayout();
            // 
            // TextBoxContent
            // 
            this.TextBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxContent.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.TextBoxContent.Location = new System.Drawing.Point(12, 34);
            this.TextBoxContent.Multiline = true;
            this.TextBoxContent.Name = "TextBoxContent";
            this.TextBoxContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxContent.Size = new System.Drawing.Size(385, 282);
            this.TextBoxContent.TabIndex = 0;
            // 
            // formkey
            // 
            this.formkey.AutoSize = true;
            this.formkey.Location = new System.Drawing.Point(10, 220);
            this.formkey.Name = "formkey";
            this.formkey.Size = new System.Drawing.Size(0, 12);
            this.formkey.TabIndex = 5;
            this.formkey.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(16, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "제 목 :";
            // 
            // TextBoxTitle
            // 
            this.TextBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitle.Location = new System.Drawing.Point(62, 7);
            this.TextBoxTitle.Name = "TextBoxTitle";
            this.TextBoxTitle.Size = new System.Drawing.Size(292, 21);
            this.TextBoxTitle.TabIndex = 11;
            // 
            // BtnSend
            // 
            this.BtnSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnSend.Location = new System.Drawing.Point(161, 322);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 15;
            this.BtnSend.Text = "공지등록";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // ToggleButtonNormalNotice
            // 
            this.ToggleButtonNormalNotice.Id = "7ce92836-65c8-4317-950f-2e4d07833ea8";
            this.ToggleButtonNormalNotice.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Disabled", global::Client.Properties.Resources.공지하기_이미지만),
            new Elegant.Ui.ControlImage("Focused", global::Client.Properties.Resources.공지하기_이미지만),
            new Elegant.Ui.ControlImage("Hovered", global::Client.Properties.Resources.공지하기_이미지만)});
            this.ToggleButtonNormalNotice.Location = new System.Drawing.Point(369, 7);
            this.ToggleButtonNormalNotice.Name = "ToggleButtonNormalNotice";
            this.ToggleButtonNormalNotice.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ToggleButtonNormalNotice.ScreenTip.Text = "일반공지";
            this.ToggleButtonNormalNotice.Size = new System.Drawing.Size(28, 24);
            this.ToggleButtonNormalNotice.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Client.Properties.Resources.Flag_green_icon16),
            new Elegant.Ui.ControlImage("Pressed", global::Client.Properties.Resources.Flag_red_icon16),
            new Elegant.Ui.ControlImage("PressedFocused", global::Client.Properties.Resources.Flag_red_icon16),
            new Elegant.Ui.ControlImage("PressedHovered", global::Client.Properties.Resources.Flag_red_icon16)});
            this.ToggleButtonNormalNotice.TabIndex = 16;
            this.ToggleButtonNormalNotice.PressedChanged += new System.EventHandler(this.ToggleButtonNormalNotice_PressedChanged);
            // 
            // SendNoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 350);
            this.Controls.Add(this.ToggleButtonNormalNotice);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.TextBoxTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.formkey);
            this.Controls.Add(this.TextBoxContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(425, 373);
            this.Name = "SendNoticeForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "공지하기";
            this.TopMost = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox TextBoxContent;
        public System.Windows.Forms.Label formkey;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox TextBoxTitle;
        public System.Windows.Forms.Button BtnSend;
        private Elegant.Ui.ToggleButton ToggleButtonNormalNotice;
    }
}