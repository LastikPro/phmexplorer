using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class BlackListModel
    {
        public DateTime Time { get; set; }
        public int Try{ get; set; }
        public  string IP { get; set; }

        public string User { get; set; }
    }
    public static class BlackList
    {
        public static List<BlackListModel> list = new List<BlackListModel>();
       
    }
}
