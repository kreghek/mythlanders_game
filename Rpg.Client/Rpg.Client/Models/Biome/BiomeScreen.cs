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
using Rpg.Client.Models.Biome.Tutorial;
using Rpg.Client.Models.Common;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biome
{
    internal class BiomeScreen : GameScreenBase
    {
        private const int CLOUD_COUNT = 20;
        private const double MAX_CLOUD_SPEED = 0.2;
        private const int CLOUD_TEXTURE_COUNT = 3;
        private static bool _tutorial;

        private readonly Core.Biome _biome;

        private readonly Cloud[] _clouds;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;

        private readonly ButtonBase[] _menuButtons;

        private readonly IList<LocationGameObject> _locationObjectList;

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
            _locationObjectList = new List<LocationGameObject>();

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
                var cloud = CreateCloud(cloudIndex, screenInitStage: true);
                _clouds[cloudIndex] = cloud;
            }

            _globe.Updated += Globe_Updated;
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (!_isNodeModelsCreated)
            {
                return;
            }

            DrawObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_tutorial)
            {
                _tutorial = true;
                var tutorialModal = new TutorialModal(new BiomeTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                    Game.GraphicsDevice);
                AddModal(tutorialModal, isLate: false);
            }

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
                        if (node.IsAvailable)
                        {
                            var centerNodePosition = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
                            var locationObject = new LocationGameObject(node.Index % 3, node.Index / 3, centerNodePosition, node.Sid, _gameObjectContentStorage, node);
                            _locationObjectList.Add(locationObject);
                        }
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    UpdateClouds(gameTime);
                    UpdateNodeGameObjects(gameTime);

                    if (!_screenTransition)
                    {
                        var mouseState = Mouse.GetState();
                        var mouseRect = new Rectangle(mouseState.Position, new Point(1, 1));

                        var index = 0;
                        _hoverNodeGameObject = null;
                        foreach (var location in _locationObjectList)
                        {
                            if (location.NodeModel?.Combat is null)
                            {
                                index++;
                                continue;
                            }

                            var detectNode = IsNodeOnHover(location.NodeModel, mouseRect);

                            if (detectNode)
                            {
                                _hoverNodeGameObject = location.NodeModel;
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
                    }
                }
            }

            foreach (var button in _menuButtons)
            {
                button.Update();
            }
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private Cloud CreateCloud(int index, bool screenInitStage)
        {
            var endPosition = new Vector2(
                Game.GraphicsDevice.Viewport.Width * 1.5f / CLOUD_COUNT * index -
                Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height);
            const float START_VIEWPORT_Y_POSITION = -100;
            var startPosition = new Vector2(endPosition.X + Game.GraphicsDevice.Viewport.Width / 2,
                START_VIEWPORT_Y_POSITION);

            var textureIndex = _random.Next(0, CLOUD_TEXTURE_COUNT);
            var speed = _random.NextDouble() + MAX_CLOUD_SPEED;
            var cloud = new Cloud(_gameObjectContentStorage.GetBiomeClouds(),
                textureIndex,
                startPosition,
                endPosition,
                speed,
                screenInitStage);

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

            DrawSummaryXpAwardLabel(spriteBatch, node, toolTipPosition + new Vector2(5, 55));

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

            var rm = new ResourceManager(typeof(UiResource));

            var localizedName = rm.GetString($"{node.GlobeNode.Sid}NodeName");
            var normalizedName = localizedName ?? node.Name.Replace('\n', ' ');
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), normalizedName,
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
            var spriteList = new List<Sprite>();
            foreach (var location in _locationObjectList)
            {
                var sprites = location.GetSprites();
                foreach (var sprite in sprites)
                {
                    spriteList.Add(sprite);
                }
            }

            var orderedSprites = spriteList.OrderByDescending(x => x.Position.Y).ToArray();

            spriteBatch.Begin();

            foreach (var location in _locationObjectList)
            {
                location.Draw(spriteBatch);
            }

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                if (!_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex].GetShadow().Draw(spriteBatch);
                }
            }

            foreach (var sprite in orderedSprites)
            {
                sprite.Draw(spriteBatch);
            }

            foreach (var location in _locationObjectList)
            {
                location.NodeModel?.Draw(spriteBatch);
            }

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                if (!_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex].GetSprite().Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private void DrawSummaryXpAwardLabel(SpriteBatch spriteBatch, GlobeNodeGameObject node, Vector2 toolTipPosition)
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
                BiomeType.Chinese => new Rectangle(WIDTH * 0, HEIGHT * 1, WIDTH, HEIGHT),
                BiomeType.Egyptian => new Rectangle(WIDTH * 0, HEIGHT * 0, WIDTH, HEIGHT),
                BiomeType.Greek => new Rectangle(WIDTH * 0, HEIGHT * 1, WIDTH, HEIGHT),
                _ => throw new InvalidOperationException("Unknown biome type")
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
            _locationObjectList.Clear();
            _isNodeModelsCreated = false;
        }

        private static bool IsNodeOnHover(GlobeNodeGameObject node, Rectangle mouseRect)
        {
            return (mouseRect.Location.ToVector2() - node.Position).Length() <= 16;
        }

        private void UpdateClouds(GameTime gameTime)
        {
            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                _clouds[cloudIndex].Update(gameTime);

                if (_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex] = CreateCloud(cloudIndex, screenInitStage: false);
                }
            }
        }

        private void UpdateNodeGameObjects(GameTime gameTime)
        {
            foreach (var locationObject in _locationObjectList)
            {
                locationObject.Update(gameTime);
            }
        }
    }
}