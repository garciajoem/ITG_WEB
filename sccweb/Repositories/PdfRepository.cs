using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using itgweb.Models;
using itgweb.ViewModel;


namespace itgweb.Repositories
{
    public class PdfRepository
    {
        private readonly itgwebEnt db = new itgwebEnt();
        public int UploadImageFileInDataBase(IEnumerable<HttpPostedFileBase> file, PdfViewModel PdfViewModel)
        {
            PdfViewModel.Img = ConvertToBytes(file.ElementAt(0));

            var PdfFile = file.ElementAt(1);
            var fileName = Path.GetFileName(PdfFile.FileName);
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfFiles/"), fileName);
            PdfFile.SaveAs(path);
            var pdf = new Pdf
            {
                Title = PdfViewModel.Title,
                Summary = PdfViewModel.Summary,
                Created = PdfViewModel.Created,
                Publish = PdfViewModel.Publish,
                Author = PdfViewModel.Author,
                ImageId = PdfViewModel.ImageId,
                NavbarId = PdfViewModel.NavbarId,
                Img = PdfViewModel.Img,
                FileName = PdfFile.FileName,
                IsExternal = PdfViewModel.IsExternal,
                ExLink = PdfViewModel.ExLink
            };
            db.Pdfs.Add(pdf);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}