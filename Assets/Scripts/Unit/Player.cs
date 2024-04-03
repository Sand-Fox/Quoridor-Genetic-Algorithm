using UnityEngine;

public class Player : BaseUnit
{
    protected override void Awake()
    {
        base.Awake();
        if (view.IsMine) ReferenceManager.Instance.player = this;
        else ReferenceManager.Instance.enemy = this;
    }
}
