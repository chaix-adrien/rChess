using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PiecePromote : MonoBehaviour {
    // Update is called once per frame
    
    void Start()
    {
        GetComponent<PieceType>().team = GameManager.teamTurn == GameManager.team.WHITE ? GameManager.team.BLACK : GameManager.team.WHITE;
        Material newMat = Resources.Load(GetComponent<PieceType>().team == GameManager.team.WHITE ? "Materials/jade" : "Materials/blackmarble", typeof(Material)) as Material;
        GetComponent<MeshRenderer>().material = newMat;
    }

    void Update()
    {

    }
    void OnMouseDown()
    {
        GameManager.instance.PromoteTo(gameObject);
    }

    void OnMouseEnter()
    {
        GetComponent<Highlight>().on();
    }
}
