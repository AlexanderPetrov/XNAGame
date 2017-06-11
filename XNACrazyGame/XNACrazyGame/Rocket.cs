using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNACrazyGame
{
    public class Rocket
    {
        Texture2D _texture;
        Vector2 _position;

        public Vector2 Position { get { return _position; } }
        public Rectangle Body {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            }
        }

        int _speed = 6;
        int _damage;

        Rectangle _gameFieldRectangle;

        public Rocket(Vector2 position, int demage, Texture2D texture, Rectangle gameFieldRectangle)
        {
            _position = position;
            _damage = demage;
            _texture = texture;
            _gameFieldRectangle = gameFieldRectangle;
        }

        public void Move()
        {
            _position.Y -= _speed;
        }

        private bool CheckIfRocketInGameFieldBorders()
        {
            _isAlive = !(_position.Y < 0);
            return _isAlive;
        }

        private bool _isAlive;
        public bool IsAlive 
        {
            get { return _isAlive; } 
        }

        public Rocket Destroy()
        {
            _isAlive = false;
            return this;
        }

        public void Update()
        {
            Move();
            CheckIfRocketInGameFieldBorders();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
