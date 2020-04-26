using System;
using System.Collections.Generic;
using System.Text;

namespace MessageProcessingSimulator.Interfaces
{
    public interface IProducerFactory<T>
    {
        IEnumerable<IProducer<T>> Build(int numProducers);
    }
}
