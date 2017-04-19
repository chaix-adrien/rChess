using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseEnter()
    {
        if (GetComponent<PieceType>().team == GameManager.teamTurn)
        {
            GameObject[] teamPieces = GameObject.FindGameObjectsWithTag("Piece");
            List<GameObject> toHighlight = new List<GameObject>();
            toHighlight.AddRange(teamPieces);
            toHighlight = toHighlight.FindAll(piece => piece.GetComponent<PieceType>().type == GetComponent<PieceType>().type && piece.GetComponent<PieceType>().team == GetComponent<PieceType>().team);
            foreach (GameObject pieceObj in toHighlight)
            {
                pieceObj.GetComponent<Highlight>().on();
            }
        }
        else
        {
            GetComponent<Highlight>().off();
        }
    }

    void OnMouseExit()
    {
        if (GetComponent<PieceType>().team == GameManager.teamTurn)
        {
            GameObject[] teamPieces = GameObject.FindGameObjectsWithTag("Piece");
            List<GameObject> toHighlight = new List<GameObject>();
            toHighlight.AddRange(teamPieces);
            toHighlight = toHighlight.FindAll(piece => piece.GetComponent<PieceType>().type == GetComponent<PieceType>().type && piece.GetComponent<PieceType>().team == GetComponent<PieceType>().team);
            foreach (GameObject pieceObj in toHighlight)
            {
                pieceObj.GetComponent<Highlight>().off();
            }
        }
    }

}
