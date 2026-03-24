using System.ComponentModel.DataAnnotations;

namespace HRM_v2.Models
{
    public class UserAccount
    {
        public string id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public int NhanVienId { get; set; }

        public string Role { get; set; } // "GiamDoc,TruongPhong" hoặc "NhanVien"

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
