using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//          ------------> Y
//         |
//         |
//         |
//         |
//        \/
//         X

//         x - 2, y - 2   |                |               |                |  x - 2, y + 2
//       -----------------------------------------------------------------------------------
//                       |  x - 1, y - 1  |               |   x - 1, y + 1 |
//      -----------------------------------------------------------------------------------
//                      |                |      x, y     |                |
//      ----------------------------------------------------------------------------------
//                     |  x + 1, y - 1  |               |   x + 1, y + 1 |               
//      ---------------------------------------------------------------------------------
//       x + 2, y - 2 |                |               |                |  x + 2, y + 2             

public class Diagonal : Figure
{
    public override void Select()
    {
        StepCoordinate.Clear();

        Vector2Int temp = new Vector2Int(coordinate.x - 1, coordinate.y - 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x - 2, coordinate.y - 2));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x - 1, coordinate.y + 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x - 2, coordinate.y + 2));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x + 1, coordinate.y - 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x + 2, coordinate.y - 2));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x + 1, coordinate.y + 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x + 2, coordinate.y + 2));
            else
                StepCoordinate.Add(temp);
        }
    }
}
