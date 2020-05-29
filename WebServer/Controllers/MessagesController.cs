using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class MessagesController : Controller
    {
        System.Threading.Mutex mutex = new System.Threading.Mutex();
        private ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        TemplateMetod templateMetod;
        private readonly ILogger _logger;
        public MessagesController(ApplicationContext context, IWebHostEnvironment appEnvironment, TemplateMetod tm, ILogger<MessagesController> loger)
        {
            db = context;
            _appEnvironment = appEnvironment;
            templateMetod = tm;
            _logger = loger;
        }
        public IActionResult Message(int actionstate, string Email)
        {
            (int, string) NewMessage = (2, Email);
            (int, string) Incoming = (1, Email);
            (int, string) Sended = (0, Email);
            switch (actionstate)
            {
                case 0: return View(Sended);
                case 1: return View(Incoming);
                case 2: return View(NewMessage);
            }
            return View();
        }

        public IActionResult MessageView(int id, int actionstate)
        {
            var obj = VirtualBD.Messages.Find(x => x.ID == id);
            if (obj != null)
            {
                MessageViewModel model = new MessageViewModel();

                model.Header = obj.Header;
                model.Name = obj.Name;
                model.Text = obj.Text;
                model.Image = obj.Image;
                model.Mark = obj.Mark;
                model.ID = obj.ID;
                model.action = actionstate;
                return View(model);
            }

            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public string Validrecipient(string Email)
        {
            if (VirtualBD.Users.Find(x => x.Email == Email) != null)
                return "ok";

            return " ";
        }
        [HttpPost]
        public void Delete(int id)
        {
            mutex.WaitOne();
            try
            {
                if (System.IO.File.Exists(_appEnvironment.WebRootPath + VirtualBD.Messages.Find(x => x.ID == id).Image))
                    System.IO.File.Delete(_appEnvironment.WebRootPath + VirtualBD.Messages.Find(x => x.ID == id).Image);

                db.Messages.Remove(VirtualBD.Messages.Find(x => x.ID == id));
                db.SaveChanges();

                VirtualBD.Messages.Remove(VirtualBD.Messages.Find(x => x.ID == id));
            }
            catch (Exception e)
            {
                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
            }
            mutex.ReleaseMutex();
         
        }
        [HttpPost]
        public void Mark(int id, int action)
        {
            if (action == 0)
            {
                VirtualBD.Messages.Find(x => x.ID == id).Mark = true;
                try
                {
                    db.Messages.Find(id).Mark = true;
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                }
            }

            if (action == 1)
            {
                VirtualBD.Messages.Find(x => x.ID == id).Mark = false;
                try
                {
                    db.Messages.Find(id).Mark = false;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                }
            }

        }
        [HttpPost]
        public void Viewed(int id, int action)
        {
            if (User.Identity.Name == VirtualBD.Messages.Find(x => x.ID == id).Addressee)
            {
                if (action == 0)
                {
                    VirtualBD.Messages.Find(x => x.ID == id).Status = true;
                    try
                    {
                        db.Messages.Find(id).Status = true;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                    }
                }

                if (action == 1)
                {
                    VirtualBD.Messages.Find(x => x.ID == id).Status = false;
                    try
                    {
                        db.Messages.Find(id).Status = false;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                    }
                }
            }
        }
        [HttpPost]
        public int Send(MessageSendModel model)
        {
            MessageModel message = new MessageModel();

            if (User.Identity.IsAuthenticated)
            {
                if (model.Addressee == null)
                    return 0;
                else
                    message.Addressee = model.Addressee;

                message.Name = User.Identity.Name;
            }
            else
            {
                message.Addressee = "Admin@Admin";

                if (model.Name != null)
                    message.Name = model.Name;
                else
                    return 0;
            }
           
            if (model.uploadedFile != null)
            {
                templateMetod.UploadFileAsync("/Img/MessagesImg/" + model.uploadedFile.FileName, model.uploadedFile, _appEnvironment);
                message.Image = "/Img/MessagesImg/" + model.uploadedFile.FileName;
            }
            else message.Image = null;

            if (model.Header != null)
                message.Header = model.Header;
            else
                message.Header = "Без темы";

            message.Text = model.Text;
            message.Mark = false;        

            message.Time = DateTime.Now.ToString();
            message.Status = false;

            VirtualBD.Messages.Add(message);
            try
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
            }

            return 1;
        }
    }
}
