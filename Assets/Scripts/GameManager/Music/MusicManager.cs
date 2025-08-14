using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum modes
{
    Calm,
    Fire,
    Blaze
}

public class MusicManager : MonoBehaviour
{
    //[SerializeField] private GameObject CalmMusicObj;
    [SerializeField] private AudioSource CalmMusic;
    //[SerializeField] private GameObject FireMusicObj;
    [SerializeField] private AudioSource FireMusic;
    [SerializeField] private float musicVolume;
    public modes musicCurrent => musicMode(activeEnemies(), 1);

    // Start is called before the first frame update
    void Start()
    {
        if (CalmMusic.volume > 0 && musicCurrent != modes.Calm)
        {
            CalmMusic.volume = musicVolume;
            FireMusic.volume = 0;
        }
        else if (FireMusic.volume > 0 && musicCurrent != modes.Fire)
        {
            CalmMusic.volume = 0;
            FireMusic.volume = musicVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CalmMusic.volume > 0 && musicCurrent != modes.Calm)
        {
            CalmMusic.volume = Mathf.Clamp(CalmMusic.volume - 1 * Time.deltaTime,0, musicVolume);
            FireMusic.volume = Mathf.Clamp(FireMusic.volume + 1 * Time.deltaTime,0, musicVolume);
        }
        else if (FireMusic.volume > 0 && musicCurrent != modes.Fire)
        {
            CalmMusic.volume = Mathf.Clamp(CalmMusic.volume + 1 * Time.deltaTime,0, musicVolume);
            FireMusic.volume = Mathf.Clamp(FireMusic.volume - 1 * Time.deltaTime,0, musicVolume);
        }
    }

    private int activeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int enemiesCounted = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Enemy>().Mood != EnemyMood.sleep && enemies[i].GetComponent<Enemy>().Mood != EnemyMood.busy)
            {
                enemiesCounted++;
            }
        }
        return enemiesCounted;
    }

    private modes musicMode(int activeCounter, int fireTreshold)
    {
        if (activeCounter >= fireTreshold)
        {
            return modes.Fire;
        }
        else
        {
            return modes.Calm;
        }
    }
}
