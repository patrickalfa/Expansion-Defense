using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text txtTimer;
    public Text txtWood;
    public Text txtStone;
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
        txtTimer.text = GameManager.instance.time.ToString();

        txtWood.text = GameManager.instance.wood +
            " (+" + GameManager.instance.woodRate + ")";

        txtStone.text = GameManager.instance.gold +
            " (+" + GameManager.instance.goldRate + ")";


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
}
