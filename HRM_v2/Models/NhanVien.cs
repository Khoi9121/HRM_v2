namespace HRM_v2.Models
{
    public class NhanVien
    {
        public int Id { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Email { get; set; }
        public int ChucVuId { get; set; }
        public DateTime? LastBirthdayEmailSent { get; set; }
    }
}
