using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The abstraction for the room object. It handles room generation and manages <c>RoomManager</c>.
/// </summary>
public class Room
{
    /// <value>We maintain a reference to all the rooms available</value>
    public static List<Room> CandidateRooms => candidates;
    /// <value>The most recent room the player entered</value>
    public static Room CurrentRoom { get; protected set; }

    public string RoomId { get; private set; }
    public string RoomScene { get; private set; }
    public bool IsCompleted { get; private set; }

    public int DoorCount { get; private set; }
    public RoomEdge[] ConnectedRoomEdges { get; private set; }

    private int _edgeCreated = 0;

    private static List<Room> candidates = new List<Room>();

    /// <summary>
    /// Register a series of rooms sequentially.
    /// </summary>
    /// <returns>The last room created in the sequence</returns>
    public static Room RegisterSequentialRooms(IEnumerable<string> roomIds)
    {
        if (roomIds.Count() == 0) return null;

        Room prevRoom = new Room(roomIds.First());
        foreach (string roomId in roomIds.Skip(1))
        {
            Room currentRoom = new Room(roomId);
            prevRoom.Connect(currentRoom, 1, 0);
            prevRoom = currentRoom;
        }
        return prevRoom;
    }

    public static void BulkConnectRooms(Room corridor, int startDoorIndex, IEnumerable<string> roomIds)
    {
        if (roomIds.Count() == 0) return;

        int doorIndex = startDoorIndex;
        foreach (string roomId in roomIds)
        {
            Room currentRoom = new Room(roomId);
            corridor.Connect(currentRoom, doorIndex++, 0);
        }
    }

    /// <summary>Enter the room by Room ID. Note that this will ignore the prerequisites</summary>
    public static void Enter(string RoomId)
    {
        Room room = CandidateRooms.Find(room => room.RoomId == RoomId);
        if (room == null) throw new ArgumentException($"The room {RoomId} cannot be found");
        room.Enter();
    }

    /// <summary>Create a new room</summary>
    /// <param name="roomId">
    /// The string id of the room
    /// </param>
    /// <param name="roomScene">
    /// The scene name of the room (same as <paramref name="roomId" /> when null)
    /// </param>
    public Room(string roomId, int doorCount = 2, string roomScene = null)
    {
        RoomId = roomId;
        RoomScene = roomScene ?? roomId;
        CandidateRooms.Add(this);
        DoorCount = doorCount;
        ConnectedRoomEdges = new RoomEdge[doorCount];
    }

    /// <summary>Mark this room as completed and return itself</summary>
    public Room Complete()
    {
        IsCompleted = true;
        return this;
    }

    /// <summary>get the upcoming rooms to choose from</summary>
    /// <returns>A list of rooms that have satisfied the prerequisites</returns>
    /// <exception cref="System.InvalidOperationException">
    /// When <c>Room.CandidateRooms</c> is empty
    /// </exception>
    public Room FindNextRoom()
    {
        var next = CandidateRooms
                        .FirstOrDefault();
        if (next == null) throw new InvalidOperationException("Not enough candidate rooms to choose from!");
        return next;
    }

    /// <summary>Enter the room and update the current room</summary>
    public void Enter(int doorIndex = 0)
    {
        PublicVars.LastEnteredDoorIndex = doorIndex;
        CurrentRoom = this;
        if (SceneManager.GetActiveScene().name != RoomScene)
        {
            SceneManager.LoadScene(RoomScene);
        }
    }

    public void EnterDoor(int doorIndex)
    {
        RoomEdge edge = ConnectedRoomEdges[doorIndex];
        if (!edge.IsTaken)
        {
            Debug.LogWarning($"Door {doorIndex} in {RoomId} is not associated with any other rooms");
            return;
        }
        edge.ConnectedRoom.Enter(edge.ConnectedDoorIndex);
    }

    /// <summary>Connect this room to another</summary>
    /// <param name="room">
    /// The room to be connected with
    /// </param>
    /// <param name="fromDoorIndex">
    /// The door to enter within the current room
    /// </param>
    /// <param name="toDoorIndex">
    /// The door where you will exit in the target room
    /// </param>
    public void Connect(Room room, int fromDoorIndex, int toDoorIndex = 0)
    {
        AddRoomEdge(room, fromDoorIndex, toDoorIndex);
        room.AddRoomEdge(this, toDoorIndex, fromDoorIndex);
    }

    protected void AddRoomEdge(Room room, int doorIndex, int connectedDoorIndex)
    {
        try
        {
            ValidateDoorIndex(doorIndex);
        }
        catch (ArgumentException e)
        {
            Debug.LogWarning(e);
            return;
        }
        RoomEdge edge = new RoomEdge(doorIndex, connectedDoorIndex, room);
        this.ConnectedRoomEdges[doorIndex] = edge;
        _edgeCreated++;
    }

    private void ValidateDoorIndex(int index)
    {
        if (index >= DoorCount) throw new ArgumentException($"{index} is not a valid index for the doors in {RoomId}");
        if (ConnectedRoomEdges[index].IsTaken) throw new ArgumentException($"{index} has already been taken for {RoomId}");
        if (_edgeCreated == DoorCount) throw new ArgumentException($"No more RoomEdges can be added to {RoomId}");
    }
}
