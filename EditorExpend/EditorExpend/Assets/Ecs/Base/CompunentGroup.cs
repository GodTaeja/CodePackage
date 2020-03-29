using System.Collections.Generic;

public class ComponentGroup
{
    private readonly List<MComponent>[] _components;

    public ComponentGroup()
    {
        _components = new List<MComponent>[64];
        for (int i = 0; i < _components.Length; i++)
        {
            _components[i] = new List<MComponent>(1024);
        }
    }

    public List<MComponent> GetComponents(int componentType)
    {
        return _components[componentType];
    }

    public void Clear()
    {
        int n = _components.Length;
        for (int i = 0; i < n; i++)
        {
            _components[i].Clear();
        }
    }

    public void AddComponent(MComponent component, int componentType)
    {
        _components[componentType].Add(component);
    }
}

