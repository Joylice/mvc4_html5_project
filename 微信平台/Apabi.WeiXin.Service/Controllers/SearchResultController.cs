using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apabi.WeiXin.Service.Common;
using Apabi.WeiXin.Service.Models;

namespace Apabi.WeiXin.Service.Controllers
{
    public class SearchResultController : Controller
    {
        //
        // GET: /SearchResult/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchResult()
        {
            string sw = Request.Params["sw"];
            GetInterfaceData getdata = new GetInterfaceData();
            getdata.GetSearchData(sw, "dlib", 1, 1);
            List<Resource> dlibList = getdata.RsList;
            ViewData["dlibList"] = dlibList;

            getdata = new GetInterfaceData();
            getdata.GetSearchData(sw, "refbook", 1, 1);
            List<Resource> refBookList = getdata.RsList;
            ViewData["refBookList"] = refBookList;

            getdata = new GetInterfaceData();
            getdata.GetSearchData(sw, "yearbook", 1, 1);
            List<Resource> yearBookList = getdata.RsList;
            ViewData["yearBookList"] = yearBookList;

            getdata = new GetInterfaceData();
            getdata.GetSearchData(sw, "newspaper", 1, 1);
            List<Resource> newsPaperList = getdata.RsList;
            ViewData["newsPaperList"] = newsPaperList;
            return View();
        }
    }
}
