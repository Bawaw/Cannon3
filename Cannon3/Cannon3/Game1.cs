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

namespace Cannon3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int SCREENWIDTH = 640;
        const int SCREENHEIGHT = 480;

        const int PPOJECTILESPEED = 12;
        const int BASETARGETSPEED = 8;
        const float COLLISIONDISTANCE = 20.0f;
        const int STARTINGLIVES = 3;
        const int LIVESTYLEWIDTH = 16;
        const int LIVESDISPLAY = 4;
        const int DIFFICULTY = 3;

        bool GameRun = false;
        int lives;
        int level;

        GameItem player;
        GameItem target;
        GameItem projectile;

        HUD titleScreen;
        HUD livesDisplay;

        Random random;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;

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
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            random = new Random();

            target = new GameItem();
            player = new GameItem();
            projectile = new GameItem();
           
            projectile.Velocity = new Point (0,-PPOJECTILESPEED);
            titleScreen = new HUD();
            titleScreen.Position = new Point(0, 0);
            titleScreen.Width = SCREENWIDTH;
            titleScreen.Height = SCREENHEIGHT;

            livesDisplay = new HUD();

            livesDisplay.Position = new Point(LIVESDISPLAY, LIVESDISPLAY);
            livesDisplay.Width = LIVESTYLEWIDTH * STARTINGLIVES;
            livesDisplay.Height = LIVESTYLEWIDTH;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Texture = Content.Load<Texture2D>(@"Textures\player");
            target.Texture = Content.Load<Texture2D>(@"Textures\target");
            projectile.Texture = Content.Load<Texture2D>(@"Textures\projectile");

            titleScreen.Texture = Content.Load<Texture2D>(@"Textures\title_screen");
            livesDisplay.Texture = Content.Load<Texture2D>(@"Textures\lives");
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
            if (GameRun)
            {
                updatePlayer();
                target.update();
                projectile.update();

                checkFire();
                checkGroundCollision();
                CheckAirCollision(); 
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                beginGame();
            }
        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (GameRun)
            {
                player.draw(spriteBatch);
                target.draw(spriteBatch);
                projectile.draw(spriteBatch);
                livesDisplay.draw(spriteBatch);
            }
            else
            {
                titleScreen.draw(spriteBatch);
            }
         
            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region gameplay

        private void beginGame()
        {
            GameRun = true;
            lives = STARTINGLIVES;
            level = 1;

            updateLivesDisplay();
            resetProjectile();
            resetTarget();
            updatePlayer();
        }

        private void endGame()
        {
            GameRun = false;
        }

        private void killPlayer()
        {
            resetTarget();

            lives--;
            updateLivesDisplay();

            if (lives == 0)
            {
                endGame();
            }

        }


        private void killTarget()
        {
            level++;
            resetTarget();
            resetProjectile();
        }

        private void updateLivesDisplay()
        {
            livesDisplay.Width = lives * LIVESTYLEWIDTH;
        }

        #endregion


        # region mechanics methods

        private void updatePlayer()
        {
            int playerX = Mouse.GetState().X;
            int playerY = SCREENHEIGHT - player.Origin.Y;
            player.Position = new Point(playerX, playerY);
        }

        private void checkFire()
        {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && 
            projectile.Position.Y < projectile.Origin.Y )
            {
                int projectileX = player.Position.X;
                int projectileY = SCREENHEIGHT - player.Texture.Height - projectile.Origin.Y;
                projectile.Position = new Point(projectileX, projectileY);
            }
        }
        private void resetTarget()
        { 
            int VelocityX = random.Next(2, 6);
            int VelocityY = BASETARGETSPEED + level / DIFFICULTY;
            if (random.Next(2) == 0)
            {
                VelocityX *= -1;
            }

            target.Position = new Point(SCREENWIDTH / 2, -target.Origin.Y);
            target.Velocity = new Point(VelocityX, VelocityY);
        }

        private void resetProjectile()
        {
            projectile.Position = new Point(0, -projectile.Origin.Y);
        }

        # endregion

        #region collision

        private void checkGroundCollision()
        { 
            if ((target.Position.Y) > (SCREENHEIGHT - target.Origin.Y))
            {
                killPlayer();
            }
        }

        private void CheckAirCollision()
        { 
            if (distance(projectile.Position, target.Position) < COLLISIONDISTANCE)
            {
                resetProjectile();
                killTarget();
                resetProjectile();
            }
        }

        # endregion

        #region utility methods

        private float distance (Point pointA, Point pointB)
        {
            int a = pointA.X - pointB.X,
                b = pointA.Y - pointB.Y;

            float c = (float)Math.Sqrt((a * a) + (b * b));

            return c;
        }

        # endregion
    }
}
