using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChatApp.Api.Extensions;
using ChatApp.Application.Services.AnonymousUsersManagement;
using ChatApp.Application.Services.Chat;
using ChatApp.Application.Services.WebsocketSender;
using ChatApp.Domain.Repositories;
using ChatApp.Infrastructure.Hubs;
using ChatApp.Infrastructure.Repos;
using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ChatApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000")
                        .AllowCredentials();
                });
            });
            services.AddControllers();
            services.AddSignalR();
            
            services.AddApplication();
            services.AddInfrastructure();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseCors("ClientPermission");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });
        }
    }
}
