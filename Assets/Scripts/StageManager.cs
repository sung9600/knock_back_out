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

    private MapManager map_instance = null;
    public MapManager mapManager
    {
        get
        {
            if (map_instance == null)
            {
                map_instance = gameObject.GetComponent<MapManager>();
            }
            return map_instance;
        }
    }

    public Player player;

    public Transform hand;
    public GameObject card_prefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            stageManager.inputManager.init();
            mapManager.init();
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject card = Instantiate(card_prefab, hand);
                card.GetComponent<RectTransform>().localPosition = new Vector3(-400 + 200 * j, -150 + 300 * i, 0);
            }
        }
    }
    private void Start()
    {
        // 이부분 따로 빼기
        Rect rect = Camera.main.rect;
        float scale_h = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scale_w = 1f / scale_h;
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        if (scale_h > 1)
        {
            //가로가 더 큼
            rect.width = scale_w;
            rect.x = (1f - scale_w) / 2f;
        }
        else
        {
            //세로가 더 큼
            rect.height = scale_h;
            rect.y = (1f - scale_h) / 2f;
        }
        Camera.main.rect = rect;
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);

    private void Update()
    {
        inputManager.onUpdate();
    }
}
