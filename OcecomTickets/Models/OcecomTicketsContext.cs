using System.Data.Entity;

namespace OcecomTickets.Models
{
    public class OcecomTicketsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public OcecomTicketsContext() : base("name=OcecomTicketsContext")
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TicketHourRecord> TicketHourRecords { get; set; }

    }
}
