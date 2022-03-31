using Microsoft.EntityFrameworkCore;
using Allsop.Models.Entities;

namespace Allsop.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Activity>? Activities { get; set; }
        public DbSet<EventStore>? EventsStore { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Voucher>? Vouchers { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
    }
}