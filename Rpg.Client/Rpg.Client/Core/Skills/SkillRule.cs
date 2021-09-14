using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Client.Core.Skills
{
    internal class SkillRule
    {
        public SkillDirection Direction { get; set; }
        public SkillScope Scope { get; set; }

        public SkillType Target { get; set; }
    }
}