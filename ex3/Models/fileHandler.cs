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

        /// <summary>
        /// this function update the data member of the class.
        /// </summary>
        /// <param name="newData"></param>
        public void UpdateData(string newData)
        {
            data = newData;

        }
        /// <summary>
        /// this function initialize the index member in order to get the first line next time.
        /// </summary>
        public void Close()
        {
            Index = 0;
        }

        /// </summary>
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
        // index property.
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
        /// <summary>
        /// read and parse all data from file.
        /// </summary>
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

        public int NumOfPoints
        {
            get
            {
                return numOfPoints;
            }
            set
            {
                numOfPoints = value;
            }
        }
        
        /// <summary>
        /// return the current index array of the parsed and add to the 
        /// index 1.
        /// </summary>
        /// <returns></returns>
        public string GetLonLat()
        {
            return this.parsedData[index++];
        }
    }

}