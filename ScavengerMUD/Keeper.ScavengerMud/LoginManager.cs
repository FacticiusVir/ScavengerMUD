using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public class LoginManager
        : ILoginManager
    {
        public async Task<LoginResult> Login(ISession session)
        {
            string username = null;
            bool isUsernameValid = false;

            while (!isUsernameValid)
            {
                await session.SendAsync("Please enter your username:");

                username = await session.ReceiveAsync();

                if (username == null)
                {
                    return LoginResult.Fail;
                }

                isUsernameValid = !string.IsNullOrWhiteSpace(username);

                if(!isUsernameValid)
                {
                    await session.SendAsync("Invalid username.");
                }
            }
            
            return new LoginResult(true, true, username);
        }
    }
}
