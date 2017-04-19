using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
    private MeshRenderer rend;
    private Shader _lastShader;
    public bool state;

    // Use this for initialization
    void Start () {
        rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (GameManager.teamWin ==  GetComponent<PieceType>().team || state == true)
            on();
        if (GameManager.teamWin == GameManager.team.BOTH)
            on(new Color32(255, 255, 255, 255));
    }

    public void on()
    {
        on((GetComponent<PieceType>().team == GameManager.teamTurn) ? new Color32(191, 166, 22, 255) : new Color32(184, 87, 0, 255));
    }

    public void on(Color color)
    {
        Shader newShader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        state = true;
        if (_lastShader != newShader)
        {
            rend.material.SetColor("_OutlineColor", color);
            rend.material.shader = newShader;
        }
        _lastShader = newShader;
    }

    public void off()
    {
        Shader newShader = Shader.Find("Standard");
        state = false;
        if (_lastShader != newShader)
            rend.material.shader = newShader;
        _lastShader = newShader;
    }

    void OnMouseEnter()
    {
        if (GameManager.gameState != GameManager.state.ANIMATION)
            on();
    }

    void OnMouseExit()
    {
       off();
    }
}
