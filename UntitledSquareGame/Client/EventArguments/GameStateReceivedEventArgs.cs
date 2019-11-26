using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public class GameStateReceivedEventArgs : EventArgs
    {
        public GameStateReceivedEventArgs(GameState gameState)
        {
            this.GameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
        }

        public GameState GameState
        {
            get;
        }
    }
}
