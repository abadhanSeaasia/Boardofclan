using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// New
public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;// reference to cell prefab that we are going to create

    [HideInInspector]
    public Cell[,] mAllCells;

    public int boardSize;
 

    public void Create()
    {
        // assigning array size at creation
        mAllCells = new Cell[boardSize, boardSize]; 

        #region Create
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);// create a new cell

                //positions the new cell
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                //setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }
        #endregion

        #region Color
        for (int x = 0; x < boardSize; x+=2)
        {
            for (int y = 0; y < boardSize; y++)
            {
                //offset for everyother line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                //Color
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(255, 5, 0, 255);
            }
        }
        #endregion


        //adjust the board rectransform size so that it is always in centre of the screen
        //this.GetComponent<RectTransform>().sizeDelta =new Vector2(boardSize* 100, boardSize * 100) ;
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        //Bounds Check
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        //Get cell
        Cell targetCell = mAllCells[targetX, targetY];

        //if the cell has a piece
        if(targetCell.mCurrentPiece !=null){
            //if friendly
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;

            //If enemy
            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;
        }

        return CellState.Free;
    }
}
