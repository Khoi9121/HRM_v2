using HRM_v2.Data;
using HRM_v2.Models;

namespace HRM_v2.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var username = context.User.Identity?.Name ?? "Anonymous";

            var log = new AuditLog
            {
                Username = username,
                Action = $"{context.Request.Method} {context.Request.Path}",
                Method = context.Request.Method,
                Endpoint = context.Request.Path,
                Timestamp = DateTime.Now
            };

            db.AuditLogs.Add(log);
            await db.SaveChangesAsync();

            await _next(context);
        }
    }
}
