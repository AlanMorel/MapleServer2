using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public class HomeSurvey
{
    public readonly long Id;
    public string Question;
    public long OwnerId;
    public bool Started;
    public bool Ended;

    public readonly bool Public;

    // Dictionary<option , List of character names>
    public readonly Dictionary<string, List<string>> Options;

    public List<string> AvailableCharacters;
    public int Answers;
    public int MaxAnswers;

    public HomeSurvey() { }

    public HomeSurvey(string question, bool publicQuestion)
    {
        Question = question;
        Id = GuidGenerator.Long();
        Started = false;
        Ended = false;
        Options = new();
        Answers = 0;
        MaxAnswers = 0;
        OwnerId = 0;
        Public = publicQuestion;
    }

    public void Start(long characterId, List<IFieldActor<Player>> players)
    {
        OwnerId = characterId;
        Started = true;
        MaxAnswers = players.Count;
        AvailableCharacters = players.Select(x => x.Value.Name).ToList();
    }

    public void End()
    {
        Ended = true;
        Started = false;
        Question = null;
    }
}
