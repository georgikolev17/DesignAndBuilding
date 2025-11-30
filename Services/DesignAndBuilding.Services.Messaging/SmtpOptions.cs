namespace DesignAndBuilding.Services.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class SmtpOptions
    {
        public string Host { get; set; } = default!;

        public int Port { get; set; } = 587;

        public bool UseSsl { get; set; } = true;

        public string User { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string FromEmail { get; set; } = default!;

        public string FromName { get; set; } = default!;
    }
}
