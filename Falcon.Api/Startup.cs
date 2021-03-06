﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Api.HostedServices;
using Falcon.Api.Utils;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Api.Module;
using Falcon.Messengers.Telegram.Module;
using Falcon.Profiles;
using Falcon.Services.RequestManagement;
using Falcon.Utils.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Falcon.Api
{
    public class Startup
    {
        private const string ApiName = "FalconApi";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddSwagger(ApiName);

            return AutofacContainerBuilder
                .Create()
                .Register(builder =>
                {
                    builder.Populate(services);
                    builder.RegisterModule<ApiLoggerModule>();
                    builder.RegisterModule<TelegramModule>();
                    builder.RegisterModule<BusModule>();
                    builder.RegisterType<RequestManagementService>().As<IRequestManagementService>();
                    builder.RegisterType<TelegramHostedService>().As<IHostedService>();
                    builder.RegisterType<ApiContext>().As<IApiContext>().As<IContext>().OnActivating(args =>
                    {
                        args.Instance.ClientName = "noname";
                        args.Instance.SessionId = Guid.NewGuid();
                    }).InstancePerLifetimeScope();
                    builder.RegisterType<MessengerContext>().As<IContext>().As<IMessengerContext>().OnActivating(args =>
                    {
                        args.Instance.ClientName = "noname";
                        args.Instance.SessionId = Guid.NewGuid();
                        args.Instance.ChatId = default;
                    }).InstancePerLifetimeScope();
                })
                .MakeProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomForwardedHeaders();
            app.UseSwagger(ApiName);
            app.UseApiLoggingMiddleware();

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