using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ReverseProxyDbContext : DbContext
    {
        public ReverseProxyDbContext(DbContextOptions<ReverseProxyDbContext> options) : base(options)
        {
        }

        // DbSets for your entities can be added here, e.g.:
        public DbSet<ServerEntity> Servers { get; set; }
        public DbSet<CertificateEntity> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the entities here if needed
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServerEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(se => se.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Target).IsRequired().HasMaxLength(250);
                entity.Property(e => e.TargetPort).IsRequired();
            });

            modelBuilder.Entity<CertificateEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(se => se.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.ServerEntity).WithOne(e => e.Certificate).HasForeignKey<CertificateEntity>(e => e.ServerId);
            });

            //modelBuilder.Entity<LovRenewUnit>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(se => se.Id).ValueGeneratedOnAdd();
            //    entity.HasData(
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.minute,
            //            Name = RenewUnitEnum.minute.ToString(),
            //            Short = "m"
            //        },
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.hour,
            //            Name = RenewUnitEnum.hour.ToString(),
            //            Short = "h"
            //        },
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.day,
            //            Name = RenewUnitEnum.day.ToString(),
            //            Short = "d"
            //        },
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.week,
            //            Name = RenewUnitEnum.week.ToString(),
            //            Short = "w"
            //        },
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.month,
            //            Name = RenewUnitEnum.month.ToString(),
            //            Short = "M"
            //        },
            //        new LovRenewUnit
            //        {
            //            Id = (int)RenewUnitEnum.year,
            //            Name = RenewUnitEnum.year.ToString(),
            //            Short = "y"
            //        });
            //});
        }
    }
}
