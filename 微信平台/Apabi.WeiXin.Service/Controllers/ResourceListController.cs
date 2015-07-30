using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apabi.WeiXin.Service.Common;
using Apabi.WeiXin.Service.Models;

namespace Apabi.WeiXin.Service.Controllers
{
    public class ResourceListController : Controller
    {
        //
        // GET: /ResourceList/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ResourceList()
        {
            string sw = Request.Params["sw"];
            int page = Convert.ToInt32(Request.Params["page"]);
            if (page == 0)
                page = 1;
            int pagesize = Convert.ToInt32(Request.Params["pagesize"]);
            if (pagesize == 0)
                pagesize = 10;
            string sourcetype = Request.Params["sourcetype"];
            GetInterfaceData getdata = new GetInterfaceData();
            getdata.GetSearchData(sw, sourcetype, page, pagesize);
            List<Resource> rsList = getdata.RsList;
            // List<Resource> rtn = GetData(apabiUrl);
            ViewData["Resources"] = rsList;
            ViewData["TotalCount"] = getdata.TotalCount;
            return View();
        }
    }
}
