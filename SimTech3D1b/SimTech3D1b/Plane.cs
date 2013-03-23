using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimTech3D1b
{
    class Plane : BasicModel
    {
        public Vector3 normal;
        public Vector3[] planeCorners;
        public Plane(Model model, Vector3 position, float scale)
            : base(model, position, scale, Color.Honeydew)
        {
            planeCorners = new Vector3[4];
            planeCorners[0] = new Vector3(- 0.5f, 0, - 0.5f);
            planeCorners[1] = new Vector3(0.5f, 0, - 0.5f);
            planeCorners[2] = new Vector3(0.5f, 0,  0.5f);
            planeCorners[3] = new Vector3(- 0.5f, 0, 0.5f);
            updateNormal();
        }

        public void updateNormal()
        {
            for (int i = 0; i < planeCorners.Length; i++)
            {
                planeCorners[i] = Vector3.Transform(planeCorners[i], GetWorld());
            }
            Vector3 cross = Vector3.Cross(planeCorners[1] - planeCorners[0], planeCorners[1] - planeCorners[2]);
            cross.Normalize();
            this.normal = cross;
        }
    }
}
