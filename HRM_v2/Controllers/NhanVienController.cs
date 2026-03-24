using HRM_v2.DTOs;
using HRM_v2.Services.Implementations;
using HRM_v2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _service;

        public NhanVienController(INhanVienService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterNhanVienDTO request)
        {
            var data = await _service.Filter(request);
            return Ok(data);
        }
        [Authorize(Roles = "GiamDoc,TruongPhong")]
        [HttpPost]
        public async Task<IActionResult> Create(NhanVienCreateDTO dto)
        {
            await _service.Create(dto);
            return Ok();
        }
        [Authorize(Roles = "GiamDoc,TruongPhong")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("Xóa thành công");
        }
        [Authorize(Roles = "GiamDoc,TruongPhong")]
        [HttpGet("thong-ke-chuc-vu")]
        public async Task<IActionResult> GetThongKeChucVu()
        {
            var result = await _service.GetThongKeChucVuAsync();
            return Ok(result);
        }

    }
}
