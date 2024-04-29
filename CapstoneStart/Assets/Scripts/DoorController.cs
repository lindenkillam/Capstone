using System.Collections;
using UnityEngine;

[System.Serializable]
public class Door
{
    public bool requireKey = false;
    public bool isOpened = false;
    public bool guestRoomDoor, specialRoomDoor;
}

public class DoorController : MonoBehaviour
{
    public Door door = new Door();
}
