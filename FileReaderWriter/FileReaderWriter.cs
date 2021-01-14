using System;
using System.IO;
using System.Text;

namespace FileReaderWriter
{

    /// <summary>File_ReaderWriter</summary>
    abstract public class FileReaderWriter
    {

        /// <summary>Mutex Lock 避免重複存取</summary>
        static protected object locker = new object();

        /// <summary>Directory Path ex:"C:\\TEST\\"</summary>
        abstract protected string DirectoryPath { set; get; }
        /// <summary>File Name ex:"TEST.txt"</summary>
        abstract protected string FileName { set; get; }

        /// <summary>Full File Path</summary>
        protected string FullFilePath { get { return DirectoryPath + FileName; } }

        virtual protected Encoding encoding { get { return Encoding.Default; } }

        /// <summary>Readbase</summary>
        /// <param name="bLine">read by line</param>
        /// <returns>success,return null for error</returns>
        string Read_base(bool bLine)
        {
            lock (locker)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FullFilePath, encoding))
                    {
                        if (bLine)
                        {
                            return sr.ReadLine();
                        }
                        else
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ReadErrorProcess(ex.Message);
                    return null;
                }
            }
        }
        /// <summary>Read</summary>
        /// <returns>success,return null for error</returns>
        protected string Read()
        {
            return Read_base(false);
        }
        /// <summary>ReadLine</summary>
        /// <returns>success,return null for error</returns>
        protected string ReadLine()
        {
            return Read_base(true);
        }

        /// <summary>write</summary>
        /// <param name="Data">data</param>
        /// <param name="bAppend">Append or Create</param>
        /// <param name="bEndNewLine">End with NewLine</param>
        /// <returns>bool success</returns>
        protected bool Write(string Data, bool bAppend, bool bEndNewLine = true)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            lock (locker)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(FullFilePath, bAppend, Encoding.Default))
                    {
                        if (bEndNewLine)
                        {
                            sw.WriteLine(Data);
                        }
                        else
                        {
                            sw.Write(Data);
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorProcess(ex.Message);
                }
                return false;
            }
        }

        /// <summary>Read Error Process</summary>
        /// <param name="msg">Exception message</param>
        void ReadErrorProcess(string msg)
        {
            Write(msg, true, true);
        }
        /// <summary>Write Error Process</summary>
        /// <param name="msg">Exception message</param>
        void WriteErrorProcess(string msg)
        {
            Write(msg, true, true);
        }

    }

}