using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apabi.WeiXin.Service.Common;
using Apabi.WeiXin.Service.Models;

namespace Apabi.WeiXin.Service.Controllers
{
    public class ResourceDetailController : Controller
    {
        //
        // GET: /ResourceDetail/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResourceDetail()
        {
            string metaid = Request.Params["metaid"];
            GetInterfaceData getdata = new GetInterfaceData();
            Resource rs = getdata.GetResourceData(metaid);
            // List<Resource> rtn = GetData(apabiUrl);
            ViewData["Resource"] = rs;
            return View();
        }

    }
}
