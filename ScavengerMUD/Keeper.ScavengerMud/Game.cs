using Keeper.Prolog.SwishApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public class Game
    {
        private ILogging log;
        private IServer server;
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        private ILoginManager loginManager = new LoginManager();

        public Game(IServer server, ILogging log)
        {
            this.server = server;
            this.log = log;

            server.NewSession += Server_NewSession;

            this.commands.Add("QUIT", new QuitCommand());
        }
        
        private async void Server_NewSession(ISession session)
        {
            this.log.Log("Incoming Connection: " + session.RemoteEndPoint);

            var loginResult = await this.loginManager.Login(session);

            bool isRunning = loginResult.IsSuccess;

            this.log.Log("Login " + (loginResult.IsSuccess ? "succeeded." : "failed."));

            while(isRunning)
            {
                string message = await session.ReceiveAsync();

                this.log.Log(string.Format("{0}: {1}", session.RemoteEndPoint, message));

                if (message != null)
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        var parts = message.Split(' ');

                        ICommand command;

                        if (this.commands.TryGetValue(parts[0].ToUpperInvariant(), out command))
                        {
                            await command.ExecuteAsync(session, parts.Skip(1).ToArray());
                        }
                        else
                        {
                            await session.SendAsync(string.Format("Unrecognised command: '{0}'", parts[0]));
                        }
                    }
                }
                else
                {
                    isRunning = false;
                }

                isRunning &= session.IsOpen;
            }

            this.log.Log("Session ended.");
        }

        private static string GetRoomDesc()
        {
            return "This is a temporary room description.";

            //Guid programGuid = WebApi.Create(File.ReadAllText("Tracery.plg") + string.Format("\r\n\r\nstoredGrammar(\"{0}\").", File.ReadAllText("Instance.json").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "")));

            //var responseDoc = WebApi.Send(programGuid, "generate(\"entrance\", Result)");

            //return responseDoc.Descendants("Result").First().Value;
        }
    }
}
