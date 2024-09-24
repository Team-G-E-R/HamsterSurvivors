using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedItem : PassiveItem
{
    private float moveSpeedBonus = 0.1f;

    private void Start()
    {
        PlayerStats.instance.PlayerMoveSpeedModifier += moveSpeedBonus;
        AddModification(new Modification("+10% к скорости передвижения", w => PlayerStats.instance.PlayerMoveSpeedModifier += 0.1f));
        AddModification(new Modification("+10% к скорости передвижения", w => PlayerStats.instance.PlayerMoveSpeedModifier += 0.1f));
        AddModification(new Modification("+20% к скорости передвижения", w => PlayerStats.instance.PlayerMoveSpeedModifier += 0.2f));
        AddModification(new Modification("+30% к скорости передвижения", w => PlayerStats.instance.PlayerMoveSpeedModifier += 0.3f));
    }
}
