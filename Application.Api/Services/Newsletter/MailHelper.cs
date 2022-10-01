using System.Globalization;
using System.Text;
using Application.Api.Models;
using Application.Api.Models.Orders;

namespace Application.Api.Services.Newsletter;

public static class MailHelper
{
    public static string GenerateSubject(Order order) => $"Order confirmation ({order.Id})";

    public static string GenerateBody(Order order)
    {
        var sb = new StringBuilder();

        sb.Append(@$"
                    <p> <b> Date: {DateTime.Now} </b> &nbsp; &nbsp; &nbsp; <b>Order id: {order.Id}</b> </p> 
                  ");
        
        foreach (var orderProduct in order.Products!)
        {
            sb.Append(@$" <div style=""text-align: center;""> 
                            <h3> {orderProduct.Name} </h3>
                            <p> {orderProduct.Price.ToString(CultureInfo.InvariantCulture)} Zł.</p>
                            <hr style=""width: 50%;"">
                          </div>
                     ");
        }

        sb.Append(@$"<p> <b> Total: {order.PriceSum()} Zł. </b> </p>");
        
        return sb.ToString();
    }
    
    public static string GenerateRegisterSubject()
    {
        var sb = new StringBuilder();

        sb.Append("Account creation");

        return sb.ToString();
    }
    
    public static string GenerateRegisterBody(int code)
    {
        var sb = new StringBuilder();

        sb.Append($@"
                    <div style=""text-align: center;""> 
                        <p>Your unique code: {code}</p>
                     </div>
                  ");

        return sb.ToString();
    }

    public static string GenerateAccountCreationSubject()
    {
        return "Account was Created - eShopAPI";
    }

    public static string GenerateAccountCreationBody()
    {

        return @"
                    <div style=""text-align: center;""> 
                        <p>Your account was created.</p>
                     </div>
                  ";
    }

    public static string GenerateInvoicePdfHtml(Invoice invoice)
    {
        var sb = new StringBuilder();

        sb.Append(@"<div style=""text-align: center;"">");
        sb.Append(@$"<h3>Invoice - {invoice.Number}</h3>");
        sb.Append(@"<table border=""1"" cellspadding=""0"" cellspacing=""0"">");
        
        foreach (var product in invoice.Order.Products)
        {
            sb.Append(@$"
        
                        <tr> 
                            <td> {product.Name} </td> <td> {product.Price} </td>
                        </tr> 

                        ");
        }
        
        sb.Append($@"                        
                        <tr> 
                            <td> Total Amount: </td> <td> {invoice.Total} </td>
                        </tr> 
                        
                        </table>
                        </div>
                        ");

        sb.Append($"<p> Buyer: {invoice.Vendor}");
        sb.Append($"<p> Payment Method: </td> <td> {invoice.PayingMethod.ToString()} </p>");
        sb.Append($"<p> Shipping Type: {invoice.Order.ShippingType.ToString()} </p>");
        sb.Append($"<p> Date: {invoice.DateOfIssue} </p>");

        return sb.ToString();
    }
}