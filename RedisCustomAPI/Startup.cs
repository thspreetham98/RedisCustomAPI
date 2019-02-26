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
            var redisConfig = Configuration.GetSection("RedisServerConfig").GetSection("LocalServer");
            services.AddTransient<IReadWriteService>(s => new ReadWriteService(redisConfig.GetValue<string>("Host"),
                                                                                redisConfig.GetValue<int>("Port"),
                                                                                redisConfig.GetValue<string>("Password")));
            services.AddTransient<IReadOnlyService>(s => new ReadOnlyService(redisConfig.GetValue<string>("Host"),
                                                                                redisConfig.GetValue<int>("Port"),
                                                                                redisConfig.GetValue<string>("Password")));
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
        }
    }
}
