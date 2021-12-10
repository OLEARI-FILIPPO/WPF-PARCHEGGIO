using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class ParkingManagementContext : DbContext
    {
        public ParkingManagementContext()
        {
        }

        public ParkingManagementContext(DbContextOptions<ParkingManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<InfoParking> InfoParking { get; set; }
        public virtual DbSet<OwnerVehicle> OwnerVehicle { get; set; }
        public virtual DbSet<Parking> Parking { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Startup.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.EntryTimeDate).HasColumnType("datetime");

                entity.Property(e => e.ExitTimeDate).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ParkingId)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Revenue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SearchDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Token)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<InfoParking>(entity =>
            {
                entity.HasKey(e => e.InfoParkId);

                entity.Property(e => e.NamePark)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OwnerVehicle>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.Property(e => e.DateBirth).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OwnerVehicle)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_OwnerVehicle_Users");
            });

            modelBuilder.Entity<Parking>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EntryTimeDate).HasColumnType("datetime");

                entity.Property(e => e.ParkingId)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Revenue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Token)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.InfoPark)
                    .WithMany(p => p.Parking)
                    .HasForeignKey(d => d.InfoParkId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Parking)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_Parking_Vehicle");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastLogout).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasIndex(e => e.VehicleId)
                    .HasName("UQ_LicensePlate_Vehicle")
                    .IsUnique();

                entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

                entity.Property(e => e.LicensePlate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_OwnerVehicle");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
