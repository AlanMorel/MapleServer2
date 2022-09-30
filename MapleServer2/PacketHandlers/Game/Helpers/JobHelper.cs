using MapleServer2.Enums;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class JobHelper
{
    public static bool CheckJobFlagForJob(List<JobCode> jobs, JobFlag jobFlag)
    {
        if (!jobs.Contains(JobCode.None) && jobFlag != JobFlag.All)
        {
            // TODO: Handle more than one flag.
            JobCode jobCode = jobFlag switch
            {
                JobFlag.Beginner => JobCode.Beginner,
                JobFlag.Knight => JobCode.Knight,
                JobFlag.Berserker => JobCode.Berserker,
                JobFlag.Wizard => JobCode.Wizard,
                JobFlag.Priest => JobCode.Priest,
                JobFlag.Archer => JobCode.Archer,
                JobFlag.HeavyGunner => JobCode.HeavyGunner,
                JobFlag.Thief => JobCode.Thief,
                JobFlag.Assassin => JobCode.Assassin,
                JobFlag.Runeblade => JobCode.Runeblade,
                JobFlag.Striker => JobCode.Striker,
                JobFlag.SoulBinder => JobCode.SoulBinder,
                _ => JobCode.None
            };

            if (!jobs.Contains(jobCode))
            {
                return false;
            }
        }

        return true;
    }
}
