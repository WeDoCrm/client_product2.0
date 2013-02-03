namespace Client
{
    public class CommonDef
    {
        public const string STR_DEMO = "DEMO";

        public const string REG_APP_NAME = "WeDo";
        public const string REG_APP_NAME_DEMO = "WeDo Demo";
        
        public const string MSG_DEL = "|";
        public const string MSG_LOGIN = "8|";
        public const string MSG_CHAT = "16|";

        public const string PATH_DELIM = "\\";

        public const string WORK_DIR = "c:\\MiniCTI";

        public const string LOG_DIR = "c:\\MiniCTI\\log";
        public const string FTP_LOG_FILE = "FTP_";

        public const string CONFIG_DIR = "config";
        public const string APP_CONFIG_NAME = "{0}.exe.config";

        public const string XML_CONFIG_DEMO = "MiniCTI_config_demo.xml";
        public const string XML_CONFIG_PROD = "MiniCTI_config.xml";
        
        public const string REG_CUR_USR_RUN = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        
        public const string UPDATE_DIR_DEMO = "WeDoUpdater_Demo";
        public const string UPDATE_DIR_PROD = "WeDoUpdater";

        public const string UPDATE_EXE = "AutoUpdater.exe";

        public const string MSGR_TITLE_PROD = "WeDo 메신저";
        public const string MSGR_TITLE_DEMO = "WeDo 메신저 데모버젼";
//string AppConfigFileName = Application.StartupPath + ...
//string AppConfigFilePath = Application.StartupPath + ...
//string MiniCtiXmlConfigPath = 
//string AppXmlConfigPath = 

        
        public const int SOCKET_PORT_CRM = 8886;
        public const string FTP_LOCAL_DIR = "c:\\temp";
        public const string FTP_HOST_DEMO = "ftp://114.202.2.33/Update/demo_client/";
        public const string FTP_HOST_PROD = "ftp://114.202.2.33/Update/client/";
        public const int FTP_PORT = 21;
        public const string FTP_USERID = "eclues";
        public const string FTP_PASS = "eclues!@";
        public const string FTP_VERSION_DEMO = "2.1.54";
        public const string FTP_VERSION_PROD = "2.1.49";


        public const string CHAT_USER_LOG_IN = ":login";
        public const string CHAT_USER_LOG_OUT = ":logout";
    }

    public class MsgrMsg
    {
        public const string REQ_UNREAD_MEMO = "7|"; //안읽은 메모 요청
        public const string LOGOUT = "9|";//로그아웃
        public const string REQ_UNRECEIVED_FILE = "10|"; //안받은 파일 요청
        public const string REQ_UNREAD_NOTICE = "11|"; //안읽은 공지 요청(11|id)
        public const string REQ_UNREAD_TRANSFERED = "23|";//안읽은 이관 요청
    }

    public class SocketDef
    {
        public const int PORT_MSG_SVR_SVR = 8881;  //8881(listener on server) <--> 8882(client)
        public const int PORT_MSG_SVR_CLI = 8882;  
        public const int PORT_MSG_CLI_SVR = 8883;  //8883(listener on client) <--> 8884(client on client)
        public const int PORT_MSG_CLI_CLI = 8884;
        public const int PORT_CHECK_SENDER = 8886;  // 8886 (client on client) <--> 8885(listener on server)
        public const int PORT_CHECK_RECEIVER = 8885; //
        public const int PORT_FILE_CLI_SVR = 9003;
        public const int PORT_FILE_CLI_CLI = 9004;

        public const int PORT_FILE_SVR_SVR = 9001;

        public const int PORT_FILE_NEW_SERVER = 9002;
        //private int listenport = 8883;
        //private int sendport = 8884;
        //private int checkport = 8886;
        //private int filereceiveport = 9003;
        //private int filesendport = 9004;
    }

    public class DownloadStatus
    {
        public const int SUCCESS = 0;
        public const int FAILED = -1;
        public const int CANCELED = 1;
        public const int START = 2;
        public const int RECEIVING = 3;
        public const int END = 4;
        public const int SENDING = 5;
    }
}