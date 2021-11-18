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
            prevRoom = new Room(roomId);
        }
        return prevRoom;
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

    /// <summary>Enter the room update the current room</summary>
    public void Enter()
    {
        CurrentRoom = this;
        if (SceneManager.GetActiveScene().name != RoomScene)
        {
            SceneManager.LoadScene(RoomScene);
        }
    }

    public void EnterDoor(int doorIndex)
    {
        try
        {
            ConnectedRoomEdges[doorIndex].ConnectedRoom.Enter();
        }
        catch (NullReferenceException)
        {
            throw new ArgumentException($"Door {doorIndex} in {RoomId} is not associated with any other rooms");
        }
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
    public void Connect(Room room, int fromDoorIndex, int toDoorIndex)
    {
        AddRoomEdge(room, fromDoorIndex);
        room.AddRoomEdge(this, toDoorIndex);
    }

    protected void AddRoomEdge(Room room, int doorIndex)
    {
        ValidateDoorIndex(doorIndex);
        RoomEdge edge = new RoomEdge(doorIndex, room);
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
