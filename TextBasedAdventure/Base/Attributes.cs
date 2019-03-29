using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventure.Base
{
    public class AttributesBase
    {
        protected readonly Dictionary<string, int> Levels;

        public IEnumerable<string> Skills
        {
            get
            {
                return Levels.Keys;
            }
        }

        public AttributesBase()
        {
            Levels = new Dictionary<string, int>();
        }

        public AttributesBase(IEnumerable<string> SkillNames) : this()
        {
            foreach (var skill in SkillNames)
            {
                Levels[skill.ToLower()] = 0;
            }
        }

        public AttributesBase(IReadOnlyDictionary<string, int> Skills) : this()
        {
            foreach (var skill in Skills.Keys)
            {
                this.Levels[skill.ToLower()] = Skills[skill];
            }
        }

        public bool CheckSkill(string SkillName, int RequiredLevel)
        {
            var skillname = SkillName.ToLower();

            return Levels[skillname] >= RequiredLevel;
        }

        public bool Check(AttributesBase Challenge)
        {
            foreach(var Skill in Challenge.Levels.Keys)
            {
                if(! CheckSkill(Skill, Challenge.Levels[Skill])) return false;
            }

            return true;
        }
    }

    public sealed class Attributes : AttributesBase, ICloneable
    {
        public Attributes() : base()
        {

        }

        public Attributes(IEnumerable<string> SkillNames) : base(SkillNames)
        {

        }

        public Attributes(IReadOnlyDictionary<string, int> Skills) : base(Skills)
        {

        }

        public int this[string SkillName]
        {
            get
            {
                var skillname = SkillName.ToLower();

                if (Levels.ContainsKey(skillname))
                {
                    return Levels[skillname];
                }

                return 0;
            }
            set
            {
                var skillname = SkillName.ToLower();

                Levels[skillname] = value;
            }
        }

        public void LevelUp(string SkillName)
        {
            var skill = SkillName.ToLower();

            if (Levels.ContainsKey(skill))
            {
                Levels[skill]++;
            }
            else Levels[skill] = 1;
        }

        public object Clone()
        {
            return new Attributes(Levels);
        }

        public ReadOnlyAttributes AsReadOnly()
        {
            return new ReadOnlyAttributes(Levels);
        }
    }

    public sealed class ReadOnlyAttributes : AttributesBase, ICloneable
    {
        public ReadOnlyAttributes() : base()
        {

        }

        public ReadOnlyAttributes(IEnumerable<string> SkillNames) : base(SkillNames)
        {

        }

        public ReadOnlyAttributes(IReadOnlyDictionary<string, int> Skills) : base(Skills)
        {

        }

        public int this[string SkillName]
        {
            get
            {
                var skillname = SkillName.ToLower();

                if (Levels.ContainsKey(skillname))
                {
                    return Levels[skillname];
                }

                return 0;
            }
        }

        public object Clone()
        {
            return new ReadOnlyAttributes(Levels);
        }

        public Attributes AsMutable()
        {
            return new Attributes(Levels);
        }
    }
}
