namespace HRM_v2.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Action { get; set; }

        public string Method { get; set; }

        public string Endpoint { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
