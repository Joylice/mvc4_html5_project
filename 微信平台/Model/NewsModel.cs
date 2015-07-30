using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    public class NewsModel
    {
        public int? NewId{ get; set; }
        [Required]
        [Display(Name="新闻标题")]
        public string Title { get; set; }
        [Display(Name = "副标题")]
        public string SubHead { get; set; }
        [Required]
        [Display(Name = "新闻发布者")]
        public string Author { get; set; }
        [Display(Name = "新闻来源")]
        public string NewSource { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "字符数限制：10~2000，其中样式会占用字数，点击“源代码”可查看全部文字。")]
        [Display(Name = "新闻内容")]
        public string Content { get; set; }
        [Display(Name = "发布时间")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd HH:mm}",ApplyFormatInEditMode=true)]
        public DateTime PubDate { get; set; }
        [Display(Name = "IsShow")]
        public Boolean IsShow { get; set; }
        public int ArticleType { get; set; }
        public string CoverUrl { get; set; }
        //时间序列化  检索字段
        public string ShowUrl { get; set; }
    }
    public class SeletedModel
    {
        public string Title { get; set; }
        public string ShowUrl { get; set; }
        public string CoverUrl { get; set; }
        public string zhaiyao { get; set; }
    }
    
}