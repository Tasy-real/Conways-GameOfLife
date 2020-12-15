using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Conways_GameOfLife
{
    class Input : GameComponent
    {
        public Input(Game game) : base(game)
        {

        }
        KeyboardState oldState;

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            SpaceTrigger = false;

            if(currentState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                SpaceTrigger = true;
            }

            oldState = currentState;
        }

        public bool SpaceTrigger { get; private set; }
    }
}
