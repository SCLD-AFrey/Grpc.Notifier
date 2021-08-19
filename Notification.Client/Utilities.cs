using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Grpc.Core;
using Grpc.Net.Client;

namespace Notification.Client
{
    public class Utilities
    {
        internal async static Task<GrpcChannel> CreateAuthenticatedChannel(string p_address, string p_token)
        {
            if (p_token == null)
            {
                p_token = await Utilities.Authenticate(p_address);
            }

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(p_token))
                {
                    metadata.Add("Authorization", $"Bearer {p_token}");
                }
                return Task.CompletedTask;
            });
            
            var channel = GrpcChannel.ForAddress(p_address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
        
        internal static async Task<string> Authenticate(string p_address)
        {
            Console.WriteLine($"Authenticating as {Environment.UserName}...");
            var httpClient = new HttpClient();
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