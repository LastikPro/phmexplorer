using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using WebServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WebServer.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        TemplateMetod templateMetod;
        private readonly ILogger _logger;
        public AccountController(ApplicationContext context,IWebHostEnvironment appEnvironment, TemplateMetod TempMetod, ILogger<AccountController> logger)
        {
            db = context;
            _appEnvironment = appEnvironment;
            templateMetod = TempMetod;
            _logger = logger;
        }
        //Authorization

        public IActionResult Authorization()
        {
            return View();
        }
        [HttpGet]
        public string Img(string data)
        {
            string SRC;

            if (VirtualBD.Users.Find(x => x.Email == data) != null)
                SRC = VirtualBD.Users.Find(x => x.Email == data).Image;
            else
                SRC = "/UserDefIcon.jpg";

            return SRC;
        }
        //Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Login(AuthorizationModel model)
        {
            User user = (User)await VirtualBD.GetFromVirtualDBAsync(model.Email);
            if (user != null)
            {
                if (user.Password == model.Password)
                {
                    await Authenticate(user);
                    return "OK";
                }
                    for (int i = 0; i < BlackList.list.Count; i++)
                {
                    if (BlackList.list[i].User == model.Email)
                        if (BlackList.list[i].Try < 5)
                            BlackList.list[i].Try++;
                        else
                        {
                            BlackList.list[i].IP = HttpContext.Connection.RemoteIpAddress.ToString();
                            BlackList.list[i].Time = DateTime.Now;
                        }                  
                } 
                if(!BlackList.list.Exists(x=>x.User== model.Email))
                BlackList.list.Add(new BlackListModel { Try = 1, User = model.Email });

                return "Не верный логин или пароль!";
            }
            else
                return "Пользователь не найден!";
        }

        //Register     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Register(AuthorizationModel model)
        {
            User user = (User)await VirtualBD.GetFromVirtualDBAsync(model.Email);

            if (user == null)
            {
                user = new User { Email = model?.Email??"Error", Password = model?.Password ?? "Error" };

                if (model.Email==null|| model.Password==null)
                    _logger.LogError("LogWarning {0}", DateTime.Now.ToString() + "==> Column Email or Password is empty!");

                user.Role = Role.User;
                user.RegisterData = DateTime.Now.ToString();
                user.Sex = 0;
                user.BD = "Неуказан";

                 user.Name = model?.Name; 

                if (model.uploadedFile != null)
                {
                    string patch = "/UserIcon/" + model.uploadedFile.FileName;
                    templateMetod.UploadFileAsync(patch, model.uploadedFile, _appEnvironment);
                    user.Image = patch;
                }
                else
                    user.Image = "/UserDefIcon.jpg";
                try
                {
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    VirtualBD.Users.Add(user);

                    await Authenticate(user);
                }
                catch (Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                }
                templateMetod.newEvent(db, $"Зареестрирован пользователь:{model.Email}", model.Email, $"{model.Name}", "");

                return "OK";
            }
            else
                return "Пользователь существует";
        }

        //UserProfil       
        public async Task<IActionResult> UserProfil(string name)
        {
            if (name != null)
            {
                User user = (User)await VirtualBD.GetFromVirtualDBAsync(name);
                if (user != null)
                {
                    if (name != User.Identity.Name)
                        user.Role = Role.Guest;
                    return View(user);
                }
                else
                    return RedirectToAction("Error", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UserProfil(UserProfilUpdate model)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = (User)await VirtualBD.GetFromVirtualDBAsync(User.Identity.Name);
              
                    user.Name = model?.Name;

                if (model.Login != null)
                {               

                    var obj = VirtualBD.Articles.FindAll(x => x.Creator == user.Email);
                    for (int i = 0; i < obj.Count; i++) { obj[i].Creator = model.Login; VirtualBD.UpdateInVirtualDBAsync(obj[i]); db.Articles.Update(obj[i]); }


                    var obj2 = VirtualBD.Comments.FindAll(x => x.Creator == user.Email);
                    for (int i = 0; i < obj2.Count; i++) { obj2[i].Creator = model.Login; VirtualBD.UpdateInVirtualDBAsync(obj2[i]); db.Comments.Update(obj2[i]); }

                    user.Email = model.Login;
                }

                    user.Sex = model.Sex;

                    user.BD = model?.BD;

                    user.Password = user?.Password;

                if (model.uploadedFile != null)
                {

                    string patch = "/UserIcon/" + model.uploadedFile.FileName;
                    await templateMetod.UploadFileAsync(patch, model.uploadedFile, _appEnvironment);
                    user.Image = patch;
                }
                try
                {
                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                    VirtualBD.UpdateInVirtualDBAsync(user);
                }
                catch (Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                }
                if (model.Login != null)
                    return RedirectToAction("Logout", "Account");
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        //Authenticate      
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            for (int i = 0; i < isOnlain.OnlineUser.Count; i++)
                if (isOnlain.OnlineUser[i].name == User.Identity.Name)
                    isOnlain.OnlineUser.RemoveAt(i);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<string> deleteUser(string name)
        {
            User obj = (User)await VirtualBD.GetFromVirtualDBAsync(name);
            if (obj != null)
                await Task.Run(() =>
                {
                    if (User.IsInRole(Role.Admin) || User.Identity.Name == obj.Name)
                    {
                        try
                        {
                            if (System.IO.File.Exists(_appEnvironment.WebRootPath + obj.Image))
                                System.IO.File.Delete(_appEnvironment.WebRootPath + obj.Image);
                            VirtualBD.Users.Remove(obj);
                            db.Users.Remove(obj);
                            db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                        }
                       
                    }
                });
            return $"{obj.Email} успешно удален!";
        }
    }
}
