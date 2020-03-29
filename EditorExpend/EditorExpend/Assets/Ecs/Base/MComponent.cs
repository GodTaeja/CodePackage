public class MComponent
{
    public MEntity Entity;

    public MComponent Extension;

    public T Get<T>(int componentType, bool mustHave = true) where T : MComponent
    {
        return Entity.Get<T>(componentType, mustHave);
    }

    /*
    public MComponent Get(C componentType, bool mustHave = true)
    {
        return Entity.Get(componentType, mustHave);
    }
    */

    public virtual void OnDestroy()
    {
    }
}
