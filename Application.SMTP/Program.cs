using Application.SMTP;
using Application.SMTP.Dtos;
using Application.SMTP.Enums;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMailBuilder, MailBuilder>();

var app = builder.Build();

app.MapGet("/{type}", ([FromServices] IMailBuilder mailBuilder, [FromRoute] MailType type, [FromBody] RequestDto request) => mailBuilder.Build(type, request));

app.Run();