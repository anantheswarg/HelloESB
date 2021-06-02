using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachineDemo
{
    public class MyMessageConsumer : Consumes<MyMessage>.Context
    {
        public void Consume(IConsumeContext<MyMessage> message)
        {
            Console.WriteLine("Recieved: " + message.Message.Text);
            if (message.Message.Text.Equals("Message sent"))
            {
                throw new InvalidOperationException("test exception");
            }
            
            //Console.WriteLine("Recieved: " + message.Message.Text);
        }
    }
}
