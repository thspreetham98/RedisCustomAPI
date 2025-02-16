﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedisCustomAPI.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace RedisCustomAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Redis API"
                });
            });

            var localConfig = Configuration.GetSection("RedisServerConfig").GetSection("LocalServer");
            var dellConfig = Configuration.GetSection("RedisServerConfig").GetSection("DellServer");

            services.AddTransient<IReadWriteService>(s => new ReadWriteService(localConfig.GetValue<string>("Host"),
                                                                                localConfig.GetValue<int>("Port"),
                                                                                localConfig.GetValue<string>("Password"),
                                                                                localConfig.GetValue<bool>("Encrypted")));
            services.AddTransient<IReadOnlyService>(s => new ReadOnlyService(dellConfig.GetValue<string>("Host"),
                                                                                dellConfig.GetValue<int>("Port"),
                                                                                dellConfig.GetValue<string>("Password"),
                                                                                dellConfig.GetValue<bool>("Encrypted")));
            //services.AddTransient<IReadWriteService>(s => new ReadWriteService("127.0.0.1", 6379, null));
            //services.AddTransient<IReadOnlyService>(s => new ReadOnlyService("127.0.0.1", 6379, null));
            //services.AddSingleton<IReadOnlyService, ReadOnlyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
