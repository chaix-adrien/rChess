using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fou : Piece {
    void Update()
    {
        base.Update();
    }

    void Start()
    {
        base.Start();
    }

    public static bool bishopMoveStyle(Piece piece, int _x, int _z)
    {
        GameManager.team teamOf;
        if (Mathf.Abs(_x) != Mathf.Abs(_z))
            return false;

        int tX = 0;
        int tZ = 0;
        for (; tX != _x || tZ != _z;)
        {
            if (tX != 0)
            {
                teamOf = piece.getTeamOf(tX, tZ);
                if (teamOf != GameManager.team.NONE)
                      return false;
            }
            tX += (_x < 0) ? -1 : 1;
            tZ += (_z < 0) ? -1 : 1;
        }
        teamOf = piece.getTeamOf(tX, tZ);
        if (teamOf == piece.team)
            return false;
        return true;
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        if (bishopMoveStyle(this, _x, _z))
            return base.canMoveOf(_x, _z, checkEchec);
        return false;
    }
}
