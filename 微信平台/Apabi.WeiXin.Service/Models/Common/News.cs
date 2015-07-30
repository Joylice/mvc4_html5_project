using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;


namespace WeiXinPinTai.Models.Common
{
    public class News
    {
        //用户信息列表
        public Model.NewsModel oneNew { get; set; }
        public PagedList<Model.NewsModel> newList { get; set; }
        public List<Model.NewsModel> dataList { get; set; }
    }
}