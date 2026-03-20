using HRM_v2.Data;
using HRM_v2.Services.Implementations;
using HRM_v2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 THÊM DÒNG NÀY (quan trọng để debug)
Console.WriteLine("ROOT PATH: " + builder.Environment.ContentRootPath);

// 1. Cấu hình dịch vụ CORS (Đăng ký Policy)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Cấu hình Controllers và Fix lỗi vòng lặp JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Cấu hình DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Đăng ký DI Service
builder.Services.AddScoped<INhanVienService, NhanVienService>();

var app = builder.Build();

// --- THỨ TỰ MIDDLEWARE (CỰC KỲ QUAN TRỌNG) ---

// Kích hoạt CORS ngay đầu tiên để tránh bị chặn bởi các Middleware khác
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Chuyển hướng HTTPS (đặt sau CORS để trình duyệt nhận được Header CORS trước khi redirect)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();