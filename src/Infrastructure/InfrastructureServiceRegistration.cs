using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Identity;
using Tvshow.Application.Contracts.Infrastructure;
using Tvshow.Application.Models.Email;
using Tvshow.Application.Models.Token;
using Tvshow.Application.Persistence;
using Tvshow.Infrastructure.MessageImplementation;
using Tvshow.Infrastructure.Repositories;
using Tvshow.Infrastructure.Services.Auth;

namespace Tvshow.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                                    IConfiguration configuration
        )
        {
            // Registra una instancia de UnitOfWork como servicio scoped.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registra tipos genéricos IAsyncRepository<T> y RepositoryBase<T> como servicios scoped.
            // Esto permite el acceso a métodos comunes para interactuar con la base de datos.
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            // Registra una instancia de EmailService como servicio transient.
            // Esto permite enviar correos electrónicos desde cualquier parte de la aplicación.
            services.AddTransient<IEmailService, EmailService>();

            // Registra una instancia de AuthService como servicio transient.
            // Esto proporciona funcionalidades de autenticación y autorización en la aplicación.
            services.AddTransient<IAuthService, AuthService>();

            // Configura JwtSettings utilizando la sección "JwtSettings" del archivo de configuración (appsettings.json, por ejemplo).
            // Esto permite acceder a las propiedades de JwtSettings en cualquier parte del código que inyecte la configuración.
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Configura EmailSettings utilizando la sección "EmailSettings" del archivo de configuración.
            // Esto permite acceder a las propiedades de EmailSettings en cualquier parte del código que inyecte la configuración.
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Devuelve la colección de servicios IServiceCollection actualizada con los servicios configurados.
            return services;
        }

    }
}
