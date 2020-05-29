using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class MessageViewModel
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public int ID { get; set; }
        public int action { get; set; }
        public bool Mark { get; set; }
        public string Image { get; set; }

    }
}
