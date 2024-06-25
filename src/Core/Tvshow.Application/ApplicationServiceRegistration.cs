using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Behaviors;
using Tvshow.Application.Mappings;

namespace Tvshow.Application
{
    public static class ApplicationServiceRegistration
    {
        // Método de extensión para IServiceCollection que configura servicios de aplicación
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración del AutoMapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());  // Agrega un perfil de mapeo personalizado
            });

            IMapper mapper = mapperConfig.CreateMapper();  // Crea un objeto IMapper basado en la configuración
            services.AddSingleton(mapper);  // Registra el objeto IMapper como un servicio singleton en IServiceCollection

            // Configuración de comportamientos de pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));  // Registra UnhandledExceptionBehavior como comportamiento de pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));  // Registra ValidationBehavior como comportamiento de pipeline

            return services;  // Devuelve IServiceCollection para permitir la encadenación de métodos
        }
    }

}
