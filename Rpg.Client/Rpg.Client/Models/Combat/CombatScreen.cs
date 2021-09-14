using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Models.Combat.Ui;
using Rpg.Client.Screens;

using static Rpg.Client.Core.ActiveCombat;

namespace Rpg.Client.Models.Combat
{
    internal class CombatScreen : GameScreenBase
    {
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
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _hudButtons;
        private readonly IUiContentStorage _uiContentStorage;

        private float _bgCenterOffsetPercentage;
        private bool _bossWasDefeat;
        private CombatResultPanel? _combatResultPanel;
        private CombatSkillPanel? _combatSkillsPanel;

        private bool _finalBossWasDefeat;

        private UnitGameObject? _selectedUnitForPlayerSkill; // �� ��, �������.
        private bool _unitsInitialized;

        public CombatScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayBattleTrack();

            _globeProvider = game.Services.GetService<GlobeProvider>();

            var globe = _globeProvider.Globe;

            _combat = globe.ActiveCombat ??
                      throw new InvalidOperationException(nameof(globe.ActiveCombat) + " is null");

            _gameObjects = new List<UnitGameObject>();
            _bulletObjects = new List<BulletGameObject>();
            _hudButtons = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();

            _dice = game.Services.GetService<IDice>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        public void Initialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage);
            _combatSkillsPanel.CardSelected += CombatSkillsPanel_CardSelected;
            _combat.UnitChanged += Combat_UnitChanged;
            _combat.UnitEntered += Combat_UnitEntered;
            _combat.UnitDied += Combat_UnitDied;
            _combat.ActionGenerated += Combat_ActionGenerated;
            _combat.Finish += Combat_Finish;
            _combat.UnitHadDamage += Combat_UnitHadDamage;
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
            else if (_combat.Finished)
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

                foreach (var unitModel in _gameObjects)
                {
                    unitModel.IsActive = false;

                    unitModel.Update(gameTime);
                }

                _combatResultPanel?.Update(gameTime);
            }
            else
            {
                foreach (var hudButton in _hudButtons)
                {
                    hudButton.Update();
                }

                foreach (var gameObject in _gameObjects)
                {
                    gameObject.Update(gameTime);
                }

                foreach (var bullet in _bulletObjects)
                {
                    bullet.Update(gameTime);
                }

                _combatSkillsPanel?.Update();

                _combatResultPanel?.Update(gameTime);
            }

            HandleBackgrounds();

            base.Update(gameTime);
        }

        private static void ApplyXp(IEnumerable<GainLevelResult> xpItems)
        {
            foreach (var item in xpItems)
            {
                item.Unit.GainXp(item.XpAmount);
            }
        }

        private void Combat_ActionGenerated(object? sender, ActionEventArgs action)
        {
            var actor = GetUnitView(action.Actor);
            UnitGameObject? target;
            var skillCard = new CombatSkillCard(action.Skill); // _combatSkillsPanel.SelectedCard;// e.Skill;
            switch (skillCard.Skill.TargetType)
            {
                case SkillTarget.Enemy:
                    {
                        var blocker = _animationManager.CreateAndUseBlocker();
                        var bulletBlocker = _animationManager.CreateAndUseBlocker();

                        blocker.Released += (s, e) =>
                        {
                            _combat.Update();
                        };

                        switch (skillCard.Skill.Scope)
                        {
                            case SkillScope.AllEnemyGroup:
                                target = actor.Unit.Unit.IsPlayerControlled
                                    ? _selectedUnitForPlayerSkill
                                    : _dice.RollFromList(_gameObjects
                                        .Where(x => x.Unit.Unit.IsPlayerControlled && !x.Unit.Unit.IsDead).ToList());

                                _selectedUnitForPlayerSkill = null;

                                actor.Attack(target, blocker, bulletBlocker, _bulletObjects, skillCard, action.Action);
                                break;

                            case SkillScope.Single:
                                target = GetUnitView(action.Target);
                                if (actor.Unit.Unit.IsPlayerControlled != target.Unit.Unit.IsPlayerControlled)
                                {
                                    actor.Attack(target, blocker, bulletBlocker, _bulletObjects, skillCard,
                                        action.Action);
                                }

                                break;

                            default:
                                throw new InvalidOperationException();
                                break;
                        }
                    }
                    break;

                case SkillTarget.Friendly:
                    {
                        switch (skillCard.Skill.Scope)
                        {
                            case SkillScope.Single:
                                target = GetUnitView(action.Target);

                                var blocker = _animationManager.CreateAndUseBlocker();

                                blocker.Released += (s, e) =>
                                {
                                    _combat.Update();
                                };

                                actor.Heal(target, blocker, skillCard, action.Action);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    break;
                default:
                    Debug.Fail("�� ����� ��� �����");
                    break;
            }
        }

        private void Combat_Finish(object? sender, CombatFinishEventArgs e)
        {
            _hudButtons.Clear();
            _combatSkillsPanel = null;
            _combatResultPanel = new CombatResultPanel(_uiContentStorage);
            _combatResultPanel.Closed += CombatResultPanel_Closed;
            if (e.Victory)
            {
                var xpItems = HandleGainXp().ToArray();
                ApplyXp(xpItems);
                HandleGlobe(true);
                _combatResultPanel.Initialize(CombatResult.Victory, xpItems);
            }
            else
            {
                HandleGlobe(false);
                _combatResultPanel.Initialize(CombatResult.Defeat, Array.Empty<GainLevelResult>());
            }
        }

        private void Combat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            _combatSkillsPanel.Unit = e.NewUnit?.Unit.IsPlayerControlled == true ? e.NewUnit : null;
            if (e.OldUnit != null)
            {
                GetUnitView(e.OldUnit).IsActive = false;
            }

            if (e.NewUnit != null)
            {
                GetUnitView(e.NewUnit).IsActive = true;
            }
        }

        private void Combat_UnitDied(object? sender, CombatUnit e)
        {
            GetUnitView(e).AnimateDeath();
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
            var unitGameObject = GetUnitView(e);

            unitGameObject.AnimateWound();
        }

        private void CombatResultPanel_Closed(object? sender, EventArgs e)
        {
            _animationManager.DropBlockers();

            var dice = Game.Services.GetService<IDice>();
            _globeProvider.Globe.UpdateNodes(dice);

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

        private void CombatSkillsPanel_CardSelected(object? sender, CombatSkillCard? skillCard)
        {
            RefreshHudButtons(skillCard);
        }

        private void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds();

            const int BG_START_OFFSET = -100;
            const int BG_MAX_OFSSET = 200;

            spriteBatch.Draw(backgrounds[0],
                new Vector2(BG_START_OFFSET + _bgCenterOffsetPercentage * 0.5f * BG_MAX_OFSSET, 0), Color.White);
            spriteBatch.Draw(backgrounds[1],
                new Vector2(BG_START_OFFSET + _bgCenterOffsetPercentage * 0.15f * BG_MAX_OFSSET, 0), Color.White);
            spriteBatch.Draw(backgrounds[2], new Vector2(BG_START_OFFSET, 0), Color.White);

            DrawBullets(spriteBatch);
            DrawUnits(spriteBatch);

            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.Draw(backgrounds[3],
                new Vector2(BG_START_OFFSET + -1 * _bgCenterOffsetPercentage * 0.5f * BG_MAX_OFSSET, 0), Color.White);

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

            _combatResultPanel?.Draw(spriteBatch, Game.GraphicsDevice);

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

        private static Vector2 GetUnitPosition(int index, bool isPlayerControlled)
        {
            var predefinedPosition = _unitPredefinedPositions[index];

            Vector2 calculatedPosition;

            if (isPlayerControlled)
            {
                calculatedPosition = predefinedPosition;
            }
            else
            {
                var xMirror = 400 + (400 - predefinedPosition.X);
                calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
            }

            return calculatedPosition;
        }

        private UnitGameObject GetUnitView(CombatUnit combatUnit)
        {
            return _gameObjects.First(x => x.Unit == combatUnit);
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

        private IEnumerable<GainLevelResult> HandleGainXp()
        {
            var aliveUnits = _combat.Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();
            var monsters = _combat.Units.Where(x => !x.Unit.IsPlayerControlled && x.Unit.IsDead).ToArray();

            var summaryXp = monsters.Sum(x => x.Unit.XpReward);
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

                yield return new GainLevelResult
                {
                    StartXp = unit.Unit.Xp,
                    Unit = unit.Unit,
                    XpAmount = gainedXp,
                    XpToLevelup = unit.Unit.XpToLevelup
                };
            }
        }

        private void HandleGlobe(bool victory)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            if (victory)
            {
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
            }
            else
            {
                if (_combat.Combat.IsBossLevel)
                {
                    var levelDiff = _combat.Biom.Level - _combat.Biom.MinLevel;
                    _combat.Biom.Level = levelDiff / 2;
                }
            }
        }

        private void InitHudButton(UnitGameObject actor, UnitGameObject target, CombatSkillCard skillCard)
        {
            switch (skillCard.Skill.TargetType)
            {
                case SkillTarget.Enemy:

                    if (actor.Unit.Unit.IsPlayerControlled != target.Unit.Unit.IsPlayerControlled)
                    {
                        var icon = new IconButton(_uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetButtonTexture(),
                            new Rectangle(target.Position.ToPoint(), new Point(32, 32)));

                        switch (skillCard.Skill.Scope)
                        {
                            case SkillScope.AllEnemyGroup:
                                icon.OnClick += (s, e) =>
                                {
                                    _selectedUnitForPlayerSkill = target;
                                    _combat.UseSkill(skillCard.Skill);
                                };
                                break;
                            case SkillScope.Single:
                                icon.OnClick += (s, e) =>
                                {
                                    _combat.UseSkill(skillCard.Skill, target.Unit);
                                };
                                break;
                            case SkillScope.Undefined:
                            default:
                                throw new InvalidOperationException();
                        }

                        _hudButtons.Add(icon);
                    }

                    break;
                case SkillTarget.Friendly:
                    if (actor.Unit.Unit.IsPlayerControlled == target.Unit.Unit.IsPlayerControlled)
                    {
                        var icon = new IconButton(_uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetButtonTexture(),
                            new Rectangle(target.Position.ToPoint(), new Point(32, 32)));

                        switch (skillCard.Skill.Scope)
                        {
                            case SkillScope.Single:
                                icon.OnClick += (s, e) =>
                                {
                                    _combat.UseSkill(skillCard.Skill, target.Unit);
                                };
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        _hudButtons.Add(icon);
                    }

                    break;
                default:
                    Debug.Fail("�� ����� ��� �����");
                    break;
            }
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

            var actor = GetUnitView(_combat.CurrentUnit);
            var skill = skillCard.Skill;

            foreach (var target in _gameObjects.Where(x => !x.Unit.Unit.IsDead))
            {
                InitHudButton(actor, target, skillCard);
            }
        }

        private void Unit_Damaged(object? sender, CombatUnit.UnitHpchangedEventArgs e)
        {
            var unitView = GetUnitView(e.Unit);
            AddComponent(new HpChangedComponent(Game, -e.Amount, unitView.Position));
        }

        private void Unit_Healed(object? sender, CombatUnit.UnitHpchangedEventArgs e)
        {
            var unitView = GetUnitView(e.Unit);
            AddComponent(new HpChangedComponent(Game, e.Amount, unitView.Position));
        }
    }
}