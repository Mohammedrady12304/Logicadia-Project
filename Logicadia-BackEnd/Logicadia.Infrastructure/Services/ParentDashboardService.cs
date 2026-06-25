using Logicadia.Application.Features.DTOs.ParentDashboard;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Services
{
    public class ParentDashboardService : IParentDashboardService
    {
        private readonly ApplicationDbContext _context;

        public ParentDashboardService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<ChildSummaryDto>> GetParentChildrenAsync(int parentUserId)
        {
            return await _context.Children
                .Where(c => c.Parent.UserId == parentUserId) 
                .Select(c => new ChildSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = c.Age,
                    CurrentLevelName = _context.UserProgress
                        .Where(p => p.ChildId == c.Id)
                        .Select(p => p.Level.Title)
                        .FirstOrDefault() ?? "Not started yet",
                    TotalAchievements = _context.UserAchievements.Count(ua => ua.ChildId == c.Id)
                })
                .ToListAsync();
        }


        public async Task<ChildProgressDetailsDto?> GetChildProgressAsync(int parentId, int childId)
        {
            var child = await _context.Children
                .FirstOrDefaultAsync(c => c.Id == childId && c.ParentId == parentId);

            if (child == null) return null;

            var progress = await _context.UserProgress
                .Include(p => p.Level)
                .Include(p => p.Story)
                .Include(p => p.Scenario)
                .FirstOrDefaultAsync(p => p.ChildId == childId);

            var achievements = await _context.UserAchievements
                .Where(ua => ua.ChildId == childId)
                .Select(ua => ua.Achievement.Title)
                .ToListAsync();

            return new ChildProgressDetailsDto
            {
                ChildId = child.Id,
                ChildName = child.Name,
                CurrentLevelId = progress?.LevelId ?? 0,
                CurrentLevelName = progress?.Level?.Title ?? "Not started yet",
                CurrentStoryId = progress?.StoryId,
                CurrentStoryTitle = progress?.Story?.Title ?? "Not selected",
                CurrentScenarioId = progress?.ScenarioId,
                CurrentScenarioName = progress?.Scenario?.Title ?? "Not selected",
                UnlockedAchievements = achievements
            };
        }

        
        public async Task<bool> AssignPathToChildAsync(int parentId, int childId, AssignPathDto dto)
        {
            var childExists = await _context.Children.AnyAsync(c => c.Id == childId && c.ParentId == parentId);
            if (!childExists) return false;

            var progress = await _context.UserProgress.FirstOrDefaultAsync(p => p.ChildId == childId);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    ChildId = childId,
                    LevelId = dto.LevelId,
                    StoryId = dto.StoryId,
                    ScenarioId = (int)dto.ScenarioId
                };
                _context.UserProgress.Add(progress);
            }
            else
            {
                progress.LevelId = dto.LevelId;
                progress.StoryId = dto.StoryId;
                progress.ScenarioId = (int)dto.ScenarioId;
                _context.UserProgress.Update(progress);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
