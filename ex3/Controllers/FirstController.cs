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
        [HttpPost]
        public string ToXml()
        {
            KeyValuePair<string, string> point = GetLonLat();
            string lon = point.Key;
            string lat = point.Value;
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

        public string GetFlightData()
        {
            KeyValuePair<string, string> point = GetLonLat();
            string lon = point.Key;
            string lat = point.Value;
            string rudder = Commands.Instance.getData("get /controls/flight/rudder");
            string throttle = Commands.Instance.getData("get /controls/engines/current-engine/throttle");
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            writer.WriteElementString("lon", lon);
            writer.WriteElementString("lat", lat);
            writer.WriteElementString("rudder", rudder);
            writer.WriteElementString("throttle", throttle);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
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
            Session["pace"] = pace;
            Session["duration"] = duration;
            return View();
        }
    }
}