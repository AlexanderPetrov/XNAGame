using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNACrazyGame
{
    public class Cannon
    {
        private const int RELOAD_TIME_IN_MILISECONDS = 250;

        Vector2 _position;
        int _speed = 7;

        int _elapsedMilisecondsSinceLastReload;
        
        public Vector2 PositionOrigin
        {
            get
            {
                return new Vector2(_position.X + _texture.Width / 2, _position.Y);
            }
        }

        Texture2D _texture;

        Rectangle _gameFieldRectangle;
        private RocketFactory _rocketFactory;

        public Cannon(Texture2D texture, Rectangle gameFieldRectangle, RocketFactory rocketFactory)
        {
            _texture = texture;
            _gameFieldRectangle = gameFieldRectangle;
            _rocketFactory = rocketFactory;

            _position = new Vector2(_gameFieldRectangle.Width / 2 - _texture.Width / 2, _gameFieldRectangle.Height - texture.Height);
            _elapsedMilisecondsSinceLastReload = 0;
        }

        public void Move(int direction)
        {
            _position.X += _speed * direction;
        }

        public Rocket Fire()
        {
            return _rocketFactory.CreateRocket(PositionOrigin);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            _elapsedMilisecondsSinceLastReload += gameTime.ElapsedGameTime.Milliseconds;

            if (kbState.IsKeyDown(Keys.Right) && _position.X + _texture.Width < _gameFieldRectangle.Width)
                Move(1);
            if (kbState.IsKeyDown(Keys.Left) && _position.X > 0)
                Move(-1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        internal bool Reloaded()
        {
            if (_elapsedMilisecondsSinceLastReload >= RELOAD_TIME_IN_MILISECONDS)
            {
                _elapsedMilisecondsSinceLastReload = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
