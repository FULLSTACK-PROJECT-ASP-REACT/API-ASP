using System;
using System.Collections.Generic;
using ApiNetCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }

    public virtual DbSet<DetailTransaction> DetailTransactions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=156.244.32.23;User Id=sa;Password=Toyotaro@12;TrustServerCertificate=True;Database=DB_Test");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("tbl_category_pk");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusCat)
                .HasDefaultValue("A")
                .IsFixedLength();
        });

        modelBuilder.Entity<CategoryProduct>(entity =>
        {
            entity.HasKey(e => e.IdCaPr).HasName("tbl_category_product_pk");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cat).WithMany(p => p.TblCategoryProducts).HasConstraintName("tbl_category_product_tbl_category_id_cat_fk");

            entity.HasOne(d => d.Pro).WithMany(p => p.TblCategoryProducts).HasConstraintName("tbl_category_product_tbl_product_id_pro_fk");
        });

        modelBuilder.Entity<DetailTransaction>(entity =>
        {
            entity.HasKey(e => e.IdDT).HasName("tbl_detail_tran_pk");

            entity.HasOne(d => d.Pro).WithMany(p => p.TblDetailTransactions).HasConstraintName("tbl_detail_tran_tbl_product_id_pro_fk");

            entity.HasOne(d => d.Tra).WithMany(p => p.TblDetailTransactions).HasConstraintName("tbl_detail_tran_tbl_transaction_id_tra_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdPro)
                .HasName("tbl_product_pk")
                .IsClustered(false);

            entity.HasIndex(e => e.IdPro, "tbl_product_id_pro_index").IsClustered();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusPro)
                .HasDefaultValue("A")
                .IsFixedLength();
            entity.Property(e => e.StockPro).HasDefaultValueSql("('10000')");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTra).HasName("tbl_transaction_pk");

            entity.Property(e => e.EmissionDateTra).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusTra)
                .HasDefaultValue("A")
                .IsFixedLength();
            entity.Property(e => e.TypeTra).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
