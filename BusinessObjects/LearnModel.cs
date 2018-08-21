using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BusinessObjects
{
    [Serializable()]
    public class LearnWrapper
    {
        public string SearchText { get; set; }
        public string SelectedTopic { get; set; }
        public List<Topics> TopicList { get; set; }      //  Webinar, News, Articles etc...
        public string SelectedExperienceLevel { get; set; }
        public List<Experiences> ExperienceLevelList { get; set; }     // Education level begin, moderate, advanced
        public string SelectedLearnType { get; set; }    // Education or Resources
        public List<LearnModel> LearnList { get; set; }
        public IPagedList<LearnModel> LearnPagedList { get; set; }
        public IPagedList<LearnModel> RecentLearnList { get; set; }
        public List<LearnModel> LatestSimillarArticleList { get; set; }
        public string SortingOrder { get; set; }
    }

    [Serializable()]
    public class LearnModel
    {
        public long ID { get; set; }

        [Required]
      //  [RegularExpression("^[a-zA-Z0-9\\s]+", ErrorMessage = "Special symbol is not allowed in resource name")]
        [MaxLength(500)]
        public string Name { get; set; }

        [AllowHtml]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Level")]
        [Required]
        public string ExperienceLevelID { get; set; }

        public string ExperienceLevelName { get; set; }

        public List<Experiences> ExperienceLevelList { get; set; }

        [Display(Name = "Topic")]
        [Required]
        public int TopicID { get; set; }

        public List<Topics> TopicList { get; set; }
                           
        [Display(Name = "Url")]
        public string Url { get; set; }

        public bool IsSponsored { get; set; }

        public bool IsActive { get; set; }

        public long UserID { get; set; }

        [Display(Name = "Type")]
        public int LearnTypeID { get; set; }            // Education and Resources

        public enum ContentTypeList
        {
            Free = 0,
            Registered = 1,
            Paid = 2

        }

        [Display(Name = "Content Type")]
        public ContentTypeList ContentType
        {
            get; set;
        }

        public string ContentForName
        {
            get; set;
        }
        public List<LearnType> LearnTypeList { get; set; }
        public int ListingStatusID { get; set; }
        public string Status { get; set; }
        public string ResourcePath { get; set; }
        public List<Documents> DocumentList { get; set; }
        public List<Images> ImageList { get; set; }
        public string LearnTypeName { get; set; }
        public string TopicName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Article Date")]
        public string ArticleDate { get; set; }

        [Display(Name = "Author Name")]
        [RegularExpression("^[a-zA-Z\\s]+", ErrorMessage = "Please enter valid Author Name")]
        public string AuthorName { get; set; }
        public List<LearnModel> LatestSimillarArticleList { get; set; }
        public int ContentTypeID { get; set; }
        public string ImagePath { get; set; }
        public int ContentForID { get; set; }
        public string LearnStatusID { get; set; }

    }
    public enum ContentFor
    {
        [Display(Name = "Investor")]
        Investor = 1,
        [Display(Name = "Broker")]
        Broker = 2,
        [Display(Name = "Investor and Broker")]
        All = 3
    }

    [Serializable()]
    public class Topics
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TopicIcon { get; set; }
    }

    [Serializable()]
    public class Experiences
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Serializable()]
    public class LearnType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Serializable()]
    public class Documents
    {
        public long id { get; set; }
        public string FileName { get; set; }
        public bool IsDeleted { get; set; }
    }

    [Serializable()]
    public class Images
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
    }

    [Serializable()]
    public class LearnHomeModel
    {
        public int id { get; set; }
        public string SearchText { get; set; }
        public List<Experiences> ExperienceLevelList { get; set; }
        public List<Topics> TopicList { get; set; }
        public List<Topics> ResourceTopicList { get; set; }
    }
}