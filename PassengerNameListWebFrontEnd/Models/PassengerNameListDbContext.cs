using PassengerNameListDataModel.ApiModels;
using PassengerNameListDataModel.DBModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PassengerNameListWebFrontEnd.Models
{
    public class PassengerNameListDbContext : DbContext
    {
        public DbSet<RecordLocator> RecordLocators { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }

        public PassengerNameListDbContext() : base("DefaultConnection")
        {
           // Database.SetInitializer<PassengerNameListDbContext>(null);
        }
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("DefaultConnection");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
           
            base.OnModelCreating(modelBuilder);
        }
    }
}