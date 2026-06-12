
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using Logicadia.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<ILevelRepository, LevelRepository>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<IStoryRepository, StoryRepository>();
            builder.Services.AddScoped<IStoryService, StoryService>();
            builder.Services.AddScoped<IScenarioRepository, ScenarioRepository>();
            builder.Services.AddScoped<IScenarioService, ScenarioService>();
            builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
            builder.Services.AddScoped<IChoiceService, ChoiceService>();
            builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); 
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
