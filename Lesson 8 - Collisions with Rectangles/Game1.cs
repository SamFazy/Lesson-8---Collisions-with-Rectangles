using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Lesson_8___Collisions_with_Rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Random generator = new Random();

        int randomYValue;
        int randomXValue;

        KeyboardState keyboardState;
        MouseState mouseState;

        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacSleepTexture;

        Texture2D pacTexture;
        Rectangle pacLocation;

        Texture2D exitTexture;
        Rectangle exitRect;

        Texture2D barrierTexture;
        Rectangle barrierRect1, barrierRect2;
        List<Rectangle> barriers;

        Texture2D coinTexture;
        Rectangle coinRect;
        List<Rectangle> coins; 

        int pacSpeed;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            pacSpeed = 3;
            pacLocation = new Rectangle(10, 10, 60, 60);

            //barrierRect1 = new Rectangle(0, 250, 350, 75);
            //barrierRect2 = new Rectangle(450, 250, 350, 75);
            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));

            coins = new List<Rectangle>();
            randomXValue = generator.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width);
            randomYValue = generator.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height);
            coins.Add(new Rectangle(randomXValue, randomYValue, coinTexture.Width, coinTexture.Height));
            randomXValue = generator.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width);
            randomYValue = generator.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height);
            coins.Add(new Rectangle(randomXValue, randomYValue, coinTexture.Width, coinTexture.Height));
            randomXValue = generator.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width);
            randomYValue = generator.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height);
            coins.Add(new Rectangle(randomXValue, randomYValue, coinTexture.Width, coinTexture.Height));
            randomXValue = generator.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width);
            randomYValue = generator.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height);
            coins.Add(new Rectangle(randomXValue, randomYValue, coinTexture.Width, coinTexture.Height));

            exitRect = new Rectangle(700, 350, 100, 100);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Pacman
            pacDownTexture = Content.Load<Texture2D>("PacDown");
            pacUpTexture = Content.Load<Texture2D>("PacUp");
            pacLeftTexture = Content.Load<Texture2D>("PacLeft");
            pacRightTexture = Content.Load<Texture2D>("PacRight");
            pacSleepTexture = Content.Load<Texture2D>("PacSleep");
            pacTexture = pacSleepTexture;

            //Barrier
            barrierTexture = Content.Load<Texture2D>("rock_barrier");

            //Exit
            exitTexture = Content.Load<Texture2D>("hobbit_door");

            //Coin
            coinTexture = Content.Load<Texture2D>("coin");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            if (mouseState.LeftButton == ButtonState.Pressed)
                if (exitRect.Contains(mouseState.X, mouseState.Y))
                    Exit();

            if (exitRect.Contains(pacLocation) && coins.Count == 0 && mouseState.LeftButton == ButtonState.Pressed)
                Exit();

            foreach (Rectangle barrier in barriers)
                if (pacLocation.Intersects(barrier))
                {

                    pacLocation = new Rectangle(10, 10, 60, 60);
                }
                


            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacTexture = pacUpTexture;
                pacLocation.Y -= pacSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacTexture = pacDownTexture;
                pacLocation.Y += pacSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacTexture = pacLeftTexture;
                pacLocation.X -= pacSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacTexture = pacRightTexture;
                pacLocation.X += pacSpeed;
            }
            if (!keyboardState.IsKeyDown(Keys.Up) && !keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Down))
            {
                pacTexture = pacSleepTexture;
            }

            if (pacLocation.Left < 0)
            {
                pacLocation.X = 0;
            }
            if (pacLocation.Top < 0)
            {
                pacLocation.Y = 0;
            }

            if (pacLocation.Bottom >= _graphics.PreferredBackBufferHeight)
            {
                pacLocation.Y = _graphics.PreferredBackBufferHeight - 60;
            }

            if (pacLocation.Right >= _graphics.PreferredBackBufferWidth)
            {
                pacLocation.X = _graphics.PreferredBackBufferWidth - 60;
            }

            for (int i = 0; i < coins.Count; i++)
            {

                if (pacLocation.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < coins.Count; i++)
            {
                foreach (Rectangle barrier in barriers) 
                    if (barrier.Intersects(coins[i]) || exitRect.Intersects(coins[i]))
                    {

                        coins.RemoveAt(i);
                        randomXValue = generator.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width);
                        randomYValue = generator.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height);
                        coins.Add(new Rectangle(randomXValue, randomYValue, coinTexture.Width, coinTexture.Height));

                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();


            foreach (Rectangle barrier in barriers)
            {
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
            }

            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(pacTexture, pacLocation, Color.White);
            
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
