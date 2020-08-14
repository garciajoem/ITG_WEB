using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using sccweb.Models;
using sccweb.Repositories;
using sccweb.ViewModel;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace sccweb.Controllers
{
    [RoutePrefix("Menugroups")]
    [ValidateInput(false)]
    public class MenugroupsController : Controller
    {
        private readonly sccwebEnt db = new sccwebEnt();
        // GET: Menugroups
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.MenugroupPanelActive = "active";
            ViewBag.List = false;

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
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
                s.menuitems
            });

            List<PageViewModel> PageLists = Page.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                Created = item.Created,
                MetaDescription = item.MetaDescription,
                MetaKeywords = item.MetaKeywords,
                Publish = item.Publish,
                AuthorId = item.AuthorId,
                ImageId = item.ImageId,
                NavbarId = item.NavbarId,
                Img = item.Img,
                SubContent = item.SubContent,
                menuitems = item.menuitems
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

            var MenuParent = db.Menugroups.Select(s => new
            {
                s.Id,
                s.IsParent,
                s.ParentId,
                s.Name,
                s.PageId,
                s.PdfId,
                s.ModalId,
                s.ExtlinkId,
                s.Type
            }).Where(s => s.IsParent == 1);

            List<MenugroupViewModel> menuParentItem = MenuParent.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                IsParent = item.IsParent,
                ParentId = item.ParentId,
                Name = item.Name,
                PageId = item.PageId,
                ExtlinkId = item.ExtlinkId,
                ModalId = item.ModalId,
                PdfId = item.PdfId,
                Type = item.Type
            }).ToList();

            var Modal = db.Modals.Select(s => new {
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

            var mi = 0;
            int lastPageId = db.Pages.Max(item => item.Id);
            int lastPdfId = db.Pdfs.Max(item => item.Id);
            int lasteLinkId = db.Extlinks.Max(item => item.Id);
            int lastModalId = db.Modals.Max(item => item.Id);
            ViewBag.PageId = lastPageId + 1;
            ViewBag.PdfId = lastPdfId + 1;
            ViewBag.ElinkId = lasteLinkId + 1;
            ViewBag.ModalId = lastModalId + 1;
            ViewBag.Pages = new SelectList(db.Pages, "Id", "Title");
            ViewBag.Pdfs = new SelectList(db.Pdfs, "Id", "Title");
            ViewBag.Elinks = new SelectList(db.Extlinks, "Id", "Title");
            ViewBag.Modals = new SelectList(db.Modals, "Id", "Title");
            ViewBag.MenuParent = menuParentItem;
            ViewBag.PdfItemLists = PdfItemLists;
            ViewBag.ExLinkItemLists = ExLinkLists;
            ViewBag.PageItemLists = PageLists;
            ViewBag.ModalItemLists = ModalLists;

            foreach (var firstchild in menuParentItem) {
                int parentId = Convert.ToInt32(firstchild.Id);
                var stId = firstchild.Id.ToString();

                var Child = db.Menugroups.Select(s => new
                {
                    s.Id,
                    s.IsParent,
                    s.ParentId,
                    s.Name,
                    s.PageId,
                    s.PdfId,
                    s.ModalId,
                    s.ExtlinkId,
                    s.Type
                }).Where(s => s.ParentId == parentId);

                List<MenugroupViewModel> MenuChildItem = Child.Select(item => new MenugroupViewModel()
                {
                    Id = item.Id,
                    IsParent = item.IsParent,
                    ParentId = item.ParentId,
                    Name = item.Name,
                    PageId = item.PageId,
                    ExtlinkId = item.ExtlinkId,
                    ModalId = item.ModalId,
                    PdfId = item.PdfId,
                    Type = item.Type
                }).ToList();

                ViewData[stId] = MenuChildItem;

                foreach (var secondchild in MenuChildItem) {
                    int firstChildId = Convert.ToInt32(secondchild.Id);
                    var stfirstId = secondchild.Id.ToString();

                    var SecondChild = db.Menugroups.Select(s => new
                    {
                        s.Id,
                        s.IsParent,
                        s.ParentId,
                        s.Name,
                        s.PageId,
                        s.PdfId,
                        s.ModalId,
                        s.ExtlinkId,
                        s.Type
                    }).Where(s => s.ParentId == firstChildId);

                    List<MenugroupViewModel> MenuSecondChildItem = SecondChild.Select(item => new MenugroupViewModel()
                    {
                        Id = item.Id,
                        IsParent = item.IsParent,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        PageId = item.PageId,
                        ExtlinkId = item.ExtlinkId,
                        ModalId = item.ModalId,
                        PdfId = item.PdfId,
                        Type = item.Type
                    }).ToList();

                    ViewData[stfirstId] = MenuSecondChildItem;

                    foreach (var thirdchild in MenuSecondChildItem) {
                        int secondChildId = Convert.ToInt32(thirdchild.Id);
                        var stsecondId = thirdchild.Id.ToString();

                        var ThirdChild = db.Menugroups.Select(s => new
                        {
                            s.Id,
                            s.IsParent,
                            s.ParentId,
                            s.Name,
                            s.PageId,
                            s.PdfId,
                            s.ModalId,
                            s.ExtlinkId,
                            s.Type
                        }).Where(s => s.ParentId == secondChildId);

                        List<MenugroupViewModel> MenuThirdChildItem = ThirdChild.Select(item => new MenugroupViewModel()
                        {
                            Id = item.Id,
                            IsParent = item.IsParent,
                            ParentId = item.ParentId,
                            Name = item.Name,
                            PageId = item.PageId,
                            ExtlinkId = item.ExtlinkId,
                            ModalId = item.ModalId,
                            PdfId = item.PdfId,
                            Type = item.Type
                        }).ToList();

                        ViewData[stsecondId] = MenuThirdChildItem;

                        foreach (var fourthchild in MenuThirdChildItem) {
                            int thirdChildId = Convert.ToInt32(fourthchild.Id);
                            var stthirdId = fourthchild.Id.ToString();

                            var FourthChild = db.Menugroups.Select(s => new
                            {
                                s.Id,
                                s.IsParent,
                                s.ParentId,
                                s.Name,
                                s.PageId,
                                s.PdfId,
                                s.ModalId,
                                s.ExtlinkId,
                                s.Type
                            }).Where(s => s.ParentId == thirdChildId);

                            List<MenugroupViewModel> MenuFourthChildItem = FourthChild.Select(item => new MenugroupViewModel()
                            {
                                Id = item.Id,
                                IsParent = item.IsParent,
                                ParentId = item.ParentId,
                                Name = item.Name,
                                PageId = item.PageId,
                                ExtlinkId = item.ExtlinkId,
                                ModalId = item.ModalId,
                                PdfId = item.PdfId,
                                Type = item.Type
                            }).ToList();

                            ViewData[stthirdId] = MenuFourthChildItem;

                            foreach (var fifthchild in MenuFourthChildItem) {
                                int fourthChildId = Convert.ToInt32(fifthchild.Id);
                                var stfourthId = fifthchild.Id.ToString();

                                var FifthChild = db.Menugroups.Select(s => new
                                {
                                    s.Id,
                                    s.IsParent,
                                    s.ParentId,
                                    s.Name,
                                    s.PageId,
                                    s.PdfId,
                                    s.ModalId,
                                    s.ExtlinkId,
                                    s.Type
                                }).Where(s => s.ParentId == fourthChildId);

                                List<MenugroupViewModel> MenuFifthChildItem = FifthChild.Select(item => new MenugroupViewModel()
                                {
                                    Id = item.Id,
                                    IsParent = item.IsParent,
                                    ParentId = item.ParentId,
                                    Name = item.Name,
                                    PageId = item.PageId,
                                    ExtlinkId = item.ExtlinkId,
                                    ModalId = item.ModalId,
                                    PdfId = item.PdfId,
                                    Type = item.Type
                                }).ToList();

                                ViewData[stfourthId] = MenuFifthChildItem;
                            }
                        }
                    }
                }
            }

            if (Session["UserId"] != null)
            {
                if (menuParentItem != null) { 
                    return View();
                }

                return null;
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        [Route("Index")]
        [HttpPost, ActionName("Index")]
        public ActionResult Index(MenugroupViewModel model, int? menutype, List<string> menugroups) {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.MenugroupPanelActive = "active";
            int pagelistId = 0;
            int pdfId = 0;
            int elinkId = 0;
            int modalId = 0;
            int pageId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["PageId"]);
            int menuId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["menuid"]);
            int parentId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["menuparent"]);
            int isExternal = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["ExLink"]);
            HttpPostedFileBase fileImg = Request.Files["fileImg"];
            HttpPostedFileBase filePdf = Request.Files["filePdf"];

            model.PageModel.Maintext = null;

            if (Request.Form["PageListId"] != "") {
                pagelistId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["PageListId"]);
            }

            if (Request.Form["PdfId"] != "") {
                pdfId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["PdfId"]);
            }

            if (Request.Form["ElinkId"] != "") {
                elinkId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["ElinkId"]);
            }

            if (Request.Form["ModalId"] != "") {
                modalId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["ModalId"]);
            }

            if (fileImg != null)
            {
                model.PageModel.Img = ConvertToBytes(fileImg);
                model.PdfModel.Img = ConvertToBytes(fileImg);
                model.ExtLinkModel.Img = ConvertToBytes(fileImg);
            }
            else
            {
                model.PageModel.Img = null;
                model.PdfModel.Img = null;
                model.ExtLinkModel.Img = null;
            }

            if (filePdf != null)
            {
                var PdfFile = filePdf;
                var PdfFileItem = Regex.Replace(PdfFile.FileName, " ", "-");
                var fileName = System.IO.Path.GetFileName(PdfFileItem);
                var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfFiles/"), fileName);
                PdfFile.SaveAs(path);

                model.PdfModel.FileName = PdfFileItem;
            }
            else
            {
                model.PdfModel.FileName = null;
            }

            switch (menutype)
            {
                case 30:
                    if (menugroups[0] != null)
                    {
                        string v = menugroups[0].ToString();
                        string[] values = v.Split(',');
                        for (int mi = 0; mi < values.Length; mi++)
                        {
                            string vi = values[mi];
                            string[] valueItems = vi.Split('-');
                            //var test = new Menugroup
                            //{
                            //    Name = valueItems[0],
                            //    IsParent = 0,
                            //    ParentId = null,
                            //    Type = 4,
                            //    ModalId = null
                            //};
                            //db.Menugroups.Add(test);

                            //var MenuItemUpdate = db.Menugroups.Find(Convert.ToInt32(valueItems[0]));
                            //if (TryUpdateModel(MenuItemUpdate, "", new string[] { "ParentId", "Order"})) ;
                            //db.Entry(MenuItemUpdate).State = EntityState.Modified;
                        }
                    }
                    
                    break;
                case 27:
                    HttpPostedFileBase SSubModalFileImg = Request.Files["ModalFileImg"];
                    var createssubmodal = new ModalModel
                    {
                        Title = model.ModalModel.Title,
                        Summary = model.ModalModel.Summary,
                        Maintext = model.ModalModel.Maintext,
                        NavbarId = menuId,
                        Img = ConvertToBytes(SSubModalFileImg),
                    };
                    var ssubmenumodalitem = new Menugroup
                    {
                        Name = model.ModalModel.Title,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 4,
                        ModalId = modalId
                    };
                    db.Menugroups.Add(ssubmenumodalitem);
                    db.Modals.Add(createssubmodal);
                    break;
                case 26:
                    HttpPostedFileBase SubModalFileImg = Request.Files["ModalFileImg"];
                    var createsubmodal = new ModalModel
                    {
                        Title = model.ModalModel.Title,
                        Summary = model.ModalModel.Summary,
                        Maintext = model.ModalModel.Maintext,
                        NavbarId = menuId,
                        Img = ConvertToBytes(SubModalFileImg),
                    };
                    var submenumodalitem = new Menugroup
                    {
                        Name = model.ModalModel.Title,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 4,
                        ModalId = modalId
                    };
                    db.Menugroups.Add(submenumodalitem);
                    db.Modals.Add(createsubmodal);
                    break;
                case 25:
                    HttpPostedFileBase ModalFileImg = Request.Files["ModalFileImg"];
                    var createmodal = new ModalModel
                    {
                        Title = model.ModalModel.Title,
                        Summary = model.ModalModel.Summary,
                        Maintext = model.ModalModel.Maintext,
                        NavbarId = menuId,
                        Img = ConvertToBytes(ModalFileImg),
                    };
                    var parentmenumodalitem= new Menugroup
                    {
                        Name = model.ModalModel.Title,
                        IsParent = 1,
                        Type = 4,
                        ModalId = modalId
                    };
                    db.Menugroups.Add(parentmenumodalitem);
                    db.Modals.Add(createmodal);
                    break;
                case 24:
                    var editMenuSss = db.Menugroups.Find(menuId);
                    editMenuSss.Name = Request["menuname"];
                    if (TryUpdateModel(editMenuSss, "", new string[] { "Name" })) ;
                    db.Entry(editMenuSss).State = EntityState.Modified;
                    break;
                case 21:
                    var editMenuSs = db.Menugroups.Find(menuId);
                    editMenuSs.Name = Request["menuname"];
                    if (TryUpdateModel(editMenuSs, "", new string[] { "Name" })) ;
                    db.Entry(editMenuSs).State = EntityState.Modified;
                    break;
                case 18:
                    var editMenuS = db.Menugroups.Find(menuId);
                    editMenuS.Name = Request["menuname"];
                    if (TryUpdateModel(editMenuS, "", new string[] { "Name" })) ;
                    db.Entry(editMenuS).State = EntityState.Modified;
                    break;
                case 15:
                    var editMenuParent = db.Menugroups.Find(menuId);
                    editMenuParent.Name = Request["menuname"];
                    if (TryUpdateModel(editMenuParent, "", new string[] { "Name" }));
                    db.Entry(editMenuParent).State = EntityState.Modified;
                    break;
                case 8:
                    HttpPostedFileBase SubLinkFileImg = Request.Files["LinkFileImg"];

                    var sublinkitem = new ExtLinkModel
                    {
                        Title = model.ExtLinkModel.Title,
                        UrlLink = model.ExtLinkModel.UrlLink,
                        Created = model.ExtLinkModel.Created,
                        Img = ConvertToBytes(SubLinkFileImg),
                        IsExternal = model.ExtLinkModel.IsExternal,
                        NavbarId = menuId
                    };
                    var submenulinkitem = new Menugroup
                    {
                        Name = model.ExtLinkModel.Title,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 3,
                        ExtlinkId = elinkId
                    };
                    db.Menugroups.Add(submenulinkitem);
                    db.Extlinks.Add(sublinkitem);
                    break;
                case 7:
                    var subpdfitem = new PdfModel
                    {
                        Title = model.PdfModel.Title,
                        Summary = model.PdfModel.Summary,
                        Created = model.PdfModel.Created,
                        Img = model.PdfModel.Img,
                        FileName = model.PdfModel.FileName,
                        IsExternal = model.PdfModel.IsExternal,
                        NavbarId = menuId,
                        ExLink = model.PdfModel.ExLink
                    };
                    var submenupdfitem = new Menugroup
                    {
                        Name = model.PdfModel.Title,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 2,
                        PdfId = pdfId
                    };
                    db.Menugroups.Add(submenupdfitem);
                    db.Pdfs.Add(subpdfitem);
                    break;
                case 6:
                    var subpageitem = new PageModel
                    {
                        Title = model.PageModel.Title,
                        Summary = model.PageModel.Summary,
                        Maintext = model.PageModel.Maintext,
                        Created = model.PageModel.Created,
                        MetaDescription = model.PageModel.MetaDescription,
                        MetaKeywords = model.PageModel.MetaKeywords,
                        Publish = model.PageModel.Publish,
                        AuthorId = model.PageModel.AuthorId,
                        ImageId = model.PageModel.ImageId,
                        NavbarId = model.PageModel.NavbarId,
                        SidenavId = menuId,
                        Img = model.PageModel.Img,
                        SubContent = Request["subcontent"],
                        PageUrl = model.PageModel.PageUrl,
                        menuitems = model.PageModel.menuitems
                    };

                    var submenuitem = new Menugroup
                    {
                        Name = model.PageModel.Title,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 1,
                        PageId = pageId
                    };
                    db.Menugroups.Add(submenuitem);
                    db.Pages.Add(subpageitem);
                    break;
                case 5:
                    HttpPostedFileBase LinkFileImg = Request.Files["LinkFileImg"];

                    var createlink = new ExtLinkModel
                    {
                        Title = model.ExtLinkModel.Title,
                        UrlLink = model.ExtLinkModel.UrlLink,
                        Created = model.ExtLinkModel.Created,
                        Img = ConvertToBytes(LinkFileImg),
                        IsExternal = model.ExtLinkModel.IsExternal
                    };
                    var parentmenulinkitem = new Menugroup
                    {
                        Name = model.ExtLinkModel.Title,
                        IsParent = 1,
                        Type = 3,
                        ExtlinkId = elinkId
                    };
                    db.Menugroups.Add(parentmenulinkitem);
                    db.Extlinks.Add(createlink);
                    break;
                case 4:
                    var createpdf = new PdfModel
                    {
                        Title = model.PdfModel.Title,
                        Summary = model.PdfModel.Summary,
                        Created = model.PdfModel.Created,
                        Img = model.PdfModel.Img,
                        FileName = model.PdfModel.FileName,
                        IsExternal = model.PdfModel.IsExternal,
                        ExLink = model.PdfModel.ExLink
                    };
                    var parentmenupdfitem = new Menugroup
                    {
                        Name = model.PdfModel.Title,
                        IsParent = 1,
                        Type = 2,
                        PdfId = pdfId
                    };
                    db.Menugroups.Add(parentmenupdfitem);
                    db.Pdfs.Add(createpdf);
                    break;
                case 3:
                    var createpage = new PageModel
                    {
                        Title = model.PageModel.Title,
                        Summary = model.PageModel.Summary,
                        Maintext = model.PageModel.Maintext,
                        Created = model.PageModel.Created,
                        MetaDescription = model.PageModel.MetaDescription,
                        MetaKeywords = model.PageModel.MetaKeywords,
                        Publish = model.PageModel.Publish,
                        AuthorId = model.PageModel.AuthorId,
                        ImageId = model.PageModel.ImageId,
                        NavbarId = model.PageModel.NavbarId,
                        SidenavId = menuId,
                        Img = model.PageModel.Img,
                        SubContent = Request["subcontent"],
                        PageUrl = model.PageModel.PageUrl,
                        menuitems = model.PageModel.menuitems
                    };

                    var parentmenuitem = new Menugroup
                    {
                        Name = model.PageModel.Title,
                        IsParent = 1,
                        Type = 1,
                        PageId = pageId
                    };
                    db.Menugroups.Add(parentmenuitem);
                    db.Pages.Add(createpage);
                    break;
                case 2:
                    var firstchild = new Menugroup
                    {
                        Name = model.Name,
                        IsParent = 0,
                        ParentId = parentId,
                        Type = 1,
                        PageId = pagelistId
                    };
                    db.Menugroups.Add(firstchild);
                    break;
                case 1:
                    var parent = new Menugroup
                    {
                        Name = model.Name,
                        IsParent = 1,
                        Type = 1,
                        PageId = pagelistId
                    };
                    db.Menugroups.Add(parent);
                    break;
                default:
                    var parentmenu = new Menugroup
                    {
                        Name = model.Name,
                        IsParent = 1,
                        Type = 1,
                        PageId = pageId
                    };
                    db.Menugroups.Add(parentmenu);
                    break;
            }

            int i = db.SaveChanges();
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult PdfFiles(string PdfId)
        {
            int Id = Convert.ToInt32(PdfId);
            var PdfFile = db.Pdfs.Find(Id);

            if (PdfFile == null)
            {
                return new HttpNotFoundResult();
            }

            return Content("~/Content/PdfFiles/" + PdfFile.FileName);
        }

        private void PopulatePagesDropDownList(object selectedPage = null)
        {
            var pagesQuery = from p in db.Pages
                             orderby p.Title
                             select p;
            ViewBag.PageLists = new SelectList(pagesQuery, "Id", "Title", selectedPage);
        }

        

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            var reader = new System.IO.BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        

        public ActionResult LinkImages(int id)
        {
            byte[] cover = GetLinkImageFromDataBase(id);
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

        public byte[] GetLinkImageFromDataBase(int Id)
        {
            var q = from temp in db.Extlinks where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }
        public ActionResult Delete(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.MenugroupPanelActive = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Menugroup = db.Menugroups.Find(id);
            if (Menugroup == null)
            {
                return HttpNotFound();
            }

            return View(Menugroup);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Menugroup =  db.Menugroups.Find(id);
            if (Menugroup == null)
            {
                return HttpNotFound();
            }

            db.Menugroups.RemoveRange(db.Menugroups.Where(c => c.ParentId == id));
            db.Menugroups.RemoveRange(db.Menugroups.Where(c => c.Id == id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}