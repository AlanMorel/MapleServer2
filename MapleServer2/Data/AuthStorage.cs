using MapleServer2.Types;

namespace MapleServer2.Data;

// TODO: This is mostly temporary while I think about how auth really should work
// It's mostly just required to pass login data to GameSession (which is why it's static)
public static class AuthStorage
{
    private static readonly Dictionary<long, AuthData> tokenStorage = new();

    public static AuthData GetData(long accountId)
    {
        return tokenStorage.GetValueOrDefault(accountId);
    }

    public static void SetData(long accountId, AuthData data)
    {
        tokenStorage[accountId] = data;
    }
}
public class AuthData
{
    public int TokenA;
    public int TokenB;
    public Player Player;
}
