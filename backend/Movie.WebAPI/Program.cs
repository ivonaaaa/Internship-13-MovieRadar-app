using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<>(); dodat neki repo
//builder.Services.AddScoped<>(); dodat service

//builder.Services.AddControllers();

//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

//if(app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
//app.UseAuthorization(); javlja gresku
//app.MapControllers();
app.Run();