using UnityEngine;

public class TreeBlock : BaseBlock
{
    public bool breaksWholeTree = false;
    public LayerMask treeLayer;
    private bool markedToDestroy;

    private void Update()
    {
        if (markedToDestroy)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(gameObject);

        if (breaksWholeTree)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, treeLayer);
            if (hit && hit.collider.GetComponent<TreeBlock>())
            {
                hit.collider.GetComponent<TreeBlock>().DieForced();
            }

            hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, treeLayer);
            if (hit && hit.collider.GetComponent<TreeBlock>())
            {
                hit.collider.GetComponent<TreeBlock>().DieForced();
            }

            hit = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, treeLayer);
            if (hit && hit.collider.GetComponent<TreeBlock>())
            {
                hit.collider.GetComponent<TreeBlock>().DieForced();
            }

            hit = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, treeLayer);
            if (hit && hit.collider.GetComponent<TreeBlock>())
            {
                hit.collider.GetComponent<TreeBlock>().DieForced();
            }
        }

        if (Game.Items.ContainsKey(itemID))
        {
            GameObject item = Instantiate(Resources.Load<GameObject>("Items/Wood_Item"));
            item.transform.position = transform.position;
        }
    }

    public void DieForced()
    {
        breaksWholeTree = true;
        markedToDestroy = true;
    }
}
