using BookStoreCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore
{
    public static class ConfigureAuth
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetSection("JWT:Issuer").Value,
                        ValidateAudience = true,
                        ValidAudience = configuration.GetSection("JWT:Audience").Value,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration.GetSection("JWT:Key").Value)),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.Configure<AuthSettings>(configuration.GetSection("JWT"));

            services.AddScoped<IAuthService, AuthService>();

        }
    }
}
