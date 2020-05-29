using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class CommentLoadModel
    {
        public int articleID { get; set; }
        public string text { get; set; }
        public IFormFile uploadedFile {get;set;}
    }
}
