using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using itgweb.Models;
using itgweb.ViewModel;

namespace itgweb.Repositories
{
    public class AuthorRepository
    {
        private readonly itgwebEnt db = new itgwebEnt();
        public int UploadImageInDataBase(HttpPostedFileBase file, AuthorViewModel AuthorViewModel)
        {
            AuthorViewModel.Img = ConvertToBytes(file);
            var author = new Author
            {
                FirstName = AuthorViewModel.FirstName,
                LastName = AuthorViewModel.LastName,
                Position = AuthorViewModel.Position,
                Summary = AuthorViewModel.Summary,
                Biography = AuthorViewModel.Biography,
                Created = AuthorViewModel.Created,
                ImageId = AuthorViewModel.ImageId,
                Email = AuthorViewModel.Email,
                Phone = AuthorViewModel.Phone,
                Img = AuthorViewModel.Img
            };
            db.Authors.Add(author);
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