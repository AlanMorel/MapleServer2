using MapleServer2.Enums;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class JobHelper
{
    public static bool CheckJobFlagForJob(List<Job> jobs, JobFlag jobFlag)
    {
        if (!jobs.Contains(Job.None) && jobFlag != JobFlag.All)
        {
            // TODO: Handle more than one flag.
            Job job = jobFlag switch
            {
                JobFlag.Beginner => Job.Beginner,
                JobFlag.Knight => Job.Knight,
                JobFlag.Berserker => Job.Berserker,
                JobFlag.Wizard => Job.Wizard,
                JobFlag.Priest => Job.Priest,
                JobFlag.Archer => Job.Archer,
                JobFlag.HeavyGunner => Job.HeavyGunner,
                JobFlag.Thief => Job.Thief,
                JobFlag.Assassin => Job.Assassin,
                JobFlag.Runeblade => Job.Runeblade,
                JobFlag.Striker => Job.Striker,
                JobFlag.SoulBinder => Job.SoulBinder,
                _ => Job.None
            };

            if (!jobs.Contains(job))
            {
                return false;
            }
        }

        return true;
    }
}
