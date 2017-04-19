using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBox : MonoBehaviour {
    public GameManager.team team;
    private List<Piece> _content = new List<Piece>();
    private Board _board;
    public int start;
    public int sens;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void add(Piece _toAdd)
    {
        if (_content.Count < 8)
        {
            _toAdd.moveAt(transform.position.x, start + ((float)_content.Count / 2 * sens));
            _toAdd.setScale(0.5f);
            _content.Add(_toAdd);
        }
    }

    public void remove(Piece _toRemove)
    {
        _content.Remove(_toRemove);
    }
}
