using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Figure : MonoBehaviour, IPointerClickHandler
{
    public delegate void SimpleEvent();
    public static event SimpleEvent Step;   //Event about the end of a move


    protected Image icon = null;
    public Vector2Int GetCoordinate { get { return coordinate; } private set { } }
    protected Vector2Int coordinate;
    protected static Figure SelectFigure = null;
    protected List<Vector2Int> StepCoordinate = new List<Vector2Int>();

    protected bool first_player = true; //true - first player figure, false - second player figure

    //An array is created with coordinates that the chip can move to
    public abstract void Select();

    //Checking the possibility and moving the shape to the resulting cell
    protected void Move(Cell cell)
    {
        if (StepCoordinate == null || StepCoordinate.Count == 0)
            return;

        bool check = false;

        foreach (var item in StepCoordinate)
        {
            if (item == cell.GetCoordinate)
            {
                check = true;
                break;
            }
        }

        if (check)
        {
            var last_cell = GetComponentInParent<Cell>();

            coordinate = cell.GetCoordinate;
            cell.SetFigure(gameObject);

            Unselect();
            Step?.Invoke();

            last_cell.Unlock();
        }
    }

    protected void Unselect()
    {
        SelectFigure = null;
        transform.localScale = Vector3.one;
        Cell.CheckCell -= Move;
    }

    public void Init(bool first_player, Vector2Int coordinates)
    {
        SelectFigure = null;
        this.first_player = first_player;

        icon = GetComponent<Image>();
        icon.color = (first_player) ? Color.red : Color.blue;

        this.coordinate = coordinates;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Playground.FirstPlayer == first_player)
        {
            if (SelectFigure == null)
            {
                SelectFigure = this;
                transform.localScale = Vector3.one * 0.7f;
                Cell.CheckCell += Move;

                Select();
            }
            else if (SelectFigure == this)
            {
                Unselect();
            }
        }
    }
}
