using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Web.Helpers;

//mvc3 
namespace HelloWorld.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public XDocument CallService() {
            Response.ContentType="application/xml";
            return new XDocument(
                new XComment("\n"+
                  "  QWORUM SUPPORT IS MISSING IN YOUR BROWSER:\n"+
                  "  This site only works correctly with Qworum enabled browsers. \n"+
                  "  Please visit http://www.qworum.com/products\n"
                ),
                new XElement(NS+"goto", 
                    new XAttribute("href", "ReceiveCallResult"),
                    new XElement(NS+"call", 
                        new XAttribute("href", "/Service/Index"),
                        new XElement("name", new XText("Dave"))
                    )
                )
            );
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReceiveCallResult() {
            XmlDocument doc = ParseXMLPost(Request);
            ViewBag.Sentence = (string)EvaluateXPath(doc, "string(/*/text())");
            return View();
        }

        // Boilerplate

        protected XNamespace NS = "http://qworum.net/";

        protected XmlDocument ParseXMLPost(HttpRequestBase request) {
            XmlDocument doc = null;
            if (request.ContentType.IndexOf("application/xml") == 0) {
                using (var reader = new StreamReader(request.InputStream)) {
                    try {
                        doc = new XmlDocument();
                        string xml = reader.ReadToEnd();
                        doc.LoadXml(xml);
                    } catch (XmlException ex) {
                        doc = null;
                    }
                }
            } else if (request.ContentType.IndexOf("application/x-www-form-urlencoded") == 0) {
                try {
                    doc = new XmlDocument();
                    FormCollection form = new FormCollection(Request.Unvalidated().Form);
                    string xml = form.Get("qworum");
                    doc.LoadXml(xml);
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
