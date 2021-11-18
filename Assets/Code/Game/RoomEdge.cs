public struct RoomEdge
{
    private readonly bool _isTaken;
    public bool IsTaken => _isTaken;
    private readonly int _doorIndex;
    public int DoorIndex => _doorIndex;
    private readonly Room _connectedRoom;
    public Room ConnectedRoom => _connectedRoom;

    public RoomEdge(int DoorIndex, Room ConnectedRoom)
    {
        _doorIndex = DoorIndex;
        _connectedRoom = ConnectedRoom;
        _isTaken = true;
    }
}