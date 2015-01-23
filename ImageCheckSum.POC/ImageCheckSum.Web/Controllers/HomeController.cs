using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using ImageCheckSum.Web.Models;

namespace ImageCheckSum.Web.Controllers
{
    public class HomeController : Controller
    {
        POCEntities entity = new POCEntities();
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase image)
        {            
            
            HashAlgorithm SHA512 = new SHA512Managed();
            var startTime = DateTime.Now.ToLocalTime();
            byte[] hash = SHA512.ComputeHash(image.InputStream);
            var endTime = DateTime.Now.ToLocalTime();
            var checksum = Convert.ToBase64String(hash.ToArray());
            if (!entity.UploadChecksums.Any(x => x.filechecksum == checksum))
            {
                UploadChecksum uploadChecksum = new UploadChecksum();
                uploadChecksum.filechecksum = checksum;
                entity.UploadChecksums.Add(uploadChecksum);
                entity.SaveChanges();
            }            
            return View();
        }
    }
}
