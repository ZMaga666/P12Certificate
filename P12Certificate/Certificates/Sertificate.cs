////using System.Security.Cryptography;
////using System.Security.Cryptography.X509Certificates;

////namespace P12Certificate.Certificates
////{
////    public class Sertificate
////    {
////        static void Main()
////        {
////            using (RSA rsa = RSA.Create(2048))
////            {


////                var request = new CertificateRequest(
////                    "CN=MyTestCert",
////                    rsa,
////                    HashAlgorithmName.SHA256,
////                    RSASignaturePadding.Pkcs1);using System.Net.Security;
//using Microsoft.AspNetCore.Server.Kestrel.Https;
//using System.Security.Cryptography.X509Certificates;

//public void ConfigureServices(IServiceCollection services)
//{
//    services.Configure<HttpsConnectionAdapterOptions>(options =>
//    {
//        options.OnAuthenticate = (context, sslOptions) =>
//        {
//            // İstemci sertifikasının gerekli olduğunu belirtiriz
//            sslOptions.ClientCertificateRequired = true;

//            // Sertifika doğrulama işlemi
//            sslOptions.ClientCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
//            {
//                // Sertifika geçerli değilse hata ver
//                if (certificate == null || sslPolicyErrors != SslPolicyErrors.None)
//                {
//                    Console.WriteLine("❌ İstemci sertifikası geçersiz.");
//                    return false; // Sertifika geçersiz
//                }

//                // Sertifika geçerliyse başarılı olduğunu bildir
//                Console.WriteLine("✅ İstemci sertifikası geçerli.");
//                return true; // Sertifika geçerli
//            };
//        };
//    });
//}


////                var certificate = request.CreateSelfSigned(
////                    DateTimeOffset.UtcNow,
////                    DateTimeOffset.UtcNow.AddYears(1));

////                byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, "MySecurePassword");
////                File.WriteAllBytes("mycert.p12", pfxBytes);

////                Console.WriteLine("P12 Certificate created:mycert.p12");







////            }








////        }

////    }
////}
