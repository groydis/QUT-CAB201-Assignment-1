﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Map
    {
        public const int WIDTH = 160;
        public const int HEIGHT = 120;

        private bool[,] thisMap = new bool[WIDTH, HEIGHT];

        Random rng = new Random();

        public Map()
        {
            for (int i = 0; i < WIDTH; i++)
            {
                thisMap[i, HEIGHT -1] = true;
                thisMap[i, 0] = false;
            }

            int newX, newY;
            for (int i = 0; i < 250; i++)
            {
                newX = rng.Next(0, 159);
                newY = rng.Next(0, 119);

                while (Get(newX, newY + 1) == false && newY <= 119)
                {
                    newY++;
                };

                thisMap[newX, newY] = true;
            }

        }

        public bool Get(int x, int y)
        {
            if (x >= 0 && x <= WIDTH)
            {
                if (y >= 0 && y <= HEIGHT)
                {
                    if (thisMap[x,y] == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckTankCollide(int x, int y)
        {
            int tankBott = y + TankModel.HEIGHT;
            int tankR = x + TankModel.WIDTH;

            if (x >= 0 && y >= 0)
            {
                if (tankR <= WIDTH && tankBott <= HEIGHT)
                {
                    for (int i = x; i < tankR; i++)
                    {
                        for (int z = y; z < tankBott; z++)
                        {
                            if (Get(i, z) == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public int TankYPosition(int x)
        {
            //Not technically correct just trying to get past it and hopefully get it working later on
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int ix = x; ix <= TankModel.WIDTH; ix++)
                {
                    if (CheckTankCollide(ix, y ) == true)
                    {
                        return y;
                    }
                }

            }
            return x;
        }

        public void DestroyTerrain(float destroyX, float destroyY, float radius)
        {
            for (int y = 0; y > HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    double dist = Math.Sqrt(Math.Pow(x - destroyX, 2) + Math.Pow(y - destroyY, 2));
                    if (dist < radius)
                    {
                        if (thisMap[x, y] == true)
                        {
                            thisMap[x, y] = false;
                        }
                    }
                }
            }
        }

        public bool GravityStep()
        {
            for (int y = 0; y > HEIGHT; y++)
            {
                for (int x = 0; x <= WIDTH; x++)
                {
                    if(thisMap[x, y] == true)
                    {
                       if(Get(x, y + 1) == false)
                       {
                            thisMap[x, y] = false;
                            thisMap[x, y + 1] = true;
                       }
                    }
                }
            }
            return false;
        }
    }
}
