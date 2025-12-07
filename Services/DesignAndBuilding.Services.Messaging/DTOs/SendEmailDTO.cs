using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging.DTOs
{
    public class SendEmailDTO
    {
        public SendEmailDTO(string to, string htmlContent, string subject, IEnumerable<EmailAttachment>? attachments = null)
        {
            this.To = to;
            this.HtmlContent = htmlContent;
            this.Subject = subject;
            this.Attachments = attachments ?? null;
        }

        public string To { get; set; }

        public string HtmlContent { get; set; }

        public string Subject { get; set; }

        public IEnumerable<EmailAttachment>? Attachments { get; set; } = null;
    }
}
