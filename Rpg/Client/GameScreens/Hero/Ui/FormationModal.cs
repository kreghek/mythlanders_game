// using System.Collections.Generic;
// using System.Linq;
//
// using Client;
// using Client.Core;
// using Client.Engine;
//
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
//
// using Rpg.Client.Core;
// using Rpg.Client.Engine;
//
// namespace Rpg.Client.GameScreens.Hero.Ui
// {
//     internal sealed class FormationModal : ModalDialogBase
//     {
//         private const int TOP_MARGIN = 50;
//         private readonly IList<ButtonBase> _buttonList;
//         private readonly Core.Heroes.Hero _character;
//         private readonly Player _player;
//         private readonly IUiContentStorage _uiContentStorage;
//
//         public FormationModal(IUiContentStorage uiContentStorage, Core.Heroes.Hero character, Player player,
//             ResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
//             resolutionIndependentRenderer)
//         {
//             _uiContentStorage = uiContentStorage;
//             _character = character;
//             _player = player;
//
//             _buttonList = new List<ButtonBase>();
//         }
//
//         protected override void DrawContent(SpriteBatch spriteBatch)
//         {
//             for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
//             {
//                 const int BUTTON_WIDTH = 100;
//                 const int BUTTON_HEIGHT = 20;
//
//                 var button = _buttonList[buttonIndex];
//                 const int BUTTON_MARGIN = 5;
//                 var offset = new Point(0, (BUTTON_HEIGHT + BUTTON_MARGIN) * buttonIndex);
//                 var panelLocation = new Point(ContentRect.Center.X - BUTTON_WIDTH / 2, ContentRect.Top + TOP_MARGIN);
//                 var buttonSize = new Point(BUTTON_WIDTH, BUTTON_HEIGHT);
//
//                 button.Rect = new Rectangle(panelLocation + offset, buttonSize);
//                 button.Draw(spriteBatch);
//             }
//         }
//
//         protected override void InitContent()
//         {
//             base.InitContent();
//
//             InitSlotAssignmentButtons(_character, _player);
//         }
//
//         protected override void UpdateContent(GameTime gameTime,
//             ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
//         {
//             base.UpdateContent(gameTime, resolutionIndependenceRenderer);
//
//             foreach (var button in _buttonList.ToArray())
//             {
//                 button.Update(resolutionIndependenceRenderer);
//             }
//         }
//
//         private IEnumerable<GroupSlot> GetAvailableSlots(IEnumerable<GroupSlot> freeSlots)
//         {
//             if (_player.Abilities.Contains(PlayerAbility.AvailableTanks))
//             {
//                 return freeSlots;
//             }
//
//             // In the first biome the player can use only first 3 slots.
//             // There is no ability to split characters on tank line and dd+support.
//             return freeSlots.Where(x => x.IsTankLine);
//         }
//
//         private bool GetIsCharacterInGroup(Core.Heroes.Hero selectedCharacter)
//         {
//             return _player.Party.GetUnits().Contains(selectedCharacter);
//         }
//
//         private void InitSlotAssignmentButtons(Core.Heroes.Hero character, Player player)
//         {
//             _buttonList.Clear();
//
//             var isCharacterInGroup = GetIsCharacterInGroup(character);
//             if (isCharacterInGroup)
//             {
//                 var reserveButton = new ResourceTextButton(nameof(UiResource.MoveToThePoolButtonTitle));
//                 _buttonList.Add(reserveButton);
//
//                 reserveButton.OnClick += (_, _) =>
//                 {
//                     player.MoveToPool(character);
//
//                     InitSlotAssignmentButtons(character, player);
//                 };
//             }
//             else
//             {
//                 var freeSlots = player.Party.GetFreeSlots();
//                 var availableSlots = GetAvailableSlots(freeSlots);
//                 foreach (var slot in availableSlots)
//                 {
//                     var slotButton = new TextButton(slot.Index.ToString());
//
//                     _buttonList.Add(slotButton);
//
//                     slotButton.OnClick += (_, _) =>
//                     {
//                         player.MoveToParty(character, slot.Index);
//
//                         InitSlotAssignmentButtons(character, player);
//                     };
//                 }
//             }
//         }
//     }
// }

