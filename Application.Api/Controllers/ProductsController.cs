using Application.Api.Models;
using Application.Api.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productService;

    public ProductsController(IProductsService productsService)
    {
        _productService = productsService;
    }

    [HttpPost]
    [Route("add")]
    public IActionResult AddProduct(Product product)
    {
        _productService.AddProduct(product);
        return Ok();
    }

    [HttpGet]
    [Route("all")]
    public List<Product> GetAllProducts()
    {
        return _productService.GetAllProducts();
    }

    [HttpGet]
    [Route("o")]
    public Product GetProduct(int id)
    {
        return _productService.GetProduct(id);
    }

    [HttpDelete]
    [Route("remove")]
    public IActionResult RemoveProduct(int id)
    {
        _productService.RemoveProduct(id);
        return Ok();
    }
}