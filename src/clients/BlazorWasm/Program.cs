using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");


            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44389") });

            builder.Services.AddOidcAuthentication(options =>
            {   // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                //options.UserOptions.RoleClaim = "admin";
                options.UserOptions.RoleClaim = "role";
                options.UserOptions.NameClaim = "role";
            });

            builder.Services.AddHttpClient("BlazorApp1.ServerAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44389");
            })
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorApp1.ServerAPI"));
            //builder.Services.AddApiAuthorization();

            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "Cookies";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = "https://localhost:5001";

            //    options.ClientId = "mvc";
            //    options.ClientSecret = "secret";
            //    options.ResponseType = "code";

            //    options.Scope.Add("api1");

            //    options.SaveTokens = true;
            //});


            await builder.Build().RunAsync();
        }
    }
}
