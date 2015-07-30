using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace DAL
{
    public class NewsDAL
    {
        //添加新闻
        public static bool AddNews(Model.NewsModel  newOne)
        {
            bool ret = false;
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("insert into News values ");
            sqlStr.Append("(");
            sqlStr.Append("@Title,@SubHead,@Author,@Source,@Content,@PubDate,@IsShow,@ArticleType,@CoverUrl");
            sqlStr.Append(")");
            SqlParameter[] paramLsit = new SqlParameter[]{
                new SqlParameter("@Title",SqlDbType.NVarChar){Value=newOne.Title},
                new SqlParameter("@SubHead",SqlDbType.NVarChar){Value= String.IsNullOrEmpty(newOne.SubHead)?" ":newOne.SubHead},
                new SqlParameter("@Author",SqlDbType.NVarChar){Value=newOne.Author},
                new SqlParameter("@Source",SqlDbType.NVarChar){Value=String.IsNullOrEmpty(newOne.NewSource)?" ":newOne.NewSource},
                new SqlParameter("@Content",SqlDbType.NVarChar){Value=newOne.Content},
                new SqlParameter("@PubDate",SqlDbType.DateTime){Value=DateTime.Now},
                new SqlParameter("@IsShow",SqlDbType.Bit){Value=newOne.IsShow},
                new SqlParameter("@ArticleType",SqlDbType.Int){Value=newOne.ArticleType},
                new SqlParameter("@CoverUrl",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(newOne.CoverUrl)?" ":newOne.CoverUrl}
            };
           int i= SqlHelper.ExecuteNonQuery( SqlHelper.SqlConnectionString ,System.Data.CommandType.Text, sqlStr.ToString(), paramLsit);
           if (i > 0)
           {
               ret= true;
           }
              return ret;
        }
        //根据新闻id获取新闻内容
        public static Model.NewsModel  GetOneNew(int id)
        {
            Model.NewsModel newOne = null;
            StringBuilder sqlStr=new StringBuilder();
            sqlStr.Append("select * from News ");
            sqlStr.Append(" where Id=@Id");
            SqlParameter param = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.SqlConnectionString, CommandType.Text, sqlStr.ToString(), param);
            while (dr.Read())
            {
                newOne = new Model.NewsModel();
                newOne.NewId = Convert.ToInt32(dr["Id"]);
                newOne.Title = dr["Title"].ToString();
                newOne.NewSource = dr["Source"].ToString();
                newOne.SubHead = dr["SubHead"].ToString();
                newOne.Author = dr["Author"].ToString();
                newOne.Content = dr["Content"].ToString();
                newOne.IsShow = Convert.ToBoolean(dr["IsShow"]);
                newOne.PubDate = Convert.ToDateTime(dr["PubDate"]);
                newOne.CoverUrl = dr["CoverUrl"].ToString();
            }
            return newOne;
        }
        //获取信息列表
        public static List<Model.NewsModel> GetDataList(int type, int count,string order)
        {
            List<Model.NewsModel> dataList = new List<Model.NewsModel>();
            if (string.IsNullOrEmpty(order))
            {
                order = "Id desc";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select top(@num)  * from News where ArticleType=@type order by {0}",order));
            SqlParameter[] paramList=new SqlParameter[]{
            new SqlParameter("@num",SqlDbType.Int){Value=count},
            new SqlParameter("@type",SqlDbType.Int){Value=type}
            };
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.SqlConnectionString,CommandType.Text,strSql.ToString(),paramList);
            while (dr.Read())
            {
                Model.NewsModel oneData = new Model.NewsModel();
                oneData.NewId=Convert.ToInt32(dr["Id"]);
                oneData.Title=dr["Title"].ToString();
                oneData.SubHead = dr["SubHead"].ToString();
                oneData.Author = dr["Author"].ToString();
                oneData.Content = dr["Content"].ToString();
                oneData.NewSource = dr["Source"].ToString();
                oneData.PubDate = Convert.ToDateTime(dr["PubDate"]);
                oneData.CoverUrl = dr["CoverUrl"].ToString();
                dataList.Add(oneData);
            }
            return dataList;
        }
        //获取新闻列表--分页
        public static List<Model.NewsModel> GetInfos(string Title,int type, int PageSize, int CurrentCount, out int TotalCount, out int totalItemCount)
        {
            string Order = string.Format("Id DESC");
            string TableName = string.Format("News");
            string Where = " 1=1 and ArticleType="+type;
            if (!string.IsNullOrEmpty(Title))
            {
                Where += string.Format(" AND Title LIKE '%{0}%'", Title);
            }
            DataSet ds = SqlHelper.GetList(SqlHelper.SqlConnectionString, Order, PageSize, CurrentCount, TableName, Where, out TotalCount, out totalItemCount);
            List<Model.NewsModel> result = new List<Model.NewsModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result.Add(new Model.NewsModel() { NewId = Convert.ToInt32(dr["Id"]), Title = dr["Title"].ToString(),Author=dr["Author"].ToString(),SubHead=dr["SubHead"].ToString(),NewSource=dr["Source"].ToString(),PubDate=Convert.ToDateTime(dr["PubDate"]),IsShow=Convert.ToBoolean(dr["IsShow"]), Content = dr["Content"].ToString() });
                }
            }
            return result;
        }
        //逐个删除
        public static bool DoDel(int id)
        {
            bool ret = false;
            string sqlStr = "delete from News where Id=@Id";
            SqlParameter param = new SqlParameter("@Id", SqlDbType.Int) { Value=id};
            int i=SqlHelper.ExecuteNonQuery(SqlHelper.SqlConnectionString, CommandType.Text, sqlStr, param);
            if (i > 0)
            {
                ret = true;
            }
            return ret;
        }
        //单个修改
        public static bool DoModafy(Model.NewsModel oneNew)
        {
            bool ret = false;
            string sqlStr = string.Empty;
            sqlStr = "update News set Title=@Title,SubHead=@SubHead,Author=@Author,Source=@Source,Content=@Content,CoverUrl=@CoverUrl where Id=@Id ";        
            SqlParameter[]paramList=new SqlParameter[]{
            new SqlParameter("@Title",SqlDbType.NVarChar){Value=oneNew.Title},
            new SqlParameter("@SubHead",SqlDbType.NVarChar){Value=oneNew.SubHead},
            new SqlParameter("@Author",SqlDbType.NVarChar){Value=oneNew.Author},
            new SqlParameter("@Source",SqlDbType.NVarChar){Value=oneNew.NewSource},
            new SqlParameter("@Content",SqlDbType.NVarChar){Value=oneNew.Content},
            new SqlParameter("@CoverUrl",SqlDbType.NVarChar){Value=string.IsNullOrEmpty(oneNew.CoverUrl)?" ":oneNew.CoverUrl},
            new SqlParameter("@Id",SqlDbType.Int){Value=oneNew.NewId}
            };
            int i = SqlHelper.ExecuteNonQuery(SqlHelper.SqlConnectionString, CommandType.Text, sqlStr, paramList);
            if (i > 0)
            {
                ret = true;
            }
            return ret;
        }
        //时间序号检索
        public static List<Model.SeletedModel> GetDataListByDate(string option, int count, int articleType)
        {
            List<Model.SeletedModel> dataList = new List<Model.SeletedModel>();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select top(@count) * from (select  Title,'/Common/ShowDetail/'+cast(ID as varchar(8)) as Url ,CONVERT(varchar(100) ,PubDate, 112)AS PubDate,Content,CoverUrl from News where ArticleType=@ArticleType)a where PubDate like @SelectedID ");
            SqlParameter[] paramList = new SqlParameter[] { 
                new SqlParameter("@count",SqlDbType.Int){Value=count},
                new SqlParameter("@SelectedID", SqlDbType.NVarChar) { Value=option+"%"},
                new SqlParameter("@ArticleType",SqlDbType.Int){Value=articleType}
            };
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.SqlConnectionString,CommandType.Text,sqlStr.ToString(),paramList);
            while (dr.Read())
            {
                Model.SeletedModel oneData = new Model.SeletedModel();
                oneData.Title = dr["Title"].ToString();
                oneData.ShowUrl = dr["Url"].ToString();
                oneData.zhaiyao = dr["Content"].ToString().Substring(0, dr["Content"].ToString().Length <200?dr["Content"].ToString().Length:200)+"...";
                oneData.CoverUrl = dr["CoverUrl"].ToString();
                dataList.Add(oneData);
            }
            return dataList;
        }
    }
}