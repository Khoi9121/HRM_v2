using HRM_v2.DTOs;

namespace HRM_v2.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDTO Login(LoginDTO dto);
    }
}
