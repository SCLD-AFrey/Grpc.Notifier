using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using SteelCloud.Encryption;
using Notification.Common;

namespace Notification.Server
{
    public class Program
    {        
        private static readonly string CertFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.CertificateFolder);
        private static X509Certificate2 _serverCert;
        public static void Main(string[] args)
        {
            string filepathfull;
            _serverCert = Utilities.GetServerCert(CertFilePath, Constants.CertificateName,EncryptionEngine.StringToSecureString(Constants.grpc.Password), out filepathfull);
            Console.WriteLine($"CERT LOADED: {filepathfull}");
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureKestrel(kestrelServerOptions => {
                            kestrelServerOptions.ConfigureHttpsDefaults(opt =>
                            {
                                opt.ClientCertificateMode = ClientCertificateMode.RequireCertificate;

                                // Verify that client certificate was issued by same CA as server certificate
                                opt.ClientCertificateValidation = (certificate, chain, errors) =>
                                    certificate.Issuer == _serverCert.Issuer;
                            });
                        });
                });
    }
}
