using MiniAddressBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAddressBook.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext() : base("MySqlConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
