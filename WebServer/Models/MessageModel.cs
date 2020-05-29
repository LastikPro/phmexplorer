using Microsoft.AspNetCore.Http;

namespace WebServer.Models
{
    public class MessageModel:IBase
    {
       public int ID { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public string Addressee { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
        public bool Mark { get; set; }
        public string Image { get; set; }
    }
    public class MessageSendModel
    {
        public string Header { get; set; }
        public string Addressee { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public IFormFile uploadedFile { get; set; }
    }
}
