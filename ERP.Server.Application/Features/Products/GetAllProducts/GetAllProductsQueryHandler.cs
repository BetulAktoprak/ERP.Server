using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProducts;

internal sealed class GetAllProductsQueryHandler(IProductQueryRepository productQueryRepository) : IRequestHandler<GetAllProductsQuery, Result<List<Product>>>
{
    public async Task<Result<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productQueryRepository.GetAllAsync(cancellationToken);

        return products.OrderBy(p => p.Name).ToList();
    }
}
