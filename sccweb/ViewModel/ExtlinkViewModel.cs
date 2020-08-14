using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itgweb.Models;

namespace itgweb.ViewModel
{
    public class ExtlinkViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlLink { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> Author { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> NavbarId { get; set; }
        public string Icon { get; set; }
        public byte[] Img { get; set; }
        public Nullable<int> IsExternal { get; set; }
    }
}