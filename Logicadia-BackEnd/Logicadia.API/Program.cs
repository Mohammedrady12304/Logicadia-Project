using Logicadia.Application.Interfaces;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using Logicadia.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Logicadia.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // ✅ استخدام AddIdentityCore بدل AddIdentity عشان متتعارضيش مع JWT
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // JWT Config
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            // ✅ JWT Authentication من غير تعارض مع Identity
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!)),

                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var errorMessage = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            message = "You are not authorized to access this resource."
                        });

                        return context.Response.WriteAsync(errorMessage);
                    }
                };
            });

            // AutoMapper - 
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Auth Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();

            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Other Services
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<IUserProgressService, UserProgressService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILevelUnlockService, LevelUnlockService>();
            builder.Services.AddScoped<IScoreService, ScoreService>();
            builder.Services.AddScoped<ILevelRepository, LevelRepository>();
            builder.Services.AddScoped<IStoryRepository, StoryRepository>();
            builder.Services.AddScoped<IStoryService, StoryService>();
            builder.Services.AddScoped<IAchievementEngine, AchievementEngine>();
            builder.Services.AddScoped<IScenarioRepository, ScenarioRepository>();
            builder.Services.AddScoped<IScenarioService, ScenarioService>();
            builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
            builder.Services.AddScoped<IChoiceService, ChoiceService>();
            builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddScoped<IParentDashboardService, ParentDashboardService>();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Logicadia.Infrastructure")
                ));

            var app = builder.Build();

            // CORS لازم قبل أي حاجة
            app.UseCors("AllowAngular");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ✅ الترتيب مهم جداً — Authentication الأول
            app.UseAuthentication();
            app.UseAuthorization();

            // ✅ Seed Admin باستخدام UserManager بدل BCrypt مباشرة
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    // إنشاء Role لو مش موجودة
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
                    }

                    // إنشاء Admin لو مش موجود
                    if (await userManager.FindByEmailAsync("admin@logicadia.com") == null)
                    {
                        var adminUser = new ApplicationUser
                        {
                            Name = "Admin",
                            UserName = "Admin",
                            Email = "admin@logicadia.com",
                            RoleId = 1,
                            CreatedAt = DateTime.UtcNow
                        };

                        var result = await userManager.CreateAsync(adminUser, "admin123");

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during Admin Seeding: {ex.Message}");
                }
            }

            app.MapControllers();
            app.Run();
        }
    }
}