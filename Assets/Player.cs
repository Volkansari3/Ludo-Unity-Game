using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public bool isBot;
    public Transform spawnPoint;
    public int startTileIndex;
    public PlayerColor color;

    public List<Pawn> pawns = new List<Pawn>();
}

