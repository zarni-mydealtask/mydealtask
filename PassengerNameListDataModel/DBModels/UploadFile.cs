using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListDataModel.DBModels
{
    public class UploadFile
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }
}
