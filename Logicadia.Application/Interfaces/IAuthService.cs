using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logicadia.Application.DTOs;




namespace Logicadia.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterParentAsync(RegisterParentDto dto);
        Task<AuthResultDto> RegisterChildAsync(RegisterChildDto dto, string parentUserId);
    }
}
