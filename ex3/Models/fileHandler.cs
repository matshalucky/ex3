﻿using System;
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
        public bool firstWrite =true;
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
        public void Close()
        {
            firstWrite = true;
        }

        //public void WriteFile()
        //{
        //    fileName += ".txt";
        //    System.IO.File.WriteAllText(@fileName, data);
        //}
        public void WriteFile()
        {
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

        public void pasreDataFromFile()
        {
            parsedData = new List<string>();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            int lineCounter = 0;
            using (StreamReader sr = System.IO.File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    parsedData.Add(s);
                    lineCounter++;
                }
            }
            
            numOfPoints = lineCounter;
        }
        public int getNumOfPoints()
        {
            return numOfPoints;
        }
        public string getLonLat()
        {
            return this.parsedData[index++];
        }
    }

}