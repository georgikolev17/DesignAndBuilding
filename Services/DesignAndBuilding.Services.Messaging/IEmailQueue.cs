using DesignAndBuilding.Services.Messaging.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging
{
    public interface IEmailQueue
    {
        Task EnqueueAsync(SendEmailDTO email, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SendEmailDTO> DequeueAllAsync(CancellationToken cancellationToken = default);
    }
}
