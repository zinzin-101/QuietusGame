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
    private int numOfRoom;

    [SerializeField] Transform playerTransform;
    [SerializeField] float timeDelayBeforeLoadScene = 2f;

    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 defaultCamPos;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (playerTransform == null)
        {
            GameObject.FindWithTag("Player");
        }

        numOfRoom = roomCycle.Length;
    }

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        currentRoom = 1;
    }

    public IEnumerator ChangeRoom()
    {
        currentRoom++;

        if (currentRoom > numOfRoom) currentRoom = 1;

        var task1 = LevelManager.Instance.NormalFadeIn(timeDelayBeforeLoadScene);
        yield return new WaitUntil(() => task1.IsCompleted);

        playerTransform.position = roomCycle[currentRoom - 1].position;
        mainCamera.transform.position = new Vector3(defaultCamPos.x + roomCycle[currentRoom - 1].position.x,
                                                    defaultCamPos.y + roomCycle[currentRoom - 1].position.y,
                                                    defaultCamPos.z + roomCycle[currentRoom - 1].position.z);
        timer.ResetTimer();

        var task2 = LevelManager.Instance.NormalFadeOut();
        yield return new WaitUntil(() => task2.IsCompleted);
    }

    public void TimerActive(bool value)
    {
        timer.SetActiveTimer(value);
    }
}
