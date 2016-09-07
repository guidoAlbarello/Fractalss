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
using System.Runtime.InteropServices;


namespace Fractals
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDevice gDevice;
        public static GraphicsDeviceManager gManager;
        public static int width = 1200, height = 800;
        public static SpriteBatch spriteBatch;

        public static MouseState mState, oldMState;
        public static KeyboardState kState, oldKState;

        public static Texture2D renderTex;
        public static bool isJulia = false;
        public static Effect fractal;
        public static Vector2 lastPosition;

        private SoundEffect sound;
        private SoundEffectInstance soundInstance;
        private double timePast;
        private double oldTimePast;
        public Game1()
        {
            gManager = new GraphicsDeviceManager(this);
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
            gDevice = gManager.GraphicsDevice;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gManager.PreferredBackBufferWidth = width;
            gManager.PreferredBackBufferHeight = height;
            gManager.ApplyChanges();
            this.IsMouseVisible = true;

            mState = Mouse.GetState();
            oldMState = mState;
            int asd;
            renderTex = new Texture2D(gDevice, width, height);
            Color[] cA = new Color[width * height];
            for (int i = 0; i < width * height; i++)
            {
                cA[i] = Color.White;
            }
            renderTex.SetData<Color>(cA);
            this.Window.Title = "FractalWithCamera";
            fractal = Content.Load<Effect>("fractal");
            fractal.CurrentTechnique = fractal.Techniques["julia"];
            fractal.Parameters["Viewport"].SetValue(new Vector2(gDevice.Viewport.Width, gDevice.Viewport.Height));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mState = Mouse.GetState();

        

            if (mState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
            {
                lastPosition = new Vector2(mState.X, mState.Y);
            }
            if (mState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Pressed)
            {
                Vector2 dif = lastPosition - new Vector2(mState.X, mState.Y);
                lastPosition = new Vector2(mState.X, mState.Y);
                fractal.Parameters["Pan"].SetValue(fractal.Parameters["Pan"].GetValueVector2() - (dif / 1000) * fractal.Parameters["zoom"].GetValueVector2().X);
            }

            if (mState.RightButton == ButtonState.Pressed)
            {
                fractal.Parameters["juliaSeed"].SetValue(((new Vector2(mState.X, mState.Y) / new Vector2(width, height)) * new Vector2(2, 2) - new Vector2(1, 1)) * fractal.Parameters["zoom"].GetValueVector2() / 3 - fractal.Parameters["Pan"].GetValueVector2());
            }          
            
            if (mState.ScrollWheelValue != oldMState.ScrollWheelValue)
            {
                if (mState.ScrollWheelValue - oldMState.ScrollWheelValue > 0)
                {
                    fractal.Parameters["zoom"].SetValue(fractal.Parameters["zoom"].GetValueVector2() * 0.9f);
                }
                else
                {
                    fractal.Parameters["zoom"].SetValue(fractal.Parameters["zoom"].GetValueVector2() * 1.1f);
                }
            }
            oldMState = mState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gDevice.Clear(Color.Black);
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, fractal);
            spriteBatch.Draw(renderTex, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
