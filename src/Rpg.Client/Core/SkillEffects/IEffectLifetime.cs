using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal interface IEffectLifetime
    {
        bool CanBeMerged();

        string GetTextDescription();

        void MergeWith(IEffectLifetime effect);
        void Update();

        event EventHandler? Disposed;
    }
}