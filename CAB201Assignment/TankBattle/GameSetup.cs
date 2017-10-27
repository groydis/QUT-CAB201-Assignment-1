﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class GameSetup : Form
    {
        public static int numOfPlayers;
        public static int numOfRounds;

        public GameSetup()
        {
            InitializeComponent();
        }

        private void setupGame_Click(object sender, EventArgs e)
        {
            Opponent[] players = new Opponent[(int)playerNumUpDown.Value];
            numOfPlayers = (int)playerNumUpDown.Value;
            numOfRounds = (int)roundsNumUpDown.Value;
            Gameplay game = new Gameplay(numOfPlayers, numOfRounds);


            /*if (numOfPlayers > 2)
            {
                for (int i =1; i <= numOfPlayers; i++)
                {
                    players[i - 1] = new PlayerController("Player 1", TankModel.GetTank(1), Gameplay.GetColour(i));
                }
            }
            else
            {
                players[0] = new PlayerController("Player 1", TankModel.GetTank(1), Gameplay.GetColour(1));
                players[1] = new PlayerController("Player 2", TankModel.GetTank(1), Gameplay.GetColour(2));
            }*/

            for (int i = 1; i <= numOfPlayers; i++)
            {
               
                SetupPlayer form = new SetupPlayer();
                form.BackColor = Gameplay.GetColour(i);
                form.Text = "Setup Player #" + i;
                form.playNumName = "Player # "+ i + " Name:";
                form.textBoxName = "Player " + i;
                form.whichPlayer = i - 1;
                form.playersArray = players;
                form.SetupGamePlay();
                form.Show();
                players = form.playersArray;

                game.CreatePlayer(i, players[i - 1]);
            }

            game.BeginGame();
            Close();
        }
    }
}
