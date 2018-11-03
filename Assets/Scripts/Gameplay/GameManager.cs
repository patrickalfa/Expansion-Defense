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
    public GAME_STAGE stage;

    [Header("Control variables")]
    public int maxTime;
    public int woodRate;
    public int stoneRate;

    [Header("Attributes")]
    public int time;
    public int wood;
    public int stone;

    [Header("References")]
    public Node _baseNode;
    public SpriteRenderer _darkness;

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
        state = GAME_STATE.PLANNING;
        Generator.instance.Generate();

        Camera.main.transform.position = _baseNode.transform.position + (Vector3.back * 10f);

        StartCoroutine(Countdown());
    }

    private void Update()
    {

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

        time = maxTime;

        while (time > 0)
        {
            yield return new WaitForSeconds(1f);

            time--;
        }

        stage = GAME_STAGE.DEFENSE;
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
}