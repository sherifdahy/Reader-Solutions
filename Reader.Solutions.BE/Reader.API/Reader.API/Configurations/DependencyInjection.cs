using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Reader.BLL.Authentication;
using Reader.BLL.Interfaces;
using Reader.BLL.Services;
using Reader.DAL.Data;
using Reader.Entities.Entities;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using FluentValidation;
using NOTE.Solutions.Entities.Interfaces;
using NOTE.Solutions.DAL.Repository;

namespace Reader.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDIConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbConfig(configuration);
            services.AddServiceConfig(configuration);
            services.AddMapsterConfig();
            services.AddIdentityDbContextConfig(configuration);
            services.AddSwaggerConfig();
            services.AddAuthConfig(configuration);
            services.AddFluentValidationConfig();
            return services;
        }

        public static IServiceCollection AddServiceConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJWTProvider, JWTProvider>();

            return services;
        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            var bllAssembly = Assembly.Load("Reader.BLL");

            mappingConfig.Scan(bllAssembly);
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }
        public static IServiceCollection AddDbConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                var connectionString = configuration.GetConnectionString("default") ?? throw new Exception("Can't find Connection String");
                option.UseSqlServer(connectionString);

            });
            return services;
        }
        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            var validationAssembly = Assembly.Load("Reader.BLL");
            services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(validationAssembly);
            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJWTProvider, JWTProvider>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                // validation 
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateActor = true,
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                };
            }).AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = false;
            });


            return services;
        }
        private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Reader.Solutions API",
                    Version = "v1",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                });
            });
            return services;
        }

        private static IServiceCollection AddIdentityDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection(); // لازم قبل Identity

            services.AddSingleton<TimeProvider>(TimeProvider.System); // حل مشكلة System.TimeProvider

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // password options
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                // requird confirmation
                //options.SignIn.RequireConfirmedEmail = true;

                // lockout options
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();
            return services;
        }
    }
}
