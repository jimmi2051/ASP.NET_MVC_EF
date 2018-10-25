using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SuperMarketMini.Domain
{
    public class SuperMarketMini_Context : DbContext
    {
        public SuperMarketMini_Context()
           : base("SuperMartketMini")
        {        
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order_Detail> Order_Detail { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Receipt_Note> Receipt_Note { get; set; }
        public virtual DbSet<Receipt_Note_Detail> Receipt_Note_Detail { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<TypeUser> TyperUsers { get; set; }
       public virtual DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
       .Property(c => c.Id)
       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}