#if !RUNINSERVER
using UnityEngine;
#endif
using System.Collections;
using System.Collections.Generic;
using Enyim.Collections;
using System.Linq;
using TrueSync;

public class LayeredRtree<T>
{
    private RTree<T> [] subTrees;
    private int maxLayer;
    private int maxEntries;
    public LayeredRtree(int maxLayer=64,int maxEntries=9)
    {
        subTrees = new RTree<T>[maxLayer];
        this.maxLayer = maxLayer;
        this.maxEntries = maxEntries;
    }

    public void Clear()
    {
        for (int i = 0; i < maxLayer; i++)
        {
            if(subTrees[i]!=null)
                subTrees[i].Clear();
        }
    }

    public void Insert(FP x,FP y,T data,int layer)
    {
        if(layer>=maxLayer)
            throw new System.Exception("layer>maxLayer:"+layer);
        //Debug.LogWarning("insert layer:"+ layer);
        RTree<T> tree = subTrees[layer];
        if (tree == null)
        {
            tree=new RTree<T>(maxEntries);
            subTrees[layer] = tree;
        }
        tree.Insert(new RTreeNode<T>(data, new Envelope(x, y, x, y)));

    }

    public List<T> Search(Envelope envelope,long layerMask)
    {
        List<T> rets = new List<T>();
        for (int i = 0; (layerMask>>i)>0; i++)
        {
            if ((layerMask >> i) % 2 == 1)
            {
                RTree<T> tree = subTrees[i];
                if (tree != null)
                {
                    rets.AddRange(tree.Search(envelope).Select((d)=>d.Data));
                }
            }
        }

        return rets;
    }

    public RTreeNode<T> Nearest(FP x, FP y,long layerMask)
    {
        FP distance = FP.MaxValue;
        RTreeNode<T> nearestNode = null;
        for (int i = 0; (layerMask >> i) > 0; i++)
        {
            if ((layerMask >> i) % 2 == 1)
            {
                RTree<T> tree = subTrees[i];
                //Debug.LogWarning("Nearest:" + i + " " + layerMask + " " + (layerMask >> i)+" "+ tree);
                if (tree != null
                    //&& tree.Envelope.Distance(x,y)<distance
                    )
                {
                    RTreeNode<T> n = tree.Nearest(x, y, distance);
                    //Debug.LogWarning("find in tree:" + tree.Envelope);
                    //Debug.LogWarning("find in tree2:" + n.Envelope);
                    //Debug.LogWarning("find in tree:" + n.Data);
                    if (n != null)
                    {
                        FP _d = n.Envelope.Distance(x, y);
                        if (_d < distance)
                        {
                            distance = _d;
                            nearestNode = n;
                        }
                    }
                }

            }
        }
        return nearestNode;
        if (nearestNode != null)
            return nearestNode;
        throw new System.Exception("cannot find nearest");
        //return ;

    }
#if !RUNINSERVER
    public void Draw()
    {
        foreach (RTree < T > tree in subTrees)
        {
            if(tree!=null)
                tree.DebugDraw(Color.red);
        }
    }
#endif
}
