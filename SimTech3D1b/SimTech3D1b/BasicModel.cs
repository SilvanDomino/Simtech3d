using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimTech3D1b
{
    class BasicModel
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;

        public Matrix rotationMatrix = Matrix.Identity;
        public Matrix translationMatrix = Matrix.Identity;
        private Matrix scaleMatrix = Matrix.Identity;
        protected Color colour;
        public Vector3 position { get; protected set; }

        public BasicModel(Model m, Vector3 position, float scale, Color colour)
        {
            model = m;
            this.position = position;
            scaleMatrix = Matrix.CreateScale(scale);
            translationMatrix = Matrix.CreateTranslation(position);
            this.colour = colour;
        }

        public virtual void Update(GameTime gameTime)
        {
            //to make sure the object is always drawn on the same spot the position is
            translationMatrix = Matrix.CreateTranslation(position);     
        }

        public void Draw(Camera3D camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.SpecularColor = colour.ToVector3() * 0.5f;   //darker shadows
                    be.EmissiveColor = colour.ToVector3() * 0.1f;     //superlight
                    be.AmbientLightColor = colour.ToVector3() * 0.8f; 
                    be.DiffuseColor = colour.ToVector3() *0.5f ;
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }

        //creates a matrix which translates the model to the right position with the proper scale and rotation.
        public virtual Matrix GetWorld()  
        {
            return world * rotationMatrix * scaleMatrix * translationMatrix;
        }

        public virtual Matrix LocalMatrix()
        {
            Matrix w = Matrix.Identity;
            return w * rotationMatrix * scaleMatrix;
        }
    }
}
