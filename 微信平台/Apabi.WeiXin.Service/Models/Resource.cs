using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apabi.WeiXin.Service.Models
{
    public class Resource
    {
        private string _Identifier;
        private string _Title;
        private string _AlternativeTitle;
        private string _Creator;
        private string _Publisher;
        private string _Keywords;
        private string _Abstract;
        private string _PublishDate;
        private string _Score;
        private string _BookStatus;
        private string _Fulltext;
        private string _Catalog;
        private string _Path;
        private string _ZTFCategory;
        private string _ISBN;
        private string _TIYAN;
        private string _ContentUrl;
        private string _CoverUrl;
        private string _CopyNumber;

        public string CopyNumber
        {
            get { return _CopyNumber; }
            set { _CopyNumber = value; }
        }

        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public string AlternativeTitle
        {
            get { return _AlternativeTitle; }
            set { _AlternativeTitle = value; }
        }
        public string Creator
        {
            get { return _Creator; }
            set { _Creator = value; }
        }
        public string Publisher
        {
            get { return _Publisher; }
            set { _Publisher = value; }
        }
        public string Keywords
        {
            get { return _Keywords; }
            set { _Keywords = value; }
        }
        public string Abstract
        {
            get { return _Abstract; }
            set { _Abstract = value; }
        }
        public string PublishDate
        {
            get { return _PublishDate; }
            set { _PublishDate = value; }
        }

        public string Score
        {
            get { return _Score; }
            set { _Score = value; }
        }

        public string BookStatus
        {
            get { return _BookStatus; }
            set { _BookStatus = value; }
        }

        public string ISBN
        {
            get { return _ISBN; }
            set { _ISBN = value; }
        }

        public string Fulltext
        {
            get { return _Fulltext; }
            set { _Fulltext = value; }
        }

        public string Catalog
        {
            get { return _Catalog; }
            set { _Catalog = value; }
        }

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        public string ZTFCategory
        {
            get { return _ZTFCategory; }
            set { _ZTFCategory = value; }
        }

        public string TIYAN
        {
            get { return _TIYAN; }
            set { _TIYAN = value; }
        }

        public string ContentUrl
        {
            get { return _ContentUrl; }
            set { _ContentUrl = value; }
        }

        public string CoverUrl
        {
            get { return _CoverUrl; }
            set { _CoverUrl = value; }
        }
    }
}