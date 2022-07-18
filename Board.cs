namespace GameOfLife {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    class Board {
        int pixelSize = 8;
        int width, height;
        Texture2D whitePixel;
        bool[,] map, oldMap;
        double timeSinceLastUpdate = 0;
        bool pause = false;

        public Board() { }

        public void Initialize(int pixelSize, int width, int height) {
            this.pixelSize = pixelSize;
            this.width = width;
            this.height = height;
            map = new bool[width, height];
            oldMap = new bool[width, height];

            InitializeBoard(width, height, 10);
        }

        private void InitializeBoard(int width, int height, int alivePercentage) {
            Random random = new Random();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int roll = random.Next(1, 100);
                    if (roll <= alivePercentage) {
                        oldMap[x, y] = true;
                        map[x, y] = true;
                    } else {
                        oldMap[x, y] = false;
                        map[x, y] = false;
                    }
                }
            }
        }

        public void LoadContent(Texture2D whitePixel) {
            this.whitePixel = whitePixel;
        }

        public void Update(GameTime gameTime) {
            if (InputManager.Instance.KeyPressed(Keys.Space))
                pause = pause ? false : true;
            
            if (!pause) {
                timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceLastUpdate >= 0.1) {
                    timeSinceLastUpdate = 0;
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            int neighbors = CheckAliveNeighbors(x, y);
                            if (oldMap[x, y] == true) {
                                if (neighbors == 2 || neighbors == 3)
                                    map[x, y] = true;
                                else
                                    map[x, y] = false;
                            } else {
                                if (neighbors == 3)
                                    map[x, y] = true;
                            }
                        }
                    }

                    oldMap = (bool[,])map.Clone();
                }
            }
            
            if (InputManager.Instance.KeyPressed(Keys.R))
                Initialize(8, width, height);
            if (InputManager.Instance.KeyPressed(Keys.C))
                InitializeBoard(width, height, 0);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                map[Mouse.GetState().Position.X / pixelSize, Mouse.GetState().Position.Y / pixelSize] = true;
            } else if (Mouse.GetState().RightButton == ButtonState.Pressed) {
                map[Mouse.GetState().Position.X / pixelSize, Mouse.GetState().Position.Y / pixelSize] = false;
            }
        }

        private int CheckAliveNeighbors(int X, int Y) {
            int neighbors = 0;

            for (int y = -1; y < 2; y++) {
                for (int x = -1; x < 2; x++) {
                    Point checkingTile = new Point(X + x, Y + y);

                    if (x == 0 && y == 0)
                        continue;
                    if (checkingTile.X == -1)
                        checkingTile.X = width - 1;
                    else if (checkingTile.X == width)
                        checkingTile.X = 0;
                    if (checkingTile.Y == -1)
                        checkingTile.Y = height - 1;
                    else if (checkingTile.Y == height)
                        checkingTile.Y = 0;

                    if (oldMap[checkingTile.X, checkingTile.Y] == true)
                        neighbors++;
                }
            }

            return neighbors;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (map[x, y] == true) {
                        // Option One (if you have integer size and coordinates)
                        spriteBatch.Draw(whitePixel, new Rectangle(pixelSize * x, pixelSize * y, pixelSize, pixelSize),
                                Color.White);

                        // Option Two (if you have floating-point coordinates)
                        //spriteBatch.Draw(whitePixel, new Vector2(10f, 20f), null,
                        //        Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f),
                        //        SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
