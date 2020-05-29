using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebServer.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServer.Controllers
{
    public class Articles : Controller
    {
        private ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        TemplateMetod templateMetod;
        private readonly ILogger _logger;
        public Articles(ApplicationContext context, IWebHostEnvironment appEnvironment, TemplateMetod tm, ILogger<Articles> loger)
        {
            db = context;
            _appEnvironment = appEnvironment;
            templateMetod = tm;
            _logger = loger;
        }
        [HttpGet]
        public async Task<IActionResult> Article(int id)
        {
            ArticleModel model = (ArticleModel)await VirtualBD.GetFromVirtualDBAsync(id, "Article");

            if (model == null) return RedirectToAction("Error", "Home");

            model.Viev++;
            try
            {
                db.Articles.Update(model);
                await db.SaveChangesAsync();

                VirtualBD.UpdateInVirtualDBAsync(model);
            }

            catch (Exception e)
            {
                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> AddArticle(AddArticleModel model)
        {
            
                var user = (User)await VirtualBD.GetFromVirtualDBAsync(User.Identity.Name);
            try
            {
                ArticleModel article = new ArticleModel();
            article.Header = model.header;
            article.Name = user.Name;
            article.Creator = user.Email;
            article.Viev = 0;
            article.Type = model.Type;
            article.Time = DateTime.Now.ToString();
            article.Text += $"<div class=\"row\"> <div class=\"col\">{templateMetod.Translete(model.Type)}</div> </div>";
            article.Text += $"<div class=\"row\"> <div class=\"col\">{model.Adress} </div> </div>";
            article.Text += model.text;

            if (model.uploadedFile != null)
            {
                if (await templateMetod.UploadFileAsync("/Img/ArticlesImg/" + model.uploadedFile.FileName, model.uploadedFile, _appEnvironment))
                {
                    article.Image = "/Img/ArticlesImg/" + model.uploadedFile.FileName;
                    templateMetod.CreateMinVersion(article.Image, "/Img/ArticleMinImg/" + model.uploadedFile.FileName, _appEnvironment);
                    article.ImageMin = "/Img/ArticleMinImg/" + model.uploadedFile.FileName;
                }
            }

            
                db.Articles.Add(article);
                await db.SaveChangesAsync();

                VirtualBD.Articles.Add(article);
            }
            catch (Exception e)
            {
                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
            }

            templateMetod.newEvent(db, "Добавленна статья", user.Email, user.Name, $"добавил статью:{model.header}");

            return "/Home/Index";
        }
        [HttpPost]
        public async Task<int> AddComment(CommentLoadModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = VirtualBD.Users.Find(x => x.Email == User.Identity.Name);
                CommentModel comment = new CommentModel();

                comment.articleID = model.articleID;
                comment.Name = user.Name;
                comment.Time = DateTime.Now.ToString();
                comment.text = model.text;
                comment.Creator = user.Email;

                if (model.uploadedFile != null)
                {
                    await templateMetod.UploadFileAsync("/Img/UserImg/" + model.uploadedFile.FileName, model.uploadedFile, _appEnvironment);
                    comment.Img = "/Img/UserImg/" + model.uploadedFile.FileName;
                }
                else comment.Img = null;

                try
                {
                    db.Comments.Add(comment);
                    await db.SaveChangesAsync();

                    VirtualBD.Comments.Add(comment);
                }
                catch (Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                }

                templateMetod.newEvent(db, $"Коментарий к <a href=\"/Articles/Article?id={model.articleID}\">статье</a>", User.Identity.Name, user.Name, model.text);
            }
            return 0;
        }
        [HttpGet]
        public string UpdateComments(int id) { return TagBuilders.BuildCommentList(id, User).Value; }
        [HttpPost]
        public async Task<string> deleteComment(int id)
        {
            await Task.Run(() =>
            {
                var obj = VirtualBD.Comments.Find(x => x.ID == id);
                if (obj != null)
                {
                    if (User.IsInRole(Role.Admin) || User.Identity.Name == obj.Creator)
                    {
                        try
                        {
                            if (System.IO.File.Exists(_appEnvironment.WebRootPath + obj.Img))
                                System.IO.File.Delete(_appEnvironment.WebRootPath + obj.Img);
                            VirtualBD.Comments.Remove(obj);
                            db.Comments.Remove(obj);
                            db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                        }
                    }
                }

            });
            return "OK";
        }
        [HttpPost]
        public async Task<IActionResult> deleteArticle(int id)
        {
            await Task.Run(() =>
            {
                var obj = VirtualBD.Articles.Find(x => x.ID == id);

                if (obj != null)
                {
                    if (User.IsInRole(Role.Admin) || User.Identity.Name == obj.Creator)
                    {
                        var it = VirtualBD.Comments.FindAll(x => x.articleID == id);
                        try
                        {
                            if (System.IO.File.Exists(_appEnvironment.WebRootPath + obj.Image))
                                System.IO.File.Delete(_appEnvironment.WebRootPath + obj.Image);

                            if (System.IO.File.Exists(_appEnvironment.WebRootPath + obj.ImageMin))
                                System.IO.File.Delete(_appEnvironment.WebRootPath + obj.ImageMin);

                            VirtualBD.Articles.Remove(obj);
                            db.Articles.Remove(obj);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                        }

                        for (int i = 0; i < it.Count; i++)
                        {
                            try
                            {
                                if (System.IO.File.Exists(_appEnvironment.WebRootPath + it[i].Img))
                                    System.IO.File.Delete(_appEnvironment.WebRootPath + it[i].Img);

                                VirtualBD.Comments.Remove(it[i]);
                                db.Comments.Remove(it[i]);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                            }
                        }
                        try { db.SaveChangesAsync(); } catch (Exception e) { _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message); }
                    }
                }
            });
            return Redirect("/Home/Index");
        }
    }
}
