using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public HexPlayer player;
    int index;
    bool between;

    void Start()
    {
        index = 0;
        between = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (between)
        {
            if (player.CurrentState == HexPlayer.State.Ready)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                player.CurrentAudio.PlayAlert();
                index++;
                between = false;
                if (index >= transform.childCount)
                    enabled = false;
            }
        }
        else
        {
            if (player.CurrentState != HexPlayer.State.Ready)
                between = true;
        }
    }
}
