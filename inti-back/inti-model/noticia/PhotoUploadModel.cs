using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.noticia
{
    public class PhotoUploadModel
    {
        public IFormFile? FOTO { get; set; }
    }
}
