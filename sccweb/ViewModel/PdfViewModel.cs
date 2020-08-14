using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itgweb.ViewModel
{
    public class PdfViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> Publish { get; set; }
        public Nullable<int> Author { get; set; }
        public byte[] Img { get; set; }
        public string FileName { get; set; }
        public Nullable<int> NavbarId { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> IsExternal { get; set; }
        public string ExLink { get; set; }
    }
}