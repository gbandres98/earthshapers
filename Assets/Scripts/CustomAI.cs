using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float NextWaypointDistance = .3f;
    public float PathUpdateFrequency = 1f;

    [Header("Behaviour")]
    public float JumpSlopeRequirement = .5f;

    private int currentWaypoint = 0;
    private List<MovementMeshNode> path;
    private Rigidbody2D rb;
    private BaseCharacter character;
    private Transform t;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<BaseCharacter>();
        t = GetComponent<Transform>();
        InvokeRepeating("UpdatePath", 0f, 6000000f);
    }

    private void UpdatePath()
    {
        GlobalMovementMesh mesh = GlobalMovementMesh.Instance;
        MovementMeshNode x = mesh.FindClosestNode(t.position);
        MovementMeshNode y = mesh.FindClosestNode(target.position);
        List<MovementMeshNode> l = mesh.GetRoute(x, y);
        if (l != null)
        {
            path = l;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.Count)
        {
            character.Move(0);
            return;
        }
        Vector2 nodePosition = path[currentWaypoint].GetPosition() + Vector2.up;
        Vector2 direction = (nodePosition - rb.position - new Vector2(0, -0.5f)).normalized;
        bool isGrounded = character.GetIsGrounded();
        if (isGrounded && (direction.y > JumpSlopeRequirement))
        {
            if ((nodePosition.y - rb.position.y - 0.5f) > 1.5f)
            {
                if ((nodePosition.x < rb.position.x) && (path[currentWaypoint - 2].GetPosition().x < rb.position.x))
                {
                    if (nodePosition.x > (rb.position.x - 1))
                    {
                        direction = new Vector2(1, 0);
                    }
                    else
                    {
                        character.Jump();
                        character.Move(-1);
                    }
                }
                else
                {
                    character.Jump();
                }
            }
            else
            {
                character.Jump();
            }
        }

        character.Move((direction.x < 0) ? -1 : 1);
        if (Math.Abs(nodePosition.x - rb.position.x) < NextWaypointDistance && direction.y < 3f)
        {
            ++currentWaypoint;
        }
    }
}