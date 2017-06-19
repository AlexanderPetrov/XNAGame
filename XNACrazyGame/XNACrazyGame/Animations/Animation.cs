using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNACrazyGame.Animations
{
    public abstract class Animation
    {
        protected Texture2D _texture;
        protected float _frameTime;
        protected int _currentFrame;
        protected int _frameCount;
        protected Vector2 _position;
        protected float _elapsedTime;
        protected bool _isPlaying;

        public bool IsPlaying { get { return _isPlaying; } }

        public Animation(Texture2D texture, int frameCount, float frameTime, Vector2 position)
        {
            _texture = texture;
            _frameCount = frameCount;
            _frameTime = frameTime;
            _position = position;
            _currentFrame = 0;
            _elapsedTime = 0.0f;
            _isPlaying = true;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
        
    }
}
