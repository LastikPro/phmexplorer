using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class UserProfilUpdate
    {
        public IFormFile uploadedFile { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
        public string BD { get; set; }
    }
}
