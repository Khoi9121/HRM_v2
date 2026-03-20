using HRM_v2.DTOs;

namespace HRM_v2.Services.Interfaces
{
    public interface INhanVienService
    {
        //Task<IEnumerable<NhanVienResponseDTO>> GetAll(int page, int pageSize);
        Task Create(NhanVienCreateDTO dto);
        Task Delete(int id);
        Task<PagedResult<NhanVienResponseDTO>> Filter(FilterNhanVienDTO request);
        Task<List<ThongKeChucVuDTO>> GetThongKeChucVuAsync();


    }
}
