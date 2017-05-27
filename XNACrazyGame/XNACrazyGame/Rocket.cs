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

        int _speed = 6;
        int _demage;

        Rectangle _gameFieldRectangle;

        public Rocket(Vector2 position, int demage, Texture2D texture, Rectangle gameFieldRectangle)
        {
            _position = position;
            _demage = demage;
            _texture = texture;
            _gameFieldRectangle = gameFieldRectangle;
        }

        public void Move()
        {
            _position.Y -= _speed;
        }

        public bool IsOutOfBorders()
        {
            return false;
        }

        public void Update()
        {
            Move();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
