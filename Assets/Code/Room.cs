using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The abstraction for the room object. It handles room generation and manages <c>RoomManager</c>.
/// </summary>
public class Room
{
    /// <value>We will select from the candidate rooms whenever one room is completed</value>
    public static List<Room> CandidateRooms { get; protected set; }
    /// <value>The most recent room the player entered</value>
    public static Room CurrentRoom { get; protected set; }

    public Room[] PrereqRooms { get; private set; }
    public RoomManager RoomPrefab { get; private set; }
    public bool IsCompleted { get; private set; }

    /// <summary>Create a new room</summary>
    /// <param name="prefabPath">
    /// The path to the prefab of the room relative to any <c>Resources</c> folder, which must have the <c>RoomManager</c> script
    /// </param>
    /// <param name="prereqRooms">The prerequisite rooms that have to be completed before entering this room</param>
    /// <exception cref="System.ArgumentException">
    /// When prefabPath is invalid
    /// </exception>
    public Room(string prefabPath, Room[] prereqRooms = null)
    {
        RoomPrefab = Resources.Load<RoomManager>(prefabPath);
        if (RoomPrefab == null) throw new ArgumentException("The room prefab does not exist or does not have a RoomManager");
        PrereqRooms = prereqRooms;
        CandidateRooms.Add(this);
    }

    /// <summary>Mark this room as completed and get the upcoming rooms to choose from</summary>
    /// <returns>A list of rooms that have satisfied the prerequisites</returns>
    /// <exception cref="System.InvalidOperationException">
    /// When <c>Room.CandidateRooms</c> is empty
    /// </exception>
    public List<Room> Complete(int doorCount)
    {
        IsCompleted = true;
        /// <TODO>Optimize this</TODO>
        var next = CandidateRooms
                        .Where(room => room.PrereqRooms == null || room.PrereqRooms.All(prereq => prereq.IsCompleted))
                        .Take(doorCount)
                        .ToList();
        if (next.Count == 0) throw new InvalidOperationException("Not enough candidate rooms to choose from!");
        return next;
    }

    /// <summary>Instantiate the room at the door and update the current room</summary>
    public void Enter(Door door)
    {
        CandidateRooms.Remove(this);
        CreateRoom(door);
        CurrentRoom = this;
    }

    /// <summary>Instantiate the room GameObject using prefab</summary>
    public RoomManager CreateRoom(Door door)
    {
        /// <TODO>Properly configure the anchor point to instantiate the room at the door</TODO>
        return GameObject.Instantiate(RoomPrefab);
    }
}
