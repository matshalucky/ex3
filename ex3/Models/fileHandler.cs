using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ex3.Models
{
    
    public class FileHandler
    {
        private static FileHandler m_Instance = null;
        //singelton
        public static FileHandler Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FileHandler();
                }
                return m_Instance;
            }
        }
        string data ="";
        string fileName;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        public void updateData(string newData)
        {
            data = newData;

        }

        //public void WriteFile()
        //{
        //    fileName += ".txt";
        //    System.IO.File.WriteAllText(@fileName, data);
        //}
        public void WriteFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName+".txt";
            using (StreamWriter outputFile = File.AppendText(path))
            {
       
                    outputFile.WriteLine(data);
                
               
            }

        }
    }
}