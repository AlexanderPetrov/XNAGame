using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNACrazyGame
{
    public abstract class PlaneBase
    {
        protected float _speed;
        protected int _health;

        protected Vector2 _position;
        protected Vector2 _origin;

        protected Texture2D _texture;

        protected Rectangle _gameFieldRectangle;

        private static Random r = new Random();
        
        public PlaneBase(float speed, int health, Texture2D texture, Rectangle gameFieldRectangle)
        {
            _speed = speed;
            _health = health;

            _texture = texture;
            _gameFieldRectangle = gameFieldRectangle;

            GenerateRandomInitPosition(gameFieldRectangle);
        }

        private void GenerateRandomInitPosition(Rectangle gameFieldRectangle)
        {
            _position = new Vector2(
                r.Next(0, _gameFieldRectangle.Width - _texture.Width), 
                -_texture.Height);
        }

        public void Move()
        {
            _position.Y += _speed;
        }

        public bool IsOutsideBorders()
        {
            return _position.Y > _gameFieldRectangle.Height;
        }
        
        public abstract void Attack();
        public abstract void Dodge();

        public virtual void Update(GameTime gameTime)
        {
            Move();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
