using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Conways_GameOfLife
{
    class Pitch : DrawableGameComponent
    {

        //Heigh/Width of Pitch
        const int PICH_HIGHT = 100;
        const int PICH_WIDTH = 100;

        const int UPDATE_INTERVAL = 30;

        //Cell dead or Alife
        bool[,,] cells;


        //Graphics
        SpriteBatch spriteBatch;
        Texture2D livingCellTexture;

        Rectangle[,] rects;

        //Input
        Input input;

        public Pitch(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {
            cells = new bool[2, PICH_WIDTH + 2, PICH_HIGHT + 2];
            rects = new Rectangle[PICH_WIDTH, PICH_HIGHT];

            this.input = input;
            this.spriteBatch = spriteBatch;
        }

        protected override void LoadContent()
        {
            livingCellTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = Color.White;
            livingCellTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);

            base.LoadContent();
        }

        int millisecondsSinceLastUpdate = 0;
        int currentIndex = 0, futureIndex = 0;

        int oldWidth, oldHeight;

        public override void Update(GameTime gameTime)
        {
            if (input.SpaceTrigger)
            {
                crateRandomCells(20);
            }

            #region Spiellogik

            //Custom Update Timer (30fps)
            millisecondsSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if(millisecondsSinceLastUpdate >= UPDATE_INTERVAL)
            {
                millisecondsSinceLastUpdate = 0;

                for(int y = 1; y < PICH_HIGHT + 1; y++)
                {
                    for(int x = 1; x < PICH_WIDTH + 1; x++)
                    {
                        int neighboursCount = 0;

                        //left 
                        if(cells[currentIndex , x - 1, y])
                        {
                            neighboursCount++;
                        }
                        //right
                        if (cells[currentIndex,x + 1, y])
                        {
                            neighboursCount++;
                        }
                        //top
                        if (cells[currentIndex, x, y - 1])
                        {
                            neighboursCount++;
                        }
                        //Bottom
                        if(cells[currentIndex, x, y + 1])
                        {
                            neighboursCount++;
                        }

                        //over x
                        //left top
                        if (cells[currentIndex, x - 1, y - 1])
                        {
                            neighboursCount++;
                        }
                        //left bottom
                        if (cells[currentIndex, x - 1, y + 1])
                        {
                            neighboursCount++;
                        }
                        //right bottom 
                        if (cells[currentIndex, x + 1, y + 1])
                        {
                            neighboursCount++;
                        }
                        //right top
                        if (cells[currentIndex, x + 1, y - 1])
                        {
                            neighboursCount++;
                        }

                        //rules
                        if(neighboursCount == 3)
                        {
                            cells[futureIndex, x, y] = true;
                        }
                        else if(neighboursCount < 2)
                        {
                            cells[futureIndex, x, y] = false;
                        }
                        else if(neighboursCount > 3)
                        {
                            cells[futureIndex, x, y] = false;
                        }
                        else if(neighboursCount == 2 && cells[currentIndex, x, y])
                        {
                            cells[futureIndex, x, y] = true;
                        }

                    }
                }   
                if(currentIndex == 0)
                {
                    currentIndex = 1;
                    futureIndex = 0;
                }
                else
                {
                    currentIndex = 0;
                    futureIndex = 1;
                }
            }
            #endregion

            #region Rectangle_Math

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            if (oldWidth != width || oldHeight != height)
            {

                int cellWith = width / PICH_WIDTH;
                int cellHeight = height / PICH_HIGHT;

                int cellSize = Math.Min(cellWith, cellHeight);

                int offsetX = (width - (cellSize * PICH_WIDTH)) / 2;
                int offsetY = (height - (cellSize * PICH_HIGHT)) / 2;

                for (int y = 1; y < PICH_HIGHT; y++)
                {
                    for (int x = 1; x < PICH_WIDTH; x++)
                    {
                        rects[x, y] = new Rectangle(offsetX + x * cellSize, offsetY + y * cellSize, cellSize, cellSize);
                    }
                }

                oldHeight = height;
                oldWidth = width;
            }
            #endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            for (int y = 1; y < PICH_HIGHT; y++)
            {
                for (int x = 1; x < PICH_WIDTH; x++)
                {
                    if (cells[currentIndex, x, y])
                    {
                        spriteBatch.Draw(livingCellTexture, rects[x - 1, y - 1], Color.White);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void crateRandomCells(int probabillity)
        {
            Random r = new Random();

            for (int x = 1; x < PICH_WIDTH; x++)
            {
                for (int y = 1; y < PICH_HIGHT; y++)
                {
                    if(r.Next(0, probabillity) == 0)
                    {
                        cells[currentIndex, x, y] = true;
                    }
                    else
                    {
                        cells[currentIndex, x, y] = false;
                    }
                }
            }
        }

    }
}
