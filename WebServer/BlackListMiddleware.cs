using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer
{
    public class BlackListMiddleware
    {
        private readonly RequestDelegate _next;
        public BlackListMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {    

            WebRequest request = WebRequest.Create("https://pereiaslav.city/read/rayon/42836/gajshin-iz-soloncyami-znovu-pominyalisya-miscyami-pislya-chergovogo-turu-chempionatu");
            request.Method = "DELETE";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            int a = 0;
            for (int i=0;i<BlackList.list.Count;i++)
                if (BlackList.list[i].IP == context.Connection.RemoteIpAddress.ToString())
                {
                    TimeSpan interval;
                    interval =DateTime.Now.Subtract(BlackList.list[i].Time);
                    if (interval.TotalMinutes > 30)
                        BlackList.list.RemoveAt(i);
                    else
                    await context.Response.WriteAsync("Error");
                }
                            
            await _next.Invoke(context);
        }
    }
}
