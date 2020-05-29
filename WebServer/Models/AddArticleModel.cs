using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class AddArticleModel
    {
        public string header { get; set; }
        public IFormFile uploadedFile { get; set; }    
        public string Type { get; set; }
        public string Adress { get; set; }
        public string text { get; set; }
    }
}
