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

namespace MassTransitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(x =>
            {
                x.ForConcreteType<MyMessageConsumer>(); 
                x.ForConcreteType<MyMessageConsumer1>();
                //x.Scan(scanner => {
                //    scanner.AddAllTypesOf<IConsumer>();
                                  
                //    });
            });
            XmlConfigurator.Configure();
            IServiceBus servicesBus = ServiceBusFactory.New(sbc =>
            //Bus.Initialize(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.UseBsonSerializer();
                sbc.ReceiveFrom("rabbitmq://localhost/ananth_test5");
                sbc.UseLog4Net();

                //sbc.Subscribe(subs =>
                //{
                //    //subs.Handler<MyMessage>(msg => Console.WriteLine(msg.Text));
                //    subs.Handler<IFault>(msg => Console.WriteLine(JsonConvert.SerializeObject(msg)));
                //    //subs.Consumer<MyMessageConsumer>();

                //});
                //});
                sbc.Subscribe(x => x.LoadFrom(container));
                
            });
           
            container.Inject(servicesBus);

            //using (IServiceBus servicesBus = ServiceBusFactory.New(sbc =>
            //Bus.Initialize(sbc =>
            //{
            //    sbc.UseRabbitMq();
            //    sbc.UseJsonSerializer();
            //    sbc.ReceiveFrom("rabbitmq://localhost/ananth_test3");
            //    sbc.Subscribe(subs =>
            //    {
            //        //subs.Handler<MyMessage>(msg => Console.WriteLine(msg.Text));
            //        //subs.Handler<MyMessage1>(msg => Console.WriteLine(msg.Text));
            //        subs.Consumer<MyMessageConsumer>();
            //    });
            //    //});
            //}))
            using (IServiceBus servicesBus1 = container.GetInstance<IServiceBus>())
            {

                //Bus.Instance.Publish(new MyMessage { Text = "Hi bro! Message sent" });
                servicesBus1.Publish(new MyMessage { Text = "Message sent" });
                //servicesBus1.Publish(new MyMessage { Text = "Hi bro1! Message sent1" });

                //
                Console.ReadKey();
            }
            //
            //servicesBus.Dispose();
        }
    }
}

