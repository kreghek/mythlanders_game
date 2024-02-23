using CombatDicesTeam.Combats;

using Core.Props;

namespace Client.Core;

internal sealed record TradeOffer(IProp Prop, IStatValue MoneyCost);