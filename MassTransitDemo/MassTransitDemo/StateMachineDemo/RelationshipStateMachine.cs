using Automatonymous;
using MassTransit;
using MassTransit.Exceptions;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineDemo
{
    public class RelationshipStateMachine :  AutomatonymousStateMachine<Relationship>
    {
        private IContainer container;
        //public static RelationshipStateMachine Instance { get; set; }
        public RelationshipStateMachine(IContainer container)
        {
            //this.Bus = serviceBus();
            this.container = container;
            Event(() => Hello);
            Event(() => GoAway);
            //Event(() => Introduce);

            State(() => Friend);
            State(() => Enemy);

            Initially(
                When(Hello)
                    .Call((saga,message) => HandlePerson(message))
                    .TransitionTo(Friend)
                
                
            );
                During(Friend, 
                    When(GoAway)
                    //.Call((saga, message) => HandlePerson1(message))
                    //.If
                    .Try(x => x.Call((saga, message) => HandlePerson1(message))
                                .TransitionTo(Enemy)
                    ,
                    x =>x.Handle<Exception>(with => with.Then( (saga, tuple) => {

                        if (exceptionRetryCount++ < 5)
                        {

                            //this.RaiseEvent(saga, PissOff);
                            //HandlePerson1(tuple.Item1);
                            //throw new Exception("test exception");
                            //throw new SagaException("test exception", typeof(Relationship), typeof(Person), saga.CorrelationId);
                        }
                        //if (tuple.Item2.GetType() == typeof(InvalidCastException))
                        //{
                        //    Console.WriteLine(tuple.Item2.Message);
                        //}
                        //else
                        //{
                        //    //this.RaiseEvent(saga, )
                        //    if (exceptionRetryCount++ < 5)
                        //    {
                        //        throw new SagaException("test exception", typeof(Relationship), typeof(Person), saga.CorrelationId);
                        //    }
                        //}
                    }))
                    //.Handle<Exception>(with => with.Then( (saga, tuple) => { 

                    //    if (tuple.Item2.GetType() == typeof(InvalidCastException))
                    //    {
                    //        Console.WriteLine(tuple.Item2.Message);
                    //    }
                    //    else
                    //    {
                    //        throw tuple.Item2;
                    //    }
                    //}))

                    //,
                    //x =>x.Handle<Exception>(with => with.Then( (saga, tuple) => { 

                    //    if (tuple.Item2.GetType() == typeof(InvalidCastException))
                    //    {
                    //        Console.WriteLine(tuple.Item2.Message);
                    //    }
                    //    else
                    //    {
                    //        throw tuple.Item2;
                    //    }
                    //}))
                ));
        }

        public IServiceBus Bus { get { return this.container.GetInstance<IServiceBus>(); } }
        
        public State Friend { get; private set; }
        public State Enemy { get; private set; }

        public Event<Person> Hello { get; private set; }
        public Event<Person1> GoAway { get; private set; }
        //public Event<Person1> Introduce { get; private set; }
        public Guid CorrelationId { get; private set; }

        private void HandlePerson(Person person)
        {
            
            if (person.Name.Equals("ananth"))
            {
                Bus.Publish<Person1>(new Person1("test", person.CorrelationId));
            }
            else
            {
                throw new Exception("not valid name");
            }
        }

        private void HandlePerson1(Person1 person)
        {
            Console.WriteLine("hello " + person.Name);
            if (person.Name.Equals("test"))
            {
                throw new IndexOutOfRangeException("not valid name");
                //throw new InvalidCastException("not valid name");
                //Console.WriteLine("hello " + person.Name);
                //Bus.Publish<Person>(new Person("test"));
            }
            else
            {
                throw new Exception("not valid name");
            }
        }

        public int exceptionRetryCount { get; set; }
    }
}
