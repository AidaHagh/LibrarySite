using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.GenericClass
{
    public interface IUploadFiles 
    {
        string UploadFileFunc(IEnumerable<IFormFile> files, string uploadPath);
    }
}
