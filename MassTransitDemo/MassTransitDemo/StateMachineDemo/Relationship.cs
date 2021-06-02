using Automatonymous;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineDemo
{
    public class Relationship : SagaStateMachineInstance
    {
        public State CurrentState { get; set; }
        public string Name { get; set; }
        public Guid CorrelationId  {get; set; }

        public IServiceBus Bus { get; set; }

        public Relationship(Guid guid)
        {
            this.CorrelationId = guid;
        }

    }
}
