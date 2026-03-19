namespace HRM_v2.DTOs
{
    public class FilterNhanVienDTO
    {
        public List<int>? ChucVuIds { get; set; }
        public string? Keyword { get; set; }
        public string? Email { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
