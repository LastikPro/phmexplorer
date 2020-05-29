using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class ArticleModel : IBase
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int ID { get; set; }
        public int Viev { get; set; }
        public string Text { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public string Creator { get; set; }
        public string ImageMin { get; set; }
    }
}
