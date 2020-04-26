using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingSimulator.Interfaces
{
    public interface IConsumer<T>
    {
        Task ConsumeAsync(T message);
    }
}
