using System;
using System.Collections.Generic;
using ApiNetCore.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ApiNetCore.ContextMysql;

public partial class MyDbContextMysql : DbContext
{
    public MyDbContextMysql()
    {
    }

    public MyDbContextMysql(DbContextOptions<MyDbContextMysql> options)
        : base(options)
    {
    }

    public virtual DbSet<Geogeoc> Geogeocs { get; set; }

    public virtual DbSet<Geogyu> Geogyus { get; set; }

    public virtual DbSet<Georeg> Georegs { get; set; }

    public virtual DbSet<Geoubi> Geoubis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.40.172;database=geomevecsa2025;user=pruebas;password=12345678", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Geogeoc>(entity =>
        {
            entity.HasKey(e => e.Geoccod).HasName("PRIMARY");

            entity.ToTable("geogeoc");

            entity.Property(e => e.Geoccod)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasComment("codigo geocerca")
                .HasColumnName("geoccod");
            entity.Property(e => e.Geocact)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasComment("estdo activo=1 inactivo =0")
                .HasColumnName("geocact");
            entity.Property(e => e.Geocarm)
                .HasPrecision(15,2)
                .HasComment("area en metros")
                .HasColumnName("geocarm");
            entity.Property(e => e.Geocciud)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("ciudad ")
                .HasColumnName("geocciud");
            entity.Property(e => e.Geoccoor)
                .HasComment("coordenadas ")
                .HasColumnType("json")
                .HasColumnName("geoccoor");
            entity.Property(e => e.Geocdesc)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasComment("descripcion")
                .HasColumnName("geocdesc");
            entity.Property(e => e.Geocdirre)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("direccion referencia")
                .HasColumnName("geocdirre");
            entity.Property(e => e.Geoceqcre)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("equipo edita")
                .HasColumnName("geoceqcre");
            entity.Property(e => e.Geoceqedi)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("equipo edita")
                .HasColumnName("geoceqedi");
            entity.Property(e => e.Geocest)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasComment("estado de geocerca")
                .HasColumnName("geocest");
            entity.Property(e => e.Geocfcre)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha hora creacion")
                .HasColumnType("datetime")
                .HasColumnName("geocfcre");
            entity.Property(e => e.Geocfedi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha y hora edicion")
                .HasColumnType("datetime")
                .HasColumnName("geocfedi");
            entity.Property(e => e.Geoclat)
                .HasPrecision(10,8)
                .HasComment("latitud retferecnia ")
                .HasColumnName("geoclat");
            entity.Property(e => e.Geoclon)
                .HasPrecision(11,8)
                .HasComment("longitud referencia ")
                .HasColumnName("geoclon");
            entity.Property(e => e.Geocnom)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("nombre de geocerca")
                .HasColumnName("geocnom");
            entity.Property(e => e.Geocpais)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("pais ")
                .HasColumnName("geocpais");
            entity.Property(e => e.Geocperm)
                .HasPrecision(10,2)
                .HasComment("perimetro")
                .HasColumnName("geocperm");
            entity.Property(e => e.Geocpri)
                .HasComment("prioridad")
                .HasColumnName("geocpri");
            entity.Property(e => e.Geocprov)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("provincia")
                .HasColumnName("geocprov");
            entity.Property(e => e.Geocsec)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("sector ")
                .HasColumnName("geocsec");
            entity.Property(e => e.Geocuscre)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario crea")
                .HasColumnName("geocuscre");
            entity.Property(e => e.Geocusedi)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario edita")
                .HasColumnName("geocusedi");
        });

        modelBuilder.Entity<Geogyu>(entity =>
        {
            entity.HasKey(e => e.Geugid).HasName("PRIMARY");

            entity.ToTable("geogyu");

            entity.HasIndex(e => e.Geugidg, "geogyu_geogeoc_geoccod_fk");

            entity.Property(e => e.Geugid)
                .HasComment("id relacion")
                .HasColumnName("geugid");
            entity.Property(e => e.Geugeqcre)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("equipo de creacion")
                .HasColumnName("geugeqcre");
            entity.Property(e => e.Geugeqedi)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("equipo de edicion")
                .HasColumnName("geugeqedi");
            entity.Property(e => e.Geuglat)
                .HasPrecision(10,8)
                .HasComment("latitud")
                .HasColumnName("geuglat");
            entity.Property(e => e.Geuglon)
                .HasPrecision(11,8)
                .HasComment("longitud")
                .HasColumnName("geuglon");
            entity.Property(e => e.Geugfcre)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha hora creacion")
                .HasColumnType("datetime")
                .HasColumnName("geugfcre");
            entity.Property(e => e.Geugfedi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha y hora edicion")
                .HasColumnType("datetime")
                .HasColumnName("geugfedi");
            entity.Property(e => e.Geugidg)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("id geocerca")
                .HasColumnName("geugidg");
            entity.Property(e => e.Geugidv)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("id usuario")
                .HasColumnName("geugidv");
            entity.Property(e => e.Geuguscre)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario crea")
                .HasColumnName("geuguscre");
            entity.Property(e => e.Geugusedi)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario edita")
                .HasColumnName("geugusedi");

            entity.HasOne(d => d.GeugidgNavigation).WithMany(p => p.Geogyus)
                .HasForeignKey(d => d.Geugidg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("geogyu_geogeoc_geoccod_fk");
        });

        modelBuilder.Entity<Georeg>(entity =>
        {
            entity.HasKey(e => e.Regid).HasName("PRIMARY");

            entity.ToTable("georeg");

            entity.Property(e => e.Regid)
                .HasComment("id registro")
                .HasColumnName("regid");
            entity.Property(e => e.Regcodcli)
                .HasMaxLength(13)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("codigo cliente")
                .HasColumnName("regcodcli");
            entity.Property(e => e.Regdirref)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("direccion referencia")
                .HasColumnName("regdirref");
            entity.Property(e => e.Regfech)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha registro")
                .HasColumnType("datetime")
                .HasColumnName("regfech");
            entity.Property(e => e.Reglat)
                .HasPrecision(10)
                .HasComment("latitu")
                .HasColumnName("reglat");
            entity.Property(e => e.Reglog)
                .HasPrecision(11)
                .HasComment("longitud")
                .HasColumnName("reglog");
            entity.Property(e => e.Regnomcli)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("nombre cleinte")
                .HasColumnName("regnomcli");
            entity.Property(e => e.Regnum1)
                .HasComment("numero 1")
                .HasColumnName("regnum1");
            entity.Property(e => e.Regnum2)
                .HasComment("numero 2")
                .HasColumnName("regnum2");
            entity.Property(e => e.Regser1)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("serie 1")
                .HasColumnName("regser1");
            entity.Property(e => e.Regser2)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("serie 2")
                .HasColumnName("regser2");
            entity.Property(e => e.Regser3)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("serie 3")
                .HasColumnName("regser3");
            entity.Property(e => e.Regtiptra)
                .HasComment("tipo transaccion 1=cobros 2=pedidos")
                .HasColumnName("regtiptra");
            entity.Property(e => e.Regusu)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario registra")
                .HasColumnName("regusu");
        });

        modelBuilder.Entity<Geoubi>(entity =>
        {
            entity.HasKey(e => e.Geubid).HasName("PRIMARY");

            entity.ToTable("geoubi");

            entity.Property(e => e.Geubid)
                .HasComment("id registro")
                .HasColumnName("geubid");
            entity.Property(e => e.Geubfech)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("fecha registro")
                .HasColumnType("datetime")
                .HasColumnName("geubfech");
            entity.Property(e => e.Geublat)
                .HasPrecision(10, 8)
                .HasComment("latitud")
                .HasColumnName("geublat");
            entity.Property(e => e.Geublon)
                .HasPrecision(11, 8)
                .HasComment("longitud")
                .HasColumnName("geublon");
            entity.Property(e => e.Geubusu)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasComment("usuario crea")
                .HasColumnName("geubusu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
