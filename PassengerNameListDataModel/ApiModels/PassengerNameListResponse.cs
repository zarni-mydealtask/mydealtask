using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListDataModel.ApiModels
{
    public class PassengerNameListResponse
    {
        public int FileId { get; set; }
        public IEnumerable<RecordLocatorPassenger> Records { get; set; }
        public IEnumerable<InputLineResult> InputLineResults { get; set; }
    }
}
