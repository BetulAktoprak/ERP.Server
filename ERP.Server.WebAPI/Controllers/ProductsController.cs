using ERP.Server.Application.Features.Products.CreateProduct;
using ERP.Server.Application.Features.Products.DeleteProductById;
using ERP.Server.Application.Features.Products.GetAllProducts;
using ERP.Server.Application.Features.Products.UpdateProduct;
using ERP.Server.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Controllers;
public class ProductsController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        GetAllProductsQuery request = new();
        var response = await mediator.Send(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        DeleteProductByIdCommand request = new(id);
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
