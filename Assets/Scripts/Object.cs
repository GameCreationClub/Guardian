using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }
    public Vector2 Vector2Position
    {
        get { return new Vector2(X, Y); }
    }
    public Vector3 Vector3Position
    {
        get { return new Vector3(X, Y); }
    }

    private void OnMouseOver()
    {
        GameManager.instance.OnObjectHover(this);
    }

    private void OnMouseDown()
    {
        GameManager.instance.OnObjectClicked(this);
    }
}
