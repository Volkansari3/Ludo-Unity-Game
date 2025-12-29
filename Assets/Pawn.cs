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
        if (isFinished)
            return;

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
        // 3️⃣ START'TAN AYRILDI MI? (ilk defa start'tan çıkınca true olsun)
        if (currentTileIndex != owner.startTileIndex)
        {
            hasLeftStartThisLife = true;
        }

        // 4️⃣ FINISH KONTROLÜ (start'ı GEÇTİ mi?)
        if (hasLeftStartThisLife && !isFinished)
        {
            Debug.Log("FINISH CHECK ÇALIŞTI -> " + owner.playerName);

            int n = TileManager.Instance.tiles.Count;

            // currentTileIndex'ten startTileIndex'e ileri yönde kaç adım var?
            int forwardToStart = (owner.startTileIndex - currentTileIndex + n) % n;

            // Eğer forwardToStart 0 ise, şu an start'tayız demek. Bu durumda finish saymayalım.
            // Start'tan ayrıldıktan sonra tekrar start'a gelince veya geçince bitirsin:
            if (forwardToStart != 0 && forwardToStart <= steps)
            {

                Debug.Log(owner.playerName + " FINISHED (passed start)!");
                GameManager.Instance.FinishPawn(this);
                return;

            }
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

