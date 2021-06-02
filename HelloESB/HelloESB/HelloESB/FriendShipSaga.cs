using Automatonymous;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloESB
{
    public class FriendShipSaga : SagaStateMachineInstance
    {
        public State CurrentState { get; set; }
        public Guid CorrelationId  {get; set; }
        public IServiceBus Bus { get; set; }

        public FriendShipSaga(Guid guid)
        {
            this.CorrelationId = guid;
        }
    }
}
