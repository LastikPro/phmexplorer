using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebServer.Controllers
{
    
    public class HomeController : Controller
    {
     
        public  HomeController(ApplicationContext context,ILogger<MessagesController> logger)
        {
            if(VirtualBD.isEx==false)
            {
                VirtualBD.LoadInVirtualBD(context);
                VirtualBD.isEx = true;
                logger.Log(LogLevel.Information, $"Server succsessful started at =>{DateTime.Now.ToString()}");
            }
          
        }     
        public  IActionResult Index()
        {      
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
     
        public IActionResult about()
        {
                return View();
        }

        public IActionResult UsersList()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Authorization", "Account");
        }
        public IActionResult MyContact()
        {
            return View();
        }
    }
}
