using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class ChildEscapePositions : GameSystem
{
    [SerializeField] private List<Transform> escapePositions;
    [SerializeField] private List<Transform> escapePositionsSecond;

    private int lastEscapeId = 0;
    private int lastEscapeIdSecond = 0;

    public Transform GetEscapePosition(bool isSecond)
    {
        if (isSecond == false)
        {
            lastEscapeId++;
            if (lastEscapeId >= escapePositions.Count) lastEscapeId = 0;
            return escapePositions[lastEscapeId];
        }
        else
        {
            lastEscapeIdSecond++;
            if (lastEscapeIdSecond >= escapePositionsSecond.Count) lastEscapeIdSecond = 0;
            return escapePositionsSecond[lastEscapeIdSecond];
        }
    }
}