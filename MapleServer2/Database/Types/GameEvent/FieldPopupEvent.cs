namespace MapleServer2.Database.Types;

public class FieldPopupEvent
{
    public int Id { get; set; }
    public int MapId { get; set; }

    public FieldPopupEvent() { }

    public FieldPopupEvent(dynamic id, dynamic mapId)
    {
        Id = id;
        MapId = mapId;
    }
}
