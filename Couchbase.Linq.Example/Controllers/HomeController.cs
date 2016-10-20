using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Couchbase.Linq.Example.Documents;
using Couchbase.Linq.Example.Models;
using Couchbase.Linq.Extensions;

namespace Couchbase.Linq.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}