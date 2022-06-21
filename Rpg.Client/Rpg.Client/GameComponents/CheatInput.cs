using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;

namespace Rpg.Client.GameComponents
{
    internal sealed class CheatInput : DrawableGameComponent
    {
        private readonly Texture2D _backgroundTexture;
        private readonly StringBuilder _currentText;
        private readonly IEventCatalog _eventCatalog;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private double? _errorCounter;
        private string? _errorText;
        private KeyboardState _lastState;

        public CheatInput(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont) : base(game)
        {
            _currentText = new StringBuilder();
            _spriteFont = spriteFont;
            _spriteBatch = spriteBatch;

            var data = new[] { Color.Black };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();
        }

        public static bool IsCheating { get; private set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (IsCheating)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, 20),
                    Color.White);
                _spriteBatch.DrawString(_spriteFont, _currentText, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }

            if (!IsCheating && _errorCounter != null)
            {
                _errorCounter -= gameTime.ElapsedGameTime.TotalSeconds;

                if (_errorCounter <= 0)
                {
                    _errorCounter = null;
                    _errorText = null;
                    return;
                }

                _spriteBatch.Begin();
                _spriteBatch.DrawString(_spriteFont, _errorText, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (CheckIsKeyPressed(keyboardState, _lastState, Keys.OemTilde))
            {
                IsCheating = true;
            }

            if (IsCheating)
            {
                if (TryConvertKeyboardInput(keyboardState, _lastState, out var key))
                {
                    _currentText.Append(key);
                }
                else if (CheckIsKeyPressed(keyboardState, _lastState, Keys.Back) && _currentText.Length > 0)
                {
                    _currentText.Remove(_currentText.Length - 1, 1);
                }
                else if (CheckIsKeyPressed(keyboardState, _lastState, Keys.Enter))
                {
                    var currentText = _currentText.ToString();
                    if (!TryParseCommand(currentText))
                    {
                        // Show error
                        _errorText = $"[ERROR]: {currentText}";
                        _errorCounter = 10;
                    }

                    IsCheating = false;
                    _currentText.Clear();
                }
            }

            _lastState = keyboardState;
        }

        private static bool CheckIsKeyPressed(KeyboardState keyboard, KeyboardState oldKeyboard, Keys keys)
        {
            return oldKeyboard.IsKeyDown(keys) && keyboard.IsKeyUp(keys);
        }

        private static SystemEventMarker GetSystemMarker(string unitSchemeSid)
        {
            return unitSchemeSid switch
            {
                "zoologist" => SystemEventMarker.MeetZoologist,

                "archer" => SystemEventMarker.MeetArcher,
                "herbalist" => SystemEventMarker.MeetHerbalist,

                "monk" => SystemEventMarker.MeetMonk,
                "spearman" => SystemEventMarker.MeetSpearman,
                "missionary" => SystemEventMarker.MeetMissionary,

                "priest" => SystemEventMarker.MeetPriest,
                "Medjay" => SystemEventMarker.MeetMedjay,
                "liberator" => SystemEventMarker.MeetLiberator,

                "hoplite" => SystemEventMarker.MeetHoplite,
                "amazon" => SystemEventMarker.MeetAmazon,
                "engineer" => SystemEventMarker.MeetEngineer,

                _ => throw new InvalidOperationException($"Unknown unit {unitSchemeSid}")
            };
        }

        private static UnitScheme GetUnitSchemeByString(string unitSchemeSid, IUnitSchemeCatalog unitSchemeCatalog)
        {
            return unitSchemeSid switch
            {
                "comissar" => unitSchemeCatalog.Heroes[UnitName.Comissar],
                "assaulter" => unitSchemeCatalog.Heroes[UnitName.Assaulter],
                "zoologist" => unitSchemeCatalog.Heroes[UnitName.Zoologist],

                "warrior" => unitSchemeCatalog.Heroes[UnitName.Swordsman],
                "archer" => unitSchemeCatalog.Heroes[UnitName.Archer],
                "herbalist" => unitSchemeCatalog.Heroes[UnitName.Herbalist],

                "monk" => unitSchemeCatalog.Heroes[UnitName.Monk],
                "spearman" => unitSchemeCatalog.Heroes[UnitName.Spearman],
                "missionary" => unitSchemeCatalog.Heroes[UnitName.Sage],

                "priest" => unitSchemeCatalog.Heroes[UnitName.Priest],
                "medjay" => unitSchemeCatalog.Heroes[UnitName.Medjay],
                "liberator" => unitSchemeCatalog.Heroes[UnitName.Liberator],

                "hoplite" => unitSchemeCatalog.Heroes[UnitName.Hoplite],
                "amazon" => unitSchemeCatalog.Heroes[UnitName.Amazon],
                "engineer" => unitSchemeCatalog.Heroes[UnitName.Engineer],

                _ => throw new InvalidOperationException($"Unknown unit {unitSchemeSid}")
            };
        }

        private void HandleAddUnit(string[] args)
        {
            var globeProvider = Game.Services.GetService<GlobeProvider>();
            var globe = globeProvider.Globe;

            var unitSchemeSid = args[0];
            var unitScheme = GetUnitSchemeByString(unitSchemeSid, _unitSchemeCatalog);

            var poolUnitList = new List<Unit>(globe.Player.Pool.Units);
            globe.Player.Pool.Units = poolUnitList;

            const int DEFAULT_LEVEL = 1;
            var unit = new Unit(unitScheme, DEFAULT_LEVEL)
            {
                IsPlayerControlled = true
            };

            poolUnitList.Add(unit);

            // Events
            var targetSystemMarker = GetSystemMarker(unitSchemeSid);
            //var characterEvent = _eventCatalog.Events.SingleOrDefault(x => x.SystemMarker == targetSystemMarker);
            // if (characterEvent is not null)
            // {
            //     // Simulate the event resolving.
            //     characterEvent.Counter = 1;
            // }
        }

        private void HandleChangeHp(string[] args)
        {
            var globeProvider = Game.Services.GetService<GlobeProvider>();
            var globe = globeProvider.Globe;

            var unitSchemeSid = args[0];
            var unitScheme = GetUnitSchemeByString(unitSchemeSid, _unitSchemeCatalog);

            var targetUnit = globe.Player.GetAll().SingleOrDefault(x => x.UnitScheme == unitScheme);
            var hpAmount = int.Parse(args[1]);

            targetUnit.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value
                .CurrentChange(hpAmount > 0 ? hpAmount : 0);
        }

        private void HandleGainRes(string[] args)
        {
            var globeProvider = Game.Services.GetService<GlobeProvider>();
            var globe = globeProvider.Globe;

            var xpAmount = int.Parse(args[0]);
            var resType = Enum.Parse<EquipmentItemType>(args[1], ignoreCase: true);

            globe.Player.Inventory.Single(x => x.Type == resType).Amount += xpAmount;
        }

        private void HandleGainXp(string[] args)
        {
            HandleGainRes(new[] { args[0], nameof(EquipmentItemType.ExperiencePoints) });
        }

        private void HandleUpdateGlobe()
        {
            var globeProvider = Game.Services.GetService<GlobeProvider>();
            var globe = globeProvider.Globe;

            var dice = Game.Services.GetService<IDice>();
            globe.Update(dice, _eventCatalog);
        }

        /// <summary>
        /// Tries to convert keyboard input to characters and prevents repeatedly returning the
        /// same character if a key was pressed last frame, but not yet unpressed this frame.
        /// </summary>
        /// <param name="keyboard">The current KeyboardState</param>
        /// <param name="oldKeyboard">The KeyboardState of the previous frame</param>
        /// <param name="key">
        /// When this method returns, contains the correct character if conversion succeeded.
        /// Else contains the null, (000), character.
        /// </param>
        /// <returns>True if conversion was successful</returns>
        /// <remarks>
        /// https://roy-t.nl/2010/02/11/code-snippet-converting-keyboard-input-to-text-in-xna.html
        /// </remarks>
        private static bool TryConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            var shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                // @formatter:off — disable formatter after this line
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: key = shift ? 'A' : 'a'; return true;
                    case Keys.B: key = shift ? 'B' : 'b'; return true;
                    case Keys.C: key = shift ? 'C' : 'c'; return true;
                    case Keys.D: key = shift ? 'D' : 'd'; return true;
                    case Keys.E: key = shift ? 'E' : 'e'; return true;
                    case Keys.F: key = shift ? 'F' : 'f'; return true;
                    case Keys.G: key = shift ? 'G' : 'g'; return true;
                    case Keys.H: key = shift ? 'H' : 'h'; return true;
                    case Keys.I: key = shift ? 'I' : 'i'; return true;
                    case Keys.J: key = shift ? 'J' : 'j'; return true;
                    case Keys.K: key = shift ? 'K' : 'k'; return true;
                    case Keys.L: key = shift ? 'L' : 'l'; return true;
                    case Keys.M: key = shift ? 'M' : 'm'; return true;
                    case Keys.N: key = shift ? 'N' : 'n'; return true;
                    case Keys.O: key = shift ? 'O' : 'o'; return true;
                    case Keys.P: key = shift ? 'P' : 'p'; return true;
                    case Keys.Q: key = shift ? 'Q' : 'q'; return true;
                    case Keys.R: key = shift ? 'R' : 'r'; return true;
                    case Keys.S: key = shift ? 'S' : 's'; return true;
                    case Keys.T: key = shift ? 'T' : 't'; return true;
                    case Keys.U: key = shift ? 'U' : 'u'; return true;
                    case Keys.V: key = shift ? 'V' : 'v'; return true;
                    case Keys.W: key = shift ? 'W' : 'w'; return true;
                    case Keys.X: key = shift ? 'X' : 'x'; return true;
                    case Keys.Y: key = shift ? 'Y' : 'y'; return true;
                    case Keys.Z: key = shift ? 'Z' : 'z'; return true;

                    //Decimal keys
                    case Keys.D0: key = shift ? ')' : '0'; return true;
                    case Keys.D1: key = shift ? '!' : '1'; return true;
                    case Keys.D2: key = shift ? '@' : '2'; return true;
                    case Keys.D3: key = shift ? '#' : '3'; return true;
                    case Keys.D4: key = shift ? '$' : '4'; return true;
                    case Keys.D5: key = shift ? '%' : '5'; return true;
                    case Keys.D6: key = shift ? '^' : '6'; return true;
                    case Keys.D7: key = shift ? '&' : '7'; return true;
                    case Keys.D8: key = shift ? '*' : '8'; return true;
                    case Keys.D9: key = shift ? '(' : '9'; return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.OemTilde: key = shift ? '~' : '`'; return true;
                    case Keys.OemSemicolon: key = shift ? ':' : ';'; return true;
                    case Keys.OemQuotes: key = shift ? '"' : '\''; return true;
                    case Keys.OemQuestion: key = shift ? '?' : '/'; return true;
                    case Keys.OemPlus: key = shift ? '+' : '='; return true;
                    case Keys.OemPipe: key = shift ? '|' : '\\'; return true;
                    case Keys.OemPeriod: key = shift ? '>' : '.'; return true;
                    case Keys.OemOpenBrackets: key = shift ? '{' : '['; return true;
                    case Keys.OemCloseBrackets: key = shift ? '}' : ']'; return true;
                    case Keys.OemMinus: key = shift ? '_' : '-'; return true;
                    case Keys.OemComma: key = shift ? '<' : ','; return true;
                    case Keys.Space: key = ' '; return true;
                    default:
                        // Ignore all other keys.
                        key = (char)0;
                        return false;
                }
                // @formatter:on — enable formatter after this line
            }

            key = (char)0;
            return false;
        }

        private bool TryParseCommand(string currentText)
        {
            var cheatParts = currentText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!cheatParts.Any())
            {
                return false;
            }

            var command = cheatParts[0];
            var commandArgs = cheatParts.Skip(1).ToArray();
            try
            {
                switch (command)
                {
                    case "add-unit":
                        HandleAddUnit(commandArgs);
                        break;

                    case "gain-xp":
                        HandleGainXp(commandArgs);
                        break;

                    case "gain-res":
                        HandleGainRes(commandArgs);
                        break;

                    case "change-hp":
                        HandleChangeHp(commandArgs);
                        break;

                    case "update-globe":
                        HandleUpdateGlobe();
                        break;

                    case "create-combat":
                        HandleCreateCombat(commandArgs);
                        break;

                    default:
                        return false;
                }
            }
            catch (Exception ex) when (
                ex is InvalidOperationException or FormatException or InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        private void HandleCreateCombat(string[] commandArgs)
        {
            if (!Enum.TryParse<GlobeNodeSid>(commandArgs[1], out var locationSid))
            {
                throw new InvalidOperationException($"Invalid location sid {commandArgs[1]}.");
            }

            var globeProvider = Game.Services.GetService<GlobeProvider>();
            var globe = globeProvider.Globe;
            var targetNode = globe.Biomes.SelectMany(x => x.Nodes).SingleOrDefault(x => x.Sid == locationSid);
            if (targetNode is null)
            {
                throw new InvalidOperationException($"Location {locationSid} not found.");
            }

            globe.AddCombat(targetNode);

            var monsterArgs = commandArgs.Skip(1).ToArray();
            for (int i = 0; i < monsterArgs.Length; i++)
            {
                var monsterInfo = monsterArgs[i];

                switch (monsterInfo)
                {
                    case "-": continue;
                    case "a":
                        var unit = new Unit(
                            _unitSchemeCatalog.AllMonsters.SingleOrDefault(x => x.Name == UnitName.Aspid), 1);
                        var combat = targetNode.CombatSequence.Combats.ToArray()[0];
                        globe.AddMonster(combat, unit, i);
                        break;
                }
            }
        }
    }
}