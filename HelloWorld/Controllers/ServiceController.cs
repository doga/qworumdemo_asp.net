using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Xml;
using System.Xml.Linq;
using System.Web.Helpers;

namespace HelloWorld.Controllers {
    public class ServiceController : HomeController {

        /* Phase 1:
         * Receive call argument, compute result
         */
        [HttpPost]
        [ValidateInput(false)]
        new public XDocument Index() {
            // input
            string name = (string)EvaluateXPath(ParseXMLPost(Request), "string(/*/text())");
            // output
            string sentence = "Hello, "+name;
            Response.ContentType="application/xml";
            return new XDocument(
                new XElement(NS+"goto", new XAttribute("href", "Show"),
                    new XElement(NS+"variable", new XAttribute("name", "result"),
                        new XElement("sentence", sentence)
                    )
                )
            );
        }

        /* Phase 2:
         * Show result to end-user
         */
        public ActionResult Show() {
            XmlDocument doc = ParseXMLPost(Request);
            ViewBag.Sentence = (string)EvaluateXPath(doc, "string(/*/text())");
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
