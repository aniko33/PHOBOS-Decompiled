using System;
using System.Windows.Forms;
using Gameform;
public class Program {
    [STAThread]
    public static void Main() {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Gameform.Gameform()); // Run game
    }
}