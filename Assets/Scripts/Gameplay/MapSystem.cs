using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public int combatRoomChance = 70;
    public int builderRoomChance = 15;
    public int healRoomChance = 10;
    public int chestRoomChance = 5;

    private static int _id = 0; // Guaranted unique room id

    public struct Map
    {
        public List<Room> rooms;
        public List<Link> links;
        private Dictionary<RoomType, int> roomTypesCount;

        public Map(Room start)
        {
            rooms = new List<Room>() { start};
            links = new List<Link>();
            roomTypesCount = new Dictionary<RoomType, int>();
            foreach(RoomType rt in System.Enum.GetValues(typeof(RoomType)))
            {
                roomTypesCount.Add(rt, 0);
            }
        }

        // Add a room to the map
        public void AddRoom(Room room)
        {
            rooms.Add(room);
            roomTypesCount[room.roomType]++;
        }

        // Add a link from one room to another
        public void AddLink(Link link)
        {
            links.Add(link);
        }
        public void AddLink(int src, int trgt)
        {
            links.Add(new Link(src, trgt));
        }

        // Get all rooms connected forward to the one given in parameter
        public List<int> GetNextRooms(int roomId)
        {
            List<int> res = new List<int>();

            foreach (Link l in this.links)
            {
                if (l.source == roomId)
                {
                    res.Add(l.target);
                }
            }

            return res;
        }
        // Get all rooms connected backward to the one given in parameter
        public List<int> GetPreviousRooms(int roomId)
        {
            List<int> res = new List<int>();

            foreach (Link l in this.links)
            {
                if (l.target == roomId)
                {
                    res.Add(l.source);
                }
            }

            return res;
        }

        public void DebugNbRoomTypes()
        {
            foreach (RoomType rt in System.Enum.GetValues(typeof(RoomType)))
            {
                Debug.Log(rt.ToString() + " " + roomTypesCount[rt]);
            }
        }
    }


    // The various types of room
    public enum RoomType
    {
        Combat,
        Heal,
        Builder,
        Chest,
        Boss
    }
    public struct Room
    {
        public Room(RoomType rt) // Must use this constructor to get a unique ID for each room
        {
            id = _id++;
            roomType = rt;
        }

        public int id;
        public RoomType roomType;
    }

    // A link goes from a room to another based on the ID
    public struct Link
    {
        public Link(int src, int trgt)
        {
            source = src;
            target = trgt;
        }

        public int source;
        public int target;
    }


    public RoomType GenerateRandomRoomType()
    {
        int randomRT = Random.Range(0, 100) + 1;
        RoomType rt;
        randomRT -= combatRoomChance;
        if (randomRT <= 0)
            rt = RoomType.Combat;
        else
        {
            randomRT -= builderRoomChance;
            if (randomRT <= 0)
                rt = RoomType.Builder;
            else
            {
                randomRT -= healRoomChance;
                if (randomRT <= 0)
                    rt = RoomType.Heal;
                else
                    rt = RoomType.Chest;
            }
        }

        return rt;
    }



    // maxFloors is the number of rooms you need to go from start to boss (ex : 3 means you need to go through 3 rooms before going into the boss room)
    // maxWidth is the maximum numbers of rooms at one depth 
    public Map GenerateMap(int maxFloors, int maxWidth = 3)
    {
        // Always start with a combat room
        Room start = new Room(RoomType.Combat);

        // Use custom initializer
        Map map = new Map(start);


        int currentFloor = 1;
        List<int> roomsOnCurrentFloor = new List<int> { start.id };

        while (currentFloor < maxFloors)
        {
            //int currentWidth = roomsOnCurrentFloor.Count;

            // Create the next rooms
            List<int> roomsOnNextFloor = new List<int>();
            int nbRoomsNextFloor = Random.Range(0, maxWidth) + 1;
            for(int i=0; i<nbRoomsNextFloor; i++)
            {
                RoomType newRT = GenerateRandomRoomType(); // The type of the new room
                Room newRoom = new Room(newRT);

                map.AddRoom(newRoom);
                roomsOnNextFloor.Add(newRoom.id);
            }

            // Create links between old rooms and new rooms
            // First we make sure that each room of the current floor is connected to at least one room (no dead-ends)
            foreach(int currMap in roomsOnCurrentFloor)
            {
                map.AddLink(currMap, roomsOnNextFloor[Random.Range(0, roomsOnNextFloor.Count)]);
            }
            // We now want to make sure that each new map is connected to at least one previous map
            foreach (int newMap in roomsOnNextFloor)
            {
                if (map.GetPreviousRooms(newMap).Count == 0)
                {
                    map.AddLink(roomsOnCurrentFloor[Random.Range(0, roomsOnCurrentFloor.Count)], newMap);
                }
            }

            roomsOnCurrentFloor = new List<int>(roomsOnNextFloor);
            currentFloor++;
        }

        Room bossRoom = new Room(RoomType.Boss);
        map.AddRoom(bossRoom);
        foreach (int currMap in roomsOnCurrentFloor)
        {
            map.AddLink(currMap, bossRoom.id);
        }



        // Debug info
        map.DebugNbRoomTypes();

        return map;
    }

    void Start()
    {
        GenerateMap(10);
    }
}
