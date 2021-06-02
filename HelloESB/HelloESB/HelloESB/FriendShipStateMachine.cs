using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using StructureMap;
using MassTransit;

namespace HelloESB
{
    public class FriendShipStateMachine : AutomatonymousStateMachine<FriendShipSaga>
    {
        //states
        public State Friend { get; private set; }
        public State Enemy { get; private set; }

        //events
        public Event<HelloFriend> BecomeFriend { get; private set; }
        public Event<HelloEnemy> BecomeEnemy { get; private set; }

        private IContainer container;
        public IServiceBus Bus { get { return this.container.GetInstance<IServiceBus>(); } }
        public Guid CorrelationId { get; private set; }

        public FriendShipStateMachine(IContainer container)
        {
            this.container = container;
            Event(() => BecomeFriend);
            Event(() => BecomeEnemy);

            State(() => Friend);
            State(() => Enemy);

            Initially(
                When(BecomeFriend)
                    .Call((saga, message) => GiveChocolates(message))
                    .TransitionTo(Friend)
            );

            During(Friend,
                   When(BecomeEnemy)
                   //.Call((saga, message) => GivePunch(message))
                   //.TransitionTo(Enemy)
                   //);
                    .Try(x => x.Call((saga, message) => GivePunch(message))
                                .TransitionTo(Enemy)
                    ,
                    x =>x.Handle<InvalidOperationException>(with => with.Then( (saga, tuple) => {
                        Console.WriteLine(tuple.Item2.Message);
                    }))
            ));


        }

        private void GivePunch(HelloEnemy message)
        {
            if(message.Name.Equals("Mary"))
            {
                throw new InvalidOperationException("Mary cannot be an enemy");
            }

            Console.WriteLine("I just got punched by " + message.Name);
        }

        private void GiveChocolates(HelloFriend message)
        {
            Console.WriteLine("I got chocolates from " + message.Name);

            Bus.Publish<HelloEnemy>(new HelloEnemy("Mary", message.CorrelationId));

        }        
    }
}
