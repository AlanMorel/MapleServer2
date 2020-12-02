using System.Numerics;
using Maple2.Data.Types;
using Maple2.Data.Utils;
using Maple2Storage.Enums;
using Maple2Storage.Types;
//using Maple2Storage.Utils;

namespace Maple2.Data.Converter {
    public class CharacterConverter : IModelConverter<Character, Maple2.Sql.Model.Character> {
        public Maple2.Sql.Model.Character ToModel(Character value, Maple2.Sql.Model.Character character) {
            if (value == null) return null;

            character ??= new Maple2.Sql.Model.Character();
            character.AccountId = value.AccountId;
            character.Id = value.Id;
            character.LastModified = value.LastModified;
            character.Name = value.Name;
            character.Gender = value.Gender;
            character.Job = (int) value.Job;
            character.Level = value.Level;
            character.SkinColor = value.SkinColor.Serialize();
            character.Experience = value.Experience;
            character.RestExperience = value.RestExperience;
            character.MapId = value.MapId;
            character.Title = value.Title;
            character.Insignia = value.Insignia;
            character.AttributePoints = value.AttributePoints.Serialize();

            character.ReturnMapId = value.ReturnMapId;
            character.ReturnPosition = value.ReturnPosition.Serialize();
            character.Mastery = value.Mastery.Serialize();
            if (value.GuildId != 0) {
                character.GuildId = value.GuildId;
            } else {
                character.GuildId = null;
            }

            character.SkillBook.MaxTabCount = value.MaxSkillTabs;
            character.SkillBook.ActiveTabId = value.ActiveSkillTabId;

            character.Currency.Mesos = value.Mesos;
            character.Currency.ValorToken = value.ValorToken;
            character.Currency.Treva = value.Treva;
            character.Currency.Rue = value.Rue;
            character.Currency.HaviFruit = value.HaviFruit;
            character.Currency.MesoToken = value.MesoToken;

            return character;
        }

        public Character FromModel(Maple2.Sql.Model.Character value) {
            if (value == null) return null;

            var character = new Character();
            character.AccountId = value.AccountId;
            character.Id = value.Id;
            character.LastModified = value.LastModified;
            character.Name = value.Name;
            character.CreationTime = value.CreationTime.ToEpochSeconds();
            character.Gender = value.Gender;
            character.Job = (JobType) value.Job;
            character.Level = value.Level;
            character.SkinColor = value.SkinColor.Deserialize<SkinColor>();
            character.Experience = value.Experience;
            character.RestExperience = value.RestExperience;
            character.MapId = value.MapId;
            character.Title = value.Title;
            character.Insignia = value.Insignia;
            character.AttributePoints = value.AttributePoints.Deserialize<AttributePoints>();
            character.GuildId = value.GuildId ?? 0;

            character.ReturnMapId = value.ReturnMapId;
            character.ReturnPosition = value.ReturnPosition.Deserialize<Vector3>();
            character.Mastery = value.Mastery.Deserialize<MasteryExp>();

            character.MaxSkillTabs = value.SkillBook.MaxTabCount;
            character.ActiveSkillTabId = value.SkillBook.ActiveTabId;

            character.Mesos = value.Currency.Mesos;
            character.ValorToken = value.Currency.ValorToken;
            character.Treva = value.Currency.Treva;
            character.Rue = value.Currency.Rue;
            character.HaviFruit = value.Currency.HaviFruit;
            character.MesoToken = value.Currency.MesoToken;

            return character;
        }
    }
}