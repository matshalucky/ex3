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
        
        public string Test()
        {
            return "success";
        }
        private KeyValuePair<string,string> GetLonLat()
        {
            Random rnd = new Random();
            string lon = Commands.Instance.getData("get /position/longitude-deg");
            string lat = Commands.Instance.getData("get /position/latitude-deg");
            float lonTemp = float.Parse(lon) + rnd.Next(50);
            float latTemp = float.Parse(lat) + rnd.Next(50);
            lon = lonTemp.ToString();
            lat = latTemp.ToString();

            return new KeyValuePair<string, string>(lon, lat);
        }

        public string createXml(string lon , string lat)
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
            return createXml(lon, lat);
        }
        [HttpPost] 
        public string GetFlightData()
        {
            KeyValuePair<string, string> point = GetLonLat();
            string lon = point.Key;
            string lat = point.Value;
            string rudder = Commands.Instance.getData("get /controls/flight/rudder");
            string throttle = Commands.Instance.getData("get /controls/engines/current-engine/throttle");
            AddData(lon, lat, rudder, throttle);
            return createXml(lon, lat);
        }
        
        private void AddData(string lon , string lat ,string rud, string throt)
        {
            string data = lon + "," + lat + "," + rud + "," + throt;
            FileHandler.Instance.updateData(data);
        }
        [HttpPost]
        public string GetFlightDataFromFile()
        {
            // parse the data from the file for the first time.
            if(FileHandler.Instance.Index == FileHandler.Instance.getNumOfPoints())
            {
                return "";
            }
            IList<string> paramList = FileHandler.Instance.getLonLat().Split(',').ToList<string>();
            string lon = paramList[0];
            string lat = paramList[1];
            return createXml(lon, lat);
        }

        [HttpPost]
        public void SaveData()
        {
            FileHandler.Instance.WriteFile();
        }
        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Map(string ip, int port)
        {
            Commands.Instance.connect(ip, port);

            ViewBag.lon = Commands.Instance.getData("get /position/longitude-deg");
            ViewBag.lat = Commands.Instance.getData("get /position/latitude-deg");
            ViewBag.ip = ip;
            ViewBag.port = port;
            return View();
        }

        public ActionResult displayRoute(string ip, int port , int time)
        {
            Commands.Instance.connect(ip, port);
            Session["time"] = time;
                       
            return View();
        }
        public ActionResult save(string ip, int port,int pace, int duration,string fileName)
        {
            Commands.Instance.connect(ip, port);
            FileHandler.Instance.FileName = fileName;
            Session["pace"] = pace;
            Session["duration"] = duration;
            return View();
        }

        public ActionResult Load(string fileName, int pace)
        {
            FileHandler.Instance.FileName = fileName;
            FileHandler.Instance.pasreDataFromFile();
            ViewBag.numOfPoints = FileHandler.Instance.getNumOfPoints();
            Session["pace"] = pace;
            return View();
        }
    }
}