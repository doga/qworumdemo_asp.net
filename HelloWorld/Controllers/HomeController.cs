using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

//mvc3 
namespace HelloWorld.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public XDocument CallService() {
            Response.ContentType="application/xml; charset=utf-8";
            return new XDocument(
                new XElement(NS+"goto", 
                    new XAttribute("href", "/Home/ReceiveCallResult"),
                    new XElement(NS+"call", 
                        new XAttribute("href", "/Service/Index"),
                        new XElement("name", new XText("Dave"))
                    )
                )
            );
        }

        [HttpPost]
        public ActionResult ReceiveCallResult() {
            XmlDocument doc = ParseXMLPost(Request);
            ViewBag.Name = (string)EvaluateXPath(doc, "/*/text()");
            return View();
        }

        protected XNamespace NS = "http://qworum.net/";

        protected XmlDocument ParseXMLPost(HttpRequestBase request) {
            XmlDocument doc = new XmlDocument();
            using (var reader = new StreamReader(request.InputStream)) {
                try {
                    doc.LoadXml(reader.ReadToEnd());
                } catch (XmlException ex) {
                    doc = null;
                }
            }
            if (doc == null) {
                try {
                    doc = new XmlDocument();
                    doc.LoadXml(request.QueryString.Get("qworum"));
                } catch (Exception ex) {
                    doc = null;
                }
            }
            return doc;
        }

        protected object EvaluateXPath(XmlDocument doc, string xpath) {
            try {
                XPathDocument xpathDoc = new XPathDocument(new XmlNodeReader(doc));
                XPathNavigator nav = xpathDoc.CreateNavigator();
                XPathExpression expr = XPathExpression.Compile(xpath);
                return nav.Evaluate(expr);
            }catch(Exception ex){}
            return null;
        }
    }
}
