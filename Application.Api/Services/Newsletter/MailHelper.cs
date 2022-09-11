using System.Globalization;
using System.Text;
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
            sb.Append(@$"
                        <h3> {orderProduct.Name} </h3>
                        <p> {orderProduct.Price.ToString(CultureInfo.InvariantCulture)} Zł.</p>
                        <hr>
                     ");
        }

        sb.Append(@$"<p> <b> Total: {order.PriceSum()} Zł. </b> </p>");
        
        return sb.ToString();
    }
}