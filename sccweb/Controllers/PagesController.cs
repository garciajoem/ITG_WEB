using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using itgweb.Models;
using itgweb.Repositories;
using itgweb.ViewModel;
using System.Web.UI;
using System.Text.RegularExpressions;
using PagedList.Mvc;
using PagedList;

namespace itgweb.Controllers
{
    [RoutePrefix("Pages")]
    [ValidateInput(false)]
    public class PagesController : Controller
    {
        // GET: Pages

        private readonly itgwebEnt db = new itgwebEnt();
        /// <summary>
        /// Retrive content from database 
        /// </summary>
        /// <returns></returns>
        /// 

        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {

            ViewBag.PageEdit = true;
            ViewBag.Footer = false;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.ShowSum,
                s.Maintext,
                s.Created,
                s.MetaDescription,
                s.MetaKeywords,
                s.Publish,
                s.AuthorId,
                s.ImageId,
                s.NavbarId,
                s.SidenavId,
                s.Img,
                s.SubContent,
                s.PageUrl,
                s.menuitems
            });

            List<PageViewModel> pageModel = Page.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                ShowSum = item.ShowSum,
                Maintext = item.Maintext,
                Created = item.Created,
                MetaDescription = item.MetaDescription,
                MetaKeywords = item.MetaKeywords,
                Publish = item.Publish,
                AuthorId = item.AuthorId,
                ImageId = item.ImageId,
                NavbarId = item.NavbarId,
                SidenavId = item.SidenavId,
                Img = item.Img,
                PageUrl = item.PageUrl,
                SubContent = item.SubContent,
                menuitems = item.menuitems
            }).ToList();

            var MenuItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.SortOrder
            }).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> SubMenuLists = MenuItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId
            }).ToList();

            var PdfItems = db.Pdfs.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.Img,
                s.FileName,
                s.NavbarId,
                s.IsExternal,
                s.ExLink
            });

            List<PdfViewModel> PdfItemLists = PdfItems.Select(item => new PdfViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                Img = item.Img,
                FileName = item.FileName,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
                ExLink = item.ExLink
            }).ToList();

            var ExLinkItems = db.Extlinks.Select(s => new
            {
                s.Id,
                s.Title,
                s.UrlLink,
                s.Img,
                s.NavbarId,
                s.IsExternal,
            });

            List<ExtlinkViewModel> ExLinkLists = ExLinkItems.Select(item => new ExtlinkViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                UrlLink = item.UrlLink,
                Img = item.Img,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
            }).ToList();

            var PageItems = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
                s.Icon
            });

            List<PageViewModel> PageLists = PageItems.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                ImageId = item.ImageId,
                Img = item.Img,
                Icon = item.Icon
            }).ToList();
            var Modal = db.Modals.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.NavbarId,
                s.Img,
                s.Icon
            });

            List<ModalViewModel> ModalLists = Modal.Select(item => new ModalViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                NavbarId = item.NavbarId,
                Img = item.Img,
                Icon = item.Icon
            }).ToList();

            var MenuFooter = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.Name == "Footer");

            List<MenugroupViewModel> MenuItemFooter = MenuFooter.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            int FooterParentId = 0;

            foreach (var FooterParent in MenuItemFooter)
            {
                FooterParentId = Convert.ToInt32(FooterParent.Id);
            }

            var FooterItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.ParentId == FooterParentId).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuFooterItems = FooterItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            ViewBag.MenuFooter = MenuItemFooter;
            ViewBag.MenuFooterItems = MenuFooterItems;
            ViewBag.ModalItems = ModalLists;
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;

            foreach (var ModalPanel in ModalLists)
            {
                if (ModalPanel.Maintext != null)
                {
                    List<string> modes = new List<string>();
                    string v = ModalPanel.Maintext.ToString();
                    string[] values = v.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        modes.Add(values[i].Trim());
                    }
                    ViewBag.ModalPanel = modes;
                }
            }

            if (Session["UserId"] != null)
            {
                return View(pageModel);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult PageImages(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Pages where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            PopulateMenuDropDownList();

            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        /// <summary>
        /// Save content and images
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PageViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            HttpPostedFileBase file = Request.Files["upload"];
            PageRepository service = new PageRepository();

            List<string> modes = new List<string>();
            string v = Request["subcontent"];
            string[] values = v.Split(',');
            for (int vi = 0; vi < values.Length; vi++)
            {
                modes.Add(values[vi].Trim());
            }

            model.SubContent = Request["subcontent"];
            if (Request.Form["MenuParents"].ToString() != "")
            {
                model.SidenavId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["MenuParents"]);
            }
            else 
            {
                model.SidenavId = null;
            }
            
            
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [Route("Detail")]
        [HttpGet]

        public ActionResult Detail(int? id, string pageName, User objUser)
        {
            ViewBag.Imgbg = true;
            ViewBag.Class = "page-item";
            ViewBag.PagePanelActive = "active";
            ViewBag.Footer = true;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PagesView = db.Pages.Find(id);
            if (PagesView == null)
            {
                return HttpNotFound();
            }

            if (PagesView.Summary != null)
            {
                ViewBag.Summary = PagesView.Summary;
            }
            else
            {
                ViewBag.Summary = "";
            }

            if (PagesView.Img != null)
            {
                ViewBag.Img = true;
            }
            else
            {
                ViewBag.Img = false;
            }

            if (Session["UserId"] != null)
            {
                ViewBag.Userlogin = true;
            }
            else
            {
                ViewBag.Userlogin = false;
            }

            var ParentMenu = db.Menugroups.Select(s => new { 
                s.Id,
                s.Name,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.PageId,
                s.PdfId,
                s.ExtlinkId,
                s.ModalId
            }).Where(s => s.Id == PagesView.SidenavId && s.Id != 1 && s.Id != 2);

            List<MenugroupViewModel> ParentMenuItem = ParentMenu.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            var Menus = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.SortOrder
            }).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> SubMenuLists = Menus.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                SortOrder = item.SortOrder
            }).ToList();

            var PdfItems = db.Pdfs.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.Img,
                s.FileName,
                s.NavbarId,
                s.IsExternal,
                s.ExLink
            });

            List<PdfViewModel> PdfItemLists = PdfItems.Select(item => new PdfViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                Img = item.Img,
                FileName = item.FileName,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
                ExLink = item.ExLink
            }).ToList();

            var ExLinkItems = db.Extlinks.Select(s => new
            {
                s.Id,
                s.Title,
                s.UrlLink,
                s.Img,
                s.NavbarId,
                s.IsExternal,
                s.Icon
            });

            List<ExtlinkViewModel> ExLinkLists = ExLinkItems.Select(item => new ExtlinkViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                UrlLink = item.UrlLink,
                Img = item.Img,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
                Icon = item.Icon
            }).ToList();

            var MenuItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.ParentId == PagesView.SidenavId).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuitemList = MenuItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            var PageItems = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
            });

            List<PageViewModel> PageLists = PageItems.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                ImageId = item.ImageId,
                Img = item.Img
            }).ToList();

            var MenuItem = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.Type,
                s.IsParent,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.PageId == PagesView.Id).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuNav = MenuItem.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                ParentId = item.ParentId,
                Type = item.Type,
                IsParent = item.IsParent,
                PageId = item.PageId,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            var Modal = db.Modals.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.NavbarId,
                s.Img,
                s.Icon
            });

            List<ModalViewModel> ModalLists = Modal.Select(item => new ModalViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                NavbarId = item.NavbarId,
                Img = item.Img,
                Icon = item.Icon
            }).ToList();

            var MenuFooter = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.Name == "Footer");

            List<MenugroupViewModel> MenuItemFooter = MenuFooter.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            int FooterParentId = 0;

            foreach (var FooterParent in MenuItemFooter)
            {
                FooterParentId = Convert.ToInt32(FooterParent.Id);
            }

            var FooterItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.ParentId == FooterParentId).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuFooterItems = FooterItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            ViewBag.MenuFooter = MenuItemFooter;
            ViewBag.MenuFooterItems = MenuFooterItems;
            ViewBag.ModalItems = ModalLists;
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.MenuItems = MenuitemList;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;
            ViewBag.ParentMenu = ParentMenuItem;
            ViewBag.HasChild = false;
            ViewBag.MenuNavItem = MenuNav;
            ViewBag.MenuNavId = "";

            foreach (var ModalPanel in ModalLists)
            {
                if (ModalPanel.Maintext != null)
                {
                    List<string> modalmodes = new List<string>();
                    string modalv = ModalPanel.Maintext.ToString();
                    string[] modalvalues = modalv.Split(',');
                    for (int i = 0; i < modalvalues.Length; i++)
                    {
                        modalmodes.Add(modalvalues[i].Trim());
                    }
                    ViewBag.ModalPanel = modalmodes;
                }
            }

            var MenuNavItemId = "";
            int MenuNavItemIdInt = 0;

            foreach (var MenuNavItem in MenuNav) {
                MenuNavItemId = MenuNavItem.Id.ToString();
                MenuNavItemIdInt = MenuNavItem.Id;
            }

            foreach (var MenuLists in ViewBag.SubMenus) {
                if (MenuLists.ParentId != null) {
                    if (MenuLists.ParentId.ToString() == MenuNavItemId)
                    {
                        ViewBag.HasChild = true;

                        break;
                    }
                }
            }

            var ChildItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.Type,
                s.IsParent,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.ParentId == MenuNavItemIdInt).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> ChildMenus = ChildItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                ParentId = item.ParentId,
                Type = item.Type,
                IsParent = item.IsParent,
                PageId = item.PageId,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId,
                SortOrder = item.SortOrder
            }).ToList();

            var ParentItem = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.Type,
                s.IsParent,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.PageId == PagesView.Id).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> Parent = ParentItem.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                ParentId = item.ParentId,
                Type = item.Type,
                IsParent = item.IsParent,
                PageId = item.PageId,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId,
                SortOrder = item.SortOrder
            }).ToList();

            ViewBag.Parent = Parent;

            foreach (var Child in Parent) {
                int parentId = Convert.ToInt32(Child.ParentId);
                var stId = Child.ParentId.ToString();
                int cmId = 0;
                var stcmId = "";

                var ChildMItems = db.Menugroups.Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.PageId,
                    s.ParentId,
                    s.Type,
                    s.IsParent,
                    s.ExtlinkId,
                    s.PdfId,
                    s.ModalId,
                    s.SortOrder
                }).Where(s => s.Id == parentId).OrderBy(s => s.SortOrder);

                List<MenugroupViewModel> CMenuItems = ChildMItems.Select(item => new MenugroupViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Type = item.Type,
                    IsParent = item.IsParent,
                    PageId = item.PageId,
                    ExtlinkId = item.ExtlinkId,
                    PdfId = item.PdfId,
                    ModalId = item.ModalId,
                    SortOrder = item.SortOrder
                }).ToList();

                ViewData[stId] = CMenuItems;
            }

            ViewBag.ChildMenu = ChildMenus;
            if (PagesView.Summary != null)
            {
                ViewBag.ShowSum = PagesView.ShowSum;
            }
            else {
                ViewBag.ShowSum = 0;
            }

            List<string> modes = new List<string>();
            string v = PagesView.SubContent.ToString();
            string[] values = v.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                modes.Add(values[i].Trim());
            }
            ViewBag.Subcontent = modes;

            if (PagesView.ShowSum != 0 && (ViewBag.HasChild == true || ParentMenu != null))
            {
                ViewBag.Col = "col-sm-9";
                ViewBag.HideCol = false;
            }
            else
            {
                ViewBag.Col = "col-12";
                ViewBag.HideCol = true;
            }

            return View(PagesView);
        }

        
        public ActionResult Edit(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            ViewBag.Img = false;
            ViewBag.Footer = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            if (Page.Img != null) {
                ViewBag.Img = true;
            }

            if (Page.SubContent != null)
            {
                List<string> modes = new List<string>();
                string v = Page.SubContent.ToString();
                string[] values = v.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    modes.Add(values[i].Trim());
                }
                ViewBag.Subcontent = modes;
            }
            else 
            {
                ViewBag.Subcontent = null;
            }

            PopulateMenuDropDownList(Page.SidenavId);

            if (Session["UserId"] != null)
            {
                return View(Page);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, PageViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            Page.SubContent = Request["subcontent"];

            if (Request.Files["upload"] != null)
            {
                HttpPostedFileBase file = Request.Files["upload"];

                Page.Img = ConvertToBytesEdit(file);

                if (TryUpdateModel(Page, "", new string[] { "Title", "Summary", "ShowSum", "Maintext", "Created", "MetaDescription", "MetaKeywords", "Publish", "AuthorId", "ImageId", "NavbarId", "SidenavId", "Img", "menuitems"}));
            }
            else 
            {
                if (TryUpdateModel(Page, "", new string[] { "Title", "Summary", "ShowSum", "Maintext", "Created", "MetaDescription", "MetaKeywords", "Publish", "AuthorId", "ImageId", "NavbarId", "SidenavId", "menuitems" }));
            }

            db.Entry(Page).State = EntityState.Modified;
            int i = db.SaveChanges();

            byte[] ConvertToBytesEdit(HttpPostedFileBase image)
            {
                byte[] imageBytes = null;
                var reader = new System.IO.BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
                return imageBytes;
            }

            return RedirectToAction("Index");
        }

        private void PopulatePagesDropDownList(object selectedPage = null)
        { 
            var pagesQuery = from p in db.Pages
                             orderby p.Title
                             select p;
            ViewBag.PageID = new SelectList(pagesQuery, "Id", "Title", selectedPage);
        }

        private void PopulateMenuDropDownList(object selectedMenu = null)
        {
            var menusQuery = from p in db.Menugroups
                             where p.IsParent == 1
                             orderby p.Name
                             select p;
            ViewBag.MenuParents = new SelectList(menusQuery, "Id", "Name", selectedMenu);
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            ViewBag.Footer = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            return View(Page);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            db.Pages.Remove(Page);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

