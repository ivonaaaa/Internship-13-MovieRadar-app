using System.Data;
using Npgsql;
using MovieRadar.Domain.Interfaces;
using MovieRadar.Infrastructure.Repositories;
using MovieRadar.Application.Services;
using MovieRadar.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

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

//if(app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization(); 
app.MapControllers();

app.Run();