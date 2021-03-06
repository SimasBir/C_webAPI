using _0124ShopAppAPI.AutoMapper;
using _0124ShopAppAPI.Data;
using _0124ShopAppAPI.Services;
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

namespace _0124ShopAppAPI
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
            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(d => d.UseSqlServer(defaultConnection));
            services.AddTransient<ShopService>();
            services.AddTransient<ShopItemService>();

            services.AddAutoMapper(typeof(ShopProfile));

            services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowMyOrigin",
            //    builder => builder.WithOrigins("http://localhost:4200",
            //                                    "https://localhost:44328")
            //                                    .AllowAnyMethod()
            //                                    .AllowAnyHeader()

            //    );
            //});

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "_0124ShopAppAPI", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "_0124ShopAppAPI v1"));
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors("AllowMyOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
