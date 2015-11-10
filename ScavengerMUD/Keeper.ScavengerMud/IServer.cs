using System;

namespace Keeper.ScavengerMud
{
    public interface IServer
    {
        event Action<ISession> NewSession;

        void Start();
        void Stop();
    }
}