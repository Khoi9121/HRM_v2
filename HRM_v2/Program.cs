using HRM_v2.Data;
using HRM_v2.Services.Implementations;
using HRM_v2.Services.Interfaces;
using HRM_v2.Services.Job;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;

// JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Swagger JWT
using Microsoft.OpenApi.Models;
using HRM_v2.Middleware;

var builder = WebApplication.CreateBuilder(args);

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

// ====================== SWAGGER + JWT ======================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRM API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ====================== DB ======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====================== DI ======================
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IBirthdayService, BirthdayService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
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

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsync(
                    "{\"message\": \"Bạn chưa đăng nhập hoặc token không hợp lệ\"}"
                );
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsync(
                    "{\"message\": \"Bạn không có quyền hạn này\"}"
                );
            }
        };
    });

builder.Services.AddAuthorization();

// ====================== HANGFIRE ======================
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// ====================== PIPELINE ======================

// CORS
app.UseCors("AllowAll");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Hangfire
app.UseHangfireDashboard();

// HTTPS
app.UseHttpsRedirection();

// 🔥 QUAN TRỌNG NHẤT
app.UseAuthentication();
app.UseAuthorization();

// ✅ Audit phải đặt SAU Auth
app.UseMiddleware<AuditMiddleware>();

// Map API
app.MapControllers();

// ====================== CRON JOB ======================
RecurringJob.AddOrUpdate<BirthdayJob>(
    "birthday-job",
    job => job.Run(),
    "* * * * *"
);

app.Run();