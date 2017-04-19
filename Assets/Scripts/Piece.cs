using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piece : MonoBehaviour {
    public float x;
	public float z;
    public bool alive = true;
    public GameManager.team team;
    protected int _sens;
    protected bool _selected;
    private Vector3 _target;
    private bool _onMove = false;
    private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private float _posXAtRemove;
    private float _posZAtRemove;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    protected void Start()
    {
        if (team == GameManager.team.WHITE)
            _sens = -1;
        else
            _sens = 1;
        Material newMat = Resources.Load(team == GameManager.team.WHITE ? "Materials/jade" : "Materials/blackmarble", typeof(Material)) as Material;
        GetComponent<MeshRenderer>().material = newMat;
        _selected = false;
        Board.instance = GameObject.Find("Board").GetComponent<Board>();
        transform.position = new Vector3(Board.instance.positionX + x, Board.instance.positionY, Board.instance.positionZ + z);
    }

    // Update is called once per frame
    protected void Update () {
        if (_selected)
            GetComponent<Highlight>().on();
        if (_onMove && alive)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _target, ref velocity, smoothTime);
                if (transform.position == _target)
                    _onMove = false;
            }
        else
            transform.position = new Vector3(Board.instance.positionX + x, Board.instance.positionY, Board.instance.positionZ + z);
        if (!alive)
            GetComponent<Highlight>().off();
    }


    void OnMouseDown()
    {
        if (team == GameManager.teamTurn && !_selected && GameManager.teamWin == GameManager.team.NONE)
        {
            select();
        } else if (team == GameManager.teamTurn)
        {
            unselect();
        }
        else if (GameManager.gameState == GameManager.state.MOVE)
          GameManager.instance.moveTo((int)x, (int)z);
    }

    public void select()
    {
        if (!alive)
            return;
        if (GameManager.instance.selectPiece(this))
            _selected = true;
    }

    public void unselect()
    {
        GetComponent<Highlight>().off();
        GameManager.instance.unselectPiece();
        _selected = false;
    }


    public virtual bool canMoveOf(int _x, int _z, bool checkEchec)
    {
        float saveX = x;
        float saveZ = z;
        Piece removed = null;

        GameManager.team teamDest = getTeamOf(_x, _z);
        if (teamDest == team)
            return false;
        if (teamDest != GameManager.team.NONE)
        {
            removed = Board.instance.removePieceAt(_x + (int)x, (int)z + _z * _sens);
        }
 
        x = x + _x;
        z = z + _z;
        bool res = true;
        if (checkEchec)
            res = !GameManager.instance.isTeamOnEchec(team);
        x = saveX;
        z = saveZ;
        if (removed)
            removed.undoRemove();
        return res;
    }

    bool doMoveOf(int _x, int _z)
    {
        GameManager.team teamDest = getTeamOf(_x, _z);
        if (teamDest != GameManager.team.NONE)
            Board.instance.removePieceAt(_x + (int)x, (int)z + _z * _sens);
        _target.x = x + _x;
        _target.y = 0;
        _target.z = z + _z * _sens;
        x = _target.x;
        z = _target.z;
        _onMove = true;
        return true;
    }

    public virtual bool moveOf(int _x, int _z)
    {
        if (canMoveOf(_x, _z, true))
            return doMoveOf(_x, _z);
        return false;
    }

    public void moveAt(float _x, float _z)
    {
        x = _x;
        z = _z;
    }

    public GameManager.team getTeamOf(int _x, int _z)
    {
        _z *= _sens;
        return Board.instance.getTeamAt((int)x + _x, (int)z + _z);
    }

    public void remove()
    {
        alive = false;
        _posXAtRemove = x;
        _posZAtRemove = z;
        GetComponent<Highlight>().off();
        gameObject.tag = "DeadPiece";
        if (team == GameManager.team.WHITE)
            GameObject.Find("DeadBoardWhite").GetComponent<DeadBox>().add(this);
        else
            GameObject.Find("DeadBoardBlack").GetComponent<DeadBox>().add(this);
    }

    public void undoRemove()
    {
        alive = true;
        gameObject.tag = "Piece";
        if (team == GameManager.team.WHITE)
            GameObject.Find("DeadBoardWhite").GetComponent<DeadBox>().remove(this);
        else
            GameObject.Find("DeadBoardBlack").GetComponent<DeadBox>().remove(this);
        setScale(1.0f);
        moveAt(_posXAtRemove, _posZAtRemove);
    }

    public void setScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
