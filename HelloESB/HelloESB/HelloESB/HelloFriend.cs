using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloESB
{
    public class HelloFriend : CorrelatedBy<Guid>
    {
        public string Name { get; set; }
        public Guid CorrelationId
        {
            get;
            private set;
        }

        public HelloFriend(string name, Guid guid)
        {
            this.CorrelationId = guid;
            this.Name = name;
        }
    }
}
