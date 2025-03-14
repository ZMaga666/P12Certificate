using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Security;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Sertifika dosya yolu ve şifresini al
        var certPath = Path.Combine(Directory.GetCurrentDirectory(), "mycert.p12");
        var certPassword = builder.Configuration["Certificates:Password"] ;

        // Sunucu sertifikasını yükle
        X509Certificate2 serverCertificate = new X509Certificate2(certPath, certPassword);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = serverCertificate;
                httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate; // Change to 'AllowCertificate'
                httpsOptions.ClientCertificateValidation = (certificate, chain, sslPolicyErrors) =>
                {
                    if (certificate == null || sslPolicyErrors != SslPolicyErrors.None)
                    {
                        Console.WriteLine("❌ Client certificate is invalid.");
                        return false;
                    }

                    Console.WriteLine("✅ Client certificate accepted.");
                    return true;
                };
            });
        });
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        var app = builder.Build();

        app.UseHttpsRedirection();

        // Middleware: İstemciden gelen sertifikayı doğrulama
        app.Use(async (context, next) =>
        {
            var clientCert = context.Connection.ClientCertificate;
            if (clientCert == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("❌ Erişim Reddedildi: Sertifika Yok.");
                return;
            }

            Console.WriteLine($"✅ Gelen İstemci Sertifikası: {clientCert.Subject}");
            await next();
        });

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();
        app.MapControllers();
        await app.RunAsync();
    }
}
