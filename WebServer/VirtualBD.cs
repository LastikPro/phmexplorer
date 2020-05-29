using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Log;
using WebServer.Models;
namespace WebServer
{
    public static class VirtualBD
    {
        public static bool isEx = false;
        public static List<User> Users = new List<User>();
        public static List<EventModel> Events = new List<EventModel>();
        public static List<ArticleModel> Articles = new List<ArticleModel>();
        public static List<CommentModel> Comments = new List<CommentModel>();
        public static List<MessageModel> Messages = new List<MessageModel>();
        public static void LoadInVirtualBD(ApplicationContext context)
        {            
                foreach (User user in context.Users.ToArray())
                    if (user != null && !Users.Exists(x => x.Email == user.Email))
                        Users.Add(user);

                foreach (EventModel Event in context.AllEvent.ToArray())
                    if (Event != null && !Events.Exists(x => x.ID == Event.ID))
                        Events.Add(Event);

                foreach (ArticleModel Article in context.Articles.ToArray())
                    if (Article != null && !Articles.Exists(x => x.ID == Article.ID))
                        Articles.Add(Article);

                foreach (CommentModel Comment in context.Comments.ToArray())
                    if (Comment != null && !Comments.Exists(x => x.ID == Comment.ID))
                        Comments.Add(Comment);

                foreach (MessageModel Message in context.Messages.ToArray())
                    if (Message != null && !Messages.Exists(x => x.ID == Message.ID))
                        Messages.Add(Message);            
        }
        public static async Task<IBase> GetFromVirtualDBAsync(string name)
        {
            User obj = null;
            await Task.Run(() =>
            {
                    foreach (var It in VirtualBD.Users)

                    if (It.Email == name){ obj = It; break;}                                                 
            });
            if (obj != null)
                return obj;

            return null;
        }
        public static async Task<IBase> GetFromVirtualDBAsync(int id, string type)
        {
            IBase obj = null;
            await Task.Run(() =>
            {
                if (type == "Events")
                    foreach (var It in VirtualBD.Events)
                        if (It.ID == id){ obj = It; break;}

                if (type == "Article")
                    foreach (var It in VirtualBD.Articles)
                        if (It.ID == id) { obj = It; break;}

                if (type == "Comments")
                    foreach (var It in VirtualBD.Comments)
                        if (It.articleID == id) { obj = It; break;}
            });

            if (obj != null)
                return obj;

            return null;
        }
        public static async void UpdateInVirtualDBAsync(User user)
        {
            int id = 0;
            await Task.Run(() =>
            {
                foreach (User _user in VirtualBD.Users)
                {
                    if (_user.ID == user.ID)
                    {
                        VirtualBD.Users[id] = user;
                        break;
                    }
                    id++;
                }
            });
        }       
        public static async void UpdateInVirtualDBAsync(EventModel Event)
        {
            int id = 0;
            await Task.Run(() =>
            {
                foreach (EventModel obj in VirtualBD.Events)
                {
                    if (obj.ID == Event.ID)
                    {
                        VirtualBD.Events[id] = Event;
                        break;
                    }
                    id++;
                }
            });
        }
        public static async void UpdateInVirtualDBAsync(ArticleModel Article)
        {
            int id = 0;
            await Task.Run(() =>
            {
                foreach (ArticleModel obj in VirtualBD.Articles)
                {
                    if (obj.ID == Article.ID)
                    {
                        VirtualBD.Articles[id] = Article;
                        break;
                    }
                    id++;
                }
            });
        }
        public static async void UpdateInVirtualDBAsync(CommentModel comment)
        {
            int id = 0;
            await Task.Run(() =>
            {
                foreach (CommentModel obj in VirtualBD.Comments)
                {
                    if (obj.articleID == comment.articleID)
                    {
                        VirtualBD.Comments[id] = comment;
                        break;
                    }
                    id++;
                }
            });
        }
        public static async void DeleteFromVirtualDBAsync(IBase obj)
        {        
            await Task.Run(()=> {
               switch(obj.GetType().Name)
                {
                    case "User":
                        Users.Remove((User)obj);
                        break;
                    case "EventModel":
                        Events.Remove((EventModel)obj);
                        break;
                    case "ArticleModel":
                        Articles.Remove((ArticleModel)obj);
                        break;
                    case "CommentModel":
                        Comments.Remove((CommentModel)obj);
                        break;
                    case "MessageModel":
                        Messages.Remove((MessageModel)obj);
                        break;
                }
            });
        }
    }
}
