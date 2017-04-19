using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tour : Piece {

    // Use this for initialization
    void Update()
    {
        base.Update();
    }

    void Start()
    {
        base.Start();
    }

    public static bool rookMoveStyle(Piece piece, int _x, int _z)
    {
        GameManager.team teamOf;
        if (_x != 0 && _z != 0)
            return false;
        int t = 0;
        int _v = (_x == 0) ? _z : _x;
        for (; t != _v; t += (_v < 0) ? -1 : 1)
            if (t != 0)
            {
                teamOf = piece.getTeamOf((_x == 0) ? 0 : t, (_x == 0) ? t : 0);
                if (teamOf != GameManager.team.NONE)
                    return false;
            }
        teamOf = piece.getTeamOf((_x == 0) ? 0 : t, (_x == 0) ? t : 0);
        if (teamOf == piece.team)
            return false;
        return true;
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        if (rookMoveStyle(this, _x, _z))
            return base.canMoveOf(_x, _z, checkEchec);
        return false;
    }
}
