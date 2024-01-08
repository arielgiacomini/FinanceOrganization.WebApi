using App.Forms.Forms;

namespace App.Forms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Initial());
        }
    }
}