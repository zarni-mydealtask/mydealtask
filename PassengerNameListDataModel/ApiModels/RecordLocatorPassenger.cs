using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListDataModel.ApiModels
{
    public class RecordLocatorPassenger
    {
        public int FileId { get; set; }
        public string RecordLocator { get; set; }

        public IEnumerable<string> Passengers { get; set; }
    }
}
