using ERP.Server.Application.Services;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace ERP.Server.Infrastructure.Services;
internal sealed class MailService(IFluentEmail fluentEmail) : IMailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        SendResponse sendResponse =
            await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync(cancellationToken);

        return sendResponse.Successful;
    }
}
