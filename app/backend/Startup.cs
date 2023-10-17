using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models; // Added this using directive
using Microsoft.AspNetCore.Cors; // Added this using directive
using backend.Services;
using backend.Models;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddControllers();

            // Configure Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounting-Api", Version = "v1" });
            });

            var accountBalances = new Dictionary<Guid, int>();
            var accounts = new Dictionary<Guid, Account>();
            var transactions = new Dictionary<Guid, Transaction>();
            services.AddSingleton<Dictionary<Guid, int>>(_ => new Dictionary<Guid, int>());
            services.AddSingleton<Dictionary<Guid, Account>>(_ => new Dictionary<Guid, Account>());

            services.AddSingleton(accountBalances);
            services.AddSingleton(accounts);
            services.AddSingleton(transactions);

            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ISharedDataService, SharedDataService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configure Swagger and Swagger UI
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name");
                    // You can add other options here if needed
                });
            }
            else
            {
                // app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowLocalhost3000");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
