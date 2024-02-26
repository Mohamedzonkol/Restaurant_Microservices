using Resturant.MessagesBus;
using Resturant.PaymentProcsser;
using Resturant.Sercices.PaymentApi.Extension;
using Resturant.Sercices.PaymentApi.Messaging;
using Resturant.Sercices.PaymentApi.RabbitMqSender;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProcessorpayment, Processorpayment>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus, AzureServiceBus>();
builder.Services.AddSingleton<IRabbitMqPaymentSender,RabbitMqPaymentSender>();
builder.Services.AddHostedService<RabbitMqPaymentConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseAzureServiceBusConsumer();
app.Run();
