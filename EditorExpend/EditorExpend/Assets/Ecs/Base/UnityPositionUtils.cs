using UnityEngine;
public static class UnityPositionUtils
{
    public static Vector3 ToVector3(TrueSync.TSVector3 v3)
    {
        Vector3 ret=new Vector3(v3.x.AsFloat(), v3.y.AsFloat(), v3.z.AsFloat());
        return ret;
    }

    public static Vector2 ToVector2(TrueSync.TSVector2 v3)
    {
        Vector2 ret = new Vector2(v3.x.AsFloat(), v3.y.AsFloat());
        return ret;
    }

    public static TrueSync.TSVector3 ToTSVector3(Vector3 v3)
    {
        TrueSync.TSVector3 ret = new TrueSync.TSVector3(v3.x,v3.y,v3.z);
        return ret;
    }

    public static Quaternion ToQuaternion(TrueSync.TSQuaternion q)
    {
        Quaternion ret=new Quaternion(q.x.AsFloat(),q.y.AsFloat(),q.z.AsFloat(),q.w.AsFloat());
        return ret;
    }

    public static TrueSync.TSQuaternion ToTSQuaternion(Quaternion q)
    {
        TrueSync.TSQuaternion ret = new TrueSync.TSQuaternion(q.x, q.y, q.z, q.w);
        return ret;
    }
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) +
               t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
    public static Vector3 BezierCubic(float t, Vector3 from, Vector3 fromControl, Vector3 toControl, Vector3 to)
    {
        // tr= t reverse
        var tr = 1 - t;
        var tr2 = tr * tr;
        var t2 = t * t;

        return tr * tr2 * from + 3 * tr2 * t * fromControl + 3 * tr * t2 * toControl + t * t2 * to;
    }

    public static Vector3 BezierQuadratic(float t, Vector3 from, Vector3 control, Vector3 to)
    {
        // tr= t reverse
        var tr = 1 - t;
        var tr2 = tr * tr;
        var t2 = t * t;

        return tr2 * from + 2 * tr * t * control + t2 * to;
    }

    public static float hideY = -100;
    public static void SetActive(Transform t,bool active)
    {
        Vector3 p = t.transform.position;
        if (!active && p.y < hideY)
            return;
        if (active && p.y > hideY)
            return;
        if (active)
            p.y -= hideY*2f;
        else
            p.y += hideY*2f;
        t.transform.position=p;
    }


}