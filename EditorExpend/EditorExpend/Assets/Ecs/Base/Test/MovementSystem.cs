using System.Collections.Generic;

public class MovementSystem : MSystem
{
    public MovementSystem(MEntityManager manager)
    {
        EntityManager = manager;
    }

    public override bool EnableJob => true;

    private readonly int[] _HandleComponents = { C.Movement,C.Position};

    public override int[] HandleComponents => _HandleComponents;
    public override MSystemJob GetJob(MEntity entity)
    {
        ComMovement _comMovement = entity.Get<ComMovement>(C.Movement);
        ComPosition _comPosition = entity.Get<ComPosition>(C.Position);
        return new MSystemJob<ComMovement, ComPosition>(_comMovement, _comPosition, UpdatePosition);
    }

    public void _Update(float deltaTime)
    {
        List<ComMovement> coms = EntityManager.GetLogicComponents<ComMovement>(C.Movement);

        int n = coms.Count;

        for (int i = 0; i < n; i++)
        {
            var move = coms[i];
            ComPosition comPosition = move.Get<ComPosition>(C.Position);
            UpdatePosition(move,comPosition, deltaTime);
        }
    }

    public static void UpdatePosition(ComMovement move, ComPosition comPosition,float deltaTime)
    {
        var m = move.MoveDirection * move.MoveSpeed * deltaTime;

        //arrive
        if (move.HasTarget)
        {
            var d = move.MoveTarget - comPosition.Position;
            if (d.sqrMagnitude <= m.sqrMagnitude)
            {
                move.IsArrived = true;
                m = d;
            }
        }

        comPosition.Position += m;
    }

}
