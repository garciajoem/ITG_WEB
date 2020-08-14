using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itgweb.ViewModel
{
    public class AuthorViewModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Summary { get; set; }
        public string Biography { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> ImageId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] Img { get; set; }
    }
}