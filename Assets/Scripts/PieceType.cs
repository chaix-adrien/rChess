using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceType : MonoBehaviour {
    public enum Type {
        PION,
        FOU,
        TOUR,
        CAVALIER,
        REINE,
        ROI
    }

    public Type type;
    public GameManager.team team;
}
