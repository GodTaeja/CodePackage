public static class C
{
    public const int Position = 0;

    public const int ViewPosition = 1;//逻辑层的pos和rotation改变之后不会改变表现层的，由变现层的组件主动去进行同步，这样防止多线程状况下脏读。

    public const int Transform = 2;

    public const int Movement = 3;

    public const int Count = Movement + 1; //占位符
}

