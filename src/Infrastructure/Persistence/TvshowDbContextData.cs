
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Models.Authorization;
using Tvshow.Domain;

namespace Tvshow.Infrastructure.Persistence
{
    public class TvshowDbContextData
    {
        public static async Task LoadDataAsync(
            TvshowDbContext context,
            UserManager<Usuario> usuarioManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory
        )
        {
            try
            {
                // Verifica si no hay roles creados en la base de datos
                if (!roleManager.Roles.Any())
                {
                    // Crea los roles de administrador y usuario si no existen
                    await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));//Role.ADMIN, hace referencia a la capa application/Models/Authorization/Role.cs
                    await roleManager.CreateAsync(new IdentityRole(Role.USER));
                }

                // Verifica si no hay usuarios creados en la base de datos
                if (!usuarioManager.Users.Any())
                {
                    // Crea un usuario administrador
                    var usuarioAdmin = new Usuario
                    {
                        Nombre = "Lalo",
                        Apellido = "S",
                        Email = "laloshow@gmail.com",
                        UserName = "lalo.s",
                        Telefono = "985644644",
                    };
                    await usuarioManager.CreateAsync(usuarioAdmin, "PasswordLaloShow123$");
                    await usuarioManager.AddToRoleAsync(usuarioAdmin, Role.ADMIN);

                    // Crea un usuario normal
                    var usuario = new Usuario
                    {
                        Nombre = "Juan",
                        Apellido = "Perez",
                        Email = "juan.perez@gmail.com",
                        UserName = "juan.perez",
                        Telefono = "98563434534",
                    };
                    await usuarioManager.CreateAsync(usuario, "PasswordJuanPerez123$");
                    await usuarioManager.AddToRoleAsync(usuario, Role.USER);

                }

                // Verifica si no hay registros en la tabla TvShows de la base de datos
                if (!context.TvShows!.Any())
                {
                    // Lee los datos desde un archivo JSON local
                    var tvshowData = File.ReadAllText("../Infrastructure/Data/tvshow.json");
                    // Deserializa los datos JSON en una lista de objetos TvShow
                    var tvshow = JsonConvert.DeserializeObject<List<TvShow>>(tvshowData);
                    // Agrega los TvShow deserializados a la base de datos y guarda los cambios
                    await context.TvShows!.AddRangeAsync(tvshow!);
                    await context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                // Si ocurre un error durante la carga de datos, registra el error utilizando el logger
                var logger = loggerFactory.CreateLogger<TvshowDbContextData>();
                logger.LogError(e.Message);
            }

        }

    }

}
