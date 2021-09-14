using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Effects;
using Rpg.Client.Engine;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Models.Combat.Ui;
using Rpg.Client.Screens;

using static Rpg.Client.Core.ActiveCombat;

namespace Rpg.Client.Models.Combat
{
    internal class CombatScreen : GameScreenBase
    {
        private readonly AnimationManager _animationManager;
        private readonly IList<BulletGameObject> _bulletObjects;
        private readonly ActiveCombat _combat;
        private readonly IDice _dice;
        private readonly IList<ButtonBase> _hudButtons;
        private readonly IList<ButtonBase> _enemyAttackList;
        private readonly IList<ButtonBase> _friendlyHealList;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _bossWasDefeat;
        private CombatResultPanel? _combatResultPanel;
        private CombatSkillPanel? _combatSkillsPanel;

        private UnitGameObject? _selectedUnitForPlayerSkill; // да да, костыль.

        private bool _finalBossWasDefeat;
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
            _enemyAttackList = new List<ButtonBase>();
            _friendlyHealList = new List<ButtonBase>();
            _hudButtons = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();

            _dice = game.Services.GetService<IDice>();
        }

        public void Initialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage);
            _combatSkillsPanel.CardSelected += _combatSkillsPanel_CardSelected;
            _combat.UnitChanged += _combat_UnitChanged;
            _combat.UnitEntered += _combat_UnitEntered;
            _combat.UnitDied += _combat_UnitDied;
            _combat.ActionGenerated += _combat_ActionGenerated;
            _combat.Finish += _combat_Finish;
            _combat.UnitHadDamage += _combat_UnitHadDamage;
            _combat.Initialize();
            _combat.Update();
        }

        private void _combat_UnitHadDamage(object? sender, CombatUnit e)
        {
            GetUnitView(e).AnimateWound();
        }

        private void _combat_Finish(object? sender, CombatFinishEventArgs e)
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

        private void _combat_ActionGenerated(object? sender, ActiveCombat.ActionEventArgs action)
        {
            var actor = GetUnitView(action.Actor);
            UnitGameObject? target;
            var skillCard = new CombatSkillCard(action.Skill);// _combatSkillsPanel.SelectedCard;// e.Skill;
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
                                    : _dice.RollFromList(_gameObjects.Where(x => x.Unit.Unit.IsPlayerControlled && !x.Unit.Unit.IsDead).ToList());

                                _selectedUnitForPlayerSkill = null;

                                actor.Attack(target, blocker, bulletBlocker, _bulletObjects, skillCard, action.Action);
                                break;

                            case SkillScope.Single:
                                target = GetUnitView(action.Target);
                                if (actor.Unit.Unit.IsPlayerControlled != target.Unit.Unit.IsPlayerControlled)
                                {
                                    actor.Attack(target, blocker, bulletBlocker, _bulletObjects, skillCard, action.Action);
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
                    Debug.Fail("Не задан тип скила");
                    break;
            }
        }

        private void _combat_UnitDied(object? sender, CombatUnit e)
        {
            GetUnitView(e).AnimateDeath();
        }

        private void _combatSkillsPanel_CardSelected(object? sender, CombatSkillCard? skillCard)
        {
            RefreshHudButtons(skillCard);
        }

        private void RefreshHudButtons(CombatSkillCard? skillCard)
        {
            _hudButtons.Clear();

            if (skillCard is null)
                return;            

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

        private void InitHudButton(UnitGameObject actor, UnitGameObject target, CombatSkillCard skillCard)
        {
            switch (skillCard.Skill.TargetType)
            {
                case SkillTarget.Enemy:

                    if (actor.Unit.Unit.IsPlayerControlled != target.Unit.Unit.IsPlayerControlled)
                    {
                        var icon = new IconButton(_uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetButtonTexture(), new Rectangle(target.Position.ToPoint(), new Point(32, 32)));

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
                            default:
                                throw new InvalidOperationException();
                                break;
                        }

                        _hudButtons.Add(icon);
                    }
                    break;
                case SkillTarget.Friendly:
                    if (actor.Unit.Unit.IsPlayerControlled == target.Unit.Unit.IsPlayerControlled)
                    {
                        var icon = new IconButton(_uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetButtonTexture(), new Rectangle(target.Position.ToPoint(), new Point(32, 32)));

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
                    Debug.Fail("Не задан тип скила");
                    break;
            }
        }

        private void _combat_UnitEntered(object? sender, CombatUnit unit)
        {
            var position = GetUnitPosition(unit.Index, unit.Unit.IsPlayerControlled);
            var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
            _gameObjects.Add(gameObject);
            unit.Damaged += Unit_Damaged;
            unit.Healed += Unit_Healed;
        }

        private void Unit_Healed(object? sender, CombatUnit.UnitHpchangedEventArgs e)
        {
            var unitView = GetUnitView(e.Unit);
            AddComponent(new HpChangedComponent(Game, e.Amount, unitView.Position));
        }

        private void Unit_Damaged(object? sender, CombatUnit.UnitHpchangedEventArgs e)
        {
            var unitView = GetUnitView(e.Unit);
            AddComponent(new HpChangedComponent(Game, -e.Amount, unitView.Position));
        }

        private void _combat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            _combatSkillsPanel.Unit = e.NewUnit?.Unit.IsPlayerControlled == true ? e.NewUnit : null;
            if (e.OldUnit != null)
                GetUnitView(e.OldUnit).IsActive = false;

            if (e.NewUnit != null)
            GetUnitView(e.NewUnit).IsActive = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        private UnitGameObject GetUnitView(CombatUnit combatUnit)
        {
            return _gameObjects.First(x => x.Unit == combatUnit);
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
                foreach(var hudButton in _hudButtons)
                {
                    hudButton.Update();
                }

                foreach(var gameObject in _gameObjects)
                {
                    gameObject.Update(gameTime);
                }

                foreach(var bullet in _bulletObjects)
                {
                    bullet.Update(gameTime);
                }

                _combatSkillsPanel?.Update();

                _combatResultPanel?.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private static void ApplyXp(IEnumerable<GainLevelResult> xpItems)
        {
            foreach (var item in xpItems)
            {
                item.Unit.GainXp(item.XpAmount);
            }
        }

        private Vector2 GetUnitPosition(int index, bool friendly)
        {
            return new Vector2(friendly ? 100 : 400, index * 128 + 100);
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

            DrawBullets(spriteBatch);
            DrawUnits(spriteBatch);

            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }

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

        private Vector2 GetUnitPosition(Unit unit)
        {
            var unitWithIndex = _combat.Units.Where(x => x.Unit.IsPlayerControlled == unit.IsPlayerControlled)
                .Select((x, i) => new { Index = i, Unit = x }).First(x => x.Unit.Unit == unit);

            return GetUnitPosition(unitWithIndex.Index, unit.IsPlayerControlled);
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

        private void HandleGlobe(bool fightWon)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            if (fightWon)
            {
                _combat.Biom.Level++;

                if (_combat.Combat.IsBossLevel)
                {
                    _combat.Biom.IsComplete = true;
                    _bossWasDefeat = true;

                    if (_combat.Biom.IsFinalBiom)
                    {
                        _finalBossWasDefeat = true;
                    }
                }
            }
            else
            {
                if (_combat.Combat.IsBossLevel)
                {
                    _combat.Biom.Level = 0;
                }
            }
        }
    }
}