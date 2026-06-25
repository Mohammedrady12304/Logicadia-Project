using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IScoreService
    {
        Task<int> CalculateTotalXpAsync(int userId);
        Task<int> GetCurrentLevelAsync(int userId);
        Task<int> GetXpToNextLevelAsync(int userId);
        Task<double> CalculateAccuracyAsync(int userId);
    }
}
