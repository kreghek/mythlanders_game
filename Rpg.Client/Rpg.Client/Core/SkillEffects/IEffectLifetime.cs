using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal interface IEffectLifetime
    {
        void Update();

        event EventHandler? Disposed;

        string GetTextDescription();

        void MergeWith(IEffectLifetime effect);
        bool CanBeMerged();
    }
}