using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum GAME_STATE
{
    PAUSED,
    PLANNING,
    DRAGGING
}

public enum GAME_STAGE
{
    EXPANSION,
    CONSTRUCTION,
    DEFENSE
}

public class GameManager : MonoBehaviour
{
    [Header("Game phase")]
    public GAME_STATE state;
    public GAME_STATE lateState;
    public GAME_STAGE stage;
    public GAME_STAGE lateStage;

    [Header("Control variables")]
    public int seed;
    public int maxTime;

    [Header("Attributes")]
    public int time;
    public int wood;
    public int gold;
    public int woodRate;
    public int goldRate;
    public int healRate;

    [Header("References")]
    public Node _baseNode;
    public SpriteRenderer _darkness;
    public Light _light;
    public string[] nightQuotes, dayQuotes;
    public Base _base;

    public bool started = false;

    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    public void StartGame()
    {
        started = true;

        SoundManager.sfxVolume = .5f;

        Random.InitState(seed);
        Generator.instance.Generate();

        Camera.main.transform.position = _baseNode.transform.position + (Vector3.back * 10f);
        _base = _baseNode.transform.GetComponentInChildren<Base>();

        state = GAME_STATE.PLANNING;

        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            Pause(true);
            UIManager.instance.SetActive("Help", true);
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        StartCoroutine(Countdown());
    }

    private void LateUpdate()
    {
        if (state != GAME_STATE.PAUSED)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Pause(true);
                UIManager.instance.SetActive("Pause", true);
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Pause(false);
                UIManager.instance.SetActive("Pause", false);
                UIManager.instance.SetActive("Help", false);
            }
        }

        OnStageChanged();

        lateState = state;
        lateStage = stage;
    }

    private void OnStageChanged()
    {
        if (lateStage != stage)
        {
            if (lateStage == GAME_STAGE.CONSTRUCTION)
                Destroy(Constructor.instance._construction.gameObject);

            switch (stage)
            {
                case GAME_STAGE.EXPANSION:
                    break;
                case GAME_STAGE.CONSTRUCTION:
                    Constructor.instance.SelectContruction(Constructor.instance.constructionIndex);
                    break;
                case GAME_STAGE.DEFENSE:
                    Spawner.instance.SpawnWave();
                    break;
            }
        }
    }

    private IEnumerator Countdown()
    {
        while (true)
        {
            stage = GAME_STAGE.EXPANSION;

            time = maxTime;

            while (time > 0)
            {
                yield return new WaitForSeconds(1f);

                time--;
                wood += woodRate;
                gold += goldRate;
                _base.Heal(healRate);

                SetDarkness((float)time / (float)maxTime);
            }

            UIManager.instance.Log(
                nightQuotes[Random.Range(0, nightQuotes.Length)],
                new Color(1f, 1f, 1f, .75f));

            SoundManager.PlaySound("night");
            SoundManager.SetChannelVolume("Drums", 1f);
            GameManager.instance.ScreenShake(.25f, .25f, 50);

            stage = GAME_STAGE.DEFENSE;
            yield return new WaitForEndOfFrame();

            while (Spawner.instance.enemiesAlive > 0)
            {
                yield return new WaitForSeconds(1f);

                time++;
            }

            yield return new WaitForSeconds(1f);

            float amount = 0f;
            while (amount < 1f)
            {
                amount = Mathf.Clamp(amount + Time.deltaTime, 0f, 1f);
                SetDarkness(amount);

                yield return new WaitForEndOfFrame();
            }

            if (Spawner.instance.waveIndex == Spawner.instance.waves.Length)
                EndGame(true);

            UIManager.instance.Log(
                dayQuotes[Random.Range(0, dayQuotes.Length)],
                new Color(0f, 0f, 0f, 1f));
            SoundManager.PlaySound("day");
            SoundManager.SetChannelVolume("Drums", 0f);
        }
    }

    private void SetDarkness(float amount)
    {
        _light.intensity = Mathf.Lerp(0f, .75f, amount);

        SoundManager.SetChannelVolume("Drums", Mathf.Clamp(1f - (amount * 4f), 0f, 1f));
    }

    public void SetStage(int stage)
    {
        this.stage = (GAME_STAGE)stage;

        if (this.stage != GAME_STAGE.CONSTRUCTION)
            SoundManager.PlaySound("button");
    }

    public void ScreenShake(float time, float strength, int vibrato)
    {
        Camera.main.DOShakePosition(time, strength, vibrato).SetUpdate(true);
    }

    public void FreezeFrame(float time)
    {
        Time.timeScale = 0f;

        transform.DOMove(Vector3.zero, time).SetUpdate(true).OnComplete(() =>
        {
            if (state != GAME_STATE.PAUSED)
                Time.timeScale = 1f;
        });
    }

    public void Pause(bool pause)
    {
        SoundManager.PlaySound("button");
        Camera.main.GetComponent<AudioLowPassFilter>().
            cutoffFrequency = (pause ? 1000 : 22000);

        if (pause)
        {
            state = GAME_STATE.PAUSED;
            Time.timeScale = 0f;
        }
        else
        {
            state = GAME_STATE.DRAGGING;
            Time.timeScale = 1f;
        }
    }

    public void EndGame(bool success)
    {
        if (state == GAME_STATE.PAUSED)
            return;

        Pause(true);

        SoundManager.SetChannelVolume("Drums", 0f);
        Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency = 5000;

        if (success)
        {
            SoundManager.SetChannelVolume("Main", 0f);
            SoundManager.PlaySound("happy", true);

            UIManager.instance.SetActive("Success", true);
            UIManager.instance.FadeIn("Success");
        }
        else
        {
            SoundManager.PlaySound("night");

            UIManager.instance.SetActive("Failure", true);
            UIManager.instance.FadeIn("Failure");
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SoundManager.PlaySound("button");
        SceneManager.LoadScene(0);
    }
}