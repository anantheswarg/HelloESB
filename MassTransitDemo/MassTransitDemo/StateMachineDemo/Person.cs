using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineDemo
{
    public class Person: CorrelatedBy<Guid>
    {
        public string Name { get; set; }
        public Guid CorrelationId { get; private set; }
        
        public Person(string name, Guid guid)
        {
            this.CorrelationId = guid;
            this.Name = name;
        }
    }
}
