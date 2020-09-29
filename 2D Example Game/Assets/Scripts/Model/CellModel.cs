using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellModel
{
    public bool isSpawnPoint;

    public enum State
    {
        Free,
        Occupied
    }

    public State state;

    public CellModel()
    {
        isSpawnPoint = false;
        state = State.Free;
    }
}
