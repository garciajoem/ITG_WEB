using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using itgweb.Models;
using itgweb.ViewModel;

namespace itgweb.Repositories
{
    public class PageRepository
    {
        private readonly itgwebEnt db = new itgwebEnt();
        public int UploadImageInDataBase(HttpPostedFileBase file, PageViewModel PageViewModel)
        {
            PageViewModel.Img = ConvertToBytes(file);
            
            var page = new Page
            {
                Title = PageViewModel.Title,
                Summary = PageViewModel.Summary,
                ShowSum = PageViewModel.ShowSum,
                Maintext = PageViewModel.Maintext,
                Created = PageViewModel.Created,
                MetaDescription = PageViewModel.MetaDescription,
                MetaKeywords = PageViewModel.MetaKeywords,
                Publish = PageViewModel.Publish,
                AuthorId = PageViewModel.AuthorId,
                ImageId = PageViewModel.ImageId,
                NavbarId = PageViewModel.NavbarId,
                SidenavId = PageViewModel.SidenavId,
                Img = PageViewModel.Img,
                SubContent = PageViewModel.SubContent,
                PageUrl = PageViewModel.PageUrl,
                menuitems = PageViewModel.menuitems
            };
            db.Pages.Add(page);
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