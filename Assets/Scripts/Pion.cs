using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pion : Piece {
    // Use this for initialization
    private bool _hasMoved = false;
    // Update is called once per frame


    void Update () {
        base.Update();
	}

    void Start()
    {
        base.Start();
    }

    private void promote()
    {
        GameManager._toPromote = gameObject;
        SceneManager.LoadScene("Promote", LoadSceneMode.Additive);
    }

    override public bool moveOf(int _x, int _z)
    {

        if (base.moveOf(_x, _z))
        {
            if (team == GameManager.team.WHITE && z == 0 ||
                team == GameManager.team.BLACK && z == 7)
            {
                promote();
            }
            _hasMoved = true;
            return true;
        }
        return false;
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        bool done = false;
        GameManager.team teamDest = getTeamOf(_x, _z);
        if (_z <= 0 || _x < -1 || _x > 1)
            return false;
        if (!_hasMoved && _z == 2 && _x == 0 && teamDest == GameManager.team.NONE
            && getTeamOf(_x, _z - 1) == GameManager.team.NONE)
            done = base.canMoveOf(_x, _z, checkEchec);
        if (_z == 1 && _x != 0 && teamDest != GameManager.team.NONE)
            done = base.canMoveOf(_x, _z, checkEchec);
        if (_z == 1 && _x == 0 && teamDest == GameManager.team.NONE)
            done = base.canMoveOf(_x, _z, checkEchec);
        return done;
    }

}
