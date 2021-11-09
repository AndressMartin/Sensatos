using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private SoundManager soundManager;
    bool sound;
    EnemyMove enemyMove;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sound)
        {
            //enemyMove.
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enemyMove = collision.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            sound = true;
        }
    }

    void CreatSound()
    {
        soundManager.InstantiatSound();
    }
}
