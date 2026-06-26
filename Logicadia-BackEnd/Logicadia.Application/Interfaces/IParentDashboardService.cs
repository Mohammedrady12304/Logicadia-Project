using Logicadia.Application.Features.DTOs.ParentDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IParentDashboardService
    {
        Task<List<ChildSummaryDto>> GetParentChildrenAsync(int parentId);
        Task<ChildProgressDetailsDto?> GetChildProgressAsync(int parentId, int childId);
        Task<bool> AssignPathToChildAsync(int parentId, int childId, AssignPathDto dto);
    }
}
