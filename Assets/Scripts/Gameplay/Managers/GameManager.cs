using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        Random.InitState(seed);
        Generator.instance.Generate();

        Camera.main.transform.position = _baseNode.transform.position + (Vector3.back * 10f);
        _base = _baseNode.transform.GetComponentInChildren<Base>();

        state = GAME_STATE.PLANNING;

        SoundManager.sfxVolume = .5f;

        StartCoroutine(Countdown());
    }

    private void LateUpdate()
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

        lateState = state;
        lateStage = stage;

        if (Input.GetMouseButtonUp(1))
            time = 0;
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

            UIManager.instance.Log(
                dayQuotes[Random.Range(0, dayQuotes.Length)],
                new Color(0f, 0f, 0f, 1f));
            SoundManager.PlaySound("day");
        }
    }

    private void SetDarkness(float amount)
    {
        _light.intensity = Mathf.Lerp(0f, .75f, amount);

        /*
        amount *= .9f;

        _darkness.color = new Color(0f, 0f, 0f, .9f - amount);

        for (int x = 0; x < Generator.instance.size; x++)
        {
            for (int y = 0; y < Generator.instance.size; y++)
            {
                Node n = Generator.instance.GetNode(x, y);
                if (!n.built)
                    n.SetColor(new Color(.1f + amount, .1f + amount, .1f + amount, 1f));
            }
        }
        */
    }

    

    public void SetStage(int stage)
    {
        this.stage = (GAME_STAGE)stage;
        SoundManager.PlaySound("button");
    }
}