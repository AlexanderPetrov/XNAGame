using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNACrazyGame.Animations
{
    public class ExplosionAnimation : Animation
    {
        int _frameHeight = 64;

        public ExplosionAnimation(Texture2D texture, int frameCount, float frameTime, Vector2 position)
            :base(texture, frameCount, frameTime, position)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, 
                new Rectangle(_currentFrame * _frameHeight, _texture.Height - _frameHeight, _frameHeight, _frameHeight), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (_elapsedTime < _frameTime)
                _elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            else
            { 
                _currentFrame++;
                _elapsedTime = _elapsedTime - _frameTime;
            }

            if (_currentFrame == _frameCount)
            {
                _isPlaying = false;
            }
        }
    }
}
