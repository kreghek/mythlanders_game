using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.GameObjects;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biome
{
    internal class BiomeScreen : GameScreenBase
    {
        private const int CLOUD_COUNT = 20;
        private const double MAX_CLOUD_SPEED = 0.2;
        private const int CLOUD_TEXTURE_COUNT = 3;

        private readonly Core.Biome _biome;

        private readonly Cloud[] _clouds;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;

        private readonly ButtonBase[] _menuButtons;

        private readonly IList<GlobeNodeGameObject> _nodeModels;

        private readonly Random _random;
        private readonly IUiContentStorage _uiContentStorage;

        private GlobeNodeGameObject? _hoverNodeGameObject;

        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomeScreen(EwarGame game) : base(game)
        {
            _random = new Random();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;

            _biome = _globe.CurrentBiome ?? throw new InvalidOperationException("");

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _nodeModels = new List<GlobeNodeGameObject>();

            var mapButton = new TextButton("To The Map", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            mapButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };

            var saveGameButton = new TextButton("Save", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));

            saveGameButton.OnClick += (s, e) =>
            {
                globeProvider.StoreGlobe();
            };

            var partyModalButton = new TextButton("Party", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            partyModalButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
            };

            _menuButtons = new ButtonBase[]
            {
                mapButton,
                saveGameButton,
                partyModalButton
            };

            _clouds = new Cloud[CLOUD_COUNT];
            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                var cloud = CreateCloud(cloudIndex);
                _clouds[cloudIndex] = cloud;
            }

            _globe.Updated += Globe_Updated;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(Game.Services.GetService<IDice>());
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    foreach (var node in _biome.Nodes)
                    {
                        var position = GetBiomeNodeGraphicPositions(_biome.Type)[node.Index];
                        var nodeModel = new GlobeNodeGameObject(node, position, _gameObjectContentStorage);

                        _nodeModels.Add(nodeModel);
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    if (!_screenTransition)
                    {
                        var mouseState = Mouse.GetState();
                        var mouseRect = new Rectangle(mouseState.Position, new Point(1, 1));

                        var index = 0;
                        _hoverNodeGameObject = null;
                        foreach (var node in _nodeModels)
                        {
                            if (node.Combat is null)
                            {
                                index++;
                                continue;
                            }

                            var detectNode = IsNodeOnHover(node, mouseRect);

                            if (detectNode)
                            {
                                _hoverNodeGameObject = node;
                            }

                            index++;
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed && _hoverNodeGameObject is not null)
                        {
                            _screenTransition = true;
                            _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group,
                                _hoverNodeGameObject,
                                _hoverNodeGameObject.Combat, _biome,
                                Game.Services.GetService<IDice>());

                            if (_hoverNodeGameObject.AvailableDialog is not null)
                            {
                                _globe.CurrentEvent = _hoverNodeGameObject.AvailableDialog;
                                _globe.CurrentEventNode = _globe.CurrentEvent.BeforeCombatStartNode;

                                _globe.CurrentEvent.Counter++;

                                ClearEventHandlerToGlobeObjects();

                                ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                            }
                            else
                            {
                                ClearEventHandlerToGlobeObjects();

                                ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                            }
                        }

                        for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
                        {
                            _clouds[cloudIndex].Update(gameTime);

                            if (_clouds[cloudIndex].IsDestroyed)
                            {
                                _clouds[cloudIndex] = CreateCloud(cloudIndex);
                            }
                        }
                    }
                }
            }

            foreach (var button in _menuButtons)
            {
                button.Update();
            }

            base.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            if (!_isNodeModelsCreated)
            {
                return;
            }

            DrawObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private Cloud CreateCloud(int index)
        {
            var startPosition1 = new Vector2(
                Game.GraphicsDevice.Viewport.Width * 1.5f / CLOUD_COUNT * index -
                Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height);
            var endPosition1 = new Vector2(startPosition1.X + Game.GraphicsDevice.Viewport.Width / 2, 0);

            var startPosition = endPosition1;
            var endPosition = startPosition1;

            var textureIndex = _random.Next(0, CLOUD_TEXTURE_COUNT);
            var speed = _random.NextDouble() + MAX_CLOUD_SPEED;
            var cloud = new Cloud(_gameObjectContentStorage.GetBiomeClouds(),
                textureIndex,
                startPosition,
                endPosition,
                speed);
            return cloud;
        }

        private void DisplayCombatRewards(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject,
            Vector2 toolTipPosition, GlobeNodeGameObject node)
        {
            if (node.GlobeNode.CombatSequence is null)
            {
                // No combat - no rewards
                return;
            }

            // TODO Display icons

            DrawSummaryXpLabel(spriteBatch, node, toolTipPosition + new Vector2(5, 55));

            var equipmentType = nodeGameObject.GlobeNode.EquipmentItem;
            if (equipmentType is not null)
            {
                var targetUnitScheme = UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(equipmentType);

                var playerUnit = _globe.Player.GetAll.Where(x => x != null)
                    .SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

                if (playerUnit is not null)
                {
                    var equipmentTypeText = GetDisplayNameOfEquipment(equipmentType);
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), equipmentTypeText,
                        toolTipPosition + new Vector2(5, 45), Color.Black);
                }
            }
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            var buttonIndex = 0;
            foreach (var button in _menuButtons)
            {
                button.Rect = new Rectangle(5, 5 + buttonIndex * 25, 100, 20);
                button.Draw(spriteBatch);
                buttonIndex++;
            }

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"Level: {_biome.Level}",
                new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 5), Color.White);

            if (_hoverNodeGameObject is not null)
            {
                DrawNodeInfo(spriteBatch, _hoverNodeGameObject);
            }

            spriteBatch.End();
        }

        private void DrawNodeInfo(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject)
        {
            var toolTipPosition = nodeGameObject.Position + new Vector2(0, 16);

            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(),
                new Rectangle(toolTipPosition.ToPoint(), new Point(200, 200)),
                Color.Lerp(Color.Transparent, Color.White, 0.75f));

            var node = nodeGameObject;

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), node.Name.Replace('\n', ' '),
                toolTipPosition + new Vector2(5, 15),
                Color.Black);

            var dialogMarkerText = node.AvailableDialog is not null ? $"(!) {node.AvailableDialog.Sid}" : string.Empty;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), dialogMarkerText,
                toolTipPosition + new Vector2(5, 25), Color.Black);

            var combatSequenceSizeText = GetCombatSequenceSizeText(node);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), combatSequenceSizeText,
                toolTipPosition + new Vector2(5, 35), Color.Black);

            DisplayCombatRewards(spriteBatch, nodeGameObject, toolTipPosition, node);

            if (node.GlobeNode.CombatSequence is not null)
            {
                var monsterIndex = 0;
                var roundIndex = 1;

                foreach (var combat in node.GlobeNode.CombatSequence.Combats)
                {
                    foreach (var monster in node.Combat.EnemyGroup.Units)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            $"(rnd {roundIndex}) {monster.UnitScheme.Name} (lvl{monster.Level})",
                            toolTipPosition + new Vector2(5, 65 + monsterIndex * 10), Color.Black);

                        monsterIndex++;
                    }

                    roundIndex++;
                }
            }
        }

        private void DrawObjects(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var backgroundTexture = _uiContentStorage.GetBiomeBackground(_biome.Type);

            spriteBatch.Draw(backgroundTexture, Game.GraphicsDevice.Viewport.Bounds, GetBiomeMapRectange(_biome.Type),
                Color.White);

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                _clouds[cloudIndex].DrawShadows(spriteBatch);
            }

            foreach (var node in _nodeModels)
            {
                node.Draw(spriteBatch);
            }

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                _clouds[cloudIndex].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawSummaryXpLabel(SpriteBatch spriteBatch, GlobeNodeGameObject node, Vector2 toolTipPosition)
        {
            var monstersAmount = node.Combat.EnemyGroup.Units.Count();
            var roundsAmount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXpLabelPosition = toolTipPosition;

            var totalXpForMonsters = node.Combat.EnemyGroup.Units.Sum(x => x.XpReward);
            var summaryXp = (int)Math.Round(totalXpForMonsters * GetCombatSequenceSizeBonus(node));
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"Xp Reward: {summaryXp}",
                summaryXpLabelPosition,
                Color.Black);
        }

        private static Rectangle GetBiomeMapRectange(BiomeType type)
        {
            const int WIDTH = 800;
            const int HEIGHT = 400;
            return type switch
            {
                BiomeType.Slavic => new Rectangle(WIDTH * 0, HEIGHT * 0, WIDTH, HEIGHT),
                BiomeType.China => new Rectangle(WIDTH * 0, HEIGHT * 1, WIDTH, HEIGHT),
                BiomeType.Egypt => new Rectangle(WIDTH * 0, HEIGHT * 0, WIDTH, HEIGHT),
                BiomeType.Greek => new Rectangle(WIDTH * 0, HEIGHT * 1, WIDTH, HEIGHT),
                _ => throw new InvalidOperationException("Unknown biome type")
            };
        }

        private static Vector2[] GetBiomeNodeGraphicPositions(BiomeType type)
        {
            return type switch
            {
                BiomeType.Slavic => new[]
                {
                    new Vector2(92, 82), // 1
                    new Vector2(320, 115), // 2
                    new Vector2(210, 165), // 3
                    new Vector2(340, 255), // 4
                    new Vector2(450, 200), // 5
                    new Vector2(680, 95), // 6
                    new Vector2(740, 200), // 7
                    new Vector2(545, 240) // 8
                },
                BiomeType.China => new[]
                {
                    new Vector2(92, 82), // 1
                    new Vector2(320, 115), // 2
                    new Vector2(210, 165), // 3
                    new Vector2(340, 255), // 4
                    new Vector2(450, 200), // 5
                    new Vector2(680, 95), // 6
                    new Vector2(740, 200), // 7
                    new Vector2(545, 240) // 8
                },
                BiomeType.Egypt => new[]
                {
                    new Vector2(92, 82), // 1
                    new Vector2(320, 115), // 2
                    new Vector2(210, 165), // 3
                    new Vector2(340, 255), // 4
                    new Vector2(450, 200), // 5
                    new Vector2(680, 95), // 6
                    new Vector2(740, 200), // 7
                    new Vector2(545, 240) // 8
                },
                BiomeType.Greek => new[]
                {
                    new Vector2(92, 82), // 1
                    new Vector2(320, 115), // 2
                    new Vector2(210, 165), // 3
                    new Vector2(340, 255), // 4
                    new Vector2(450, 200), // 5
                    new Vector2(680, 95), // 6
                    new Vector2(740, 200), // 7
                    new Vector2(545, 240) // 8
                },
                _ => throw new InvalidOperationException($"Unknown biome type {type}.")
            };
        }

        private static float GetCombatSequenceSizeBonus(GlobeNodeGameObject node)
        {
            var count = node.GlobeNode.CombatSequence.Combats.Count;
            switch (count)
            {
                case 1:
                    return 1;

                case 3:
                    return 1.25f;

                case 5:
                    return 1.5f;

                default:
                    Debug.Fail("Unknown size");
                    return 1;
            }
        }

        private static string GetCombatSequenceSizeText(GlobeNodeGameObject node)
        {
            var count = node.GlobeNode.CombatSequence.Combats.Count;
            switch (count)
            {
                case 1:
                    return "Short";

                case 3:
                    return "Medium (+25% XP)";

                case 5:
                    return "Long (+50% XP)";

                default:
                    Debug.Fail("Unknown size");
                    return string.Empty;
            }
        }

        private static string? GetDisplayNameOfEquipment(EquipmentItemType? equipmentType)
        {
            if (equipmentType is null)
            {
                return null;
            }

            var rm = new ResourceManager(typeof(UiResource).FullName, typeof(UiResource).Assembly);

            var equipmentDisplayName = rm.GetString($"{equipmentType}EquipmentItemDisplayName");

            if (equipmentDisplayName is null)
            {
                return $"{equipmentType} equipment items";
            }

            return equipmentDisplayName;
        }

        private void Globe_Updated(object? sender, EventArgs e)
        {
            // This happens when cheat is used.
            _nodeModels.Clear();
            _isNodeModelsCreated = false;
        }

        private static bool IsNodeOnHover(GlobeNodeGameObject node, Rectangle mouseRect)
        {
            return (mouseRect.Location.ToVector2() - node.Position).Length() <= 16;
        }
    }
}