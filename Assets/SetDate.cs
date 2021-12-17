using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDate : MonoBehaviour
{
    public GameManager GameManager;
    public int x, y, number;

    private void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    private void OnMouseDown()
    {
        GameManager.Check(this.gameObject);
    }
}
