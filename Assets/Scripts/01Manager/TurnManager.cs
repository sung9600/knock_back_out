using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{

    public static TurnManager turnManager
    {
        get
        {
            return instance;
        }
    }
    private static TurnManager instance;
    public static int turn = 0;
    public phase phase = phase.map_gimmick;

    public Turns[] turns;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        turns = new Turns[5];
        turns[0] = ScriptableObject.CreateInstance<PlayerSetTurn>();
        turns[1] = ScriptableObject.CreateInstance<PlayerSetTurn>();
        turns[2] = ScriptableObject.CreateInstance<PlayerSetTurn>();
        turns[3] = ScriptableObject.CreateInstance<PlayerSetTurn>();
        turns[4] = ScriptableObject.CreateInstance<PlayerSetTurn>();
    }

    private void Start()
    {
        StartCoroutine("turnmanage");
    }
    public TextMeshProUGUI indicator;

    public int turnIndex;
    IEnumerator turnmanage()
    {
        while (true)
        {
            if (StageManager.gameStop) yield return null;
            if (turnIndex > turns.Length - 1)
            {
                yield break;
            }
            if (turns[turnIndex].Execute())
            {
                // turn 종료
                Debug.Log(string.Format("turn {0} over", turn));
                turnIndex++;
                phase = phase.map_gimmick;
                if (turnIndex > turns.Length - 1)
                {
                    // 게임 클리어 조건 : 생존
                    turnIndex = 0;
                    yield break;
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
