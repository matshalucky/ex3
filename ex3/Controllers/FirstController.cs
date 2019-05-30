using ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Windows.Input;
using System.Diagnostics;
//
namespace ex3.Controllers
{
    public class FirstController : Controller
    {

        
        private KeyValuePair<string,string> GetLonLat()
        {
            Random rnd = new Random();
            string lon = Commands.Instance.GetData("get /position/longitude-deg");
            string lat = Commands.Instance.GetData("get /position/latitude-deg");
            float lonTemp = float.Parse(lon) + rnd.Next(50);
            float latTemp = float.Parse(lat) + rnd.Next(50);
            //float lonTemp = float.Parse(lon);
            //float latTemp = float.Parse(lat); ;
            lon = lonTemp.ToString();
            lat = latTemp.ToString();

            return new KeyValuePair<string, string>(lon, lat);
        }
        public string CreateXml(string lon , string lat)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            writer.WriteElementString("lon", lon);
            writer.WriteElementString("lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
        [HttpPost]
        public string ToXml()
        {
            KeyValuePair<string, string> point = GetLonLat();
            string lon = point.Key;
            string lat = point.Value;
            return CreateXml(lon, lat);
        }
        [HttpPost] 
        public string GetFlightData()
        {
            KeyValuePair<string, string> point = GetLonLat();
            string lon = point.Key;
            string lat = point.Value;
            string rudder = Commands.Instance.GetData("get /controls/flight/rudder");
            string throttle = Commands.Instance.GetData("get /controls/engines/current-engine/throttle");
            AddData(lon, lat, rudder, throttle);
            return CreateXml(lon, lat);
        }
        
        private void AddData(string lon , string lat ,string rud, string throt)
        {
            string data = lon + "," + lat + "," + rud + "," + throt;
            FileHandler.Instance.UpdateData(data);
        }
        [HttpPost]
        public void SaveData()
        {
            FileHandler.Instance.WriteFile();
        }
        [HttpPost]
        public void CloseConnection()
        {
            Commands.Instance.Close();
            FileHandler.Instance.Close();
            
        }
        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Map(string ip, int port)
        {
            Commands.Instance.Connect(ip, port);

            ViewBag.lon = Commands.Instance.GetData("get /position/longitude-deg");
            ViewBag.lat = Commands.Instance.GetData("get /position/latitude-deg");
            ViewBag.ip = ip;
            ViewBag.port = port;
            return View();
        }

        public ActionResult DisplayRoute(string ip, int port , int time)
        {
            Commands.Instance.Connect(ip, port);
            Session["time"] = time;
            return View();
        }
        public ActionResult Save(string ip, int port,int pace, int duration,string fileName)
        {
            Commands.Instance.Connect(ip, port);
            FileHandler.Instance.FileName = fileName;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            Session["pace"] = pace;
            Session["duration"] = duration;
            return View();
        }

        public ActionResult Load(string fileName, int pace)
        {
            return View();
        }

        private bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public ActionResult MapOrLoad(string s , int num)
        {
            
            if (ValidateIPv4(s))
            {
                string ip = s;
                int port = num;
                Commands.Instance.Connect(ip, port);
                ViewBag.lon = Commands.Instance.GetData("get /position/longitude-deg");
                ViewBag.lat = Commands.Instance.GetData("get /position/latitude-deg");
                ViewBag.ip = ip;
                ViewBag.port = port;
                return View("Map");
            } else
            {
                string fileName = s;
                int pace = num;
                FileHandler.Instance.FileName = fileName;
                FileHandler.Instance.PasreDataFromFile();
                ViewBag.numOfPoints = FileHandler.Instance.GetNumOfPoints();
                Session["pace"] = pace;
                return View("Load");
            }
        }
        [HttpPost]
        public string GetFlightDataFromFile()
        {
            // parse the data from the file for the first time.
            if (FileHandler.Instance.Index == FileHandler.Instance.GetNumOfPoints())
            {
                return "";
            }
            IList<string> paramList = FileHandler.Instance.GetLonLat().Split(',').ToList<string>();
            string lon = paramList[0];
            string lat = paramList[1];
            return CreateXml(lon, lat);
        }
    }

}