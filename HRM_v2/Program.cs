using HRM_v2.Data;
using HRM_v2.Services.Implementations;
using HRM_v2.Services.Interfaces;
using HRM_v2.Services.Job;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;

// 🔥 JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Debug đường dẫn
Console.WriteLine("ROOT PATH: " + builder.Environment.ContentRootPath);

// ====================== CORS ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ====================== CONTROLLER ======================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====================== DB CONTEXT ======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====================== DI SERVICES ======================
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IBirthdayService, BirthdayService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<BirthdayJob>();

// ====================== JWT ======================
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

// ====================== HANGFIRE ======================
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// ====================== MIDDLEWARE ======================

// CORS
app.UseCors("AllowAll");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Hangfire Dashboard
app.UseHangfireDashboard();

// HTTPS
app.UseHttpsRedirection();

// 🔥 JWT Middleware (QUAN TRỌNG: phải trước Authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ====================== CRON JOB ======================
RecurringJob.AddOrUpdate<BirthdayJob>(
    "birthday-job",
    job => job.Run(),
    "* * * * *" // chạy mỗi phút (test)
);

app.Run();