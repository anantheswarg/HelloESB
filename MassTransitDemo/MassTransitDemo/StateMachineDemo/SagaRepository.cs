using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineDemo
{
    public class SagaRepository : ISagaRepository<Relationship>
    {
        public IEnumerable<Guid> Find(ISagaFilter<Relationship> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Action<MassTransit.IConsumeContext<TMessage>>> GetSaga<TMessage>(MassTransit.IConsumeContext<TMessage> context, Guid sagaId, MassTransit.Pipeline.InstanceHandlerSelector<Relationship, TMessage> selector, ISagaPolicy<Relationship, TMessage> policy) where TMessage : class
        {
            //Relationship saga = new 
            throw new NotImplementedException();
        }

        public IEnumerable<TResult> Select<TResult>(Func<Relationship, TResult> transformer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TResult> Where<TResult>(ISagaFilter<Relationship> filter, Func<Relationship, TResult> transformer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Relationship> Where(ISagaFilter<Relationship> filter)
        {
            throw new NotImplementedException();
        }
    }
}
