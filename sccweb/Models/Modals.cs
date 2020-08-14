using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itgweb.Models
{
    public class Modal
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        [AllowHtml]
        public string Maintext { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> Author { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> NavbarId { get; set; }
        public string Icon { get; set; }
        public byte[] Img { get; set; }
    }
}