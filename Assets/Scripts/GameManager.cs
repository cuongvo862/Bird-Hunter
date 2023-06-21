using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float spawnTime;
    public Bird[] birdPrefabs;
    public int timeLimit;

    int m_curTimeLimit;
    int m_birdKilled;
    bool m_isGameOver;

    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public int BirdKilled { get => m_birdKilled; set => m_birdKilled = value; }

    public override void Awake()
    {
        MakeSingleton(false);

        m_curTimeLimit = timeLimit;

        //xoa tat ca du lieu da luu vao bo nho
        //PlayerPrefs.DeleteAll();
    }
    public override void Start()
    {
        GameGUIManager.Ins.ShowGameGUI(false);
        GameGUIManager.Ins.UpdateKilledCounting(m_birdKilled);
    }

    public void PlayGame()
    {
        StartCoroutine(GameSpawn());

        StartCoroutine(TimeCountDown());

        GameGUIManager.Ins.ShowGameGUI(true);
    }
    IEnumerator TimeCountDown()
    {
        while (m_curTimeLimit > 0)
        {
            yield return new WaitForSeconds(1f);

            m_curTimeLimit--;

            if (m_curTimeLimit <= 0)
            {
                m_isGameOver = true;

                if (m_birdKilled > Prefs.bestScore)
                {
                    GameGUIManager.Ins.gameDialog.UpdateDialog("NEW BEST", "BEST KILLED : x" + m_birdKilled);
                }
                else if (m_birdKilled < Prefs.bestScore)
                {
                    GameGUIManager.Ins.gameDialog.UpdateDialog("YOUR BEST", "BEST KILLED : x" + Prefs.bestScore);

                }
                Prefs.bestScore = m_birdKilled;

                GameGUIManager.Ins.gameDialog.Show(true);
                GameGUIManager.Ins.gameDialog = GameGUIManager.Ins.gameDialog;

            }
            GameGUIManager.Ins.UpdateTimer(IntToTime(m_curTimeLimit));
        }
    }
    IEnumerator GameSpawn()
    {
        while (!IsGameOver)
        {
            spawnBird();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void spawnBird()
    {
        Vector3 spawnPos = Vector3.zero;

        float randCheck = Random.Range(0f, 1f);

        if(randCheck >= 0.5f)
        {
            spawnPos = new Vector3(12, Random.Range(0f, 4f), 0);
        }
        else
        {
            spawnPos = new Vector3(-12, Random.Range(0f, 4f), 0);
        }

        if(birdPrefabs !=null && birdPrefabs.Length > 0)
        {
            int randIdx = Random.Range(0, birdPrefabs.Length);

            if(birdPrefabs[randIdx] != null)
            {
                Bird birdClone = Instantiate(birdPrefabs[randIdx], spawnPos, Quaternion.identity);
            }
        }
    }

    string IntToTime(int time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);

        return minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
