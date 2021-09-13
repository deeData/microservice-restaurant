using Mango.Services.Email.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.Email.Repository
{
    interface IEmailRepository
    {
        Task SendAndLogEmail(UpdatePaymentResultMessage message);

    }
}
