using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public int currentTileIndex = -1; // -1 = spawn
    public bool isInPlay = false;
    public bool isFinished = false;

    public Player owner;
    public Vector3 spawnPosition;

    // Bu çıkışta start'tan ayrıldı mı?
    public bool hasLeftStartThisLife = false;

    public void Move(int steps, int startIndex)
    {
        // 1️⃣ SPAWN'DAYSA
        if (!isInPlay)
        {
            if (steps == 6)
            {
                currentTileIndex = startIndex;
                isInPlay = true;
                hasLeftStartThisLife = false;

                Vector3 spawnTargetPos =
                    TileManager.Instance.tiles[currentTileIndex].position;

                Vector3 spawnDirection = spawnTargetPos - transform.position;
                spawnDirection.y = 0;

                if (spawnDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(spawnDirection);
                }

                transform.position = spawnTargetPos;
            }
            return;
        }


        // 2️⃣ HEDEF TILE HESAPLA
        int targetIndex =
            (currentTileIndex + steps) % TileManager.Instance.tiles.Count;

        // 3️⃣ START'TAN AYRILDI MI?
        if (currentTileIndex != owner.startTileIndex)
        {
            hasLeftStartThisLife = true;
        }

        // 4️⃣ FINISH KONTROLÜ
        if (
            hasLeftStartThisLife &&
            !isFinished &&
            targetIndex == owner.startTileIndex
        )
        {
            isFinished = true;
            Debug.Log(owner.playerName + " FINISHED!");
            GameManager.Instance.FinishPawn(this);
            return;
        }

        // 5️⃣ NORMAL İLERLEME
        // 5️⃣ NORMAL İLERLEME

        Vector3 currentPos = transform.position;
        Vector3 targetPos = TileManager.Instance.tiles[targetIndex].position;

        // YÖNÜ HESAPLA
        Vector3 direction = targetPos - currentPos;
        direction.y = 0;

        // YÖNE DÖN
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // POZİSYONU GÜNCELLE
        currentTileIndex = targetIndex;
        transform.position = targetPos;

    }
}

