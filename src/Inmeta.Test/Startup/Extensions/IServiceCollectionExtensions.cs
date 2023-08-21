using Inmeta.Test.Data;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Repositories;
using Inmeta.Test.OAuth.Api.Refit;
using Inmeta.Test.OAuth.Services;
using Inmeta.Test.OAuth.Services.Abstractions;
using Inmeta.Test.Services;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Startup.Authorization.Handlers;
using Inmeta.Test.Startup.Authorization.Requirements;
using Inmeta.Test.Startup.RouteConstraints;
using Inmeta.Test.Startup.Swagger.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Inmeta.Test.Startup.Extensions
{
	internal static class IServiceCollectionExtensions
    {
        internal static void ConfigureServices(
            this IServiceCollection services,
            IConfiguration config,
            HashidsService hashidsService,
            JsonSerializerOptions jsonOptions
        )
        {
            services.AddSingleton(provider => jsonOptions);
            services.AddSingleton<IHashidsService, HashidsService>(provider => hashidsService);
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthorizationHandler, AdminOrCustomerHashIdDtoPropertyMatchAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AdminOrCustomerHashIdQueryMatchAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AdminOrCustomerHashIdRouteValueMatchAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AdminOrOrderHashIdRouteValueMatchAuthorizationHandler>();

			services.AddScoped<IAddressRepository, AddressRepository>();
			services.AddScoped<IAddressService, AddressService>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IOrderLogRepository, OrderLogRepository>();
			services.AddScoped<IOrderLogService, OrderLogService>();

			services.AddRefitClient<IGoogleOAuthApi>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(config["AppSettings:OAuth:Google:TokenBaseUri"]!));
            services.AddRefitClient<IMicrosoftOAuthApi>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(config["AppSettings:OAuth:Microsoft:TokenBaseUri"]!));
        }

        internal static void ConfigureApi(
            this IServiceCollection services,
			IConfiguration config,
			JsonSerializerOptions jsonOptions
        )
        {
            services
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
						string[] allowedOrigins = config.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
						string[] exposedHeaders = config.GetSection("ExposedHeaders").Get<string[]>() ?? Array.Empty<string>();
						builder
                            .WithOrigins(allowedOrigins)
                            .WithExposedHeaders(exposedHeaders)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
					});
                })
                .AddRouting(options => { options.ConstraintMap.Add("email", typeof(EmailRouteConstraint)); })
				.AddOutputCache(options =>
                {
                    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
                })
                .AddControllers()
                .AddJsonOptions(c =>
                {
                    c.JsonSerializerOptions.Encoder = jsonOptions.Encoder;
                    c.JsonSerializerOptions.WriteIndented = jsonOptions.WriteIndented;
                    foreach (var converter in jsonOptions.Converters)
                    {
                        c.JsonSerializerOptions.Converters.Add(converter);
                    }
                });

            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inmeta.Test.Orders API", Version = "v1" });
                    c.OperationFilter<HttpResponseOperationFilter>();
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Inmeta.Test JWT bearer access_token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }

        internal static void ConfigureAuthentication(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddAuthentication("Cookie")
                .AddCookie("Cookie", options =>
                {
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = config["AppSettings:OAuth:InmetaTest:Issuer"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = config["AppSettings:OAuth:InmetaTest:Issuer"],
                        ValidAudiences = new string[]
                        {
                            config["AppSettings:OAuth:Google:Web:ClientId"]!,
							config["AppSettings:OAuth:Google:Android:ClientId"]!,
							config["AppSettings:OAuth:Microsoft:Web:ClientId"]!,
					        config["AppSettings:OAuth:Microsoft:Android:ClientId"]!,
						},
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Secrets:OAuth:InmetaTest:SigningKey"]!))
                    };
                });
        }

        internal static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOrCustomerHashIdDtoPropertyMatch", p => p.AddRequirements(new AdminOrCustomerHashIdDtoPropertyMatchRequirement()));
                options.AddPolicy("AdminOrCustomerHashIdQueryMatch", p => p.AddRequirements(new AdminOrCustomerHashIdQueryMatchRequirement()));
                options.AddPolicy("AdminOrCustomerHashIdRouteValueMatch", p => p.AddRequirements(new AdminOrCustomerHashIdRouteValueMatchRequirement()));
                options.AddPolicy("AdminOrOrderHashIdRouteValueMatch", p => p.AddRequirements(new AdminOrOrderHashIdRouteValueMatchRequirement()));
            });
        }

        internal static void ConfigureData(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddDbContext<OrdersDbContext>(options =>
            {
                var connectionString = config.GetConnectionString("DbConnectionStringInmetaTest");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            dbContext.Database.Migrate();
        }
    }
}