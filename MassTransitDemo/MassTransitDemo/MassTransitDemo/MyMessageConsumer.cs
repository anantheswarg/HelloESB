using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitDemo
{
    public class MyMessageConsumer : Consumes<MyMessage>.All
    {
        private IServiceBus bus;

        public MyMessageConsumer(IServiceBus bus)
        {
            this.bus = bus;
        }

        public void Consume(IConsumeContext<MyMessage> message)
        {
            bus.Publish(new MyMessage1() { Text = " test 1" });
            //Console.WriteLine("Recieved: " + message.Message.Text);
            //if (message.Message.Text.Equals("Message sent"))
            //{
            //    throw new InvalidOperationException("test exception");
            //}
            //Console.WriteLine("Recieved: " + message.Message.Text);
        }

        public void Consume(MyMessage message)
        {
            bus.Publish(new MyMessage1() { Text = " test 1" });
            
        }

        public Guid CorrelationId
        {
            get { throw new NotImplementedException(); }
        }

        public bool Accept(MyMessage message)
        {
            throw new NotImplementedException();
        }
    }

    
}
