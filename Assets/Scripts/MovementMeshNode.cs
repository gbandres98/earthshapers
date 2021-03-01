using System.Collections.Generic;
using UnityEngine;

public class MovementMeshNode : MonoBehaviour
{
    public LayerMask GroundLayer;

    private bool isValid = false;
    private Vector2 position;
    private readonly List<MovementMeshNode> connected = new List<MovementMeshNode>();

    public bool IsValid()
    {
        return isValid;
    }

    private RaycastHit2D BlockAtRelativePos(float x, float y)
    {
        return Physics2D.Raycast(position + new Vector2(x, y), Vector2.zero, 0f, GroundLayer);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public List<MovementMeshNode> GetConnected()
    {
        return connected;
    }

    public void AddToConnected(MovementMeshNode c)
    {
        foreach (MovementMeshNode m in connected)
        {
            if (m.GetPosition() == c.GetPosition())
            {
                return;
            }
        }
        connected.Add(c);
    }

    private void Awake()
    {
        position = GetComponent<Transform>().position;
    }

    private bool IsFree(float x, float y)
    {
        return !(bool)BlockAtRelativePos(x, y);
    }

    private void AddConnectionIfValid(float x, float y, List<Vector2> requirements)
    {
        RaycastHit2D block = BlockAtRelativePos(x, y);
        if (!block)
        {
            return;
        }
        MovementMeshNode node = block.collider.gameObject.GetComponent<MovementMeshNode>();
        if (!node || !node.IsValid())
        {
            return;
        }
        foreach (Vector2 coords in requirements)
        {
            float xR = coords.x;
            float yR = coords.y;
            if (!IsFree(xR, yR))
            {
                return;
            }
        }
        AddToConnected(node);
        node.AddToConnected(this);
    }

    private void Start()
    {
        #pragma warning disable IDE0055
        if (!GlobalMovementMesh.Instance || !IsFree(0, 1) || !IsFree(0, 2))
        {
            isValid = false;
            return;
        }
        isValid = true;
        GlobalMovementMesh.Instance.AddNode(this);
        AddConnectionIfValid( 1, 0, new List<Vector2>());
        AddConnectionIfValid(-1, 0, new List<Vector2>());
        // Need jump
        if (IsFree(0, 3))
        {
            AddConnectionIfValid( 1, 1, new List<Vector2> { new Vector2(-1, 3) });
            AddConnectionIfValid(-1, 1, new List<Vector2> { new Vector2( 1, 3) });
        }
        if (IsFree(0, 3) && IsFree(0, 4))
        {
            AddConnectionIfValid( 1, 2, new List<Vector2> { new Vector2(-1,3), new Vector2(-1,4) });
            AddConnectionIfValid(-1, 2, new List<Vector2> { new Vector2( 1,3), new Vector2( 1,4) });
        }
        if (IsFree(0, 3) && IsFree(0, 4) && IsFree(0, 5))
        {
            AddConnectionIfValid( 1, 3, new List<Vector2> {new Vector2(-1,3), new Vector2(-1,4), new Vector2(-1,5)});
            AddConnectionIfValid(-1, 3, new List<Vector2> {new Vector2( 1,3), new Vector2( 1,4), new Vector2( 1,5)});
        }
        // Drop down
        AddConnectionIfValid( 1,-1, new List<Vector2> { new Vector2( 1,2) });
        AddConnectionIfValid( 1,-2, new List<Vector2> { new Vector2( 1,2), new Vector2( 1,1) });
        AddConnectionIfValid( 1,-3, new List<Vector2> { new Vector2( 1,2), new Vector2( 1,1), new Vector2( 1,0) });
        AddConnectionIfValid(-1,-1, new List<Vector2> { new Vector2(-1,2) });
        AddConnectionIfValid(-1,-2, new List<Vector2> { new Vector2(-1,2), new Vector2(-1,1) });
        AddConnectionIfValid(-1,-3, new List<Vector2> { new Vector2(-1,2), new Vector2(-1,1), new Vector2(-1,0) });
    }
}