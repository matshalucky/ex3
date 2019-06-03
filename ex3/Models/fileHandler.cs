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
        
        public string data ="";
        public string fileName;
        public StreamWriter writer;
        //file name property.
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

        public void UpdateData(string newData)
        {
            data = newData;

        }
        public void Close()
        {
            Index = 0;
        }

        //public void WriteFile()
        //{
        //    fileName += ".txt";
        //    System.IO.File.WriteAllText(@fileName, data);
        //}
        public void WriteFile()
        {
            //create path
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            using (StreamWriter outputFile = File.AppendText(path))
            {
                    outputFile.WriteLine(data);
            }

        }
        List<string> parsedData;
        int numOfPoints = 0;
        int index = 0;
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }
        //read and parse all data from file.
        public void PasreDataFromFile()
        {
            parsedData = new List<string>();
            //create path.
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            int lineCounter = 0;
            //read all lines from file.
            using (StreamReader sr = System.IO.File.OpenText(path))
            {
                string s = "";
                //adding each line to the list. 
                while ((s = sr.ReadLine()) != null)
                {
                    parsedData.Add(s);
                    lineCounter++;
                }
            }
            
            numOfPoints = lineCounter;
        }
        public int GetNumOfPoints()
        {
            return numOfPoints;
        }
        public string GetLonLat()
        {
            return this.parsedData[index++];
        }
    }

}