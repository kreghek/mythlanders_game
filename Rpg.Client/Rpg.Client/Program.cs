using System;
using System.Globalization;
using System.Threading;

namespace Rpg.Client
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            var defaultCulture = CultureInfo.GetCultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;

            using (var game = new EwarGame())
            {
                game.Run();
            }
        }
    }
}