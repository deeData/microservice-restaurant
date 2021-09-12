using AutoMapper;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*copied over from ProductAPI and modified for ShoppingCartAPI*/

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //create mapper and add as singleton, configure to .netcoreAPI
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //accessible to Controller
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

            services.AddControllers();
            services.AddHttpClient<ICouponRepository, CouponRepository>(u => u.BaseAddress = 
                new Uri(Configuration["ServiceUrls:CouponAPI"]));

            //add token
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    //Idenity Server URL
                    options.Authority = "https://localhost:44393";
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            //add authorization 
            services.AddAuthorization(options => { });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.ShoppingCartAPI", Version = "v1" });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mango.Services.ShoppingCartAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //add Authentication before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
