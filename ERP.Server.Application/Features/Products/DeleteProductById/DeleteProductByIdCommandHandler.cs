using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.DeleteProductById;

internal sealed class DeleteProductByIdCommandHandler(
    IProductCommandRepository productCommandRepository,
    IProductQueryRepository productQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService) : IRequestHandler<DeleteProductByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        Product? product = await productQueryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result<string>.Failure("Product not found");
        }
        productCommandRepository.Delete(product);

        #region Outbox
        await outboxService.AddAsync(TableNames.Product, OperationNames.Delete, request.Id, cancellationToken);
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Delete is successful";
    }
}
