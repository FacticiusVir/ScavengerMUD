using System;
using System.Net;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public interface ISession
    {
        EndPoint RemoteEndPoint { get; }

        bool IsOpen { get; }

        void Close();

        Task SendAsync(string message);

        Task<string> ReceiveAsync();
    }
}