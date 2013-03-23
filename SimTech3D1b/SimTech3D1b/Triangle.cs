using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimTech3D1b
{
    class Triangle : Plane
    {
        public Triangle(Model model, Vector3 position, float scale)
            : base(model, position, scale)
        {
            this.colour = new Color(97, 64, 81);
            planeCorners = new Vector3[3];
            planeCorners[2] = new Vector3(0.5f, 0, 0.5f);
            planeCorners[1] = new Vector3(0.5f, 0, -0.5f);
            planeCorners[0] = new Vector3(-0.5f, 0, -0.5f);
            updateNormal();
        }

    }
}
