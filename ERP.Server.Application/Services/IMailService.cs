using FluentEmail.Core.Models;

namespace ERP.Server.Application.Services;
public interface IMailService
{
    Task<SendResponse> SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);

}
