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
using System.Web.Security;
using itgweb.Repositories;
using itgweb.ViewModel;

namespace itgweb.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();

        [Route("Login")]
        public ActionResult Login() {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard admin-login";

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
                PdfId = item.PdfId,
                ModalId = item.ModalId
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
            }).Where(s => s.Publish == 1);

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

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
            }).Where(s => s.Publish == 1);

            List<PageViewModel> PageLists = Page.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                ImageId = item.ImageId,
                Img = item.Img
            }).ToList();

            var Modal = db.Modals.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.NavbarId,
                s.Img
            });

            List<ModalViewModel> ModalLists = Modal.Select(item => new ModalViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                NavbarId = item.NavbarId,
                Img = item.Img
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
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;
            ViewBag.ModalItems = ModalLists;


            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard";
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
                PdfId = item.PdfId,
                ModalId = item.ModalId
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
            }).Where(s => s.Publish == 1);

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

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
            }).Where(s => s.Publish == 1);

            List<PageViewModel> PageLists = Page.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                ImageId = item.ImageId,
                Img = item.Img
            }).ToList();

            var Modal = db.Modals.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.NavbarId,
                s.Img
            });

            List<ModalViewModel> ModalLists = Modal.Select(item => new ModalViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                NavbarId = item.NavbarId,
                Img = item.Img
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
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;
            ViewBag.ModalItems = ModalLists;


            if (ModelState.IsValid)
            {
                if (ValidateUser(objUser.Username, objUser.Password))
                {
                    FormsAuthentication.SetAuthCookie(objUser.Username, false);
                    return RedirectToAction("AdminDashboard");
                }
                else
                {
                    ViewBag.Class = "admin admin-dashboard admin-login";
                    ModelState.AddModelError("", "");
                }

                return View(objUser);
            }

            return null;
        }

        private bool ValidateUser(string Username, string Password)
        {

            bool isValid = false;

            using (itgwebEnt db = new itgwebEnt())
            {
                var obj = db.Users.Where(a => a.Username.Equals(Username) && a.Password.Equals(Password)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserId"] = obj.Id;
                    Session["Username"] = obj.Username;

                    isValid = true;
                }

            }

            return isValid;
        }

        // GET: Admin
        public ActionResult AdminDashboard()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard";

            if (Session["UserId"] != null)
            {
                return Redirect("~/Menugroups");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}