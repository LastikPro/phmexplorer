using System;
using System.Threading.Tasks;
using WebServer.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.Extensions.Logging;
using WebServer.Log;
using System.Drawing.Imaging;

namespace WebServer
{
    public  class TemplateMetod
    {
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private readonly ILogger _logger = LoggerSubsystem.CreateLogger<TemplateMetod>();
        public async Task<bool> UploadFileAsync(string path, IFormFile uploadedFile, IWebHostEnvironment _appEnvironment)
        {           
            if (uploadedFile != null)
            {
                try
                {
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }  
                catch(Exception e)
                {
                    _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
                    return false;
                }
               
                return true;
            }
            return false;
        }
        public void newEvent (ApplicationContext context,string EventType,string Email,string Name ,string messege )
        {
            EventModel Event = new EventModel();
            try
            {
                Event.EventType = EventType;
                Event.Name = Email;
                Event.Time = DateTime.Now.ToString();
                if (messege == null) messege = "Изображение";
                if (messege.Length > 70)
                    Event.messege = messege.Substring(0, 70) + "...";
                else
                    Event.messege = messege;

                Event.UserName = Name;

                context.AllEvent.Add(Event);
                Parallel.Invoke(() => { context.SaveChanges(); }, () => { VirtualBD.Events.Add(Event); });
            }
            catch(Exception e)
            {
                _logger.LogError("LogError {0}", DateTime.Now.ToString() + "==>" + e.Message);
            }
               
                                                       
        }
        public bool CreateMinVersion(string InPatch,string OutPatch,IWebHostEnvironment _appEnvironment)
        {
            try
            {
                Bitmap b1 = new Bitmap(_appEnvironment.WebRootPath + InPatch);
            Bitmap b2 = new Bitmap(b1, new Size(440, 240));
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
           var myEncoder = Encoder.Quality;
           var myEncoderParameters = new EncoderParameters(1);
           var myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;
           
            
                b2.Save(_appEnvironment.WebRootPath + OutPatch, myImageCodecInfo, myEncoderParameters);
            }
            catch(Exception e) {
                _logger.LogError("LogError {0}MinVers", DateTime.Now.ToString() + "==>" + e.Message);
            }
           
            return false;
        }      
        public string Translete(string item)
        {
            switch (item)
            {
                case "Market": item = "Продуктовые магазины"; break;
                case "Shop": item = "Магазин"; break;
                case "Restaurants": item = "Рестораны"; break;
                case "Cafe": item = "Кафе"; break;
                case "EducationalInstitutions": item = "Образовательные учреждения"; break;
                case "ConstructionShops": item = "Строительные магазины"; break;
                case "Museums": item = "Музеи"; break;
                case "SityBilding": item = "Городская инфраструктура";break;
            }
            return item;
        }
    }
}