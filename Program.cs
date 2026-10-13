using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Winiffer_P1_P4.Service;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<AutorService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var autorService = scope.ServiceProvider.GetRequiredService<AutorService>();
    await autorService.InicializeAsync();
}


app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
