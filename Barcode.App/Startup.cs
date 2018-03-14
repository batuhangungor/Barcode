﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Barcode.Data.Context;
using Microsoft.EntityFrameworkCore;
using Barcode.Data.Repository;
using Barcode.Models;

namespace Barcode.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = @"Server=DESKTOP-3I7VBMV\SQLEXPRESS; Database=Barcode;Trusted_Connection=True;";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IRepository<Product>, EFRepository<Product>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            
        }
    }
}
