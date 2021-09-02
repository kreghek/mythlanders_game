﻿using System;
using System.Globalization;
using System.Threading;

namespace Rpg.Client
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var defaultCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;

            using (var game = new Game1())
                game.Run();
        }
    }
}
