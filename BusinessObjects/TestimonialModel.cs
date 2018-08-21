using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BusinessObjects
{
    [Serializable()]
    public class TestimonialModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        public string TestimonialImagePath { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    [Serializable()]
    public class TestimonialModelWrapper
    {
        public IPagedList<TestimonialModel> TestimonialPagedList { get; set; }
        public List<TestimonialModel> TestimonialList { get; set; }
    }
}