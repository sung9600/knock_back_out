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
                card.GetComponent<Cards>().SetOrigin(card.GetComponent<RectTransform>().localPosition);
            }
        }
        Input.multiTouchEnabled = false;
    }

    private void Update()
    {
        inputManager.onUpdate();
    }

}
