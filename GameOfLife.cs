namespace GameOfLife {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class GameOfLife : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D whitePixel;
        Board board;
        int pixelSize = 4;

        public GameOfLife() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            board = new Board();
        }

        protected override void Initialize() {
            base.Initialize();
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;
            graphics.ApplyChanges();
            
            board.Initialize(pixelSize, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / pixelSize,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / pixelSize);
        }

        protected override void LoadContent() {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });
            board.LoadContent(whitePixel);
        }

        protected override void UnloadContent() {
            base.UnloadContent();
            spriteBatch.Dispose();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            whitePixel.Dispose();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Instance.Update();
            board.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            board.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }
    }
}
