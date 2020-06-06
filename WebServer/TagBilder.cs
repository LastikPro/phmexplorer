using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebServer.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace WebServer
{
    public static class TagBuilders
    {      
        public static HtmlString BuildCommentList(int id, System.Security.Claims.ClaimsPrincipal claims)
        {

            string result = "", Image = "", Delete = "";

            foreach (var obj in VirtualBD.Comments)
            {

                if (obj.articleID == id)
                {
                    if (claims.IsInRole(Role.Admin) || claims.Identity.Name == obj.Creator)
                        Delete = $"<div class=\"col text-right\"> <button class=\"delete btn btn-danger\" id=\"{obj.ID}\" data=\"/Articles/deleteComment\">Удалить</button></div>";

                    if (obj.Img != null)
                        Image = $"<img src=\"{obj.Img}\" style=\"max-width:220px;\">";
                    else
                        Image = "";

                    result += $" <div class=\"row CommentTooArticle\" style=\"margin-top:25px; margin-bottom:10px; \"> <div class=\"col\">";
                    result += $" <div class=\"row border-bottom:1px solid black;\"> <div class=\"col-auto\"><img src = \"{@VirtualBD.Users.Find(x => x.Email == obj.Creator).Image}\" width=\"25\" height=\"25\"></div> <div class=\"col-auto\"> <a href=\"/Account/UserProfil?name={obj.Creator}\">{obj.Name} </a> </div> <div class=\"col-auto\">{obj.Time}</div></div>";
                    result += $"<div class=\"row\" style=\"margin-top: 15px;\"> <div class=\"col\"> {Image} <div>{obj.text}</div> </div> {Delete} </div>";
                    result += $"</div> </div> ";
                }
            }
            return new HtmlString(result);
        }
       
    }
}