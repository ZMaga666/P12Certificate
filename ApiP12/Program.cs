using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApiP12.Services;

var builder = WebApplication.CreateBuilder(args);

// Sertifikayı yükle
var certPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Certificates:Path"]);
var certPassword = builder.Configuration["Certificates:Password"];
var clientCertificate = new X509Certificate2(certPath, certPassword);

// HttpClient servisini sertifika ile ekleyelim
builder.Services.AddHttpClient("P12ApiClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(clientCertificate);
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    });

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
