using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishArea : MonoBehaviour
{
    public PlayerColor ownerColor;

    private List<Pawn> finishedPawns = new List<Pawn>();
    private Vector3[] slots;

    private void Awake()
    {
        CreateSlots();
    }

    void CreateSlots()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 center = rend.bounds.center;
        Vector3 size = rend.bounds.size;

        float spacing = size.x / 4f;

        // 1x4 dizilim (soldan saða)
        slots = new Vector3[4];

        for (int i = 0; i < 4; i++)
        {
            float xOffset = (-size.x / 2f) + spacing / 2f + i * spacing;
            slots[i] = new Vector3(
                center.x + xOffset,
                center.y + 0.1f,
                center.z
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pawn pawn = other.GetComponent<Pawn>();
        if (pawn == null) return;

        // Yanlýþ oyuncuysa alma
        if (pawn.owner.color != ownerColor) return;

        // Zaten bitmiþse tekrar alma
        if (pawn.isFinished) return;

        pawn.isFinished = true;
        pawn.isInPlay = false;

        finishedPawns.Add(pawn);

        int slotIndex = finishedPawns.Count - 1;

        if (slotIndex < slots.Length)
        {
            pawn.transform.position = slots[slotIndex];
        }

        GameManager.Instance.OnPawnFinished(pawn.owner);
    }

    public void AddPawn(Pawn pawn)
    {
        Debug.Log("AddPawn called on " + ownerColor);

        if (pawn.isFinished) return;

        pawn.isFinished = true;
        pawn.isInPlay = false;

        finishedPawns.Add(pawn);
        int slotIndex = finishedPawns.Count - 1;

        Debug.Log("Slot index: " + slotIndex);

        if (slotIndex < slots.Length)
        {
            pawn.transform.position = slots[slotIndex];
            Debug.Log("Pawn moved to slot");
        }
        else
        {
            Debug.Log("No slot available!");
        }

        GameManager.Instance.OnPawnFinished(pawn.owner);
    }


}
