using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Lesson_8___Collisions_with_Rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;

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

        Texture2D coinTexture;
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

            barrierRect1 = new Rectangle(0, 250, 350, 75);
            barrierRect2 = new Rectangle(450, 250, 350, 75);

            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 300, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 300, coinTexture.Width, coinTexture.Height));

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
                pacLocation.Y = _graphics.PreferredBackBufferHeight - 75;
            }

            if (pacLocation.Right >= _graphics.PreferredBackBufferWidth)
            {
                pacLocation.X = _graphics.PreferredBackBufferWidth - 75;
            }

            for (int i = 0; i < coins.Count; i++)
            {
                if (pacLocation.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            
            _spriteBatch.Draw(barrierTexture, barrierRect1, Color.White);
            _spriteBatch.Draw(barrierTexture, barrierRect2, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(pacTexture, pacLocation, Color.White);
            _spriteBatch.Draw(coinTexture, coinRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
