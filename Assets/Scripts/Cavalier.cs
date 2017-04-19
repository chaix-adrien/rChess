using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavalier : Piece {

    // Use this for initialization
    void Update()
    {
        base.Update();
    }

    void Start()
    {
        GameManager.instance.saveDefaultRotation(gameObject);
        base.Start();
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        if (!(Mathf.Abs(_x) == 2 && Mathf.Abs(_z) == 1) &&
           !(Mathf.Abs(_x) == 1 && Mathf.Abs(_z) == 2))
            return false;
        return base.canMoveOf(_x, _z, checkEchec);
    }
}
