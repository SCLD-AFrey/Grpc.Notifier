using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Notification.Common;
using SteelCloud.Encryption;

namespace Notification.Server
{
    public class NotificationService : NotifierRpc.NotifierRpcBase
    {
        public override Task<NotificationReply> WriteNotification(NotificationRequest request, ServerCallContext context)
        {
            return Task.FromResult(new NotificationReply()
            {
                Message = $"You said: {request.Content}"
            });
        }

        public override Task<CertReply> CreateCertification(CertRequest request, ServerCallContext context)
        {
            string filepath;
            var certFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "certData");
            var cert = Utilities.GetServerCert(certFilePath, request.Filename,
                EncryptionEngine.StringToSecureString("P@ssword"), out filepath);
            
            return Task.FromResult(new CertReply()
            {
                Filepath = $"Cert Path: {filepath}"
            });

        }
        
    }
}