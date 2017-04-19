using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private int _onRotate = 0;
    public int _speed = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_onRotate > 0)
        {
            transform.RotateAround(new Vector3(3.5f, 0, 3.5f), Vector3.up, _speed);
            _onRotate -= _speed;
            if (_onRotate <= 0)
                GameObject.Find("Glass Chess Set").GetComponent<GameManager>().animationEnd();
        }

    }

    public void rotate()
    {
        _onRotate = 180;
    }
}
