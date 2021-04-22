public class AI_BreakBlockTask : AI_BaseTask
{
    private readonly float BLOCK_BREAK_MIN_DISTANCE = 1f;
    private readonly float BLOCK_BREAK_MAX_DISTANCE = 5f;
    private BaseBlock block;

    public override void Start()
    {
        base.Start();

        block = BlockManager.Instance.GetBlockAtPosition(transform.position);
        if (!block)
        {
            Destroy();
        }
    }

    public override float GetMinDistance()
    {
        return BLOCK_BREAK_MIN_DISTANCE;
    }

    public override float GetMaxDistance()
    {
        return BLOCK_BREAK_MAX_DISTANCE;
    }

    public override void Do()
    {
        worker.GetComponent<BaseCharacter>().AttackBlock(block);
    }

    public override bool Done()
    {
        return block == null;
    }

    public override string GetDebugTaskDescription()
    {
        return $"AI_BreakBlockTask | {transform.position}";
    }
}
