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

    //[SerializeField] PallorMortisScript pallorScript;
    //[SerializeField] PallorAnimation pallorAnimation;
    [SerializeField] PallorMortisScript1 pallorScript;
    public PallorMortisScript1 PallorScript => pallorScript;

    private PlayerInteractScript playerInteractScript;

    [SerializeField] HangmanScript hangmanScript;
    public bool HangManActive => hangmanScript.IsActive;

    private bool canPickUpBag;
    public bool CanPickUpBag => canPickUpBag;

    private bool canPickBonsai;
    public bool CanPickBonsai => canPickBonsai;

    [SerializeField] GameObject skipButton;
    private bool canSkipRoom;

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

        if (skipButton != null)
        {
            EnableSkip(false);
        }
    }

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        currentRoom = 1;
        canSkipRoom = false;
    }

    public IEnumerator ChangeRoom()
    {
        canStartDialogue = false;
        currentRoom++;

        if (currentRoom > numOfRoom) currentRoom = 1;

        var task1 = LevelManager.Instance.NormalFadeIn(timeDelayBeforeLoadScene);
        yield return new WaitUntil(() => task1.IsCompleted);

        if (playerInteractScript.IsSitting)
        {
            playerInteractScript.SetSit(false);
        }

        playerTransform.position = roomCycle[currentRoom - 1].spawnPos.position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + roomCycle[currentRoom - 1].cameraPos.position.x,
                                                    defaultCamPos.y + roomCycle[currentRoom - 1].cameraPos.position.y,
                                                    defaultCamPos.z + roomCycle[currentRoom - 1].cameraPos.position.z);
        timer.ResetTimer();

        DialogueManager.Instance.ResetDialogue();

        AllowPlayerToMove(true);

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
        canStartDialogue = true;

        switch (currentRoom)
        {
            case 1:
                if (DialogueManager.Instance.DialogueQueue.Count == 0)
                    pallorScript.TriggerAllDialogue();
                break;
        }
        canSkipRoom = true;
    }

    public IEnumerator ChangeRoom(float fadeIn)
    {
        canStartDialogue = false;
        currentRoom++;

        if (currentRoom > numOfRoom) currentRoom = 1;

        var task1 = LevelManager.Instance.NormalFadeIn(fadeIn);
        yield return new WaitUntil(() => task1.IsCompleted);

        if (playerInteractScript.IsSitting)
        {
            playerInteractScript.SetSit(false);
        }

        playerTransform.position = roomCycle[currentRoom - 1].spawnPos.position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + roomCycle[currentRoom - 1].cameraPos.position.x,
                                                    defaultCamPos.y + roomCycle[currentRoom - 1].cameraPos.position.y,
                                                    defaultCamPos.z + roomCycle[currentRoom - 1].cameraPos.position.z);
        timer.ResetTimer();

        DialogueManager.Instance.ResetDialogue();

        AllowPlayerToMove(true);

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
        canStartDialogue = true;

        switch (currentRoom)
        {
            case 1:
                if (DialogueManager.Instance.DialogueQueue.Count == 0)
                    pallorScript.TriggerAllDialogue();
                break;
        }
        canSkipRoom = true;
    }

    public IEnumerator ChangeRoomFinal()
    {
        canStartDialogue = false;
        canSkipRoom = false;

        var task1 = LevelManager.Instance.NormalFadeIn(timeDelayBeforeLoadScene);
        yield return new WaitUntil(() => task1.IsCompleted);

        playerTransform.position = finalRoom.spawnPos.position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + finalRoom.cameraPos.position.x,
                                                    defaultCamPos.y + finalRoom.cameraPos.position.y,
                                                    defaultCamPos.z + finalRoom.cameraPos.position.z);
        timer.ResetTimer();
        TimerActive(false);

        DialogueManager.Instance.ResetDialogue();

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
        canStartDialogue = true;

        AllowPlayerToMove(false);
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
        if (!canSkipRoom) return;
        canSkipRoom = false;
        if (playerInteractScript.IsSitting)
        {
            playerInteractScript.SetSit(false);
        }
        StartCoroutine(ChangeRoom(0.25f));
        //timer.ResetTimer();
        //pallorScript.PlayHeadExplodeAnimation();
    }

    public void NextRoomButtonAnimation()
    {
        if (!canSkipRoom) return;
        canSkipRoom = false;
        if (playerInteractScript.IsSitting)
        {
            playerInteractScript.SetSit(false);
        }
        StartCoroutine(ChangeRoom(2f));
        //timer.ResetTimer();
        pallorScript.PlayHeadExplodeAnimation();
    }

    public async void NextRoomButton(bool waitForDialogue)
    {
        if (!canSkipRoom) return;
        canSkipRoom = false;
        if (playerInteractScript.IsSitting)
        {
            playerInteractScript.SetSit(false);
        }

        if (waitForDialogue)
        {
            while (DialogueManager.Instance.IsRunning)
            {
                await Task.Delay(1);
            }
        }
        StartCoroutine(ChangeRoom(0.25f));
        //timer.ResetTimer();
        //pallorScript.PlayHeadExplodeAnimation();
    }

    public void HangManRoomButton()
    {
        StartCoroutine(ChangeRoomFinal());
    }

    public void ResetTimer()
    {
        timer.ResetTimer();
    }

    public void AllowBagPickup(bool value)
    {
        canPickUpBag = value;
    }

    public void AllowBonsaiPickup(bool value)
    {
        canPickBonsai = value;
    }

    public void EnableSkip(bool value)
    {
        skipButton.SetActive(value);
    }

    public void SetCanSkipRoom(bool value)
    {
        canSkipRoom = value;
    }
}
