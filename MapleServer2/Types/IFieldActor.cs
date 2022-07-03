using Maple2Storage.Types;
using MapleServer2.Managers;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public interface IFieldActor : IFieldObject
{
    public CoordF Velocity { get; }
    public short Animation { get; set; }

    public Stats Stats { get; }
    public bool IsDead { get; }

    public List<Status> Statuses { get; set; }
    public SkillCast SkillCast { get; }
    public bool OnCooldown { get; set; }
    public AdditionalEffects AdditionalEffects { get; }

    public FieldManager FieldManager { get; }
    public FieldNavigator Navigator { get; }

    public void Cast(SkillCast skillCast);
    public void Damage(DamageHandler damage, GameSession session);
    public void Heal(GameSession session, Status status, int amount);

    public void RecoverHp(int amount);
    public void ConsumeHp(int amount);
    public void RecoverSp(int amount);
    public void ConsumeSp(int amount);
    public void RecoverStamina(int amount);
    public void ConsumeStamina(int amount, bool noRegen = false);

    public void Perish();

    public void Animate(string sequenceName, float duration = -1);

    public void EffectAdded(AdditionalEffect effect);
    public void EffectRemoved(AdditionalEffect effect);
    public void InitializeEffects();

    public void ComputeStats();
    public void StatsComputed();
}

public interface IFieldActor<out T> : IFieldActor, IFieldObject<T>
{
}
