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

//                        |                |   x - 2, y    |                |
//       -----------------------------------------------------------------------------------
//                       |                |   x - 1, y    |                |
//      -----------------------------------------------------------------------------------
//          x, y - 2    |    x, y - 1    |      x, y     |   x, y + 1     |     x, y + 2
//      ----------------------------------------------------------------------------------
//                     |                |   x + 1, y    |                |  
//      ---------------------------------------------------------------------------------
//                    |                |   x + 2, y    |                |

public class Axis : Figure
{
    public override void Select()
    {
        StepCoordinate.Clear();

        Vector2Int temp = new Vector2Int(coordinate.x, coordinate.y - 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x, coordinate.y - 2));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x, coordinate.y + 1);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x, coordinate.y + 2));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x - 1, coordinate.y);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x - 2, coordinate.y));
            else
                StepCoordinate.Add(temp);
        }

        temp = new Vector2Int(coordinate.x + 1, coordinate.y);

        if (Cell.cells.ContainsKey(temp))
        {
            if (Cell.cells[temp].GetLocked)
                StepCoordinate.Add(new Vector2Int(coordinate.x + 2, coordinate.y));
            else
                StepCoordinate.Add(temp);
        }
    }
}
