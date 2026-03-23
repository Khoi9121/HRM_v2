using HRM_v2.Data;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HRM_v2.Services.Job
{
    public class BirthdayJob
    {
        private readonly IBirthdayService _birthdayService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public BirthdayJob(
            IBirthdayService birthdayService,
            IEmailService emailService,
            AppDbContext context)
        {
            _birthdayService = birthdayService;
            _emailService = emailService;
            _context = context;
        }

        public void Run()
        {
            try
            {
                var list = _birthdayService.GetNhanVienSinhNhatHomNay()
                           ?? new List<NhanVien>();

                if (!list.Any())
                {
                    Console.WriteLine("Không có nhân viên sinh nhật hôm nay.");
                    return;
                }

                foreach (var nv in list)
                {
                    try
                    {
                        // ✅ Check email hợp lệ
                        if (string.IsNullOrEmpty(nv.Email))
                        {
                            Console.WriteLine($"Nhân viên {nv.TenNhanVien} không có email.");
                            continue;
                        }

                        // ✅ Gửi mail
                        _emailService.SendBirthdayEmail(nv.Email, nv.TenNhanVien);

                        // ✅ Đánh dấu đã gửi
                        nv.LastBirthdayEmailSent = DateTime.Now;

                        Console.WriteLine($"Đã gửi mail cho {nv.TenNhanVien}");

                        // ⏳ Delay tránh spam SMTP
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi gửi mail cho {nv.TenNhanVien}: {ex.Message}");
                    }
                }

                // ✅ Lưu DB
                _context.SaveChanges();

                Console.WriteLine("Hoàn thành job gửi mail sinh nhật.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi toàn bộ job: " + ex.Message);
            }
        }
    }
}