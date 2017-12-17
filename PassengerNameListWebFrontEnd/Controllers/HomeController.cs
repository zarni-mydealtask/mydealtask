using Newtonsoft.Json;
using PassengerNameListDataModel.ApiModels;
using PassengerNameListDataModel.DBModels;
using PassengerNameListWebFrontEnd.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PassengerNameListWebFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private const string mydealKey = "mydeal-key";
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ManageFile()
        {
            return View();
        }

        public ActionResult AddRecord()
        {
            return View();
        }
        public static string GetPassengerDataLine(string passenger, string recordLocator)
        {
            return "1" + passenger + " .L/" + recordLocator;
        }

        public ActionResult SaveRecord(RecordLocatorPassenger record)
        {
            var recordId = 0;
            using (var uow = new UnitOfWork(new Models.PassengerNameListDbContext()))
            {
                var fileRecord = uow.UploadFileRepo.Get(record.FileId);

                var recordDb = new RecordLocator();
                recordDb.UploadFileId = record.FileId;
                recordDb.Code = record.RecordLocator;
                uow.RecordLocatorRepo.Add(recordDb);
                uow.Complete();
                foreach (var item in record.Passengers)
                {
                    var passenger = new Passenger() ;
                    passenger.Name = item;
                    passenger.RecordLocatorId = recordDb.Id;
                    uow.PassengerRepo.Add(passenger);

                    fileRecord.FileContent += Environment.NewLine + "1" + record.RecordLocator;
                }                
                uow.Complete();
                recordId = recordDb.Id;
            }

            var jsonResult = new JsonResult();
            var addResult = new AddRecordResult { RecordLocatorId = recordId };
            jsonResult.Data = addResult;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public async Task<ActionResult> UploadFile()
        {
            var file = Request.Files[0];
            var apiUrl = ConfigurationManager.AppSettings["PassengerNameListApi.Url"];
            
            var fileContent = string.Empty;
            using (var stream = new StreamReader(file.InputStream))
            {
                fileContent = stream.ReadToEnd();
            }

            PassengerNameListResponse data;
            using (var client = new HttpClient())
            {
                
                using (var content =new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
                    content.Add(new StreamContent(stream),"passengerFile", file.FileName);
                    content.Headers.Add("APIKey", mydealKey);
                    using ( var message =   await client.PostAsync(apiUrl + "/passengernamelist/UploadNameList", content))
                    {
                        var response = await message.Content.ReadAsStringAsync();
                        data = JsonConvert.DeserializeObject<PassengerNameListResponse>(response);
                    }
                }
           }
           
            var uploadFileId = 0;
            if (data != null)
            {
                using (var uow = new UnitOfWork(new Models.PassengerNameListDbContext()))
                {
                    var fileInput = new UploadFile() { FileContent = fileContent, FileName = file.FileName };
                    uow.UploadFileRepo.Add(fileInput);
                    uow.Complete();
                    uploadFileId = fileInput.Id;
                    foreach (var item in data.Records)
                    {                   
                        var record = new RecordLocator();
                        record.Code = item.RecordLocator;
                        record.UploadFileId = fileInput.Id;
                        uow.RecordLocatorRepo.Add(record);
                        uow.Complete();
                        foreach (var passenger in item.Passengers)
                        {
                            var passengerModel = new Passenger();
                            passengerModel.Name = passenger;
                            passengerModel.RecordLocatorId = record.Id;
                            uow.PassengerRepo.Add(passengerModel);
                        }
                        uow.Complete();
                    }
                }                
            }
            var jsonResult = new JsonResult();
            var uploadResult = new UploadFileResult { FileId = uploadFileId,  InputLineResults = data.InputLineResults, UploadFileId = uploadFileId, UploadFileName = file.FileName };
            jsonResult.Data = uploadResult;
            return jsonResult;
        }
        public ActionResult DisplayRecord()
        {
            return View();
        }
        public FileResult DownloadFile(int fileId)
        {
            using (var uow = new UnitOfWork(new Models.PassengerNameListDbContext()))
            {
                var fileRecord = uow.UploadFileRepo.Get(fileId);
                var bytes = Encoding.ASCII.GetBytes(fileRecord.FileContent);
                var f = File(bytes, System.Net.Mime.MediaTypeNames.Text.Plain,fileRecord.FileName);

                return f;
            }
        }
        public ActionResult SearchRecords(int fileId, string keyword)
        {
            List<RecordLocatorPassenger> resultRecords = ShowAllRecords(fileId);
            var filter = (from x in resultRecords
                          where x.RecordLocator.ToLower().Contains(keyword.ToLower())
                           || x.Passengers.Any(y=> y.ToLower().Contains(keyword.ToLower()))
                          select x
                         );

            var jsonResult = new JsonResult();
            jsonResult.Data = filter.ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult GetRecords(int fileId)
        {
            List<RecordLocatorPassenger> resultRecords = ShowAllRecords(fileId);
            var jsonResult = new JsonResult();
            jsonResult.Data = resultRecords;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        private static List<RecordLocatorPassenger> ShowAllRecords(int fileId)
        {
            var resultRecords = new List<RecordLocatorPassenger>();
            using (var uow = new UnitOfWork(new Models.PassengerNameListDbContext()))
            {
                var records = uow.RecordLocatorRepo.Find(x => x.UploadFileId == fileId).ToList();
                if (records.Any())
                {
                    var recordIds = records.Select(x => x.Id).ToList();
                    var passengers = uow.PassengerRepo.Find(x => recordIds.Contains(x.RecordLocatorId)).ToList();
                    foreach (var item in records)
                    {
                        var resultRecord = new RecordLocatorPassenger();
                        resultRecord.RecordLocator = item.Code;
                        resultRecord.Passengers = passengers.Where(x => x.RecordLocatorId == item.Id).Select(x => x.Name);
                        resultRecords.Add(resultRecord);
                    }
                }

            }

            return resultRecords;
        }
    }
}