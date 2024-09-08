using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProducts;
public sealed record GetAllProductsQuery() : IRequest<Result<List<Product>>>;
