using CryptoApp.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerExtensions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.AddWebApplication();

app.Run();
