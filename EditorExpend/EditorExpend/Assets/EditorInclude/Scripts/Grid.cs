

[System.Serializable]
public class Grid {
    public int PosX;
    public int PosY;

    public string Name;

    public Property Property;
}

public enum Property
{
    Fire,
    Water,
    Light,
    Dark
}