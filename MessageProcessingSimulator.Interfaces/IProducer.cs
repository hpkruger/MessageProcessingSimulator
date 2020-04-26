using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingSimulator.Interfaces
{
    public interface IProducer<T>
    {
        Task<T> ProduceAsync();
    }
}
