using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reine : Piece {

    // Use this for initialization
    void Update()
    {
        base.Update();
    }

    void Start()
    {
        base.Start();
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        bool done = false;
        if (Tour.rookMoveStyle(this, _x, _z))
            done = base.canMoveOf(_x, _z, checkEchec);
        if (!done && Fou.bishopMoveStyle(this, _x, _z))
            done = base.canMoveOf(_x, _z, checkEchec);
        return done;
    }
}
