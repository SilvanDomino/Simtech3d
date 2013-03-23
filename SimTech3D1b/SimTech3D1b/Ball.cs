using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimTech3D1b
{
    class Ball : BasicModel
    {
        public Vector3 velocity = Vector3.Zero;
        public Vector3 acceleration = Vector3.Zero;
        public Vector3 gravity = new Vector3(0, -0.0005f, 0);
        public float radius = 0.5f;
        private float mass = 1;
        private bool fallTime = false;

        public Ball(Model model, Vector3 position, float scale)
            :base(model, position, scale, Color.DeepSkyBlue)
        {
            
        }

        public bool CollidesWithPlane(Plane plane){
            if (CollidesWithPlaneCheck(plane) == 0)
            {
                velocity = Vector3.Reflect(velocity, plane.normal);
                acceleration += new Vector3(0, 0.001f, 0);
                return true;
            }
            return false;
        }

        public bool CollidesWithTriangle(Triangle triangle)
        {
            float result = RayTriangleTest(this.position+(triangle.normal * -this.radius), this.velocity, triangle, 1.0f);
            if (result != float.MaxValue)
            {
                velocity = Vector3.Reflect(velocity, triangle.normal);
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            
            if(fallTime)
                acceleration += gravity;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                fallTime = true;
                //this.velocity += new Vector3(0.001f, 0, 0);
            }

            velocity += acceleration;
            position += velocity;
            base.Update(gameTime);
            acceleration = Vector3.Zero;
        }

        private float RayTriangleTest(Vector3 rayOrg, Vector3 rayDelta, Triangle triangle, float minT)
        {
            // We'll return this huge number if no intersection is detected
            const float kNoIntersection = float.MaxValue;
            Console.WriteLine("org: " + rayOrg + "     delta: " + rayDelta);
            // Retrieve triange vertices
            Vector3 p0 = triangle.planeCorners[0];
            Vector3 p1 = triangle.planeCorners[0];
            Vector3 p2 = triangle.planeCorners[0];

            // Compute clockwise edge vectors.
            Vector3 e1 = p1 - p0;
            Vector3 e2 = p2 - p1;

            // Compute surface normal. (Unnormalized)
            Vector3 n = triangle.normal;

            // Compute gradient, which tells us how steep of an angle
            // we are approaching the *front* side of the triangle
            float dot = Vector3.Dot(n, rayDelta);

            // Check for a ray that is parallel to the triangle or not
            // pointing toward the front face of the triangle.
            //
            // Note that this also will reject degenerate triangles and
            // rays as well. We code this in a very particular
            // way so that NANs will bail here. (This does not
            // behave the same as "dot >= 0.0f" when NANs are involved.)
            if (!(dot < 0.0f))
            {
                
                return kNoIntersection;
            }
            //Console.WriteLine("Dot: {0}, n: {1}, delta: {2}", dot, n, rayDelta);

            // Compute d value for the plane equation. We will
            // use the plane equation with d on the right side:
            //
            // Ax + By + Cz = d
            float d = Vector3.Dot(n, p1);

            // Compute parametric point of intersection with the plane
            // containing the triangle, checking at the earliest
            // possible stages for trivial rejection
            float t = d - Vector3.Dot(n, rayOrg);

            // Is ray origin on the backside of the polygon? Again,
            // we phrase the check so that NANs will bail
            if (!(t <= 0.0f))
            {
                Console.WriteLine("check");
                return kNoIntersection;
            }
            //Console.WriteLine("t: {0}", t);

            // Closer intersection already found? (Or does
            // ray not reach the plane?)
            //
            // since dot < 0:
            //
            // t/dot > minT
            //
            // is the same as
            //
            // t < dot*minT
            //
            // (And then we invert it for NAN checking...)
            if (!(t >= dot * minT))
            {
                
                return kNoIntersection;
            }

            // OK, ray intersects the plane. Compute actual parametric
            // point of intersection
            t /= dot;
            //assert(t >= 0.0f);
            //assert(t <= minT);

            // Compute 3D point of intersection
            Vector3 p = rayOrg + rayDelta * t;
            p += rayDelta;

            // Find dominant axis to select which plane
            // to project onto, and compute u's and v's
            float u0, u1, u2;
            float v0, v1, v2;
            
            if (Math.Abs(n.X) > Math.Abs(n.Y))
            {
                if (Math.Abs(n.X) > Math.Abs(n.Z))
                {
                    u0 = p.Y - p0.Y;
                    u1 = p1.Y - p0.Y;
                    u2 = p2.Y - p0.Y;
                    v0 = p.Z - p0.Z;
                    v1 = p1.Z - p0.Z;
                    v2 = p2.Z - p0.Z;
                }
                else
                {
                    u0 = p.X - p0.X;
                    u1 = p1.X - p0.X;
                    u2 = p2.X - p0.X;
                    v0 = p.Y - p0.Y;
                    v1 = p1.Y - p0.Y;
                    v2 = p2.Y - p0.Y;
                }
            }
            else
            {
                if (Math.Abs(n.Y) > Math.Abs(n.Z))
                {
                    u0 = p.X - p0.X;
                    u1 = p1.X - p0.X;
                    u2 = p2.X - p0.X;
                    v0 = p.Z - p0.Z;
                    v1 = p1.Z - p0.Z;
                    v2 = p2.Z - p0.Z;
                }
                else
                {
                    u0 = p.X - p0.X;
                    u1 = p1.X - p0.X;
                    u2 = p2.X - p0.X;
                    v0 = p.Y - p0.Y;
                    v1 = p1.Y - p0.Y;
                    v2 = p2.Y - p0.Y;
                }
            }

            // Compute denominator, check for invalid
            float temp = u1 * v2 - v1 * u2;
            if (!(temp != 0.0f))
            {
                return kNoIntersection;
            }
            temp = 1.0f / temp;

            // Compute barycentric coords, checking for out-of-range
            // at each step
            float alpha = (u0 * v2 - v0 * u2) * temp;
            if (!(alpha >= 0.0f))
            {
                return kNoIntersection;
            }

            float beta = (u1 * v0 - v1 * u0) * temp;
            if (!(beta >= 0.0f))
            {
                return kNoIntersection;
            }

            float gamma = 1.0f - alpha - beta;
            if (!(gamma >= 0.0f))
            {
                return kNoIntersection;
            }

            // Return parametric point of intersection
            Console.WriteLine("COLLISION");
            return t;
        }



        public int CollidesWithPlaneCheck(Plane plane)
        {
            Vector3 planeNormal = plane.normal;
            float d = Vector3.Dot(planeNormal, this.position) - this.radius;
            //is on front
            if (d >= this.radius)
            {
                return +1;
            }
            //is on back of plane
            if (d <= -this.radius)
            {
                return -1;
            }

            //intersects plane
            return 0;
        }
    }
}
