using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public static Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();

    public delegate void Vector2Event(Cell cell);
    public static event Vector2Event CheckCell;

    public Vector2Int GetCoordinate { get { return coordinate; } private set { } }
    private Vector2Int coordinate;

    public bool GetLocked { get { return locked; } private set { } }
    private bool locked = false;

    public void Init(Vector2Int coordinate)
    {
        this.coordinate = coordinate;
        cells.Add(coordinate, this);
    }

    void OnDisable()
    {
        cells.Remove(coordinate);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!locked)
        {
            CheckCell?.Invoke(this);
        }
    }

    public bool SetFigure(GameObject obj)
    {
        if (locked)
            return false;

        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        locked = true;

        return true;
    }

    public void Unlock()
    {
        if (transform.childCount == 0)
        {
            locked = false;
        }
    }
}
