using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itgweb.Models
{
    public class Menugroup
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> IsParent { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> PageId { get; set; }
        public Nullable<int> ExtlinkId { get; set; }
        public Nullable<int> PdfId { get; set; }
        public Nullable<int> ModalId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public virtual Page Pages { get; set; }
        public virtual Extlink Extlinks { get; set; }
        public virtual Pdf Pdfs { get; set; }
        public virtual Modal Modals { get; set; }
        public ICollection<Page> Pagelists { get; set; }
        public ICollection<Pdf> PdfLists { get; set; }
        public ICollection<Extlink> ExtLinkLists { get; set; }
        public ICollection<Modal> ModalLists { get; set; }
    }
}