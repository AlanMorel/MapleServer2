using MapleServer2.Database;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public class AuthData
{
    public readonly int TokenA;
    public readonly int TokenB;

    public readonly long AccountId;
    public long OnlineCharacterId;

    public AuthData(long accountId)
    {
        AccountId = accountId;
        TokenA = GuidGenerator.Int();
        TokenB = GuidGenerator.Int();
        DatabaseManager.AuthData.Insert(this);
    }

    public AuthData(int tokenA, int tokenB, long accountId, long onlineCharacterId)
    {
        TokenA = tokenA;
        TokenB = tokenB;
        AccountId = accountId;
        OnlineCharacterId = onlineCharacterId;
    }
}
