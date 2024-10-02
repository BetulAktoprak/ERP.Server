using ERP.Server.Application.Services;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Mails.SendExampleMail;

internal sealed class SendExampleMailCommandHandler(
    IMailService mailService) : IRequestHandler<SendExampleMailCommand, Result<string>>
{
    public Task<Result<string>> Handle(SendExampleMailCommand request, CancellationToken cancellationToken)
    {
        string to = "betulaktoprak@hotmail.com"
    }
}
