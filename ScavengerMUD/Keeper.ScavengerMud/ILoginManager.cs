using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public interface ILoginManager
    {
        Task<LoginResult> Login(ISession session);
    }
}
