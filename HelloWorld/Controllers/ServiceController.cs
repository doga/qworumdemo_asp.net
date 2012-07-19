using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Xml;
using System.Xml.Linq;

namespace HelloWorld.Controllers {
    public class ServiceController : HomeController {

        /* Phase 1:
         * Receive call argument, compute result
         */
        [HttpPost]
        new public XDocument Index() {
            string name = "none";
            try{
              name=(string)EvaluateXPath(ParseXMLPost(Request), "/*/text()");
            }catch(Exception ex){}
            Response.ContentType="application/xml";
            return new XDocument(
                new XElement(NS+"goto", new XAttribute("href", "/Service/Show"),
                    new XElement(NS+"variable", new XAttribute("name", "result"),
                        new XElement("sentence", new XText("Hello, "+name))
                    )
                )
            );
        }

        /* Phase 2:
         * Show result to end-user
         */
        public ActionResult Show() {
            XmlDocument doc = ParseXMLPost(Request);
            ViewBag.Sentence = (string)EvaluateXPath(doc, "/*/text()");
            return View();
        }

        /* Phase 3:
         * Return result
         */
        public XDocument Result() {
            Response.ContentType="application/xml";
            return new XDocument(
                new XElement(NS+"variable", new XAttribute("name", "result"))
            );
        }
    }
}
