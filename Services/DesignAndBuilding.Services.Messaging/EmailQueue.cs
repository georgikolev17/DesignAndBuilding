using DesignAndBuilding.Services.Messaging.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging
{
    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<SendEmailDTO> _channel;

        public EmailQueue()
        {
            // Unbounded queue for now (we can make it bounded later if needed)
            _channel = Channel.CreateUnbounded<SendEmailDTO>();
        }

        public Task EnqueueAsync(SendEmailDTO email, CancellationToken cancellationToken = default)
        {
            return _channel.Writer.WriteAsync(email, cancellationToken).AsTask();
        }

        public IAsyncEnumerable<SendEmailDTO> DequeueAllAsync(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
