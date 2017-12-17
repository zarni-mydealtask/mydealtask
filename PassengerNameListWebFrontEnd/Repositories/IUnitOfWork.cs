using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListWebFrontEnd.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IRecordLocatorRepository RecordLocatorRepo { get; }
        int Complete();
    }
}
