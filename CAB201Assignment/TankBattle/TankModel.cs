﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public abstract class TankModel
    {
        public const int WIDTH = 4;
        public const int HEIGHT = 3;
        public const int NUM_TANKS = 1;

        public abstract int[,] DisplayTankSprite(float angle);

        public static void LineDraw(int[,] graphic, int X1, int Y1, int X2, int Y2)
        {
            int dx = X2 - X1;
            int dy = Y2 - Y1;
            int y = Y1;
            int eps = 0;

            for (int x = X1; x >= X2; x--)
            {
                graphic[x, y] = 1;
                eps += dy;
                if ((eps << 1) >= dx)
                {
                    y++; eps -= dx;
                }
            }
        }

        public Bitmap CreateBMP(Color tankColour, float angle)
        {
            int[,] tankGraphic = DisplayTankSprite(angle);
            int height = tankGraphic.GetLength(0);
            int width = tankGraphic.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            Color tankOutline = Color.FromArgb(255, 0, 0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tankGraphic[y, x] == 0)
                    {
                        bmp.SetPixel(x, y, transparent);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, tankColour);
                    }
                }
            }

            // Outline each pixel
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (tankGraphic[y, x] != 0)
                    {
                        if (tankGraphic[y - 1, x] == 0)
                            bmp.SetPixel(x, y - 1, tankOutline);
                        if (tankGraphic[y + 1, x] == 0)
                            bmp.SetPixel(x, y + 1, tankOutline);
                        if (tankGraphic[y, x - 1] == 0)
                            bmp.SetPixel(x - 1, y, tankOutline);
                        if (tankGraphic[y, x + 1] == 0)
                            bmp.SetPixel(x + 1, y, tankOutline);
                    }
                }
            }

            return bmp;
        }

        public abstract int GetTankArmour();

        public abstract string[] WeaponList();

        public abstract void ActivateWeapon(int weapon, BattleTank playerTank, Gameplay currentGame);

        public static TankModel GetTank(int tankNumber)
        {
            return new NormTank();
        }
    }

    public class NormTank : TankModel
    {
        public override int GetTankArmour()
        {
            return 100;
        }
        public override string[] WeaponList()
        {
            return new string[] { "Standard Shell", "Falling Mines", "jumping Tinnies" };
        }

        public override int[,] DisplayTankSprite(float angle)
        {
            int length = 3;
            int end_Y = 0;
            int end_X = 0;
            int[,] norm = { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                            { 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

            if (angle > 0)
            {
                end_X = (int)Math.Cos((Math.PI / 180) * angle) * length;
                end_Y = (int)Math.Sin((Math.PI / 180) * angle) * length;

            } else if (angle < 0)
            {
                end_X = (int)Math.Sin((Math.PI / 180) * angle) * length;
                end_Y = (int)Math.Cos((Math.PI / 180) * angle) * length;
            }
            else
            {
                end_X = 7;
                end_Y = 1;
            }

            LineDraw(norm, 7, 6, end_X, end_Y);
            return norm;
        }

        public override void ActivateWeapon(int weapon, BattleTank playerTank, Gameplay currentGame)
        {
            float X_pos = playerTank.GetX() + (WIDTH /2);
            float Y_pos = playerTank.Y() + (HEIGHT / 2);

            Opponent player = playerTank.GetPlayer();

            Blast blast = new Blast(100,4,4);
            Shell shell = new Shell(X_pos,Y_pos,playerTank.GetTankAngle(),playerTank.GetCurrentPower(),0.01f,blast,player);

            currentGame.AddWeaponEffect(shell);       

        }
    }
}
