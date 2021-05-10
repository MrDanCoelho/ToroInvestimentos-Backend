using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToroInvestimentos.Backend.Application.Services;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Interfaces.IAdapters;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.Relationship;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using ToroInvestimentos.Backend.Infra.Adapters;
using ToroInvestimentos.Backend.Infra.AutoMapperProfiles;
using ToroInvestimentos.Backend.Infra.Maps;
using ToroInvestimentos.Backend.Infra.Maps.BankAccount;
using ToroInvestimentos.Backend.Infra.Maps.BankClient;
using ToroInvestimentos.Backend.Infra.Maps.Relationship;
using ToroInvestimentos.Backend.Infra.Maps.User;
using ToroInvestimentos.Backend.Infra.Migrations;
using ToroInvestimentos.Backend.Infra.Repositories;
using ToroInvestimentos.Backend.Infra.Repositories.BankAccount;
using ToroInvestimentos.Backend.Infra.Repositories.BankClient;
using ToroInvestimentos.Backend.Infra.Repositories.Relationship;
using ToroInvestimentos.Backend.Infra.Repositories.User;

namespace ToroInvestimentos.Backend.Infra
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// Dependency injection helper method
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        /// <param name="config">The app's <see cref="IConfiguration"/></param>
        public static void ConfigureAllServices(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureSettings(config);
            services.ConfigureRepositories(config);
            services.ConfigureAdapters();
            services.ConfigureServices();
            services.ConfigureDistributedCache(config);
            services.ConfigureLogger(config);
            services.ConfigureSwagger();
        }

        
        /// <summary>
        /// <see cref="AppSettings"/> configuration Helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        /// <param name="config">The app's <see cref="IConfiguration"/></param>
        private static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);

            services.AddAutoMapper(typeof(DefaultProfile));
            services.AddHttpContextAccessor();
        }


        /// <summary>
        /// Repository configuration helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        /// <param name="config">The app's <see cref="IConfiguration"/></param>
        private static void ConfigureRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(config.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(TestMigration).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
            
            FluentMapper.Initialize(c =>
            {
                c.AddMap(new BankAccountMap());
                c.AddMap(new BankAccountExchangeMap());
                c.AddMap(new BankAccountStockMap());

                c.AddMap(new BankClientMap());
                c.AddMap(new BankClientAccountMap());

                c.AddMap(new UserBankClientMap());
                c.AddMap(new UserMap());
                
                c.AddMap(new RefreshTokenMap());
                c.AddMap(new StockMap());
                c.ForDommel();
            });

            services.AddTransient<IDbConnection>(a 
                => new NpgsqlConnection(config.GetConnectionString("DefaultConnection")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IBankAccountExchangeRepository, BankAccountExchangeRepository>();
            services.AddScoped<IBankAccountStockRepository, BankAccountStockRepository>();
            
            services.AddScoped<IBankClientRepository, BankClientRepository>();
            
            services.AddScoped<IBankClientAccountRepository, BankClientAccountRepository>();
            services.AddScoped<IUserBankClientRepository, UserBankClientRepository>();
            
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            
        }

        /// <summary>
        /// Adapter configuration helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        private static void ConfigureAdapters(this IServiceCollection services)
        {
            services.AddHttpClient<IBrokerAdapter, BrokerAdapter>()
                .AddPolicyHandler(HttpPolicy.GetRetryPolicy());
        }

        /// <summary>
        /// Service configuration helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBrokerService, BrokerService>();
        }

        /// <summary>
        /// Distributed cache helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        /// <param name="config">The app's <see cref="IConfiguration"/></param>
        private static void ConfigureDistributedCache(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetSection("ConnectionStrings:DistributedCache").Value;

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;

                var assemblyName = Assembly.GetEntryAssembly()?.GetName();
                if (assemblyName != null) options.InstanceName = assemblyName.Name;
            });
        }

        /// <summary>
        /// Logging configuration helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        /// <param name="config">The app's <see cref="IConfiguration"/></param>
        private static void ConfigureLogger(this IServiceCollection services, IConfiguration config)
        {
            var serilogLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
 
            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }
        
        /// <summary>
        /// Swagger configuration helper
        /// </summary>
        /// <param name="services">The app's <see cref="IServiceCollection"/></param>
        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ToroInvestimentos.Backend.API", Version = "v1"});
                
                c.OperationFilter<RemoveVersionFromParameter>();
                
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                c.DocInclusionPredicate((version, desc) =>
                {
                    var actionDescriptor = desc.ActionDescriptor.DisplayName;

                    if (actionDescriptor == null) return false;
                    if (!actionDescriptor.Contains(version)) return false;
                    
                    var values = desc.RelativePath.Split("/").Select(v => v.Replace("v{version}", version));

                    desc.RelativePath = string.Join("/", values);

                    return true;
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                
                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
    
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var (key, value) in swaggerDoc.Paths)
            {
                paths.Add(key.Replace("v{version}", swaggerDoc.Info.Version), value);
            }

            swaggerDoc.Paths = paths;
        }
    }
}