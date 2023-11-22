using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupedDigiClockScript : MonoBehaviour
{
    [SerializeField] DigiClockScript[] digiclock;
    private int numOfDigiClock;

    [SerializeField] GameObject spawnItem;
    [SerializeField] Transform spawnPos;

    private bool activated;

    private void Awake()
    {
        if (digiclock == null)
        {
            activated = true;
        }

        numOfDigiClock = digiclock.Length;
        activated = false;
    }

    private void Update()
    {
        if (activated) return;

        for (int i = 0; i < numOfDigiClock; i++)
        {
            if (digiclock[i] == null) continue;

            if (!digiclock[i].Completed)
            {
                return;
            }
        }

        SpawnItem();
        
        for (int i = 0; i < numOfDigiClock; i++)
        {
            digiclock[i].SetDisable(true);
            digiclock[i].SetActiveGameObject(false);
        }

        activated = true;
    }

    public void SpawnItem()
    {
        Instantiate(spawnItem, spawnPos.position, Quaternion.identity);
    }
}
