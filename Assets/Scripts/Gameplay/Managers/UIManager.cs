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

    public GameObject pnlConstructions;
    public Button[] btnConstructions;

    ///////////////////////////////////

    private int lateConstructionIndex = 0;

    ///////////////////////////////////

    private void LateUpdate()
    {
        txtTimer.text = GameManager.instance.time.ToString();

        txtWood.text = "W: " + GameManager.instance.wood +
            " (+" + GameManager.instance.woodRate + ")";

        txtStone.text = "R: " + GameManager.instance.stone +
            " (+" + GameManager.instance.stoneRate + ")";


        if (GameManager.instance.lateStage != GameManager.instance.stage)
        {
            switch (GameManager.instance.stage)
            {
                case GAME_STAGE.EXPANSION:
                    txtStage.text = "EXPANSION";
                    break;
                case GAME_STAGE.CONSTRUCTION:
                    txtStage.text = "CONSTRUCTION";
                    pnlConstructions.SetActive(true);
                    break;
                case GAME_STAGE.DEFENSE:
                    txtStage.text = "DEFENSE";
                    pnlConstructions.SetActive(false);
                    break;
            }
        }

        if (lateConstructionIndex != GameManager.instance.constructionIndex)
        {
            btnConstructions[lateConstructionIndex].transform.Find("Outline").gameObject.SetActive(false);
            btnConstructions[GameManager.instance.constructionIndex].transform.Find("Outline").gameObject.SetActive(true);
            lateConstructionIndex = GameManager.instance.constructionIndex;
        }
    }
}
