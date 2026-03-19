using Microsoft.EntityFrameworkCore;
using HRM_v2.Models;
using HRM_v2.DTOs;

namespace HRM_v2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<NhanVienResponseDTO> NhanVienResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NhanVienResponseDTO>().HasNoKey();
        }
    }
}