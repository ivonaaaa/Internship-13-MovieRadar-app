using System.Data;
using Npgsql;
using MovieRadar.Domain.Interfaces;
using MovieRadar.Infrastructure.Repositories;
using MovieRadar.Application.Services;
using MovieRadar.Application.Interfaces;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserCommandHandler).Assembly));


//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieRadar API", Version = "v1" });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:5500", "http://127.0.0.1:5500"); /*.AllowAnyOrigin(); */ //ili .WithOrigins("http://localhost:5000"); ili koji je vec na FE port
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

//builder.Services.AddAuthentication(); //za jwt dodat .AddJwtBearer();
//builder.Services.AddAuthorization();

//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseCors("CorsPolicy");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();