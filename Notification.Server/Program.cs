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

namespace Notification.Server
{
    public class Program
    {        
        private static string m_certFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "certData");
        private static string m_certFileName = "scld.crt";
        private static X509Certificate2 m_cert;
        public static void Main(string[] args)
        {
            m_cert = Utilities.GetServerCert(m_certFilePath, m_certFileName,EncryptionEngine.StringToSecureString("P@ssword"));
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
                                    certificate.Issuer == m_cert.Issuer;
                            });
                        });
                });
    }
}
