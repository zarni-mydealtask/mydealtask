using PassengerNameListWebFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PassengerNameListDataModel.DBModels;

namespace PassengerNameListWebFrontEnd.Repositories
{
    public class RecordLocatorRepository : Repository<RecordLocator>,IRecordLocatorRepository
    {
        public RecordLocatorRepository(PassengerNameListDbContext dbContext) : base(dbContext)
        {
        }
        public PassengerNameListDbContext PassengerNameListDbContext { get { return Context as PassengerNameListDbContext; } }
    }
}