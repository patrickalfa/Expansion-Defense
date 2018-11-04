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
    public int constructionIndex;

    [Header("Attributes")]
    public int time;
    public int wood;
    public int stone;
    public int woodRate;
    public int stoneRate;

    [Header("References")]
    public Node _baseNode;
    public SpriteRenderer _darkness;
    public Construction _construction;

    [Header("Prefabs")]
    public GameObject[] pfConstructions;

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

        state = GAME_STATE.PLANNING;

        StartCoroutine(Countdown());
    }

    private void LateUpdate()
    {
        lateState = state;
        lateStage = stage;

        if (Input.GetMouseButtonUp(1))
            Spawner.instance.SpawnWave();
    }

    private IEnumerator Countdown()
    {
        time = maxTime;

        while (time > 0)
        {
            yield return new WaitForSeconds(1f);

            time--;
            wood += woodRate;
            stone += stoneRate;

            SetDarkness((float)time / (float)maxTime);
        }

        stage = GAME_STAGE.CONSTRUCTION;

        _construction = Instantiate(pfConstructions[constructionIndex]).
            GetComponent<Construction>();
        _construction.gameObject.SetActive(false);

        time = maxTime;

        while (time > 0)
        {
            yield return new WaitForSeconds(1f);

            time--;
        }

        stage = GAME_STAGE.DEFENSE;

        Destroy(_construction.gameObject);

        while (true)
        {
            yield return new WaitForSeconds(1f);

            time++;
        }
    }

    private void SetDarkness(float amount)
    {
        _darkness.color = new Color(0f, 0f, 0f, 1f - amount);

        for (int x = 0; x < Generator.instance.size; x++)
        {
            for (int y = 0; y < Generator.instance.size; y++)
            {
                Node n = Generator.instance.GetNode(x, y);
                if (!n.built)
                    n.SetColor(new Color(amount, amount, amount, 1f));
            }
        }
    }

    public void Construct(Node node)
    {
        if (!_construction.Build(node))
            return;

        _construction = Instantiate(pfConstructions[constructionIndex]).
            GetComponent<Construction>();
        _construction.gameObject.SetActive(false);
    }

    public void SelectContruction(int index)
    {
        constructionIndex = index;

        if (_construction)
        {
            Destroy(_construction.gameObject);

            _construction = Instantiate(pfConstructions[constructionIndex]).
                GetComponent<Construction>();
            _construction.gameObject.SetActive(false);
        }
    }
}