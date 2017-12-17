using PassengerNameListWebFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerNameListWebFrontEnd.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PassengerNameListDbContext _context;
        public IRecordLocatorRepository RecordLocatorRepo { get; private set; }
        public IPassengerRepository PassengerRepo { get; private set; }
        public IUploadFileRepository UploadFileRepo { get; private set; }

        public UnitOfWork(PassengerNameListDbContext context)
        {
            _context = context;
            RecordLocatorRepo = new RecordLocatorRepository(_context);
            PassengerRepo = new PassengerRepository(_context);
            UploadFileRepo = new UploadFileRepository(_context);
        }
        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
