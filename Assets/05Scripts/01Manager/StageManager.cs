using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    private static StageManager instance = null;
    public static StageManager stageManager
    {
        get
        {
            return instance;
        }
    }

    private InputManager input_instance = null;
    public InputManager inputManager
    {
        get
        {
            if (input_instance == null)
            {
                input_instance = new InputManager();
            }
            return input_instance;
        }
    }

    [SerializeField]
    private List<GameObject> enemy_prefabs;
    public List<GameObject> getEnemy_Prefabs()
    {
        return enemy_prefabs;
    }

    public GameObject getEnemy_Prefab_byIndex(int index)
    {
        return enemy_prefabs[index];
    }
    public GameObject getEnemy_Prefab_byName(string name)
    {
        int prefab_count = enemy_prefabs.Count;
        for (int i = 0; i < prefab_count; i++)
        {
            if (enemy_prefabs[i].name == name) return enemy_prefabs[i];
        }
        return null;
    }

    public StageStatus stage = StageStatus.CANNOT_TOUCH;
    public Transform hand;
    public GameObject big_Card = null;
    public GameObject card_Prefab;
    private static bool gameStop;
    public bool isgameStopped()
    {
        return gameStop;
    }
    [SerializeField] private Player player;
    public Player GetPlayer()
    {
        return player;
    }
    [SerializeField] private List<Characters> characters;
    public Characters GetCharacterByIndex(int idx)
    {
        return characters[idx];
    }
    public List<Characters> GetCharactersList()
    {
        return characters;
    }

    public Characters GetCharacterByVector3Int(Vector3Int pos)
    {
        Pos p = new Pos(pos.x, pos.y);
        foreach (Characters character in characters)
        {
            if (Pos.equals(character.curpos, p)) return character;
        }
        return null;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            stageManager.inputManager.init();
        }
        //player = GameObject.Find("Player").GetComponent<Player>();
        Input.multiTouchEnabled = false;
        Screen.SetResolution(1920, 1080, true, 60);
        characters = new List<Characters>();

        GameObject[] os = Resources.LoadAll<GameObject>("Prefabs");
        foreach (GameObject o in os)
        {
            if (o.GetComponent<Enemy>() != null)
            {
                enemy_prefabs.Add(o);
            }
        }
    }

    private void LateUpdate()
    {
        inputManager.onLateUpdate();
    }

}
