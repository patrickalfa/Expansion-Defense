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

    private void LateUpdate()
    {
        txtTimer.text = GameManager.instance.time.ToString();

        txtWood.text = "W: " + GameManager.instance.wood +
            " (+" + GameManager.instance.woodRate + ")";

        txtStone.text = "R: " + GameManager.instance.stone +
            " (+" + GameManager.instance.stoneRate + ")";

        switch (GameManager.instance.stage)
        {
            case GAME_STAGE.EXPANSION:
                txtStage.text = "EXPANSION";
                break;
            case GAME_STAGE.CONSTRUCTION:
                txtStage.text = "CONSTRUCTION";
                break;
            case GAME_STAGE.DEFENSE:
                txtStage.text = "DEFENSE";
                break;
        }
    }
}
