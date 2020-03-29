using UnityEngine;

public class _ComTransform : MComponent
{
    public Transform Transform;
    public bool DonotDestroy = false;

    public Pool<GameObject> GameObjectPool { get; set; }


    public override void OnDestroy()
    {
        base.OnDestroy();
        if(DonotDestroy)
            return;
        if (Transform != null)
        {
#if !DISABLE_MESH_COMBINE
            Transform.position=new Vector3(0,UnityPositionUtils.hideY,0);
#endif
            if (GameObjectPool != null)
            {
                GameObjectPool.Put(Transform.gameObject);
            }
            else
                Object.Destroy(Transform.gameObject);
        }
    }
}
