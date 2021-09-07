﻿using System;
using System.IO;
using System.Text.Json;

using Rpg.Client.Core;

namespace Rpg.Client.Models
{
    internal sealed class GlobeProvider
    {
        private Globe? _globe;

        public Globe Globe
        {
            get
            {
                if (_globe is null)
                {
                    throw new InvalidOperationException("Globe is not initialized.");
                }
                return _globe;
            }
            private set => _globe = value;
        }

        public void GenerateNew()
        {
            var globe = new Globe
            {
                Player = new Player
                {
                    Group = new Group
                    {
                        Units = new[]
                        {
                            new Unit(UnitSchemeCatalog.SwordmanHero, 1)
                            {
                                IsPlayerControlled = true
                            }
                        }
                    }
                }
            };

            Globe = globe;
        }

        public void StoreGlobe()
        {
            var serializedGlobe = JsonSerializer.Serialize(_globe);
            var binPath = AppContext.BaseDirectory;
            var saveFilePath = Path.Combine(binPath, "globe.json");
            File.WriteAllText(saveFilePath, serializedGlobe);
        }

        public void LoadGlobe()
        {
            var binPath = AppContext.BaseDirectory;
            var saveFilePath = Path.Combine(binPath, "globe.json");

            var json = File.ReadAllText(saveFilePath);

            var globe = JsonSerializer.Deserialize<Globe>(json);

            if (globe is null)
            {
                throw new InvalidOperationException("Error during globe loading.");
            }

            Globe = globe;
        }
    }
}
