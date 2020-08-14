using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itgweb.Models;

namespace itgweb.ViewModel
{
    public class PageModel : Page { }
    public class PdfModel : Pdf { }
    public class ExtLinkModel : Extlink { }
    public class ModalModel : Modal { }
    public class MenugroupViewModel
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
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Pdf> Pdfs { get; set; }
        public virtual ICollection<Extlink> Extlinks { get; set; }
        public virtual ICollection<Menugroup> Menugroups { get; set; }
        public PageModel PageModel { get; set; }
        public PdfModel PdfModel { get; set; }
        public ExtLinkModel ExtLinkModel { get; set; }
        public ModalModel ModalModel { get; set; }
        public MenugroupViewModel() {
            ModalModel = new ModalModel();
            PageModel = new PageModel();
            PdfModel = new PdfModel();
            ExtLinkModel = new ExtLinkModel();
        }
    }
}