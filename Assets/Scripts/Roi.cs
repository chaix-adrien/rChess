using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roi : Piece {
    // Use this for initialization
    void Update()
    {
        if (GameManager.instance.isTeamOnEchec(team) && !_selected)
           GetComponent<Highlight>().on(new Color32(191, 10, 10, 255));
        if (_selected)
        {
            GetComponent<Highlight>().off();
            GetComponent<Highlight>().on();
        }

        base.Update();
    }

    void Start()
    {
        base.Start();
    }

    override public bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        if (Mathf.Abs(_x) > 1 || Mathf.Abs(_z) > 1)
            return false;
        return base.canMoveOf(_x, _z, checkEchec);
    }
}
