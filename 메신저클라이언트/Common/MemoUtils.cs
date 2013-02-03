using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Client.Common
{
    class MemoUtils
    {
        public static void MemoFileWrite(string myId, string memo)
        {
            StreamWriter sw = null;
            try
            {
                string today = DateTime.Now.ToShortDateString();
                FileInfo memofile = new FileInfo(@"C:\MiniCTI\" + myId + "\\Memo\\" + today + ".mem");
                if (!memofile.Exists)
                {
                    MemoFileCheck(myId);
                }
                sw = memofile.AppendText();
                sw.WriteLine(memo);
                sw.Close();
                MsgrLogger.WriteLog("메모저장");
            }
            catch (Exception e)
            {
                MsgrLogger.WriteLog("logFileWriter() 에러 : " + e.ToString());
            }
        }

        public static ArrayList MemoFileRead(string myId)
        {
            StreamReader sr = null;
            ArrayList list = new ArrayList(); ;
            try
            {
                string today = DateTime.Now.ToShortDateString();
                DirectoryInfo info = new DirectoryInfo(@"C:\MiniCTI\" + myId + "\\Memo");
                if (!info.Exists)
                {
                    MemoFileCheck(myId);
                }
                else
                {
                    FileInfo[] files = info.GetFiles("*.mem");
                    foreach (FileInfo file in files)
                    {
                        sr = new StreamReader(file.FullName);
                        while (!sr.EndOfStream)
                        {
                            string memoitem = sr.ReadLine();
                            list.Add(memoitem);
                        }
                        sr.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                MsgrLogger.WriteLog(exception.ToString());
            }
            return list;
        }


        public static void MemoFileCheck(string myId)
        {
            DirectoryInfo privateFolder = new DirectoryInfo(@"C:\MiniCTI\" + myId);
            DirectoryInfo memoDir = new DirectoryInfo(@"C:\MiniCTI\" + myId + "\\Memo");
            try
            {
                if (!privateFolder.Exists)
                {
                    privateFolder.Create();
                    MsgrLogger.WriteLog("개인 폴더 생성 : " + myId);
                }
                if (!memoDir.Exists)
                {
                    memoDir.Create();
                    MsgrLogger.WriteLog("Memo 저장 폴더 생성");
                }
                string today = DateTime.Now.ToShortDateString();
                FileInfo memoFile = new FileInfo(@"C:\MiniCTI\" + myId + "\\Memo\\" + today + ".mem");
                if (!memoFile.Exists)
                {
                    memoFile.Create();
                    MsgrLogger.WriteLog(today + ".mem 파일 생성");
                }
            }
            catch (Exception e) { };
        }

        public static bool DelMemo(string myId, string source, string time)
        {
            DirectoryInfo info = new DirectoryInfo(@"C:\MiniCTI\" + myId + "\\Memo");
            StreamReader sr = null;
            bool isFind = false;
            if (!info.Exists)
            {
                MemoUtils.MemoFileCheck(myId);
            }
            else
            {
                FileInfo[] files = info.GetFiles("*.mem");
                foreach (FileInfo file in files)
                {
                    MsgrLogger.WriteLog(file.Name);
                    if (file.Name.Equals(time + ".mem"))
                    {
                        sr = new StreamReader(file.FullName);
                        string str = sr.ReadToEnd();
                        sr.Close();
                        if (str.Contains(source))
                        {
                            str = str.Remove(str.IndexOf(source), source.Length);
                            StreamWriter sw = new StreamWriter(file.FullName);
                            sw.Write(str);
                            sw.Close();
                            isFind = true;
                        }
                        break;
                    }
                }

                if (isFind == false)
                {
                    foreach (FileInfo file in files)
                    {
                        sr = new StreamReader(file.FullName);
                        string str = sr.ReadToEnd();
                        sr.Close();
                        if (str.Contains(source))
                        {
                            str = str.Remove(str.IndexOf(source), source.Length);
                            StreamWriter sw = new StreamWriter(file.FullName);
                            sw.Write(str);
                            sw.Close();
                            isFind = true;
                            break;
                        }
                    }
                }
            }
            return isFind;
        }

    }
}
