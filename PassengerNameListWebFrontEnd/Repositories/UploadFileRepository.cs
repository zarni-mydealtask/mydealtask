using PassengerNameListWebFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PassengerNameListDataModel.DBModels;

namespace PassengerNameListWebFrontEnd.Repositories
{
    public class UploadFileRepository : Repository<UploadFile>,IUploadFileRepository
    {
        public UploadFileRepository(PassengerNameListDbContext dbContext) : base(dbContext)
        {
        }
        public PassengerNameListDbContext PassengerNameListDbContext { get { return Context as PassengerNameListDbContext; } }
    }
}