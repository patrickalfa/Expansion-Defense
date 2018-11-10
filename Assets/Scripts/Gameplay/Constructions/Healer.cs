using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Construction
{
    public int healRateValue;

    private LineRenderer _line;

    protected override void Awake()
    {
        base.Awake();
        _line = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (!built)
            return;

        if (!_line.enabled)
        {
            if (GameManager.instance.stage == GAME_STAGE.EXPANSION &&
                GameManager.instance._base.health < GameManager.instance._base.maxHealth)
                _line.enabled = true;
        }
        else
        {
            if (GameManager.instance.stage == GAME_STAGE.DEFENSE ||
                GameManager.instance._base.health == GameManager.instance._base.maxHealth)
                _line.enabled = false;
        }
    }

    public override bool Build(Node node)
    {
        if (!base.Build(node))
            return false;

        GameManager.instance.healRate += healRateValue;

        _line.SetPosition(0, _transform.position);
        _line.SetPosition(1, GameManager.instance._baseNode.transform.position);

        return true;
    }

    public override void Demolish()
    {
        GameManager.instance.healRate -= healRateValue;
        base.Demolish();
    }
}
