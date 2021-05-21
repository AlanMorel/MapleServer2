namespace MapleServer2.Types
{
    public class DamageHandler
    {
        // TODO: This Class is WIP, all the calculation made here is for testing purposes
        private double Damage { get; set; }
        private bool IsCrit { get; set; }

        private DamageHandler(double damage, bool isCrit)
        {
            Damage = damage;
            IsCrit = isCrit;
        }

        private static DamageHandler HandleCrit(SkillCast skillCast)
        {
            double damage = skillCast.GetDamageRate();
            bool crit = SkillCast.RollCrit();
            if (crit)
            {
                damage += damage;
            }
            DamageHandler toReturn = new DamageHandler(damage, crit);
            return toReturn;
        }

        public static DamageHandler CalculateSkillDamage(SkillCast skillCast) => HandleCrit(skillCast);

        // TODO: Calculate Damage properly 
        public double GetDamage() => Damage * 100;

        public bool IsCritical()
        {
            return IsCrit;
        }
    }
}
