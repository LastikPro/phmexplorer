using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class EventModel:IBase
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int ID { get; set; }
        public string EventType { get; set; }
        public string UserName { get; set; }
        public string messege { get; set; }
    }
}
