using System;
using System.Collections.Generic;
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


        public void WriteFile(string fileName, string data)
        {
            fileName += ".txt";
            System.IO.File.WriteAllText(@fileName, data);
        }
    }
}