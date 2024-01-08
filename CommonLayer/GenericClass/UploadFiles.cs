using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.GenericClass
{
    public class UploadFiles : IUploadFiles
    {
        private readonly IHostingEnvironment _environment;
        public UploadFiles(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public string UploadFileFunc(IEnumerable<IFormFile> files, string uploadPath)
        {
            var upload = Path.Combine(_environment.WebRootPath, uploadPath);
            var fileName = "";

            foreach (var item in files)
            {
                fileName=Guid.NewGuid().ToString().Replace("-","")+Path.GetExtension(item.FileName);
                using(var fs = new FileStream(Path.Combine(upload,fileName),FileMode.Create))
                {
                    item.CopyTo(fs);    
                }
            }
            return fileName;    
        }
    }
}
