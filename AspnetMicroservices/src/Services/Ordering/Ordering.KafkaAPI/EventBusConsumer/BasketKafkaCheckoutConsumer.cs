using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.KafkaAPI.Extensions;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.KafkaAPI.EventBusConsumer
{
    public class BasketKafkaCheckoutConsumer : IHostedService
    {
        private readonly string topic = "test";
        private readonly string groupId = "test_group";
        private readonly string bootstrapServers = "localhost:29092";

        //private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BasketKafkaCheckoutConsumer> _logger;

        public BasketKafkaCheckoutConsumer(    ILogger<BasketKafkaCheckoutConsumer> logger, IServiceScopeFactory serviceScopeFactory)
        {
            //_mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();
                    //HandlyMessage handlyMessage = new HandlyMessage();
                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            var orderCommand = JsonSerializer.Deserialize<CheckoutOrderCommand>(consumer.Message.Value);
                            //var result = await _mediator.Send(orderCommand);
                            using var scope = _serviceScopeFactory.CreateScope();
                            var mediator = scope.ServiceProvider.GetService<IMediator>();
                            mediator.Send(orderCommand, cancellationToken);
                            _logger.LogInformation($"BasketCheckoutEvent consumed successfully: {consumer.Message}");

                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
