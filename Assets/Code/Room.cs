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
    /// <value>We will select from the candidate rooms whenever one room is completed</value>
    public static List<Room> CandidateRooms => candidates;
    /// <value>The most recent room the player entered</value>
    public static Room CurrentRoom { get; protected set; }

    public string RoomId { get; private set; }
    public Room[] PrereqRooms { get; private set; }
    public string RoomScene { get; private set; }
    public bool IsCompleted { get; private set; }

    private static List<Room> candidates = new List<Room>();

    /// <summary>Enter the room by Room ID update the current room</summary>
    public static void Enter(string RoomId)
    {
        CandidateRooms.Find(room => room.RoomId == RoomId).Enter();
    }

    /// <summary>Create a new room</summary>
    /// <param name="roomId">
    /// The string id of the room
    /// </param>
    /// <param name="roomScene">
    /// The scene name of the room (same as <paramref name="roomId" /> when null)
    /// </param>
    /// <param name="prereqRooms">The prerequisite rooms that have to be completed before entering this room</param>
    public Room(string roomId, string roomScene = null, Room[] prereqRooms = null)
    {
        RoomId = roomId;
        RoomScene = roomScene ?? roomId;
        PrereqRooms = prereqRooms;
        CandidateRooms.Insert(UnityEngine.Random.Range(0, CandidateRooms.Count), this);
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
        /// <TODO>Optimize this</TODO>
        var next = CandidateRooms
                        .Where(room => room.PrereqRooms == null || room.PrereqRooms.All(prereq => prereq.IsCompleted))
                        .First();
        if (next == null) throw new InvalidOperationException("Not enough candidate rooms to choose from!");
        return next;
    }

    /// <summary>Enter the room update the current room</summary>
    public void Enter()
    {
        CandidateRooms.Remove(this);
        CurrentRoom = this;
        if (SceneManager.GetActiveScene().name != RoomScene)
        {
            SceneManager.LoadScene(RoomScene);
        }
    }
}
