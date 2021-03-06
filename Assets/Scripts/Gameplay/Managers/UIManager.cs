﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text txtTimer;
    public Text txtWood;
    public Text txtStone;
    public Text txtHealth;
    public Text txtStage;
    public Text txtLog;

    public GameObject pnlStages;
    public Button[] btnStages;

    public GameObject pnlConstructions;
    public Button[] btnConstructions;

    ///////////////////////////////////

    private int lateConstructionIndex = 0;

    ///////////////////////////////////

    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<UIManager>();
            return m_instance;
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.started)
            return;

        txtTimer.text = GameManager.instance.time.ToString();

        txtWood.text = GameManager.instance.wood +
            " (+" + GameManager.instance.woodRate + ")";

        txtStone.text = GameManager.instance.gold +
            " (+" + GameManager.instance.goldRate + ")";

        txtHealth.text = GameManager.instance._base.health +
            " (+" + GameManager.instance.healRate + ")";

        if (GameManager.instance.lateStage != GameManager.instance.stage)
            HandleStageUI();

        if (lateConstructionIndex != Constructor.instance.constructionIndex)
            ChangeContructionButton();
    }

    private void HandleStageUI()
    {
        switch (GameManager.instance.stage)
        {
            case GAME_STAGE.EXPANSION:
                txtStage.text = "EXPANSION";
                pnlStages.SetActive(true);
                pnlConstructions.SetActive(false);
                txtTimer.gameObject.SetActive(true);
                btnStages[1].transform.Find("Outline").gameObject.SetActive(false);
                btnStages[0].transform.Find("Outline").gameObject.SetActive(true);
                break;
            case GAME_STAGE.CONSTRUCTION:
                txtStage.text = "CONSTRUCTION";
                pnlStages.SetActive(true);
                pnlConstructions.SetActive(true);
                txtTimer.gameObject.SetActive(true);
                btnStages[0].transform.Find("Outline").gameObject.SetActive(false);
                btnStages[1].transform.Find("Outline").gameObject.SetActive(true);
                break;
            case GAME_STAGE.DEFENSE:
                txtStage.text = "DEFENSE";
                pnlStages.SetActive(false);
                pnlConstructions.SetActive(false);
                txtTimer.gameObject.SetActive(false);
                break;
        }
    }

    private void ChangeContructionButton()
    {
        btnConstructions[lateConstructionIndex].transform.Find("Outline").gameObject.SetActive(false);
        btnConstructions[Constructor.instance.constructionIndex].transform.Find("Outline").gameObject.SetActive(true);
        lateConstructionIndex = Constructor.instance.constructionIndex;
    }

    public void Log(string text, Color color)
    {
        txtLog.text = text;
        txtLog.color = color;

        CancelInvoke("ClearLog");
        Invoke("ClearLog", text.Length / 10f);
    }

    private void ClearLog()
    {
        txtLog.text = "";
    }

    public void OnBtnPlay()
    {
        string field = transform.Find("Start").Find("Panel").
                        Find("FldSeed").GetComponent<InputField>().text;
        print(field);

        GameManager.instance.seed = (field != "" ?
            int.Parse(field) : (int)(Random.value * 10000f));


        transform.Find("Start").gameObject.SetActive(false);
        SoundManager.PlaySound("button");
        GameManager.instance.StartGame();
    }

    public void OnBtnQuit()
    {
        SoundManager.PlaySound("button");
        Application.Quit();
    }

    public void SetActive(string name, bool state)
    {
        transform.Find(name).gameObject.SetActive(state);
    }

    public void FadeIn(string name)
    {
        StartCoroutine("DoFadeIn", transform.Find(name).GetComponent<CanvasGroup>());
    }

    private IEnumerator DoFadeIn(CanvasGroup group)
    {
        group.alpha = 0f;

        while (group.alpha < .9f)
        {
            yield return new WaitForEndOfFrame();
            group.alpha += 0.01f;
        }
    }
}
