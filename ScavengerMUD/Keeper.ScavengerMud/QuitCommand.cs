using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public class QuitCommand
        : ICommand
    {
        public async Task ExecuteAsync(ISession session, string[] parts)
        {
            await Task.Run(() => session.Close());
        }
    }
}
