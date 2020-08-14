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

namespace itgweb.Controllers
{
    [RoutePrefix("Authors")]
    [ValidateInput(false)]
    public class AuthorsController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();
        // GET: Authors
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.AuthorPanelActive = "active";

            var Author = db.Authors.Select(s => new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Position,
                s.Summary,
                s.Biography,
                s.Created,
                s.ImageId,
                s.Email,
                s.Phone,
                s.Img
            });

            List<AuthorViewModel> AuthorModel = Author.Select(item => new AuthorViewModel()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Position = item.Position,
                Summary = item.Summary,
                Biography = item.Biography,
                Created = item.Created,
                ImageId = item.ImageId,
                Email = item.Email,
                Phone = item.Phone,
                Img = item.Img
            }).ToList();
            return View(AuthorModel);
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

        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Authors where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.AuthorPanelActive = "active";
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(AuthorViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.AuthorPanelActive = "active";
            HttpPostedFileBase file = Request.Files["upload"];
            AuthorRepository service = new AuthorRepository();
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}