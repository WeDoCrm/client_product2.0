using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Client.PopUp;

namespace Client
{
    public partial class DownloadForm : FlashWindowForm
    {
        public event EventHandler<CustomEventArgs> DownloadYNChecked;
        public event EventHandler<CustomEventArgs> DownloadStatusChanged;

        bool mFileCanceled = false;
        //private string mFormKey;
        //private Hashtable mDownloadInfo;
        private DownloadInfo mDownloadInfo;
        //string mFilename;
        //int mFilesize;
        //string mMyId;
        //string mSenderName;
        //string mSenderId;

        public DownloadForm()
        {
            InitializeComponent();
        }

        //public void setFormKey(string key)
        //{
        //    mFormKey = key;
        //}

        public string getFormKey()
        {
            return mDownloadInfo.MsgFileKey;
        }

        public void ShowDialog(DownloadInfo info)
        {
            try
            {
                mDownloadInfo = info;
                //logWrite("DownloadForm:sender name=" + mDownloadInfo.SenderName);
                //logWrite("DownloadForm:sender id=" + mDownloadInfo.SenderId);
                //logWrite("DownloadForm:filename=" + mDownloadInfo.FileName);
                //logWrite("DownloadForm:filesize=" + mDownloadInfo.FileSize);
                //logWrite("DownloadForm:myid=" + mDownloadInfo.MyId);
                //logWrite("DownloadForm:formKey=" + mDownloadInfo.MsgFileKey);
                //logWrite("DownloadForm:DownloadPath=" + mDownloadInfo.DownloadPath);
                //logWrite("DownloadForm:SenderFileName=" + mDownloadInfo.SenderFileName);
            } 
            catch (Exception exception)
            {
                //logWrite(exception.ToString());
            }
            string[] filenameArray = mDownloadInfo.FileName.Split('\\');
            string shortFileName = filenameArray[filenameArray.Length - 1];;
            labelMainMessage.Text = string.Format("{0}[{1}]님이 파일을 보내려고 합니다.\n파일을 저장하시겠습니까?", 
                mDownloadInfo.SenderName, mDownloadInfo.SenderId);
            TextBoxSaveDir.Text = string.Format(@"C:\MiniCTI\{0}\Files", mDownloadInfo.MyId);
            labelFileName.Text = string.Format(labelFileName.Text, shortFileName);
            PanelFinishFileSave.Visible = false;
            setProgressVisible(false);
            this.Show();
        }

        private void SelectDir()
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = mDownloadInfo.DownloadPath;
            folderBrowserDialog1.ShowDialog();
            TextBoxSaveDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void AllowDownload()
        {
            ButtonSaveDir.Enabled = false;
            ButtonClose.Visible = true;
            ButtonSaveFile.Visible = false;
            ButtonCancelFile.Visible = false;
            PanelRunFileSave.Refresh();
            labelMainMessage.Text = "파일 받기를 진행합니다.";
            //start something
            mDownloadInfo.DownloadAccepted = true;
            DownloadYNChecked(this, new CustomEventArgs(mDownloadInfo));
        }

        public void ShowFileReceiveStatus(int stat, int idx)
        {
            switch (stat)
            {
                case DownloadStatus.START:
                    setProgressVisible(true);
                    ShowFlashWindow();
                    break;
                case DownloadStatus.RECEIVING:
                    ProgressBarFileRcv.Value = idx;
                    break;
                case DownloadStatus.END:
                    DisplaySaveResult();
                    ShowFlashWindow();
                    break;
                case DownloadStatus.FAILED:
                    DisplayErrorResult();
                    ShowFlashWindow();
                    break;
            }
        }

        public void ShowFlashWindow()
        {
            if (this.WindowState ==  FormWindowState.Minimized)
                DoFlashWindow();
        }

        private void RejectDownload()
        {
            this.Close();
            mDownloadInfo.DownloadAccepted = false;
            DownloadYNChecked(this, new CustomEventArgs(mDownloadInfo));
        }

        private void DisplaySaveResult()
        {
            this.Text = "파일받기완료";
            labelMainMessage.Text = "파일 받기가 완료되었습니다.";
            setProgressVisible(false);
            PanelRunFileSave.Visible = false;
            PanelFinishFileSave.Location = PanelRunFileSave.Location;
            PanelFinishFileSave.Visible = true;
            //clean resources
            DownloadStatusChanged(this, new CustomEventArgs(DownloadStatus.SUCCESS));
        }

        private void DisplayErrorResult()
        {
            this.Text = "파일받기실패";
            ButtonClose.Text = "확 인";
            labelMainMessage.Text = "전송오류로 파일 받기가 중단되었습니다.";
            mFileCanceled = true;
            setProgressVisible(false);
            //clean resources
            DownloadStatusChanged(this, new CustomEventArgs(DownloadStatus.FAILED));
        }

        private void CancelFile()
        {
            //FTP_Cancel
            this.Text = "파일받기취소";
            ButtonClose.Text = "확 인";
            labelMainMessage.Text = "파일 받기가 취소되었습니다.";
            mFileCanceled = true;
            setProgressVisible(false);
            //clean resources
            DownloadStatusChanged(this, new CustomEventArgs(DownloadStatus.CANCELED));
        }
        
        int mPanelTopOrg;

        private void setProgressVisible(bool visible)
        {
            ProgressBarFileRcv.Visible = visible;
            if (visible)
            {
                PanelRunFileSave.Top = mPanelTopOrg;
            }
            else
            {
                mPanelTopOrg = PanelRunFileSave.Top;
                PanelRunFileSave.Top = ProgressBarFileRcv.Top;
            }
        }

        private void ButtonSaveFile_Click(object sender, EventArgs e)
        {
            AllowDownload();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (!mFileCanceled)
            {
                CancelFile();
            }
            else
            {
                this.Close();
            }
        }

        private void ButtonCancelFile_Click(object sender, EventArgs e)
        {

            RejectDownload();
        }

        private void ButtonSaveDir_Click(object sender, EventArgs e)
        {
            SelectDir();
        }

        private void ButtonOpenDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (mDownloadInfo.FileName != null && mDownloadInfo.FileName.Trim() != "")
                {

                    FileInfo fileinfo = new FileInfo(mDownloadInfo.FileName);
                    string dirname = fileinfo.DirectoryName;
                    System.Diagnostics.Process.Start(dirname);
                }
            }
            catch (Exception exception)
            {
                //logWrite(exception.ToString());
            }
        }

        private void ButtonOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (mDownloadInfo.FileName != null && mDownloadInfo.FileName.Trim() != "")
                   System.Diagnostics.Process.Start(mDownloadInfo.FileName);
            }
            catch (Exception exception)
            {
                //logWrite(exception.ToString());
            }
        }
    }
}
