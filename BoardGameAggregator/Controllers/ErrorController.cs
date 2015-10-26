using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoardGameAggregator.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult PageNotFound()
        {
            return View();
        }
    }
}