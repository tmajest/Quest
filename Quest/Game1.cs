using System;
using System.Collections.Generic;

using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Characters;
using Quest.Levels;
using Quest.Physics;

namespace Quest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;

        private Color backgroundColor = new Color(100, 100, 150);
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Hero hero;
        private Level level;
        private Camera camera;
        private Bug bug;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera(spriteBatch, 0, 0, ScreenWidth, ScreenHeight);

            this.level = new Level(
                path: Path.Combine("Levels", "Files", "level1.txt"),
                textureMap: new Dictionary<char, Texture2D>
                {
                    {'G', Content.Load<Texture2D>("Tiles\\grass3") },
                    {'D', Content.Load<Texture2D>("Tiles\\dirt3") },
                    {'T', Content.Load<Texture2D>("Tiles\\tree") },
                },
                level: 1);

            var physicsEngine = new PhysicsEngine(level);


            hero = Hero.Build(
                content: Content,
                position: new Vector2(300, 0),
                direction: Direction.Right,
                physicsEngine: physicsEngine);

            bug = Bug.Build(Content, new Vector2(1000, 400), Direction.Right, physicsEngine);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            hero.Update(gameTime, level);
            bug.Update(gameTime, level);
            camera.Update(hero.Velocity, hero.Rectangle);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            level.Draw(camera);
            hero.Draw(camera);
            bug.Draw(camera);

            base.Draw(gameTime);
        }
    }
}
