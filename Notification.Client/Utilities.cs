using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Grpc.Core;
using Grpc.Net.Client;

namespace Notification.Client
{
    public class Utilities
    {
        internal async static Task<GrpcChannel> CreateAuthenticatedChannel(string p_address, string p_token, X509Certificate2 p_cert)
        {
            if (p_token == null)
            {
                p_token = await Utilities.Authenticate(p_address, p_cert);
            }

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(p_token))
                {
                    metadata.Add("Authorization", $"Bearer {p_token}");
                }
                return Task.CompletedTask;
            });
            
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(p_cert);
            var httpClient = new HttpClient(handler);

            var channel = GrpcChannel.ForAddress($"https://{p_address}", new GrpcChannelOptions
            {
                HttpClient = httpClient,
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
        
        internal static async Task<string> Authenticate(string p_address, X509Certificate2 p_cert)
        {
            Console.WriteLine($"Authenticating as {Environment.UserName}...");

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(p_cert);
            var httpClient = new HttpClient(handler);
            
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://{p_address}/getToken?name={HttpUtility.UrlEncode(Environment.UserName)}"),
                Method = HttpMethod.Get,
                Version = new Version(2, 0)
            };
            var tokenResponse = await httpClient.SendAsync(request);
            tokenResponse.EnsureSuccessStatusCode();

            var token = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Successfully authenticated.");

            return token;
        }
    }
}