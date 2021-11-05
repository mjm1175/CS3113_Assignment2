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
    public Scene RoomScene { get; private set; }
    public bool IsCompleted { get; private set; }

    /// <summary>Create a new room</summary>
    /// <param name="roomScene">
    /// The scene name of the room
    /// </param>
    /// <param name="prereqRooms">The prerequisite rooms that have to be completed before entering this room</param>
    /// <exception cref="System.ArgumentException">
    /// When roomScene is invalid
    /// </exception>
    public Room(string roomScene, Room[] prereqRooms = null)
    {
        RoomScene = SceneManager.GetSceneByName(roomScene);
        if (RoomScene == null || !RoomScene.IsValid()) throw new ArgumentException("The room scene does not exist or is not valid");
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
        CurrentRoom = this;
        SceneManager.LoadScene(RoomScene.name);
    }
}
