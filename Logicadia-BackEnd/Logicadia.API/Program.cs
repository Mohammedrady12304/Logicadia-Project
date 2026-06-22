
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using Logicadia.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Logicadia.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Controllers
            builder.Services.AddControllers();
            // JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey!)
                            )
                    };
            });


            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Auth Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IUserService, UserService>();

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
        builder.Configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly("Logicadia.Infrastructure")
        ));


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

            app.UseAuthentication();
            app.UseAuthorization();
            // Seed Admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context =
                        services.GetRequiredService<ApplicationDbContext>();

                    if (!context.Users.Any(u =>
                        u.Email == "admin@logicadia.com"))
                    {
                        var adminUser = new ApplicationUser
                        {
                            Name = "Admin",

                            Email = "admin@logicadia.com",
                            PasswordHash =
                                BCrypt.Net.BCrypt.HashPassword(
                                    "admin123"
                                ),

                            RoleId = 1,

                            CreatedAt = DateTime.UtcNow
                        };


                        context.Users.Add(adminUser);

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error during Admin Seeding: {ex.Message}"
                    );
                }
            }
            app.MapControllers();

            app.Run();
        }
    }
}
