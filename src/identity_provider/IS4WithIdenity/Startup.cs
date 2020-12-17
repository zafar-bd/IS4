// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IS4WithIdenity.Data.Identity;
using IS4WithIdenity.Models;
using IS4WithIdenity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Reflection;

namespace IS4WithIdenity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();// (options => { options.RootDirectory = "/Areas/Identity/Pages"; });
            var authIdentityConnectionString = Configuration.GetConnectionString("AuthIdentity");
            var authIDPConnectionString = Configuration.GetConnectionString("AuthIDP");
           // SeedIdentityData.EnsureSeedData(authIdentityConnectionString);
           // SeedIDPData.EnsureSeedData(authIDPConnectionString);
            services.AddTransient<IProfileService, ProfileService>();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<AppIdentityDbContext>(options =>
                {
                    options.LogTo(tsql => Debug.Write(tsql));
                    options.UseInMemoryDatabase("AuthIdentity");
                    //options.UseSqlServer(authIdentityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

                    //,
                    //         b =>
                    //         {
                    //             b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //});
                });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                 .AddInMemoryIdentityResources(Config.IdentityResources)
                 .AddInMemoryApiScopes(Config.ApiScopes)
                 .AddInMemoryApiResources(Config.Apis)
                 .AddInMemoryClients(Config.Clients)
                 //.AddConfigurationStore(options =>
                 //{
                 //    options.ConfigureDbContext = db =>
                 //    {
                 //        //db.LogTo(tsql => Debug.Write(tsql));
                 //        db.UseSqlServer(authIDPConnectionString,
                 //        b =>
                 //        {
                 //            // b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                 //            b.MigrationsAssembly(migrationsAssembly);
                 //        });
                 //    };
                 //})
                 //// this adds the operational data from DB (codes, tokens, consents)
                 //.AddOperationalStore(options =>
                 //{
                 //    options.ConfigureDbContext = db =>
                 //    {
                 //        //db.LogTo(tsql => Debug.Write(tsql));
                 //        db.UseSqlServer(authIDPConnectionString,
                 //                b =>
                 //            {
                 //                //b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                 //                b.MigrationsAssembly(migrationsAssembly);
                 //            });
                 //    };
                 //    options.EnableTokenCleanup = true;
                 //})
                 .AddAspNetIdentity<ApplicationUser>();

            // not recommended for production - you need to store your key material somewhere secure
            services.AddLocalApiAuthentication();
           // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           //.AddJwtBearer(options =>
           //{
           //    options.Authority = "https://localhost:5001";
           //    options.TokenValidationParameters = new TokenValidationParameters
           //    {
           //        ValidateAudience = false,
           //        ValidateIssuer = false
           //    };
           //});
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}