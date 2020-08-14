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

namespace itgweb.Controllers
{
    [RoutePrefix("Pdfs")]
    [ValidateInput(false)]

    public class PdfsController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();
        // GET: Pdfs
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;

            var Pdf = db.Pdfs.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Created,
                s.Publish,
                s.Author,
                s.ImageId,
                s.NavbarId,
                s.Img,
                s.FileName,
                s.IsExternal,
                s.ExLink
            });

            List<PdfViewModel> pdfModel = Pdf.Select(item => new PdfViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Created = item.Created,
                Publish = item.Publish,
                Author = item.Author,
                ImageId = item.ImageId,
                NavbarId = item.NavbarId,
                Img = item.Img,
                FileName = item.FileName,
                IsExternal = item.IsExternal,
                ExLink = item.ExLink
            }).ToList();
            return View(pdfModel);
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

        public ActionResult PdfImages(int id)
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
            var q = from temp in db.Pdfs where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> file, PdfViewModel PdfViewModel)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;

            var PdfFile = file.ElementAt(1);
            var PdfFileItem = Regex.Replace(PdfFile.FileName, " ", "-");
            var fileName = System.IO.Path.GetFileName(PdfFileItem);
            var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfFiles/"), fileName);
            PdfFile.SaveAs(path);

            if (file.ElementAt(0) != null)
            {
                PdfViewModel.Img = ConvertToBytes(file.ElementAt(0));
            }
            else {
                PdfViewModel.Img = null;
            }
            
            var pdf = new Pdf
            {
                Title = PdfViewModel.Title,
                Summary = PdfViewModel.Summary,
                Created = PdfViewModel.Created,
                Publish = PdfViewModel.Publish,
                Author = PdfViewModel.Author,
                ImageId = PdfViewModel.ImageId,
                NavbarId = PdfViewModel.NavbarId,
                IsExternal = PdfViewModel.IsExternal,
                ExLink = PdfViewModel.ExLink,
                Img = PdfViewModel.Img,
                FileName = PdfFileItem
            };
            db.Pdfs.Add(pdf);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(PdfViewModel);
        }
        

        public ActionResult Edit(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PdfItem = db.Pdfs.Find(id);
            if (PdfItem == null)
            {
                return HttpNotFound();
            }

            if (PdfItem.Img != null)
            {
                ViewBag.Img = true;
            }
            return View(PdfItem);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, PdfViewModel model, MenugroupViewModel mgmodel)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            HttpPostedFileBase fileImg = Request.Files["fileImg"];
            HttpPostedFileBase filePdf = Request.Files["filePdf"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PdfItem = db.Pdfs.Find(id);
            var MenuItem = db.Menugroups.Where(mi => mi.PdfId == id);
            if (PdfItem == null)
            {
                return HttpNotFound();
            }

            if (fileImg != null)
            {
                model.Img = ConvertToBytes(fileImg);
            }
            else
            {
                model.Img = null;
            }

            if (PdfItem.FileName != null)
            {
                var PdfFile = filePdf;
                var PdfFileItem = Regex.Replace(PdfFile.FileName, " ", "-");
                var fileName = System.IO.Path.GetFileName(PdfFileItem);
                var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfFiles/"), fileName);
                PdfFile.SaveAs(path);

                model.FileName = PdfFileItem;

                if (TryUpdateModel(PdfItem, "", new string[] { "Title", "Summary", "Created", "Publish", "Author", "Img", "Filename", "NavbarId", "ImageId", "IsExternal", "Exlink" })) ;
            }
            else
            {
                if (TryUpdateModel(PdfItem, "", new string[] { "Title", "Summary", "Created", "Publish", "Author", "Img", "NavbarId", "ImageId", "IsExternal", "Exlink" }));
            }

            db.Entry(PdfItem).State = EntityState.Modified;
            int i = db.SaveChanges();

            return RedirectToAction("Index");
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            var reader = new System.IO.BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            ViewBag.Footer = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Pdf = db.Pdfs.Find(id);
            if (Pdf == null)
            {
                return HttpNotFound();
            }

            return View(Pdf);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Pdf = db.Pdfs.Find(id);
            if (Pdf == null)
            {
                return HttpNotFound();
            }

            db.Pdfs.Remove(Pdf);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}