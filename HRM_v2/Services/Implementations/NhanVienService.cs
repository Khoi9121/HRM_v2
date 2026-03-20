using HRM_v2.Data;
using HRM_v2.DTOs;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace HRM_v2.Services.Implementations
{
    public class NhanVienService : INhanVienService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public NhanVienService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
        public async Task<PagedResult<NhanVienResponseDTO>> Filter(FilterNhanVienDTO request)
        {
            var chucVuIds = request.ChucVuIds != null && request.ChucVuIds.Any()
                ? string.Join(",", request.ChucVuIds)
                : null;

            //var connectionString = "Server=MSI;Database=HRM_v2;Trusted_Connection=True;TrustServerCertificate=True";
            var connectionString = _config["ConnectionStrings:DefaultConnection"];

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_FilterNhanVien", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ChucVuIds", (object?)chucVuIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Keyword", (object?)request.Keyword ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)request.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", request.Page);
            cmd.Parameters.AddWithValue("@PageSize", request.PageSize);

            using var reader = await cmd.ExecuteReaderAsync();

            int totalItem = 0;
            if (await reader.ReadAsync())
            {
                totalItem = reader.GetInt32(0);
            }

            await reader.NextResultAsync();

            var data = new List<NhanVienResponseDTO>();

            while (await reader.ReadAsync())
            {
                data.Add(new NhanVienResponseDTO
                {
                    Id = reader.GetInt32(0),
                    TenNhanVien = reader.GetString(1),
                    TenChucVu = reader.GetString(2),
                    Email = reader.GetString(3)
                });
            }

            return new PagedResult<NhanVienResponseDTO>
            {
                TotalItem = totalItem,
                Data = data
            };
        }
        // test
    }
}
