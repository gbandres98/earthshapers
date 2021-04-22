public class AI_PlaceBlockTask : AI_BaseTask
{
    private readonly float BLOCK_PLACE_MIN_DISTANCE = 1f;
    private readonly float BLOCK_PLACE_MAX_DISTANCE = 5f;
    public int BlockID;
    private bool placed;

    public override void Start()
    {
        base.Start();

        if (BlockManager.Instance.GetBlockAtPosition(transform.position))
        {
            Destroy();
        }
    }

    public override bool AssignWorker(AI_Controller worker)
    {
        BaseCharacter character = worker.GetComponent<BaseCharacter>();

        if (!character)
        {
            return false;
        }

        Inventory inventory = worker.GetComponent<Inventory>();

        if (!inventory || !inventory.HasItem(BlockID, 1))
        {
            return false;
        }

        return base.AssignWorker(worker);
    }

    public override float GetMinDistance()
    {
        return BLOCK_PLACE_MIN_DISTANCE;
    }

    public override float GetMaxDistance()
    {
        return BLOCK_PLACE_MAX_DISTANCE;
    }

    public override void Do()
    {
        placed = worker.GetComponent<BaseCharacter>().PlaceBlock(BlockID, transform.position);
    }

    public override bool Done()
    {
        return placed;
    }

    public override string GetDebugTaskDescription()
    {
        return $"AI_PlaceBlockTask | {Game.Items[BlockID]} | {transform.position}";
    }
}
