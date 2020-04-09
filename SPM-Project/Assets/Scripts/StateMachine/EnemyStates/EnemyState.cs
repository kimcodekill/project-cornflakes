using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    private SimpleEnemy enemy;

    public SimpleEnemy Enemy => enemy = enemy != null ? enemy : (SimpleEnemy)Owner;
}