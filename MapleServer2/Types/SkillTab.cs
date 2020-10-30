using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MaplePacketLib2.Tools;

namespace MapleServer2.Types {
    public class SkillTab : IByteSerializable {
        public long Id { get; set; }
        public string Name { get; private set; }
        public ICollection<SkillTabEntry> Skills => skills;

        private List<SkillTabEntry> skills;

        public SkillTab(string name, ICollection<SkillTabEntry> entries = null) {
            this.Name = name;
            this.skills = entries?.ToList() ?? new List<SkillTabEntry>();
        }

        public void AddOrUpdate(in SkillTabEntry skill) {
            // Iterate backwards for safe modification
            for (int i = skills.Count - 1; i >= 0; i--) {
                if (skills[i].SkillId == skill.SkillId) {
                    skills.RemoveAt(i);
                }
            }

            skills.Add(skill);
        }

        public void Rename(string name) {
            this.Name = name;
        }

        public void SetSkills(IEnumerable<SkillTabEntry> skills) {
            this.skills = skills.ToList();
        }

        public override string ToString() => $"SkillTab(Id:{Id},Name:{Name},Skills:{string.Join(",", Skills)})";

        public void WriteTo(IByteWriter writer) {
            writer.WriteLong(Id);
            writer.WriteUnicodeString(Name);

            writer.WriteInt(skills.Count);
            foreach (SkillTabEntry entry in skills) {
                writer.Write<SkillTabEntry>(entry);
            }
        }

        public void ReadFrom(IByteReader reader) {
            Id = reader.ReadLong();
            Name = reader.ReadUnicodeString();
            skills = new List<SkillTabEntry>();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++) {
                AddOrUpdate(reader.Read<SkillTabEntry>());
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public readonly struct SkillTabEntry {
        public readonly int SkillId;
        public readonly int Points;

        public SkillTabEntry(int skillId, int points) {
            this.SkillId = skillId;
            this.Points = points;
        }

        public override string ToString() => $"SkillTabEntry(SkillId:{SkillId},Points:{Points})";
    }
}