using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SimTech3D1b
{
    public class MainGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        List<BasicModel> models = new List<BasicModel>();       
        Ball ball;
        Plane plane;
        Triangle triangle;
        public MainGame(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ball.Update(gameTime);
            //ball.CollidesWithPlane(plane);
            ball.CollidesWithTriangle(triangle);
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ball = new Ball(Game.Content.Load<Model>(@"Models/Ball LowDef"), new Vector3(0,5,0),1);
            plane = new Plane(Game.Content.Load<Model>(@"Models/Plane LowDef"), new Vector3(0, 0, 0),20);
            triangle = new Triangle(Game.Content.Load<Model>(@"Models/Triangle"), new Vector3(-7, 1, -8), 40);
            
            plane.rotationMatrix = Matrix.CreateRotationX(0.1f);
            plane.updateNormal();
            models.Add(ball);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            ball.Draw(((Game1)Game).camera);
            //plane.Draw(((Game1)Game).camera);
            triangle.Draw(((Game1)Game).camera);
            base.Draw(gameTime);
        }
    }
}
