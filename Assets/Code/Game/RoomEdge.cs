public struct RoomEdge
{
    private readonly bool _isTaken;
    public bool IsTaken => _isTaken;
    private readonly int _doorIndex;
    public int DoorIndex => _doorIndex;
    private readonly int _connectedDoorIndex;
    public int ConnectedDoorIndex => _connectedDoorIndex;
    private readonly Room _connectedRoom;
    public Room ConnectedRoom => _connectedRoom;

    public RoomEdge(int DoorIndex, int connectedDoorIndex, Room ConnectedRoom)
    {
        _doorIndex = DoorIndex;
        _connectedDoorIndex = connectedDoorIndex;
        _connectedRoom = ConnectedRoom;
        _isTaken = true;
    }
}