using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Notification.Common;
using SteelCloud.Encryption;

namespace Notification.Server
{
    public class NotificationService : NotifierRpc.NotifierRpcBase
    {
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ILogger<NotificationService> logger) => _logger = logger;
        [Authorize]
        public override Task<NotificationReply> WriteNotification(NotificationRequest p_request, ServerCallContext p_context)
        {
            var test = p_context.GetHttpContext().User;            
            
            return Task.FromResult(new NotificationReply()
            {
                Message = $"You said '{p_request.Content}'"
            });
        }
        
        [Authorize(Roles = "ADMIN")]
        public override Task<CertReply> CreateCertification(CertRequest p_request, ServerCallContext p_context)
        {
            string filepath;
            var certFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Constants.CertificateFolder);
            var cert = Utilities.GetServerCert(certFilePath, p_request.Filename,
                EncryptionEngine.StringToSecureString(Constants.grpc.Password), out filepath);
            
            return Task.FromResult(new CertReply()
            {
                Filepath = $"Cert Path: {filepath}"
            });

        }
       
    }
}