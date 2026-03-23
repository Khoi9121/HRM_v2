using HRM_v2.Data;
using HRM_v2.DTOs;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRM_v2.Services.Implementations
{
    public class BirthdayService : IBirthdayService
    {
        private readonly AppDbContext _context;

        public BirthdayService(AppDbContext context)
        {
            _context = context;
        }

        public List<NhanVien> GetNhanVienSinhNhatHomNay()
        {
            var today = DateTime.Today;

            return _context.NhanViens
            .Where(x => x.NgaySinh.HasValue
                && x.NgaySinh.Value.Day == today.Day
                && x.NgaySinh.Value.Month == today.Month
                && (x.LastBirthdayEmailSent == null
                    || x.LastBirthdayEmailSent.Value.Date < today))
            .ToList();
        }
    }
}
