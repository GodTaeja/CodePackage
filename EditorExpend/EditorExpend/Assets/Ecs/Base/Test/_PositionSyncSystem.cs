using UnityEngine;

public class PositionSyncSystem : MSystem
{
    public PositionSyncSystem(MEntityManager manager)
    {
        EntityManager = manager;
    }

    public void Update(float inter,bool sync=false)
    {
        var list = EntityManager.GetComponents<ComPosition>(C.Position);

        int n = list.Count;
        for (int i = 0; i < n; i++)
        {

            var comPosition = list[i];
            var syncPos = list[i].Get<ComPosition>(C.ViewPosition,false);
            if (syncPos == null)
            {
                syncPos = list[i].Entity.Add<ComPosition>(C.ViewPosition);
                syncPos.Position = comPosition.Position;
                syncPos.LastPosition = comPosition.LastPosition;
            }

            if (sync)
            {
                syncPos.Position = comPosition.Position;
                syncPos.LastPosition = comPosition.LastPosition;
            }
                

            var lastPos = syncPos.LastPosition;
            var newPos = syncPos.Position;

            if (comPosition.HasFirstSyncRotation)
            {
                if (lastPos == newPos)
                    continue;
            }

            var comTransform = comPosition.Get<_ComTransform>(C.Transform, false);

            if (comTransform == null)
                continue;

            if (!comPosition.HasFirstSyncRotation)
                comPosition.HasFirstSyncRotation = true;

            var transform = comTransform.Transform;

            var pos = TrueSync.TSVector3.Lerp(lastPos, newPos, inter);
            transform.localPosition = UnityPositionUtils.ToVector3(pos);
        }
    }
}
