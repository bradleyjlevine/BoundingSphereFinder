using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System;
namespace BoundingSphereFinder
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<Vector3> v = new List<Vector3>();

            Console.Write("Enter obj file: ");
            string file = Console.ReadLine();
            
            try
            {
                FileStream obj = new FileStream(file, FileMode.Open);
                StreamReader r = new StreamReader(obj);
                List<string> line = new List<string>();
                int j = 0;
                while (r.Peek() > 0)
                {
                    line.Add(r.ReadLine());
                }


                char[] c = {' '};
                for(int i = 0; i < line.Count; i++)
                {
                    string[] tokens = line[i].Split(c);

                    if (tokens[0].Equals("v"))
                    {
                        v.Add(new Vector3((float)Convert.ToDouble(tokens[1]), (float)Convert.ToDouble(tokens[2]), (float)Convert.ToDouble(tokens[3])));
                    }
                }

                r.Close();
                obj.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            string name = file.Remove(file.IndexOf('.'));

            Vector3[] boundingbox = FindBoundingBox(v);

            Console.WriteLine("\nBounding Box for {0}: \nMin: {1}\nMax: {2}\n", name, boundingbox[0], boundingbox[1]);

            Vector4 boundingSphere = FindBoundingSphere(boundingbox, v);

            Console.WriteLine("Bounding Sphere for {0}: \nCenter: {1}\nRadius: {2}\n", name, new Vector3(boundingSphere.X, boundingSphere.Y, boundingSphere.Z), boundingSphere.W);
        }

        private Vector4 FindBoundingSphere(Vector3[] b, List<Vector3> v)
        {
            Vector4 r = new Vector4();

            Vector3 m = new Vector3((b[0].X + b[1].X) / 2, (b[0].Y + b[1].Y) / 2, (b[0].Z + b[1].Z) / 2);

            float d = float.MinValue;
            float t;

            for(int i = 0; i < v.Count; i++)
            {
                if ((t = Vector3.Distance(m, v[i])) > d) d = t; 
            }

            r.X = m.X;
            r.Y = m.Y;
            r.Z = m.Z;
            r.W = d;

            return r;
            
        }

        private Vector3[] FindBoundingBox(List<Vector3> v)
        {
            Vector3[] r = new Vector3[2];
            float mX = float.MinValue, mY = float.MinValue, mZ = float.MinValue;
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;

            for (int i = 0; i < v.Count; i++)
            {
                if (v[i].X > mX) mX = v[i].X;
                if (v[i].X < minX) minX = v[i].X;

                if (v[i].Y > mY) mY = v[i].Y;
                if (v[i].Y < minY) minY = v[i].Y;

                if (v[i].Z > mZ) mZ = v[i].Z;
                if (v[i].Z < minZ) minZ = v[i].Z;
            }

            r[0] = new Vector3(minX, minY, minZ);
            r[1] = new Vector3(mX, mY, mZ);

            return r;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
