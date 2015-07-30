using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Webdiyer.WebControls.Mvc;
using System.IO;

namespace WeiXinPinTai.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditNews()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditNews(int id=1)
        {
            Models.Common.News oneNew = new Models.Common.News();
            oneNew.oneNew = DAL.NewsDAL.GetOneNew(id);
            return View(oneNew);
        } 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(Models.Common.News indexNews, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                  if (image != null && image.ContentLength > 0)
                  {
                      string fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + Path.GetFileName(image.FileName);
                      string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                      image.SaveAs(filePath);
                      indexNews.oneNew.CoverUrl = "/Images/" + fileName ; 
                  }
                if (string.IsNullOrEmpty(indexNews.oneNew.NewId.ToString()))
                {
                    indexNews.oneNew.ArticleType = Convert.ToInt32(Request.Form["hidValue"]);
                    DAL.NewsDAL.AddNews(indexNews.oneNew);
                }
                else
                {
                    indexNews.oneNew.ArticleType = Convert.ToInt32(Request.Form["hidValue"]);
                    DAL.NewsDAL.DoModafy(indexNews.oneNew);
                }
                return RedirectToAction("ManagerNewsList", "Common", new { type = indexNews.oneNew.ArticleType });

            }
            return View(indexNews);
        }
        [HttpGet]
        public ActionResult SeparateType(int?id=0)
        {
         return RedirectToAction("ManagerNewsList", "Common", new { type=id});
        }
        public ActionResult ManagerNewsList(int? id = 1, string title = null,int? type=0)
        {
            int totalCount = 0;
            int totalItemCount = 0;
            int pageIndex = id ?? 1;
            int ArticleType = type ?? 1;
            int pageSize = 2;
            PagedList<Model.NewsModel> InfoPager = DAL.NewsDAL.GetInfos(title, ArticleType, pageSize, pageIndex, out totalCount, out totalItemCount).AsQueryable().ToPagedList(pageIndex, pageSize);
            InfoPager.TotalItemCount = totalItemCount;
            //当前页最后一条被删除之后的，页索引超出范围
            InfoPager.CurrentPageIndex = (int)(id > totalCount?totalCount:id ?? 1);
            Models.Common.News indexNew = new Models.Common.News();
            indexNew.newList = InfoPager;
            return View(indexNew);
        }
        
        public ActionResult DoDelete(int? id=1, FormCollection values=null)
        {
            if (values["val"] != null)
            {
               int[] delItems = Array.ConvertAll(values["val"].Split(','), i => int.Parse(i));
                foreach (var one in delItems)
                {
                    DAL.NewsDAL.DoDel(one);
                }
            }
            return RedirectToAction("ManagerNewsList","Common",new {id=Request.Params["hidValue"]});
        }
        public ActionResult GoBack(int? id=1,int? type=0)
        {
            return RedirectToAction("ManagerNewsList", "Common", new { id=id,type=type});
        }
       
        public ActionResult ShowNewsList()
        {
            Models.Common.News indexData = new Models.Common.News();
            List<Model.NewsModel> dataList = DAL.NewsDAL.GetDataList(0, 6, "");
            indexData.dataList = dataList;
            return View(indexData);
        }
        public ActionResult ShowHotSpotList()
        {
            Models.Common.News indexData = new Models.Common.News();
            List<Model.NewsModel> dataList = DAL.NewsDAL.GetDataList(1, 6, "");
            indexData.dataList = dataList;
            return View(indexData);
        }
        [HttpGet]
        public ActionResult ShowDetail(int id)
        {
            Models.Common.News indexData = new Models.Common.News();
            Model.NewsModel oneData = DAL.NewsDAL.GetOneNew(id);
            indexData.oneNew = oneData;
            return View("~/Views/Common/ShowDetail.cshtml", indexData);
        }
        [HttpGet]
        public ActionResult GetDataListByDate(string option,int id=6,int type=0)
        {
            List<Model.SeletedModel> dataList = DAL.NewsDAL.GetDataListByDate(option, id,type);
            string baseUrl = Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_Port"];
            foreach (var one in dataList)
            {
                one.CoverUrl = baseUrl + one.CoverUrl;
                one.ShowUrl = baseUrl + one.ShowUrl;
            }
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
    }
}
