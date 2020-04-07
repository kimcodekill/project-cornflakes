using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State
{
    private SimpleEnemy enemy;

    public SimpleEnemy Enemy => enemy = enemy != null ? enemy : (SimpleEnemy)Owner;
}
