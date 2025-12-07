using DesignAndBuilding.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public interface IEmailsService
    {
        Task SendNewAssignmentNotificationEmailAsync(Assignment assignment);
    }
}
