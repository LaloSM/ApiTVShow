using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using Tvshow.Api.Middlewares;
using Tvshow.Application;
using Tvshow.Application.Features.Tvshow.Queries.GetTvShowList;
using Tvshow.Domain;
using Tvshow.Infrastructure;
using Tvshow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Agrega la cadena de conexión que se crea en appsettings
builder.Services.AddDbContext<TvshowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"),
    b => b.MigrationsAssembly(typeof(TvshowDbContext).Assembly.FullName))
);

builder.Services.AddMediatR(typeof(GetTvShowListQuery).Assembly);

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
}).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configurar otros servicios como Identity, JWT, CORS, Swagger, etc.

//Este es la logica para trabajar con el identity 
IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<Usuario>();//la clase en la que vamos a estar trabajando es en el del usuario
identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

//Soporte de roles
identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();

//Agrega los claims del usuario y del identityRole
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();

//Estas soon las reglas que se van a insertar en el modelo del usuario
identityBuilder.AddEntityFrameworkStores<TvshowDbContext>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();


//Declaramos la configuracion de nuestros tokens
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));//este codigo va a leer lo que esta en el appsettings en el JwSettings:Key y va a obtener esa informacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});


//configuracion de los cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );

});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();//Se agrega la autenticacion
app.UseAuthorization();
app.UseCors("CorsPolicy");//agregada el nombre del cors que hemos declarado en la parte de arriba

app.MapControllers();

// Resto del código para configurar el pipeline de solicitud HTTP

// Archivo de migración y carga de datos iniciales
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<TvshowDbContext>();
        var usuarioManager = service.GetRequiredService<UserManager<Usuario>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();
        await TvshowDbContextData.LoadDataAsync(context, usuarioManager, roleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error en la migración");
    }
}

app.Run();