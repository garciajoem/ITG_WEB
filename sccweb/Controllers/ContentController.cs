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
    [RoutePrefix("Content")]
    public class ContentController : Controller
    {
        private readonly itgwebEnt db = new itgwebEnt();

        public ActionResult PdfFiles(string PdfId)
        {
            int Id = Convert.ToInt32(PdfId);
            var PdfFile = db.Pdfs.Find(Id);

            if(PdfFile == null)
            {
                return new HttpNotFoundResult();
            }

            return Content("~/Content/PdfFiles/"+PdfFile.FileName);
        }
    }
}