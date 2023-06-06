// using System.Collections.Generic;
// using System.Linq;
//
// using Client;
//
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
//
// using Rpg.Client.Engine;
//
// namespace Rpg.Client.GameScreens.Hero.Ui
// {
//     internal class GeneralInfoPanel : PanelBase
//     {
//         private readonly Core.Heroes.Hero _hero;
//         private readonly SpriteFont _mainFont;
//
//         public GeneralInfoPanel(Core.Heroes.Hero hero)
//         {
//             _hero = hero;
//             _mainFont = UiThemeManager.UiContentStorage.GetMainFont();
//         }
//
//         protected override string TitleResourceId => nameof(UiResource.HeroGeneralInfoTitle);
//
//         protected override Color CalculateColor()
//         {
//             return Color.White;
//         }
//
//         protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
//         {
//             var unitName = _hero.UnitScheme.Name;
//             var name = GameObjectHelper.GetLocalized(unitName);
//
//             var sb = new List<string>
//             {
//                 name,
//                 string.Format(UiResource.HitPointsLabelTemplate,
//                     _hero.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.ActualMax),
//                 string.Format(UiResource.ShieldPointsLabelTemplate,
//                     _hero.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value.ActualMax),
//                 string.Format(UiResource.ManaLabelTemplate, 0,
//                     _hero.EnergyPoolSize),
//                 string.Format(UiResource.CombatLevelTemplate, _hero.Level),
//                 string.Format(UiResource.CombatLevelUpTemplate, _hero.LevelUpXpAmount)
//             };
//
//             for (var statIndex = 0; statIndex < sb.Count; statIndex++)
//             {
//                 var line = sb[statIndex];
//                 spriteBatch.DrawString(_mainFont, line,
//                     new Vector2(contentRect.Left, contentRect.Top + statIndex * 22), Color.Wheat);
//             }
//         }
//     }
// }

