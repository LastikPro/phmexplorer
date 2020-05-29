using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
namespace WebServer
{
    public class session
    {
        public DateTime lastaction;
        public string name;
    }
    public static class isOnlain
    {
       
        static Mutex mutex = new Mutex();
        public static List<session> OnlineUser = new List<session>();
        public static void AddUser(HttpContext context)
        {          
            if(VirtualBD.isEx)
            if (context.User.Identity.Name != null)
                if (!OnlineUser.Exists(x => x.name == context.User.Identity.Name))
                {                      
                        if (!VirtualBD.Users.Exists(x => x.Email == context.User.Identity.Name))
                            context.Response.Cookies.Delete(".AspNetCore.Cookies");
                        else
                        {
                            session user = new session();

                            user.name = context.User.Identity.Name;
                            user.lastaction = DateTime.UtcNow;
                            OnlineUser.Add(user);
                        }              
                }         
        }
        public static void resum(object name)
        {
            Parallel.For(0, OnlineUser.Count, (int x) => {
                for (int i = 0; i < x; i++)
                    if (OnlineUser[i].name == (string)name)
                        OnlineUser[i].lastaction = DateTime.UtcNow;
            });

        }
        public static void StatusControl()
        {
            DateTime date = DateTime.UtcNow;
            
            Parallel.For(0, OnlineUser.Count, (int x) => {
                for (int i = 0; i < OnlineUser.Count; i++)
                {
                    TimeSpan interval;
                    interval = date.Subtract(OnlineUser[i].lastaction);
                    if (interval.TotalSeconds > 180)
                    {
                        mutex.WaitOne();
                        OnlineUser.RemoveAt(i);
                        mutex.ReleaseMutex();
                    }                                        
                }
            });
        }
    }
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;
        public  UserStatusMiddleware(RequestDelegate next)
        {
            this._next = next;        
        }
        public async Task InvokeAsync(HttpContext context)
        {          
        isOnlain.AddUser(context);

            Thread thread1 = new Thread(isOnlain.StatusControl);
            Thread thread2 = new Thread( new ParameterizedThreadStart(isOnlain.resum));

            thread2.Start(context.User.Identity.Name);
            thread1.Start();
          
            await _next.Invoke(context);
        }
    }
}
