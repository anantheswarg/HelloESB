using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachineDemo
{
    public class Person1 : CorrelatedBy<Guid>
    {
        public string Name { get; set; }
        public Guid CorrelationId { get; set; }
        
        public Person1(string name, Guid guid)
        {
            this.CorrelationId = guid;
            this.Name = name;
        }
    }
}
