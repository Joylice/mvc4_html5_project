using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Apabi.WeiXin.Service.Models;

namespace Apabi.WeiXin.Service.Content
{
    public class BindXmlData
    {
        /// <summary>
        /// 读取xml数据并绑定实体集（移动接口）
        /// </summary>
        /// <param name="backstr"></param>
        /// <returns></returns>
        public List<Resource> BindResources(string backstr, ref string Totalcount)
        {
            List<Resource> rsList = new List<Resource>();
            Resource rs = new Resource();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(backstr);
            //doc.Save("C:\\Users\\tianshuai\\Desktop\\新建文件夹\\test.xml");  
            if (doc.GetElementsByTagName("TotalCount")[0] != null)
                Totalcount = doc.GetElementsByTagName("TotalCount")[0].InnerText;
            else
                Totalcount = "";
            XmlNodeList xl = doc.GetElementsByTagName("Record");
            foreach (XmlNode xn in xl)
            {
                if (xn.SelectSingleNode("Identifier") != null)
                {
                    rs.Identifier = xn.SelectSingleNode("Identifier").InnerText.ToString();
                }
                else
                    rs.Identifier = "";

                if (xn.SelectSingleNode("CoverUrl") != null)
                {
                    rs.CoverUrl = xn.SelectSingleNode("CoverUrl").InnerText.ToString();
                }
                else
                    rs.CoverUrl = "";

                if (xn.SelectSingleNode("ISBN") != null)
                {
                    rs.ISBN = xn.SelectSingleNode("ISBN").InnerText.ToString();
                }
                else
                    rs.ISBN = "";

                if (xn.SelectSingleNode("Title") != null)
                {
                    rs.Title = xn.SelectSingleNode("Title").InnerText.ToString();
                }
                else
                    rs.Title = "";

                if (xn.SelectSingleNode("Creator") != null)
                {
                    rs.Creator = xn.SelectSingleNode("Creator").InnerText.ToString();
                }
                else
                    rs.Creator = "";

                if (xn.SelectSingleNode("Publisher") != null)
                {
                    rs.Publisher = xn.SelectSingleNode("Publisher").InnerText.ToString();
                }
                else
                    rs.Publisher = "";

                if (xn.SelectSingleNode("PublishDate") != null)
                {
                    rs.PublishDate = xn.SelectSingleNode("PublishDate").InnerText.ToString();
                }
                else
                    rs.PublishDate = "";

                if (xn.SelectSingleNode("Abstract") != null)
                {
                    rs.Abstract = xn.SelectSingleNode("Abstract").InnerText.ToString();
                }
                else
                    rs.Abstract = "";

                if (xn.SelectSingleNode("BookStatus") != null)
                {
                    rs.BookStatus = xn.SelectSingleNode("BookStatus").InnerText.ToString();
                }
                else
                    rs.BookStatus = "";
                if (xn.SelectSingleNode("CopyNumber") != null)
                {
                    rs.CopyNumber = xn.SelectSingleNode("CopyNumber").InnerText.ToString();
                }
                else
                    rs.CopyNumber = "";
                rsList.Add(rs);
                rs = new Resource();
            }
            return rsList;
        }
    }
}