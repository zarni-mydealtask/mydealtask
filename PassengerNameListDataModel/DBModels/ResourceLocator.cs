using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListDataModel.DBModels
{
    public class RecordLocator
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        public int UploadFileId { get; set; }
    }
}
