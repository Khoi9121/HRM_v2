using HRM_v2.DTOs;
using HRM_v2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRM_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Create(NhanVienCreateDTO dto)
        {
            await _service.Create(dto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("Xóa thành công");
        }
        
    }
}
