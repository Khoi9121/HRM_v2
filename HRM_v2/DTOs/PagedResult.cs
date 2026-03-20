namespace HRM_v2.DTOs
{
    public class PagedResult<T>
    {
        public int TotalItem { get; set; }
        public List<T> Data { get; set; } = new();
    }
}
