using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tthk.ContactsRegistry.Data
{
    public class ContactsContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasMany(x => x.Emails).WithOne(x => x.Contact).HasForeignKey(x => x.ContactId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasMany(x => x.PhoneNumbers).WithOne(x => x.Contact).HasForeignKey(x => x.ContactId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
