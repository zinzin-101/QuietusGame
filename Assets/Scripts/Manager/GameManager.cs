using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] Timer timer;

    [SerializeField] Transform[] roomCycle;
    private int currentRoom;
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

        playerTransform.position = roomCycle[currentRoom - 1].position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + roomCycle[currentRoom - 1].position.x,
                                                    defaultCamPos.y + roomCycle[currentRoom - 1].position.y,
                                                    defaultCamPos.z + roomCycle[currentRoom - 1].position.z);
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

    public void AllowPlayerToMove(bool value)
    {
        playerCanMove = value;
    }

    public void NextRoomButton()
    {
        StartCoroutine(ChangeRoom());
        timer.ResetTimer();
    }
}
