using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public int worldWidth = 20;
    public int worldHeight = 20;

    public float cubeSize = 2;

    public static Board instance;
    public float positionX = 0;
    public float positionY = 0;
    public float positionZ = 0;
    private Piece _lastRemoved;
    void Start()
    {
        instance = this;
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight;  z++)
            {
                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.parent = transform;

                if (x % 2 == 0)
                    if (z % 2 == 0)
                        block.GetComponent<Renderer>().material.color = Color.black;
                    else
                        block.GetComponent<Renderer>().material.color = Color.white;
                else
                {
                    if (z % 2 == 0)
                        block.GetComponent<Renderer>().material.color = Color.white;
                    else
                        block.GetComponent<Renderer>().material.color = Color.black;
                }
                float xP = positionX + x * cubeSize;
                float zP = positionZ + z * cubeSize;
                block.transform.localScale = new Vector3(cubeSize, 0.01f, cubeSize);
                block.transform.localPosition = new Vector3(xP, positionY, zP);
                block.AddComponent<Tile>();
                block.GetComponent<Tile>().x = x;
                block.GetComponent<Tile>().z = z;

            }
        }
    }

    //Probably unwanted in update
    void Update()
    {
        
    }


    public GameManager.team getTeamAt(int x, int z)
    {
        GameObject[] pieces;

        pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Piece pieceInstance = piece.GetComponent<Piece>();
            if (pieceInstance.x == x && pieceInstance.z == z)
                return pieceInstance.team;
        }
        return GameManager.team.NONE;
    }

    public Piece removePieceAt(int x, int z)
    {
        GameObject[] pieces;

        pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Piece pieceInstance = piece.GetComponent<Piece>();
            if (pieceInstance.x == x && pieceInstance.z == z)
            {
                _lastRemoved = pieceInstance;
                pieceInstance.remove();
                return pieceInstance;
            }  
        }
        return null;
    }
    public void undoRemovePiece()
    {
        if (!_lastRemoved)
            return;
        _lastRemoved.undoRemove();
    }
}
