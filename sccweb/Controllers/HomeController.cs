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
    public class HomeController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();

        public ActionResult Index()
        {
            ViewBag.Footer = true;
            ViewBag.Header = true;
            ViewBag.Sidenav = false;

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

            var HomeItems = db.Menugroups.Select(s => new
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
            }).Where(s => s.ParentId == 2).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> HomeMenuLists = HomeItems.Select(item => new MenugroupViewModel()
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

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
                s.Icon,
            }).Where(s => s.Publish == 1);

            List<PageViewModel> PageLists = Page.Select(item => new PageViewModel()
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

            var MenuHeader = db.Menugroups.Select(s => new
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
            }).Where(s => s.Name == "Header");

            List<MenugroupViewModel> MenuItemHeader = MenuHeader.Select(item => new MenugroupViewModel()
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

            int HeaderParentId = 0;

            foreach (var HeaderParent in MenuItemHeader)
            {
                HeaderParentId = Convert.ToInt32(HeaderParent.Id);
            }

            var HeaderItems = db.Menugroups.Select(s => new
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
            }).Where(s => s.ParentId == HeaderParentId).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuHeaderItems = HeaderItems.Select(item => new MenugroupViewModel()
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

            foreach (var FooterParent in MenuItemFooter) {
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
            ViewBag.MenuHeader = MenuItemHeader;
            ViewBag.MenuFooterItems = MenuFooterItems;
            ViewBag.MenuHeaderItems = MenuHeaderItems;
            ViewBag.ModalItems = ModalLists;
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;
            ViewBag.HomeItems = HomeMenuLists;


            foreach (var ModalPanel in ModalLists) {
                if (ModalPanel.Maintext != null) {
                    var stfirstId = ModalPanel.Id.ToString();
                    List<string> modes = new List<string>();
                    string v = ModalPanel.Maintext.ToString();
                    string[] values = v.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        modes.Add(values[i].Trim());
                    }
                    ViewData[stfirstId] = modes;
                }
            }

            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}