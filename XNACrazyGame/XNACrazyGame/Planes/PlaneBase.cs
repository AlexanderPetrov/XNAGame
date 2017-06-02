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

        public Rectangle Body
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            }
        }

        private bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
        }

        public PlaneBase Destroy()
        {
            _isAlive = false;
            return this;
        }

        protected Rectangle _gameFieldRectangle;

        private static Random r = new Random();
        
        public PlaneBase(float speed, int health, Texture2D texture, Rectangle gameFieldRectangle)
        {
            _isAlive = true;
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

        private bool CheckIfPlaneInGameFieldBorders()
        {
            _isAlive = !(_position.Y + _texture.Height < 0);
            return !_isAlive;
        }
        
        public abstract void Attack();
        public abstract void Dodge();

        public virtual void Update(GameTime gameTime)
        {
            Move();
            CheckIfPlaneInGameFieldBorders();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
