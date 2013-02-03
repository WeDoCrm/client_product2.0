using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
using Client.PopUp;
using Client.Common;

namespace Client {

    public partial class ChatForm : FlashWindowForm {
    
        private Font defaultMessageFont
            = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        private Color defaultMessageColor = Color.Black;

        private Font userNamePartFont = new Font("굴림", 9.0f, FontStyle.Regular);
        private Color userNamePartColor = Color.DarkSlateGray;
        private Font messagePartFont;
        private Color messagePartColor;

        public event EventHandler<EventArgs> ChatMessageAdded;

        public ChatForm() {
            try {
                InitializeComponent();
                this.Shown += new EventHandler(ChatForm_Shown);
                this.Activated += new EventHandler(Form_Activated);
                this.chatBox.TextChanged += new EventHandler(chatBox_TextChanged);
                txtbox_exam.Font = defaultMessageFont;
                txtbox_exam.ForeColor = defaultMessageColor;
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
        }

        public string GetFormKey() {
            return this.Formkey.Text;
        }

        public void SetFormKey(string key) {
            this.Formkey.Text = key;
        }

        public void SetCustomFont(string customColor, string customFont) {

            Color color = ChatUtils.GetCustomColor(customColor);
            Font  font = ChatUtils.GetCustomFont(customFont);
            
            if (font == null)
                messagePartFont = defaultMessageFont;
            else
                messagePartFont = font;
            
            if (color == null)
                messagePartColor = defaultMessageColor;
            else
                messagePartColor = color;
            
            txtbox_exam.Font = messagePartFont;
            txtbox_exam.ForeColor = messagePartColor;
        }

        private void ChatForm_Shown(object sender, EventArgs e) {
            this.ReBox.Focus();
        }

        private void chatBox_TextChanged(object sender, EventArgs e) {
            try {
                if (this.Focused == false && this.ReBox.Focused == false && this.btnSend.Focused == false && chatBox.Focused == false)
                    DoFlashWindow();
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
        }

        private void ChatForm_Load(object sender, EventArgs e) {
            try {
                StopFlash();
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
        }


        private void label_font_Click(object sender, EventArgs e) {
            try {
                FontDialog dialog = new FontDialog();
                
                dialog.Font = txtbox_exam.Font;
                dialog.Color = txtbox_exam.ForeColor;
                dialog.ShowColor = true;

                DialogResult result = dialog.ShowDialog(this);
                
                if (result == DialogResult.OK) {
                    messagePartFont = dialog.Font;
                    messagePartColor = dialog.Color;
                    txtbox_exam.Font = dialog.Font;
                    txtbox_exam.ForeColor = dialog.Color;
                    txtbox_exam.Refresh();
                }
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
        }

        private void label_font_MouseEnter(object sender, EventArgs e) {
            label_font.ForeColor = Color.DarkViolet;
        }

        private void label_font_MouseLeave(object sender, EventArgs e) {
            label_font.ForeColor = Color.Black;
        }

        private void Form_Activated(object sender, EventArgs e) {
            try {
                StopFlash();
                this.ReBox.Focus();
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
        }

        private void ReBox_KeyDown(object sender, KeyEventArgs e) {
            StopFlash();
            if (e.Modifiers == Keys.ControlKey) {
                switch (e.KeyData) {
                    case Keys.C:
                        ReBox.Copy();
                        break;
                    case Keys.P:
                        ReBox.Paste();
                        break;
                    case Keys.X:
                        ReBox.Cut();
                        break;
                    case Keys.A:
                        ReBox.SelectAll();
                        break;
                }
            }
        }

        private void ReBox_MouseClick(object sender, MouseEventArgs e) {
            StopFlash();
        }

        private void ReBox_MouseEnter(object sender, EventArgs e) {
            StopFlash();
        }

        public string[] GetChattersID() {
            string[] chatters = null;
            try {
                if (ChattersTree.Nodes.Count != 0) {
                    chatters = ChatUtils.GetLoggedInIdFromNodeTag(ChattersTree.Nodes);
                }
            } catch (Exception exception) {
                MsgrLogger.WriteLog(exception.ToString());
            }
            return chatters;
        }

        private void AddMessage(string msg) {
            chatBox.AppendText(msg + "\r\n");
            chatBox.ScrollToCaret();
        }

        public void SetChatterOnFormOpening(string id, string name) {
            AddChatter(id, name);
        }

        public void AddNextChatter(string id, string name) {
            AddChatter(id, name);
            PostUserJoinMessage(name);
        }

        private void AddChatter(string id, string name) {
            TreeNode node = ChattersTree.Nodes.Add(id, name + "(" + id + ")");
            node.Tag = id + CommonDef.CHAT_USER_LOG_IN;
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            if (ChattersTree.Nodes.Count == 1)
                this.Text += name;
            else
                this.Text += "/" + name;
        }

        /// <summary>
        /// 다자간 채팅중 대화창을 닫은 구성원을 알림
        /// </summary>
        /// <param name="id"></param>
        public void DeleteChatter(string id) {
            TreeNode[] node = ChattersTree.Nodes.Find(id, false);
            string message = string.Format("{0}님이 창을 닫고 대화를 종료하였습니다.\r\n", node[0].Text);
            AddMessage(message);
            //form.chatBox.AppendText("\r\n###" + node[0].Text + "님이 창을 닫고 대화를 종료하였습니다.." + "\r\n\r\n");
            node[0].Remove();
        }

        /// <summary>
        /// 트리노드에 대화자 노드 추가
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="name">user name</param>
        public void AddChatterToNode(string id, string name) {

            TreeNode[] nodearray = ChattersTree.Nodes.Find(id, false);

            if (nodearray != null && nodearray.Length != 0) {
                foreach (TreeNode anode in nodearray) {
                    if (!id.Equals(ChatUtils.GetIdFromNodeTag(anode.Tag.ToString()))) {
                        AddNextChatter(id, name);
                    }
                }
            } else {
                AddNextChatter(id, name);
            }
        }

        public void SetChatterStatus(string userId, string userName, string status) {
            TreeNode[] nodearray = ChattersTree.Nodes.Find(userId, false);

            if (nodearray != null && nodearray.Length != 0) {
                if (nodearray.Length > 1) {
                    MsgrLogger.WriteLog(string.Format("채팅창에 참여한 {0}이 하나이상 존재.", userId));
                }
                TreeNode anode = nodearray[0];
                if (userId.Equals(ChatUtils.GetIdFromNodeTag(anode.Tag.ToString()))) {

                    switch (status) {
                        case MsgrUserStatus.BUSY://"busy":
                            anode.ImageIndex = 0;
                            anode.SelectedImageIndex = 0;
                            anode.Text = string.Format("{0}({1}){2}", userName, userId, "(통화중)");
                            anode.ForeColor = Color.Black;
                            anode.Tag = userId + CommonDef.CHAT_USER_LOG_IN;
                            break;

                        case MsgrUserStatus.AWAY://"away":
                            anode.ImageIndex = 1;
                            anode.SelectedImageIndex = 1;
                            anode.Text = string.Format("{0}({1}){2}", userName, userId, "(자리비움)");
                            anode.ForeColor = Color.Black;
                            anode.Tag = userId + CommonDef.CHAT_USER_LOG_IN;
                            break;

                        case MsgrUserStatus.LOGOUT://"logout":
                            anode.ImageIndex = 2;
                            anode.SelectedImageIndex = 2;
                            anode.Text = string.Format("{0}({1})", userName, userId);
                            anode.ForeColor = Color.Gray;
                            anode.Tag = userId + CommonDef.CHAT_USER_LOG_OUT;
                            break;

                        case MsgrUserStatus.ONLINE://"online":
                            anode.ImageIndex = 3;
                            anode.SelectedImageIndex = 3;
                            anode.Text = string.Format("{0}({1})", userName, userId);
                            anode.ForeColor = Color.Black;
                            anode.Tag = userId + CommonDef.CHAT_USER_LOG_IN;
                            break;

                        case MsgrUserStatus.DND://"DND":
                            anode.ImageIndex = 4;
                            anode.SelectedImageIndex = 4;
                            anode.Text = string.Format("{0}({1}){2}", userName, userId, "(다른용무중)");
                            anode.ForeColor = Color.Black;
                            anode.Tag = userId + CommonDef.CHAT_USER_LOG_IN;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 창을 닫지 않음. 노드는 로그아웃으로 표시
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void SetChatterLogOut(string userId, string userName) {

            //메시지 표시
            TreeNode[] node = ChattersTree.Nodes.Find(userId, false);
            if (node.Length != 0) {
                AddMessage(string.Format("{0}님이 로그아웃 하셨습니다.\r\n", userName));
            }
            //노드에 로그아웃으로 표시함
            SetChatterStatus(userId, userName, MsgrUserStatus.LOGOUT);
            MsgrLogger.WriteLog(string.Format("{0}({1})를 로그아웃 처리.", userName, userId));
        }

        public void PostCanNotJoinMessage(string userName) {
            AddMessage(userName + " 님은 대화가 불가능한 상태이므로 참가하지 못했습니다.\r\n");
        }

        public void PostUserJoinMessage(string userName) {
            AddMessage(userName + "님을 대화에 초대하였습니다.\r\n");
        }

        public void PostMyMessage(string myName, string message) {
            PostChatMessage(myName, message, true);
        }

        public void PostUserMessage(string userName, string message)
        {
            PostChatMessage(userName, message, false);
        }
        public void PostChatMessage(string userName, string message, bool isMine)
        {
            string msgUserName = userName + " 님의 말 :";
            int startPos = chatBox.Text.Length;
            AddMessage(msgUserName);
            chatBox.Select(startPos, msgUserName.Length);
            chatBox.SelectionFont = this.userNamePartFont;
            chatBox.SelectionColor = this.userNamePartColor;
            chatBox.ScrollToCaret();

            string msgMain = "ㆍ  " + message;
            startPos = chatBox.Text.Length;
            AddMessage(msgMain);
            if (isMine)
            {
                chatBox.Select(startPos, msgMain.Length);
                chatBox.SelectionFont = this.messagePartFont;
                chatBox.SelectionColor = this.messagePartColor;
            }
            chatBox.ScrollToCaret();

            if (!isMine)
            {
                chatStatBar.Text = "마지막 메시지를 받은 시간:" + DateTime.Now.ToString();
            }
        }

        private void ReBox_TextChanged(object sender, EventArgs e)
        {
            if (ReBox.Lines.Length >= 5)
            {
                ReBox.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                ReBox.ScrollBars = ScrollBars.None;
            }
        }

        private void CopyCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            chatBox.Copy();
        }

        private void SelectAllCtrlCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            chatBox.SelectAll();
        }

        private void CutCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ReBox.Cut();
        }

        private void CopyCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ReBox.Copy();
        }

        private void PasteCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ReBox.Paste();
        }

        private void SelectAllCtrlCToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ReBox.SelectAll();
        }

        private void chatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.ControlKey)
            {
                switch (e.KeyData)
                {
                    case Keys.C:
                        chatBox.Copy();
                        break;
                    case Keys.A:
                        chatBox.SelectAll();
                        break;
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                ChatMessageAdded(sender, e);
            }
            catch (Exception exception)
            {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }

        private void ChatForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ChatMessageAdded(sender, e);
                }
            }
            catch (Exception exception)
            {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }

        private void ReBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.Enter) //Ctrl + enter
                {
                    return;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    ChatMessageAdded(sender, e);
                }
            }
            catch (Exception exception)
            {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }
    }
}