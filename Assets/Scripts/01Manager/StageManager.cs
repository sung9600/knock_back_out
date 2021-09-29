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

    public Transform hand1;
    public Transform hand2;
    public GameObject card_Prefab;
    public static bool gameStop;
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
        Input.multiTouchEnabled = false;
        Screen.SetResolution(1080, 1920, true, 60);
    }

    private void LateUpdate()
    {
        inputManager.onUpdate();
    }

}
