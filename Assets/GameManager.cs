using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Pawn pawnPrefabCowboy;
    public Pawn pawnPrefabAshigaru;  // Ashigaru
    public Pawn pawnPrefabPolice;    // Police Officer
    public Pawn pawnPrefabSkeleton;   // Skeleton
    

    public GameObject rollDiceButton;

    public Transform redSpawn;
    public Transform blueSpawn;
    public Transform greenSpawn;
    public Transform yellowSpawn;

    public Material redPawnMat;
    public Material bluePawnMat;
    public Material greenPawnMat;
    public Material yellowPawnMat;

    public FinishArea redFinishArea;
    public FinishArea blueFinishArea;
    public FinishArea greenFinishArea;
    public FinishArea yellowFinishArea;

    public TextMeshProUGUI turnText;



    public List<Player> players = new List<Player>();

    int currentPlayerIndex = 0;
    Dice dice = new Dice();
    int sixRollCount = 0;

    bool waitingForInput = true;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreatePlayers();
        SpawnPawns();

        FinishArea[] areas = FindObjectsOfType<FinishArea>();

        foreach (FinishArea area in areas)
        {
            switch (area.ownerColor)
            {
                case PlayerColor.Red:
                    redFinishArea = area;
                    break;
                case PlayerColor.Blue:
                    blueFinishArea = area;
                    break;
                case PlayerColor.Green:
                    greenFinishArea = area;
                    break;
                case PlayerColor.Yellow:
                    yellowFinishArea = area;
                    break;
            }
        }

        Player firstPlayer = players[currentPlayerIndex];

        if (turnText != null)
        {
            if (firstPlayer.isBot)
                turnText.text = "Turn: " + firstPlayer.playerName;
            else
                turnText.text = "Your Turn";
        }

    }

    void CreatePlayers()
    {
        players.Clear();

        players.Add(new Player
        {
            playerName = "Player Cowboy",
            isBot = false,
            spawnPoint = redSpawn,
            startTileIndex = 0,
            color = PlayerColor.Red
        });

        players.Add(new Player
        {
            playerName = "Player Police Officer",
            isBot = true,
            spawnPoint = greenSpawn,
            startTileIndex = 13,
            color = PlayerColor.Blue
        });

        players.Add(new Player
        {
            playerName = "Player Skeleton",
            isBot = true,
            spawnPoint = yellowSpawn,
            startTileIndex = 26,
            color = PlayerColor.Green
        });

        players.Add(new Player
        {
            playerName = "Player Ashigaru",
            isBot = true,
            spawnPoint = blueSpawn,
            startTileIndex = 39,
            color = PlayerColor.Yellow
        });
    }


    void SpawnPawns()
    {
        foreach (Player player in players)
        {
            Transform plane = player.spawnPoint.GetChild(0);
            Renderer planeRenderer = plane.GetComponent<Renderer>();

            Vector3 center = planeRenderer.bounds.center;
            Vector3 size = planeRenderer.bounds.size;

            float offsetX = size.x * 0.25f;
            float offsetZ = size.z * 0.25f;

            Vector3[] positions = new Vector3[]
            {
            center + new Vector3(-offsetX, 0.1f, -offsetZ),
            center + new Vector3(-offsetX, 0.1f,  offsetZ),
            center + new Vector3( offsetX, 0.1f, -offsetZ),
            center + new Vector3( offsetX, 0.1f,  offsetZ),
            };

            for (int i = 0; i < 4; i++)
            {
                Pawn pawnPrefabToUse = GetPawnPrefab(player);

                Pawn pawn = Instantiate(
                    pawnPrefabToUse,
                    positions[i],
                    Quaternion.identity,
                    GameObject.Find("Pawns").transform
                );

                pawn.owner = player;
                pawn.spawnPosition = positions[i];

                player.pawns.Add(pawn);
            }
        }
    }


    void NextTurn()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        waitingForInput = true;
        sixRollCount = 0;

        Player nextPlayer = players[currentPlayerIndex];
        Debug.Log("Turn: " + nextPlayer.playerName);

        if (turnText != null)
        {
            if (nextPlayer.isBot)
                turnText.text = "Turn: " + nextPlayer.playerName;
            else
                turnText.text = "Your Turn";
        }


        // Buton sadece gerçek oyuncuda açýk olsun
        rollDiceButton.SetActive(!nextPlayer.isBot);

        if (nextPlayer.isBot)
        {
            Invoke(nameof(BotTurn), 1f);
        }
    }


    void BotTurn()
    {
        Player bot = players[currentPlayerIndex];
        TakeTurn(bot);
    }

    Pawn GetPawnToMove(Player player, int roll)
    {
        // 6 geldiyse, spawn'da pawn varsa onu çýkar
        if (roll == 6)
        {
            foreach (Pawn pawn in player.pawns)
            {
                if (!pawn.isInPlay)
                {
                    return pawn;
                }
            }
        }

        // Aksi halde oyunda olan ilk pawn'u oynat
        foreach (Pawn pawn in player.pawns)
        {
            if (pawn.isInPlay)
            {
                return pawn;
            }
        }

        // Hiç oynayacak pawn yoksa
        return null;
    }


    void TakeTurn(Player player)
    {
        waitingForInput = false;

        int roll = dice.Roll();
        Debug.Log(player.playerName + " rolled " + roll);

        Pawn pawnToMove = GetPawnToMove(player, roll);

        if (pawnToMove != null)
        {
            pawnToMove.Move(roll, player.startTileIndex);
            CheckPawnCollision(pawnToMove);
        }

        if (roll == 6)
        {
            sixRollCount++;

            if (sixRollCount >= 2)
            {
                sixRollCount = 0;
                NextTurn();
            }
            else
            {
                // 6 geldi ama tekrar oynama
                if (player.isBot)
                {
                    Invoke(nameof(BotTurn), 1f); // BOT otomatik devam
                }
                else
                {
                    waitingForInput = true; // PLAYER bekler
                }
            }
        }
        else
        {
            sixRollCount = 0;
            NextTurn();
        }
    }

    void CheckPawnCollision(Pawn movedPawn)
    {
        foreach (Player player in players)
        {
            if (player == movedPawn.owner)
                continue;

            foreach (Pawn otherPawn in player.pawns)
            {
                if (!otherPawn.isInPlay)
                    continue;

                if (otherPawn.currentTileIndex == movedPawn.currentTileIndex)
                {
                    // Safe tile kontrolü
                    if (TileManager.Instance.safeTileIndexes.Contains(movedPawn.currentTileIndex))
                    {
                        Debug.Log("Safe tile - no capture");
                        return;
                    }

                    SendPawnToSpawn(otherPawn);
                }

            }
        }
    }

    void SendPawnToSpawn(Pawn pawn)
    {
        pawn.isInPlay = false;
        pawn.currentTileIndex = -1;

        // Spawn plane merkezine gönder
        pawn.hasLeftStartThisLife = false; // bu hayat bitti
        pawn.transform.position = pawn.spawnPosition;

        Debug.Log(pawn.owner.playerName + " pawn was eaten!");
    }

    public void OnPawnFinished(Player player)
    {
        int finishedCount = 0;

        foreach (Pawn pawn in player.pawns)
        {
            if (pawn.isFinished)
                finishedCount++;
        }

        Debug.Log(player.playerName + " finished pawns: " + finishedCount);

        if (finishedCount >= 4)
        {
            GameOver(player);
        }
    }

    void GameOver(Player winner)
    {
        Debug.Log("GAME OVER! Winner: " + winner.playerName);

        // Þimdilik oyunu durduralým
        Time.timeScale = 0f;
    }

    public void FinishPawn(Pawn pawn)
    {
        Debug.Log("FinishPawn called for " + pawn.owner.playerName);

        switch (pawn.owner.color)
        {
            case PlayerColor.Red:
                Debug.Log("Sending to RED finish area");
                redFinishArea.AddPawn(pawn);
                break;

            case PlayerColor.Blue:
                Debug.Log("Sending to BLUE finish area");
                blueFinishArea.AddPawn(pawn);
                break;

            case PlayerColor.Green:
                Debug.Log("Sending to GREEN finish area");
                greenFinishArea.AddPawn(pawn);
                break;

            case PlayerColor.Yellow:
                Debug.Log("Sending to YELLOW finish area");
                yellowFinishArea.AddPawn(pawn);
                break;
        }
    }

    public void OnRollDiceButton()
    {
        Player currentPlayer = players[currentPlayerIndex];

        // Sadece gerçek oyuncu için
        if (currentPlayer.isBot)
            return;

        if (!waitingForInput)
            return;

        AudioManager.Instance.PlayDice();

        TakeTurn(currentPlayer);
    }


    Pawn GetPawnPrefab(Player player)
    {
        switch (player.color)
        {
            case PlayerColor.Red:
                return pawnPrefabCowboy;

            case PlayerColor.Blue:
                return pawnPrefabPolice;

            case PlayerColor.Green:
                return pawnPrefabSkeleton;

            case PlayerColor.Yellow:
                return pawnPrefabAshigaru;
        }

        return null;
    }






}

