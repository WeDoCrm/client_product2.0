using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Common
{
    class CallCtrlUtils
    {
        /// <summary>
        /// 파일 전송 전 파일 수신 전용 포트로 IPEndPoint 의 port 값 변경
        /// </summary>
        /// <param name="port">현재 파일 수신대상자 IPEndpoint의 port</param>
        /// <returns></returns>
        public static int getFileReceivePort(int port)
        {
            if (port == PortInfoServer.LICENSE_PORT || port == PortInfoServer.SEND_PORT) port = PortInfoServer.FILE_RCV_PORT;
            if (port == PortInfoMsgr.LISTEN_PORT || port == PortInfoMsgr.SEND_PORT) port = PortInfoMsgr.FILE_RCV_PORT;
            return port;
        }

        /// <summary>
        /// 메시지 전송 전 수신자의 수신전용 포트값으로 변경
        /// </summary>
        /// <param name="port">현재 메시지 수신자 IPEndPoint의 포트값</param>
        /// <returns></returns>
        public static int getListenPort(int port)
        {
            int returnPort = 0;
            if (port == PortInfoMsgr.SEND_PORT) returnPort = PortInfoMsgr.LISTEN_PORT;
            if (port == PortInfoServer.CRM_PORT) returnPort = PortInfoServer.CHECK_PORT;
            //if (port == 8884) listenport = 8883;
            //if (port == 8886) listenport = 8885;
            //if (port == 8888) listenport = 8887;
            return returnPort;
        }
    }


    class PortInfoServer
    {
        public static int LISTEN_PORT    = 8881;//listenport 수신전용
        public static int SEND_PORT      = 8882;//발신전용 수신측은 8883(클라이언트)으로 받음
        public static int CHECK_PORT     = 8885;//서버체크전용 <== 클라이언트 8886
        public static int CRM_PORT       = 8886; //CRM 서버 체크 전용 TCP 수신
        public static int FILE_RCV_PORT = 9001; //파일수신 전용 <==클라이언트 9004
        public static int LICENSE_PORT   = 5999; //
    }

    class PortInfoMsgr
    {
        public static int LISTEN_PORT    = 8883; //수신전용 <== 8884(클라이언트)
                                              //         <== 8882(위두서버)
        public static int SEND_PORT      = 8884; //발신전용 수신측은 8883으로 받음 
        public static int FILE_RCV_PORT  = 9003; //파일수신 <== 9004(클라이언트)
                                              //         <== 0   (위두서버/랜덤포트)
        public static int FILE_SEND_PORT = 9004; //파일발신
        public static int CHEKC_PORT     = 8886; //==> 8885(위두서버)

        public static int FILE_RCV_TCP_PORT = 9002; //파일수신 TCP
    }

}
