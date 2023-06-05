using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core.AnimationFrameSets
{
    internal class CompositeAnimationFrameSet : IAnimationFrameSet
    {
        private readonly IReadOnlyList<IAnimationFrameSet> _animationSequence;
        private int _index;

        public CompositeAnimationFrameSet(IReadOnlyList<IAnimationFrameSet> animationSequence)
        {
            _animationSequence = animationSequence;

            var current = GetCurrentAnimationFrameSet();
            current.End += CurrentAnimationFrameSet_End;
        }

        public bool IsLoop { get; init; }

        private void CurrentAnimationFrameSet_End(object? sender, EventArgs e)
        {
            var current = GetCurrentAnimationFrameSet();
            current.Reset();
            current.End -= CurrentAnimationFrameSet_End;

            if (_index < _animationSequence.Count - 1)
            {
                _index++;
            }
            else
            {
                if (IsLoop)
                {
                    _index = 0;
                }
                else
                {
                    End?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private IAnimationFrameSet GetCurrentAnimationFrameSet()
        {
            return _animationSequence[_index];
        }

        public bool IsIdle { get; init; }

        public Rectangle GetFrameRect()
        {
            return GetCurrentAnimationFrameSet().GetFrameRect();
        }

        public void Reset()
        {
            _index = 0;

            foreach (var animationFrameSet in _animationSequence)
            {
                animationFrameSet.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            var current = GetCurrentAnimationFrameSet();
            current.Update(gameTime);
        }

        public event EventHandler? End;
        public event EventHandler<AnimationKeyFrameEventArgs>? KeyFrameHandled;
    }
}