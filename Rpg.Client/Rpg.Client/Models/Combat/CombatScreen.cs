using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.GameObjects;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Models.Combat.Ui;
using Rpg.Client.Screens;

using static Rpg.Client.Core.ActiveCombat;

namespace Rpg.Client.Models.Combat
{
    internal class CombatScreen : GameScreenBase
    {
        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        private static readonly Vector2[] _unitPredefinedPositions =
        {
            new Vector2(300, 300),
            new Vector2(200, 250),
            new Vector2(200, 350)
        };

        private readonly AnimationManager _animationManager;
        private readonly IList<BulletGameObject> _bulletObjects;
        private readonly ActiveCombat _combat;
        private readonly IDice _dice;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly Globe _globe;
        private readonly GlobeNodeGameObject1 _globeNodeGameObject;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _hudButtons;
        private readonly IUiContentStorage _uiContentStorage;

        private float _bgCenterOffsetPercentage;
        private bool _bossWasDefeat;
        private CombatResultModal? _combatResultModal;
        private CombatSkillPanel? _combatSkillsPanel;

        private bool _finalBossWasDefeat;

        private bool _unitsInitialized;

        public CombatScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayBattleTrack();

            _globeProvider = game.Services.GetService<GlobeProvider>();

            _globe = _globeProvider.Globe;

            _combat = _globe.ActiveCombat ??
                      throw new InvalidOperationException(
                          nameof(_globe.ActiveCombat) + " can't be null in this screen.");

            _globeNodeGameObject = _combat.Node;

            _gameObjects = new List<UnitGameObject>();
            _bulletObjects = new List<BulletGameObject>();
            _hudButtons = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();
            _dice = Game.Services.GetService<IDice>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);

            base.Draw(gameTime, spriteBatch);

            DrawModals(spriteBatch);
        }

        public void Initialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage, _combat);
            _combatSkillsPanel.CardSelected += CombatSkillsPanel_CardSelected;
            _combat.UnitChanged += Combat_UnitChanged;
            _combat.UnitReadyToControl += Combat_UnitReadyToControl;
            _combat.UnitEntered += Combat_UnitEntered;
            _combat.UnitDied += Combat_UnitDied;
            _combat.ActionGenerated += Combat_ActionGenerated;
            _combat.Finish += Combat_Finish;
            _combat.UnitHadDamage += Combat_UnitHadDamage;
            _combat.UnitPassed += Combat_UnitPassed;
            _combat.Initialize();
            _combat.Update();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            if (!_unitsInitialized)
            {
                Initialize();
                _unitsInitialized = true;
            }
            else
            {
                HandleBullets(gameTime);

                HandleUnits(gameTime);

                if (_combat.Finished)
                {
                    HandleFinishedCombatHud(gameTime);
                }
                else
                {
                    HandleCombatHud(gameTime);
                }
            }

            HandleBackgrounds();

            base.Update(gameTime);
        }

        private void Actor_SkillAnimationCompleted(object? sender, EventArgs e)
        {
            if (sender is UnitGameObject unit)
            {
                unit.SkillAnimationCompleted -= Actor_SkillAnimationCompleted;
            }

            _combat.Update();
        }

        private static void ApplyXp(IEnumerable<XpAward> xpItems)
        {
            foreach (var item in xpItems)
            {
                item.Unit.GainXp(item.XpAmount);
            }
        }


        private void Combat_ActionGenerated(object? sender, ActionEventArgs action)
        {
            var actor = GetUnitGameObject(action.Actor);
            var target = GetUnitGameObject(action.Target);

            var blocker = _animationManager.CreateAndUseBlocker();

            actor.SkillAnimationCompleted += Actor_SkillAnimationCompleted;

            var bulletBlocker = _animationManager.CreateAndUseBlocker();

            actor.UseSkill(target, blocker, bulletBlocker, _bulletObjects, action.Skill, action.Action);
        }

        private void Combat_Finish(object? sender, CombatFinishEventArgs e)
        {
            _hudButtons.Clear();
            _combatSkillsPanel = null;

            if (e.Victory)
            {
                var completedCombats = _globeNodeGameObject.GlobeNode.CombatSequence.CompletedCombats;
                completedCombats.Add(_combat.Combat);

                var currentCombatList = _combat.Node.GlobeNode.CombatSequence.Combats.ToList();
                if (currentCombatList.Count == 1)
                {
                    var xpItems = HandleGainXp(completedCombats).ToArray();
                    ApplyXp(xpItems);
                    GainEquipmentItems(_globeNodeGameObject.GlobeNode, _globeProvider.Globe.Player);
                    HandleGlobe(CombatResult.Victory);

                    _combatResultModal = new CombatResultModal(_uiContentStorage, Game.GraphicsDevice,
                        CombatResult.Victory,
                        xpItems);
                }
                else
                {
                    _combatResultModal = new CombatResultModal(_uiContentStorage, Game.GraphicsDevice,
                        CombatResult.NextCombat,
                        Array.Empty<XpAward>());
                }
            }
            else
            {
                HandleGlobe(CombatResult.Defeat);

                _combatResultModal = new CombatResultModal(_uiContentStorage, Game.GraphicsDevice, CombatResult.Defeat,
                    Array.Empty<XpAward>());
            }

            _combatResultModal.Show();

            _combatResultModal.Closed += CombatResultModal_Closed;
        }

        private void Combat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            if (e.OldUnit != null)
            {
                var oldView = GetUnitGameObject(e.OldUnit);
                oldView.IsActive = false;
            }

            _combatSkillsPanel.Unit = null;
            _combatSkillsPanel.SelectedCard = null;
            _combatSkillsPanel.IsEnabled = false;
        }

        private void Combat_UnitDied(object? sender, CombatUnit e)
        {
            var unitGameObject = GetUnitGameObject(e);
            unitGameObject.AnimateDeath();
        }

        private void Combat_UnitEntered(object? sender, CombatUnit unit)
        {
            var position = GetUnitPosition(unit.Index, unit.Unit.IsPlayerControlled);
            var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
            _gameObjects.Add(gameObject);
            unit.Damaged += Unit_Damaged;
            unit.Healed += Unit_Healed;
        }

        private void Combat_UnitHadDamage(object? sender, CombatUnit e)
        {
            var unitGameObject = GetUnitGameObject(e);

            unitGameObject.AnimateWound();
        }

        private void Combat_UnitPassed(object? sender, CombatUnit e)
        {
            AddComponent(new MovePassedComponent(Game, GetUnitGameObject(e).Position));
        }

        private void Combat_UnitReadyToControl(object? sender, CombatUnit e)
        {
            if (!e.Unit.IsPlayerControlled)
            {
                return;
            }

            if (_combatSkillsPanel is null)
            {
                return;
            }

            _combatSkillsPanel.IsEnabled = true;
            _combatSkillsPanel.Unit = e;
            var unitGameObject = GetUnitGameObject(e);
            unitGameObject.IsActive = true;
        }

        private void CombatResultModal_Closed(object? sender, EventArgs e)
        {
            _animationManager.DropBlockers();

            var currentCombatList = _globeNodeGameObject.GlobeNode.CombatSequence.Combats.ToList();
            currentCombatList.Remove(_combat.Combat);
            _globeNodeGameObject.GlobeNode.CombatSequence.Combats = currentCombatList;

            if (_combat.Node.GlobeNode.CombatSequence.Combats.Any())
            {
                var nextCombat = _globeNodeGameObject.GlobeNode.CombatSequence.Combats.First();
                _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group,
                    _globeNodeGameObject,
                    nextCombat,
                    _combat.Biom,
                    _dice);

                ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
            }
            else
            {
                _globeProvider.Globe.UpdateNodes(_dice);
                RestoreGroupAfterCombat();

                if (_bossWasDefeat)
                {
                    if (_finalBossWasDefeat)
                    {
                        ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
                    }
                    else
                    {
                        ScreenManager.ExecuteTransition(this, ScreenTransition.EndGame);
                    }
                }
                else
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                }
            }
        }

        private void CombatSkillsPanel_CardSelected(object? sender, CombatSkillCard? skillCard)
        {
            RefreshHudButtons(skillCard);
        }

        private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
            {
                var xFloat = backgroundStartOffset + _bgCenterOffsetPercentage * (BACKGROUND_LAYERS_COUNT - i - 1) *
                    BACKGROUND_LAYERS_SPEED * backgroundMaxOffset;
                var roundedX = (int)Math.Round(xFloat);
                var position = new Vector2(roundedX, 0);
                spriteBatch.Draw(backgrounds[i], position, Color.White);
            }
        }

        private void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            var xFloat = backgroundStartOffset +
                         -1 * _bgCenterOffsetPercentage * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
            var roundedX = (int)Math.Round(xFloat);
            spriteBatch.Draw(backgrounds[3], new Vector2(roundedX, 0), Color.White);
        }

        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds();

            const int BG_START_OFFSET = -100;
            const int BG_MAX_OFSSET = 200;

            DrawBackgroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFSSET);

            DrawBullets(spriteBatch);
            DrawUnits(spriteBatch);

            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }

            DrawForegroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFSSET);

            spriteBatch.End();
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (_combat.CurrentUnit?.Unit.IsPlayerControlled == true && !_animationManager.HasBlockers)
            {
                if (_combatSkillsPanel is not null)
                {
                    _combatSkillsPanel.Draw(spriteBatch, Game.GraphicsDevice);
                }

                foreach (var button in _hudButtons)
                {
                    button.Draw(spriteBatch);
                }
            }

            try
            {
                if (_globeNodeGameObject.GlobeNode.CombatSequence is not null)
                {
                    var combatCountRemains = _globeNodeGameObject.GlobeNode.CombatSequence.Combats.Count;

                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"Combats remains: {combatCountRemains}",
                        new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 5), Color.White);
                }
            }
            catch
            {
                // TODO Fix NRE in the end of the combat with more prefessional way 
            }

            spriteBatch.End();
        }

        private void DrawModals(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _combatResultModal?.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawUnits(SpriteBatch spriteBatch)
        {
            var list = _gameObjects.ToArray();
            foreach (var gameObject in list)
            {
                gameObject.Draw(spriteBatch);
            }
        }

        private static void GainEquipmentItems(GlobeNode globeNode, Player? player)
        {
            var equipmentItemType = globeNode.EquipmentItem;

            var targetUnitScheme = UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(equipmentItemType);
            var targetUnit = player.Group.Units.SingleOrDefault(x => x.UnitScheme == targetUnitScheme);
            if (targetUnit is null)
            {
                targetUnit = player.Pool.Units.SingleOrDefault(x => x.UnitScheme == targetUnitScheme);
            }

            if (targetUnit is not null)
            {
                targetUnit.GainEquipmentItem(1);
            }
        }

        private UnitGameObject GetUnitGameObject(CombatUnit combatUnit)
        {
            return _gameObjects.First(x => x.CombatUnit == combatUnit);
        }

        private Vector2 GetUnitPosition(int index, bool isPlayerControlled)
        {
            var predefinedPosition = _unitPredefinedPositions[index];

            Vector2 calculatedPosition;

            if (isPlayerControlled)
            {
                calculatedPosition = predefinedPosition;
            }
            else
            {
                var width = Game.GraphicsDevice.Viewport.Width;
                // Move from right edge.
                var xMirror = width - predefinedPosition.X;
                calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
            }

            return calculatedPosition;
        }

        private void HandleBackgrounds()
        {
            var mouse = Mouse.GetState();
            _bgCenterOffsetPercentage = ((float)mouse.X - Game.GraphicsDevice.Viewport.Width / 2) /
                                        Game.GraphicsDevice.Viewport.Width / 2;
            if (_bgCenterOffsetPercentage < -1)
            {
                _bgCenterOffsetPercentage = -1;
            }
            else if (_bgCenterOffsetPercentage > 1)
            {
                _bgCenterOffsetPercentage = 1;
            }
        }

        private void HandleBullets(GameTime gameTime)
        {
            foreach (var bullet in _bulletObjects.ToArray())
            {
                if (bullet.IsDestroyed)
                {
                    _bulletObjects.Remove(bullet);
                }
                else
                {
                    bullet.Update(gameTime);
                }
            }
        }

        private void HandleCombatHud(GameTime gameTime)
        {
            foreach (var hudButton in _hudButtons)
            {
                hudButton.Update();
            }

            _combatSkillsPanel?.Update();

            _combatResultModal?.Update(gameTime);
        }

        private void HandleFinishedCombatHud(GameTime gameTime)
        {
            if (_combatResultModal?.IsVisible == true)
            {
                _combatResultModal?.Update(gameTime);
            }
        }

        private IEnumerable<XpAward> HandleGainXp(IList<Core.Combat> completedCombats)
        {
            var combatSequenceCoeffs = new[] { 1f, 0 /*not used*/, 1.25f, /*not used*/0, 1.5f };

            var aliveUnits = _combat.Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();
            var monsters = completedCombats.SelectMany(x => x.EnemyGroup.Units).ToArray();

            var sequenceBonus = combatSequenceCoeffs[completedCombats.Count - 1];
            var summaryXp = (int)Math.Round(monsters.Sum(x => x.XpReward) * sequenceBonus);
            var xpPerPlayerUnit = summaryXp / aliveUnits.Length;

            var remains = summaryXp - (xpPerPlayerUnit * aliveUnits.Length);

            var remainsUsed = false;
            foreach (var unit in aliveUnits)
            {
                var gainedXp = xpPerPlayerUnit;

                if (!remainsUsed)
                {
                    gainedXp += remains;
                    remainsUsed = true;
                }

                yield return new XpAward
                {
                    StartXp = unit.Unit.Xp,
                    Unit = unit.Unit,
                    XpAmount = gainedXp,
                    XpToLevelup = unit.Unit.LevelupXp
                };
            }
        }

        private void HandleGlobe(CombatResult result)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            switch (result)
            {
                case CombatResult.Victory:
                    _combat.Biom.Level++;

                    if (_combat.Combat.IsBossLevel)
                    {
                        _combat.Biom.IsComplete = true;
                        _bossWasDefeat = true;

                        if (_combat.Biom.IsFinal)
                        {
                            _finalBossWasDefeat = true;
                        }
                    }

                    break;

                case CombatResult.Defeat:
                    var levelDiff = _combat.Biom.Level - _combat.Biom.MinLevel;
                    _combat.Biom.Level = Math.Max(levelDiff / 2, _combat.Biom.MinLevel);

                    break;

                default:
                    throw new InvalidOperationException("Unknown combat result.");
            }
        }

        private void HandleUnits(GameTime gameTime)
        {
            foreach (var unitModel in _gameObjects)
            {
                unitModel.Update(gameTime);
            }
        }

        private void InitHudButton(UnitGameObject target, CombatSkillCard skillCard)
        {
            var icon = new IconButton(_uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetButtonTexture(),
                new Rectangle(target.Position.ToPoint(), new Point(32, 32)));

            icon.OnClick += (s, e) =>
            {
                _combat.UseSkill(skillCard.Skill, target.CombatUnit);
            };

            _hudButtons.Add(icon);
        }

        private void RefreshHudButtons(CombatSkillCard? skillCard)
        {
            _hudButtons.Clear();

            if (skillCard is null)
            {
                return;
            }

            if (_combat.CurrentUnit is null)
            {
                Debug.Fail("WTF!");
                return;
            }

            var actor = GetUnitGameObject(_combat.CurrentUnit);
            var skill = skillCard.Skill;

            foreach (var target in _gameObjects.Where(x => !x.CombatUnit.Unit.IsDead))
            {
                if (skillCard.Skill.TargetType == SkillTargetType.Enemy && target.CombatUnit.Unit.IsPlayerControlled ==
                    _combat.CurrentUnit.Unit.IsPlayerControlled)
                {
                    continue;
                }

                if (skillCard.Skill.TargetType == SkillTargetType.Friendly && target.CombatUnit.Unit.IsPlayerControlled !=
                    _combat.CurrentUnit.Unit.IsPlayerControlled)
                {
                    continue;
                }

                InitHudButton(target, skillCard);
            }
        }

        private void RestoreGroupAfterCombat()
        {
            foreach (var unit in _globe.Player.GetAll)
            {
                unit.RestoreHPAfterCombat();
                unit.RestoreManaPoint();
            }
        }

        private void Unit_Damaged(object? sender, CombatUnit.UnitHpChangedEventArgs e)
        {
            var unitView = GetUnitGameObject(e.Unit);
            AddComponent(new HpChangedComponent(Game, -e.Amount, unitView.Position));
        }

        private void Unit_Healed(object? sender, CombatUnit.UnitHpChangedEventArgs e)
        {
            var unitView = GetUnitGameObject(e.Unit);
            AddComponent(new HpChangedComponent(Game, e.Amount, unitView.Position));
        }
    }
}