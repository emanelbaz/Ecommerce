using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Interfaces
{
    public interface IMessageBroker
    {
        Task PublishAsync(string topic, object message);
    }
}
