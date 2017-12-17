using PassengerNameListDataModel.ApiModels;
using PassengerNameListWebApi.BusinessLogic;
using PassengerNameListWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PassengerNameListWebApi.Controllers
{
    public class PassengerNameListController : ApiController
    {
        private IRecordParser _recordParser;

        public PassengerNameListController()
        {
            _recordParser = new RecordParser();
        }
        [Route("PassengerNameList/UploadNameList")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadNameList()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var rawData = "";
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                rawData = await file.ReadAsStringAsync();           
            }
            var lines = rawData.Split(Environment.NewLine.ToCharArray());

            var tempRecords = new RecordLocatorPassenger();
            var nameList = new List<RecordLocatorPassenger>();
            var inputLineResults = new List<InputLineResult>();
            foreach (var line in lines)
            {
                
                var found = _recordParser.ParseRecord(line, out string passengerName, out string recordLocator);
                var lineResult = new InputLineResult() { LineContent = line, Processed = found };

                if (found)
                {
                    var passengers = new List<string> {  passengerName  };
                    var recordToUpdate = nameList.FirstOrDefault(x => x.RecordLocator == recordLocator);
                    if (recordToUpdate == null)
                    {
                        recordToUpdate = new RecordLocatorPassenger();
                        recordToUpdate.RecordLocator =  recordLocator ;
                        recordToUpdate.Passengers = Enumerable.Empty<string>();
                        nameList.Add(recordToUpdate);
                    }
                                       
                    recordToUpdate.Passengers = recordToUpdate.Passengers.Concat(passengers);
                }
                inputLineResults.Add(lineResult);
            };
            var result = new PassengerNameListResponse() {  Records = nameList, InputLineResults = inputLineResults };
            return Ok(result);
        }
    }
}
