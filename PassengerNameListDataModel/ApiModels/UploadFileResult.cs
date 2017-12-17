using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListDataModel.ApiModels
{
    public class UploadFileResult
    {
        public IEnumerable<InputLineResult> InputLineResults { get; set; }
        public int UploadFileId { get; set; }
        public string UploadFileName { get; set; }
        public int FileId { get; set; }
    }
}
