using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private HexStack currentStack;
    private Vector3 currentHexStackInitialPos;

    [Header("Data")]
    private GridCell targetCell;

    [Header("Action")]
    public static Action<GridCell> onStackPlaced;
    public static Action onTouchHexa;

    public bool canPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
            ManageControl();
    }

    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            StartMoving();
        else if (gameState == GameManager.GameState.Gameover)
            StopMoving();
        else if (gameState == GameManager.GameState.LevelComplete)
            StopMoving();
    }

    private void StartMoving()
    {
        canPlay = true;
    }

    private void StopMoving()
    {
        canPlay &= false;
    }
    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
        else if (Input.GetMouseButton(0) && currentStack != null)
            ManageMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentStack != null)
            ManageMouseUp();
    }

    private void ManageMouseUp()
    {
        if(targetCell == null)
        {
            currentStack.transform.position = currentHexStackInitialPos;
            currentStack = null;
            return; 
        }

        currentStack.transform.position = targetCell.transform.position.With(y: 0.2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Place();

        targetCell.AssignStack(currentStack);

        onStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentStack = null ;
    }
    private void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMask);

        if (hit.collider == null)
        {
            return;
        }
        onTouchHexa?.Invoke();
        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentHexStackInitialPos = currentStack.transform.position;
    }

    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
        {
            DraggingAboveGround();
        }
        else
            DraggingAboveGridCell(hit);

    }

    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMask);

        if(hit.collider == null)
        {
            return ;
        }

        Vector3 currentStackTargetPos = hit.point.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, currentStackTargetPos, Time.deltaTime * 30);

        targetCell = null;

    }

    private void DraggingAboveGridCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
            DraggingAboveNonOccupiedGridcCell(gridCell);
    }

    private void DraggingAboveNonOccupiedGridcCell(GridCell gridCell)
    {
        Vector3 currentStackTargetPos = gridCell.transform.position.With(y: 2);


        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, currentStackTargetPos, Time.deltaTime * 30);

        targetCell = gridCell;
    }



    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
