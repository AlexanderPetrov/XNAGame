using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNACrazyGame
{
    public class RocketFactory
    {
        Texture2D _rocketTexture;
        private Rectangle _gameFieldRectangle;


        private const int _lowDemage = 2;


        public RocketFactory(Texture2D rocketTexture, Rectangle gameFieldRectangle)
        {
            _rocketTexture = rocketTexture;
            _gameFieldRectangle = gameFieldRectangle;
        }

        public Rocket CreateRocket(Vector2 position)
        {
            return new Rocket(new Vector2(position.X - _rocketTexture.Width / 2, position.Y), _lowDemage, _rocketTexture, _gameFieldRectangle);
        }
    }
}
