using Maple2Storage.Types;

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

    public void MoveBy(CoordF displacement);
    public void MoveTo(CoordF to);
    public void Cast(SkillCast skillCast);
    public void Damage(DamageHandler damage);

    public void RecoverHp(int amount);
    public void ConsumeHp(int amount);
    public void RecoverSp(int amount);
    public void ConsumeSp(int amount);
    public void RecoverStamina(int amount);
    public void ConsumeStamina(int amount);

    public abstract void Perish();
}

public interface IFieldActor<out T> : IFieldActor, IFieldObject<T> { }
