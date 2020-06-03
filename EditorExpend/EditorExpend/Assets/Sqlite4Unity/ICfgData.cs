public interface ICfgData
{
    void Init(DataBase dataBase);

    void PrepareData();//给cfg进行预处理使用，init读取数据完毕之后，再进行prepare，防止引用其他cfg的时候还没有初始化完成
}
