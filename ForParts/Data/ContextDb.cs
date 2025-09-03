
using ForParts.Models.Auth;
using ForParts.Models.Budgetes;
using ForParts.Models.Customers;
using ForParts.Models.Invoice;
using ForParts.Models.Product;
using ForParts.Models.Supply;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Data
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options)
            : base(options)
        {
        }

        //LOGIN REGISTER
        public DbSet<User> Users { get; set; }

        //SUPPLY
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Glass> Glasses { get; set; }
        public DbSet<Accessory> Accessories { get; set; }

        //STOCK
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        //FACTURAS
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMovement> ProductMovements { get; set; }
        //BUDGETS
        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Formula> Formulas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único en Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.userEmail)
                .IsUnique()
                .HasDatabaseName("UQ_UserEmail");

            // Índice único en Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.userName)
                .IsUnique()
                .HasDatabaseName("UQ_UserName");

            // Índice filtrado en Email (solo confirmados)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.userEmail)
                .HasFilter("[IsEmailConfirmed] = 1")
                .HasDatabaseName("UQ_Users_Active_Email");

            // Herencia TPT para Supply y sus derivados
            modelBuilder.Entity<Supply>().ToTable("Supply");
            modelBuilder.Entity<Glass>().ToTable("Glass");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Accessory>().ToTable("Accessory");

            //Indice de busqueda por Sku
            modelBuilder.Entity<Supply>()
                .HasIndex(u => u.codeSupply)
                .IsUnique()
                .HasDatabaseName("UQ_CodeSupply");

            //Indice de busqueda por Sku
            modelBuilder.Entity<Stock>()
                .HasIndex(u => u.codeSupply )
                .IsUnique()
                .HasDatabaseName("UQ_CodeSupply");

            modelBuilder.Entity<Invoice>()
    .HasMany(i => i.Items)
    .WithOne(ii => ii.Invoice)
    .HasForeignKey(ii => ii.InvoiceId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerId);

            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.DireccionFiscal);
            modelBuilder.Entity<SupplyNecessary>()
.HasOne(sn => sn.supply)
.WithMany()
.HasForeignKey(sn => sn.supplyId);

            modelBuilder.Entity<SupplyNecessary>()
                .HasOne(sn => sn.Product)
                .WithMany(p => p.ProductoInsumos)
                .HasForeignKey(sn => sn.productId);

            // Configuración de la relación uno a uno entre Supply y Stock
            modelBuilder.Entity<Stock>()
                .HasOne(st => st.Supply)
                .WithOne(sp => sp.Stock)
                .HasForeignKey<Stock>(st => st.SupplyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Asegurar que Stock tiene su clave primaria configurada
            modelBuilder.Entity<Stock>()
                .HasKey(s => s.idStock);
        }
    }
}
