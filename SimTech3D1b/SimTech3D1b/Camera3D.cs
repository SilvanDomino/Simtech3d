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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SimTech3D1b
{
    public class Camera3D : Microsoft.Xna.Framework.GameComponent
    {
        //Camera matrices
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;

        MouseState prevMouseState;
        float speed = 1;

        public Camera3D(Game game, Vector3 pos, Vector3 dir)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = dir;
            cameraDirection.Normalize();
            cameraUp = Vector3.Up;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 1000);
        }
        public override void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            // Move forward/backward
            if (Keyboard.GetState( ).IsKeyDown(Keys.W))
                cameraPosition += cameraDirection * speed;
            if (Keyboard.GetState( ).IsKeyDown(Keys.S))
                cameraPosition -= cameraDirection * speed;
            // Move side to side
            if (Keyboard.GetState( ).IsKeyDown(Keys.A))
                cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            if (Keyboard.GetState( ).IsKeyDown(Keys.D))
                cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;

            
            // Yaw rotation
            cameraDirection = Vector3.Transform(
                cameraDirection, 
                Matrix.CreateFromAxisAngle(
                    cameraUp, 
                    (-MathHelper.PiOver4 / 300) * (Mouse.GetState( ).X - prevMouseState.X)
                )
            );
            // Pitch rotation
            cameraDirection = Vector3.Transform(
                cameraDirection,
                Matrix.CreateFromAxisAngle(
                    Vector3.Cross(cameraUp, cameraDirection),
                    (MathHelper.PiOver4 / 120) *
                    (Mouse.GetState( ).Y - prevMouseState.Y)
                )
            );


            // Reset prevMouseState
            prevMouseState = Mouse.GetState( );

            base.Update(gameTime);
            // Recreate the camera view matrix
            CreateLookAt();
        }
        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }
        

    }
}
