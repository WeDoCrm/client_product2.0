using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CRMmanager;
using System.Collections;
using System.Drawing;
using Client.Common;

namespace Client
{
    public class ChatUtils
    {
        /// <summary>
        /// 채팅장 트리노드에서 로그인/로그아웃정보를 뺀 사용자id목록을 구함
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string GetIdFromNodeTag(string tag)
        {
            string[] tempArr = tag.Split(':');
            if (tempArr.Length > 1)
            {
                return tempArr[0];
            }
            else
            {
                return tag;
            }
        }

        /// <summary>
        /// 채팅창 트리노드에서 로그인상태인 사용자 id만 구함
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public static string[] GetLoggedInIdFromNodeTag(TreeNodeCollection Nodes)
        {
            List<string> chatterList = new List<string>();
            
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!((string)Nodes[i].Tag).Contains(CommonDef.CHAT_USER_LOG_OUT))
                {
                    chatterList.Add(ChatUtils.GetIdFromNodeTag((string)Nodes[i].Tag));
                }
            }
            return chatterList.ToArray();

        }

        public static string TagAsLoggedInId(string id)
        {
            return (id + CommonDef.CHAT_USER_LOG_IN);
        }

        public static string TagAsLoggedOutId(string id)
        {
            return (id + CommonDef.CHAT_USER_LOG_OUT);
        }

        public static bool ContainsFormKeyInChatForm(string formKey, string idInMsg)
        {
            string[] formKeyArr = formKey.Split('/');
            string[] idInMsgArr = idInMsg.Split('/');

            if (formKeyArr.Length == 0 || formKeyArr.Length != idInMsg.Length)
            {
                return false;
            }

            for (int i = 0; i < formKeyArr.Length; i++)
            {
                if (!idInMsg.Contains(formKeyArr[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// 자기 id를 앞으로 해서 formkey 생성 : myid/id1/id2...
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public static string GetFormKey(string ids, string myId)
        {
            SortedList aList = GetFormKeySortedList(ids);

            string formKey = myId;
            ICollection keys = aList.Keys;
            foreach (string key in keys) {
                if (key != myId)
                    formKey += "/" + key;
            }
            return formKey;
        }

        /// <summary>
        /// 로그인한 id가 채팅창에 이미 있는 경우, {userid}_out -> {userid}로 변경해준다.
        /// 없는 경우 기존처럼 처리
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="myId"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public static string GetFormKeyWithUserAdded(string ids, string myId, string addedId) {
            
            SortedList aList = GetFormKeySortedList(ids);
            
            if (!ids.Contains(addedId)) { //존재하지 않는 경우 추가
                aList.Add(addedId, addedId);
            }
    
            string formKey = myId;
            ICollection keys = aList.Keys;
            foreach (string key in keys) {
                if (key == myId)
                    continue;
                //기존 logout, quit처리된 경우 복원해줌.
                if (key == addedId + "_out"
                    || key == addedId + "_quit") {
                    formKey += "/" + addedId;
                } else {
                    formKey += "/" + key;
                }
            }
            return formKey;
        }

        public static string GetFormKeyWithMultiUsersAdded(string ids, string myId, string addedIds) {
            SortedList aList = GetFormKeySortedList(addedIds);
            ICollection keys = aList.Keys;
            string resultKey = ids;
            foreach (string key in keys) {
                resultKey = GetFormKeyWithUserAdded(resultKey, myId, key);
            }
            return resultKey;
        }
        public static string GetFormKeyWithUserLogOut(string ids, string myId, string logoutId) {
            return GetFormKeyWithUserRemoved(ids, myId, logoutId, "_out");
        }

        public static string GetFormKeyWithUserQuit(string ids, string myId, string quitId) {
            return GetFormKeyWithUserRemoved(ids, myId, quitId, "_quit");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="myId"></param>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public static string GetFormKeyWithUserRemoved(string ids, string myId, string outId, string tag) {
            if (ids.Contains(outId)) { //
                SortedList aList = GetFormKeySortedList(ids);

                string formKey = myId;
                ICollection keys = aList.Keys;
                foreach (string key in keys) {
                    if (key == myId)
                        continue;
                    if (key == outId) {
                        formKey += "/" + outId + tag;
                    } else {
                        formKey += "/" + key;
                    }
                }
                return formKey;
            } else {
                return GetFormKey(ids, myId);
            }
        }

        public static SortedList GetFormKeySortedList(string ids) {

            SortedList aList = new SortedList();
            string[] idsArr = ids.Split('/');
            foreach (string item in idsArr) {
                aList.Add(item, item);
            }
            return aList;
        }

        public static ChatForm GetParentChatForm(Control ctrl) {
           
            Control parent = ctrl.Parent;
        
            while (! (parent is ChatForm))
            {
                parent = parent.Parent;
            }
            return (ChatForm)parent;
        }

        public static AddMemberForm GetParentAddMemberForm(Control ctrl)
        {
            Control parent = ctrl.Parent;

            while (!(parent is AddMemberForm))
            {
                parent = parent.Parent;
            }
            return (AddMemberForm)parent;
        }

        public static Hashtable GetMember(Hashtable treeSource, string teamname)
        {
            Hashtable memTable = new Hashtable();
            if (treeSource.ContainsKey(teamname))
            {
                ArrayList list = (ArrayList)treeSource[teamname];
                foreach (object obj in list)
                {
                    string item = (string)obj;
                    string[] array = item.Split('!');
                    string tempid = array[0];
                    string tempname = array[1];
                    memTable[tempid] = tempname;
                }
            }
            return memTable;
        }

        public static Color GetCustomColor(string customColor)
        {
            Color c = System.Drawing.Color.Black;
            try
            {
                if (customColor != null && customColor.Length > 0)
                {
                    MsgrLogger.WriteLog("customColor = " + customColor);
                    c = Color.FromName(customColor);
                }
            }
            catch (Exception ex)
            {
                MsgrLogger.WriteLog(string.Format("GetCustomColor({0}) Error : {1}",customColor, ex.ToString()));
            }

            return c;
        }

        public static Font GetCustomFont(string customFont)
        {
            System.Drawing.Font f = null;
            try
            {
                if (customFont != null && customFont.Length > 0)
                {
                    MsgrLogger.WriteLog("customFont = " + customFont);
                    IntPtr ptr = new IntPtr(Convert.ToInt32(customFont));
                    f = System.Drawing.Font.FromHfont(ptr);
                }
            }
            catch (Exception ex)
            {
                MsgrLogger.WriteLog(string.Format("GetCustomFont({0}) Error : {1}", customFont, ex.ToString()));
            }
            return f;
        }
    }

    public class CRMUtils
    {
        public static bool DisplayCrmPopUp(FRM_MAIN frm)
        {
            if (frm == null) return false;
            frm.WindowState = FormWindowState.Normal;
            frm.StartPosition = FormStartPosition.Manual;
            frm.SetBounds(0, 0, frm.Width, frm.Height);
            frm.Show();
            frm.Activate();
            frm.TopLevel = true;
            return true;
        }
    }

    public class CustomEventArgs : EventArgs
    {
        private Object obj;

        public CustomEventArgs(Object s)
        {
            obj = s;
        }

        public Object GetItem
        {
            get { return obj; }
        }
    }

    public class DownloadInfo
    {
        string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        int fileSize;
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        string senderFileName;
        public string SenderFileName
        {
            get { return senderFileName; }
            set { senderFileName = value; }
        }

        string downloadPath;
        public string DownloadPath
        {
            get { return downloadPath; }
            set { downloadPath = value; }
        }

        string msgFileKey;
        public string MsgFileKey
        {
            get { return msgFileKey; }
            set { msgFileKey = value; }
        }

        string senderId;
        public string SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }

        string senderName;
        public string SenderName
        {
            get { return senderName; }
            set { senderName = value; }
        }

        string myId;
        public string MyId
        {
            get { return myId; }
            set { myId = value; }
        }

        bool downloadAccepted;
        public bool DownloadAccepted
        {
            get { return downloadAccepted; }
            set { downloadAccepted = value; }
        }
    }

    public class NoticeInfo
    {
            bool isEmergency;
            public bool IsEmergency
            {
                get { return isEmergency; }
                set { isEmergency = value; }
            }
            string title;
            public string Title
            {
                get { return title; }
                set { title = value; }
            }
            string content;
            public string Content
            {
                get { return content; }
                set { content = value; }
            }
    }

    public class MsgrUserStatus
    {
        public const string BUSY = "busy";

        public const string AWAY = "away";

        public const string LOGOUT = "logout";
        public const string ONLINE = "online";
        public const string DND = "DND";//다른용무중
    }
    //public class Logger
    //{
    //    //public void logWrite(string clog)
    //    //{
    //    //    try
    //    //    {
    //    //        clog = string.Format("[{0}]{1}\r\n", DateTime.Now.ToString(), clog);

    //    //        if (Log.logBox.InvokeRequired)
    //    //        {
    //    //            WriteLog writelog = new WriteLog(Log.logBox.AppendText);

    //    //            Invoke(writelog, clog);
    //    //        }
    //    //        else Log.logBox.AppendText(clog);

    //    //        logFileWrite(clog);
    //    //    }
    //    //    catch (Exception e)
    //    //    {

    //    //    }
    //    //}

    //    public void logFileWrite(string _log)
    //    {
    //        //StreamWriter sw = null;
    //        //fileInfo = new FileInfo(@"C:\MiniCTI\log\" + date + ".txt");
    //        //if (!fileInfo.Exists)
    //        //{
    //        //    fileInfo.Create();
    //        //}

    //        //try
    //        //{
    //        //    sw = new StreamWriter(@"C:\MiniCTI\log\" + date + ".txt", true);
    //        //    sw.WriteLine(_log);
    //        //    sw.Close();
    //        //}
    //        //catch (Exception e)
    //        //{
    //        //    Console.WriteLine("logFileWriter() 에러 : " + e.ToString());
    //        //}
    //    }
    //}
}
