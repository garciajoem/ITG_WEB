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
    [RoutePrefix("Extlinks")]
    [ValidateInput(false)]
    public class ExtlinksController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();
        // GET: Extlinks
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            ViewBag.Footer = false;

            var Extlink = db.Extlinks.Select(s => new
            {
                s.Id,
                s.Title,
                s.UrlLink,
                s.Created,
                s.Author,
                s.ImageId,
                s.NavbarId,
                s.Img,
                s.IsExternal
            });

            List<ExtlinkViewModel> ExtlinkModel = Extlink.Select(item => new ExtlinkViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                UrlLink = item.UrlLink,
                Created = item.Created,
                Author = item.Author,
                ImageId = item.ImageId,
                NavbarId = item.NavbarId,
                Img = item.Img,
                IsExternal = item.IsExternal
            }).ToList();
            return View(ExtlinkModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            ViewBag.Footer = false;
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(ExtlinkViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            ViewBag.Footer = false;
            HttpPostedFileBase file = Request.Files["upload"];
            ExtlinkRepository service = new ExtlinkRepository();
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int? id) {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            ViewBag.Footer = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ELink = db.Extlinks.Find(id);
            if (ELink == null)
            {
                return HttpNotFound();
            }

            if (ELink.Img != null)
            {
                ViewBag.Img = true;
            }
            return View(ELink);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, ExtlinkViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UrlItem = db.Extlinks.Find(id);
            if (UrlItem == null)
            {
                return HttpNotFound();
            }

            if (Request.Files["fileImg"] != null)
            {
                HttpPostedFileBase file = Request.Files["fileImg"];

                UrlItem.Img = ConvertToBytes(file);
                if (TryUpdateModel(UrlItem, "", new string[] { "Title", "UrlLink", "Created", "Author", "ImageId", "NavbarId", "Img", "IsExternal"})) ;
            }
            else
            {
                if (TryUpdateModel(UrlItem, "", new string[] { "Title", "UrlLink", "Created", "Author", "ImageId", "NavbarId", "IsExternal", })) ;
            }


            db.Entry(UrlItem).State = EntityState.Modified;
            int i = db.SaveChanges();

            byte[] ConvertToBytes(HttpPostedFileBase image)
            {
                byte[] imageBytes = null;
                var reader = new System.IO.BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
                return imageBytes;
            }

            return RedirectToAction("Index");
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

        public ActionResult LinkImages(int id)
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

        public ActionResult ModalImages(int id)
        {
            byte[] cover = GetModalImageFromDataBase(id);
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
        public byte[] GetModalImageFromDataBase(int Id)
        {
            var q = from temp in db.Modals where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Extlinks where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            ViewBag.Footer = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ExtLink = db.Extlinks.Find(id);
            if (ExtLink == null)
            {
                return HttpNotFound();  
            }

            return View(ExtLink);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ExtLink = db.Extlinks.Find(id);
            if (ExtLink == null)
            {
                return HttpNotFound();
            }

            db.Extlinks.Remove(ExtLink);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}