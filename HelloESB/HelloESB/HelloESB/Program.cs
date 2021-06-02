using log4net.Config;
using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Log4NetIntegration;
using StructureMap;
using MassTransit.Saga;
using Automatonymous;

namespace HelloESB
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var container = new Container(x =>
            {
                x.ForConcreteType<MyMessageConsumer>();
            });

            IServiceBus servicesBus = ServiceBusFactory.New(sbc =>
            //Bus.Initialize(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.UseJsonSerializer();
                sbc.ReceiveFrom("rabbitmq://localhost/ananth_test9");
                sbc.UseLog4Net();
                sbc.SetDefaultRetryLimit(2);

                sbc.Subscribe(subs =>
                {
                    var machine = new FriendShipStateMachine(container);

                    //subs.StateMachineSaga(machine, new InMemorySagaRepository<FriendShipSaga>());
                    subs.StateMachineSaga(machine, new InMemorySagaRepository<FriendShipSaga>(), x =>
                    {

                        x.Correlate<HelloFriend>(machine.BecomeFriend, (saga, msg) => saga.CorrelationId == msg.CorrelationId);
                        x.Correlate<HelloEnemy>(machine.BecomeEnemy, (saga, msg) => saga.CorrelationId == msg.CorrelationId);

                    });
                });

                //sbc.Subscribe(subs =>
                //{
                //    //subs.Handler<MyMessage3>(msg => Console.WriteLine(msg.Text));
                //    subs.Handler<IFault>(msg => Console.WriteLine(JsonConvert.SerializeObject(msg)));
                //    //subs.Consumer<MyMessageConsumer>();

                //});


                //});

                //sbc.Subscribe(x => x.LoadFrom(container));
            });
            container.Inject(servicesBus);
            var message = new MyMessage3();
            message.Text = "Message sent";

            using (IServiceBus servicesBus1 = container.GetInstance<IServiceBus>())
            {
                Guid guid = Guid.NewGuid();
                
                //Bus.Instance.Publish(new MyMessage { Text = "Hi bro! Message sent" });
                //servicesBus1.Publish(new MyMessage3 { Text = "Message sent" });
                servicesBus1.Publish(new HelloFriend("John", guid));
                Console.ReadKey();

            }

            //Bus.Instance.Publish(message);

            //Console.ReadKey();

            //Bus.Instance.Dispose();
        }
    }
}
