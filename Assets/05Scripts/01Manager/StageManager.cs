using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region variables & attributes
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
    [SerializeField]
    private Transform CharacterCanvas = null;
    public Transform getCharacterCanvas() { return CharacterCanvas; }

    public StageStatus stage = StageStatus.CANNOT_TOUCH;
    [SerializeField] private Transform hand;
    public Transform getHand() { return hand; }
    [SerializeField] private GameObject card_Preview = null;
    public GameObject getCardPreview() { return card_Preview; }
    [SerializeField] private GameObject card_Prefab;
    public GameObject getCardPrefab() { return card_Prefab; }
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
    public Characters GetCharacterByPos(Pos pos)
    {
        foreach (Characters character in characters)
        {
            if (Pos.equals(character.curpos, pos)) return character;
        }
        return null;
    }

    public Characters GetEnemyByID(int id)
    {
        foreach (Characters characters in characters)
        {
            Enemy enemy = characters.GetComponent<Enemy>();
            if (enemy != null && enemy.id == id)
            {
                return characters;
            }
        }
        return null;
    }

    public int character_count = 0;

    #endregion

    public AttackList attackList = new AttackList();



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
                Debug.Log(o.name);
                enemy_prefabs.Add(o);
            }
        }
    }
    private void Start()
    {

        // for (int i = 0; i < 7; i++)
        // {
        //     for (int j = 0; j < 7; j++)
        //     {
        //         Vector3 a = MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(i, j, 0));
        //         Vector3 b = Camera.main.WorldToScreenPoint(a);
        //         Debug.Log(string.Format("{0},{1}: {2} /// {3}", i, j, a, b));
        //     }
        // }
    }

    private void LateUpdate()
    {
        inputManager.onLateUpdate();
    }

}
