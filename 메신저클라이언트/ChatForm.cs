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

        private Hashtable htUserColorTable = new Hashtable();
        private MsgColor msgColor = new MsgColor();
        
        private string finalPoster;

        private ToolTip toolTip = new ToolTip();
        private ToolTip tipAddChatter = new ToolTip();

        private string myId;

        public event EventHandler<EventArgs> ChatMessageAdded;

        private bool isActivated = false;
#region 폼처리메소드

        public ChatForm(string myId) {
            try {
                InitializeComponent();
                this.Activated += new EventHandler(Form_Activated);
                this.chatBox.TextChanged += new EventHandler(chatBox_TextChanged);
                txtbox_exam.Font = defaultMessageFont;
                txtbox_exam.ForeColor = defaultMessageColor;
                StatusLabelChatStatus.Text = "";
                this.myId = myId;
                tipAddChatter.SetToolTip(BtnAddChatter, "대화 상대방 추가");

            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
                throw;
            }
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

        private void chatBox_TextChanged(object sender, EventArgs e) {
            try {
                //if (this.Focused == false && this.ReBox.Focused == false && this.btnSend.Focused == false && chatBox.Focused == false)
                if (!isActivated) DoFlashWindow();
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
                this.ReBox.Focus();
            } catch (Exception ex) {
                MsgrLogger.WriteLog(ex.ToString());
            }
        }

        public void SetForward() {
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.Show();
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

        private void ReBox_TextChanged(object sender, EventArgs e) {
            if (ReBox.Lines.Length >= 4)
                ReBox.ScrollBars = ScrollBars.Vertical;
            else
                ReBox.ScrollBars = ScrollBars.None;
        }

        private void CopyCtrlCToolStripMenuItem1_Click(object sender, EventArgs e) {
            chatBox.Copy();
        }

        private void SelectAllCtrlCToolStripMenuItem1_Click(object sender, EventArgs e) {
            chatBox.SelectAll();
        }

        private void CutCtrlCToolStripMenuItem2_Click(object sender, EventArgs e) {
            ReBox.Cut();
        }

        private void CopyCtrlCToolStripMenuItem2_Click(object sender, EventArgs e) {
            ReBox.Copy();
        }

        private void PasteCtrlCToolStripMenuItem2_Click(object sender, EventArgs e) {
            ReBox.Paste();
        }

        private void SelectAllCtrlCToolStripMenuItem2_Click(object sender, EventArgs e) {
            ReBox.SelectAll();
        }

        private void chatBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Modifiers == Keys.ControlKey) {
                switch (e.KeyData) {
                    case Keys.C:
                        chatBox.Copy();
                        break;
                    case Keys.A:
                        chatBox.SelectAll();
                        break;
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            try {
                ChatMessageAdded(sender, e);
            } catch (Exception exception) {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }

        private void ChatForm_KeyDown(object sender, KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.Enter) {
                    ChatMessageAdded(sender, e);
                }
            } catch (Exception exception) {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }

        private void ReBox_KeyUp(object sender, KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.Enter) {
                    ChatMessageAdded(sender, e);
                }
            } catch (Exception exception) {
                MsgrLogger.WriteLog(exception.ToString());
            }
        }

#endregion


#region 채팅정보처리메소드

        private void AddMessage(string msg) {
            chatBox.AppendText(msg + "\r\n");
            chatBox.ScrollToCaret();
            if (isActivated) ReBox.Focus();
        }

        public void SetChatterOnFormOpening(UserObject userObj) {
            if (userObj == null || userObj.Id == null
                || userObj.Id.Trim() == "" || userObj.Id.Trim() == myId)
                return;
            AddChatter(userObj);
        }

        public bool HasSingleChatter() {
            return (ChattersTree.Nodes.Count == 1);
        }

#endregion


#region 채팅메시지처리메소드
        
        public string GetFormKey() {
            return this.Formkey.Text;
        }

        public void SetFormKey(string key) {
            this.Formkey.Text = key;
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

        public bool ContainsId(string id) {
            TreeNode[] nodeArray = ChattersTree.Nodes.Find(id, false);
            return (nodeArray != null && nodeArray.Length > 0);
        }

        public void AddNextChatter(UserObject userObj) {
            if (userObj == null || userObj.Id == null
                || userObj.Id.Trim() == "" || userObj.Id.Trim() == myId)
                return;
            AddChatter(userObj);
            PostUserJoinMessage(userObj.Name);
        }

        /// <summary>
        /// 1. 노드 추가
        /// 2. 창타이틀에 이름 반영
        /// 3. 색깔반영
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private void AddChatter(UserObject userObj) {

            TreeNode node = ChattersTree.Nodes.Add(userObj.Id, userObj.TitleName + "(" + userObj.Id + ")");
            node.Tag = userObj;
            //node.ImageIndex = 0;
            //node.SelectedImageIndex = 0;
            node.BackColor = Color.FromArgb(205, 220, 237);
            if (ChattersTree.Nodes.Count == 1)
                this.Text += userObj.Name;
            else
                this.Text += "/" + userObj.Name;
            //채팅참여했다가 나간후 다시 참여한 경우, 원래 색을 그대로 쓴다.
            if (!htUserColorTable.Contains(userObj.Id)) {
                htUserColorTable.Add(userObj.Id, msgColor.GetColor(ChattersTree.Nodes.Count));
            }
            //상태지정
            SetChatterStatus(userObj.Id, userObj.Name, userObj.Status);
        }

        /// <summary>
        /// 다자간 채팅중 대화창을 닫은 구성원을 알림
        /// </summary>
        /// <param name="id"></param>
        public void DeleteChatter(string id, string name) {
            TreeNode[] node = ChattersTree.Nodes.Find(id, false);
            string message = string.Format("{0}님이 창을 닫고 대화를 종료하였습니다.\r\n", node[0].Text);
            AddMessage(message);
            node[0].Remove();

            this.Text = ChatUtils.RemoveFromTitle(this.Text, name);
        }

        /// <summary>
        /// 트리노드에 대화자 노드 추가
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="name">user name</param>
        public void AddChatterToNode(UserObject userObj) {
            if (userObj == null || userObj.Id == null
                || userObj.Id.Trim() == "" || userObj.Id.Trim() == myId)
                return;

            TreeNode[] nodearray = ChattersTree.Nodes.Find(userObj.Id, false);

            if (nodearray != null && nodearray.Length != 0) {
                //있는경우는 오류
                MsgrLogger.WriteLog(string.Format("AddChatterToNode:{0}({1})이 이미 존재함.", userObj.Name, userObj.Id));
            } else {
                AddNextChatter(userObj);
            }
        }

        public void SetChatterStatus(string userId, string userName, string status) {
            TreeNode[] nodearray = ChattersTree.Nodes.Find(userId, false);

            if (nodearray != null && nodearray.Length != 0) {
                if (nodearray.Length > 1) 
                    MsgrLogger.WriteLog(string.Format("채팅창에 참여한 {0}이 하나이상 존재.", userId));
                
                TreeNode anode = nodearray[0];
                try {
                    UserObject userObj = (UserObject)anode.Tag;
                    if (userId.Equals(userObj.Id)) {

                        switch (status) {
                            case MsgrUserStatus.ONLINE://"online":
                                anode.ImageIndex = 0;
                                anode.SelectedImageIndex = 0;
                                anode.ForeColor = Color.Black;
                                break;
                            case MsgrUserStatus.LOGOUT://"logout":
                                anode.ImageIndex = 1;
                                anode.SelectedImageIndex = 1;
                                anode.ForeColor = Color.Gray;
                                break;
                            case MsgrUserStatus.BUSY://"busy":
                                anode.ImageIndex = 2;
                                anode.SelectedImageIndex = 2;
                                anode.ForeColor = Color.Black;
                                break;
                            case MsgrUserStatus.DND://"DND":
                                anode.ImageIndex = 3;
                                anode.SelectedImageIndex = 3;
                                anode.ForeColor = Color.Black;
                                break;
                            case MsgrUserStatus.AWAY://"away":
                                anode.ImageIndex = 4;
                                anode.SelectedImageIndex = 4;
                                anode.ForeColor = Color.Black;
                                break;
                        }
                        userObj.Status = status;
                        anode.Text = string.Format("{0}({1})", userObj.TitleName, userId);
                        anode.Tag = userObj; //userId + CommonDef.CHAT_USER_LOG_IN;
                    }
                } catch (Exception ex) {
                    MsgrLogger.WriteLog(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 경우: 1:1, 다자대화
        /// 1. 로그아웃메시지 표시
        /// 2. 노드 로그아웃표시
        /// 3. 1:1인경우 창 비활성화시킴
        /// 
        /// 다자창의 경우는 LogOut이전에 Quit이 먼저발생하나, 예외상황을 감안함.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void SetChatterLogOut(string userId, string userName) {

            TreeNode[] node = ChattersTree.Nodes.Find(userId, false);

            if (node != null && node.Length != 0 ) {
                //메시지 표시
                AddMessage(string.Format("{0}님이 로그아웃하셨습니다.\r\n", userName));
                //노드에 로그아웃으로 표시함
                SetChatterStatus(userId, userName, MsgrUserStatus.LOGOUT);
                MsgrLogger.WriteLog(string.Format("{0}({1})를 로그아웃 처리.", userName, userId));
                //1:1 인 경우
                if (this.HasSingleChatter()) {
                    ReBox.Enabled = false;
                    toolTip.SetToolTip(ReBox, string.Format("{0}님이 로그아웃하셨습니다.\r\n", userName));
                }
            }
        }

        /// <summary>
        /// 1:1창에서 out->in인 경우만 해당
        /// 다자창에서 busy/dnd/away->in인 경우만 해당
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public void SetChatterLogIn(string userId, string userName) {

            TreeNode[] node = ChattersTree.Nodes.Find(userId, false);

            if ( node != null && node.Length != 0 ) {
                //메시지 표시
                if (this.HasSingleChatter()) {//다자창에서는 필요없음
                    AddMessage(string.Format("{0}님이 로그인하셨습니다.\r\n", userName));
                    ReBox.Enabled = true;
                    toolTip.RemoveAll();
                }
                //노드에 로그인으로 표시함
                SetChatterStatus(userId, userName, MsgrUserStatus.ONLINE);
                MsgrLogger.WriteLog(string.Format("{0}({1})를 로그인 처리.", userName, userId));
            }
        }

        public void PostCanNotJoinMessage(string userName) {
            AddMessage(userName + " 님은 대화가 불가능한 상태이므로 참가하지 못했습니다.\r\n");
        }

        public void PostUserJoinMessage(string userName) {
            AddMessage(userName + "님을 대화에 초대하였습니다.\r\n");
        }

        public void PostMyMessage(string myName, string message) {
            PostChatMessage(this.myId, myName, message, true);
        }

        public void PostUserMessage(string userId, string userName, string message)
        {
            PostChatMessage(userId, userName, message, false);
        }
        public void PostChatMessage(string userId, string userName, string message, bool isMine)
        {
            int startPos = 0;
            //이전 메시지올린사람과 동일인 경우, 생략.
            if (finalPoster != userName) {
                string msgUserName = userName + " 님의 말 :";
                startPos = chatBox.Text.Length;
                AddMessage(msgUserName);
                chatBox.Select(startPos, msgUserName.Length);
                chatBox.SelectionFont = this.userNamePartFont;
                chatBox.SelectionColor = this.userNamePartColor;
                chatBox.ScrollToCaret();
            }

            string msgMain = "ㆍ  " + message;
            startPos = chatBox.Text.Length;
            AddMessage(msgMain);
            if (isMine) {
                chatBox.Select(startPos, msgMain.Length);
                chatBox.SelectionFont = this.messagePartFont;
                chatBox.SelectionColor = this.messagePartColor;
            } else {
                chatBox.Select(startPos, msgMain.Length);
                chatBox.SelectionColor = (Color)htUserColorTable[userId];
            }
            chatBox.ScrollToCaret();

            finalPoster = userName;

            if (!isMine) {
                StatusLabelChatStatus.Text = "마지막 메시지를 받은 시간:" + DateTime.Now.ToString();
            }
            if (isActivated) ReBox.Focus();
        }
#endregion

        private void ChatForm_Activated(object sender, EventArgs e)
        {
            ReBox.Focus();
            isActivated = true;
            Console.WriteLine("ChatForm_Activated");
        }

        private void ChatForm_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
        }

        public bool IsActivated() {
            return isActivated;
        }

        private void ChatForm_Shown(object sender, EventArgs e) {
            this.TopMost = true;
        }

        private void ChatForm_Enter(object sender, EventArgs e) {
            Console.WriteLine("ChatForm_Enter");
        }
    }
}