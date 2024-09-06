using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProducts;
public sealed record GetAllProductsQuery(
    string Name,
    int Type) : IRequest<Result<List<Product>>>;
