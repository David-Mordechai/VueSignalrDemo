using System.Reflection;
using Backend;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policyBuilder => policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapHub<PayloadHub>("/ws");

app.MapGet("api/test", () => "test api");

var uiAssembly = Assembly.GetCallingAssembly();
var uiProvider = new ManifestEmbeddedFileProvider(uiAssembly, "AppUi");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = uiProvider,
    ContentTypeProvider = new FileExtensionContentTypeProvider()
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    if(path.StartsWithSegments("/ws") || path.StartsWithSegments("/api"))
    {
        await next();
        return;
    }
    
    var index = uiProvider.GetFileInfo("index.html");
    if (index.Exists)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync(index);
    }
});

app.Run("http://*:40001/");