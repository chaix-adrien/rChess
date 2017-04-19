using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public enum team
    {
        BLACK,
        WHITE,
        NONE,
        BOTH,
    }

    public enum state
    {
        SELECT,
        MOVE,
        ANIMATION,
    }

    public static  team teamTurn = team.WHITE;
    public static state gameState;
	public Piece selected_piece;
    public AudioClip invalidSound;
    public AudioClip validSound;
    public static team teamWin = team.NONE;
    public static GameObject _toPromote = null;
    public static Scene boardScene;
    private AudioSource _audioSource;
    private Roi[] _kings = new Roi[2];
    public static GameManager instance;
    private List<Piece>[] _teamPieces = new List<Piece>[2];
    private List<KeyValuePair<Piece, Vector2>> _lastMove = new List<KeyValuePair<Piece, Vector2>>();
    // Use this for initialization

    private Vector3[] _defaultRotation = new Vector3[2];
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start () {
        instance = this;
        boardScene = SceneManager.GetActiveScene();
        _audioSource = GetComponent<AudioSource>();
        _teamPieces[(int)GameManager.team.WHITE] = new List<Piece>();
        _teamPieces[(int)GameManager.team.BLACK] = new List<Piece>();
        GameObject[] pieces;

        pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Piece pieceInstance = piece.GetComponent<Roi>();
            if (pieceInstance)
            {
                _kings[(int)pieceInstance.team] = (Roi)pieceInstance;
            }
            pieceInstance = piece.GetComponent<Piece>();
            _teamPieces[(int)pieceInstance.team].Add(pieceInstance);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isTeamOnMat(team.WHITE))
            teamWin = team.BLACK;
        if (isTeamOnMat(team.BLACK))
            teamWin = team.WHITE;
        if (isTeamOnDraw(team.WHITE))
            teamWin = team.BOTH;
        if (isTeamOnDraw(team.BLACK))
            teamWin = team.BOTH;

        if (Input.GetKeyDown(KeyCode.Z) && _lastMove.Count != 0)
        {
            KeyValuePair<Piece, Vector2> move = _lastMove[_lastMove.Count - 1];
            move.Key.moveOf((int)move.Value.x * -1, (int)move.Value.y * -1);
            _lastMove.Remove(move);
            nextTurn();
        }
    }

    public void saveDefaultRotation(GameObject obj)
    {
        _defaultRotation[(int)obj.GetComponent<PieceType>().team] = obj.transform.eulerAngles;
    }

    void unselectAll()
    {
        GameObject[] pieces;

        pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Piece pieceInstance = piece.GetComponent<Piece>();
            pieceInstance.unselect();
        }
    }

    public bool selectPiece(Piece _toSelect)
    {
        if (gameState == state.ANIMATION || _toSelect.team != teamTurn)
            return false;
        unselectAll();
        selected_piece = _toSelect;
        gameState = state.MOVE;
        return true;
    }

    public void unselectPiece()
    {
        selected_piece = null;
        if (gameState == state.MOVE)
            gameState = state.SELECT;
    }

    void nextTurn()
    {

        if (teamTurn == team.WHITE)
            teamTurn = team.BLACK;
        else
            teamTurn = team.WHITE;
        GameObject.Find("Camera").GetComponent<Camera>().rotate();
    }

    public void animationEnd()
    {
        gameState = state.SELECT;
    }

    public bool moveTo(int x, int z)
    {
        if (gameState == state.MOVE)
        {
            int dX = x - (int)selected_piece.x;
            int dZ = z - (int)selected_piece.z;
            if (selected_piece.team == team.WHITE)
                dZ *= -1;
            _lastMove.Add(new KeyValuePair<Piece, Vector2>(selected_piece, new Vector2(dX, dZ)));
            if (selected_piece.moveOf(dX, dZ))
            {
                gameState = state.ANIMATION;
                nextTurn();
                unselectAll();
                _audioSource.PlayOneShot(validSound, 1.0f);

                return true;
            } else
            {
                _lastMove.RemoveAt(_lastMove.Count - 1);
                _audioSource.PlayOneShot(invalidSound, 0.5f);
            }
        }
        return false;
    }

    public bool canSomeoneMoveInTeam(team _team)
    {
        foreach (Piece piece in _teamPieces[(int)_team])
        {
            if (piece.alive == true)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int z = 0; z < 8; z++)
                    {
                        int dX = x - (int)piece.x;
                        int dZ = z - (int)piece.z;
                        if (_team == team.WHITE)
                            dZ *= -1;
                        if (piece.canMoveOf(dX, dZ, true))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool isTeamOnMat(team _team)
    {
        if (isTeamOnEchec(_team) && !canSomeoneMoveInTeam(_team))
            return true;
        return false;
    }

    public bool isTeamOnDraw(team _team)
    {
        if (!isTeamOnEchec(_team) && !canSomeoneMoveInTeam(_team))
            return true;
        return false;
    }

    public bool isTeamOnEchec(team team)
    {
        team otherTeam = (team == team.WHITE) ? team.BLACK : team.WHITE;
        Roi king = _kings[(int)team];
        foreach (Piece ennemy in _teamPieces[(int)otherTeam])
        {
            if (ennemy.alive == true)
            {
                int dX = (int)king.x - (int)ennemy.x;
                int dZ = (int)king.z - (int)ennemy.z;
                if (ennemy.team == team.WHITE)
                    dZ *= -1;
                if (ennemy.canMoveOf(dX, dZ, false))
                {
                    king.GetComponent<Highlight>().on(new Color32(191, 10, 10, 255));
                    ennemy.GetComponent<Highlight>().on(new Color32(184, 87, 0, 255));
                    return true;
                }
            }
        }
        return false;
    }

    public void PromoteTo(GameObject _destObject)
    {
        _teamPieces[(int)_toPromote.GetComponent<PieceType>().team].Remove(_toPromote.GetComponent<Piece>());
        Piece save = _toPromote.GetComponent<Piece>();
        Destroy(_toPromote.GetComponent<Piece>());
        _toPromote.GetComponent<MeshFilter>().mesh = _destObject.GetComponent<MeshFilter>().mesh;
        
        Piece copy;

        PieceType.Type type = _destObject.GetComponent<PieceType>().type;
        if (type == PieceType.Type.FOU)
            copy = _toPromote.AddComponent<Fou>();
        else if (type == PieceType.Type.TOUR)
            copy = _toPromote.AddComponent<Tour>();
        else if (type == PieceType.Type.REINE)
            copy = _toPromote.AddComponent<Reine>();
        else if (type == PieceType.Type.CAVALIER)
            copy = _toPromote.AddComponent<Cavalier>();
        else
            copy = _toPromote.AddComponent<Pion>();
        // Copied fields can be restricted with BindingFlags

        GameManager.team team = _destObject.GetComponent<PieceType>().team;
        copy.team = team;
        copy.x = save.x;
        copy.z = save.z;
        _toPromote.transform.eulerAngles = _defaultRotation[(int)team];
        _toPromote.GetComponent<PieceType>().type = type;
        _teamPieces[(int)team].Add(copy);
        isTeamOnMat(team == GameManager.team.WHITE ? GameManager.team.BLACK : GameManager.team.WHITE);
        SceneManager.UnloadScene("Promote");
    }
}
