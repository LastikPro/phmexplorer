using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebServer
{
    public static class MessageManager
    {
        public static async Task<int> UnreadMessage(string Email)
        {
            int respons = 0;
           await Task.Run(()=> {
               Parallel.For(0, VirtualBD.Messages.Count, i => {

                   if (VirtualBD.Messages[i].Addressee == Email)
                       if (VirtualBD.Messages[i].Status == false)
                           respons++;
               });
             
            });
              
            return respons;
        }
    }
}
