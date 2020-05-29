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
        public static async Task<HtmlString> BuildUsersList(System.Security.Claims.ClaimsPrincipal claims)
        {
            string result = "",Delete="";
            (string status, string statusClass) statusparam;
            await Task.Run(()=> {
                for (int i = 0; i < VirtualBD.Users.Count; i++)
                {
                    if (claims.IsInRole(Role.Admin))
                        Delete = $"<div > <text class=\"del\" style=\"color:blue;\"  id=\"{VirtualBD.Users[i].Email}\" href=\"/Account/deleteUser\">Удалить</text></div>";

                        statusparam=  setStatus( VirtualBD.Users[i].Email);

                    result += $"<tr class=\"tableitem {statusparam.statusClass} \">";
                    result += $"<th scope=\"row\">{i + 1}</th>";
                    result += $"<th><img src=\"{VirtualBD.Users[i].Image}\" id=\"UserImg\" \"></th> ";
                    result += $"<td> <a href=\"/Account/UserProfil?name={VirtualBD.Users[i].Email}\">{VirtualBD.Users[i].Name}</a> {Delete} </td>";
                    result += $"<td> <div class=\"row\">{VirtualBD.Users[i].Email} </div> <div class=\"row\"> <a class =\"fa fa-envelope-o\"href=\"/Messages/Message?actionstate=2&Email={VirtualBD.Users[i].Email}\"> Написать</a> </div> </td>";
                    result += $"<td> <img style=\"transform:translateX(-15px); \" src=\"{statusparam.status}\"> </td>";
                    result += "</tr>";
                }
            });
           
            return new HtmlString(result);
        }
        public static async Task<HtmlString> BuildEvent()
        {
            string result = "";
            int count=0;
            await Task.Run(()=>{
               for (int i = VirtualBD.Events.Count; i > 0; i--)
                {
                    result += "<tr class=\"eventEllement\" > <th> <div style=\"margin-top:15px;\" class=\"row border-bottom\">";
                    result += " <div class=\"col text-center\">";
                    result += $"<div style=\"font - size: 20px; \"> <b> {VirtualBD.Events[i - 1].EventType}  </b>  </div>";
                    result += $"<div style=\"transform: translate(-20px,1px); \"> {VirtualBD.Events[i - 1].Time}</div>";
                    result += "</div> </div>";
                    result += $"<div class=\"row\"> <div class=\"col\"><a href=\"/Account/UserProfil?name={VirtualBD.Events[i - 1].Name}\">{VirtualBD.Events[i - 1].UserName}:</a> {VirtualBD.Events[i - 1].messege}</div> </div>";
                    result += "</th> </tr>";
                    if (count == 5)
                        break;
                    count++;
                }
            });
           
            return new HtmlString(result);
        }
        public static async Task<HtmlString> BuildArticlesList(System.Security.Claims.ClaimsPrincipal claims)
        {
            string result = "",Delete="";
            await Task.Run(() =>
            {
                foreach (var obj in VirtualBD.Articles)
                {                   
                    if (claims.IsInRole(Role.Admin) || claims.Identity.Name == obj.Creator)
                        Delete = $"<div class=\"col text-right\"> <text class=\"del\"style=\"color:blue;\"  id=\"{obj.ID}\" href=\"/Articles/deleteArticle\">Удалить</text></div>";
                     
                    result += $"<tr class=\"tableitem \" id=\"{obj.ID}\"> ";
                    result += $"<th class=\"{obj.Type}\"> <a href=\"/Articles/Article?id={obj.ID}\"> <img src=\"{obj.ImageMin}\" width=\"220\" height=\"120\" /> </a> </th>";
                    result += $"<th> <div class=\"row\"> <a href=\"/Articles/Article?id={obj.ID}\"> <b style=\"font-size:18px; \">{obj.Header} </b> </a> </div>";
                    result += $"<div class=\"row\" style=\"margin-top:15px\">  <a style=\"color:black;\" href=\"/Account/UserProfil?name={obj.Creator}\"> <div class=\"col-auto\" style=\"background-color: slateblue;\"> {obj.Name} </div> </a> <div class=\"col-auto\" id=\"Time\" style=\"background-color: green;\">{obj.Time} </div> <div class=\"col-2 \" style=\"background-color: red;\"> <txt class=\"fa fa-eye\">{obj.Viev}</txt> </div> <div class=\"col-2 \" style=\"background-color: red;\"> <txt class=\"fa fa-comments\">{VirtualBD.Comments.FindAll(x => x.articleID == obj.ID).Count}</txt> </div> {Delete}</div> ";
                    result += $"<div class=\"row \" style=\"margin-top:10px\">#{new TemplateMetod().Translete(obj.Type)}</div>";
                    result += $" </th> </tr>";
                }
            });
           
            return new HtmlString(result);
        }
        public static HtmlString BuildMessages(System.Security.Claims.ClaimsPrincipal claims,string type)
        {
            string result = "",status="",statusTeg="",mark="star";
           
                foreach (var obj in VirtualBD.Messages)
                {
                    if(type=="Send")
                    if (obj.Addressee == claims.Identity.Name)
                    {                      
                        if (obj.Status == false)
                        {
                            status = "Не прочитано";
                            statusTeg = " ";
                        }                           
                        else 
                        {
                            status = "Прочитано";
                            statusTeg = "active";
                        }

                        if (obj.Mark == true)
                            mark = "fa fa-star";
                        else
                            mark = "star";

                        result += $" <tr class=\"list1 {statusTeg}\" id=\"{obj.ID}\">" +
                                                       $" <th>" +
                                                           " <div class=\"row\">" +
                                                                "<div class=\"col-9\">" +
                                                                  $"<a href=\"/Messages/MessageView?id={obj.ID}\" style=\"color:#fff;\">{status}</a>" +
                                                               " </div>" +
                                                               " <div class=\"col\">" +
                                                                  $"<div class=\"{mark}\" style=\"color: gold; \"> </div>" +
                                                               "</div> </div> </th>" +
                                                      $"<th><a href=\"/Messages/MessageView?id={obj.ID}\" style=\"color:#fff;\">{obj.Time}</a></th>" +
                                                        $"<th><a href=\"/Messages/MessageView?id={obj.ID}\" style=\"color:#fff;\">{obj.Name}</a></th>" +
                                                       $"<th><a href=\"/Messages/MessageView?id={obj.ID}\" style=\"color:#fff;\">{obj.Header}</a></th> " +
                                                      "<th><input type = \"checkbox\" /> </th> </tr> ";
                    }
                    if(type=="Resive")
                    if (obj.Name == claims.Identity.Name)
                    {
                        if (obj.Status == false)
                        {
                            status = "Не прочитано";
                            statusTeg = " ";
                        }
                        else
                        {
                            status = "Прочитано";
                            statusTeg = "active";
                        }

                        if (obj.Mark == true)
                            mark = "fa fa-star";
                        else
                            mark = "star";
                        result += $" <tr class=\"list2 {statusTeg}\" id=\"{obj.ID}\">" +
                                                       " <th>" +
                                                           " <div class=\"row\">" +
                                                                "<div class=\"col-9\">" +
                                                                  $"<a href=\"/Messages/MessageView?id={obj.ID}&actionstate=1\" style=\"color:black;\">{status}</a>" +
                                                               " </div>" +
                                                               " <div class=\"col\">" +
                                                                  $"<div class=\"{mark}\" style=\"color: gold; \"> </div>" +
                                                               "</div> </div> </th>" +
                                                      $"<th><a href=\"/Messages/MessageView?id={obj.ID}&actionstate=1\" style=\"color:black;\">{obj.Time}</a></th>" +
                                                        $"<th><a href=\"/Messages/MessageView?id={obj.ID}&actionstate=1\" style=\"color:black;\">{obj.Addressee}</a></th>" +
                                                       $"<th><a href=\"/Messages/MessageView?id={obj.ID}&actionstate=1\" style=\"color:black;\">{obj.Header}</a></th>" +
                                                      "<th><input type = \"checkbox\" /> </th> </tr> ";
                    }
                }                         
            return new HtmlString(result);
        }
        public static  HtmlString  BuildCommentList(int id,System.Security.Claims.ClaimsPrincipal claims)
        {
           
            string result = "", Image = "",Delete="";

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
        private static (string, string) setStatus( string name)
        {
            (string status, string statusClass) statusparam=(status:"/Img/ofline.png", statusClass: "ofline");       
            foreach (session user in isOnlain.OnlineUser)
            {
                if (user.name == name)
                {
                    statusparam.status = "/Img/online.png";
                    statusparam.statusClass = "online";
                    break;
                }
            }
            return statusparam;
        }      
    }
}