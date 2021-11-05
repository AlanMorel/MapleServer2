namespace MapleServer2.Types;

public class GatheringCount
{
    public int RecipeId;
    public int CurrentCount;
    public int MaxCount;

    public GatheringCount(int recipeId, int currentCount, int maxCount)
    {
        RecipeId = recipeId;
        CurrentCount = currentCount;
        MaxCount = maxCount;
    }
}
