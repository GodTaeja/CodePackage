using System;

public class MEntity
{
    long _componentsMask;

    public int Id { get; set; }

    public string Name { get; set; } = "";

    public bool IsLogicALive { get; set; } = true;

    private readonly MComponent[] _components;

    public Action<MEntity, int, MComponent> OnComponentAdded;

    public MEntityManager EntityManager;

    public MEntity(int entityId, MEntityManager EntityManager)
    {
        this.EntityManager = EntityManager;
        Id = entityId;
        _components = new MComponent[64];
    }

    public string GetEntityNameOrId()
    {
        if (string.IsNullOrEmpty(Name))
            return Id.ToString();
        return Name;
    }

    public MComponent Get(int componentType, bool mustHave = true)
    {
        int index = componentType;
        return _components[index];
    }

    public T Get<T>(int componentType, bool mustHave = true) where T : MComponent
    {
        int index = componentType;
        var ret = _components[index];
        if (ret == null && mustHave)
        {
            throw new Exception($"Can't find the Component {componentType} from entity: {GetEntityNameOrId()}");
        }

        var ret2 = ret as T;
        if (ret != null && ret2 == null)
        {
            throw new Exception($"Incorrect compnent type: require - {typeof(T).Name}, but real - {componentType}, from entity: {GetEntityNameOrId()}");
        }

        return ret2;
    }

    public static void CheckComponentParams<T>(int componentType) where T : MComponent
    {
        return;
        if (!typeof(T).Name.Contains(componentType.ToString()))
        {
            throw new Exception($"Mismatching componentType, <T>={typeof(T)}, enum C={componentType}");
        }
    }

    public T Add<T>(int componentType) where T : MComponent, new()
    {
        var com = new T();
        Add(componentType, com);
        return com;
    }

    public MComponent Add(int componentType, MComponent component)
    {
        component.Entity = this;

        int index = componentType;
        _components[index] = component;
        OnComponentAdded(this, componentType, component);
        _componentsMask |= 1L << index;

        return component;
    }


    public bool HasComponent(int componentType)
    {
        return (_componentsMask & (1L << componentType)) != 0;
        //        return _components[(int) componentType] != null;
    }

    public void RemoveAllComponents()
    {
        Name = null;
        _componentsMask = 0;

        int n = _components.Length;
        for (int i = 0; i < n; i++)
        {
            var com = _components[i];
            if (com != null)
            {
                com.Entity = null; //标记删除,后续_componentGroup将延迟删除
                com.OnDestroy();
                _components[i] = null;
            }
        }
    }

    public void DestroyAllComponets()
    {

        int n = _components.Length;
        for (int i = 0; i < n; i++)
        {
            var com = _components[i];
            if (com != null)
            {
                com.OnDestroy();
            }
        }
    }
}




