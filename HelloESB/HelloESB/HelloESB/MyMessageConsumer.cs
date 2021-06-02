using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloESB
{
    public class MyMessageConsumer : Consumes<MyMessage3>.Context
    {
        public void Consume(IConsumeContext<MyMessage3> message)
        {
            Console.WriteLine(message.Message.Text);

            //throw new InvalidOperationException("invalid op");
        }
    }
}
