using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    OutboxService outboxService,
    IProductCommandRepository productCommandRepository,
    IUnitOfWork unitOfWork,
    IProductQueryRepository productQueryRepository) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await productQueryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result<string>.Failure("Product not found");
        }

        if (request.Name != product.Name)
        {
            bool isNameExist = await productQueryRepository.IsNameExistsAsync(request.Name, cancellationToken);
            if (isNameExist)
            {
                return Result<string>.Failure("Product name already exist");
            }
        }

        product.Name = request.Name;
        product.Type = ProductTypeEnum.FromValue(request.TypeValue);
        productCommandRepository.Update(product);

        #region Outbox

        await outboxService.AddAsync(TableNames.Product, OperationNames.Create, product.Id, cancellationToken);

        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Update is successful";
    }
}
