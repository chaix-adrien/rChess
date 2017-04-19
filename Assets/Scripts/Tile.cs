using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int x;
    public int z;
    private Color _saveColor = Color.green;
    private GameManager _gameManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("Glass Chess Set").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
    }

    void OnMouseUp()
    {
        _gameManager.moveTo(x, z);        
    }

    void OnMouseEnter()
    {
        _saveColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.grey;
     }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = _saveColor;
    }
}
