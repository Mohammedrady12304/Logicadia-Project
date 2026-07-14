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
            // Check child belongs to current parent user
            var child = await _context.Children
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c =>
                    c.Id == childId &&
                    c.Parent.UserId == parentId
                );

            if (child == null)
                return null;


            // Get child progress
            var progress = await _context.UserProgress
                .Include(p => p.Level)
                .Include(p => p.Story)
                .Include(p => p.Scenario)
                .FirstOrDefaultAsync(p => p.ChildId == childId);


            // Get achievements
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
            var child = await _context.Children
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c =>
                    c.Id == childId &&
                    c.Parent.UserId == parentId
                );

            if (child == null)
                return false;

            child.Age = dto.Age;
            child.Interests = dto.Interests;
            child.FavoriteColor = dto.FavoriteColor;
            child.FavoriteAnimal = dto.FavoriteAnimal;
            child.LearningTopic = dto.LearningTopic;
            child.ReadingLevel = dto.ReadingLevel;
            child.PreferredLanguage = dto.PreferredLanguage;

            
            _context.Set<Child>().Update(child);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;



        }
    }
}
