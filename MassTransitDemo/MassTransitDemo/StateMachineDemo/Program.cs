using log4net.Config;
using MassTransit;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Log4NetIntegration;
using Newtonsoft.Json;
using MassTransit.Saga;
using Automatonymous;

namespace StateMachineDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            IServiceBus servicesBus = null;

            var container = new Container(x =>
            {
                x.ForConcreteType<MyMessageConsumer>();
                x.ForConcreteType<MyMessageConsumer1>();
                //x.For<IServiceBus>().Singleton().Use(() => servicesBus);
            });

            servicesBus = ServiceBusFactory.New(sbc =>
            //Bus.Initialize(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.UseBsonSerializer();
                sbc.ReceiveFrom("rabbitmq://localhost/ananth_test6");
                sbc.UseLog4Net();
                
                sbc.Subscribe(subs =>
                {
                    //subs.Handler<MyMessage>(msg => Console.WriteLine(msg.Text));
                    subs.Handler<IFault>((context, fault) => 
                    {
                        Console.WriteLine("Correlation Id", context.CorrelationId);
                        //context.CorrelationId = msg.Messages[0].ToString();
                        Console.WriteLine(JsonConvert.SerializeObject(fault));
                    });
                    //subs.Consumer<MyMessageConsumer>();
                    var machine = new RelationshipStateMachine(container);

                    subs.StateMachineSaga(machine, new InMemorySagaRepository<Relationship>(), x =>
                    {

                        x.Correlate(machine.Hello, (saga, msg) => saga.CorrelationId == msg.CorrelationId);

                    });
                     // subs.StateMachineSaga(new RelationshipStateMachine(), new SagaRepository());
                    //subs.Saga<Relationship>(new InMemorySagaRepository<Relationship>()).Permanent();
                });
                //});
                //sbc.Subscribe(x => x.LoadFrom(container));
                
            });

            container.Inject(servicesBus);
            //container.Inject<IServiceBus>(servicesBus);

            //var relationship = new Relationship();
            //var machine = new RelationshipStateMachine();

            //machine.RaiseEvent(relationship, machine.Hello);
            using (IServiceBus servicesBus1 = container.GetInstance<IServiceBus>())
            {
                Guid guid = Guid.NewGuid();
                //Bus.Instance.Publish(new MyMessage { Text = "Hi bro! Message sent" });
                servicesBus1.Publish(new Person("ananth", guid));
                //servicesBus1.Publish(new MyMessage { Text = "Hi bro1! Message sent1" });
                Console.ReadKey();
            }
        }
    }
}
