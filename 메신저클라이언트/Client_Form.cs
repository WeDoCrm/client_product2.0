using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CRMmanager;
using Microsoft.Win32;
using Elegant.Ui.Samples.ControlsSample.Sockets;
using Client.Common;
using System.Collections.Generic;


namespace Client
{
	/// <summary>
	/// Form1에 대한 요약 설명입니다.
	/// </summary>
    public partial class Client_Form : System.Windows.Forms.Form
    {
       
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //초기 설정 관련
        private string AppName = "";
        private string AppConfigName = "";
        private string AppRegName = "";
        private string AppConfigFullPath = "";
        private string XmlConfigFullPath = "";
        private string XmlConfigOrgFullPath = "";

        private string MsgrTitle = "";

        private string[] news = new string[10];
        private DirectoryInfo di = new DirectoryInfo(@"C:\MiniCTI");
        private DirectoryInfo memodi = null;
        private DirectoryInfo dialogdi = null;
        private DirectoryInfo FilesDir = null;
        private DirectoryInfo privatefolder = null;
        private DirectoryInfo MonthFolder = null;
        private DirectoryInfo DayFolder = null;
        public string date = DateTime.Now.ToShortDateString();
        private FileInfo fileInfo = null;
        private string myname = null;
        private string myid = null;
        private string mypass = null;
        private string serverIP = null;
        private string socket_port_crm = null;
        private string MadeNoticeDetail = null;
        private string extension = null;
        private string com_cd = null;
        private string version = null;
        private string custom_font = null;
        private string custom_color = null;
        private string top = null;

        private bool isMadeNoticeResult = false;
        private bool nopop = false;
        private bool nopop_outbound = false; //발신시 팝업중지
        private bool isHide = false;
        private bool firstCall = false;
        public XmlDocument xmldoc = new XmlDocument();
        private Point mousePoint = Point.Empty;
        private Color labelColor;
        
        private IPAddress local = null;
        private static PopForm popform = null;
        private MissedCallForm missedcallform = null;
        private MissedCallList missedlistform = null;
        private SetAutoStartForm configform = null;
        private SetServer_Form setserverform = null;
        private NotifyForm notifyform = null;
        private AboutForm aboutform = null;
        private ToolTip tooltip = new ToolTip();
        private System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer timerForNotify = new System.Windows.Forms.Timer();

        private NoticeResultForm noticeresultform = null;
        private NoticeListForm noticelistform = null;
        private NoReceiveBoardForm noreceiveboardform = null;
        private MemoListForm mMemoListForm = null;
        private DownloadResultForm downloadform = null;
        private DialogListForm mDialogListForm = null;
        private SetExtensionForm extform = null;
        private SelectTransferForm selecttransferform = null;
        private RequestUpdate request = null;
        private int mainform_width = 0;
        private int mainform_height = 0;
        private int missedCallCount = 0;
        private int screenWidth = 0;
        private int screenHeight = 0;
        private int mainform_x = 0;
        private int mainform_y = 0;


        private int listenport = PortInfoMsgr.LISTEN_PORT;
        private int sendport = PortInfoMsgr.SEND_PORT;
        private int checkport = PortInfoMsgr.CHEKC_PORT;
        private int filereceiveport = PortInfoMsgr.FILE_RCV_PORT;
        private int filesendport = PortInfoMsgr.FILE_SEND_PORT;
        private Socket ServerSocket = null;
        private UdpClient listenSock = null;
        private UdpClient sendSock = null;
        private UdpClient checkSock = null;
        private UdpClient filesock = null;
        private UdpClient filesendSock = null;
        private IPEndPoint server = null;
        private IPEndPoint filesender = null;
        private IPEndPoint filesend = null;

        //FTP
        ShowFileSendDetailDelegate showFileSendDetailDelegate;
        ShowFileSendStatDelegate showFileSendStatDelegate;
        ShowFileSendStatExDelegate showFileSendStatExDelegate;
        ShowCloseButtonDelegate showCloseButtonDelegate;
        private delegate void ShowFileSendStatDelegate(string stat, int idx, SendFileForm form);
        private delegate void ShowFileSendStatExDelegate(int status, string desc, int idx, SendFileForm form);
        private delegate void ShowFileSendDetailDelegate(string key, string detail, FileSendDetailListView view);
        private delegate bool ShowCloseButtonDelegate(FileSendDetailListView view);
        private delegate string SaveFileDialogDelegate(string filename);


        //FTP over tcp 
        private FtpSocketListener mFtpSocketListener;
        private string mDownloadPath;
        private delegate void ShowFileRcvStatDelegate(int status, int index, DownloadForm form);
        private delegate void FTP_ShowDialogDelegate(DownloadInfo info);

        private Thread receive = null;
        private Thread checkThread = null;
        private Hashtable ChatFormList = new Hashtable();  //채팅창  key=id, value=chatform
        private Hashtable MemoFormList = new Hashtable(); //key=time, value=SendMemoForm
        private Hashtable TeamInfoList = new Hashtable(); //key=id, value=team
        private Hashtable InList = new Hashtable();       //key=id, value=IPEndPoint
        private Hashtable MemberInfoList = new Hashtable();
        private Hashtable FileSendDetailList = new Hashtable();
        private Hashtable FileSendFormList = new Hashtable();
        private Hashtable FileSendThreadList = new Hashtable();
        private Hashtable FtpClientThreadList = new Hashtable();
        private Object FtpClientLock = new Object();
        private Hashtable FileReceiverThreadList = new Hashtable();
        private Hashtable NoticeDetailForm = new Hashtable();
        private Hashtable FileRcvFormList = new Hashtable();
        private Hashtable TransferNotiArea = new Hashtable();
        private Hashtable CustomerList = new Hashtable();
        private ArrayList NotiFormList = new ArrayList();
        private ArrayList TransferNotiFormTable = new ArrayList();
        private Hashtable treesource = new Hashtable();
        private ArrayList MemoTable = new ArrayList();
        private ArrayList omitteamlist = new ArrayList();
        private bool connected = false;
        private bool serverAlive = true;
        private bool receiverStart = false;


        //스레드에서 폼 호출용 델리게이트
        private delegate void WriteLog(string m);
        private delegate void PanelCtrlDelegate(bool l);
        private delegate void FormTextCtrlDelegate(string c);
        private delegate void ArrangeCtrlDelegate(string[] ar);
        private delegate void ChildNodeDelegate(string st, ArrayList list);
        private delegate void AddChatMsgDelegate(string senderId, string sendername, string sendermsg);
        private delegate void ChangeStat(string name, string team);
        private delegate void Loginfail();
        private delegate void LogOutDelegate();
        //private delegate int onFlashWindow(ChatForm form);
        private delegate Hashtable GetMemberDelegate(Hashtable treeSource, string teamName);
        private delegate Hashtable GetAllMemberDelegate();
        private delegate void DelChatterDelegate(string id, string name);
        private delegate string[] GetChattersDelegate();
        private delegate void SetChatterStatusDelegate(string userId, string userName, string status);
        //private delegate void AddChatterDelegate(string id, string name);
        private delegate void AddChatterDelegate(UserObject userObj);
        private delegate void NoticeListSorting(int index);
        private delegate void ShowTransInfoDele(string ani, string senderid, string date, string time);

        public delegate void stringDele(string ani);
        public delegate void doublestringDele(string ani, string calltype);
        private delegate void objectDele(object obj);
        private delegate void RingingDele(string ani, string name, string server_type);
        private delegate void AbandonDele();
        public delegate void NoParamDele();
        public delegate void intParamDele(int i);

        public event stringDele onAnswerEvent;
        public event NoParamDele onLoginEvent;
        public ArrayList cmstorage = new ArrayList();
        string ANI = "";
        string crmstat = "";
        CRMmanager.CRMmanager cm;
        private SkinSoft.AquaSkin.AquaSkin aquaSkin1;
        private PictureBox pbx_stat;
        private Label label4;
        private PictureBox pic_NRtrans;
        private Label NRtrans;
        private ToolStripMenuItem weDo정보ToolStripMenuItem;
        private Panel InfoBar;
        CRMmanager.FRM_MAIN crm_main;

        const string MEMO_HEADER = "m|";

        public Client_Form()
        {
            try
            {
                //
                // Windows Form 디자이너 지원에 필요합니다.
                //

                InitializeComponent();
                if (cm == null)
                    cm = new CRMmanager.CRMmanager();
                if (crm_main == null) {
                    crm_main = new FRM_MAIN();
                    crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                }
                //
                // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
                //
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }

        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        private void Client_Form_Load(object sender, EventArgs e)
        {
            startForm();
        }

        private void startForm()
        {
            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.SetBounds(screenWidth - this.Width, 0, this.Width, this.Height);

            checkProductMode();
            this.Text = MsgrTitle;

            readconfig();
            LogFileCheck();
            logWrite("LogFileCheck() 완료" + DateTime.Now.ToString());
            setLoginInfo();
            Microsoft.Win32.SystemEvents.SessionEnding += new Microsoft.Win32.SessionEndingEventHandler(SystemEvents_SessionEnding);
            t1.Interval = 10000;
            t1.Tick += new EventHandler(t1_Tick);
            timerForNotify.Interval = 10000;
            timerForNotify.Tick += new EventHandler(timerForNotify_Tick);
            makeTransferNotiArea();
            StartService();
            
        }

        /// <summary>
        /// 각 경로 지정
        /// </summary>
        private void checkProductMode()
        {
            string temp = Process.GetCurrentProcess().ProcessName;
            if (temp.IndexOf('.') < 0)
            {
                AppName = temp;
            }
            else
            {
                AppName = temp.Substring(0, temp.IndexOf('.'));
            }

            AppConfigName = string.Format(CommonDef.APP_CONFIG_NAME, temp);
            AppConfigFullPath = Application.StartupPath + CommonDef.PATH_DELIM + AppConfigName;

            AppRegName = CommonDef.REG_APP_NAME;
            XmlConfigFullPath = CommonDef.WORK_DIR + CommonDef.PATH_DELIM + CommonDef.CONFIG_DIR + CommonDef.PATH_DELIM + CommonDef.XML_CONFIG_PROD;
            XmlConfigOrgFullPath = Application.StartupPath + CommonDef.PATH_DELIM + CommonDef.XML_CONFIG_PROD;
            MsgrTitle = CommonDef.MSGR_TITLE_PROD;
            version = CommonDef.APP_VERSION;

        }

        private void makeTransferNotiArea()
        {
            try
            {
                int remnantArea = screenHeight;
                int end = screenHeight - 48;
                TransferNotiArea[end.ToString()] = "0";
                for (int i = 1; i < 5; i++)
                {
                    end -= 48;
                    TransferNotiArea[end.ToString()] = "0";
                    logWrite("TransferNotiArea[" + end.ToString() + "]");
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        /// <summary>
        /// app.config xml 설정파일 정보 로드
        /// </summary>
        private void readconfig()
        {
            try
            {
                top = System.Configuration.ConfigurationSettings.AppSettings["topmost"].ToString();
                string sNopop = System.Configuration.ConfigurationSettings.AppSettings["nopop"].ToString();

                if (sNopop.Equals("1"))
                {
                    this.nopop = true;
                }

                string sNopop_outbound = System.Configuration.ConfigurationSettings.AppSettings["nopop_outbound"].ToString();
                if (sNopop_outbound.Equals("1"))
                {
                    this.nopop_outbound = true;
                }

                if (top.Equals("1"))
                {
                    this.TopMost = true;
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }


        private void setLoginInfo()
        {
            try
            {
                id.Text = System.Configuration.ConfigurationSettings.AppSettings["id"];
                extension = System.Configuration.ConfigurationSettings.AppSettings["extension"];

                custom_font = System.Configuration.ConfigurationSettings.AppSettings["custom_font"];
                custom_color = System.Configuration.ConfigurationSettings.AppSettings["custom_color"];

                if (System.Configuration.ConfigurationSettings.AppSettings["save_pass"].Equals("1"))
                {
                    tbx_pass.Text = System.Configuration.ConfigurationSettings.AppSettings["pass"];
                    cbx_pass_save.Checked = true;
                }
            }
            catch (Exception ex)
            {
                logWrite("setLoginInfo Error : " + ex.ToString());
            }
        }

        private void t1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (popform != null)
                {
                    popform.Close();
                    t1.Stop();
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void SystemEvents_SessionEnding(object sender, Microsoft.Win32.SessionEndingEventArgs e)
        {
            try
            {
                //MessageBox.Show("시스템 종료");
                if (connected == true)
                {
                    LogOut();
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void menu_notifyicon_MouseLeave(object sender, EventArgs e)
        {
            menu_notifyicon.Close();
        }

        private void notifyIcon_Click(object sender, MouseEventArgs e)
        {
            
            int pointx = System.Windows.Forms.StatusBar.MousePosition.X;
            int pointy = System.Windows.Forms.StatusBar.MousePosition.Y;
            if (e.Button == MouseButtons.Left)
            {
                string topmost = System.Configuration.ConfigurationSettings.AppSettings["topmost"];
                if (topmost.Equals("0"))
                {
                    this.TopMost = false;
                }
                else
                {
                    this.TopMost = true;
                }
                //this.WindowState = FormWindowState.Normal;
                this.SetBounds(mainform_x, mainform_y, mainform_width, mainform_height);
                this.ShowInTaskbar = true;
                this.Show();
                this.Activate();
                isHide = false;
                firstCall = false;
                
            }
            else
            {
                menu_notifyicon.Show(pointx, pointy);
            }
        }
        
        private void login_Click(object sender, EventArgs e)
        {
            checkInfoForLogin();
        }

        /// <summary>
        /// 각 통신에 사용할 UDP 소켓 인스턴스 생성 및 메시지 수신 Thread 시작
        /// </summary>
        /// 
        private void StartService()
        {
            try
            {
                if (this.myid != null && this.mypass != null && extension != null)
                {
                    //init Logger
                    Logger.setLogDir(CommonDef.LOG_DIR);
                    Logger.setLogFile(CommonDef.FTP_LOG_FILE);
                    //Logger.setLogLevel(LOGLEVEL);

                    panel_progress.Visible = true;
                    defaultCtrl(false);
                    logWrite("StartService() 시작");

                    serverIP = System.Configuration.ConfigurationSettings.AppSettings["serverip"];
                    socket_port_crm = System.Configuration.ConfigurationSettings.AppSettings["socket_port_crm"];

                    IPHostEntry ihe = Dns.GetHostByName(Dns.GetHostName());
                    local = ihe.AddressList[0];
                    logWrite(local.ToString());
                    if (serverIP.ToLower().Equals("localhost")
                        || serverIP.ToLower().Equals("127.0.0.1"))
                    {
                        //serverIP = "127.0.0.1";
                        IPHostEntry host = Dns.Resolve(Dns.GetHostName());
                        serverIP = host.AddressList[0].ToString();
                    }
                    server = new IPEndPoint(IPAddress.Parse(serverIP), 8881);
                    try
                    {
                        IPEndPoint listen = new IPEndPoint(IPAddress.Any, listenport);
                        IPEndPoint send = new IPEndPoint(IPAddress.Any, sendport);
                        IPEndPoint check = new IPEndPoint(IPAddress.Any, checkport);

                        listenSock = new UdpClient(listen);
                        listenSock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 2000);
                        //listenSock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                        sendSock = new UdpClient(send);
                        checkSock = new UdpClient(check);

                        //filesender = new IPEndPoint(IPAddress.Any, filereceiveport);
                        //filesock = new UdpClient(filesender);

                        filesend = new IPEndPoint(IPAddress.Any, filesendport);
                        filesendSock = new UdpClient(filesend);
                    }
                    catch (Exception ex)
                    {
                        logWrite("StartService() socket 생성에러 : " + ex.ToString());
                    }

                    try
                    {
                        receive = new Thread(new ThreadStart(Receive));
                        receive.Start();
                        logWrite("Recieve 스레드 시작");
                    }
                    catch (Exception e)
                    {
                        logWrite("recieve 스레스 시작 에러 :" + e.ToString());
                    }

                    logWrite("서버에 접속시도");

                    SendInfo();
                }
                else if (this.myid == null)
                {
                    id.Focus();
                }
                else
                {
                    tbx_pass.Focus();
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void SendCheck()
        {
            try
            {
                IPEndPoint checkiep = new IPEndPoint(server.Address, 8885);
                byte[] re = null;
                checkSock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                byte[] buffer = Encoding.ASCII.GetBytes("1");
                while (true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        while (connected == true)
                        {
                            Thread.Sleep(3000);

                            try
                            {
                                int count = checkSock.Send(buffer, buffer.Length, checkiep);
                                re = checkSock.Receive(ref checkiep);
                                if (re == null)
                                {
                                    logWrite("메시지가 전달되지 않음");
                                    break;
                                }
                                else
                                {
                                    //로그인 시도중인 상황에서 서버응답 성공시
                                    if (serverAlive == false)
                                    {
                                        serverAlive = true;
                                        NoParamDele relogin = new NoParamDele(StartService);
                                        Invoke(relogin);
                                        PanelCtrlDelegate dele = new PanelCtrlDelegate(ServerFailCtrl);
                                        Invoke(dele, true);
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                logWrite("서버 체크 실패 " + i + "번째");
                                break;
                            }
                        } //while
                    } //for

                    if (connected == true)
                    {
                        if (serverAlive == true)
                        {
                            serverAlive = false;
                            LogOutDelegate logoutdele = new LogOutDelegate(LogOut);
                            Invoke(logoutdele);
                            LogOutDelegate dele = new LogOutDelegate(disposeServerFail);
                            Invoke(dele);
                        }
                    }
                } //while
            }
            catch (Exception ex2)
            {
                logWrite(ex2.ToString());
            }
        }

        private void disposeServerFail()
        {
            try
            {
                panel_progress.Visible = true;
                label_progress_status.Text = "접속중";
                ServerFailCtrl(false);
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void defaultCtrl(bool value)
        {
            tbx_pass.Enabled = value;
            id.Enabled = value;
            btn_login.Enabled = value;
            cbx_pass_save.Enabled = value;
            panel_progress.Visible = !value;
        }

        private void ServerFailCtrl(bool value)
        {
            label_id.Visible = value;
            label_pass.Visible = value;
            tbx_pass.Visible = value;
            id.Visible = value;
            btn_login.Visible = value;
            cbx_pass_save.Visible = value;
            pbx_loginCancel.Visible = !value;
        }

        /// <summary>
        /// 메시지 수신전용 메소드(프로토콜 : UDP)
        /// </summary>
        /// 
        private void Receive()
        {
            try
            {
                server = new IPEndPoint(IPAddress.Parse(serverIP), 8881);

                receiverStart = true;
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);// = new IPEndPoint(IPAddress.Any, 8883);

                byte[] buffer = null;


                while (true)
                {
                    try
                    {
                        logWrite("Receive() 수신대기 ");
                        if (listenSock != null)
                        {
                            buffer = listenSock.Receive(ref sender);
                        }
                        logWrite("수신! ");
                        if (buffer != null && buffer.Length != 0)
                        {
                            logWrite("sender IP : " + sender.Address.ToString());
                            logWrite("sender port : " + sender.Port.ToString());

                            listenSock.Send(buffer, buffer.Length, sender);  //정상적으로 메시지 수신하면 응답(udp통신의 실패방지)

                            string msg = Encoding.UTF8.GetString(buffer);

                            logWrite("수신 메시지 : " + msg);
                            MsgFilter(msg, sender);
                        }
                    }
                    catch (ThreadAbortException e)
                    {
                        logWrite("Receive() 에러 : " + e.ToString());
                    }
                    catch (SocketException e)
                    {
                        //CHOI_DEBUG 테스트위해 if막음
                        //if (connected == true)
                        logWrite("Receive() 에러 : " + e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (connected == true)
                    logWrite("##경고## : Receiver 가 중단되었습니다. ");
                logWrite(ex.ToString());
            }
            finally
            {
                logWrite("Msg Receive가 종료되었습니다.");
            }
        }

        /// <summary>
        /// 파일 수신 전용 메소드
        /// </summary>
        /// <param name="obj">파일 수신에 필요한 파일 정보(파일이름, 크기, 보낸사람, 보낸사람 id</param>
        private void FileReceiver(object obj)
        {
            try
            {
                Hashtable fileinfo = (Hashtable)obj;
                string filename = null;
                int filesize = 0;
                string sendername = null;
                string senderid = null;
                string fileseq = null;
                int rowIndex = 0;
                string NRseq = null;

                foreach (DictionaryEntry de in fileinfo)
                {
                    if (de.Key.ToString().Equals("name"))
                    {
                        sendername = de.Value.ToString();
                        logWrite("File sender name : " + sendername);
                    }
                    else if (de.Key.ToString().Equals("senderid"))
                    {
                        senderid = de.Value.ToString();
                        logWrite("File sender id : " + senderid);
                    }
                    else if (de.Key.Equals("filenum"))
                    {
                        fileseq = de.Value.ToString();
                    }
                    else if (de.Key.Equals("rowindex"))
                    {
                        rowIndex = Convert.ToInt32(de.Value);
                    }
                    else if (de.Key.Equals("NRseq"))
                    {
                        NRseq = de.Value.ToString();
                    }
                    else if (de.Key.Equals("filename"))
                    {
                        filename = (string)de.Value;
                        logWrite("FileReceiver() : filename=" + filename);
                    }
                    else if (de.Key.Equals("filesize"))
                    {
                        filesize = (int)de.Value;
                        logWrite("FileReceiver() : filesize=" + filesize);
                    }
                }
                #region 파일중복체크/막음
                //FileInfo fi = new FileInfo(filename);
                //int filenum = 0;
                //int strlength = filename.Length;
                //string ext = fi.Extension;

                ////저장할 파일과 같은 이름의 파일이 존재할 경우 처리로직
                //if (fi.Exists == true)
                //{
                //    string str = filename.Substring(0, (strlength - 4));
                //    string tempname = null;
                //    do
                //    {
                //        filenum++;
                //        str += "(" + filenum.ToString() + ")"; //같은 이름의 파일이 있을 경우 "파일명(filenum).확장자" 의 형태로 저장
                //        tempname = str + ext;
                //        fi = new FileInfo(tempname);
                //    } while (fi.Exists == true);
                //    filename = tempname;
                //}
                #endregion
                logWrite("CHOI_DEBUG FileReceiver 1");//CHOI_DEBUG
                if (fileseq == null) //서버수신이 아니고 직접수신인 경우
                {
                    FILE_TCPReceive(filename);
                    logWrite("CHOI_DEBUG FileReceiver 2");//CHOI_DEBUG
                    return;
                }
                logWrite("CHOI_DEBUG FileReceiver 3");//CHOI_DEBUG
            
#region //File UDP Receive
                /*****************************************************************************************************************************
                *****************************************************************************************************************************
                ******************* File UDP Receive********************************************************************************************
                *****************************************************************************************************************************
                * 1. run Listener : file name , size, listen port
                * 2. set event listener
                * 3. progress bar setting
                *****************************************************************************************************************************
                */
                int receivefailcount = 0;
                int sendfailcount = 0;
                byte[] buffer = null;

                FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read, 40960); //한번에 40960 바이트씩 수신

                if (filesock == null || filesock.Client == null)
                {
                    filesender = new IPEndPoint(IPAddress.Any, filereceiveport);
                    filesock = new UdpClient(filesender);
                }

                filesock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);

                try
                {
                    lock (filesock)
                    {
                        while (true)
                        {
                            Thread.Sleep(10);
                            //logWrite("FileReceiver() 수신대기 ");
                            try
                            {
                                buffer = filesock.Receive(ref filesender);
                                //logWrite("수신!");
                            }
                            catch (Exception se)
                            {
                                logWrite("FileReceiver()  filesock.Receive 에러 : " + se.ToString());
                                receivefailcount++;
                                if (receivefailcount == 5)
                                {
                                    logWrite("수신 실패 5회로 FileReceiver 중단!");
                                    break;
                                }
                            }
                            if (buffer != null && buffer.Length != 0)
                            {
                                //logWrite("sender IP : " + filesender.Address.ToString());
                                //logWrite("sender port : " + filesender.Port.ToString());

                                byte[] receivebyte = Encoding.UTF8.GetBytes(buffer.Length.ToString());

                                try
                                {
                                    filesock.Send(receivebyte, receivebyte.Length, filesender);  //정상적으로 메시지 수신하면 응답(udp통신의 실패방지)

                                    //logWrite(buffer.Length.ToString() + " byte 수신!");
                                }
                                catch (Exception se1)
                                {
                                    logWrite("FileReceiver() filesock.Send 에러 : " + se1.ToString());
                                    sendfailcount++;
                                    if (sendfailcount == 5)
                                    {
                                        logWrite("응답전송 실패 5회로 FileReceiver 중단!");
                                        break;
                                    }
                                }
                                if (fs.CanWrite == true)
                                {
                                    try
                                    {
                                        fs.Write(buffer, 0, buffer.Length);
                                        fs.Flush();
                                    }
                                    catch (Exception e)
                                    {
                                        logWrite("FileStream.Write() 에러 : " + e.ToString());
                                    }
                                }
                                FileInfo finfo = new FileInfo(filename);

                                int size = Convert.ToInt32(finfo.Length);

                                if (size > 0)
                                {
                                    if (NRseq == null) NRseq = "0";
                                    SendMsg("14|" + NRseq, server);
                                    intParamDele intdele = new intParamDele(refreshNRfile);
                                    Invoke(intdele, rowIndex);

                                }

                                if (size >= filesize)  //파일 데이터를 모두 수신했는지 체크
                                {
                                    if (filesender.Address.Equals(server.Address))
                                    {
                                        ChangeStat showdownloadform = new ChangeStat(ShowDownloadForm); //다운로드 상태바 표시 delegate
                                        Invoke(showdownloadform, new object[] { "서버로 부터", filename });
                                    }
                                    else
                                    {
                                        foreach (DictionaryEntry de in InList)
                                        {
                                            if (((IPEndPoint)de.Value).Address.Equals(filesender.Address))
                                            {
                                                senderid = de.Key.ToString();
                                                logWrite(senderid);
                                                sendername = GetUserName(senderid);
                                                break;
                                            }
                                        }
                                        logWrite("파일 전송 완료");

                                        ChangeStat showdownloadform = new ChangeStat(ShowDownloadForm);
                                        if (senderid.Length > 1)
                                        {
                                            Invoke(showdownloadform, new object[] { sendername + "(" + senderid + ") 님으로 부터", filename });
                                        }
                                        else
                                        {
                                            Invoke(showdownloadform, new object[] { "서버로 부터", filename });
                                        }
                                    }
                                    fs.Close();
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    logWrite("FileReceive() 에러 : " + e.ToString());
                }
                logWrite("FileReceiver 가 중단되었습니다. ");
#endregion //File UDP Send

           }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        public void FILE_UDPReceive(string filename)
        {
            
        }

        //수락여부 확인 플로우로 넘김
        //1.수락
        //      FTP_ShowDialog(Hashtable info);
        //                 form.ShowDialog(Hashtable info);
        //          event FTP_DialogResultReceived(CustomEvent);
        //2-1.거부
        //2-2.종료
        //          event FTP_StatusChanged
        //                  
        //2.수락메시지전송
        //      
        //3.수신대기 
        //      FileReceiver
        //4.수신
        //      FTP_ShowStatus(Receive|Cancel|Done|Error)
        //              form.ShowStatus(Hashtable info);
        //5.완료
        //      FTP_StopReceiving()
        //6.취소
        //      FTP_CancelReceiving()
        //7.실패
        //     
        ShowFileRcvStatDelegate showFileRcvStatDelegate;
        public void FTP_ShowDialg(DownloadInfo info)
        {
            //call DownloadForm
            DownloadForm frm = new DownloadForm();
            frm.DownloadYNChecked += FTP_DiaglogResultReceived;
            frm.DownloadStatusChanged += FTP_DialogStatusChanged;
            
            frm.ShowDialog(info);
            //frm.Show();
            FileRcvFormList[info.MsgFileKey] = frm;
            mFormKeyRcv = info.MsgFileKey; //CHOI_DEBUG
            showFileRcvStatDelegate = new ShowFileRcvStatDelegate(ShowFileReceiveStatus);
        }

        public void FTP_DiaglogResultReceived(object sender, CustomEventArgs e)
        {
            DownloadInfo info = (DownloadInfo)e.GetItem;
            string fileName = info.FileName;
            int fileSize = info.FileSize;
            string senderId = info.SenderId;
            string senderName = info.SenderName;
            string msgFileKey = info.MsgFileKey;
            IPEndPoint siep = (IPEndPoint)InList[info.SenderId];
            siep.Port = 8883;

            if (info.DownloadAccepted) //수락허가
            {
                //SendOk
                //파일받기 수락 메시지전송
                //SendMsg("Y|" + tempMsg[1] + "|" + tempMsg[3] + "|" + this.myid, siep); //구성(Y|파일명|파일Key|수신자id)
                SendMsg("Y|" + info.SenderFileName + "|" + info.MsgFileKey + "|" + info.MyId, siep); //구성(Y|파일명|파일Key|수신자id)
                //파일수신대기/수신

                Hashtable hsInfo = new Hashtable();
                hsInfo["filename"] = info.FileName;
                hsInfo["filesize"] = info.FileSize;
                hsInfo["name"] = info.SenderName;
                hsInfo["senderid"] = info.SenderId;
                hsInfo["myid"] = info.MyId;
                FileReceiver((object)hsInfo);
            }
            else  //수락거부 파일 받기 거부("N|파일명|파일키|id)
            {
                SendMsg("N|" + info.SenderFileName + "|" + info.MsgFileKey + "|" + info.MyId, siep); //구성(Y|파일명|파일Key|수신자id)
                //if (FileSendFormList.ContainsKey(key) && FileSendFormList[key] != null)
                //{
                //    FileSendFormList.Remove(key);
                //    logWrite("FileSendFormList.Remove(key) :" + key);
                //}
            }
        }

        public void FTP_DialogStatusChanged(object sender, CustomEventArgs e)
        {
            //SUCCESS
            //CANCEL
            //ERROR
            int result = (int)e.GetItem;
            switch (result) 
            {
                case DownloadStatus.SUCCESS :
                    break;
                case  DownloadStatus.FAILED:
                    logWrite("CHOI_DEBUG FTP_DialogStatusChanged:FAILED");
                    break;
                case DownloadStatus.CANCELED:
                    FTP_Server_Cancel();
                    break;
            }
        }

        public void FTP_Stop()
        {
            logWrite("FTP Server 중지");
            try
            {
                if (mFtpSocketListener != null)
                    mFtpSocketListener.StopListening();
            }
            catch (Exception ex)
            {
                logWrite("FTP Server 중지실패:" + ex.ToString());
            }
        }

        public void FTP_Server_Cancel()
        {
            if (mFtpSocketListener != null)
                mFtpSocketListener.CancelReceiving(null);
        }

        string mFormKeyRcv;
        public void FTP_RcvStatusChanged(object sender, SocStatusEventArgs e)
        {
            if (e.Status.Cmd == MsgDef.MSG_BYE)
            {
                FTP_Stop();
            }
            if (e.Status.status == SocHandlerStatus.ERROR)
            {
                logWrite("CHOI_DEBUG e.Status.status == SocHandlerStatus.ERROR");

                FTP_InvokeDownloadForm(DownloadStatus.FAILED, -1, mFormKeyRcv);
            }
            else
            {
                switch (e.Status.ftpStatus)
                {
                    case FTPStatus.RECEIVE_STREAM://진행
                        FTP_InvokeDownloadForm(DownloadStatus.RECEIVING, (int)(100 * e.Status.fileSizeDone / e.Status.FileSize), mFormKeyRcv);
                        break;
                    case FTPStatus.RECEIVED_FILE_INFO://시작
                        FTP_InvokeDownloadForm(DownloadStatus.START, -1, mFormKeyRcv);
                        break;
                    case FTPStatus.SENT_DONE://종료
                        FTP_InvokeDownloadForm(DownloadStatus.END, -1, mFormKeyRcv);
                        break;
                }
            }
        }

        public void FTP_InvokeDownloadForm(int status, int value, string formKey)
        {
            DownloadForm downloadForm = (DownloadForm)FileRcvFormList[formKey];
            if (FileRcvFormList.Count != 0 && FileRcvFormList[formKey] != null)
            {
                Invoke(showFileRcvStatDelegate, new object[] { status, value, downloadForm });
            }
        }

        /// <summary>
        /// 파일 전송 상태값 변경
        /// </summary>
        /// <param name="stat">상태</param>
        /// <param name="form">대상 전송 폼</param>
        private void ShowFileReceiveStatus(int stat, int idx, DownloadForm form)
        {
            try
            {

                logWrite("CHOI_DEBUG ShowFileReceiveStatus stat"+stat);
                form.ShowFileReceiveStatus(stat, idx);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        public void FILE_TCPReceive(string filename)
        {
            /*****************************************************************************************************************************
             *****************************************************************************************************************************
             ******************* File UDP Send********************************************************************************************
             *****************************************************************************************************************************
             * 1. run Listener : file name , size, listen port
             * 2. set event listener
             * 3. progress bar setting
             *****************************************************************************************************************************
             */
            try
            {
                mFtpSocketListener = new FtpSocketListener(SocketDef.PORT_FILE_NEW_SERVER, FilesDir.FullName);

                mFtpSocketListener.SocStatusChanged += FTP_RcvStatusChanged;
                mFtpSocketListener.StartListening();
            }
            catch (SocketException e)
            {
                logWrite("FileReceive() 에러 : " + e.ToString());
            }
            logWrite("FileReceiver 가 중단되었습니다. ");
        }

        //1. 

        /// <summary>
        /// 다운로드 완료를 알리고 다운폴더 열기 또는 파일열기 선택 폼
        /// </summary>
        /// <param name="sendername">파일 보낸사람</param>
        /// <param name="filename">파일명</param>
        private void ShowDownloadForm(string sendername, string filename)
        {
            try
            {
                string[] farray = filename.Split('\\');
                downloadform = new DownloadResultForm();
                downloadform.btn_opendir.MouseClick += new MouseEventHandler(btn_opendir_Click);
                downloadform.btn_openfile.MouseClick += new MouseEventHandler(btn_openfile_Click);
                downloadform.label_sender.Text = sendername;
                downloadform.label_filename.Text = farray[(farray.Length - 1)];
                downloadform.label_fullname.Text = filename;
                downloadform.DoFlashWindow();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }


        /// <summary>
        /// 전송완료된 상태에서 파일열기 선택시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_openfile_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string filename = null;
                int num = button.Parent.Controls.Count;
                for (int i = 0; i < num; i++)
                {
                    if (button.Parent.Controls[i].Name.Equals("label_fullname"))
                    {
                        filename = button.Parent.Controls[i].Text;
                        break;
                    }
                }
                System.Diagnostics.Process.Start(filename);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        /// <summary>
        /// 전송완료된 상태에서 다운폴더 열기 선택시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_opendir_Click(object sender, MouseEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string filename = null;
                int num = button.Parent.Controls.Count;
                for (int i = 0; i < num; i++)
                {
                    if (button.Parent.Controls[i].Name.Equals("label_fullname"))
                    {
                        filename = button.Parent.Controls[i].Text;
                        break;
                    }
                }
                FileInfo fileinfo = new FileInfo(filename);
                string dirname = fileinfo.DirectoryName;
                System.Diagnostics.Process.Start(dirname);
                
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 메시지 전송 메소드(채팅 및 서버 전송 메시지)
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="iep">메시지 발신자 IPEndPoint</param>
        /// 
        public void SendMsg(string msg, IPEndPoint iep)
        {
            try
            {
                //iep.Port=listenport; //보내기전 포트를 수신전용 포트로 수정
                if (iep.Port == 8882) //WeDo서버에서 8882에서 보낸경우(to:8883) 8881로 보냄
                {
                    iep.Port = 8881;
                }
                else if (iep.Port != 8881) //그외의 경우 WeDo서버 8884에서 보낸경우 8883으로 보냄
                {
                    iep.Port = listenport;
                }

                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                byte[] re = null;
                sendSock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 500);//정상 수신 메시지 대기시간 설정

                logWrite("Receiver IP : " + iep.Address.ToString());
                logWrite("Receiver port : " + iep.Port.ToString());
                logWrite("발송 메시지 :" + msg);

                for (int i = 0; i < 2; i++)
                {
                    logWrite("SendMsg() : " + i.ToString() + " 번째 시도!");
                    sendSock.Send(buffer, buffer.Length, iep);

                    try
                    {
                        re = sendSock.Receive(ref iep);
                        if (re != null && re.Length != 0)
                        {
                            logWrite("응답메시지 : " + Encoding.UTF8.GetString(re));
                            break;
                        }

                    }
                    catch (SocketException e) {
                        logWrite(string.Format("SocketError[{0}] {1}",e.SocketErrorCode,e.ToString()));
                    }
                }

                if (re == null || re.Length == 0)
                {
                    logWrite("SendMsg() 에러 : 보낸 메세지에 응답하지 않습니다.");
                    string[] str1 = msg.Split('|');

                    if (str1[0].Equals("8")) //최초 로그인 접속일 경우
                    {
                        logWrite("로그인 접속에 서버가 응답하지 않습니다. [서버정보] :" + iep.Address.ToString() + ":" + iep.Port.ToString());
                        MessageBox.Show(this, "서버가 응답하지 않습니다.\r\n[서버정보] :" + iep.Address.ToString() + ":" + iep.Port.ToString(), "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        panel_progress.Visible = false;
                        defaultCtrl(true);
                        closing();
                    }
                    else if (iep.Port == 8881)
                    {
                        logWrite("서버가 응답하지 않습니다. 서버정보 :" + iep.Address.ToString() + ":" + iep.Port.ToString());
                    }
                    else if (str1[0].Equals("m"))
                    {
                        SendMsg("4|" + msg, server);
                    }
                    else
                    {
                        logWrite("EndPoint : " + iep.Address.ToString() + ":" + iep.Port.ToString() + " 가 응답하지 않았습니다.");
                        foreach (DictionaryEntry de in InList)
                        {
                            IPEndPoint ipoint = (IPEndPoint)de.Value;
                            listenport = CallCtrlUtils.getListenPort(ipoint.Port);

                            if (iep.Port == ipoint.Port)
                            {
                                SendMsg("3|" + de.Key.ToString(), server);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        /// <summary>
        /// 다른 사용자(들)에게 파일 전송 준비
        /// </summary>
        /// <param name="list">파일 전송 대상자 목록</param>
        /// <param name="filename">파일명</param>
        /// <param name="timekey">파일전송시간(여러 파일전송작업 간 구분키가 됨)</param>
        private void FileSendRequest(ArrayList list, string filename, string timekey) 
        {
            try
            {
                FileInfo fi = new FileInfo(filename);
                long size = fi.Length;
                ShowFileSendDetailDelegate detail = new ShowFileSendDetailDelegate(ShowFileSendDetail);


                //파일 정보 보내기(F|파일명|파일크기|파일key|전송자id)
                string msg = "F|" + filename + "|" + size.ToString() + "|" + timekey + "|" + this.myid;
                string smsg = "5|" + filename + "|" + size.ToString() + "|" + timekey + "|" + this.myid + "|";
                logWrite("FileSendRequest() 파일정보 메시지 생성 : " + msg);
                logWrite("FileSendRequest() 파일정보 메시지 생성 : " + smsg);
                FileSendDetailListView view = (FileSendDetailListView)FileSendDetailList[timekey];
                bool outter = false;
                foreach (string id in list)
                {
                    if (InList.ContainsKey(id) && InList[id] != null) //전송대상자가 로그인 상태인 경우
                    {
                        IPEndPoint fiep = (IPEndPoint)InList[id];
                        fiep.Port = listenport;
                        SendMsg(msg, fiep);
                        Invoke(detail, new object[] { id, "수락 대기중", view });
                    }
                    else  //전송대상자가 로그아웃 상태인 경우
                    {
                        smsg += id + ";";
                        Invoke(detail, new object[] { id, "서버전송", view });
                        outter = true;
                    }
                }
                if (outter == true)
                    SendMsg(smsg, server);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        /// <summary>
        /// 파일 전송 메소드
        /// </summary>
        /// <param name="obj">파일 전송에 필요한 정보(Y|파일명|파일폼구분key|수신자id) </param>
        private void SendFile(object obj) //tempMsg(Y|파일명|파일폼키|수신자id)
        {
            try
            {
                Hashtable table = (Hashtable)obj;
                IPEndPoint iep = null;
                string[] info = null;

                foreach (DictionaryEntry de in table)
                {
                    info = (string[])de.Key;
                    iep = (IPEndPoint)de.Value;
                }

                //수신수락시 전송인 경우
                if (info[0].Equals("Y"))
                {
                    //iep.Port = filereceiveport;   //사용자파일전용 포트로 변경
                    FILE_TCPSend(info[1], info[2], info[3], iep);

                }
                else
                {
                    iep.Port = 9001;   //서버파일전용 포트로 변경
                    FILE_UDPSend(info[1], info[2], info[3], iep);
                }

            }
            catch (IOException ioexception)
            {
                MessageBox.Show("파일이 현재 편집상태 입니다.\r\n 파일을 닫거나 읽기전용으로 변경해 주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        public void FILE_UDPSend(string fileName, string formKey, string rcvId, IPEndPoint iep) 
        {
            logWrite("FILE_UDPSend: 파일전송 포트 변경 :" + iep.Port.ToString());

            SendFileForm sendFileForm = (SendFileForm)FileSendFormList[formKey];
            FileSendDetailListView fileSendDetailList = (FileSendDetailListView)FileSendDetailList[formKey];

            showFileSendDetailDelegate = new ShowFileSendDetailDelegate(ShowFileSendDetail);
            showFileSendStatDelegate = new ShowFileSendStatDelegate(ShowFileSendStat);
            showCloseButtonDelegate = new ShowCloseButtonDelegate(ShowCloseButton);
            
            FileInfo fi = new FileInfo(fileName);
            logWrite("FILE_UDPSend: FileInfo 인스턴스 생성 : " + fileName);

            int read = 0;
            byte[] buffer = null;
            byte[] re = null;

            if (filesendSock == null)
            {
                filesend = new IPEndPoint(IPAddress.Any, filesendport);
                filesendSock = new UdpClient(filesend);
            }
            else
            {
                filesendSock.Close();
                //filesend = new IPEndPoint(IPAddress.Any, filesendport);
                filesendSock = new UdpClient(filesend);
            }

            filesendSock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);

            if (!fi.Exists)
            {
                logWrite("SendFile() 파일이 없음 : " + fileName);
                return;
            }

            BufferedStream bs = new BufferedStream(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 40960), 40960);

            double sendfilesize = Convert.ToDouble(fi.Length);
            double percent = (40960 / sendfilesize) * 100;
            double total = 0.0;

            lock (filesendSock)
            {
                while (true)
                {
                    for (int i = 0; i < 3; i++) //udp 통신의 전송실패 방지
                    {
                        try
                        {
                            logWrite("FileReceiver IP : " + iep.Address.ToString());
                            logWrite("FileReceiver port : " + iep.Port.ToString());
                            if (sendfilesize >= 40960.0)
                                buffer = new byte[40960];
                            else buffer = new byte[Convert.ToInt32(sendfilesize)];
                            read = bs.Read(buffer, 0, buffer.Length);
                            filesendSock.Send(buffer, buffer.Length, iep);
                            logWrite("filesendSock.Send() : " + i.ToString() + " 번째 시도!");
                        }
                        catch (Exception e)
                        {
                            logWrite("SendFile() BufferedStream.Read() 에러 :" + e.ToString());
                        }
                        try
                        {
                            re = filesendSock.Receive(ref iep);
                            int reSize = int.Parse(Encoding.UTF8.GetString(re));
                            if (reSize == buffer.Length) break;
                        }
                        catch (SocketException e1)
                        {
                            logWrite(e1.ToString());
                        }
                    }

                    if (re == null || re.Length == 0)
                    {
                        logWrite("filesendSock.Send() 상대방이 응답하지 않습니다. 수신자 정보 : " + iep.Address.ToString() + ":" + iep.Port.ToString());
                        MessageBox.Show("파일전송 실패 : 상대방이 응답하지 않음", "전송실패", MessageBoxButtons.OK);
                        if (FileSendFormList.Count != 0 && FileSendFormList[formKey] != null)
                        {
                            Invoke(showFileSendStatDelegate, new object[] { "전송실패",-1, sendFileForm});
                        }
                        break;
                    }
                    else
                    {
                        sendfilesize = (sendfilesize - 40960.0);
                        total += percent;
                        if (total >= 100.0) total = 100.0;
                        string[] totalArray = (total.ToString()).Split('.');

                        if (!iep.Address.ToString().Equals(serverIP))
                        {
                            if (FileSendFormList.Count != 0 && FileSendFormList[formKey] != null)
                            {
                                Invoke(showFileSendStatDelegate, new object[] { "전송중(" + totalArray[0] + " %)",(int)total, sendFileForm });
                            }
                            if (FileSendDetailList.Count != 0 && FileSendDetailList[formKey] != null)
                            {
                                string detailmsg = "전송중(" + totalArray[0] + " %)";
                                Invoke(showFileSendDetailDelegate, new object[] { rcvId, detailmsg, fileSendDetailList });
                            }
                        }
                    }
                    if (total == 100.0)
                    {
                        string detailmsg = "전송완료";
                        if (iep.Address.ToString().Equals(serverIP))
                        {
                            Invoke(showFileSendDetailDelegate, new object[] { "server", detailmsg, fileSendDetailList });
                        }
                        else
                        {
                            Invoke(showFileSendDetailDelegate, new object[] { rcvId, detailmsg, fileSendDetailList });
                        }
                        bool isFinished = (bool)Invoke(showCloseButtonDelegate, fileSendDetailList);  //자세히
                        if (isFinished == false)
                            Invoke(showFileSendStatDelegate, new object[] { detailmsg, (int)total, sendFileForm });  //FileSendForm
                        else
                            Invoke(showFileSendStatDelegate, new object[] { "Finish",(int)total, sendFileForm });  //FileSendForm
                    }
                    if (total == 100.0)
                    {
                        bs.Close();
                        break;
                    }
                }
            }

            try
            {
                if (FileSendThreadList.Count != 0 && FileSendThreadList[formKey + "|" + rcvId] != null)
                {
                    ((Thread)FileSendThreadList[formKey + "|" + rcvId]).Abort();
                    logWrite("FileSendThread Abort()!");
                }
            }
            catch (ThreadAbortException te)
            {
                logWrite(te.ToString());
            }
        }

        public void FILE_TCPSend(string fileName, string formKey, string rcvId, IPEndPoint iep) 
        {
            mRcvId = rcvId;
            logWrite("FILE_TCPSend: 파일전송 포트 변경 :" + SocketDef.PORT_FILE_NEW_SERVER);

            FileInfo fi = new FileInfo(fileName);
            logWrite("FILE_TCPSend: FileInfo 인스턴스 생성 : " + fileName);

            showFileSendDetailDelegate = new ShowFileSendDetailDelegate(ShowFileSendDetail);
            showFileSendStatDelegate = new ShowFileSendStatDelegate(ShowFileSendStat);
            showFileSendStatExDelegate = new ShowFileSendStatExDelegate(ShowFileSendStatEx);
            showCloseButtonDelegate = new ShowCloseButtonDelegate(ShowCloseButton);
            
            SendFileForm sendFileForm = (SendFileForm)FileSendFormList[formKey];
            FileSendDetailListView fileSendDetailList = (FileSendDetailListView)FileSendDetailList[formKey];

            if (!fi.Exists)
            {
                logWrite("SendFile() 파일이 없음 : " + fileName);
                return;
            }

            FtpClientManager ftpClient = new FtpClientManager(iep.Address.ToString(), SocketDef.PORT_FILE_NEW_SERVER, formKey);
            FtpClientThreadList[formKey] = ftpClient;
            ftpClient.SocStatusChanged += FTP_SndStatusChanged;

            if (!ftpClient.IsConnected())
            {
                if (ftpClient.Connect())
                    logWrite("[FILE_TCPSend]Server Connected.");
                else
                {
                    logWrite("[FILE_TCPSend]Server Not Connected.");
                    MessageBox.Show("파일전송 실패 : 상대방이 응답하지 않음", "전송실패", MessageBoxButtons.OK);
                    if (FileSendFormList.Count != 0 && FileSendFormList[formKey] != null)
                    {
                        //CHOI_DEBUG Invoke(showFileSendStatDelegate, new object[] { "전송실패", -1, sendFileForm });
                        Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.FAILED, "전송실패", -1, sendFileForm });
                    }
                    return;
                }
            }
            else
                logWrite("[FILE_TCPSend]Server Already Connected.");

            string file = Utils.GetFileName(fileName);
            string path = Utils.GetPath(fileName);

            ftpClient.setFileName(file);
            ftpClient.setFilePath(path);

            try
            {
                if (ftpClient.SendFile())
                {
                    string detailmsg = "전송완료";
                    if (iep.Address.ToString().Equals(serverIP))
                    {
                        Invoke(showFileSendDetailDelegate, new object[] { "server", detailmsg, fileSendDetailList });
                    }
                    else
                    {
                        Invoke(showFileSendDetailDelegate, new object[] { rcvId, detailmsg, fileSendDetailList });
                    }
                    bool isFinished = (bool)Invoke(showCloseButtonDelegate, fileSendDetailList);  //자세히
                    if (isFinished == false)
                        //CHOI_DEBUG Invoke(showFileSendStatDelegate, new object[] { detailmsg, 100, sendFileForm });  //FileSendForm
                        Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.SUCCESS, detailmsg, 100, sendFileForm });  //FileSendForm
                    else
                        //CHOI_DEBUG Invoke(showFileSendStatDelegate, new object[] { "Finish", 100, sendFileForm });  //FileSendForm
                        Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.SUCCESS, "Finish", 100, sendFileForm });  //FileSendForm
                }
                else
                {
                    logWrite("[FILE_TCPSend]Server Not Connected.");
                    MessageBox.Show("파일전송 실패 : 상대방이 응답하지 않음", "전송실패", MessageBoxButtons.OK);
                    if (FileSendFormList.Count != 0 && FileSendFormList[formKey] != null)
                    {
                        Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.FAILED, "전송실패", -1, sendFileForm });
                        //CHOI_DEBUG Invoke(showFileSendStatDelegate, new object[] { "전송실패", -1, sendFileForm });
                    }

                    ftpClient.Close();
                }
            }
            catch (IOException)
            {
            }
            finally
            {

            }
            try
            {
                if (FileSendThreadList.Count != 0 && FileSendThreadList[formKey + "|" + rcvId] != null)
                {
                    ((Thread)FileSendThreadList[formKey + "|" + rcvId]).Abort();
                    logWrite("FileSendThread Abort()!");
                }
            }
            catch (ThreadAbortException te)
            {
                logWrite(te.ToString());
            }
        }


        string mRcvId;

        private void FTP_SndStatusChanged(object sender, SocStatusEventArgs e)
        {
            try
            {
                StateObject stateObject = e.Status;
                if (FileSendFormList.Count != 0 && FileSendFormList[stateObject.key] != null)
                {
                    SendFileForm sendFileForm = (SendFileForm)FileSendFormList[stateObject.key];
                    FileSendDetailListView fileSendDetailList = (FileSendDetailListView)FileSendDetailList[stateObject.key];
                    logWrite("CHOI_DEBUG e.Status.status == SocHandlerStatus.ERROR");
                    switch (e.Status.status) 
                    {
                        case SocHandlerStatus.FTP_SENDING:
                            int transRate = (int)(stateObject.fileSizeDone * (long)100 / stateObject.FileSize);

                            logWrite("전송중(" + transRate + " %)");
                            Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.SENDING, "전송중(" + transRate + " %)", transRate, sendFileForm });
                            Invoke(showFileSendDetailDelegate, new object[] { mRcvId, "전송중(" + transRate + " %)", fileSendDetailList });
                            break;
                        case SocHandlerStatus.FTP_END:
                            logWrite("파일전송완료");
                            Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.SUCCESS, "전송완료", -1, sendFileForm });
                            Invoke(showFileSendDetailDelegate, new object[] { mRcvId, "전송완료", fileSendDetailList });
                            break;
                        case SocHandlerStatus.FTP_SERVER_CANCELED:
                        case SocHandlerStatus.FTP_CANCELED:
                            logWrite("파일전송취소");
                            Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.CANCELED, "전송취소", -1, sendFileForm });
                            Invoke(showFileSendDetailDelegate, new object[] { mRcvId, "전송취소", fileSendDetailList });
                            break;
                        case SocHandlerStatus.ERROR:
                            logWrite("파일전송실패");
                            Invoke(showFileSendStatExDelegate, new object[] { DownloadStatus.FAILED, "전송실패", -1, sendFileForm });
                            Invoke(showFileSendDetailDelegate, new object[] { mRcvId, "전송실패", fileSendDetailList });
                            break;
                    }

                }
                else
                {
                    logWrite(string.Format("에러 : FileSendFormList" + "해당화면없음."));
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
            //AppendConsoleMsg(sender, e, "");


            //sendfilesize = (sendfilesize - 40960.0);
            //total += percent;
            //if (total >= 100.0) total = 100.0;
            //string[] totalArray = (total.ToString()).Split('.');

            //if (!iep.Address.ToString().Equals(serverIP))
            //{
            //    if (FileSendFormList.Count != 0 && FileSendFormList[formKey] != null)
            //    {
            //        Invoke(showFileSendStatDelegate, new object[] { "전송중(" + totalArray[0] + " %)", form });
            //    }
            //    if (FileSendDetailList.Count != 0 && FileSendDetailList[formKey] != null)
            //    {
            //        string detailmsg = "전송중(" + totalArray[0] + " %)";
            //        Invoke(showFileSendDetailDelegate, new object[] { rcvId, detailmsg, view });
            //    }
            //}

        }

        /// <summary>
        /// 파일 전송 상태값 변경
        /// </summary>
        /// <param name="stat">상태</param>
        /// <param name="form">대상 전송 폼</param>
        private void ShowFileSendStat(string stat, int idx, SendFileForm form)
        {
            try
            {
                if (stat.Equals("Finish"))
                {
                    form.label_result.Text = "모든 파일 전송이 완료되었습니다.";
                    form.btn_cancel.Text = "닫  기";
                }
                else
                {
                    form.setProgress(idx);
                    form.label_result.Text = stat;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 파일 전송 상태값 변경
        /// </summary>
        /// <param name="stat">상태</param>
        /// <param name="form">대상 전송 폼</param>
        private void ShowFileSendStatEx(int status, string desc, int idx, SendFileForm form)
        {
            try
            {
                switch (status) { 
                    case DownloadStatus.SUCCESS:
                        form.label_result.Text = "모든 파일 전송이 완료되었습니다.";
                        form.btn_cancel.Text = "닫  기";
                        break;
                    case DownloadStatus.FAILED:
                        form.label_result.Text = "파일 전송이 실패되었습니다.";
                        form.setProgress(-1);
                        form.btn_cancel.Text = "닫  기";
                        break;
                    case DownloadStatus.SENDING:
                        form.setProgress(idx);
                        form.label_result.Text = desc;
                        break;
                    case DownloadStatus.CANCELED:
                        form.label_result.Text = "파일 전송이 취소되었습니다.";
                        form.setProgress(-1);
                        form.btn_cancel.Text = "닫  기";
                        break;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// FileSendForm 내 자세히 보기 클릭시 나타나는 리스트 뷰 폼의 상태값 변경
        /// </summary>
        /// <param name="keyid">리스트 뷰의 아이템 구분 id(파일 받는 사람의 id)</param>
        /// <param name="detail">표시하고자 하는 상태값</param>
        /// <param name="view">리스트 뷰 객체</param>
        private void ShowFileSendDetail(string keyid, string detail, FileSendDetailListView view)
        {
            try
            {
                ListViewItem[] itemArray = null;
                if (keyid.Equals("server"))
                {
                    foreach (ListViewItem item in view.listView.Items)
                    {
                        if (item.SubItems[1].Text.Equals("서버전송"))
                        {
                            item.SubItems[1].Text = detail;
                        }
                    }
                }
                else
                {
                    if (view.listView.Items.ContainsKey(keyid))
                    {
                        itemArray = view.listView.Items.Find(keyid, false);
                    }
                    if (itemArray != null)
                    {
                        itemArray[0].SubItems[1].Text = detail;
                    }
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// FileSendDetailListView 에서 모든 대상자가 전송완료 되면 FileSendForm 버튼을 변경
        /// </summary>
        /// <param name="view">FileSendDetailListView 객체</param>
        /// <returns></returns>
        private bool ShowCloseButton(FileSendDetailListView view)
        {
            bool isFinished = false;
            int num = 0;
            try
            {
                for (int i = 0; i < view.listView.Items.Count; i++)
                {
                    if (!view.listView.Items[i].SubItems[1].Text.Equals("전송완료")) num++;
                }
                if (num == 0) isFinished = true;
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
            return isFinished;
        }

        /// <summary>
        /// 수신된 메시지를 분석하여 각 요청에 맞게 처리
        /// </summary>
        /// <param name="obj">ArrayList로 형변환할 Object</param>
        protected void MsgFilter(object obj, IPEndPoint iep)
        {
            try
            {
                string msg = ((string)obj).Trim(); //수신 메시지
                string tempFormKey;
                string[] tempMsg = msg.Split('|');
                string code = tempMsg[0];

                switch (code)
                {
#region //일반메신저기능

                    case "f"://로그인 실패시(f|n or p)
                        try
                        {
                            FormTextCtrlDelegate informationMsg = new FormTextCtrlDelegate(ShowMessageBox);
                            if (tempMsg[1].Equals("n"))
                            {
                                Invoke(informationMsg, "등록되지 않은 사용자 입니다.");
                            }
                            else
                                Invoke(informationMsg, "비밀번호가 틀렸습니다.");

                            Loginfail fail = new Loginfail(logInFail);
                            Invoke(fail, null);
                        }
                        catch (Exception e) { };
                        break;

                    case "g": //로그인 성공시 (g|name|team|company|com_cd)
                        
                        connected = true;

                        stringDele changeProgressStyle = new stringDele(chageProgressbar);
                        Invoke(changeProgressStyle, "로딩중");

                        //MiniCTI config 파일 설정
                        FileInfo tempfileinfo = new FileInfo(XmlConfigFullPath);//"C:\\MiniCTI\\config\\MiniCTI_config_demo.xml");
                        if (!tempfileinfo.Exists)
                        {
                            logWrite("MiniCTI config 파일 없음");
                            FileInfo temp = new FileInfo(XmlConfigOrgFullPath);//Application.StartupPath + "\\MiniCTI_config_demo.xml");
                            temp.CopyTo(tempfileinfo.FullName);
                        }
                        setCRM_DB_HOST(XmlConfigFullPath, serverIP);//"c:\\MiniCTI\\config\\MiniCTI_config_demo.xml", serverIP);

                        //서버측에서 전달된 이름 저장
                        myname = tempMsg[1];
                        myid = this.id.Text;
                        com_cd = tempMsg[4];
                        logWrite("로그인 성공! (" + DateTime.Now.ToString() + ")");

                       
                        //개인 정보를 Client_Form 에 표시
                        FormTextCtrlDelegate FlushName = new FormTextCtrlDelegate(FlushInfo);
                        FormTextCtrlDelegate FlushTeam = new FormTextCtrlDelegate(Flushteam);
                        
                        Invoke(FlushName, tempMsg[1]);

                        if (tempMsg[2].Length > 0) {
                            Invoke(FlushTeam, tempMsg[2]);
                        } else {
                            Invoke(FlushTeam, tempMsg[3]);
                        }

                        //화면의 모든 콘트롤에 keydown이벤트설정
                        this.KeyDown += new KeyEventHandler(Client_Form_KeyDown);
                        int count = this.Controls.Count;

                        for (int i = 0; i < count; i++) {
                            this.Controls[i].KeyDown += new KeyEventHandler(Client_Form_KeyDown);
                        }

                        //쪽지 및 대화 저장 폴더, 파일 생성
                        MemoUtils.MemoFileCheck(this.myid);
                        DialogFileCheck();
                        FileDirCheck();

                        if (checkThread == null) {
                            checkThread = new Thread(new ThreadStart(SendCheck));
                            checkThread.Start();
                            logWrite("SendCheck 스레드 시작");
                        }

                        break;


                    case "M": //다른 클라이언트 목록 및 접속상태 정보(M|팀이름|id!멤버이름|id!멤버이름)


                        if (tempMsg[1].Equals("e")) { //모든 팀트리 정보 전송완료 메시지일 경우 -> Client_Form을 로그인 상태로 하위 구성요소를 활성화 한다.
                        
                            PanelCtrlDelegate loginPanel = new PanelCtrlDelegate(LogInPanelVisible); //로그인 패널 컨트롤 호출용
                            PanelCtrlDelegate TreeViewPanel = new PanelCtrlDelegate(TreeViewVisible);  //memTreeView 컨트롤호출용
                            PanelCtrlDelegate btnCtrl = new PanelCtrlDelegate(ButtonCtrl);//각종 버튼 컨트롤호출용
                            
                            Invoke(TreeViewPanel, true);
                            
                            // 버튼 활성화
                            Invoke(btnCtrl, true);

                            //tooltip 설정
                            tooltip.AutoPopDelay = 0;
                            tooltip.AutomaticDelay = 2000;
                            tooltip.InitialDelay = 100;

                            Invoke(loginPanel, false);

                            //crm 프로그램 실행
                            Thread thread = new Thread(new ThreadStart(SetUserInfo));
                            thread.Start();
                            //NoParamDele dele = new NoParamDele(SetUserInfo);
                            //Invoke(dele);
                        } else { // 팀트리 정보를 수신한 경우
                        
                            ArrayList list = new ArrayList();
                            if (tempMsg.Length > 2) {
                                string teamName = tempMsg[1];

                                for (int i = 2; i < tempMsg.Length; i++) {//배열 순서 2번째 부터인 id!name을 추출
                                
                                    if (tempMsg[i].Length != 0) {
                                    
                                        list.Add(tempMsg[i]);
                                        string[] memInfo = tempMsg[i].Split('!');  //<id>와 <name>을 분리하여 memInfo에 저장
                                        MemberInfoList[memInfo[0]] = memInfo[1];    //key=id, value=name
                                        logWrite("MemberInfoList[" + memInfo[0] + "] = " + memInfo[1]);

                                        //다른 상담원의 <소속>과 <id>를 hashtable에 저장(key=id, value=소속)
                                        if (teamName.Trim() == "") { //소속이 미지정인 경우
                                            teamName = MsgrMsg.UNDEFINED_TEAM;
                                        }
                                        TeamInfoList[memInfo[0]] = teamName;
                                        logWrite("TeamInfoList[" + memInfo[0] + "] = " + teamName);
                                        //logWrite("사용자 정보 등록 : 이름(" + memInfo[1] + ") 아이디(" + memInfo[0] + ")");
                                    }
                                }
                                ChildNodeDelegate AddMemNode = new ChildNodeDelegate(MakeMemTree);
                                object[] ar = { teamName, list };
                                Invoke(AddMemNode, ar);

                                treesource[teamName] = list;
                                memTree.Tag = treesource;
                                logWrite(teamName + " 팀 리스트 생성!");

                                //델리게이트 생성
                            }
                        }
                        break;

                    case "y":    //로그인 Client 리스트 y|id|상태값 
                        string team = (string)TeamInfoList[tempMsg[1]];

                        int Port = 8883;
                        logWrite("팀 : "+team+" 사용자 id : " + tempMsg[1] + "  port : " + Port.ToString());

                        //InList[tempMsg[1]] = server;   //로그인 리스트 테이블에 저장(key=id, value=IPEndPoint)

                        ChangeStat changelogin = new ChangeStat(ChangeMemStat);
                        Invoke(changelogin, new object[] { tempMsg[1], tempMsg[2] });

                        break;

                    case "IP":    //로그인 Client 리스트 IP|id|ip주소 

                        Port = 8883;
                        logWrite(" 사용자 id : " + tempMsg[1] + "IP 주소 : " + tempMsg[2] + "  port : " + Port.ToString());

                        InList[tempMsg[1]] = new IPEndPoint(IPAddress.Parse(tempMsg[2]), Port);   //로그인 리스트 테이블에 저장(key=id, value=IPEndPoint)

                        break;

                    case "a":  //중복로그인 시도를 알려줌
                        Loginfail relogin = new Loginfail(ReLogin);
                        Invoke(relogin, null);

                        break;

                    case "u": //서버측에서 강제 로그아웃 메시지 수신한 경우
                        LogOutDelegate logoutDel = new LogOutDelegate(LogOut);
                        Invoke(logoutDel, null);
                        break;

                    case "d":  //상대방 대화메시지일 경우 (d|Formkey|id/id/...|name|메시지)
                        tempFormKey = ChatUtils.GetFormKey(tempMsg[1], myid);

                        string[] ids = tempMsg[2].Split('/');
                        if (!ids[0].Equals(myid)) {
                        
                            if (ChatFormList.Contains((string)tempFormKey) && ChatFormList[(string)tempFormKey] != null) { //이미 발신자와 채팅중일 경우
                            
                                ChatForm form = (ChatForm)ChatFormList[(string)tempFormKey];
                                AddChatMsgDelegate addMsg = new AddChatMsgDelegate(form.PostUserMessage);
                                Invoke(addMsg, new object[] { ids[0], tempMsg[3], tempMsg[4] });//id/id... 첫번째가 문자올린 대화자
                                //if() 사용자 상태에 따라
                                //form.Activate();
                            }
                            else {  //새로운 대화요청일 경우, 대화창 생성
                                ArrangeCtrlDelegate OpenChatForm = new ArrangeCtrlDelegate(NewChatForm);
                                Invoke(OpenChatForm, new object[] { tempMsg });
                            }
                        }
                        break;

                    case "c":  //c|formkey|id/id/..|name  //대화중 초대가 일어난 경우
                        {
                            tempFormKey = ChatUtils.GetFormKey(tempMsg[1], myid);

                            ChatForm form = (ChatForm)ChatFormList[(string)tempFormKey];

                            if (form != null) {

                                string[] addedIdArray = tempMsg[2].Split('/');
                                AddChatterDelegate addChatter = new AddChatterDelegate(form.AddChatterToNode);

                                foreach (string addedId in addedIdArray) {
                                    UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, addedId);
                                    Invoke(addChatter, new object[] { userObj });
                                }

                                string newFormKey = ChatUtils.GetFormKeyWithMultiUsersAdded(tempFormKey, myid, tempMsg[2]);
                                SetChatFormKeyUpdate(form, newFormKey, tempFormKey);
                            
                            } else {
                                logWrite(string.Format("'c' key[{0}]를 갖는 채팅창이 존재하지 않음.", tempFormKey));
                            }
                        }
                        break;

                    case "C":  //보낸공지 읽음확인 수신(C|확인자id|noticeid)
                        ArrangeCtrlDelegate NoticeDetailResultFormAdd = new ArrangeCtrlDelegate(NoticeReaderAdd);
                        Invoke(NoticeDetailResultFormAdd, new object[] { tempMsg });
                        break;

                    case "q": //다자 대화중 상대방이 대화창 나감 (q|Formkey|id)
                        {
                            tempFormKey = ChatUtils.GetFormKey(tempMsg[1], myid);

                            ChatForm form = (ChatForm)ChatFormList[(string)tempFormKey];
                            if (form != null)
                            {
                                //노드삭제
                                DelChatterDelegate delchatter = new DelChatterDelegate(form.DeleteChatter);
                                Invoke(delchatter, new object[] { tempMsg[2], GetUserName(tempMsg[2]) });
                                //formkey 변경
                                string newFormKey = ChatUtils.GetFormKeyWithUserQuit(tempFormKey, myid, tempMsg[2]);
                                SetChatFormKeyUpdate(form, newFormKey, tempFormKey);
                            }
                            else
                            {
                                logWrite(string.Format("'q' key[{0}]를 갖는 채팅창이 존재하지 않음.", tempFormKey));
                            }
                        }
                        break;

                    case "dc": //다자 대화중 상대방이 연결 끊김 (dc|Formkey|id)
                        {
                            tempFormKey = ChatUtils.GetFormKey(tempMsg[1], myid);

                            ChatForm form = (ChatForm)ChatFormList[(string)tempFormKey];
                            if (form != null)
                            {
                                //노드삭제
                                DelChatterDelegate delchatter = new DelChatterDelegate(form.DeleteChatter);
                                Invoke(delchatter, new object[] { tempMsg[2], GetUserName(tempMsg[2]) });
                                //formkey 변경
                                string newFormKey = ChatUtils.GetFormKeyWithUserQuit(tempFormKey, myid, tempMsg[2]);
                                SetChatFormKeyUpdate(form, newFormKey, tempFormKey);
                            }
                            else
                            {
                                logWrite(string.Format("'dc' key[{0}]를 갖는 채팅창이 존재하지 않음.", tempFormKey));
                            }
                        }
                        break;

                    case "m"://메모를 수신한 경우 m|name|id|message|수신자id
                        ArrangeCtrlDelegate memo = new ArrangeCtrlDelegate(MakeMemo);
                        Invoke(memo, new object[] { tempMsg });
                        MemoUtils.MemoFileWrite(this.myid, msg + "|" + DateTime.Now.ToString());
                        break;
#endregion //일반메신저기능

                    case "F":  //파일받기전 파일 정보 수신     F|파일명|파일크기|파일key|전송자id
                        string name = GetUserName(tempMsg[4]);
                        string[] filenameArray = tempMsg[1].Split('\\'); //파일명 추출
                        int filesize = int.Parse(tempMsg[2]);            //파일크기 캐스팅
                        IPEndPoint siep = (IPEndPoint)InList[tempMsg[4]];
                        siep.Port = 8883;
                        string savefilename = "C:\\MiniCTI\\" + this.myid + "\\Files\\" + filenameArray[filenameArray.Length - 1];

                        //수락여부 확인 플로우로 넘김
                        //1.수락
                        //      FTP_ShowDialog(Hashtable info);
                        //                 form.ShowDialog(Hashtable info);
                        //          event FTP_DialogResultReceived(CustomEvent);
                        //2-1.거부
                        //2-2.종료
                        //          event FTP_OnStatusChanged
                        //                  
                        //2.수락메시지전송
                        //      
                        //3.수신대기 
                        //      FileReceiver
                        //4.수신
                        //      FTP_ShowStatus(Receive|Cancel|Done|Error)
                        //              form.ShowStatus(Hashtable info);
                        //5.완료
                        //      FTP_StopReceiving()
                        //6.취소
                        //      FTP_CancelReceiving()
                        //7.실패
                        //      
                        if (InList.ContainsKey(tempMsg[4]) && InList[tempMsg[4]] != null) //전송대상자가 로그인 상태인 경우
                        {
                            //Hashtable info = new Hashtable();
                            //info["filename"] = savefilename;
                            //info["filesize"] = filesize;
                            //info["name"] = name;
                            //info["senderid"] = tempMsg[4];
                            //info["myid"] = myid;
                            DownloadInfo info = new DownloadInfo();
                            info.FileName = savefilename;
                            info.FileSize = filesize;
                            info.SenderName = name;
                            info.SenderId = tempMsg[4];
                            info.MyId = myid;
                            info.MsgFileKey = tempMsg[3];
                            info.SenderFileName = tempMsg[1];
                            if (mDownloadPath != null && mDownloadPath.Trim() != "")
                                info.DownloadPath = mDownloadPath;
                            else
                                info.DownloadPath = string.Format(@"C:\MiniCTI\{0}\Files", myid);
                            //FTP_ShowDialg(info);
                            FTP_ShowDialogDelegate delShow = new FTP_ShowDialogDelegate(FTP_ShowDialg);
                            Invoke(delShow, new object[] { info });

                        }
                        break;
                        #region 파일로직변경으로 막음
                        //if (InList.ContainsKey(tempMsg[4]) && InList[tempMsg[4]] != null) //전송대상자가 로그인 상태인 경우
                        //{
                        //    Hashtable info = new Hashtable();
                        //    info[savefilename] = filesize;
                        //    info["name"] = name;
                        //    info["senderid"] = tempMsg[4];

                        //    //파일받기 수락 메시지전송
                        //    SendMsg("Y|" + tempMsg[1] + "|" + tempMsg[3] + "|" + this.myid, siep); //구성(Y|파일명|파일Key|수신자id)

                        //    //파일수신대기/수신
                        //    FileReceiver((object)info);
                        //}
                        
                        //downloadform.Show();
                        
                        //break;
                        #endregion
                    case "Y"://파일 받기 수락 메시지(Y|파일명|파일key|수신자id)
                        ShowFileSendDetailDelegate show = new ShowFileSendDetailDelegate(ShowFileSendDetail);
                        Hashtable table = new Hashtable();
                        table[tempMsg] = (IPEndPoint)InList[tempMsg[3]];

                        //파일 전송 스레드 시작
                        Thread sendfile = new Thread(new ParameterizedThreadStart(SendFile));
                        sendfile.Start((object)table);

                        FileSendThreadList[tempMsg[2] + "|" + tempMsg[3]] = sendfile;

                        FileSendDetailListView view = (FileSendDetailListView)FileSendDetailList[tempMsg[2]];
                        Invoke(show, new object[] { tempMsg[3], "전송대기중", view });

                        break;

                    case "FS"://파일 받기 수락 메시지(FS|파일명|파일key|수신자id)
                        ShowFileSendDetailDelegate show_ = new ShowFileSendDetailDelegate(ShowFileSendDetail);
                        Hashtable table_ = new Hashtable();
                        table_[tempMsg] = server;

                        //파일 전송 스레드 시작
                        Thread sendfile_ = new Thread(new ParameterizedThreadStart(SendFile));
                        sendfile_.Start((object)table_);

                        FileSendThreadList[tempMsg[2] + "|" + tempMsg[3]] = sendfile_;

                        FileSendDetailListView view_ = (FileSendDetailListView)FileSendDetailList[tempMsg[2]];
                        Invoke(show_, new object[] { tempMsg[3], "전송대기중", view_ });

                        break;

                    case "N": //파일 받기 거부("N|파일명|파일키|id)

                        ShowFileSendDetailDelegate detail = new ShowFileSendDetailDelegate(ShowFileSendDetail);

                        FileSendDetailListView View = (FileSendDetailListView)FileSendDetailList[tempMsg[2]];

                        Invoke(detail, new object[] { tempMsg[3], "받기 거부", View });

                        string Name = GetUserName(tempMsg[3]);
                        string[] FilenameArray = tempMsg[1].Split('\\');
                        MessageBox.Show(Name + " 님이 파일 받기를 거부하셨습니다.\r\n\r\n파일명 : " + FilenameArray[(FilenameArray.Length - 1)], "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;
                    #region //일반 메신저기능
                    case "i":  //추가 로그인 상담원일 경우  형태 : i|id|소속팀명|ip|이름

                        try {
                            //1. 로그인 리스트 테이블에 추가
                            IPEndPoint addiep = new IPEndPoint(IPAddress.Parse(tempMsg[3]), 8883);
                            InList[tempMsg[1]] = addiep;

                            //2. memTree 뷰에 로그인 사용자 상태 변경
                            ChangeStat change = new ChangeStat(ChangeMemStat);
                            Invoke(change, new object[] { tempMsg[1], "online" });
                            TeamInfoList[tempMsg[1]] = tempMsg[2];
                            
                            //3. 각 채팅창 key변경 및 채팅창 노드/상태변경
                            ChangeStat logInChatter = new ChangeStat(SetUserStatusInChatForm);
                            Invoke(logInChatter, new object[] { tempMsg[1], MsgrUserStatus.ONLINE });

                            //4. 로그인 했음 메시지 창 띄움
                            // 추후 구현
                        } catch (Exception e) {
                            logWrite(e.ToString());
                        }
                        break;

                    case "o":  //로그아웃 상담원이 발생할 경우  o|id|소속
                        try {
                            //1. 로그인 리스트 테이블에서 삭제
                            lock (this) {
                                InList[tempMsg[1]] = null;
                            }
                            //2. memTree 뷰에 로그아웃 사용자 상태 변경
                            ChangeStat logout = new ChangeStat(ChangeMemStat);
                            Invoke(logout, new object[] { tempMsg[1], tempMsg[2] });

                            //3. 각 채팅창 key변경 및 채팅창 노드/상태변경
                            ChangeStat logOutChatter = new ChangeStat(SetUserStatusInChatForm);
                            Invoke(logOutChatter, new object[] {tempMsg[1], MsgrUserStatus.LOGOUT});
                        } catch (Exception e) {
                            logWrite(e.ToString());
                        }
                        break;

                    case "n":  //공지사항 메시지 (n|메시지|발신자id|mode|noticetime|제목)

                        logWrite("공지사항 수신!");
                        if (!tempMsg[2].Equals(this.myid)) //자기가 보낸 공지일 경우 보이지 않음
                        {
                            ArrangeCtrlDelegate notice = new ArrangeCtrlDelegate(ShowNoticeDirect);
                            Invoke(notice, new object[] { tempMsg });
                        }
                      
                        break;

                    case "A": //부재중 정보 전달(A|mnum|fnum|nnum|tnum)
                        ArrangeCtrlDelegate ShowAbsentInfo = new ArrangeCtrlDelegate(ShowAbsentInfoNumber);
                        Invoke(ShowAbsentInfo, new object[] { tempMsg });
                        
                        break;

                    case "Q"://안읽은 메모 리스트 (Q|sender;content;time;seqnum|sender;content;time;seqnum|...
                        ArrangeCtrlDelegate ShowNRmemoList = new ArrangeCtrlDelegate(ShowMemoList);
                        Invoke(ShowNRmemoList, new object[] { tempMsg });

                        break;

                    case "T"://안읽은 공지 리스트 (T|sender;content;time;mode;seqnum|sender;content;time;mode;seqnum|...
                        ArrangeCtrlDelegate ShowNRnoticeList = new ArrangeCtrlDelegate(ShowNotReadNoticeList);
                        Invoke(ShowNRnoticeList, new object[] { tempMsg });

                        break;

                    case "R"://안읽은 파일 리스트 (R|보낸사람;filenum;filename;time;size;seqnum|보낸사람;filenum;filename;time;size;seqnum|...
                        ArrangeCtrlDelegate ShowNRFileList = new ArrangeCtrlDelegate(ShowFileList);
                        Invoke(ShowNRFileList, new object[] { tempMsg });

                        break;

                    case "t": //"t|ntime†content†nmode†title†안읽은사람1:안읽은사람2:...|...
                        
                        ArrangeCtrlDelegate ShowNoticeResultFromDB = new ArrangeCtrlDelegate(showNoticeResultFromDB);
                        Invoke(ShowNoticeResultFromDB, new object[] { tempMsg });

                        break;

                    case "L"://공지사항 리스트 수신한 경우  L|time!content!mode!sender!seqnum
                        if (!(tempMsg.Length < 2))
                        {
                            ArrangeCtrlDelegate makeNoticeList = new ArrangeCtrlDelegate(MakeNoticeListForm);
                            Invoke(makeNoticeList, new object[] { tempMsg });
                        }
                        else
                        {
                            MessageBox.Show("등록된 공지가 없습니다.", "공지없음", MessageBoxButtons.OK);
                        }
                        break;
                            #endregion //일반 메신저 기능
                    case "s"://각 클라이언트 상태값 변경 메시지 s|id|상태|IPAddress
                        {
                            if (!tempMsg[1].Equals(this.myid))
                            {
                                //1. 트리상태변경 
                                ChangeStat presenceupdate = new ChangeStat(PresenceUpdate);
                                Invoke(presenceupdate, new object[] { tempMsg[1], tempMsg[2] });
                                //2. 로그인리스트 테이블에 추가
                                IPEndPoint tempiep = new IPEndPoint(IPAddress.Parse(tempMsg[3]), listenport);
                                lock (InList)
                                {
                                    InList[tempMsg[1]] = tempiep;
                                }
                                //3. 채팅창 상태변경
                                //   각 채팅창 key변경 및 채팅창 노드/상태변경
                                ChangeStat changeStat = new ChangeStat(SetUserStatusInChatForm);
                                Invoke(changeStat, new object[] { tempMsg[1], tempMsg[2] });
                            }
                        }
                        break;
                    #region //전화기능
                    case "Ring": //발신자 표시(Ring|ani|name|server_type)

                        RingingDele ringdele = new RingingDele(Ringing);
                        Invoke(ringdele, new object[] { tempMsg[1], tempMsg[2], tempMsg[3] });

                        break;

                    case "Dial": //다이얼시 고객정보 팝업(Dial|ani)
                        if (this.nopop_outbound == false)
                        {
                            doublestringDele dialdele = new doublestringDele(Answer);
                            Invoke(dialdele, new object[] { tempMsg[1], "2" });
                        }
                        break;

                    case "Answer": //offhook시 고객정보 팝업(Answer|ani|type)
                        doublestringDele answerdele = new doublestringDele(Answer);
                        Invoke(answerdele, new object[] { tempMsg[1], tempMsg[2] });
                        
                        break;

                    case "Abandon": //Abandon 발생시
                        AbandonDele abandon = new AbandonDele(Abandoned);
                        Invoke(abandon);
                        break;

                    case "Other": //다른사람이 응답시
                        AbandonDele other = new AbandonDele(OtherAnswer);
                        Invoke(other);
                        break;
                    #endregion //전화기능
                    case "pass"://고객정보 전달 수신(pass|ani|senderID|receiverID
                        if (tempMsg[2].Equals(this.myid))
                        {
                            //MessageBox.Show("이관되었습니다.");
                        }
                        else
                        {
                            //notifyTransfer(tempMsg);
                            ArrangeCtrlDelegate passdele = new ArrangeCtrlDelegate(notifyTransfer);
                            Invoke(passdele, new object[]{tempMsg});
                        }
                        break;

                    case "trans"://부재중 이관 메시시 수신(trans|sender†content†time†seqnum|...)
                        ArrangeCtrlDelegate ShowNRTransList = new ArrangeCtrlDelegate(showNoreadTransfer);
                        Invoke(ShowNRTransList, new object[] { tempMsg });
                        break;

                }
            }
            catch (Exception exception)
            {
                logWrite(exception.StackTrace);
            }
        }

        private void Client_Form_onLoginEvent()
        {
            SetUserInfo();
        }

        private void notifyTransfer(string[] tempMsg)//pass|ani|senderID|receiverID|TONG_DATE|TONG_TIME|CustomerName
        {
            try
            {
                notifyform = new NotifyForm();
                notifyform.button1.MouseClick += new MouseEventHandler(NotifyForm_Confirm_MouseClick);
                notifyform.Tag = tempMsg[1];
                notifyform.label_TONGDATE.Text = tempMsg[4];
                notifyform.label_TONGTIME.Text = tempMsg[5];
                notifyform.label_ani.Text = tempMsg[1];
                notifyform.label_senderid.Text = tempMsg[2];
                if (tempMsg.Length > 6)
                {
                    if (tempMsg[6].Length > 0)
                    {
                        notifyform.label_Customer.Text = tempMsg[6];
                    }
                    else
                    {
                        notifyform.label_Customer.Text = tempMsg[1];
                    }

                    string senderName = GetUserName(tempMsg[2]);
                    notifyform.label_sender.Text = "from " + senderName + "(" + tempMsg[2] + ")";
                }

                notifyform.Focus();
                notifyform.Show();
                timerForNotify.Start();
                
            }
            catch (Exception ex)
            {
                
            }
        }

        private void timerForNotify_Tick(object sender, EventArgs e)
        {
            try
            {
                if (notifyform != null)
                {
                    timerForNotify.Stop();
                    int height_point = 0;

                    if (TransferNotiArea.Count > 0)
                    {
                        foreach (DictionaryEntry de in TransferNotiArea)
                        {
                            if (de.Value.ToString().Equals("0"))
                            {
                                int temp = Convert.ToInt32(de.Key.ToString());
                                if (temp > height_point)
                                {
                                    height_point = temp;
                                }
                            }
                            else
                            {
                                logWrite("TransferNotiArea[" + de.Key.ToString() + "] = " + de.Value.ToString());
                                logWrite(de.Key.ToString() + " is not 0");
                            }
                        }

                        if (height_point == 0)
                        {
                            //가장 오래된 태그폼 삭제
                            NoParamDele dele = new NoParamDele(closeNoticeForm);
                            Invoke(dele);

                            foreach (DictionaryEntry de in TransferNotiArea)
                            {
                                if (de.Value.ToString().Equals("0"))
                                {
                                    int temp = Convert.ToInt32(de.Key.ToString());
                                    if (temp > height_point)
                                    {
                                        height_point = temp;
                                    }
                                }
                                else
                                {
                                    logWrite("TransferNotiArea[" + de.Key.ToString() + "] = " + de.Value.ToString());
                                    logWrite(de.Key.ToString() + " is not 0");
                                }
                            }
                        }
                    }
                    TransferNotiForm miniform = new TransferNotiForm();
                    miniform.pbx_icon.Image = global::Client.Properties.Resources.img_customer;
                    miniform.MouseClick += new MouseEventHandler(miniform_MouseClick);
                    miniform.pbx_icon.MouseClick+=new MouseEventHandler(pbx_icon_MouseClick_for_Transfer);
                    miniform.label_Customer.MouseClick += new MouseEventHandler(label_Customer_MouseClick);
                    miniform.label_from.MouseClick += new MouseEventHandler(label_Customer_MouseClick);
                    miniform.label_Customer.Text = notifyform.label_Customer.Text;
                    miniform.label_from.Text = notifyform.label_sender.Text;
                    miniform.label_ani.Text = notifyform.label_ani.Text;
                    miniform.label_date.Text = notifyform.label_TONGDATE.Text;
                    miniform.label_time.Text = notifyform.label_TONGTIME.Text;
                    miniform.label_senderid.Text = notifyform.label_senderid.Text;
                    screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                    screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                    miniform.SetBounds(screenWidth - miniform.Width, height_point, miniform.Width, miniform.Height);
                    notifyform.Close();
                    notifyform = null;
                    miniform.TopLevel = true;
                    miniform.Show();
                    TransferNotiArea[height_point.ToString()] = "1";
                    NotiFormList.Add(miniform);
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void closeNoticeForm()
        {
            try
            {
                logWrite("closeNoticeForm 시작");
                TransferNotiForm miniform = (TransferNotiForm)NotiFormList[0];
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }
                miniform.Close();
                NotiFormList.RemoveAt(0);
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }


        private void pbx_icon_MouseClick_for_Transfer(object sender, MouseEventArgs e)
        {
            try
            {

                PictureBox label = (PictureBox)sender;

                TransferNotiForm miniform = (TransferNotiForm)label.Parent;

                ShowTransInfoDele dele = new ShowTransInfoDele(showTransferInfo);

                Invoke(dele, new object[] { miniform.label_ani.Text, miniform.label_senderid.Text, miniform.label_date.Text, miniform.label_time.Text });
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }

                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void label_Customer_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                Label label = (Label)sender;

                TransferNotiForm miniform = (TransferNotiForm)label.Parent;

                ShowTransInfoDele dele = new ShowTransInfoDele(showTransferInfo);

                Invoke(dele, new object[] { miniform.label_ani.Text, miniform.label_senderid.Text, miniform.label_date.Text, miniform.label_time.Text });
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }
                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }

        }

        /// <summary>
        /// 수신전화 태그폼 클릭시 고객정보창 팝업 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Customer_MouseClick_for_Call(object sender, MouseEventArgs e)
        {
            try
            {

                Label label = (Label)sender;

                TransferNotiForm miniform = (TransferNotiForm)label.Parent;

                string ani = "";
                string temp = miniform.label_Customer.Text;
                string[] tempArr = temp.Split('(');
                if (tempArr.Length > 1)
                {
                    ani = tempArr[1].Split(')')[0];
                }
                else
                {
                    ani = temp;
                }

                doublestringDele dele = new doublestringDele(showCustomerPopup);
                Invoke(dele, new object[] { ani, "1" });

                logWrite("miniform.Top = " + miniform.Top.ToString());
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }

                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }

        }


        private void miniform_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TransferNotiForm miniform = (TransferNotiForm)sender;

                ShowTransInfoDele dele = new ShowTransInfoDele(showTransferInfo);

                Invoke(dele, new object[] { miniform.label_ani.Text, miniform.label_senderid.Text, miniform.label_date.Text, miniform.label_time.Text });
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }

                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
           
        }

        /// <summary>
        /// 수신전화 태그폼 클릭시 고객정보 화면 팝업처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniform_MouseClick_for_Call(object sender, MouseEventArgs e)
        {
            try
            {
                TransferNotiForm miniform = (TransferNotiForm)sender;
                string ani = "";
                string temp = miniform.label_Customer.Text;
                string[] tempArr = temp.Split('(');
                if (tempArr.Length > 1)
                {
                    ani = tempArr[1].Split(')')[0];
                }
                else
                {
                    ani = temp;
                }
                doublestringDele dele = new doublestringDele(showCustomerPopup);
                Invoke(dele, new object[]{ani, "1"});

                logWrite("miniform.Top = " + miniform.Top.ToString());
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }

                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }

        }

        private void showTransferInfo(string ani, string senderid , string tong_date, string tong_time)
        {
            logWrite("showTransferInfo(" + ani + ", " + senderid + ", " + tong_date + ", " + tong_time);
            try
            {
                crm_main.OpenCustomerPopupTransfer(ani, senderid, tong_date, tong_time, "3");
                CRMUtils.DisplayCrmPopUp(crm_main);
            }
            catch (System.ObjectDisposedException dis)
            {
                try
                {
                    cm.SetUserInfo(this.com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);
                    crm_main = new FRM_MAIN();
                    crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                    CRMUtils.DisplayCrmPopUp(crm_main);
                }
                catch (Exception ex1)
                {
                    logWrite(ex1.ToString());
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void NotifyForm_Confirm_MouseClick(object sender, MouseEventArgs e)
        {
            if (notifyform != null)
            {
                timerForNotify.Stop();
                ShowTransInfoDele dele = new ShowTransInfoDele(showTransferInfo);

                Invoke(dele, new object[] { notifyform.label_ani.Text, notifyform.label_senderid.Text, notifyform.label_TONGDATE.Text, notifyform.label_TONGTIME.Text });
                notifyform.Close();
            
            }
        }

        private void makeSelectTransferForm(string ani)
        {
            try
            {
                selecttransferform = new SelectTransferForm();
                selecttransferform.Tag = ani;
                selecttransferform.pbx_trans_confirm.MouseClick += new MouseEventHandler(pbx_trans_confirm_MouseClick);
                foreach (DictionaryEntry de in InList)
                {
                    if (!de.Key.ToString().Equals(this.myid))
                    {
                        if (de.Value != null)
                        {
                            string tempName = GetUserName(de.Key.ToString());
                            selecttransferform.cbx_receiver.Items.Add(tempName + "(" + de.Key.ToString() + ")");
                        }
                    }
                }
                selecttransferform.Show();
                selecttransferform.cbx_receiver.DroppedDown = true;
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void pbx_trans_confirm_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string itemstring = selecttransferform.cbx_receiver.SelectedItem.ToString();
                string[] tempArr = itemstring.Split('(');
                string[] tempArr1 = tempArr[1].Split(')');
                string receiverID = tempArr1[0];
                string ani = selecttransferform.Tag.ToString();
                selecttransferform.Close();
                string tempmsg = MsgrMsg.REQ_UNREAD_TRANSFERED + ani + "|" + receiverID;
                SendMsg(tempmsg, server);
                MessageBox.Show("고객정보 전달 완료!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void chageProgressbar(string status)
        {
            label_progress_status.Text = status;
        }

        private void SetUserInfo()
        {
            logWrite("SetUserInfo 실행");
            logWrite("com_cd = " + this.com_cd);
            logWrite("myid = " + this.myid);
            logWrite("pass = " + tbx_pass.Text);
            
            cm.SetUserInfo(this.com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);

            //NoParamDele dele = new NoParamDele(startCRMmanager);
            //Invoke(dele);
        }

        private void startCRMmanager()
        {
            try
            {
                if (crm_main == null) {
                    crm_main = new FRM_MAIN();
                    crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                }
                CRMUtils.DisplayCrmPopUp(crm_main);
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

       
        /// <summary>
        /// 발신자 표시 처리
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="name"></param>
        /// <param name="server_type"></param>

        private void Ringing(string ani, string name, string server_type)
        {
            try
            {
                logWrite("Ringing : ani[" + ani + "]name[" + name + "]server_type[" + server_type + "]nopop[" + nopop + "]");

                CustomerList[ani] = name;
                if (popform != null)
                {
                    t1.Stop();
                    popform.Close();
                }
                //getForegroundWindow();
                popform = new PopForm();
                popform.Tag = name;
                if (name.Length > 0)
                {
                    popform.label1.Text = name + "\r\n" + ani;
                }
                else
                {
                    popform.label1.Text = ani;
                }

                if (isHide == false && firstCall == false)
                {
                    popform.TopMost = true;
                    firstCall = true;
                }
                else
                {
                    popform.TopMost = false;
                }
                this.TopLevel = true;
                popform.TopLevel = true;
                popform.Show();
                //getForegroundWindow();
                
                t1.Start();

            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }


        private void Answer(string ani, string calltype)
        {
            logWrite("Answer : ani[" + ani + "]calltype[" + calltype + "]nopop[" + nopop + "]nopop_outbound[" + nopop_outbound + "]");
            if (popform != null)
            {
                t1.Stop();
                if (nopop == true)
                {
                    string name = popform.Tag.ToString();
                    showAnswerCallInfo(ani, name);
                }
                popform.Close();
                
            }
            //cm.POPUP(ani, DateTime.Now.ToString("yyyyMMddhhmmss"), "1");
            //cm.SetUserInfo(this.com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);

            if (nopop == false)
            {
                try
                {
                    crm_main.OpenCustomerPopup(ani, DateTime.Now.ToString("yyyyMMddHHmmss"), calltype);

                    CRMUtils.DisplayCrmPopUp(crm_main);
                }
                catch (System.ObjectDisposedException dis)
                {
                    logWrite("에러발생:"+dis.ToString());

                    //getForegroundWindow();
                    cm.SetUserInfo(com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);
                    crm_main = new FRM_MAIN();
                    crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                    crm_main.OpenCustomerPopup(ani, DateTime.Now.ToString("yyyyMMddHHmmss"), calltype);
                    CRMUtils.DisplayCrmPopUp(crm_main);
                }
            }
        }


        private void showCustomerPopup(string ani, string calltype)
        {
            try
            {
                //getForegroundWindow();
                crm_main.OpenCustomerPopup(ani, DateTime.Now.ToString("yyyyMMddHHmmss"), calltype);
                CRMUtils.DisplayCrmPopUp(crm_main);
            }
            catch (System.ObjectDisposedException dis)
            {
                //getForegroundWindow();
                cm.SetUserInfo(com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);
                crm_main = new FRM_MAIN();
                crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                crm_main.OpenCustomerPopup(ani, DateTime.Now.ToString("yyyyMMddHHmmss"), calltype);
                CRMUtils.DisplayCrmPopUp(crm_main);
            }
        }

        private void showAnswerCallInfo(string ANI, string name)
        {
            try
            {

                int height_point = 0;

                if (TransferNotiArea.Count > 0)
                {
                    foreach (DictionaryEntry de in TransferNotiArea)
                    {
                        if (de.Value.ToString().Equals("0"))
                        {
                            int temp = Convert.ToInt32(de.Key.ToString());
                            if (temp > height_point)
                            {
                                height_point = temp;
                            }
                        }
                        else
                        {
                            logWrite("TransferNotiArea[" + de.Key.ToString() + "] = " + de.Value.ToString());
                            logWrite(de.Key.ToString() + " is not 0");
                        }
                    }

                    if (height_point == 0)
                    {
                        //가장 오래된 태그폼 삭제
                        NoParamDele dele = new NoParamDele(closeNoticeForm);
                        Invoke(dele);

                        foreach (DictionaryEntry de in TransferNotiArea)
                        {
                            if (de.Value.ToString().Equals("0"))
                            {
                                int temp = Convert.ToInt32(de.Key.ToString());
                                if (temp > height_point)
                                {
                                    height_point = temp;
                                }
                            }
                            else
                            {
                                logWrite("TransferNotiArea[" + de.Key.ToString() + "] = " + de.Value.ToString());
                                logWrite(de.Key.ToString() + " is not 0");
                            }
                        }
                    }
                }

                TransferNotiForm miniform = new TransferNotiForm();
                miniform.TopMost = false;
                miniform.pbx_icon.Image = global::Client.Properties.Resources.phone_black;
                miniform.pbx_close.Visible = true;
                miniform.pbx_close.MouseClick += new MouseEventHandler(pbx_close_MouseClick);
                miniform.pbx_icon.MouseClick += new MouseEventHandler(pbx_icon_MouseClick);
                miniform.MouseClick += new MouseEventHandler(miniform_MouseClick_for_Call);
                miniform.label_Customer.MouseClick += new MouseEventHandler(label_Customer_MouseClick_for_Call);
                miniform.label_from.MouseClick += new MouseEventHandler(label_Customer_MouseClick_for_Call);
                if (name.Length > 1)
                {
                    miniform.label_Customer.Text = name + "(" + ANI + ")";
                }
                else
                {
                    miniform.label_Customer.Text = ANI;
                }
                //miniform.label_from.Text = notifyform.label_sender.Text;
                //miniform.label_ani.Text = notifyform.label_ani.Text;
                //miniform.label_date.Text = notifyform.label_TONGDATE.Text;
                //miniform.label_time.Text = notifyform.label_TONGTIME.Text;
                miniform.label_from.Text = "시간 : "+ DateTime.Now.ToShortTimeString();
                screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                miniform.SetBounds(screenWidth - miniform.Width, height_point, miniform.Width, miniform.Height);
                miniform.Show();
                TransferNotiArea[height_point.ToString()] = "1";
                NotiFormList.Add(miniform);
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void pbx_icon_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                PictureBox label = (PictureBox)sender;

                TransferNotiForm miniform = (TransferNotiForm)label.Parent;

                string ani = "";
                string temp = miniform.label_Customer.Text;
                string[] tempArr = temp.Split('(');
                if (tempArr.Length > 1)
                {
                    ani = tempArr[1].Split(')')[0];
                }
                else
                {
                    ani = temp;
                }

                doublestringDele dele = new doublestringDele(showCustomerPopup);
                Invoke(dele, new object[] { ani, "1" });

                logWrite("miniform.Top = " + miniform.Top.ToString());
                if (TransferNotiArea.ContainsKey(miniform.Top.ToString()))
                {
                    TransferNotiArea[miniform.Top.ToString()] = "0";
                }

                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        /// <summary>
        /// 인스턴트 수신목록 태크 닫기 버튼 처리 : 태그폼을 닫고 해당 TransferNotiArea를 비운다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbx_close_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBox pbx = (PictureBox)sender;
                TransferNotiForm miniform = (TransferNotiForm)pbx.Parent;
                logWrite("miniform.Top = " + miniform.Top.ToString());
                TransferNotiArea[miniform.Top.ToString()] = "0";
                foreach (TransferNotiForm form in NotiFormList)
                {
                    if (miniform.Equals(form))
                    {
                        logWrite("같은 TransferNotiForm 찾음!");
                        NotiFormList.Remove(form);
                        break;
                    }
                }
                miniform.Close();
                
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void crm_main_Activated(object sender, EventArgs e) {
            getForegroundWindow();
        }

        private void getForegroundWindow() {

            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            if (hwnd != null)
            {
                GetWindowThreadProcessId(hwnd, out pid);
                System.Diagnostics.Process CurProc = System.Diagnostics.Process.GetProcessById((int)pid);
                logWrite("GetForegroundWindow() : "+ CurProc.ProcessName);
                
            }
                
        }

        private void crm_main_FormClosing(object sender, FormClosingEventArgs e) {

            if (e.CloseReason == CloseReason.UserClosing) {

                e.Cancel = true;
                crm_main.Hide();
            }
           
        }

        private void Client_Form_onAnswerEvent(string ani)
        {
            //Answer(ani);
        }

        /// <summary>
        /// 인입호를 다른 클라이언트가 수신시 벨울림창 처리
        /// </summary>
        private void OtherAnswer()
        {
            if (popform != null)
            {
                t1.Stop();
                popform.Close();
            }
        }

        private void Abandoned()
        {
            if (popform != null)
            {
                t1.Stop();
                popform.Close();
               
            }
        }

        private void missedcallform_MouseClick(object sender, MouseEventArgs e)
        {
            if (missedcallform != null)
            {
                missedcallform.Close();
                missedcallform = null;
                missedCallCount = 0;
            }

            if (missedlistform != null)
            {
                
            }
        }

        /// <summary>
        /// 각종 단축키 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M && e.Modifiers == Keys.Control) //쪽지 보내기
            {
                MakeSendMemo(new Hashtable());
            }
            else if (e.KeyCode == Keys.N && e.Modifiers == Keys.Control) //공지하기
            {
                MakeSendNotice();
            }
            else if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control) //파일 보내기
            {
                MakeSendFileForm(new Hashtable());
            }
        }

        /// <summary>
        /// 각 클라이언트 상태 변경 처리
        /// </summary>
        /// <param name="statid"></param>
        /// <param name="presence"></param>
        private void PresenceUpdate(string statid, string presence)
        {
            try {

                if (TeamInfoList != null && TeamInfoList.ContainsKey(statid)) {

                    string tname = (string)TeamInfoList[statid];
                    TreeNode[] teamNode = memTree.Nodes.Find(tname, false);
                    TreeNode[] memNode = teamNode[0].Nodes.Find(statid, false);
                    UserObject userObj = (UserObject)memNode[0].Tag;
                    switch (presence) {
                        case MsgrUserStatus.BUSY://"busy":
                            memNode[0].ImageIndex = 6;
                            memNode[0].SelectedImageIndex = 6;
                            userObj.Status = MsgrUserStatus.BUSY;
                            break;

                        case MsgrUserStatus.AWAY://"away":
                            memNode[0].ImageIndex = 4;
                            memNode[0].SelectedImageIndex = 4;
                            userObj.Status = MsgrUserStatus.AWAY;
                            break;

                        case MsgrUserStatus.LOGOUT://"logout":
                            memNode[0].ForeColor = Color.Gray;
                            memNode[0].ImageIndex = 0;
                            memNode[0].SelectedImageIndex = 0;
                            InList[statid] = null;
                            userObj.Status = MsgrUserStatus.LOGOUT;
                            break;

                        case MsgrUserStatus.ONLINE://"online":
                            memNode[0].ForeColor = Color.Black;
                            memNode[0].ImageIndex = 1;
                            memNode[0].SelectedImageIndex = 1;
                            userObj.Status = MsgrUserStatus.ONLINE;
                            break;

                        case MsgrUserStatus.DND://"DND":
                            memNode[0].ImageIndex = 5;
                            memNode[0].SelectedImageIndex = 5;
                            userObj.Status = MsgrUserStatus.DND;
                            break;
                    }
                    memNode[0].Text = userObj.TitleName;
                    memNode[0].Tag = userObj;

                    logWrite(statid + "의 상태값" + presence + " 로 변경");
                }

            } catch (Exception e) {
                logWrite(id + " 상태값 변경 오류 : " + e.ToString());
            }
        }

        /// <summary>
        /// 1. 채팅창에 user상태 변경
        /// 2. login: user가 속한 채팅창의 user정보 활성화.
        /// 3. logout: 반대로 처리
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        private void SetChatFormKeyUpdate(ChatForm chatForm, string newFormKey, string oldFormKey) {
            lock (ChatFormList) {
                chatForm.SetFormKey(newFormKey);
                ChatFormList.Remove((string)oldFormKey);
                ChatFormList[(string)newFormKey] = chatForm;
            }
        }

        /// <summary>
        /// 공지사항 리스트 폼 생성
        /// </summary>
        /// <param name="tempMsg"></param>
        private void ShowNotice(string[] tempMsg) { //n|메시지 | 발신자id | mode | noticetime |seqnum| 제목
        
            try {
                if (tempMsg.Length > 6) {

                    string nname = GetUserName(tempMsg[2]);
                    Notice nform = new Notice();
                    if (tempMsg[0].Equals("r")) { //부재중 공지사항 리스트에서 수신
                        //nform.btn_confirm.MouseClick += new MouseEventHandler(btn_confirm_Click);
                    } else {//실시간 공지사항 수신시 확인결과 전송 처리
                        nform.btn_confirm.MouseClick += new MouseEventHandler(sendReadNotice);
                    }
                    nform.SetNoticeInfo(/*title*/tempMsg[6], /*content*/tempMsg[1], /*writerId*/ tempMsg[2], /*writerName*/ nname, /*tagKey*/ tempMsg[4]);
                    nform.Show();
                    nform.Activate();
                    if (!tempMsg[0].Equals("r")) {
                        nform.DoFlashWindow();
                    }
                } else {
                    logWrite("메시지 배열 크기 작음 : " + tempMsg.Length.ToString());
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void ShowNoticeDirect(string[] tempMsg) { //n|메시지 | 발신자id | mode | noticetime |제목
        
            try {
            
                if (tempMsg.Length > 5) {
                
                    string nname = GetUserName(tempMsg[2]);
                    Notice nform = new Notice();
                    
                    if (tempMsg[0].Equals("n")) { //실시간 공지사항 수신시 확인결과 전송 처리
                    
                        nform.NoticeAlreadyRead += sendReadNotice;
                    }
                    
                    nform.SetNoticeInfo(/*title*/tempMsg[5], /*content*/tempMsg[1], /*writerId*/ tempMsg[2], /*writerName*/ nname, /*tagKey*/ tempMsg[4]);
                    nform.Show();
                    nform.Activate();
                    
                    if (!tempMsg[0].Equals("r")) {
                        nform.DoFlashWindow();
                    }
                }
                else
                {
                    logWrite("메시지 배열 크기 작음 : " + tempMsg.Length.ToString());
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 공지사항의 "확인" 버튼을 눌러 수신확인 결과를 보낸다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendReadNotice(object sender, EventArgs e)
        {
            Notice noticeFrm = (Notice)sender;
            try
            {
                string senderid = null;
                string noticeid = null;

                if (noticeFrm.Tag.ToString().Length != 0)
                {
                    noticeid = noticeFrm.Tag.ToString();
                }
                logWrite("noticeid : " + noticeid);
                senderid = noticeFrm.GetSenderId();
                SendMsg("21|" + this.myid + "|" + noticeid + "|" + senderid, server);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// MemoListForm 생성
        /// </summary>
        /// <param name="tempMsg"></param>
        private void ShowMemoList(string[] tempMsg)  //(Q|sender†content†time†seqnum|...|
        {
            try
            {
                //notreadmemoform = new NotReadMemoForm();
                //notreadmemoform.listView.Click += new EventHandler(listView_ItemSelectionChanged);
                //notreadmemoform.FormClosing += new FormClosingEventHandler(notreadmemoform_FormClosing);
                //for (int i = 1; i < tempMsg.Length; i++)
                //{
                //    string[] array = tempMsg[i].Split('†');
                //    string sender = array[0];
                //    string content = array[1];
                //    string time = array[2];
                //    string seqnum = array[3];
                //    ListViewItem item = notreadmemoform.listView.Items.Add(time, "보기", null);
                //    string name = getName(sender);
                //    item.SubItems.Add(name + "(" + sender + ")");
                //    item.SubItems.Add(time);
                //    item.SubItems.Add(content);
                //    item.Tag = seqnum;
                //}
                //notreadmemoform.Show();
                //notreadmemoform.Activate();
                //isMemo = true;

                if (noreceiveboardform == null)
                {
                    noreceiveboardform = new NoReceiveBoardForm();
                    noreceiveboardform.panel_memo.Enabled = true;
                    noreceiveboardform.dgv_memo.Visible = true;
                    noreceiveboardform.label_memo.Text = "부재중 쪽지 (" + this.NRmemo.Text + ")";
                    noreceiveboardform.dgv_memo.CellMouseClick += new DataGridViewCellMouseEventHandler(dgv_memo_CellMouseClick);
                    noreceiveboardform.FormClosing += new FormClosingEventHandler(noreceiveboardform_FormClosing);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string content = array[1];
                        string time = array[2];
                        string seqnum = array[3];

                        string name = GetUserName(sender);
                        int rownum = noreceiveboardform.dgv_memo.Rows.Add(new object[] { name + "(" + sender + ")", time, content });
                        DataGridViewRow row = noreceiveboardform.dgv_memo.Rows[rownum];
                        row.Tag = seqnum;
                    }
                }
                else
                {
                    noreceiveboardform.panel_memo.Enabled = true;
                    noreceiveboardform.dgv_memo.Visible = true;
                    noreceiveboardform.label_memo.Text = "부재중 쪽지 (" + this.NRmemo.Text + ")";
                    noreceiveboardform.dgv_memo.CellMouseClick += new DataGridViewCellMouseEventHandler(dgv_memo_CellMouseClick);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string content = array[1];
                        string time = array[2];
                        string seqnum = array[3];

                        string name = GetUserName(sender);
                        bool isExist = false;
                        DataGridViewRowCollection collection = noreceiveboardform.dgv_memo.Rows;

                        foreach (DataGridViewRow item in collection)
                        {
                            if (item.Tag.ToString().Equals(seqnum))
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                        {
                            int rownum = noreceiveboardform.dgv_memo.Rows.Add(new object[] { name + "(" + sender + ")", time, content });
                            DataGridViewRow row = noreceiveboardform.dgv_memo.Rows[rownum];
                            row.Tag = seqnum;
                        }
                    }
                }
                noreceiveboardform.WindowState = FormWindowState.Normal;
                noreceiveboardform.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void noreceiveboardform_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            noreceiveboardform.Hide();
        }

        private void dgv_memo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            intParamDele dele = new intParamDele(delNRmemo);
            Invoke(dele, e.RowIndex);
        }

        private void delNRmemo(int rowIndex)
        {
            try
            {
                DataGridViewRow row = noreceiveboardform.dgv_memo.Rows[rowIndex];
                string[] temparr = row.Cells[0].Value.ToString().Split('(');
                string name = temparr[0];
                string id = temparr[1].Split(')')[0].ToString();
                string content = row.Cells[2].Value.ToString();
                string msgtime = row.Cells[1].Value.ToString();

                MakeMemo(new string[] { "", name, id, content });

                MemoUtils.MemoFileWrite(this.myid, "m|" + name + "|" + id + "|" + content + "|" + this.myid + "|" + msgtime);
                noreceiveboardform.dgv_memo.Rows.RemoveAt(rowIndex);
                SendMsg("14|" + row.Tag.ToString(), server);

                int mnum = Convert.ToInt32(NRmemo.Text);
                if (mnum != 0)
                {
                    NRmemo.Text = (mnum - 1).ToString();
                }

                if (NRmemo.Text.Equals("0"))
                {
                    noreceiveboardform.panel_memo.Enabled = false;
                }
                noreceiveboardform.label_memo.Text = "부재중 메모(" + NRmemo.Text + ")";
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }


        private void notreadmemoform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form form = (Form)sender;
            e.Cancel = true;
            form.Hide();
        }

        /// <summary>
        /// 읽지 않은 공지사항을 보여줌
        /// </summary>
        /// <param name="tempMsg"></param>
        private void ShowNotReadNoticeList(string[] tempMsg)  //(T|sender†content†time†mode†seqnum†title|sender†content†time†mode†seqnum|...
        {
            try
            {
                if (noreceiveboardform == null)
                {
                    noreceiveboardform = new NoReceiveBoardForm();
                    noreceiveboardform.dgv_notice.Visible = true;
                    noreceiveboardform.panel_notice.Enabled = true;
                    noreceiveboardform.label_notice.Text = "부재중 공지 (" + this.NRnotice.Text + ")";
                    noreceiveboardform.dgv_notice.CellMouseClick += new DataGridViewCellMouseEventHandler(dgv_notice_CellMouseClick);
                    noreceiveboardform.FormClosing += new FormClosingEventHandler(noreceiveboardform_FormClosing);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {

                        string[] array = null;
                        if (tempMsg[i].Split('†').Length > 5)
                        {
                            array = tempMsg[i].Split('†');
                        }

                        if (array != null && array.Length > 5)
                        {
                            logWrite("notreadnotice_tag = " + array[2]);
                            string sender = "없음";
                            if (array[0] != null && array[0].Length != 0)
                            {
                                sender = array[0];
                            }

                            string content = "내용없음";

                            if (array.Length > 5)
                            {
                                content = array[1];
                            }

                            string time = "없음";
                            if (array[(array.Length - 3)] != null && array[(array.Length - 3)].Length != 0)
                            {
                                time = array[2];
                            }

                            string mode = "";
                            if (array[(array.Length - 2)] != null && array[(array.Length - 2)].Length != 0)
                            {
                                mode = array[3];
                            }

                            string seqnum = null;
                            if (array[(array.Length - 1)] != null && array[(array.Length - 1)].Length != 0)
                            {
                                seqnum = array[4];
                            }

                            string name = GetUserName(sender);

                            string title = "공지사항";
                            if (array[5].Length > 1)
                            {
                                title = array[5];
                            }

                            if (seqnum != null && seqnum.Length != 0)
                            {
                                if (mode.Equals("e"))
                                {
                                    int rownum = noreceiveboardform.dgv_notice.Rows.Add(new object[] { "긴급", title, content, name + "(" + sender + ")", time });
                                    noreceiveboardform.dgv_notice.Rows[rownum].DefaultCellStyle.ForeColor = Color.Red;
                                    noreceiveboardform.dgv_notice.Rows[rownum].Tag = seqnum;
                                }
                                else
                                {
                                    int rownum = noreceiveboardform.dgv_notice.Rows.Add(new object[] { "일반", title, content, name + "(" + sender + ")", time });
                                    noreceiveboardform.dgv_notice.Rows[rownum].Tag = seqnum;
                                }
                            }
                        }
                    }
                }
                else
                {
                    noreceiveboardform.dgv_notice.Visible = true;
                    noreceiveboardform.panel_notice.Enabled = true;
                    noreceiveboardform.label_notice.Text = "부재중 공지 (" + this.NRnotice.Text + ")";

                    for (int i = 1; i < tempMsg.Length; i++)
                    {

                        string[] array = null;
                        if (tempMsg[i].Split('†').Length > 5)
                        {
                            array = tempMsg[i].Split('†');
                        }

                        if (array != null && array.Length > 5)
                        {
                            logWrite("notreadnotice_tag = " + array[2]);
                            string sender = "없음";
                            if (array[0] != null && array[0].Length != 0)
                            {
                                sender = array[0];
                            }

                            string content = "내용없음";

                            if (array.Length > 5)
                            {
                                content = array[1];
                            }

                            string time = "없음";
                            if (array[(array.Length - 3)] != null && array[(array.Length - 3)].Length != 0)
                            {
                                time = array[2];
                            }

                            string mode = "";
                            if (array[(array.Length - 2)] != null && array[(array.Length - 2)].Length != 0)
                            {
                                mode = array[3];
                            }

                            string seqnum = null;
                            if (array[(array.Length - 1)] != null && array[(array.Length - 1)].Length != 0)
                            {
                                seqnum = array[4];
                            }

                            string name = GetUserName(sender);

                            string title = "공지사항";
                            if (array[5].Length > 1)
                            {
                                title = array[5];
                            }

                            if (seqnum != null && seqnum.Length != 0)
                            {
                                bool isExist = false;
                                DataGridViewRowCollection collection = noreceiveboardform.dgv_notice.Rows;

                                foreach (DataGridViewRow item in collection)
                                {
                                    if (item.Tag.ToString().Equals(seqnum))
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }

                                if (isExist == false)
                                {
                                    if (mode.Equals("e"))
                                    {
                                        int rownum = noreceiveboardform.dgv_notice.Rows.Add(new object[] { "긴급", title, content, name + "(" + sender + ")", time });
                                        noreceiveboardform.dgv_notice.Rows[rownum].DefaultCellStyle.ForeColor = Color.Red;
                                        noreceiveboardform.dgv_notice.Rows[rownum].Tag = seqnum;
                                    }
                                    else
                                    {
                                        int rownum = noreceiveboardform.dgv_notice.Rows.Add(new object[] { "일반", title, content, name + "(" + sender + ")", time });
                                        noreceiveboardform.dgv_notice.Rows[rownum].Tag = seqnum;
                                    }
                                }
                            }
                        }

                    }
                }
                noreceiveboardform.WindowState = FormWindowState.Normal;
                noreceiveboardform.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void dgv_notice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            intParamDele dele = new intParamDele(delNRnotice);
            Invoke(dele, e.RowIndex);
        }

        private void delNRnotice(int rowIndex)
        {
            try
            {
                DataGridViewRow row = noreceiveboardform.dgv_notice.Rows[rowIndex];

                string temp = row.Cells[3].Value.ToString();
                string[] ar1 = temp.Split('(');
                string[] ar2 = ar1[1].Split(')');
                string name = ar1[0];
                string id = ar2[0];
                string msg = row.Cells[2].Value.ToString();
                string mode = row.Cells[0].Value.ToString();
                string ntime = row.Cells[4].Value.ToString();
                string seqnum = row.Tag.ToString();
                string title = row.Cells[1].Value.ToString();
                string[] array = new string[] { "r", msg, id, mode, ntime, seqnum, title };  //n|메시지|발신자id|mode|seqnum|title
                ShowNotice(array);

                SendMsg("14|" + seqnum, server);
                noreceiveboardform.dgv_notice.Rows.RemoveAt(rowIndex);
                
                
                int mnum = Convert.ToInt32(NRnotice.Text);
                if (mnum != 0)
                {
                    NRnotice.Text = (mnum - 1).ToString();
                }

                if (NRnotice.Text.Equals("0"))
                {
                    noreceiveboardform.panel_notice.Enabled = false;
                }
                noreceiveboardform.label_notice.Text = "부재중 공지(" + NRnotice.Text + ")";
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void notreadnoticeform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form form = (Form)sender;
            e.Cancel = true;
            form.Hide();
        }

        /// <summary>
        /// 부재중 전송파일 리스트 폼 생성
        /// </summary>
        /// <param name="tempMsg"></param>
        private void ShowFileList(string[] tempMsg) //R|sender†filenum†filename†time†size†seqnum|sender†filenum†filename†time†size†seqnum|...
        {
            //try
            //{
            //    notreceivefileform = new NotReceiveFileForm();
            //    notreceivefileform.listView1.Click += new EventHandler(File_ItemSelectionChanged);
            //    notreceivefileform.FormClosing += new FormClosingEventHandler(notreceivefileform_FormClosing);
            //    for (int i = 1; i < tempMsg.Length; i++)
            //    {
            //        string[] array = tempMsg[i].Split('†');
            //        string sender = array[0];
            //        string filename = array[2];
            //        string time = array[3];
            //        string filesize = array[4];
            //        ListViewItem item = notreceivefileform.listView1.Items.Add(time, "보기", null);
            //        string name = getName(sender);
            //        item.SubItems.Add(filename);
            //        item.SubItems.Add(name + "(" + sender + ")");
            //        item.SubItems.Add(time);
            //        item.SubItems.Add(filesize);
            //        item.Tag = array[1];
            //        item.ToolTipText = array[5];
            //    }
            //    notreceivefileform.Show();
            //    notreceivefileform.Activate();
            //    isFile = true;
            //}
            //catch (Exception exception)
            //{
            //    logWrite(exception.ToString());
            //}

            try
            {
                if (noreceiveboardform != null)
                {
                    noreceiveboardform.dgv_file.Visible = true;
                    noreceiveboardform.panel_file.Enabled = true;
                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string filename = array[2];
                        string time = array[3];
                        string filesize = array[4];
                        
                        string name = GetUserName(sender);
                        int rownum = noreceiveboardform.dgv_file.Rows.Add(new object[] { filename, filesize, name + "(" + sender + ")", time });
                        DataGridViewRow row = noreceiveboardform.dgv_file.Rows[rownum];
                        row.Tag = array[1];
                        row.ErrorText = array[5];
                    }

                    noreceiveboardform.label_file.Text = "부재중 파일(" + NRfile.Text + ")";
                }
                else
                {
                    noreceiveboardform = new NoReceiveBoardForm();
                    noreceiveboardform.panel_file.Enabled = true;
                    noreceiveboardform.dgv_file.Visible = true;
                    noreceiveboardform.dgv_file.CellClick += new DataGridViewCellEventHandler(dgv_file_CellClick);
                    noreceiveboardform.FormClosing += new FormClosingEventHandler(noreceiveboardform_FormClosing);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string filename = array[2];
                        string time = array[3];
                        string filesize = array[4];

                        string name = GetUserName(sender);
                        bool isExist = false;
                        DataGridViewRowCollection collection = noreceiveboardform.dgv_file.Rows;

                        foreach (DataGridViewRow item in collection)
                        {
                            if (item.ErrorText.Equals(array[5]))
                            {
                                isExist = true;
                                break;
                            }
                        }

                        if (isExist == false)
                        {
                            int rownum = noreceiveboardform.dgv_file.Rows.Add(new object[] { filename, filesize, name + "(" + sender + ")", time });
                            DataGridViewRow row = noreceiveboardform.dgv_file.Rows[rownum];
                            row.Tag = array[1];
                            row.ErrorText = array[5];
                        }
                    }

                    noreceiveboardform.label_file.Text = "부재중 파일(" + NRfile.Text + ")";
                }
                noreceiveboardform.WindowState = FormWindowState.Normal;
                noreceiveboardform.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void dgv_file_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            intParamDele dele = new intParamDele(delNRfile);
            Invoke(dele, e.RowIndex);
        }

        /// <summary>
        /// 서버에서 파일수신(부재중 전송파일 리스트 폼)
        /// </summary>
        /// <param name="rowIndex"></param>
        private void delNRfile(int rowIndex)
        {
            try
            {
                DataGridViewRow row = noreceiveboardform.dgv_file.Rows[rowIndex];
                string filenum = row.Tag.ToString();
                string filename = row.Cells[0].Value.ToString();
                int filesize = Convert.ToInt32(row.Cells[1].Value.ToString());

                DialogResult result = MessageBox.Show("서버로 부터 다음과 같은 파일을 받으시겠습니까?\r\n\r\n 파일명 : " + filename, "파일전송", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialogDelegate save = new SaveFileDialogDelegate(ShowSaveFileDialog);
                    string savefilename = (string)Invoke(save, filename);

                    Hashtable info = new Hashtable();
                    info["filename"] = savefilename;
                    info["filesize"] = filesize;
                    info["filenum"] = filenum;
                    info["rowindex"] = rowIndex;
                    info["NRseq"] = row.ErrorText;
                    logWrite("파일사이즈 : " + filesize.ToString());
                    Thread file = new Thread(new ParameterizedThreadStart(FileReceiver));
                    file.Start((object)info);

                    SendMsg("12|" + this.myid + "|" + filenum, server);
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void refreshNRfile(int rowIndex)
        {
            try
            {

                if (noreceiveboardform != null)
                {
                    noreceiveboardform.dgv_file.Rows.RemoveAt(rowIndex);
                }

                int mnum = Convert.ToInt32(NRfile.Text);

                if (mnum != 0)
                {
                    NRfile.Text = (mnum - 1).ToString();
                }

                if (NRfile.Text.Equals("0"))
                {
                    noreceiveboardform.panel_file.Enabled = false;
                }
                noreceiveboardform.label_file.Text = "부재중 파일(" + NRfile.Text + ")";
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void notreceivefileform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form form = (Form)sender;
            e.Cancel = true;
            form.Hide();
        }

        private void showNoreadTransfer(string[] tempMsg)
        {
            try
            {
                if (noreceiveboardform == null)
                {
                    noreceiveboardform = new NoReceiveBoardForm();
                    noreceiveboardform.panel_trans.Enabled = true;
                    noreceiveboardform.dgv_transfer.Visible = true;
                    noreceiveboardform.label_trans.Text = "부재중 이관 (" + this.NRtrans.Text + ")";
                    noreceiveboardform.dgv_transfer.CellMouseClick += new DataGridViewCellMouseEventHandler(dgv_transfer_CellMouseClick);
                    noreceiveboardform.FormClosing += new FormClosingEventHandler(noreceiveboardform_FormClosing);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string content = array[1]; // array[1] => 22&ani&senderID&receiverID&일자&시간&CustomerName
                        string time = array[2];
                        string seqnum = array[3];
                        string name = GetUserName(sender);
                        
                        string[] temp = content.Split('&');
                        int rownum = 0;
                        if (temp.Length > 2)
                        {
                            rownum = noreceiveboardform.dgv_transfer.Rows.Add(new object[] { time, temp[1], name + "(" + sender + ")" });
                            DataGridViewRow row = noreceiveboardform.dgv_transfer.Rows[rownum];
                            row.Tag = seqnum + "|" + content;
                        }
                        else
                        {
                            rownum = noreceiveboardform.dgv_transfer.Rows.Add(new object[] { time, content, name + "(" + sender + ")" });
                            DataGridViewRow row = noreceiveboardform.dgv_transfer.Rows[rownum];
                            row.Tag = seqnum;
                        }
                        
                    }
                }
                else
                {
                    noreceiveboardform.panel_trans.Enabled = true;
                    noreceiveboardform.dgv_transfer.Visible = true;
                    noreceiveboardform.label_trans.Text = "부재중 이관 (" + this.NRtrans.Text + ")";
                    noreceiveboardform.dgv_transfer.CellMouseClick+=new DataGridViewCellMouseEventHandler(dgv_transfer_CellMouseClick);

                    for (int i = 1; i < tempMsg.Length; i++)
                    {
                        string[] array = tempMsg[i].Split('†');
                        string sender = array[0];
                        string content = array[1]; // array[1] => 22&ani&senderID&receiverID&일자&시간&CustomerName
                        string time = array[2];
                        string seqnum = array[3];

                        string name = GetUserName(sender);
                        bool isExist = false;
                        string[] temp = content.Split('&');

                        DataGridViewRowCollection collection = noreceiveboardform.dgv_transfer.Rows;


                        foreach (DataGridViewRow item in collection)
                        {
                            if (item != null)
                            {
                                if (item.Tag != null)
                                {
                                    if (item.Tag.ToString().Equals(seqnum))
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (isExist == false)
                        {
                            int rownum = 0;
                            if (temp.Length > 2)
                            {
                                rownum = noreceiveboardform.dgv_transfer.Rows.Add(new object[] { time, temp[1], name + "(" + sender + ")" });
                                DataGridViewRow row = noreceiveboardform.dgv_transfer.Rows[rownum];
                                row.Tag = seqnum + "|" + content;
                            }
                            else
                            {
                                rownum = noreceiveboardform.dgv_transfer.Rows.Add(new object[] { time, content, name + "(" + sender + ")" });
                                DataGridViewRow row = noreceiveboardform.dgv_transfer.Rows[rownum];
                                row.Tag = seqnum;
                            }
                        }
                    }
                }
                noreceiveboardform.WindowState = FormWindowState.Normal;
                noreceiveboardform.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void dgv_transfer_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            intParamDele dele = new intParamDele(delNRTrans);
            Invoke(dele, e.RowIndex);
        }

        private void delNRTrans(int rowIndex)
        {
            try
            {
                DataGridViewRow row = noreceiveboardform.dgv_transfer.Rows[rowIndex];
                string[] contentArray = null;
                string[] temp = row.Tag.ToString().Split('|'); // seqnum|22&ani&senderID&receiverID&일자&시간&CustomerName
                string ani = row.Cells[1].Value.ToString();
                if (temp.Length > 1)
                {
                    if (temp[1].Length > 0)
                    {
                        contentArray = temp[1].Split('&');

                        ShowTransInfoDele dele = new ShowTransInfoDele(showTransferInfo);
                        Invoke(dele, new object[] { contentArray[1], contentArray[2], contentArray[4], contentArray[5] });
                    }
                    else
                    {
                        doublestringDele answerdele = new doublestringDele(Answer);
                        Invoke(answerdele, new object[] { ani, "3" });
                    }
                }
                else
                {
                    doublestringDele answerdele = new doublestringDele(Answer);
                    Invoke(answerdele, new object[] { ani, "3" });
                }
               

                noreceiveboardform.dgv_transfer.Rows.RemoveAt(rowIndex);
                
                //선택한 부재중 이관 row 관련 DB 삭제 요청
                SendMsg("14|" + temp[0], server);

                int mnum = Convert.ToInt32(NRtrans.Text);
                if (mnum != 0)
                {
                    NRtrans.Text = (mnum - 1).ToString();
                }

                if (NRtrans.Text.Equals("0"))
                {
                    noreceiveboardform.panel_trans.Enabled = false;
                }
                noreceiveboardform.label_trans.Text = "부재중 이관(" + this.NRtrans.Text + ")";
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        /// <summary>
        /// 부재중 쪽지 리스트에서 해당 아이템을 클릭시 메모창 생성 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_ItemSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                ListViewItem mitem = view.SelectedItems[0];
                string temp = mitem.SubItems[1].Text;
                string[] ar1 = temp.Split('(');
                string[] ar2 = ar1[1].Split(')');
                string name = ar1[0];
                string id = ar2[0];
                string msg = mitem.SubItems[3].Text;
                string msgtime=mitem.SubItems[2].Text;
                string[] array = new string[] { "m", name, id, msg };
                MakeMemo(array);

                MemoUtils.MemoFileWrite(this.myid, "m|" + name + "|" + id + "|" + msg + "|" + this.myid + "|" + msgtime);

                SendMsg("14|" + mitem.Tag.ToString(), server);
                view.SelectedItems[0].Remove();
                int mnum = Convert.ToInt32(NRmemo.Text);
                if (mnum != 0)
                {
                    NRmemo.Text=(mnum - 1).ToString();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 부재중 공지사항 리스트 폼에서 아이템 클릭시 해당 공지사항 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notice_ItemSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                ListViewItem mitem = view.SelectedItems[0];
                string temp = mitem.SubItems[3].Text;
                string[] ar1 = temp.Split('(');
                string[] ar2 = ar1[1].Split(')');
                string name = ar1[0];
                string id = ar2[0];
                string msg = mitem.SubItems[2].Text;
                string mode = mitem.SubItems[0].Text;
                string ntime = mitem.SubItems[4].Text;
                string seqnum = mitem.Tag.ToString();
                string title = mitem.SubItems[1].Text;
                string[] array = new string[] { "r", msg, id, mode, ntime, seqnum, title};  //n|메시지|발신자id|mode|seqnum|title
                ShowNotice(array);

                SendMsg("14|" + seqnum, server);
                view.SelectedItems[0].Remove();
                int mnum = Convert.ToInt32(NRnotice.Text);
                if (mnum != 0)
                {
                    NRnotice.Text = (mnum - 1).ToString();
                }

            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 부재중 전송파일 리스트 폼에서 아이템 클릭시 해당 파일 수신 준비
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void File_ItemSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                ListViewItem mitem = view.SelectedItems[0];
                string filenum = mitem.Tag.ToString();
                string filename = mitem.SubItems[1].Text;
                int filesize = Convert.ToInt32(mitem.SubItems[4].Text);

                DialogResult result = MessageBox.Show("서버로 부터 다음과 같은 파일을 받으시겠습니까?\r\n\r\n 파일명 : " + filename, "파일전송", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialogDelegate save = new SaveFileDialogDelegate(ShowSaveFileDialog);
                    string savefilename = (string)Invoke(save, filename);

                    Hashtable info = new Hashtable();
                    info["filename"] = savefilename;
                    info["filesize"] = filesize;
                    logWrite(filesize.ToString());
                    Thread file = new Thread(new ParameterizedThreadStart(FileReceiver));
                    file.IsBackground = true;
                    file.Start((object)info);
                    SendMsg("12|" + this.myid + "|" + filenum, server);
                    SendMsg("14|" + mitem.ToolTipText, server);
                    view.SelectedItems[0].Remove();
                    int mnum = Convert.ToInt32(NRfile.Text);
                    if (mnum != 0)
                    {
                        NRfile.Text = (mnum - 1).ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 로그인 시 부재중 건수 정보 보이기
        /// </summary>
        /// <param name="arg"></param>
        private void ShowAbsentInfoNumber(string[] arg)
        {
            NRmemo.Text = arg[1];
            NRfile.Text = arg[2];
            NRnotice.Text = arg[3];
            NRtrans.Text = arg[4];

            if (Convert.ToInt32(arg[3].Trim()) > 0)
            {
                SendMsg(MsgrMsg.REQ_UNREAD_NOTICE + this.myid, server);
            }
        }

        /// <summary>
        /// 중복로그인 시도 경우 
        /// </summary>
        public void ReLogin()
        {
            panel_progress.Visible = false;
            defaultCtrl(true);
            DialogResult result = MessageBox.Show(this, "아이디 " + this.id.Text + "는 이미 로그인 상태입니다.\r\n 기존 접속을 로그아웃 하시겠습니까?", "로그인 중복", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                SendMsg("2|" + this.id.Text, server);
                closing();
                MessageBox.Show(this, "정상적으로 로그아웃 했습니다. 다시 로그인해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else closing();
        }

        public void logInFail()
        {
            id.Focus();
        }

        private void ShowMessageBox(string msg)
        {
            panel_progress.Visible = false;
            defaultCtrl(true);
            MessageBox.Show(this, msg, "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 로그인 성공시 자기이름 출력
        /// </summary>
        /// <param name="Name"></param>
        public void FlushInfo(string Name)
        {
            name.Text = Name + "(" + this.myid + ")"; ;
        }

        /// <summary>
        /// 로그인 성공시 소속 팀 출력
        /// </summary>
        /// <param name="Name"></param>
        public void Flushteam(string Name)
        {
            string clientName = Name;
            team.Text = clientName ;
        }


        /// <summary>
        /// 로그인 및 로그아웃 일때 폼 패널 및 버튼 컨트롤
        /// </summary>
        /// <param name="value"></param>
        public void LogInPanelVisible(bool value)
        {
            label_pass.Visible = value;
            label_id.Visible = value;
            id.Visible = value;
            tbx_pass.Visible = value;
            default_panal.Visible = value;
            btn_login.Visible = value;
            btn_login.Visible = value;
            pic_title.Visible = value;

            panel_logon.Visible = !value;
        }


        public void TreeViewVisible(bool value)
        {
            try {
                memTree.Visible = value;
            } catch (Exception ex) {
                logWrite(ex.ToString());
            }
        }

        public void StatBarText(string value)
        {
            //statusBar.Text = value;
        }

        public void ButtonCtrl(bool value)
        {
            InfoBar.Visible = value;
            memTree.Visible = value;
            MnDialogue.Enabled = value;
            MnSendFile.Enabled = value;
            MnLogout.Enabled = value;
            MnMemo.Enabled = value;
            //MnNoticeShow.Enabled = value;
            MnNotice.Enabled = value;
            Mn_server.Enabled = !value;
            Mn_extension.Enabled = !value;
        }

        /// <summary>
        /// 파일 저장시 FileDailog 창 생성
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string ShowSaveFileDialog(string filename)
        {
            string savefilename = null;
            SaveFileDialog savefiledialog = new SaveFileDialog();
            savefiledialog.FileName = filename;
            DialogResult savefileresult = savefiledialog.ShowDialog();
            if (savefileresult == DialogResult.OK)
            {
                savefilename = savefiledialog.FileName;
            }
            return savefilename;
        }

        /// <summary>
        /// ___________1:1창__________다자창_______
        /// 
        /// on   | key(out->in)      key변경없음
        ///        노드상태변경      노드상태변경
        /// 
        /// out  | key(in->out)      quit처리(**비정상종료로 quit없이 아웃됨)
        ///        노드상태변경
        ///        메시지display
        /// 
        /// busy | key변경없음       key변경없음
        ///        노드상태변경      노드상태변경
        /// 
        /// away |     동일
        /// 
        /// dnd  |     동일
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void SetUserStatusInChatForm(string id, string status)
        {
            try {

                string[] keys = new string[ChatFormList.Keys.Count];
                ChatFormList.Keys.CopyTo(keys, 0);
                
                foreach (string key in keys) {
                    if (!key.Contains(id))
                        continue;
                    ChatForm form = (ChatForm)ChatFormList[key];

                    switch (status) {
                        case MsgrUserStatus.ONLINE:
                            //1:1채팅인 경우만
                            if (form.HasSingleChatter()) {
                                //1.키변경
                                string newFormKey = ChatUtils.GetFormKeyWithUserAdded(key, myid, id);
                                SetChatFormKeyUpdate(form, newFormKey, key);
                                //2.노드상태변경
                                //3.메시지디스플레이
                                form.SetChatterLogIn(id, GetUserName(id));
                            } else {//다자창
                                //1.키변경없음
                                //2.노드상태변경
                                form.SetChatterLogIn(id, GetUserName(id));
                            }
                            break;
                        case MsgrUserStatus.LOGOUT:
                            //1:1채팅인 경우만
                            if (form.HasSingleChatter()) {
                                //1.키변경
                                string newFormKey = ChatUtils.GetFormKeyWithUserLogOut(key, myid, id);
                                SetChatFormKeyUpdate(form, newFormKey, key);
                                //2.노드상태변경
                                //3.메시지디스플레이
                                form.SetChatterLogOut(id, GetUserName(id));
                            } else {//다자창
                                //quit처리
                                //  1.키변경
                                string newFormKey = ChatUtils.GetFormKeyWithUserQuit(key, myid, id);
                                SetChatFormKeyUpdate(form, newFormKey, key);
                                //  2. 노드삭제
                                form.DeleteChatter(id, GetUserName(id));
                            }
                            break;
                        case MsgrUserStatus.BUSY:
                        case MsgrUserStatus.AWAY:
                        case MsgrUserStatus.DND:
                            //노드상태변경
                            form.SetChatterStatus(id, GetUserName(id), status);
                            break;
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 팀정보를 서버로 부터 받은 경우 TreeView에 트리 생성
        /// </summary>
        /// <param name="team"></param>
        /// <param name="list"></param>
        private void MakeMemTree(string team, ArrayList list) //list[] {id!name}
        {
            try
            {
                int nodeNum = memTree.Nodes.Count;
                TreeNode node=null;
                if (team.Length != 0)
                {
                    if (!memTree.Nodes.ContainsKey(team))
                    {
                        node = memTree.Nodes.Add(team, "");//팀노드 추가
                        node.Text = team;
                        node.NodeFont = new Font("굴림", 9.75f, FontStyle.Bold);
                        node.ForeColor = Color.IndianRed;
                        node.EnsureVisible();
                        //memTree.e

                        if (list != null && list.Count != 0)
                        {
                            foreach (object obj in list)  //list[] {id!name}
                            {
                                string m = (string)obj;
                                if (m.Length != 0)
                                {
                                    string[] arg = m.Split('!');
                                    if (!arg[1].Equals(myname))
                                    {
                                        TreeNode tempNode = memTree.Nodes[nodeNum].Nodes.Add(arg[0], arg[1]);   //사용자 노드 추가(노드 key=id, value=name)
                                        tempNode.ToolTipText = arg[0]; //MouseOver일 경우 나타남 
                                        tempNode.ForeColor = Color.Gray;
                                        tempNode.Tag = new UserObject(arg[0], arg[1], team);//arg[0];
                                        tempNode.ImageIndex = 0;
                                        tempNode.SelectedImageIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                memTree.ExpandAll();
                //if (!memTree.Nodes[0].IsExpanded) memTree.Nodes[0].Expand();
                //if (team.Equals(this.team.Text)) node.Expand(); 

                //this.statusBar.Text = "";
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkInfoForLogin();
            }
        }


        private void passwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkInfoForLogin();
            }
        }

        /// <summary>
        /// 로그인 요청시 서버측에 로그인 정보 전송
        /// </summary>
        private void SendInfo()
        {
            string info = "8|" + this.myid + "|" + this.mypass + "|" + this.extension + "|" + local.ToString();
            SendMsg(info, server);

            //Application.StartupPath + "\\WDMsg_Client_Demo.exe.config"
            setConfigXml(AppConfigFullPath, "id", this.myid); 
            setConfigXml(AppConfigFullPath, "extension", this.extension);
            if (cbx_pass_save.Checked == true)
            {
                setConfigXml(AppConfigFullPath, "pass", this.mypass);  
                setConfigXml(AppConfigFullPath, "save_pass", "1");
            }
        }

        private void setConfigXml(string filename, string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode pnode = doc.SelectSingleNode("//appSettings");
            if (pnode.HasChildNodes)
            {
                XmlNodeList nodelist = pnode.ChildNodes;
                foreach (XmlNode node in nodelist)
                {
                    if (node.Attributes["key"].Value.Equals(key))
                    {
                        node.Attributes["value"].Value = value;
                        break;
                    }
                }
                doc.Save(filename);
            }
        }

        private void setCRM_DB_HOST(string filename, string val)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode pnode = doc.SelectSingleNode("//db");

            if (pnode.HasChildNodes)
            {
                XmlNodeList nodelist = pnode.ChildNodes;
                foreach (XmlNode node in nodelist)
                {
                    if (node.Name.Equals("dbserverip"))
                    {
                        node.InnerText = val;
                    }
                }
                doc.Save(filename);
            }
        }

        private void MnDialog_Click(object sender, EventArgs e)
        {
            try {
                if (memTree.SelectedNode.GetNodeCount(true) != 0)
                    return; //팀원 노드 선택

                UserObject userObj = (UserObject)memTree.SelectedNode.Tag;
                logWrite("대화선택:" + userObj.Id);

                OpenSingleChatForm(userObj);
            } catch (Exception ex) {
                logWrite(ex.ToString());
            }
        }

        /// <summary>
        /// 우클릭시 팀노드 선택한경우 팀팝업메뉴, 이외의 경우 접속시 대화메뉴, 오프라인시 쪽지메뉴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try {
                
                TreeView view = (TreeView)sender;
                view.SelectedNode = e.Node;

                if (e.Button == MouseButtons.Right) {

                    if (e.Node.GetNodeCount(false) != 0) {
                        mouseMenuG.Show(view, e.Location, ToolStripDropDownDirection.BelowRight);
                    } else {
                        //if (InList.ContainsKey(e.Node.Tag)) { //접속한 경우
                        if (((UserObject)e.Node.Tag).Status != MsgrUserStatus.LOGOUT) {
                            mouseMenuN.Show(view, e.Location, ToolStripDropDownDirection.BelowRight);
                        } else {
                            mouseMenuC.Show(view, e.Location, ToolStripDropDownDirection.BelowRight);
                        }
                    }
                }
            }
            catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }


        private void memTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            try {
                if (memTree.SelectedNode.GetNodeCount(true) == 0) { //팀원 노드 선택
                    
                    UserObject userObj = (UserObject)e.Node.Tag;
                  
                    logWrite("대화선택:"+userObj.Id);

                    OpenSingleChatForm(userObj);
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 대화하기 메뉴를 클릭한 경우
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chat_Click(object sender, EventArgs e)
        {
            try
            {
                //if (memTree.SelectedNode.Level == 0) //상담원전체를 나타내는 노드를 선택한 견우 
                //{
                //    MessageBox.Show(this, "그룹 전체와는 대화할수 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else 
                if (memTree.SelectedNode.GetNodeCount(false) != 0)//선택한 노드가 하위 노드를 가지고 있을 경우
                {
                    logWrite("그룹대화 요청!");
                    List<UserObject> groupList = new List<UserObject>();                      //대화불가능자 아이디 문자열
                    int chattable = 0;
                    string chatIdList = myid;

                    foreach (TreeNode node in memTree.SelectedNode.Nodes) {
                        UserObject userObj = (UserObject)node.Tag;
                        if (userObj == null)
                            continue;
                        if (userObj.Status != MsgrUserStatus.LOGOUT) {
                            chattable++;
                            chatIdList = "/" + userObj.Id;
                        }
                        groupList.Add(userObj);
                    }

                    if (chattable == 0) //대화가능한 상대방이 없을경우
                    {
                        DialogResult result = MessageBox.Show(this, "요청한 상대방 모두가 대화가 불가능한 상태입니다.\r\n 대신 쪽지를 보내시겠습니까?", "알림", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            Hashtable MemoReceiver = new Hashtable();
                            foreach (TreeNode node in memTree.SelectedNode.Nodes)
                            {
                                UserObject userObj = (UserObject)node.Tag;
                                if (userObj == null)
                                    continue;
                                MemoReceiver[userObj.Id] = userObj.Name;
                            }
                            MakeSendMemo(MemoReceiver);//쪽지보내기
                        }
                    } else if (chattable == 1) {
                        UserObject userObj = (UserObject)groupList[0];
                        logWrite("대화선택:" + userObj.Id);
                        OpenSingleChatForm(userObj);
                    } else
                        MakeChatForm(groupList, chatIdList);
                }
                else //선택한 노드가 최하위 노드인 경우
                {
                    logWrite("일대일대화 요청");
                    UserObject userObj = (UserObject)memTree.SelectedNode.Tag;
                    OpenSingleChatForm(userObj);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        public void OpenSingleChatForm(UserObject userObj) {
            if (userObj != null && InList.ContainsKey(userObj.Id) && InList[userObj.Id] != null) {
                ChatForm chatForm = ChatUtils.FindChatForm(ChatFormList, userObj.Id);
                //존재하는 대화창이 없는 경우 생성
                if (chatForm == null) {
                    MakeChatForm(userObj);
                } else {
                    chatForm.SetForward();
                }

            } else  //대화가능한 상대방이 없을경우
                    {
                DialogResult result = MessageBox.Show(this, "대화할 상대방이 대화가 불가능한 상태입니다.\r\n 대신 쪽지를 보내시겠습니까?", "알림", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK) {
                    Hashtable MemoReceiver = new Hashtable();
                    MemoReceiver[userObj.Id] = userObj.Name;
                    MakeSendMemo(MemoReceiver);
                }
            }

        }

        /// <summary>
        /// 서버에서 로그인 클라이언트 리스트를 전송받아 해당 클라이언트 상태를 로그인으로 변경
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        private void ChangeMemStat(string id, string status)
        {
            try
            {
                PresenceUpdate(id, status);
            }
            catch (Exception e)
            {
                logWrite(id + " 상태값 변경 오류 : " + e.ToString());
            }
     
        }

        /// <summary>
        /// memTree에 로그인 클라이언트 노드 추가
        /// </summary>
        /// <param name="tempMsg">i|id|소속|클라이언트 IP주소|이름</param>
        private void AddMemTreeNode(string[] tempMsg)//i|id|소속|address|이름
        {
            try
            {
                TreeNode[] nodeArray = memTree.Nodes.Find(tempMsg[2], true);
                if (!nodeArray[0].Nodes.ContainsKey(tempMsg[1]))
                {
                    TreeNode tempNode = nodeArray[0].Nodes.Add(tempMsg[1], tempMsg[4]);
                    tempNode.ToolTipText = tempMsg[1]; //MouseOver일 경우 나타남 
                    tempNode.Tag = new UserObject(tempMsg[1], tempMsg[4], MsgrUserStatus.ONLINE);
                    tempNode.ImageIndex = 1;
                    tempNode.SelectedImageIndex = 1;
                    nodeArray[0].Expand();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 새로운 대화메시지 수신시 대화창 생성
        /// </summary>
        /// <param name="ar">d|formkey|id/id/...|name|메시지내용</param>
        private void NewChatForm(string[] ar)    //ar = d|formkey|id/id/...|name|메시지내용
        {
            try
            {
                ChatForm chatForm = new ChatForm(myid);
                chatForm.SetCustomFont(custom_color, custom_font);
                chatForm.SetFormKey(ChatUtils.GetFormKey(ar[1], myid));
                ChatFormList[chatForm.GetFormKey()] = chatForm;

                string[] tempidar = ar[2].Split('/'); //첫번째가 대화메시지 띄운 사람.

                foreach (string tempId in tempidar) {
                    if (!this.myid.Equals(tempId)) {
                        UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, tempId);
                        chatForm.SetChatterOnFormOpening(userObj);
                    }
                }

                chatForm.ChatMessageAdded += ChatMessageAdded;
                chatForm.BtnAddChatter.MouseClick += new MouseEventHandler(BtnAddChatter_Click);
                chatForm.chatSendFile.MouseClick += new MouseEventHandler(chatSendFile_Click);
                chatForm.FormClosing += new FormClosingEventHandler(chatForm_FormClosing);
                chatForm.txtbox_exam.ForeColorChanged += new EventHandler(txtbox_exam_Changed);
                chatForm.txtbox_exam.FontChanged += new EventHandler(txtbox_exam_Changed);

                //chatForm.WindowState = FormWindowState.Minimized;
                //chatForm.Show();
                chatForm.PostUserMessage(tempidar[0], ar[3]/*user name*/, ar[4]/*user msg*/);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 대화창에서 파일보내기 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chatSendFile_Click(object sender, MouseEventArgs e)
        {
            try
            {
                Hashtable list = new Hashtable();
                Button button = (Button)sender;
                ChatForm chatForm = ChatUtils.GetParentChatForm((Button)sender);
                TreeView view = chatForm.ChattersTree;

                if (view.Nodes.Count != 0) {

                    TreeNodeCollection col = view.Nodes;
                    foreach (TreeNode node in col)
                    {
                        UserObject userObj = (UserObject)node.Tag;
                        if (userObj.Status != MsgrUserStatus.LOGOUT)
                            list[userObj.Id] = userObj.Name;//key=id, value=name 인 값 목록
                    }
                }
                MakeSendFileForm(list);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 사용자가 대화하기를 선택시 대화창 생성
        /// </summary>
        /// <param name="chatter"></param>
        /// <param name="ids"></param>
        private void MakeChatForm(UserObject userObj) {
        
            try {
                if (userObj.Id == null || userObj.Id == "")
                    return;
                ChatForm chatForm = new ChatForm(myid);

                chatForm.SetCustomFont(custom_color, custom_font);
                chatForm.SetFormKey(ChatUtils.GetFormKey(userObj.Id, this.myid)); //DateTime.Now.ToString() +"!"+ this.myid;
                ChatFormList[chatForm.GetFormKey()] = chatForm;

                chatForm.SetChatterOnFormOpening(userObj);//대화창에 참가자 노드 추가(key=id, text=name)
                logWrite("Formkey 생성 : <" + chatForm.GetFormKey() + ">");
                
                chatForm.ChatMessageAdded += ChatMessageAdded;
                
                chatForm.BtnAddChatter.MouseClick += new MouseEventHandler(BtnAddChatter_Click);
                chatForm.chatSendFile.MouseClick += new MouseEventHandler(chatSendFile_Click);
                chatForm.FormClosing += new FormClosingEventHandler(chatForm_FormClosing);

                chatForm.txtbox_exam.ForeColorChanged += new EventHandler(txtbox_exam_Changed);
                chatForm.txtbox_exam.FontChanged += new EventHandler(txtbox_exam_Changed);
                chatForm.Show();
                chatForm.ReBox.Focus();

            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void ChatMessageAdded(object sender, EventArgs e) {

            ChatForm chatForm = ChatUtils.GetParentChatForm((Control)sender);
            TextBox msgBox = chatForm.ReBox;

            string str = null;

            if (msgBox.Text.Trim().Length != 0) {
            
                if (chatForm.ChattersTree.Nodes.Count == 0) {//채팅참가자 리스트뷰에 참가자가 없다면
                
                    MessageBox.Show("대화 상대방이 없습니다.\r\n대화할 상대방을 추가해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    logWrite("ChattersTree 에 대화상대방 없음");
                
                } else {//채팅참가자가 있을 경우만
                
                    string key = null;
                    string ids = this.myid;

                    key = chatForm.GetFormKey();
                    logWrite("Formkey=<" + key + ">");

                    TreeView view = chatForm.ChattersTree;

                    if (view.Nodes.Count != 0) {
                        for (int n = 0; n < view.Nodes.Count; n++) {
                            UserObject userObj = (UserObject)view.Nodes[n].Tag;
                            ids += "/" + userObj.Id;
                        }
                    }

                    str = "16|" + key + "|" + ids + "|" + myname + "|" + msgBox.Text.Trim();    //d|Formkey|id/id/..|발신자name|메시지 
                    logWrite("대화메시지 생성 : " + str);

                    SendMsg(str, server);

                    chatForm.PostMyMessage(myname, msgBox.Text.Trim());

                    msgBox.Clear();
                    msgBox.Focus();
                }
            }
        }

        private void txtbox_exam_Changed(object sender, EventArgs e)
        {
            try
            {
                TextBox box = (TextBox)sender;
                Color txtcolor = box.ForeColor;
                Font  txtfont = box.Font;
                logWrite("사용자 폰트/색상 변경 : " + txtcolor.Name + "/" + txtfont.ToString());
                saveFontColor(txtcolor, txtfont);
            }
            catch (Exception ex)
            {
                logWrite("txtbox_exam_Changed Error : " + ex.ToString());
            }
        }

        private void saveFontColor(Color color, Font font)
        {
            try {
                string _color = color.Name;
                TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
                string _font = fontConverter.ConvertToString(font);
                setConfigXml(AppConfigFullPath, "custom_color", _color);
                setConfigXml(AppConfigFullPath, "custom_font", _font);
            } catch (Exception ex) {
                logWrite("saveFontColor Error : " + ex.ToString());
            }
        }

        /// <summary>
        /// 사용자가 다수의 상대방과 대화하기를 요청했을 경우 대화창 생성
        /// </summary>
        /// <param name="chattersName">대화가능한 상대방의 이름열</param>
        /// <param name="nonchatters">대화불가능한 상대방의 이름열</param>
        /// <param name="ids">대화가능자의 id</param>
        private void MakeChatForm(List<UserObject> groupList, string chatIdList)
        {
            try {
                ChatForm chatForm = new ChatForm(myid);

                chatForm.SetCustomFont(custom_color, custom_font);
                chatForm.SetFormKey(ChatUtils.GetFormKey(chatIdList, this.myid));//DateTime.Now.ToString() + this.myid;
                logWrite("Formkey 생성 : " + chatForm.GetFormKey());

                foreach (UserObject userObj in groupList) {
                    if (userObj.Status != MsgrUserStatus.LOGOUT) {
                        chatForm.SetChatterOnFormOpening(userObj);
                    } else {
                        chatForm.PostCanNotJoinMessage(userObj.Name);
                    }
                }

                chatForm.ChatMessageAdded += ChatMessageAdded;
                
                chatForm.BtnAddChatter.MouseClick += new MouseEventHandler(BtnAddChatter_Click);
                chatForm.chatSendFile.MouseClick += new MouseEventHandler(chatSendFile_Click);
                chatForm.FormClosing += new FormClosingEventHandler(chatForm_FormClosing);
                chatForm.txtbox_exam.ForeColorChanged += new EventHandler(txtbox_exam_Changed);
                chatForm.txtbox_exam.FontChanged += new EventHandler(txtbox_exam_Changed);
                ChatFormList[chatForm.GetFormKey()] = chatForm;       //ChatterList 저장(key=id/id/.. value=chatform)

                chatForm.Show();
                chatForm.ReBox.Focus();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }

        }

        /// <summary>
        /// 대화창에서 대화자 추가 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddChatter_Click(object sender, MouseEventArgs e) {
            try {
                ChatForm chatForm = ChatUtils.GetParentChatForm((Button)sender);
                string key = chatForm.GetFormKey();
                logWrite(chatForm.GetFormKey());
                logWrite("상담원추가 폼 키값 생성 :" + key);
                MakeAddChatterForm(key);
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 대화자 추가 설정 폼 생성
        /// </summary>
        /// <param name="formkey"></param>
        private void MakeAddChatterForm(string formkey)  {

            try {
                //로그인사용자만 선택
                AddMemberForm addform = new AddMemberForm(true, formkey);
                addform.BtnConfirm.MouseClick += new MouseEventHandler(BtnConfirmAddMember_Click);
                addform.radiobt_g.Click += new EventHandler(radiobt_g_Click);
                addform.radiobt_con.Click += new EventHandler(radiobt_con_Click);
                addform.radiobt_all.Enabled = false;
                addform.combobox_team.SelectedValueChanged += new EventHandler(combobox_team_SelectedValueChanged);
                addform.CurrInListBox.MouseDoubleClick += new MouseEventHandler(CurrInListBox_MouseDoubleClick);

                ChatForm form = (ChatForm)ChatFormList[formkey];
                addform.SetAddListBox(form.ChattersTree.Nodes);
                addform.SetCurrInListBox(InList, this.memTree.Nodes);

                addform.radiobt_con.Checked = true;
                addform.ShowDialog(form);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 쪽지 및 파일 전송 수신자 추가 폼에서 "접속자" 라디오 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobt_con_Click(object sender, EventArgs e) {
        
            try {
            
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((RadioButton)sender);

                addMemberForm.combobox_team.Visible = false;
                
                addMemberForm.label_choice.Visible = false;

                ListBox addbox = addMemberForm.AddListBox;
                ListBox box = addMemberForm.CurrInListBox;
                box.Items.Clear();
                if (InList.Count != 0) {
                
                    foreach (DictionaryEntry de in InList) {
                    
                        if (de.Value != null) {
                        
                            UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, de.Key.ToString());
                            ListBoxItem item = new ListBoxItem(userObj);
                            if (ListBox.NoMatches == addbox.FindStringExact(item.Text)) {
                                box.Items.Add(item);
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 쪽지 및 파일 전송 수신자 추가 폼에서 "전체" 라디오 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobt_all_Click(object sender, EventArgs e) {

            try {
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((RadioButton)sender);
                ListBox addbox = addMemberForm.AddListBox;

                addMemberForm.combobox_team.Visible = false;

                addMemberForm.label_choice.Visible = false;

                ListBox box = addMemberForm.CurrInListBox;
                box.Items.Clear();

                GetAllMemberDelegate getAll = new GetAllMemberDelegate(GetAllMember);
                Hashtable all = (Hashtable)Invoke(getAll, null);
            
                if (all != null && all.Count != 0) {
                    foreach (DictionaryEntry de in all) {
                    
                        if (de.Value != null) {
                            UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, de.Key.ToString());
                            ListBoxItem item = new ListBoxItem(userObj);
                            if (ListBox.NoMatches == addbox.FindStringExact(item.Text)) {
                                box.Items.Add(item);
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 트리의 모든 사용자 목록을 불러와 Hashtable에 저장
        /// </summary>
        /// <returns></returns>
        private Hashtable GetAllMember()
        {

            Hashtable all = new Hashtable();//key=id, value=name
            try
            {
                if (treesource != null && treesource.Count != 0)  //treesource(key=팀이름, value=list(id!name)
                {
                    foreach (DictionaryEntry de in treesource)
                    {
                        ArrayList list = (ArrayList)de.Value;
                        foreach (object obj in list)
                        {
                            string item = (string)obj;
                            string[] array = item.Split('!');
                            string tempid = array[0];
                            string tempname = array[1];
                            all[tempid] = tempname;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
            return all;
        }


        private void radiobt_g_Click(object sender, EventArgs e)
        {
            objectDele dele = new objectDele(showGroupComboBoxForAddMember);
            Invoke(dele, sender);
        }

        private void showGroupComboBoxForAddMember(object sender)
        {
            try
            {
                logWrite("그룹별 보기 선택!");
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((RadioButton)sender);

                addMemberForm.label_choice.Visible = true;

                ComboBox box = addMemberForm.combobox_team;
                box.Items.Clear();
                foreach (DictionaryEntry de in treesource)
                {
                    if (de.Key.ToString() == "") {
                        box.Items.Add("기타");
                    } else {
                        box.Items.Add(de.Key.ToString());
                    }
                }
                box.Visible = true;

                addMemberForm.CurrInListBox.Items.Clear();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void combobox_team_SelectedValueChanged(object sender, EventArgs e) {
        
            try {
            
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((ComboBox)sender);
                ComboBox box = addMemberForm.combobox_team;
                string teamname = (String)box.SelectedItem;
                if (teamname == "기타")
                    teamname = "";

                GetMemberDelegate getmember = new GetMemberDelegate(ChatUtils.GetMember);
                Hashtable memTable = (Hashtable)Invoke(getmember,new object[] {treesource, teamname});
                int num = box.Parent.Controls.Count;
                ListBox addbox=addMemberForm.AddListBox;

                ListBox listbox = addMemberForm.CurrInListBox;

                listbox.Items.Clear();
                foreach (DictionaryEntry de in memTable) {
                
                    //string tempname = (String)de.Value;
                    string tempid = (String)de.Key;
                    if (InList.ContainsKey(tempid) && InList[tempid] != null) {
                    
                        //string item = tempname + "(" + tempid + ")";
                        UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, tempid);
                        ListBoxItem item = new ListBoxItem(userObj);
                        if (ListBox.NoMatches == addbox.FindStringExact(item.Text)) {
                            listbox.Items.Add(item);
                        }
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void combobox_team_SelectedValueChangedAll(object sender, EventArgs e) {
        
            try {
            
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((ComboBox)sender);
                ComboBox box = addMemberForm.combobox_team;
                ListBox addbox = addMemberForm.AddListBox;
                string teamname = (String)box.SelectedItem;
                if (teamname == "기타") teamname = "";
                GetMemberDelegate getmember = new GetMemberDelegate(ChatUtils.GetMember);
                Hashtable memTable = (Hashtable)Invoke(getmember, new object[] { treesource, teamname });
                int num = box.Parent.Controls.Count;

                ListBox listbox = addMemberForm.CurrInListBox;

                listbox.Items.Clear();
                foreach (DictionaryEntry de in memTable) {

                    string tempid = (String)de.Key;
                    
                    UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(memTree.Nodes, tempid);
                    ListBoxItem item = new ListBoxItem(userObj);
                    
                    if (ListBox.NoMatches == addbox.FindStringExact(item.Text)) {
                        listbox.Items.Add(item);
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void BtnConfirmAddMember_Click(object sender, MouseEventArgs e)
        {
            try
            {
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((Button)sender);
                string key = addMemberForm.formkey.Text;

                ListBox box = addMemberForm.AddListBox;
                ChatForm form = (ChatForm)ChatFormList[key];

                if (box.Items.Count != 0) {
                    
                    string addlist = null;

                    foreach (ListBoxItem item in box.Items) {
                    
                        UserObject userObj = (UserObject)item.Tag;
                        
                        addlist += userObj.Id + "/";
                    
                    }

                    //추가한 사용자 리스트 기존 대화자에게 전송
                    if (form.ChattersTree.Nodes.Count != 0) {

                        GetChattersDelegate getChatters = new GetChattersDelegate(form.GetChattersID);
                        string[] chatters = (string[])Invoke(getChatters);
                        string msg = "17|" + key + "|" + addlist + "|" + myname;      //c|formkey|id/id/...|name|

                        if (chatters != null && chatters.Length != 0) {

                            foreach (string tempid in chatters) {
                                SendMsg(msg + "|" + tempid, server);
                            }
                        }
                    }

                    //추가한 사용자 채팅창의 대화자 리스트에 추가
                    foreach (ListBoxItem item in box.Items)
                    {
                        UserObject userObj = (UserObject)item.Tag;

                        //대화자 노드 추가 델리게이트 호출
                        AddChatterDelegate addChatter = new AddChatterDelegate(form.AddChatterToNode);
                        Invoke(addChatter, new object[] { userObj });
                    }

                    //채팅창 폼키에 대화자리스트 반영
                    string newFormKey = ChatUtils.GetFormKeyWithMultiUsersAdded(form.GetFormKey(), myid, addlist);
                    SetChatFormKeyUpdate(form, newFormKey, form.GetFormKey());
                }

                addMemberForm.Close();
                form.TopMost = true;
                form.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private string GetUserName(string id)
        {
            return (string)MemberInfoList[id];
        }

        private void chatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                ChatForm chat = (ChatForm)sender;

                string key = chat.GetFormKey();
                RichTextBox box = chat.chatBox;

                try {
                    DialogFileWrite(key, box.Text, chat.Text);
                } catch (Exception e1) {
                    logWrite("chatForm_FormClosing() : ChatterList.Remove() 에러 " + e1.ToString());
                }

                TreeView tree = chat.ChattersTree;

                if (tree.Nodes.Count > 1) {                     //2명이상과 대화중 폼을 닫을 경우
                

                    for (int n = 0; n < tree.Nodes.Count; n++) {  // -1 : 마지막 공백 배열 제외

                        string str = "18|" + key + "|" + this.myid;    //q|Formkey|id 

                        UserObject userObj = (UserObject)tree.Nodes[n].Tag;

                        if (!this.myid.Equals(userObj.Id)) { //자신 빼고 전송

                            str += "|" + userObj.Id;

                            logWrite("대화상대방 id : " + userObj.Id);
                            logWrite("대화종료 메시지 생성 : " + str);
                            SendMsg(str, server);
                        }
                    }
                }

                ChatFormList.Remove(key);
                logWrite("채팅창 닫음으로 key=" + key + " ChatterList 테이블에서 삭제");
               
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void MakeMemo(string[] tempMemo) {//tempMemo(m|name|id|message)
        
            try {
            
                MemoForm memoForm = new MemoForm();
                //memoForm.Text = tempMemo[1] + "님의 쪽지";
                ////memoForm.MemoCont.Text = tempMemo[3];  
                //memoForm.MemoCont.Lines = tempMemo[3].Split(new string[] {Environment.NewLine}, StringSplitOptions.None); //줄바꿈 지원 2012.9.1
                //memoForm.senderid.Text = tempMemo[2];
                memoForm.setMemoInfo(tempMemo[2], tempMemo[1], tempMemo[3]);
                memoForm.MemoReplyDone += MemoReplyDone;
                memoForm.Show();
                memoForm.Activate();
                MemoTable.Add(memoForm);
            } catch (Exception e) {
                logWrite(e.ToString());
            }
        }

        private void MemoReplyDone(object sender, CustomEventArgs e) {
            try {
                MemoForm memoForm = (MemoForm)sender;
                string senderId = memoForm.getSenderId();
                string memoContent = (string)e.GetItem;
                string msg = "19|" + this.myname + "|" + this.myid + "|" + memoContent.Trim();
                string smsg = "4|" + this.myname + "|" + this.myid + "|" + memoContent.Trim();

                if (InList.ContainsKey(senderId) && InList[senderId] != null) {
                    IPEndPoint senderIP = (IPEndPoint)InList[senderId];
                    SendMsg(msg + "|" + senderId, server);
                } else
                    SendMsg(smsg + "|" + senderId, server);
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }


        private void MakeSendMemo(Hashtable MemoReceiver) { //MemoReceiver(key=id, value=name)
        
            try {
                SendMemoForm form = new SendMemoForm();
                form.formkey.Text = DateTime.Now.ToString();

                if (MemoReceiver.Count != 0) {
                    foreach (DictionaryEntry de in MemoReceiver) {
                        if (de.Value != null) {
                            form.txtbox_receiver.Text += (String)de.Value + "(" + (String)de.Key + ");";
                        }
                    }
                    logWrite("쪽지전송 리스트 생성 : " + form.txtbox_receiver.Text);
                }
                form.BtnReceiver.MouseClick += new MouseEventHandler(BtnReceiver_Click);
                form.BtnSend.MouseClick += new MouseEventHandler(BtnSend_Click);
                form.textBox1.KeyUp += new KeyEventHandler(textBox1_KeyDown);
                form.Show();
                form.textBox1.Focus();
                MemoFormList[form.formkey.Text] = form;
            } catch (Exception e) {
                logWrite(e.ToString());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control) {

                    SendMemoForm form = (SendMemoForm)ChatUtils.GetParentSendMemoForm((TextBox)sender);

                    if (form.txtbox_receiver.Text.Length != 0) {

                        TextBox memoContent = form.textBox1;

                        if (memoContent.Text.Length != 0) {
                            string msg = "19|" + this.myname + "|" + this.myid + "|" + memoContent.Text.Trim(); //m|name|발신자id|message
                            string smsg = "4|" + this.myname + "|" + this.myid + "|" + memoContent.Text.Trim(); //m|name|발신자id|message
                            logWrite("쪽지 메시지 생성 : " + msg);

                            string[] tempID = form.txtbox_receiver.Text.Split(';');

                            for (int n = 0; n < tempID.Length; n++) {
                                if (tempID[n].Length != 0) {
                                    string[] array = tempID[n].Split('(');
                                    string[] array1 = array[1].Split(')');
                                    string reID = array1[0];

                                    if (InList.ContainsKey(reID) && InList[reID] != null) {
                                        SendMsg(msg + "|" + reID, server);
                                    } else {
                                        SendMsg(smsg + "|" + reID, server);
                                    }

                                    form.Dispose();
                                    break;

                                }
                            }
                        }
                        
                    } else {
                        DialogResult result = MessageBox.Show(form, "쪽지를 받을 상대방을 지정해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK) {


                            string key = form.formkey.Text;
                            AddMemoReceiver(key);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void MakeSendNotice()
        {
            SendNoticeForm form = new SendNoticeForm();
            //form.BtnSend.MouseClick += new MouseEventHandler(BtnSendNotice_Click);
            //form.TextBoxContent.KeyUp += new KeyEventHandler(textBox1_KeyDown_SendNotice);
            form.NoticeRegisterRequested += RegisterNotice;
            form.Show();
        }

        public void RegisterNotice(object sender, CustomEventArgs e)
            //bool isEmergency, string title, string content)
        {
            NoticeInfo info = (NoticeInfo)(e.GetItem);

            String NoticeTime = DateTime.Now.ToString();
            if (!info.IsEmergency)
            {
                MakeNoticeResult(NoticeTime, info.Title, info.Content, "일반");
                SendMsg("6|" + info.Content + "|" + this.myid + "|n" + "|" + NoticeTime + "|" + info.Title, server);
            }
            else
            {
                MakeNoticeResult(NoticeTime, info.Title, info.Content, "긴급");
                SendMsg("6|" + info.Content + "|" + this.myid + "|e" + "|" + NoticeTime + "|" + info.Title, server);
            }
        }

        private void MakeNoticeResult(string noticetime, string title, string content, string mode)
        {
            try
            {
                if (isMadeNoticeResult == false)
                {
                    if (noticeresultform != null)
                        noticeresultform.Close();
                    noticeresultform = new NoticeResultForm();
                    noticeresultform.listView1.Click += new EventHandler(listView1_Click);
                    noticeresultform.FormClosing += new FormClosingEventHandler(noticeresultform_FormClosing);
                    noticeresultform.FormClosed += new FormClosedEventHandler(noticeresultform_FormClosed);

                    ListViewItem item = noticeresultform.listView1.Items.Add(noticetime, "자세히", null);
                    item.Tag = noticetime;
                    item.SubItems.Add(noticetime);
                    item.SubItems.Add(mode);
                    item.SubItems.Add(title);
                    item.SubItems.Add(content);

                    NoticeDetailResultForm noticedetailresultform = new NoticeDetailResultForm();
                    noticedetailresultform.FormClosing += new FormClosingEventHandler(noticedetailresultform_FormClosing);
                    foreach (DictionaryEntry de in MemberInfoList)
                    {
                        string receiver = de.Value.ToString() + "(" + de.Key.ToString() + ")";
                        ListViewItem ditem = noticedetailresultform.listView1.Items.Add(de.Key.ToString(), receiver, null);
                        ditem.ForeColor = Color.Red;
                        ListViewItem.ListViewSubItem subitem = ditem.SubItems.Add("확인 안함");
                    }
                    isMadeNoticeResult = true;
                    NoticeDetailForm[noticetime] = noticedetailresultform;
                }
                else
                {
                    ListViewItem item = noticeresultform.listView1.Items.Add(noticetime, "자세히", null);
                    item.Tag = noticetime;
                    item.SubItems.Add(noticetime);
                    item.SubItems.Add(mode);
                    item.SubItems.Add(title);
                    item.SubItems.Add(content);

                    NoticeDetailResultForm noticedetailresultform = new NoticeDetailResultForm();
                    noticedetailresultform.FormClosing += new FormClosingEventHandler(noticedetailresultform_FormClosing);
                    foreach (DictionaryEntry de in MemberInfoList)
                    {
                        string receiver = de.Value.ToString() + "(" + de.Key.ToString() + ")";
                        ListViewItem ditem = noticedetailresultform.listView1.Items.Add(de.Key.ToString(), receiver, null);
                        ditem.ForeColor = Color.Red;
                        ditem.SubItems.Add("확인 안함");
                    }
                    NoticeDetailForm[noticetime] = noticedetailresultform;
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void noticeresultform_FormClosed(object sender, FormClosedEventArgs e)
        {
            isMadeNoticeResult = false;
        }

        private void showNoticeResultFromDB(string[] tempMsg)// ntime†content†nmode†title†안읽은사람1:안읽은사람2:...
        {

            logWrite("showNoticeResultFromDB 실행"); 
            string noticetime = "";
            string title = "";
            string content = "";
            string mode = "";
            ArrayList notreader = new ArrayList();


            try
            {
                if (isMadeNoticeResult == false)
                {
                    if (noticeresultform != null)
                        noticeresultform.Close();
                    noticeresultform = new NoticeResultForm();
                    noticeresultform.listView1.Click += new EventHandler(listView1_Click);
                    noticeresultform.FormClosing += new FormClosingEventHandler(noticeresultform_FormClosing);
                    noticeresultform.FormClosed += new FormClosedEventHandler(noticeresultform_FormClosed);

                    foreach (string strarr in tempMsg)
                    {
                        logWrite(strarr);
                        string[] itemarr = strarr.Split('†');

                        if (itemarr.Length > 3)
                        {
                            noticetime = itemarr[0];
                            content = itemarr[1];
                            mode = itemarr[2];
                            title = itemarr[3];
                            string[] readers = itemarr[4].Split(':');
                            foreach (string readerid in readers)
                            {
                                if (readerid.Trim().Length > 0)
                                {
                                    notreader.Add(readerid.Trim());
                                }
                            }

                            //발송 공지 항목을 리스트에 추가
                            ListViewItem item = noticeresultform.listView1.Items.Add(noticetime, "자세히", null);
                            item.Tag = noticetime;
                            item.SubItems.Add(noticetime);
                            if (mode.Equals("e"))
                            {
                                item.SubItems.Add("긴급");
                            }
                            else
                            {
                                item.SubItems.Add("일반");
                            }
                            item.SubItems.Add(title);
                            item.SubItems.Add(content);

                            //발송 공지 항목 각각의 상세 확인 리스트폼 생성
                            NoticeDetailResultForm noticedetailresultform = new NoticeDetailResultForm();
                            noticedetailresultform.FormClosing += new FormClosingEventHandler(noticedetailresultform_FormClosing);
                            foreach (DictionaryEntry de in MemberInfoList)
                            {
                                string receiver = de.Value.ToString() + "(" + de.Key.ToString() + ")";
                                ListViewItem ditem = noticedetailresultform.listView1.Items.Add(de.Key.ToString(), receiver, null);

                                if (notreader.Contains(de.Key.ToString()))
                                {
                                    ditem.ForeColor = Color.Red;
                                    ListViewItem.ListViewSubItem subitem = ditem.SubItems.Add("확인안함");
                                }
                                else
                                {
                                    ditem.ForeColor = Color.Blue;
                                    ListViewItem.ListViewSubItem subitem = ditem.SubItems.Add("읽음");
                                }
                            }

                            NoticeDetailForm[noticetime] = noticedetailresultform;
                        }
                    }
                }
                else
                {
                    foreach (string strarr in tempMsg)
                    {
                        string[] itemarr = strarr.Split('†');

                        if (itemarr.Length > 3)
                        {
                            noticetime = itemarr[0];
                            content = itemarr[1];
                            mode = itemarr[2];
                            title = itemarr[3];
                            string[] readers = itemarr[4].Split(':');

                            foreach (string readerid in readers)
                            {
                                if (readerid.Trim().Length > 0)
                                {
                                    notreader.Add(readerid.Trim());
                                }
                            }

                            ListView.ListViewItemCollection collection = noticeresultform.listView1.Items;
                            bool isexist = false;

                            foreach (ListViewItem row in collection)
                            {
                                if (noticetime.Equals(row.Tag.ToString().Trim()))
                                {
                                    isexist = true;
                                    break;
                                }
                            }

                            if (isexist == false)
                            {
                                ListViewItem item = noticeresultform.listView1.Items.Add(noticetime, "자세히", null);
                                item.Tag = noticetime;
                                item.SubItems.Add(noticetime);
                                item.SubItems.Add(mode);
                                item.SubItems.Add(title);
                                item.SubItems.Add(content);

                                NoticeDetailResultForm noticedetailresultform = new NoticeDetailResultForm();
                                noticedetailresultform.FormClosing += new FormClosingEventHandler(noticedetailresultform_FormClosing);
                                foreach (DictionaryEntry de in MemberInfoList)
                                {
                                    string receiver = de.Value.ToString() + "(" + de.Key.ToString() + ")";
                                    ListViewItem ditem = noticedetailresultform.listView1.Items.Add(de.Key.ToString(), receiver, null);

                                    if (notreader.Contains(de.Key.ToString()))
                                    {
                                        ditem.ForeColor = Color.Red;
                                        ListViewItem.ListViewSubItem subitem = ditem.SubItems.Add("확인안함");
                                    }
                                    else
                                    {
                                        ditem.ForeColor = Color.Blue;
                                        ListViewItem.ListViewSubItem subitem = ditem.SubItems.Add("읽음");
                                    }
                                }
                                NoticeDetailForm[noticetime] = noticedetailresultform;
                            }

                        }
                    }
                }

                noticeresultform.Show();
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void noticedetailresultform_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form form = (Form)sender;
            form.Hide();
        }

        private void noticeresultform_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form form = (Form)sender;
            form.Hide();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                ListViewItem item = view.SelectedItems[0];
                string noticeid = item.Tag.ToString();

                if (MadeNoticeDetail != null)
                {
                    NoticeDetailResultForm form = (NoticeDetailResultForm)NoticeDetailForm[MadeNoticeDetail];
                    form.Hide();
                }
                if (NoticeDetailForm.Count != 0 && NoticeDetailForm.ContainsKey(noticeid) && NoticeDetailForm[noticeid] != null)
                {
                    NoticeDetailResultForm form = (NoticeDetailResultForm)NoticeDetailForm[noticeid];
                    form.ShowDialog(noticeresultform);
                    MadeNoticeDetail = noticeid;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void NoticeReaderAdd(string[] msg)//C|id|noticeid
        {
            try
            {
                if (NoticeDetailForm.ContainsKey(msg[2]) && NoticeDetailForm[msg[2]] != null)
                {
                    NoticeDetailResultForm form = (NoticeDetailResultForm)NoticeDetailForm[msg[2]];
                    ListViewItem[] itemArray = form.listView1.Items.Find(msg[1], false);
                    if (itemArray != null && itemArray.Length != 0)
                    {
                        itemArray[0].ForeColor = Color.Blue;
                        itemArray[0].SubItems[1].Text = "읽 음";
                    }
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void MnMemo_Click(object sender, EventArgs e) {
            try {
                if (memTree.SelectedNode.GetNodeCount(true) != 0)
                    return; //팀원 노드 선택

                UserObject userObj = (UserObject)memTree.SelectedNode.Tag;
                logWrite("쪽지선택:" + userObj.Id);

                Hashtable MemoReceiver = new Hashtable();
                MemoReceiver[userObj.Id] = userObj.Name;
                MakeSendMemo(MemoReceiver);
            } catch (Exception ex) {
                logWrite(ex.ToString());
            }
        
        }

        private void BtnReceiver_Click(object sender, MouseEventArgs e) {

            try {
            
                SendMemoForm form = (SendMemoForm)ChatUtils.GetParentSendMemoForm((Button)sender);
                AddMemoReceiver(form.formkey.Text);
            
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 쪽지쓰기 대상자 추가
        /// </summary>
        /// <param name="formkey"></param>
        private void AddMemoReceiver(string formkey) {
            
            try {
            
                //전체 선택
                AddMemberForm addform = new AddMemberForm(false, formkey);
                addform.BtnConfirm.MouseClick += new MouseEventHandler(BtnConfirmForMemo_Click);
                addform.radiobt_g.Click += new EventHandler(radiobt_g_Click);
                addform.radiobt_con.Click += new EventHandler(radiobt_con_Click);
                addform.radiobt_all.Click += new EventHandler(radiobt_all_Click);
                addform.combobox_team.SelectedValueChanged += new EventHandler(combobox_team_SelectedValueChangedAll);
                addform.CurrInListBox.MouseDoubleClick += new MouseEventHandler(CurrInListBox_MouseDoubleClick);

                SendMemoForm form = (SendMemoForm)MemoFormList[formkey];
                
                string[] receiverArray = form.txtbox_receiver.Text.Split(';');
                addform.SetAddListBox(receiverArray, memTree.Nodes);

                Hashtable all = GetAllMember();
                addform.SetCurrInListBox(all, this.memTree.Nodes);

                addform.ShowDialog(form);
            
            } catch (Exception e) {
            
                logWrite(e.ToString());
            
            }
        }

        private void CurrInListBox_MouseDoubleClick(object sender, MouseEventArgs e) {
            
            try {
            
                ListBox box = (ListBox)sender;
                string additem = null;

                if (box.SelectedItems.Count != 0) {

                    additem = box.SelectedItem.ToString();

                    int count = box.Parent.Controls.Count;

                    for (int i = 0; i < count; i++) {

                        if (box.Parent.Controls[i].Name.Equals("AddListBox")) {

                            ListBox addbox = (ListBox)box.Parent.Controls[i];

                            if (addbox.Items.Contains(additem)) {

                                MessageBox.Show(this, "이미 선택된 사용자 입니다.", "중복선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            } else {

                                addbox.Items.Add(additem);
                                box.Items.Remove(additem);
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void BtnConfirmForMemo_Click(object sender, MouseEventArgs e) {

            try {
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((Button)sender);

                string key = addMemberForm.formkey.Text;
                SendMemoForm form = (SendMemoForm)MemoFormList[key];

                ListBox box = addMemberForm.AddListBox;

                if (box.Items.Count != 0) {
                    
                    if (form.txtbox_receiver.Text.Length != 0) {
                        form.txtbox_receiver.Clear();
                    }

                    foreach (ListBoxItem item in box.Items) {
                        UserObject userObj = (UserObject)item.Tag;
                        string str = string.Format("{0}({1})",userObj.Name,userObj.Id);
                        form.txtbox_receiver.AppendText(str + ";");
                    }
                }

                addMemberForm.Dispose();
                form.Activate();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        private void BtnSend_Click(object sender, MouseEventArgs e) {
        
            try {

                SendMemoForm form = (SendMemoForm)ChatUtils.GetParentSendMemoForm((Button)sender);


                if (form.txtbox_receiver.Text.Length != 0) {
                    TextBox memoContent = form.textBox1;

                    if (memoContent.Text.Length != 0) {

                        string msg = "19|" + this.myname + "|" + this.myid + "|" + memoContent.Text.Trim(); //m|name|발신자id|message
                        string smsg = "4|" + this.myname + "|" + this.myid + "|" + memoContent.Text.Trim(); //m|name|발신자id|message
                        logWrite("쪽지 메시지 생성 : " + msg);

                        string[] tempID = form.txtbox_receiver.Text.Split(';');

                        for (int n = 0; n < tempID.Length; n++) {
                            if (tempID[n].Length != 0) {
                                string[] array = tempID[n].Split('(');
                                string[] array1 = array[1].Split(')');
                                string reID = array1[0];
                                logWrite("쪽지 수신자 아이디 : " + reID);
                                if (InList.ContainsKey(reID) && InList[reID] != null) {
                                    SendMsg(msg + "|" + reID, server);
                                } else {
                                    SendMsg(smsg + "|" + reID, server);
                                }
                            }
                        }
                        form.Dispose();
                    }

                } else {
                    DialogResult result = MessageBox.Show(form, "쪽지를 받을 상대방을 지정해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == DialogResult.OK) {

                        string key = form.formkey.Text;

                        AddMemoReceiver(key);
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void MakeSendFileForm(Hashtable list)//key=id, value=name
        {
            try
            {
                SendFileForm sendform = new SendFileForm();
                sendform.formkey.Text = DateTime.Now.ToLongTimeString();//CHOI_DEBUG
                sendform.btn_start.MouseClick += new MouseEventHandler(btn_start_Click);
                sendform.btn_cancel.MouseClick += new MouseEventHandler(btn_cancel_Click);
                sendform.btn_receivers.MouseClick += new MouseEventHandler(btn_receivers_Click);
                sendform.label_detail.MouseClick += new MouseEventHandler(label_detail_Click);
                sendform.btn_selectfile.MouseClick += new MouseEventHandler(btn_selectfile_Click);
                ToolTip tip = new ToolTip();
                tip.IsBalloon = true;
                tip.ToolTipIcon = ToolTipIcon.Info;
                tip.ToolTipTitle = "받는사람";
                tip.SetToolTip(sendform.txtbox_FileReceiver, sendform.txtbox_FileReceiver.Text);
                FileSendFormList[sendform.formkey.Text] = sendform;

                bool isAll = false;
                if (list != null && list.Count != 0)
                {
                    foreach (DictionaryEntry de in list)
                    {
                        if (de.Value != null)
                        {
                            if (((string)de.Value).Equals("all"))
                            {
                                sendform.txtbox_FileReceiver.Text = "상담원전체;";
                                isAll = true;
                            }
                            else
                                sendform.txtbox_FileReceiver.Text += (string)de.Value + "(" + (string)de.Key + ");";
                        }
                        if (isAll == true) break;
                    }
                    sendform.Show();
                    sendform.Activate();
                }
                else
                {
                    sendform.Show();
                    sendform.Activate();
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void btn_selectfile_Click(object sender, MouseEventArgs e)
        {
            try
            {
                SendFileForm form = (SendFileForm)ChatUtils.GetParentSendFileForm((Button)sender);

                ShowFileSelectDialog(form);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void ShowFileSelectDialog(SendFileForm sendform)
        {
            try
            {

                bool FileSelected = false;

                DialogResult result = openFileDialog.ShowDialog(sendform);
                string filename = null;
                if (result == DialogResult.OK)
                {

                    if (openFileDialog.FileName != null || openFileDialog.FileName.Length != 0)
                    {
                        filename = openFileDialog.FileName;
                        FileSelected = true;
                        sendform.Enabled = true;
                    }
                }
                else sendform.Enabled = true;

                if (FileSelected == true)
                {
                    string[] filenameArray = filename.Split('\\');
                    if (filenameArray.Length > 2)
                    {
                        sendform.label_filename.Text = filenameArray[0] + "\\..\\" + filenameArray[(filenameArray.Length - 1)];
                        sendform.label_filename.Tag = filename;
                    }
                    else sendform.label_filename.Text = filename;

                    ToolTip tip = new ToolTip();
                    tip.SetToolTip(sendform.label_filename, filename);

                    FileInfo fi = new FileInfo(filename);
                    int fsize = Convert.ToInt32(fi.Length / 1000);
                    if (fsize.ToString().Length > 3)
                    {
                        fsize = fsize / 1000;
                        sendform.label_filesize.Text = fsize + " MB (" + fi.Length.ToString() + " byte)";
                    }
                    else sendform.label_filesize.Text = fsize + " Kb (" + fi.Length.ToString() + " byte)";
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void MnSendFile_Click(object sender, EventArgs e)
        {
            try {
                if (memTree.SelectedNode.GetNodeCount(true) != 0)
                    return; //팀원 노드 선택

                UserObject userObj = (UserObject)memTree.SelectedNode.Tag;
                logWrite("파일전송 선택:" + userObj.Id);
                Hashtable list = new Hashtable();
                if (userObj.Status != MsgrUserStatus.LOGOUT) {
                    list[userObj.Id] = userObj.Name;//key=id, value=name 인 값 목록
                    MakeSendFileForm(list);
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 1명의 사용자를 선택하여 파일 전송
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StripMn_file_Click(object sender, EventArgs e)
        {
            try {
                TreeNode node = memTree.SelectedNode;
                Hashtable list = new Hashtable();

                if (node.GetNodeCount(false) != 0) {
                    MessageBox.Show(this, "2명 이상의 사용자에게는 파일을 전송할 수 없습니다.", "사용자선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else {
                    UserObject userObj = (UserObject)node.Tag;
                    if (userObj.Status != MsgrUserStatus.LOGOUT) {
                        list[userObj.Id] = userObj.Name;
                        MakeSendFileForm(list);
                    }
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void label_detail_Click(object sender, MouseEventArgs e)
        {
            try
            {
                SendFileForm form = (SendFileForm)ChatUtils.GetParentSendFileForm((Label)sender);

                string key = form.formkey.Text;

                logWrite("label_detail_Click : formkey(" + key + ")");

                FileSendDetailListView view = (FileSendDetailListView)FileSendDetailList[key];
                view.Show();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_receivers_Click(object sender, MouseEventArgs e) {
        
            try {
            
                SendFileForm form = (SendFileForm)ChatUtils.GetParentSendFileForm((Button)sender);

                string key = form.formkey.Text;
                logWrite("btn_receivers_Click : formkey(" + key + ")");
                AddFileReceiver(key);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_cancel_Click(object sender, MouseEventArgs e) {
        
            try {
            
                SendFileForm form = (SendFileForm)ChatUtils.GetParentSendFileForm((Button)sender);

                string key = form.formkey.Text;
                logWrite("파일전송취소버튼 key:" + key);

                if (FtpClientThreadList.ContainsKey(key) && FtpClientThreadList[key] != null) {
                    try {
                        ((FtpClientManager)FtpClientThreadList[key]).CancelSending();
                    } catch (Exception te) {
                        logWrite(te.ToString());
                    }
                }

                if (FileSendThreadList.ContainsKey(key) && FileSendThreadList[key] != null) {
                    try {
                        ((Thread)FileSendThreadList[key]).Abort();
                    } catch (ThreadAbortException te) {
                        logWrite(te.ToString());
                    }
                }

                if (FileSendFormList.ContainsKey(key) && FileSendFormList[key] != null) {
                    FileSendFormList.Remove(key);
                    logWrite("FileSendFormList.Remove(key) :" + key);
                }
                
                if (FileSendDetailList.ContainsKey(key) && FileSendDetailList[key] != null) {
                    FileSendDetailListView view = (FileSendDetailListView)FileSendDetailList[key];
                    view.Dispose();
                    FileSendDetailList.Remove(key);
                    logWrite("FileSendDetailList.Remove(key) :" + key);
                }

                form.Dispose();

            } catch (Exception exception) {

                logWrite(exception.ToString());
            }
        }

        private void btn_start_Click(object sender, MouseEventArgs e)
        {
            try
            {
                //해당 폼 찾기
                SendFileForm form = (SendFileForm)ChatUtils.GetParentSendFileForm((Button)sender);

                string key = form.formkey.Text;
                string filename = null;

                logWrite("파일전송시작 : formkey 값 얻어옴(" + key + ")");
                if (FileSendFormList.ContainsKey(key) && FileSendFormList[key] != null) {

                    form = (SendFileForm)FileSendFormList[key];
                    logWrite("key값에 대한 Form 찾음! ");
                } else {
                    logWrite("키값에 대한 SendFileForm 없음! key=" + key);
                }

                //전송파일 선택안한 경우
                if (form.label_filename.Text.Length == 0) {
                
                    logWrite("btn_start_Click : 전송할 파일이 없음");
                    DialogResult result = MessageBox.Show(form, "전송할 파일을 선택해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    bool FileSelected = false;

                    if (result == DialogResult.OK) {
                    
                        DialogResult fresult = openFileDialog.ShowDialog(form);
                        if (fresult == DialogResult.OK) {
                        
                            if (openFileDialog.FileName.Length != 0) {
                            
                                filename = openFileDialog.FileName;
                                FileSelected = true;
                            }
                        }
                        if (FileSelected == true) {
                        
                            string[] filenameArray = filename.Split('\\');
                            if (filenameArray.Length > 2) {

                                form.label_filename.Text = filenameArray[0] + "\\...\\" + filenameArray[(filenameArray.Length - 1)];
                                form.label_filename.Tag = filename;
                                logWrite(" btn_start_Click()  form.label_filename.Tag = " + form.label_filename.Tag.ToString());
                            
                            } else {
                            
                                form.label_filename.Text = filename;
                            
                            }

                            ToolTip tip = new ToolTip();
                            tip.SetToolTip(form.label_filename, filename);

                            FileInfo fi = new FileInfo(filename);
                            int fsize = Convert.ToInt32(fi.Length / 1000);

                            if (fsize.ToString().Length > 3) {
                                fsize = fsize / 1000;
                                form.label_filesize.Text = fsize + " MB (" + fi.Length.ToString() + " byte)";
                            } else {
                                form.label_filesize.Text = fsize + " Kb (" + fi.Length.ToString() + " byte)";
                            }
                        }
                    }
                    //수신자 선택안한 경우
                } else if (form.txtbox_FileReceiver.Text.Length == 0) {
                
                    logWrite("btn_start_Click : 받는사람이 없음");
                    DialogResult result = MessageBox.Show(form, "파일을 받는 사람이 없습니다. 추가해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        AddFileReceiver(form.formkey.Text);
                    }
                }
                else
                {
                    logWrite("btn_start_Click : 전송할 파일과 받을사람 체크 완료");
                    form.btn_start.Visible = false;
                    form.btn_cancel.Left += 40;
                    form.label_result.Text = "전송 대기중";
                    string tempname = null;
                    string tempid = null;
                    ArrayList list = new ArrayList();
                    string[] receiverArray = form.txtbox_FileReceiver.Text.Split(';');

                    //전송 상태 자세히 보기 생성
                    FileSendDetailListView view = new FileSendDetailListView();
                    view.FormClosing += new FormClosingEventHandler(FileSendDetailListView_FormClosing);

                    foreach (string receiver in receiverArray)
                    {
                        if (receiver.Length != 0)
                        {
                            string[] receiverArg = receiver.Split('(');
                            tempname = receiverArg[0];
                            string[] receiverArg1 = receiverArg[1].Split(')');
                            tempid = receiverArg1[0];
                            list.Add(tempid);

                            ListViewItem item = view.listView.Items.Add(tempid, receiver, null);
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                        }
                    }
                    FileSendDetailList[form.formkey.Text] = view;
                    form.label_detail.Visible = true;
                    if (list.Count > 1)
                    {
                        view.Show();
                    }
                    FileSendRequest(list, form.label_filename.Tag.ToString(), form.formkey.Text);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void FileSendDetailListView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Form form = (Form)sender;
                form.Hide();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        /// <summary>
        /// 파일전송 대상자 추가
        /// </summary>
        /// <param name="formkey"></param>
        private void AddFileReceiver(string formkey) {
            
            try {
            
                //로그인사용자, 1명만 선택
                AddMemberForm addform = new AddMemberForm(true, formkey, false);
                addform.BtnConfirm.Click += new EventHandler(BtnConfirmForFile_Click);
                addform.radiobt_g.Click += new EventHandler(radiobt_g_Click);
                addform.radiobt_con.Click += new EventHandler(radiobt_con_Click);
                addform.radiobt_all.Click += new EventHandler(radiobt_all_Click);
                addform.combobox_team.SelectedValueChanged += new EventHandler(combobox_team_SelectedValueChangedAll);
                addform.CurrInListBox.MouseDoubleClick += new MouseEventHandler(CurrInListBox_MouseDoubleClick);

                addform.SetCurrInListBox(InList, this.memTree.Nodes);

                SendFileForm form = (SendFileForm)FileSendFormList[formkey];
                string[] receiverArray =  form.txtbox_FileReceiver.Text.Split(';');
                addform.SetAddListBox(receiverArray, memTree.Nodes);

                addform.ShowDialog(form);
            
            } catch (Exception e) {
                logWrite(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirmForFile_Click(object sender, EventArgs e)
        {
            try
            {
                AddMemberForm addMemberForm = ChatUtils.GetParentAddMemberForm((Button)sender);
                string key = addMemberForm.formkey.Text;

                SendFileForm form = (SendFileForm)FileSendFormList[key];

                ListBox box = addMemberForm.AddListBox;

                if (box.Items.Count != 0)
                {
                    if (form.txtbox_FileReceiver.Text.Length != 0)
                    {
                        form.txtbox_FileReceiver.Clear();
                    }
                    foreach (ListBoxItem item in box.Items)
                    {
                        string str = item.Text;
                        if (str.Length != 0)
                        {
                            form.txtbox_FileReceiver.AppendText(str + ";");
                        }
                    }
                }

                addMemberForm.Dispose();
                form.Activate();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void StripMn_gfile_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable list = new Hashtable();
                int sendable = 0;

                foreach (TreeNode node in memTree.SelectedNode.Nodes) {
                    
                    UserObject userObj = (UserObject)node.Tag;
                    if (userObj.Status != MsgrUserStatus.LOGOUT) {
                        list[userObj.Id] = userObj.Name;
                        sendable++;
                    }
                }

                if (sendable > 0) {
                    MakeSendFileForm(list);
                } else {
                    MessageBox.Show(this, "파일을 전송할 수 없습니다. 파일을 받을 사람이 로그인상태가 아닙니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void pbx_confirm_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (configform.cbx_autostart.CheckState == CheckState.Checked)
                {
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "autostart", "1");
                    setConfigXml(AppConfigFullPath, "autostart", "1");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("autostart", "1");
                    //RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(CommonDef.REG_CUR_USR_RUN, true);
                    //rkApp.SetValue("WeDo", Application.ExecutablePath.ToString());
                    rkApp.SetValue(AppRegName, Application.ExecutablePath.ToString());
                    rkApp.Close();
                }
                else
                {
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "autostart", "0");
                    setConfigXml(AppConfigFullPath, "autostart", "0");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("autostart", "0");
                    //RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(CommonDef.REG_CUR_USR_RUN, true);
                    if (rkApp.GetValue(AppRegName) != null)
                    {
                        rkApp.DeleteValue(AppRegName);
                    }
                    rkApp.Close();
                }

                if (configform.cbx_topmost.CheckState == CheckState.Checked)
                {
                    this.TopMost = true;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "topmost", "1");
                    setConfigXml(AppConfigFullPath, "topmost", "1");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("topmost", "1");
                }
                else
                {
                    this.TopMost = false;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "topmost", "0");
                    setConfigXml(AppConfigFullPath, "topmost", "0");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("topmost", "0");
                }

                if (configform.cbx_nopop.CheckState == CheckState.Checked)
                {
                    this.nopop = true;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "nopop", "1");
                    setConfigXml(AppConfigFullPath, "nopop", "1");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("nopop", "1");
                }
                else
                {
                    this.nopop = false;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "nopop", "0");
                    setConfigXml(AppConfigFullPath, "nopop", "0");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("nopop", "0");
                }

                if (configform.cbx_nopop_outbound.CheckState == CheckState.Checked)
                {
                    this.nopop_outbound = true;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "nopop_outbound", "1");
                    setConfigXml(AppConfigFullPath, "nopop_outbound", "1");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("nopop_outbound", "1");
                }
                else
                {
                    this.nopop_outbound = false;
                    //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "nopop_outbound", "0");
                    setConfigXml(AppConfigFullPath, "nopop_outbound", "0");
                    System.Configuration.ConfigurationSettings.AppSettings.Set("nopop_outbound", "0");
                }

                configform.Close();
                configform = null;
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void cbx_topmost_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void cbx_autostart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void btnSetting_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                logWrite("Set server ip 실행!");
                string tempip = "";
                if (setserverform.rbt_ip.Checked == true)
                {
                    tempip = setserverform.tbx_ip1.Text + "." + setserverform.tbx_ip2.Text + "." + setserverform.tbx_ip3.Text + "." + setserverform.tbx_ip4.Text;
                }
                else
                {
                    tempip = "localhost";
                }
                setConfigXml(AppConfigFullPath, "serverip", tempip);
                //-->setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "serverip", tempip);
                setCRM_DB_HOST(XmlConfigFullPath, tempip);
                //-->setCRM_DB_HOST("c:\\MiniCTI\\config\\MiniCTI_config_demo.xml", tempip);
                setCRM_DB_HOST(XmlConfigOrgFullPath, tempip);
                //-->setCRM_DB_HOST(Application.StartupPath + "\\MiniCTI_config_demo.xml", tempip);
                serverIP = tempip;
                System.Configuration.ConfigurationSettings.AppSettings.Set("serverip", serverIP);

                setserverform.Close();
                setserverform = null;

            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        

        private void btnSetting_Click(object sender, System.EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                SetServer_Form form = (SetServer_Form)button.Parent;
                int count = button.Parent.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    if ("tb_Address".Equals(button.Parent.Controls[i].Name))
                    {
                        serverIP = button.Parent.Controls[i].Text;
                        
                        break;
                    }
                }
                form.Dispose();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 로깅에 필요한 경로 생성
        /// 1. ./log, ./config 경로 생성
        /// 2. minicti config file 없을경우 백업본 복사
        /// 4. log file cleansing필요
        /// </summary>
        public void LogFileCheck()
        {
            try
            {
                if (!di.Exists)
                {
                    di.Create();
                }

                DirectoryInfo logDir = new DirectoryInfo(di.FullName + "\\log");
                if (!logDir.Exists)
                {
                    logDir.Create();
                }

                DirectoryInfo configDir = new DirectoryInfo(di.FullName + "\\config");
                if (!configDir.Exists)
                {
                    configDir.Create();
                }

                FileInfo CRMCFGfileinfo = new FileInfo(XmlConfigFullPath);//"C:\\MiniCTI\\config\\MiniCTI_config_demo.xml");
                if (!CRMCFGfileinfo.Exists)
                {
                    logWrite("MiniCTI config 파일 없음");
                    FileInfo temp = new FileInfo(XmlConfigOrgFullPath);//Application.StartupPath + "\\MiniCTI_config_demo.xml");
                    temp.CopyTo(CRMCFGfileinfo.FullName);
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
     
            }
        }

        /// <summary>
        /// 개인폴더 생성
        /// </summary>
        private void FileDirCheck()
        {
            try
            {
                privatefolder = new DirectoryInfo(@"C:\MiniCTI\" + id.Text);
                FilesDir = new DirectoryInfo(@"C:\MiniCTI\" + id.Text + "\\Files");
                if (!privatefolder.Exists)
                {
                    privatefolder.Create();
                    logWrite("개인 폴더 생성 : " + id.Text);
                }

                if (!FilesDir.Exists)
                {
                    FilesDir.Create();
                    logWrite("대화 저장 폴더 생성");
                }
            }
            catch (Exception e) { };
        }

        private void DialogFileCheck()
        {
            try
            {
                privatefolder = new DirectoryInfo(@"C:\MiniCTI\" + id.Text);
                dialogdi=new DirectoryInfo(@"C:\MiniCTI\" + id.Text+"\\Dialog");
                if (!privatefolder.Exists)
                {
                    privatefolder.Create();
                    logWrite("개인 폴더 생성 : " + id.Text);
                }

                if (!dialogdi.Exists)
                {
                    dialogdi.Create();
                    logWrite("대화 저장 폴더 생성");
                }

                string today = DateTime.Now.ToShortDateString();
                string month=today.Substring(0, 7);

                MonthFolder = new DirectoryInfo(@"C:\MiniCTI\"+id.Text+"\\Dialog\\" + month);
                if (!MonthFolder.Exists)
                {
                    MonthFolder.Create();
                    logWrite(month + " 폴더 생성");
                }

                DayFolder = new DirectoryInfo(@"C:\MiniCTI\" + id.Text + "\\Dialog\\" + month + "\\" + today);
                if (!DayFolder.Exists)
                {
                    DayFolder.Create();
                    logWrite(today + " 폴더 생성");
                }
            }
            catch (Exception e) { };
        }

        public void logWrite(string clog)
        {
            try
            {
                clog = string.Format("[{0}]{1}\r\n",DateTime.Now.ToString(),clog);
                
                if (Log.logBox.InvokeRequired)
                {
                    WriteLog writelog = new WriteLog(Log.logBox.AppendText);

                    Invoke(writelog, clog);
                }
                else Log.logBox.AppendText(clog);

                logFileWrite(clog);
            }
            catch (Exception e)
            {
                
            }
        }

        public void logFileWrite(string _log)
        {
            MsgrLogger.WriteLog(_log);
        }

        private void DialogFileWrite(string dialogkey, string dialog, string person)
        {
            try
            {
                if (dialog.Length < 1) return;

                string[] array=dialogkey.Split('!');
                StreamWriter sw = null;
                if (!dialogdi.Exists || MonthFolder == null || DayFolder == null)
                {
                    DialogFileCheck();
                }
                else if (!MonthFolder.Exists || !DayFolder.Exists)
                {
                    DialogFileCheck();
                }
                string today = DateTime.Now.ToShortDateString();
                string now = DateTime.Now.Hour.ToString() +"시_"+ DateTime.Now.Minute.ToString() +"분_"+ DateTime.Now.Second.ToString()+"초";
                string dkey = now + "!" + person;
                string month = today.Substring(0, 7);
                logWrite("DialogFileWrite dkey = " + dkey);
                FileInfo path = new FileInfo("C:\\MiniCTI\\" + this.myid + "\\Dialog\\" + month + "\\" + today + "\\" + dkey + ".dlg");
                sw = new StreamWriter(path.FullName, false);
                try
                {
                    sw.Write(dialog);
                    sw.Flush();
                    sw.Close();
                    logWrite("대화저장");
                }
                catch (Exception e)
                {
                    Console.WriteLine("logFileWriter() 에러 : " + e.ToString());
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        /// <summary>
        /// 메신저 클라이언트 리소스 정리
        /// </summary>
        private void closing()
        {
            if (receiverStart == true)
            {
                if (serverAlive == true)
                    connected = false;

                try
                {
                    if (listenSock != null)
                    {
                        listenSock.Close();
                        logWrite("listenSock 닫음");
                    }

                    if (sendSock != null)
                    {
                        sendSock.Close();
                        logWrite("sendSock 닫음");
                    }

                    if (filesock != null)
                    {
                        filesock.Close();
                        logWrite("filesock 닫음");
                    }

                    if (filesendSock != null)
                    {
                        filesendSock.Close();
                        logWrite("filesendSock 닫음");
                    }

                    if (FtpClientThreadList.Count != 0)
                    {
                        foreach (DictionaryEntry de in FtpClientThreadList)
                        {
                            if (de.Value != null)
                            {
                                ((FtpClientManager)de.Value).Close();
                            }
                        }
                        logWrite("FtpClientThreadList clear!");
                    }

                    if (FileReceiverThreadList.Count != 0)
                    {
                        foreach (DictionaryEntry de in FileReceiverThreadList)
                        {
                            if (de.Value != null)
                            {
                                ((Thread)de.Value).Abort();
                            }
                        }
                        logWrite("FileReceiverThreadList clear!");
                    }

                    if (FileSendThreadList.Count != 0)
                    {
                        foreach (DictionaryEntry de in FileSendThreadList)
                        {
                            if (de.Value != null)
                            {
                                ((Thread)de.Value).Abort();
                            }
                        }
                    }
                    if (receive != null)
                    {
                        if (receive.IsAlive == true)
                        {
                            receive.Abort();
                            receiverStart = false;
                            logFileWrite("receive Thread 종료!");
                        }
                    }
                    else
                    {
                        logWrite("receive Thread is null");
                    }

                    if (serverAlive == true)
                    {
                        if (checkThread != null)
                        {
                            if (checkThread.IsAlive == true)
                            {
                                checkThread.Abort();
                                logFileWrite("checkThread Thread 종료!");
                                checkThread = null;
                            }
                        }
                        else
                        {
                            logFileWrite("checkThread is null");
                        }
                    }
                    else
                    {
                        logFileWrite("serverAlive is false");
                    }
                }
                catch (ThreadAbortException ex)
                {
                    logFileWrite("closing() 에러 : " + ex.ToString());
                }
                catch (SocketException ex)
                {
                    logFileWrite("closing() 에러 : " + ex.ToString());
                }
            }
        }

        private void LogOut()
        {
            try
            {
                if (connected == true)
                {
                    string logout = MsgrMsg.LOGOUT + this.myid;
                    SendMsg(logout, server);

                    //열려있는 대화창 및 쪽지폼 확인 및 삭제
                    LogOutDelegate logoutFormClose = new LogOutDelegate(LogoutFormClose);
                    Invoke(logoutFormClose, null);

                    if (MemoTable.Count != 0)
                    {
                        foreach (MemoForm form in MemoTable)
                        {
                            form.Dispose();
                        }
                        MemoTable.Clear();
                    }
                    memTree.Nodes.Clear();
                    closing();
                    ButtonCtrl(false);
                    LogInPanelVisible(true);
                    defaultCtrl(true);
                    if (cbx_pass_save.Checked == false)
                    {
                        tbx_pass.Text = "";
                    }
                    label_stat.Text = "온라인 ▼";
                    id.Focus();
                    id.SelectAll();
                    if (serverAlive == true)
                    {
                        connected = false;
                    }
                }
                else
                {
                    logWrite("connected ==false");
                }
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        public void LogoutFormClose() { //로그아웃 전에 열린 폼 닫기 및 정보테이블 삭제
        
            //자원해제 대상
        //private Hashtable ChatFormList = new Hashtable();  //채팅창  key=id, value=chatform
        //private Hashtable MemoFormList = new Hashtable(); //key=time, value=SendMemoForm
        //private Hashtable TeamInfoList = new Hashtable(); //key=id, value=team
        //private Hashtable InList = new Hashtable();       //key=id, value=IPEndPoint
        //private Hashtable MemberInfoList = new Hashtable();
        //private Hashtable FileSendDetailList = new Hashtable();
        //private Hashtable FileSendFormList = new Hashtable();
        //private Hashtable FileSendThreadList = new Hashtable();
        //private Hashtable FileReceiverThreadList = new Hashtable();
        //private Hashtable NoticeDetailForm = new Hashtable();
        //private Hashtable treesource = new Hashtable();
        //private ArrayList MemoTable = new ArrayList();
        //private ArrayList omitteamlist = new ArrayList();
        
            try {
                if (noticelistform != null) {
                    noticelistform.Close();
                    noticelistform = null;
                }

                if (noticeresultform != null) {
                    noticeresultform.Close();
                    noticeresultform = null;
                }

                if (noreceiveboardform != null) {
                    noreceiveboardform.Close();
                    noreceiveboardform = null;
                }

                if (ChatFormList.Count != 0) {

                    foreach (DictionaryEntry de in ChatFormList) {
                        if (de.Value != null) {
                            try
                            {
                                ChatForm form = (ChatForm)de.Value;
                                form.Dispose();
                            }
                            catch (Exception e)
                            {
                                logWrite("form.Dispose() 에러 : " + e.ToString());
                            }
                        }
                    }
                    ChatFormList.Clear();
                    logWrite("ChatFormList Clear!");
                }

                if (MemoFormList.Count != 0)
                {
                    foreach (DictionaryEntry de in MemoFormList)
                    {
                        if (de.Value != null)
                        {
                            try
                            {
                                SendMemoForm form = (SendMemoForm)de.Value;
                                form.Dispose();
                            }
                            catch (Exception e)
                            {
                                logWrite("form.Dispose() 에러 : " + e.ToString());
                            }
                        }
                    }
                    MemoFormList.Clear();
                    logWrite("MemoFormList Clear!");
                }

                TeamInfoList.Clear();
                InList.Clear();
                MemberInfoList.Clear();
                FileSendDetailList.Clear();

                if (FileSendFormList.Count != 0)
                {
                    foreach (DictionaryEntry de in FileSendFormList)
                    {
                        if (de.Value != null)
                        {
                            try
                            {
                                SendFileForm form = (SendFileForm)de.Value;
                                form.Dispose();
                            }
                            catch (Exception e)
                            {
                                logWrite("form.Dispose() 에러 : " + e.ToString());
                            }
                        }
                    }
                    FileSendFormList.Clear();
                    logWrite("FileSendFormList Clear!");
                }

                FileSendThreadList.Clear();
                FileReceiverThreadList.Clear();
                FtpClientThreadList.Clear();

                if (NoticeDetailForm.Count != 0)
                {
                    foreach (DictionaryEntry de in NoticeDetailForm)
                    {
                        if (de.Value != null)
                        {
                            try
                            {
                                NoticeDetailResultForm form = (NoticeDetailResultForm)de.Value;
                                form.Dispose();
                            }
                            catch (Exception e)
                            {
                                logWrite("form.Dispose() 에러 : " + e.ToString());
                            }
                        }
                    }
                    NoticeDetailForm.Clear();
                    logWrite("NoticeDetailForm Clear!");
                }

                treesource.Clear();

                if (MemoTable.Count != 0)
                {
                    foreach (MemoForm memoform in MemoTable)
                    {
                        memoform.Dispose();
                    }
                    MemoTable.Clear();
                    logWrite("MemoTable Clear!");
                }

                omitteamlist.Clear();
            }
            catch (Exception e)
            {
                logWrite(e.ToString());
            }
        }

        private void Btnlogout_Click(object sender, EventArgs e)
        {
            logWrite("Btnlogout_Click !");
            try
            {
                if (ChatFormList.Count != 0)
                {
                    foreach (DictionaryEntry de in ChatFormList)
                    {
                        if (de.Value != null)
                        {
                            DialogResult result = MessageBox.Show(this, "로그아웃하면 현재 열려있는 폼이 모두 닫힙니다.", "알림", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result == DialogResult.OK)
                            {
                                LogOut();
                            }
                            break;
                        }
                    }
                }
                else LogOut();
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }



        private void StripMn_memo_Click(object sender, EventArgs e)
        {
            try {
                
                Hashtable MemoReceiver = new Hashtable();
                UserObject userObj = (UserObject)memTree.SelectedNode.Tag;
                MemoReceiver[userObj.Id] = userObj.Name;
                MakeSendMemo(MemoReceiver);

            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void MnExit_Click(object sender, EventArgs e) {
            QuitMsg();
        }

        private void QuitMsg() {
            bool isOk = true ;
            if (connected == true)
            {
                this.notifyIcon.Visible = false;
                LogOut();
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                this.notifyIcon.Visible = false;
                closing();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void StripMn_gmemo_Click(object sender, EventArgs e) {
            
            try {
                if (memTree.SelectedNode.GetNodeCount(false) != 0)  {
                    
                    Hashtable MemoReceiver = new Hashtable();

                    foreach (TreeNode node in memTree.SelectedNode.Nodes) {
                        
                        UserObject userObj = (UserObject)node.Tag;
                        MemoReceiver[userObj.Id] = userObj.Name;
                    }

                    MakeSendMemo(MemoReceiver);
                }
            } catch (Exception exception) {
                logWrite(exception.ToString());
            }
        }

        private void MnNotice_Click(object sender, EventArgs e) {
            MakeSendNotice();
        }

        //private void pic_connector_Click(object sender, EventArgs e)
        //{

        //    if (cbox_connector.CheckState == CheckState.Unchecked)
        //    {
        //        cbox_connector.CheckState = CheckState.Checked;
        //    }
        //    else
        //    {
        //        cbox_connector.CheckState = CheckState.Unchecked;
        //    }
        //}

        private void pic_noticelist_Click(object sender, EventArgs e) {
            SendMsg("1|" + this.myid, server);
        }

        /// <summary>
        /// 공지함 리스트 생성
        /// </summary>
        /// <param name="msg"></param>
        private void MakeNoticeListForm(string[] msg) {//L|time‡content‡mode‡sender‡seqnum‡title|...
        
            try {

                {
                    if (noticelistform != null)
                    {
                        noticelistform.Close();
                    }
                    noticelistform = new NoticeListForm();

                    noticelistform.listView.SelectedIndexChanged += new EventHandler(listView_Click);
                    noticelistform.KeyDown += new KeyEventHandler(noticelistform_KeyDown);
                    noticelistform.listView.KeyDown += new KeyEventHandler(noticelistform_KeyDown);
                    noticelistform.btn_del.MouseClick += new MouseEventHandler(btn_del_Click);

                    noticelistform.listView.CheckBoxes = true;
                    noticelistform.btn_del.Visible = true;
                    noticelistform.cancel.Visible = true;
                    noticelistform.btn_all.Visible = true;


                    foreach (string item in msg)
                    {
                        try
                        {
                            if (!item.Equals("L") && item.Length != 0)
                            {

                                string[] arr = null;

                                if (item.Split('‡').Length > 4)// '|' -> shift+\ 
                                {
                                    arr = item.Split('‡');  //time‡content‡mode‡sender‡seqnum‡title|...
                                }

                                ListViewItem listitem = null;

                                if (arr != null && arr.Length > 4)
                                {
                                    logWrite("notice_time = " + arr[0]);

                                    if (arr[2].Equals("n"))
                                    {
                                        listitem = noticelistform.listView.Items.Add(arr[0], "일반", null);
                                    }
                                    else
                                    {
                                        listitem = noticelistform.listView.Items.Add(arr[0], "긴급", null);
                                        listitem.ForeColor = Color.Red;
                                    }


                                    string sender = "";
                                    if (arr[3] != null)
                                    {
                                        sender = GetUserName(arr[3].Trim());

                                        if (arr[1].Contains("\n\r\n\r"))
                                        {
                                            logWrite("찾음");
                                            arr[1].Replace("♪", " ");
                                        }
                                    }

                                    listitem.SubItems.Add(arr[5].Trim());
                                    listitem.SubItems.Add(arr[1].Trim());
                                    listitem.SubItems.Add(sender + "(" + arr[3].Trim() + ")");
                                    listitem.SubItems.Add(arr[0]);
                                    listitem.Tag = arr[4];
                                    logWrite("seqnum = " + arr[4]);
                                    noticelistform.listView.ListViewItemSorter = new ListViewItemComparerDe(3);

                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            logWrite(ex1.ToString());
                        }
                    }
                }

                noticelistform.Show();
            }
            catch (Exception e)
            {
                logWrite("MakeNoticeListForm() " + e.StackTrace.ToString());
            }
        }

        private void cancel_MouseClick(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(selectcancelforNoticeList);
            Invoke(dele);
        }

        private void btn_all_MouseClickForNoticeList(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(selectAllforNoticeList);
            Invoke(dele);
        }

        private void selectAllforNoticeList()
        {
           ListView.ListViewItemCollection collection = noticelistform.listView.Items;
           foreach (ListViewItem item in collection)
           {
               item.Checked = true;
           }
        }

        private void selectcancelforNoticeList()
        {
            ListView.ListViewItemCollection collection = noticelistform.listView.Items;
            foreach (ListViewItem item in collection)
            {
                item.Checked = false;
            }
        }


        /// <summary>
        /// 관리자의 경우 선택된 공지사항 삭제버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_Click(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            NoticeListForm noticeListForm = ChatUtils.GetParentNoticeListForm(button);
            ListView view = noticeListForm.listView;
            
            string delnotices = "15|";
            if (view.CheckedItems.Count == 0)
            {
                MessageBox.Show(view.Parent, "삭제할 공지를 선택해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ListView.CheckedListViewItemCollection col = view.CheckedItems;
                foreach (ListViewItem item in col)
                {
                    logWrite(item.Tag.ToString());
                    delnotices += item.Tag.ToString() + ";";
                    view.Items.RemoveAt(item.Index);
                }
                SendMsg(delnotices, server);
                
            }
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            
            NoticeListSorting sorting = new NoticeListSorting(Noticelistform_Sorting);
            Invoke(sorting, new object[] { e.Column });
        }

        private void Noticelistform_Sorting(int columnindex)
        {
            ListView.ListViewItemCollection collection= noticelistform.listView.Items;

        }

        private void noticelistform_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
                {
                    FindTextForm form = new FindTextForm();
                    form.btn_find.Click += new EventHandler(btn_find_Click);
                    form.txtbox.KeyDown += new KeyEventHandler(txtbox_KeyDown);
                    form.Show(noticelistform);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string word = null;
                    TextBox box = (TextBox)sender;
                    word = box.Text;
                    ShowFindText(word);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int num = button.Parent.Controls.Count;
                string word = null;
                for (int i = 0; i < num; i++)
                {
                    if (button.Parent.Controls[i].Name.Equals("txtbox"))
                    {
                        TextBox box = (TextBox)button.Parent.Controls[i];
                        word = box.Text;
                    }
                }
                ShowFindText(word);
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }


        /// <summary>
        /// 공지사항 리스트에서 텍스트 검색기능
        /// </summary>
        /// <param name="word"></param>
        private void ShowFindText(string word)
        {
            try
            {
                logWrite("검색 시작");
                FindListForm form = new FindListForm();
                ListView.ListViewItemCollection col = noticelistform.listView.Items;
                int findnum = 0;
                foreach (ListViewItem item in col)
                {
                    TextBox box = new TextBox();
                    box.Text = item.SubItems[1].Text;
                    if (box.Text.Contains(word))
                    {
                        string date = item.SubItems[3].Text;
                        form.txtbox_result.AppendText("#################################\r\n\r\n");
                        form.txtbox_result.AppendText("공지일자 : <" + date + ">\r\n\r\n");
                        form.txtbox_result.AppendText(box.Text + "\r\n\r\n");
                        findnum++;
                    }
                }
                int indexnum = form.txtbox_result.Text.IndexOf(word);
                form.txtbox_result.Select(indexnum, word.Length);
                form.txtbox_result.KeyDown += new KeyEventHandler(txtbox_result_KeyDown);
                logWrite("찾은 갯수 : " + findnum.ToString());
                if (findnum == 0)
                {
                    MessageBox.Show("검색된 결과가 없습니다.", "결과없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    form.Show();
                    form.TopMost = true;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void txtbox_result_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TextBox box = (TextBox)sender;
                    string text = box.SelectedText;
                    int textlength = box.SelectionLength;
                    int newstart = textlength + box.SelectionStart;
                    newstart = box.Text.IndexOf(text, newstart);
                    if (newstart == -1)
                    {
                        newstart = box.Text.IndexOf(text, 0);
                    }
                    box.DeselectAll();
                    box.Select(newstart, textlength);
                    box.ScrollToCaret();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        //공지사항 목록 아이템 선택
        private void listView_Click(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                if (view.SelectedItems.Count != 0)
                {
                    ListViewItem mitem = view.SelectedItems[0];
                    string temp = mitem.SubItems[3].Text; //보낸사람
                    string[] ar1 = temp.Split('(');
                    string[] ar2 = ar1[1].Split(')');
                    string name = ar1[0];   
                    string id = ar2[0];
                    string msg = mitem.SubItems[2].Text; //내용
                    string mode = mitem.SubItems[0].Text; 
                    string ntime = mitem.SubItems[4].Text;
                    string seqnum = mitem.Tag.ToString();
                    string title = mitem.SubItems[1].Text;
                    string[] array = new string[] { "r", msg, id, mode, ntime, seqnum, title };  //n|메시지|발신자id|mode|title
                    ShowNotice(array);
                    noticelistform.TopMost = false;
                    mitem.Selected = false;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void pic_memolist_Click(object sender, EventArgs e)
        {
            MakeMemoList();
        }

        /// <summary>
        /// 쪽지함 리스트 폼 생성
        /// </summary>
        private void MakeMemoList()
        {
            try
            {
               
                ArrayList list = MemoUtils.MemoFileRead(this.myid);
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("저장된 쪽지가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (mMemoListForm != null)
                    {
                        mMemoListForm.Close();
                    }
                    mMemoListForm = new MemoListForm();
                    
                    mMemoListForm.listView.SelectedIndexChanged += new EventHandler(memolistView_Click);
                    mMemoListForm.btn_del.MouseClick += new MouseEventHandler(btn_del_Click_forMemo);

                    string source = "";
                    foreach (object obj in list)
                    {
                        if (((string)obj).Length > 2 && ((string)obj).Substring(0, 2) == MEMO_HEADER)
                        {
                            source = (string)obj;  //item(m|name|id|message|수신사id|time)
                        }
                        else
                        {
                            source += Environment.NewLine +(string)obj;  //item(m|name|id|message|수신사id|time)
                        }
                        if (source.Split('|').Length < 6) continue; 

                        logWrite("memo[" + source + "]");
                        
                        //> 2 && source.Substring(0,2) == MEMO_HEADER ) 

                        if (source.Length != 0)
                        {
                            string[] subitems = source.Split('|');
                            ListViewItem item = mMemoListForm.listView.Items.Add(subitems[5]);
                            item.SubItems.Add(subitems[1] + "(" + subitems[2] + ")");
                            item.SubItems.Add(subitems[3]);
                            item.Tag = source;
                        }
                        source = "";
                    }
                    mMemoListForm.Show();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_all_MouseClickForMemoListForm(object sender, EventArgs e)
        {
            NoParamDele dele = new NoParamDele(selectAllForMemoList);
            Invoke(dele);
        }

        private void btn_cancel_MouseClickForMemoListForm(object sender, EventArgs e)
        {
            NoParamDele dele = new NoParamDele(selectCancelForMemoList);
            Invoke(dele);
        }

        private void selectAllForMemoList()
        {
           ListView.ListViewItemCollection collection = mMemoListForm.listView.Items;
           foreach (ListViewItem item in collection)
           {
               item.Checked = true;
           }
        }

        private void selectCancelForMemoList()
        {
            ListView.ListViewItemCollection collection = mMemoListForm.listView.Items;
            foreach (ListViewItem item in collection)
            {
                item.Checked = false;
            }
        }

        private void btn_del_Click_forMemo(object sender, MouseEventArgs e)
        {
            ListView view = mMemoListForm.listView;
            ListView.CheckedListViewItemCollection col = view.CheckedItems;
            if (col.Count != 0)
            {
                foreach (ListViewItem item in col)
                {
                    string time = item.SubItems[0].Text;
                    time = time.Substring(0, 10);
                    bool del_success = MemoUtils.DelMemo(this.myid, item.Tag.ToString(), time);
                    if (del_success == true)
                    {
                        view.Items.RemoveAt(item.Index);
                    }
                }
            }
        }

        private void memolistView_Click(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                if (view.SelectedItems.Count != 0)
                {
                    string source = (string)view.SelectedItems[0].Tag;
                    string[] memo = source.Split('|');
                    MakeMemo(memo);
                    view.SelectedItems[0].Selected = false;
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void Client_Form_SizeChanged(object sender, EventArgs e)
        {

            int rightgap = this.Width - 290;

            webBrowser1.Width = rightgap + 260;


            int heightgap = this.Height - 600;


            webBrowser1.SetBounds(webBrowser1.Left, 435 + heightgap, webBrowser1.Width, webBrowser1.Height);
            pictureBox2.SetBounds(pictureBox2.Left, 430 + heightgap, pictureBox2.Width, pictureBox2.Height);//임시이미지판
            panel_logon.Width = this.Width;
            panel_logon.Height = this.Height - (600-519);
            memTree.Width = this.Width - (290-220);
            memTree.Height = this.Height - (600-325);

            InfoBar.Width = this.Width;

        }

        private void StripMn_Quit_Click(object sender, EventArgs e)
        {
            QuitMsg();
        }

        private void btn_cancel_MouseClick(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(dialogSelectCancel);
            Invoke(dele);
        }

        private void dialogSelectCancel()
        {
            ListView.ListViewItemCollection collection = mDialogListForm.listView.Items;
            foreach (ListViewItem item in collection)
            {
                item.Checked = false;
            }
        }

        private void btn_all_MouseClick(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(dialogSelectAll);
            Invoke(dele);
        }

        private void dialogSelectAll()
        {
            ListView.ListViewItemCollection collection = mDialogListForm.listView.Items;
            foreach (ListViewItem item in collection)
            {
                item.Checked = true;
            }
        }

        private void btn_del_Click_forDialog(object sender, MouseEventArgs e)
        {
            try
            {
                ListView view = mDialogListForm.listView;
                ListView.CheckedListViewItemCollection col = view.CheckedItems;
                if (col.Count != 0)
                {
                    foreach (ListViewItem item in col)
                    {
                        FileInfo tempfi = (FileInfo)item.Tag;
                        tempfi.Delete();
                        view.Items.RemoveAt(item.Index);
                    }
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void DialogLisForm_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = true;
        }

        private void DialoglistView_Click(object sender, EventArgs e)
        {
            try
            {
                ListView view = (ListView)sender;
                if (view.SelectedItems.Count != 0)
                {
                    ListViewItem item = view.SelectedItems[0];
                    FileInfo fi = (FileInfo)item.Tag;
                    logWrite(fi.Name);
                    StreamReader sr = fi.OpenText();
                    DialogContent form = new DialogContent();
                    string dialogstr = "";
                    while (!sr.EndOfStream)
                    {
                        dialogstr += sr.ReadLine()+"\r\n";
                    }
                    string title = item.SubItems[1].Text;
                    form.setContentInfo(title, dialogstr);
                    form.Show(mDialogListForm);
                    item.Selected = false;
                    sr.Close();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void pic_notice_Click(object sender, EventArgs e)
        {
            MakeSendNotice();
        }

        private void StripMn_show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }


        #region cbox_connector_CheckStateChanged...
        /// <summary>
        /// 사용안함....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cbox_connector_CheckStateChanged(object sender, EventArgs e)
        //{
        //    logWrite("cbox_connector_CheckStateChanged 실행!");
        //    try
        //    {
        //        if (cbox_connector.CheckState == CheckState.Checked)
        //        {
        //            try
        //            {
        //                TreeNodeCollection col1 = memTree.Nodes[0].Nodes;
        //                foreach (TreeNode node in col1)
        //                {
        //                    ArrayList list = new ArrayList();
        //                    TreeNodeCollection col2 = node.Nodes;
        //                    foreach (TreeNode mnode in col2)
        //                    {
        //                        list.Add(mnode.Tag.ToString());
        //                    }
        //                    foreach (object obj in list)
        //                    {
        //                        string tag = (string)obj;
        //                        TreeNode[] cnode = node.Nodes.Find(tag, false);
        //                        if (cnode[0].ImageIndex == 0) cnode[0].Remove();
        //                    }
        //                }
        //                memTree.ExpandAll();
        //            }
        //            catch (Exception ex1)
        //            {
        //                logWrite("cbox_connector_CheckStateChanged() 에러" + ex1.ToString());
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                ArrayList list = new ArrayList();
        //                TreeNodeCollection col = memTree.Nodes[0].Nodes;
        //                foreach (TreeNode node in col)
        //                {
        //                    if (node.IsExpanded == true)
        //                    {
        //                        list.Add(node.Text);
        //                    }
        //                }

        //                Hashtable table = (Hashtable)memTree.Tag;

        //                foreach (DictionaryEntry de in table)  //table(key=(string)팀이름, value=(string[])팀구성원)
        //                {
        //                    string tempTeamName = (string)de.Key;
        //                    ArrayList arraylist = (ArrayList)de.Value;
        //                    int nodeNum = memTree.Nodes[0].GetNodeCount(false);
        //                    TreeNode node = null;
        //                    if (tempTeamName.Length != 0)
        //                    {
        //                        if (!memTree.Nodes[0].Nodes.ContainsKey(tempTeamName))
        //                        {
        //                            node = memTree.Nodes[0].Nodes.Add(tempTeamName, tempTeamName);   //팀노드 추가
        //                            node.ImageIndex = 2;
        //                            node.SelectedImageIndex = 2;
        //                        }
        //                        if (arraylist != null && arraylist.Count != 0)
        //                        {
        //                            foreach (object obj in arraylist)  //list[] {id!name}
        //                            {
        //                                string m = (string)obj;
        //                                if (m.Length != 0)
        //                                {
        //                                    string[] arg = m.Split('!');
        //                                    if (!arg[1].Equals(this.myname))
        //                                    {
        //                                        TreeNode[] nodes = memTree.Nodes[0].Nodes.Find(tempTeamName, false);
        //                                        if (!nodes[0].Nodes.ContainsKey(arg[0]))
        //                                        {
        //                                            TreeNode tempNode = nodes[0].Nodes.Add(arg[0], arg[1]);   //사용자 노드 추가(노드 key=id, value=name)
        //                                            tempNode.ToolTipText = arg[0]; //MouseOver일 경우 나타남 
        //                                            tempNode.Tag = arg[0];
        //                                            tempNode.ImageIndex = 0;
        //                                            tempNode.SelectedImageIndex = 0;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (!memTree.Nodes[0].IsExpanded) memTree.Nodes[0].Expand();
        //                    if (team.Equals(this.team.Text)) node.Expand(); 

        //                }
        //                foreach (object obj in list)
        //                {
        //                    string nodetext = (string)obj;
        //                    TreeNodeCollection col1 = memTree.Nodes[0].Nodes;
        //                    foreach (TreeNode node in col1)
        //                    {
        //                        if (node.Text.Equals(nodetext))
        //                        {
        //                            node.Expand();
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex2)
        //            {
        //                logWrite("cbox_connector_CheckStateChanged() 에러" + ex2.ToString());
        //            }
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        logWrite(exception.ToString());
        //    }
        //}
        #endregion


        public void LoadXml()
        {
            try
            {
                xmldoc.Load("MsgCfg.xml");
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void saveSvrIP(string svrip)
        {
            XmlNode node = xmldoc.SelectSingleNode("//appcfg");
            if (node.HasChildNodes)
            {
                XmlNodeList nodelist = node.ChildNodes;
                foreach (XmlNode itemNode in nodelist)
                {
                    if (itemNode.Attributes["key"].Value.Equals("serverip"))
                    {
                        itemNode.Attributes["value"].Value = svrip;
                        break;
                    }
                }
            }
            xmldoc.Save("MsgCfg.xml");
            System.Configuration.ConfigurationSettings.AppSettings.Set("serverip", svrip);
            serverIP = svrip;
        }

        private void CheckXml()
        {
            try
            {
                xmldoc.Load("MsgCfg.xml");
                XmlNode node = xmldoc.SelectSingleNode("//teamcfg");
                if (node.HasChildNodes)
                {
                    XmlNodeList nodelist = node.ChildNodes;
                    foreach (XmlNode itemNode in nodelist)
                    {
                        if (itemNode.Attributes["visible"].Value.Equals("false"))
                        {
                            omitteamlist.Add(itemNode.Attributes["tname"].Value);
                        }
                    }
                    if (omitteamlist.Count == 0)
                    {
                        node.RemoveAll();
                    }
                }
                xmldoc.Save("MsgCfg.xml");
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private XmlElement MakeXmlNode(string nodename)
        {
            XmlNode node = xmldoc.SelectSingleNode("/root");
            XmlElement element = xmldoc.CreateElement(nodename);
            node.AppendChild(element);
            return element;

        }

        private XmlNodeList GetNodeList(string nodename)
        {
            XmlNode node = xmldoc.SelectSingleNode("/root/" + nodename);
            XmlNodeList list = null;
            if (node.HasChildNodes)
            {
                list = node.ChildNodes;
            }
            return list;
        }

        private void pbx_login_MouseClick(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(checkInfoForLogin);
            Invoke(dele);
        }

        /// <summary>
        /// collapsed된 아이콘 모양
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memTree_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 3;
            e.Node.SelectedImageIndex = 3;
        }

        /// <summary>
        /// expanded된 아이콘 모양
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 2;
            e.Node.SelectedImageIndex = 2;
        }

        private void pbx_sizemark_MouseDown(object sender, MouseEventArgs e)
        {
             if (e.Button == MouseButtons.Left)
            {
                mousePoint = e.Location;
            }
        }

        private void pbx_sizemark_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int xdifference = e.X - mousePoint.X;
                int ydifference = e.Y - mousePoint.Y;

                this.Width = 285 + xdifference;
                this.Height = 418 + ydifference;
            }
        }

        private void Mn_extension_Click(object sender, EventArgs e)
        {
            NoParamDele npdele = new NoParamDele(makeExtensionForm);
            Invoke(npdele);
        }

        private void makeExtensionForm()
        {
            try
            {
                if (extform == null)
                {
                    extform = new SetExtensionForm();
                    if (extension != null && extension.Length > 0)
                    {
                        extform.tbx_extension.Text = extension;
                    }
                    extform.tbx_extension.KeyDown += new KeyEventHandler(tbx_extension_KeyDown);
                    extform.btn_ext_confirm.MouseClick += new MouseEventHandler(btn_ext_confirm_MouseClick);
                }
                extform.ShowDialog();
                extform.tbx_extension.Focus();

            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void btn_ext_confirm_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (extform.tbx_extension.Text.Trim().Length > 0)
                {
                    extension = extform.tbx_extension.Text.Trim();
                    extform.Close();
                }
                if (connected == false)
                {
                    checkInfoForLogin();
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }  
        }

        private void tbx_extension_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (extform.tbx_extension.Text.Trim().Length > 0)
                    {
                        extension = extform.tbx_extension.Text.Trim();
                        extform.Close();
                    }
                    if (connected == false)
                        checkInfoForLogin();
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void tbx_pass_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    NoParamDele dele = new NoParamDele(checkInfoForLogin);
                    Invoke(dele);
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void checkInfoForLogin()
        {
            if (id.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "아이디를 입력해 주세요", "알림", MessageBoxButtons.OK);
                id.Focus();
            }
            else if (tbx_pass.Text.Trim().Length == 0)
            {
                logWrite("아이디 체크 완료!");
                MessageBox.Show(this, "비밀번호를 입력해 주세요", "알림", MessageBoxButtons.OK);
                tbx_pass.Focus();
            }
            else if (extension == null || extension.Length == 0)
            {
                logWrite("비밀번호 체크 완료!");
                MessageBox.Show(this, "내선번호를 설정해 주세요", "알림", MessageBoxButtons.OK);
                makeExtensionForm();
            }
            else
            {
                this.myid = id.Text.ToLower().Trim();
                this.mypass = tbx_pass.Text.Trim();
                StartService();
            }
        }

        private void label_stat_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseMenuStat.Show(label_stat, e.Location, ToolStripDropDownDirection.BelowRight);
            }
        }

        private void StripMn_online_Click(object sender, EventArgs e)
        {
            stringDele dele = new stringDele(changeMyStat);
            Invoke(dele, "온라인");
            string statmsg = "20|" + this.myid + "|online";
            SendMsg(statmsg, server);
        }

        private void StringMn_away_Click(object sender, EventArgs e)
        {
            stringDele dele = new stringDele(changeMyStat);
            Invoke(dele, "자리비움");
            string statmsg = "20|" + this.myid + "|away";
            SendMsg(statmsg, server);
        }

        private void StripMn_logout_Click(object sender, EventArgs e)
        {
            stringDele dele = new stringDele(changeMyStat);
            Invoke(dele, "오프라인 표시");
            string statmsg = "20|" + this.myid + "|logout";
            SendMsg(statmsg, server);
        }

        private void StripMn_DND_Click(object sender, EventArgs e)
        {
            stringDele dele = new stringDele(changeMyStat);
            Invoke(dele, "다른용무중");
            string statmsg = "20|" + this.myid + "|DND";
            SendMsg(statmsg, server);
        }

        private void StripMn_busy_Click(object sender, EventArgs e)
        {
            stringDele dele = new stringDele(changeMyStat);
            Invoke(dele, "통화중");
            string statmsg = "20|" + this.myid + "|busy";
            SendMsg(statmsg, server);
        }

        private void changeMyStat(string statname)
        {
            label_stat.Text = statname + " ▼";
            switch (statname)
            {
                case "자리비움":
                    pbx_stat.Image = global::Client.Properties.Resources.부재중;
                    break;

                case "오프라인 표시":
                    pbx_stat.Image = global::Client.Properties.Resources.로그아웃;
                    break;

                case "다른용무중" :
                    pbx_stat.Image = global::Client.Properties.Resources.다른용무중;
                    break;

                case "통화중" :
                    pbx_stat.Image = global::Client.Properties.Resources.통화중;
                    break;
                    
                case "온라인":
                    pbx_stat.Image = global::Client.Properties.Resources.온라인;
                    break;
            }
        }


        private void Mn_default_Click(object sender, EventArgs e)
        {
            try
            {
                string autostart = System.Configuration.ConfigurationSettings.AppSettings["autostart"];
                string topmost = System.Configuration.ConfigurationSettings.AppSettings["topmost"];
                if (configform != null)
                {
                    configform.Close();
                }
                configform = new SetAutoStartForm();
                configform.pbx_confirm.MouseClick += new MouseEventHandler(pbx_confirm_MouseClick);

                if (autostart.Equals("1"))
                {
                    configform.cbx_autostart.Checked = true;
                }

                if (topmost.Equals("1"))
                {
                    configform.cbx_topmost.Checked = true;
                }

                if (nopop == true)
                {
                    configform.cbx_nopop.Checked = true;
                }

                if (nopop_outbound == true)
                {
                    configform.cbx_nopop_outbound.Checked = true;
                }

                configform.ShowDialog();

            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void Mn_server_Click(object sender, EventArgs e)
        {
            NoParamDele dele = new NoParamDele(makeSetServerForm);
            Invoke(dele);
        }

        private void makeSetServerForm()
        {
            try
            {
                if (setserverform != null)
                {
                    setserverform.Close();
                    setserverform = null;
                }
                setserverform = new SetServer_Form();
                setserverform.btnSetting.MouseClick += new MouseEventHandler(btnSetting_MouseClick);
                serverIP = System.Configuration.ConfigurationSettings.AppSettings["serverip"];
                string[] iparr = null;

                if (serverIP.Equals("localhost"))
                {
                    setserverform.rbt_local.Checked = true;
                }
                else
                {
                    setserverform.rbt_ip.Checked = true;
                    iparr = serverIP.Split('.');
                    if (iparr.Length > 3)
                    {
                        setserverform.tbx_ip1.Text = iparr[0];
                        setserverform.tbx_ip2.Text = iparr[1];
                        setserverform.tbx_ip3.Text = iparr[2];
                        setserverform.tbx_ip4.Text = iparr[3];
                    }
                    else
                    {
                        logWrite("serverIP 값이 올바르지 않음 : " + serverIP);
                    }
                }
                setserverform.ShowDialog();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void cbx_pass_save_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_pass_save.Checked == false)
            {
                uncheckPass_save();
            }
        }

        private void uncheckPass_save()
        {
            try
            {
                System.Configuration.ConfigurationSettings.AppSettings.Set("save_pass", "0");
                //setConfigXml(Application.StartupPath + "\\WDMsg_Client_Demo.exe.config", "save_pass", "0");
                setConfigXml(AppConfigFullPath, "save_pass", "0");
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void Mn_notify_dispose_Click(object sender, EventArgs e)
        {
            QuitMsg();
        }

        private void Mn_notify_show_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }

        private void MnFile_MouseClick(object sender, MouseEventArgs e)
        {
            TM_file_sub.Show(new Point((this.Left), (this.Top + 48)), ToolStripDropDownDirection.BelowRight);
        }

        private void MnMotion_MouseClick(object sender, MouseEventArgs e)
        {
            TM_motion_sub.Show(new Point((this.Left + 60), (this.Top + 48)), ToolStripDropDownDirection.BelowRight);
        }

        private void MnOption_MouseClick(object sender, MouseEventArgs e)
        {
            TM_option_sub.Show(new Point((this.Left + 120), (this.Top + 48)), ToolStripDropDownDirection.BelowRight);
        }

        private void MnHelp_MouseClick(object sender, MouseEventArgs e)
        {
            TM_help_sub.Show(new Point((this.Left + 180), (this.Top + 48)), ToolStripDropDownDirection.BelowRight);
        }

        private void btn_crm_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CRMUtils.DisplayCrmPopUp(crm_main);
            }
            catch (System.ObjectDisposedException dis)
            {
                try
                {
                    cm.SetUserInfo(this.com_cd, this.myid, tbx_pass.Text, serverIP, socket_port_crm);
                    crm_main = new FRM_MAIN();
                    crm_main.FormClosing += new FormClosingEventHandler(crm_main_FormClosing);
                    CRMUtils.DisplayCrmPopUp(crm_main);
                }
                catch (Exception ex1)
                {
                    logWrite(ex1.ToString());
                }
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void btn_board_MouseClick(object sender, MouseEventArgs e)
        {
            SendMsg("1|" + this.myid, server);  //공지게시판 조회
        }

        private void btn_memobox_MouseClick(object sender, MouseEventArgs e)
        {
            MakeMemoList();
        }

        private void MakeDialogueboxList()
        {
            try
            {
                ArrayList list = new ArrayList();

                if (!dialogdi.Exists)
                {
                    DialogFileCheck();
                }
                else
                {
                    DirectoryInfo[] diarray = dialogdi.GetDirectories(); //월별폴더 검색

                    foreach (DirectoryInfo tempdi in diarray)
                    {
                        DirectoryInfo[] diarray1 = tempdi.GetDirectories();    //일별폴더 검색
                        foreach (DirectoryInfo tempdi1 in diarray1)
                        {
                            FileInfo[] fiarray = tempdi1.GetFiles("*.dlg");
                            foreach (FileInfo tempfi in fiarray)
                            {
                                list.Add(tempfi);
                            }
                        }

                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("저장된 대화기록이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (mDialogListForm != null)
                    {
                        mDialogListForm.Close();
                    }
                    mDialogListForm = new DialogListForm();

                    mDialogListForm.listView.SelectedIndexChanged += new EventHandler(DialoglistView_Click);
                    mDialogListForm.btn_del.MouseClick += new MouseEventHandler(btn_del_Click_forDialog);
                    
                    foreach (object obj in list)
                    {
                        FileInfo tempfi = (FileInfo)obj;
                        string fname = tempfi.Name;
                        string[] temparray = fname.Split('!');
                        ListViewItem item = mDialogListForm.listView.Items.Add(tempfi.Directory.Name + " " + temparray[0]);
                        string[] array = temparray[1].Split('.');//파일 확장자명 제거
                        //string tempname = getName(array[0]);
                        item.SubItems.Add(array[0]);
                        item.Tag = tempfi;
                    }
                    mDialogListForm.Show();
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_dialoguebox_MouseClick(object sender, MouseEventArgs e)
        {
            MakeDialogueboxList();
        }

        private void btn_sendnotice_MouseClick(object sender, MouseEventArgs e)
        {
            MakeSendNotice();
        }

        private void btn_resultnotice_MouseClick(object sender, MouseEventArgs e)
        {
            MakeNoticeResultList();
        }

        private void MakeNoticeResultList()
        {
            try
            {
                if (isMadeNoticeResult != false)
                {
                    noticeresultform.Show();
                    SendMsg("13|" + this.myid, server);
                }
                else
                {
                    SendMsg("13|" + this.myid, server);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_login_MouseClick(object sender, MouseEventArgs e)
        {
            NoParamDele dele = new NoParamDele(checkInfoForLogin);
            Invoke(dele);
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://jmtech.co.kr/");
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            tooltip.Show("재민정보통신 홈페이지로 바로가기", pictureBox2, 3000);
        }

        private void name_MouseClick(object sender, MouseEventArgs e)
        {
            SetUserInfo();
        }

        private void NRmemo_Click(object sender, MouseEventArgs e)
        {
            try
            {
                //안읽은 메모 요청
                if (!NRmemo.Text.Equals("0"))
                {
                    if (noreceiveboardform == null)
                    {
                        string msg = MsgrMsg.REQ_UNREAD_MEMO + this.myid;
                        SendMsg(msg, server);
                    }
                    else
                    {
                        noreceiveboardform.Show();
                    }
                }
                else
                {
                    MessageBox.Show("부재중 등록된 메모가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void pic_NRnotice_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                ////안읽은 공지 요청(11|id)
                if (!NRnotice.Text.Equals("0"))
                {
                    if (noreceiveboardform == null)
                    {
                        string msg = MsgrMsg.REQ_UNREAD_NOTICE + this.myid;
                        SendMsg(msg, server);
                    }
                    else
                    {
                        noreceiveboardform.Show();
                    }
                }
                else
                {
                    MessageBox.Show("부재중 등록된 공지가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void pic_NRfile_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //안받은 파일 요청
                if (!NRfile.Text.Equals("0"))
                {
                    if (noreceiveboardform == null)
                    {
                        string msg = MsgrMsg.REQ_UNRECEIVED_FILE + this.myid;
                        SendMsg(msg, server);
                    }
                    else
                    {
                        noreceiveboardform.Show();
                    }
                }
                else
                {
                    MessageBox.Show("부재중 수신된 파일내역이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void Client_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                mainform_width = this.Width;
                mainform_height = this.Height;
                mainform_x = this.Left;
                mainform_y = this.Top;
                this.Hide();
                //this.SetBounds(0, screenHeight, this.Width, this.Height);
                //this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                //this.TopMost = true;
                isHide = true;
            }
            else
            {
                if (connected == true)
                {
                    string logout = MsgrMsg.LOGOUT + this.myid;
                    SendMsg(logout, server);
                    logWrite(logout);
                    closing();
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    closing();
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void weDo정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                aboutform = new AboutForm();
                aboutform.lbl_version.Text = this.version;
                aboutform.ShowDialog();
            }
            catch (Exception ex)
            {
                logWrite(ex.ToString());
            }
        }

        private void pic_NRtrans_MouseClick(object sender, MouseEventArgs e)
        {
            try
            { 
                //안읽은 이관 요청
                if (!NRtrans.Text.Equals("0"))
                {
                    if (noreceiveboardform == null)
                    {
                        string msg = MsgrMsg.REQ_UNREAD_TRANSFERED + this.myid;
                        SendMsg(msg, server);
                    }
                    else
                    {
                        noreceiveboardform.Show();
                    }
                }
                else
                {
                    MessageBox.Show("부재중 등록된 이관내역이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception exception)
            {
                logWrite(exception.ToString());
            }
        }

        private void btn_crm_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("고객관리", btn_crm);
        }

        private void btn_board_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("공지게시판", btn_board);
        }

        private void btn_memobox_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("쪽지함", btn_memobox);
        }

        private void btn_dialoguebox_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("대화함", btn_dialoguebox);
        }

        private void btn_sendnotice_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("공지하기", btn_sendnotice);
        }

        private void btn_resultnotice_MouseEnter(object sender, EventArgs e)
        {
            tooltip.Show("공지결과 보기", btn_resultnotice);
        }

        private void UpperMemoToolTipShow(object sender, EventArgs e)
        {
            tooltip.Show("부재중 쪽지", (Control)sender);
        }

        private void UpperMemoToolTipHide(object sender, EventArgs e)
        {
            tooltip.Hide((Control)sender);
        }

        private void UpperNoticeToolTipShow(object sender, EventArgs e)
        {
            tooltip.Show("부재중 공지", (Control)sender);
        }

        private void UpperNoticeToolTipHide(object sender, EventArgs e)
        {
            tooltip.Hide((Control)sender);
        }

        private void UpperFileToolTipShow(object sender, EventArgs e)
        {
            tooltip.Show("부재중 파일", (Control)sender);
        }

        private void UpperFileToolTipHide(object sender, EventArgs e)
        {
            tooltip.Hide((Control)sender);
        }

        private void UpperTransToolTipShow(object sender, EventArgs e)
        {
            tooltip.Show("부재중 이관", (Control)sender);
        }

        private void UpperTransToolTipHide(object sender, EventArgs e)
        {
            tooltip.Hide((Control)sender);
        }

        private void label_stat_MouseEnter(object sender, EventArgs e)
        {
            labelColor = label_stat.ForeColor;
            label_stat.ForeColor = Color.DarkOrange;
        }

        private void label_stat_MouseLeave(object sender, EventArgs e)
        {
            label_stat.ForeColor = labelColor;
        }

        private void Client_Form_Activated(object sender, EventArgs e)
        {
            //getForegroundWindow();
        }

    }
}