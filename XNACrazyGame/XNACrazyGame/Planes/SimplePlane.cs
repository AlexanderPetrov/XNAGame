using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNACrazyGame
{
    public class SimplePlane : PlaneBase
    {
        public SimplePlane(float speed, int health, Texture2D texture, Rectangle gameFieldRectangle)
            :base(speed, health, texture, gameFieldRectangle)
        {
        }

        public override void Attack()
        {
        }

        public override void Dodge()
        {
        }
    }
}
