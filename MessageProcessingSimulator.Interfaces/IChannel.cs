using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingSimulator.Interfaces
{
    public interface IChannel<T>
    {
        Task PostAsync(T message);
        Task<T> ReadAsync();
        Task<bool> WaitToReadAsync();
    }
}
