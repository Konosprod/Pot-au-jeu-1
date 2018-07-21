using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;

    private GameObject player;

    [Header("UI")]
    public GameObject panel;
    public Fader fader;
    public GameObject canvasUi;

    [Header("Maps")]
    public MapSystem mapSystem;
    public MapSystem.Map map;
    private MobSpawn mobSpawner;
    public List<GameObject> mobs;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject buttonPrefab;
    public GameObject nextMapTriggerPrefab;
    public GameObject[] prefabsRooms;

    [Header("Camera")]
    public Cinemachine.CinemachineVirtualCamera camera;

    [HideInInspector]
    public int currentRoomId;
    private bool isOver = false;
    private bool triggerSpawned = false;
    private bool choiceShowed = false;

	// Use this for initialization
	void Awake () {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        mobSpawner = GetComponent<MobSpawn>();
	}

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        map = mapSystem.GenerateMap(10);

        switch(map.rooms[0].roomType)
        {
            case MapSystem.RoomType.Combat:
                {
                    Instantiate(prefabsRooms[0].gameObject);
                    mobs = mobSpawner.SpawnMonsters(0);
                }
                break;
        }

        player = Instantiate(playerPrefab.gameObject);
        currentRoomId = map.rooms[0].id;
        camera.Follow = player.transform;

    }

    private void OnSceneChanged(Scene scene, LoadSceneMode arg1)
    {
        if(scene.name == "Game")
        {
            switch (map.GetRoom(currentRoomId).roomType)
            {
                case MapSystem.RoomType.Combat:
                    {
                        Instantiate(prefabsRooms[0].gameObject);
                        mobs = mobSpawner.SpawnMonsters(map.GetRoom(currentRoomId).floorLevel);
                    }
                    break;

                case MapSystem.RoomType.Heal:
                    {
                        Instantiate(prefabsRooms[1].gameObject);
                    }
                    break;

                case MapSystem.RoomType.Chest:
                    {
                        Instantiate(prefabsRooms[2].gameObject);
                    }
                    break;

                case MapSystem.RoomType.Boss:
                    {
                        Instantiate(prefabsRooms[3].gameObject);
                    }
                    break;

                default:
                    Debug.Log("a gérer");
                    break;
            }

            //player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().canMove = true;
            fader = GameObject.FindObjectOfType<Fader>();
            camera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
            camera.Follow = player.transform;
            triggerSpawned = false;
        }
        else
        {
            fader = GameObject.FindObjectOfType<Fader>();
            player.GetComponent<PlayerController>().canMove = false;
        }

        choiceShowed = false;
        player.transform.position = Vector2.zero;
    }

    // The player died RIP
    public void LoseTheGame()
    {
        isOver = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(mobs.Count == 0 && triggerSpawned == false)
        {
            GameObject go = Instantiate(nextMapTriggerPrefab.gameObject);
            go.transform.position = Vector3.zero;
            triggerSpawned = true;
        }
	}

    public void ShowNextMaps()
    {
        if (!choiceShowed)
        {
            List<int> nextRoomsId = map.GetNextRooms(currentRoomId);

            foreach (int id in nextRoomsId)
            {
                Debug.Log("From : " + currentRoomId.ToString() + " To : " + id.ToString());
                GameObject button = Instantiate(buttonPrefab.gameObject, panel.transform);
                button.GetComponentInChildren<Text>().text = map.GetRoom(id).roomType.ToString();
                button.GetComponent<ButtonChoice>().id = id;
            }
            choiceShowed = true;
            canvasUi.SetActive(true);
        }
    }

    public void LoadNextScene()
    {
        foreach (Transform child in panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        canvasUi.SetActive(false);


        if (map.GetRoom(currentRoomId).roomType != MapSystem.RoomType.Builder)
            fader.StartFadetoScene("Game");
        else
            fader.StartFadetoScene("Building");
    }
}
