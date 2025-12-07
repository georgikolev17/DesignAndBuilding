using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging
{
    public static class EmailBuilder
    {
        /// <summary>
        /// Builds a simple HTML notification email.
        /// </summary>
        /// <param name="messageHtml">Main message (can contain basic HTML like &lt;p&gt;, &lt;br/&gt;).</param>
        /// <param name="linkUrl">Optional link URL to the assignment.</param>
        /// <param name="linkText">Text for the link button (optional).</param>
        public static string BuildNotificationEmail(string messageHtml, string? linkUrl = null, string? linkText = null)
        {
            var linkBlock = string.Empty;

            if (!string.IsNullOrWhiteSpace(linkUrl))
            {
                linkText ??= "Отворете заданието";

                linkBlock = $@"
                <p style=""margin-top:16px;"">
                    <a href=""{linkUrl}"" 
                       style=""display:inline-block;padding:10px 16px;
                              background-color:#4e73df;color:#ffffff;
                              text-decoration:none;border-radius:4px;
                              font-size:14px;"">
                        {linkText}
                    </a>
                </p>";
            }

            return $@"
            <!DOCTYPE html>
            <html lang=""bg"">
            <head>
                <meta charset=""utf-8"" />
                <title>BuildNet Notification</title>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
            </head>
            <body style=""margin:0;padding:0;background-color:#f4f4f4;font-family:Arial, sans-serif;"">
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#f4f4f4;padding:20px 0;"">
                    <tr>
                        <td align=""center"">
                            <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""0"" 
                                   style=""background-color:#ffffff;border-radius:4px;overflow:hidden;border:1px solid #e0e0e0;"">

                                <!-- Header -->
                                <tr>
                                    <td style=""background-color:#4e73df;color:#ffffff;padding:16px 24px;font-size:18px;font-weight:bold;"">
                                        BuildNet
                                    </td>
                                </tr>

                                <!-- Content -->
                                <tr>
                                    <td style=""padding:20px 24px;color:#333333;font-size:14px;line-height:1.5;"">
                                        {messageHtml}
                                        {linkBlock}
                                    </td>
                                </tr>

                                <!-- Footer -->
                                <tr>
                                    <td style=""padding:12px 24px;font-size:12px;color:#999999;border-top:1px solid #eeeeee;"">
                                        Това съобщение е генерирано автоматично от BuildNet.<br />
                                        Моля, не отговаряйте на този имейл.
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";
        }
    }
}
