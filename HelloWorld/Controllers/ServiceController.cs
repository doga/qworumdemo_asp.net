using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Xml;
using System.Xml.Linq;

namespace HelloWorld.Controllers {
    public class ServiceController : HomeController {
        new public XDocument Index() {
            string name = (string)EvaluateXPath(ParseXMLPost(Request), "/*/text()");
            Response.ContentType="application/xml; charset=utf-8";
            return new XDocument(
                new XElement(NS+"goto", new XAttribute("href", "/Service/Show"),
                    new XElement(NS+"variable", new XAttribute("name", "result"),
                        new XElement("sentence", new XText(name))
                    )
                )
            );
        }

        public ActionResult Show() {
            XmlDocument doc = ParseXMLPost(Request);
            ViewBag.Sentence = (string)EvaluateXPath(doc, "/*/text()");
            return View();
        }

        public XDocument Result() {
            Response.ContentType="application/xml; charset=utf-8";
            return new XDocument(
                new XElement(NS+"variable", new XAttribute("name", "result"))
            );
        }
    }
}
