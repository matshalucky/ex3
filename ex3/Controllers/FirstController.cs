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

        [HttpPost]
        public string ToXml()
        {
            Random rnd = new Random();
            string lon = Commands.Instance.getData("get /position/longitude-deg") + rnd.Next(50);
            string lat = Commands.Instance.getData("get /position/latitude-deg") + rnd.Next(50);
            //Initiate XML stuff
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
    }
}