using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;

    private GameObject player;

    [Header("UI")]
    public GameObject panel;
    public Fader fader;
    public GameObject canvasUi;
    public GameObject panelRetry;
    public GameObject panelWin;
    public GameObject notificationText;

    [Header("Maps")]
    public MapSystem mapSystem;
    public MapSystem.Map map;
    private MobSpawn mobSpawner;
    public List<GameObject> mobs;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject buttonPrefab;
    public GameObject nextMapTriggerPrefab;
    public GameObject bossPrefab;
    public GameObject[] prefabsRooms;

    [Header("Camera")]
    public Cinemachine.CinemachineVirtualCamera camera;

    [HideInInspector]
    public int currentRoomId;
    private bool isOver = false;
    private bool triggerSpawned = false;
    private bool choiceShowed = false;

    private int roomCount = 0;
    private int nbFloor = 10;

    // Use this for initialization
    void Awake()
    {
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
        map = mapSystem.GenerateMap(nbFloor,4);

        switch (map.rooms[0].roomType)
        {
            case MapSystem.RoomType.Combat:
                {
                    SoundManager._instance.PlayMusic(SoundType.Fight);
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
        if (scene.name == "Game")
        {
            notificationText = GameObject.Find("NotificationText");
            switch (map.GetRoom(currentRoomId).roomType)
            {
                case MapSystem.RoomType.Combat:
                    {
                        SoundManager._instance.PlayMusic(SoundType.Fight);
                        Instantiate(prefabsRooms[0].gameObject);
                        mobSpawner.canvasTransf = GameObject.Find("MainCanvas").transform;
                        mobs = mobSpawner.SpawnMonsters(map.GetRoom(currentRoomId).floorLevel);
                    }
                    break;

                case MapSystem.RoomType.Heal:
                    {
                        SoundManager._instance.PlayMusic(SoundType.Calm);
                        Instantiate(prefabsRooms[1].gameObject);
                        player.GetComponent<Health>().Heal(player.GetComponent<Health>().maxHp / 2);
                        ShowNotification("Vous avez ete soigne...", 2f);
                    }
                    break;

                case MapSystem.RoomType.Chest:
                    {
                        SoundManager._instance.PlayMusic(SoundType.Calm);
                        Instantiate(prefabsRooms[2].gameObject);
                        ShowNotification("Vous sentez la richesse non loin...", 2f);
                    }
                    break;

                case MapSystem.RoomType.Boss:
                    {
                        SoundManager._instance.PlayMusic(SoundType.Boss);
                        Instantiate(prefabsRooms[3].gameObject);
                        mobs.Add(Instantiate(bossPrefab, new Vector2(8,0), Quaternion.identity));
                    }
                    break;

                default:
                    Debug.Log("a gérer");
                    break;
            }

            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                player = Instantiate(playerPrefab);

            player.GetComponent<PlayerController>().canMove = true;
            fader = GameObject.FindObjectOfType<Fader>();
            camera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
            camera.Follow = player.transform;
            triggerSpawned = false;
            panelRetry.SetActive(false);
            panelWin.SetActive(false);
            canvasUi.SetActive(false);
        }
        else if(scene.name == "Building")
        {
            SoundManager._instance.PlayMusic(SoundType.Calm);
            fader = GameObject.FindObjectOfType<Fader>();
            player.GetComponent<PlayerController>().canMove = false;
        }
        else
        {
            panelRetry.SetActive(false);
            panelWin.SetActive(false);
            canvasUi.SetActive(false);

            Destroy(player.gameObject);
            player = Instantiate(playerPrefab.gameObject);

            roomCount = 0;
            currentRoomId = 0;
            triggerSpawned = false;

            map = mapSystem.GenerateMap(nbFloor);
            currentRoomId = map.rooms[0].id;

            Destroy(player.gameObject);
        }

        roomCount++;
        choiceShowed = false;
        if (map.GetRoom(currentRoomId).roomType == MapSystem.RoomType.Boss)
            player.transform.position = new Vector2(-8, 0);
        else
            player.transform.position = Vector2.zero;
    }

    // The player died RIP
    public void LoseTheGame()
    {
        isOver = true;
        canvasUi.SetActive(true);
        panelRetry.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (mobs.Count == 0 && triggerSpawned == false && roomCount <= nbFloor)
        {
            GameObject go = Instantiate(nextMapTriggerPrefab.gameObject);
            go.transform.position = Vector3.zero;
            triggerSpawned = true;
        }

        if (roomCount == nbFloor + 1 && mobs.Count == 0 && triggerSpawned == false)
        {
            triggerSpawned = true;
            canvasUi.SetActive(true);
            panelWin.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ShowNextMaps()
    {
        if (!choiceShowed)
        {
            List<int> nextRoomsId = map.GetNextRooms(currentRoomId);

            foreach (int id in nextRoomsId)
            {
                //Debug.Log("Next room : " + id);
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

    public void Reset()
    {
        panelRetry.SetActive(false);
        panelWin.SetActive(false);
        canvasUi.SetActive(false);

        Destroy(player.gameObject);
        player = Instantiate(playerPrefab.gameObject);

        roomCount = 0;
        currentRoomId = 0;
        triggerSpawned = false;

        map = mapSystem.GenerateMap(nbFloor);
        currentRoomId = map.rooms[0].id;


        fader.StartFadetoScene("Game");
    }

    public void QuitGame()
    {
        fader.StartFadetoScene("MainMenu");
    }

    public void ShowNotification(string text, float time)
    {
        StartCoroutine(Notification(text, time));
    }

    IEnumerator Notification(string text, float time)
    {
        Debug.Log("here");
        notificationText.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(time);
        notificationText.GetComponent<Text>().text = "";
    }
}
