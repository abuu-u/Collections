using System.Text.Json;
using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Helpers;
using Collections.Api.Repositories;
using Collections.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    const string fileName = "topics.json";
    await using var openStream = File.OpenRead(fileName);
    var topics = await JsonSerializer.DeserializeAsync<List<Topic>>(openStream) ??
                 throw new Exception("Create topics.json file");
    topics.ForEach(t =>
    {
        if(dataContext.Topics.Contains( new Topic {Id = t.Id}))
        {
            dataContext.Topics.Update(t);
        } else
        {
            dataContext.Topics.Add(t);
        }
    });
    await dataContext.SaveChangesAsync();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
