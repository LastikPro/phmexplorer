using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class CommentModel : IBase
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public int articleID { get; set; }
        public string text { get; set; }
        public string Img { get; set; }
        public string Creator { get; set; }
    }
}
