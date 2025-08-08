using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    [Header("게임 매니저 설정")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject mapPrefab;
    [SerializeField] Transform playerSpawnPos;

    Player player;
    public Player Player { get { return player; } }

    // 게임 매니저가 모든 것을 생성하도록
    protected override void Awake()
    {
        base.Awake();
        //Instantiate(mapPrefab); // 맵도 생성 -> 작업 다 하고 마지막에
        player = Instantiate(playerPrefab, playerSpawnPos.position, playerSpawnPos.rotation).GetComponent<Player>();

        //UIManager.Instance;
    }

}
