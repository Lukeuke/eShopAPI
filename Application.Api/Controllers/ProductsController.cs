using Application.Api.Models;
using Application.Api.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    // TODO: Make this Authorized only
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
    [Route("get")]
    public Product GetProduct(int id)
    {
        return _productService.GetProduct(id);
    }
    
    [HttpGet]
    [Route("searchByName")]
    public List<Product> GetProductsByName(string name)
    {
        return _productService.GetProductsByName(name);
    }

    [HttpDelete]
    [Route("remove")]
    public IActionResult RemoveProduct(int id)
    {
        _productService.RemoveProduct(id);
        return Ok();
    }
}