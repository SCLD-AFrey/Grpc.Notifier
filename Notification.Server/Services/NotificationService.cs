using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Common;

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
    }
}