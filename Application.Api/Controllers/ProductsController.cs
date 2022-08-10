using Application.Api.Authorization;
using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productService;
    private readonly ApplicationContext _context;

    public ProductsController(IProductsService productsService, ApplicationContext context)
    {
        _productService = productsService;
        _context = context;
    }

    [HttpPost]
    [Route("add")]
    public IActionResult AddProduct(Product product, Guid userId)
    {
        // TODO: Make this as attribute
        var user = _context.Users.First(u => u.Id == userId);

        if (user.Roles.Any(role => role == ERoles.User))
        {
            return Unauthorized();
        }
        
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
    public IActionResult RemoveProduct(int id, Guid userId)
    {
        // TODO: Make this as attribute
        var user = _context.Users.First(u => u.Id == userId);

        if (user.Roles.Any(role => role == ERoles.User))
        {
            return Unauthorized();
        }
        
        _productService.RemoveProduct(id);
        return Ok();
    }

    [HttpPut]
    [Route("update")]
    public IActionResult ChangeProduct(Guid userId, Product product)
    {
        var user = _context.Users.First(u => u.Id == userId);

        if (user.Roles.Any(role => role == ERoles.User))
        {
            return Unauthorized();
        }
        
        var (success, content) = _productService.UpdateProduct(product);

        if (success)
        {
            return Ok(content);
        }

        return BadRequest(content);
    }

    [HttpPatch]
    [Route("rate")]
    public IActionResult RateProduct(int productId, float value)
    {
        var (success, content) = _productService.RateProduct(productId, value);

        if (!success) return BadRequest(content);

        return Ok(content);
    }
}