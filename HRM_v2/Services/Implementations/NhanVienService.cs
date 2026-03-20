using HRM_v2.Data;
using HRM_v2.DTOs;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRM_v2.Services.Implementations
{
    public class NhanVienService : INhanVienService
    {
        private readonly AppDbContext _context;

        public NhanVienService(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<NhanVienResponseDTO>> GetAll(int page, int pageSize)
        //{
        //    return await _context.Set<NhanVienResponseDTO>()
        //        .FromSqlRaw("EXEC sp_GetNhanVienPaging @Page, @PageSize",
        //            new SqlParameter("@Page", page),
        //            new SqlParameter("@PageSize", pageSize))
        //        .ToListAsync();
        //}

        public async Task Create(NhanVienCreateDTO dto)
        {
            var nv = new NhanVien
            {
                TenNhanVien = dto.TenNhanVien,
                Email = dto.Email,
                ChucVuId = dto.ChucVuId
            };

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);

            if (nv == null)
                throw new Exception("Nhân viên không tồn tại");

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<NhanVienResponseDTO>> Filter(FilterNhanVienDTO request)
        {
            // 🔥 fallback default
            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var chucVuIds = request.ChucVuIds != null && request.ChucVuIds.Any()
                ? string.Join(",", request.ChucVuIds)
                : null;

            return await _context.NhanVienResponses
                .FromSqlRaw(
                    "EXEC sp_FilterNhanVien @ChucVuIds, @Keyword, @Email, @Page, @PageSize",
                    new SqlParameter("@ChucVuIds", (object?)chucVuIds ?? DBNull.Value),
                    new SqlParameter("@Keyword", (object?)request.Keyword ?? DBNull.Value),
                    new SqlParameter("@Email", (object?)request.Email ?? DBNull.Value),
                    new SqlParameter("@Page", request.Page),
                    new SqlParameter("@PageSize", request.PageSize)
                )
                .ToListAsync();
        }
        // test
    }
}
