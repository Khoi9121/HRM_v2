using System.ComponentModel.DataAnnotations.Schema;

namespace HRM_v2.DTOs
{
    public class NhanVienResponseDTO
    {
        public int Id { get; set; }
        public string TenNhanVien { get; set; }
        public string TenChucVu { get; set; }
        [NotMapped]
        public string test { get; set; } = "Xin chào";
    }
}
