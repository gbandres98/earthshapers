using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float NextWaypointDistance = .3f;
    public float PathUpdateInterval = 1f;

    [Header("Behaviour")]
    public float JumpSlopeRequirement = .5f;

    private int currentWaypoint = 0;
    private List<Vector2> path;
    private Rigidbody2D rb;
    private BaseCharacter character;
    private Transform t;
    private Vector3 fallback_target_pos;
    private GlobalMovementMesh globalMovementMesh;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<BaseCharacter>();
        t = GetComponent<Transform>();
        InvokeRepeating("UpdatePath", 0f, PathUpdateInterval);
        globalMovementMesh = GlobalMovementMesh.Instance;
    }

    public void ChangeObjective(float x, float y)
    {
        target = null;
        fallback_target_pos = new Vector3(x, y, 0);
    }

    private void UpdatePath()
    {
        MovementMeshNode x = globalMovementMesh.FindClosestNode(t.position);
        MovementMeshNode y = globalMovementMesh.FindClosestNode(target != null ? target.position : fallback_target_pos);
        List<MovementMeshNode> l = globalMovementMesh.GetRoute(x, y);
        if (l != null)
        {
            path = new List<Vector2>();
            foreach (MovementMeshNode n in l)
            {
                path.Add(n.GetPosition());
            }
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null || path.Count < 2)
        {
            return;
        }
        if (currentWaypoint == 0 && Math.Abs(path[0].y - path[1].y) < 0.1f)
        {
            currentWaypoint = 1;
        }
        if (currentWaypoint >= path.Count)
        {
            // Reached goal, stop
            character.Move(0);
            return;
        }
        bool isGrounded = character.IsGrounded;
        Vector2 nodePosition = path[currentWaypoint] + Vector2.up;
        Vector2 aiCenter = rb.position - new Vector2(0, 0.5f);
        Vector2 vector = nodePosition - aiCenter;
        int direction = (vector.x > 0) ? 1 : -1;
        if (vector.y < -0.5f)
        {
            // Drop down
            character.Move(isGrounded ? direction : 0);
        }
        else if (isGrounded && (vector.y > JumpSlopeRequirement))
        {
            // Jump
            if ((nodePosition.y - aiCenter.y) <= 1.5f)
            {
                // Small jump
                character.Jump();
            }
            else
            {
                // High jump, need horizontal space to avoid bonking head
                if (
                    (direction == -1 && (nodePosition.x > (aiCenter.x - 1.3f))) ||
                    (direction == 1 && (nodePosition.x < (aiCenter.x + 1.3f)))
                )
                {
                    character.Move(-1 * direction);
                }
                else
                {
                    character.Move(direction);
                    character.Jump();
                }
            }
        }
        else
        {
            character.Move(direction);
        }
        if (Vector2.Distance(nodePosition, aiCenter) < NextWaypointDistance)
        {
            ++currentWaypoint;
        }
    }
}