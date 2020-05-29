using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public interface IBase
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
