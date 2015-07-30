using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Apabi.WeiXin.Service.Content;
using Apabi.WeiXin.Service.Models;

namespace Apabi.WeiXin.Service.Common
{
    public class GetInterfaceData
    {
        private List<Resource> rsList = new List<Resource>();
        private string totalCount = string.Empty;

        public string TotalCount
        {
            get { return totalCount; }
        }

        public List<Resource> RsList
        {
            get { return rsList; }
        }

        /// <summary>
        /// 获取检索接口数据
        /// </summary>
        /// <param name="sw">检索词</param>
        /// <param name="sourcetype">检索库</param>
        /// <param name="curpage">页码</param>
        /// <param name="pagesize">每页显示个数</param>
        public void GetSearchData(string sw, string sourcetype, int curpage, int pagesize)
        {
            Thread apabiThread = new Thread(new ThreadStart(() => GetData(sw, sourcetype, curpage, pagesize)));
            apabiThread.IsBackground = true;
            apabiThread.Start();
            while (apabiThread.IsAlive)
            {
                //判断线程是否结束
            }
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="sw">检索词</param>
        /// <param name="sourcetype">检索库</param>
        /// <param name="curpage">页码</param>
        /// <param name="pagesize">每页显示个数</param>
        public Resource GetResourceData(string metaid)
        {
            Resource rs = new Resource();
            string apabiUrl = System.Configuration.ConfigurationManager.AppSettings["apabiUrl"];
            BindXmlData bx = new BindXmlData();
            string para = "/mobile.mvc?api=bookdetail&metaid=" + metaid;
            WebRequest request = WebRequest.Create(apabiUrl + para);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string backstr = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            response.Close();
            rs = (bx.BindResources(backstr, ref totalCount))[0];
            return rs;
        }

        private void GetData(string sw, string sourcetype, int curpage, int pagesize)
        {
            string apabiUrl = System.Configuration.ConfigurationManager.AppSettings["apabiUrl"];
            string backstr = "";
            string para = "";
            BindXmlData bx = new BindXmlData();
            if (sourcetype == "dlib")
                para = "/mobile.mvc?api=metadatasearch&type=1&key=" + sw + "&order=0&ordertype=0&page=" + curpage + "&pagesize=" + pagesize;
            if (sourcetype == "refbook")
                para = "/mobile.mvc?api=metadatasearch&type=1&key=" + sw + "&order=0&ordertype=0&page=" + curpage + "&pagesize=" + pagesize;
            if (sourcetype == "yearbook")
                para = "/mobile.mvc?api=metadatasearch&type=1&key=" + sw + "&order=0&ordertype=0&page=" + curpage + "&pagesize=" + pagesize;
            if (sourcetype == "newspaper")
                para = "/mobile.mvc?api=metadatasearch&type=1&key=" + sw + "&order=0&ordertype=0&page=" + curpage + "&pagesize=" + pagesize;
            WebRequest request = WebRequest.Create(apabiUrl + para);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            backstr = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            response.Close();

            #region
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //string paraUrlCoded = "pid=search.api&sourcetype=dlib&keyword=" + sw + "&curpage=1&pagesize=1";
            //byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(paraUrlCoded);
            //req.Method = "POST";
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentLength = requestBytes.Length;
            //Stream requestStream = req.GetRequestStream();
            //requestStream.Write(requestBytes, 0, requestBytes.Length);
            //requestStream.Close();
            //HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            //StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            //backstr = sr.ReadToEnd();
            //sr.Close();
            //res.Close();
            //JObject json = (JObject)JsonConvert.DeserializeObject(backstr);
            #endregion

            rsList = bx.BindResources(backstr, ref totalCount);

        }
    }
}