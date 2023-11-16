using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnRoom
    {
        public Transform spawnPos, cameraPos;
    }

    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] Timer timer;

    [SerializeField] SpawnRoom[] roomCycle;
    private int currentRoom;

    [SerializeField] SpawnRoom finalRoom;

    public int CurrentRoom => currentRoom;
    private int numOfRoom;

    [SerializeField] Transform playerTransform;
    [SerializeField] float timeDelayBeforeLoadScene = 2f;

    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 defaultCamPos;

    private bool playerCanSit;
    public bool PlayerCanSit => playerCanSit;

    private bool playerCanMove;
    public bool PlayerCanMove => playerCanMove;

    private bool canStartDialogue;
    public bool CanStartDialogue => canStartDialogue;

    [SerializeField] PallorMortisScript pallorScript;
    //[SerializeField] PallorAnimation pallorAnimation;

    private PlayerInteractScript playerInteractScript;

    [SerializeField] HangmanScript hangmanScript;

    private void Awake()
    {
        SoundManager.Initialize();
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        if (playerTransform == null)
        {
            GameObject.FindWithTag("Player");
        }

        playerInteractScript = playerTransform.GetComponent<PlayerInteractScript>();

        numOfRoom = roomCycle.Length;

        playerCanSit = true;
        playerCanMove = true;
        canStartDialogue = true;
    }

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        currentRoom = 1;
    }

    public IEnumerator ChangeRoom()
    {
        canStartDialogue = false;
        currentRoom++;

        if (currentRoom > numOfRoom) currentRoom = 1;

        var task1 = LevelManager.Instance.NormalFadeIn(timeDelayBeforeLoadScene);
        yield return new WaitUntil(() => task1.IsCompleted);

        playerTransform.position = roomCycle[currentRoom - 1].spawnPos.position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + roomCycle[currentRoom - 1].cameraPos.position.x,
                                                    defaultCamPos.y + roomCycle[currentRoom - 1].cameraPos.position.y,
                                                    defaultCamPos.z + roomCycle[currentRoom - 1].cameraPos.position.z);
        timer.ResetTimer();

        DialogueManager.Instance.ResetDialogue();

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
        canStartDialogue = true;

        switch (currentRoom)
        {
            case 1:
                pallorScript.PlayPhaseDialogue();
                break;
        }
    }

    public IEnumerator ChangeRoomFinal()
    {
        canStartDialogue = false;

        var task1 = LevelManager.Instance.NormalFadeIn(timeDelayBeforeLoadScene);
        yield return new WaitUntil(() => task1.IsCompleted);

        playerTransform.position = finalRoom.spawnPos.position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + finalRoom.cameraPos.position.x,
                                                    defaultCamPos.y + finalRoom.cameraPos.position.y,
                                                    defaultCamPos.z + finalRoom.cameraPos.position.z);
        timer.ResetTimer();

        DialogueManager.Instance.ResetDialogue();

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
        canStartDialogue = true;

        hangmanScript.StartRoom();
    }

    public void TimerActive(bool value)
    {
        timer.SetActiveTimer(value);
    }

    public void TimerForcedStop(bool value)
    {
        timer.SetForcedStopTimer(value);
    }

    public void AllowPlayerToSit(bool value) //by this I mean allow player to eject from the chair
    {
        playerCanSit = value;
    }

    public bool GetPlayerSittingStat()
    {
        return playerInteractScript.IsSitting;
    }

    public void AllowPlayerToMove(bool value)
    {
        playerCanMove = value;
    }

    public void NextRoomButton()
    {
        StartCoroutine(ChangeRoom());
        timer.ResetTimer();
        pallorScript.PlayHeadExplodeAnimation();
    }

    public void ResetTimer()
    {
        timer.ResetTimer();
    }
}