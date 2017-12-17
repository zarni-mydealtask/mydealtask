using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListWebApi.BusinessLogic
{
    public interface IRecordParser
    {
        bool ParseRecord(string record, out string passengerName, out string recordLocator);
    }
}
