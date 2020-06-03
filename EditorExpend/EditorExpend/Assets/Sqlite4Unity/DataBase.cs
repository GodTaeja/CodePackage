using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if !RUNINSERVER
using UnityEngine;
using SQLite4Unity3d;

#endif

/// <summary>
/// DataBase 的职责包括：连接数据库，断开数据库，删除数据库，数据查询。
/// </summary>
public class DataBase
{
#if !RUNINSERVER
    private SQLiteConnection _connection;
#else
    private System.Data.SQLite.SQLiteConnection _connection;
#endif
    //    private const string Key = "jedi@p21";
    private const string Key = null;

    public bool Connect()
    {
        Debug.Log("DataBase connect: " + Predefine.DatabaseName);

        //Debug.Log("sqlite3 version: " + SQLite3.LibVersionNumber());
        if (_connection != null)
        {
            Debug.Log("dataBase has connected, needn't connect again");
            return true;
        }
#if !RUNINSERVER
        var dbPath = "";
        Debug.Log("DataBase file path: " + dbPath);
        _connection = new SQLiteConnection(Key, dbPath, SQLiteOpenFlags.ReadOnly);
#else
#if DEBUG
        var dbPath = "D:/Work/Unity/Project/Client/p25/Assets/StreamingAssets/cfg";
#else
        var dbPath = "./cfg";
#endif
        Debug.Log("DataBase file path: " + dbPath);
        _connection = new System.Data.SQLite.SQLiteConnection();
        System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
        connstr.DataSource = dbPath;
        //connstr.
        _connection.ConnectionString = connstr.ToString();
        _connection.Open();
#endif
        return true;
    }

    public void Close()
    {
        Debug.Log("db close connection: " + Predefine.DatabaseName);
        _connection.Close();
        _connection = null;
    }

    /// <summary>
    /// 将表格转换成字段
    /// </summary>
    /// <typeparam name="TKey">表格主键作为字典的 key </typeparam>
    /// <typeparam name="TValue">表格每一条数据作为字典的 value </typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public Dictionary<TKey, TValue> TableAsDictionary<TKey, TValue>(Func<TValue, TKey> func) where TValue : new()
    {
#if RUNINSERVER
        var e = TableAsList<TValue>();
        int n = e.Count;
        Dictionary<TKey, TValue> dir = new Dictionary<TKey, TValue>(n);
        for (var i = 0; i < n; i++)
        {
            var value = e[i];
            dir.Add(func(value), value);
        }

        return dir;
#else
        var map = GetMapping(typeof(TValue));
        var connection = _connection;
        var str = typeof(TValue).Name;
        connection.CommandText = "select * from " + str;
        connection.Prepare();
        var n = connection.GetColumnCont();
        var dir = new Dictionary<TKey, TValue>(n);
        try
        {
            var cols = new TableMapping.Column[n];
            for (var i = 0; i < n; i++)
            {
                var name = connection.ColumnName16(i);
                var col = map.FindColumn(name);
                cols[i] = col;
                if (col == null)
                {
                    Debug.LogError("table:" + str + " ,col:" + name + " is Null");
                    throw new Exception();
                }
            }

            var t = map.MappedType;
            while (connection.Step() == SQLite3.Result.Row)
            {
                var obj = Activator.CreateInstance(t);
                for (var i = 0; i < n; i++)
                {
                    var col = cols[i];
                    var val = connection.ReadCol(i, col.ColumnType);
                    col.FieldInfo.SetValue(obj, val);
                }

                var tv = (TValue) obj;
                var tk = func(tv);
                dir.Add(tk, tv);
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0}\n : {1}", str, e);
        }
        finally
        {
            connection.FinalizeStmt();
        }

        return dir;
#endif
    }

    private static Dictionary<string, TableMapping> _mappings = null;

    public List<T> TableAsList<T>() where T : new()
    {
        var map = GetMapping(typeof(T));
        var str = typeof(T).Name;
        var query = "select * from " + str;
#if !RUNINSERVER
        var connection = _connection;
        connection.CommandText = query;
        connection.Prepare();
        var n = connection.GetColumnCont();
        var list = new List<T>(n);
        try
        {
            var cols = new TableMapping.Column[n];
            for (var i = 0; i < n; i++)
            {
                var name = connection.ColumnName16(i);
                var col = map.FindColumn(name);
                cols[i] = col;
                if (col == null)
                {
                    Debug.LogError("table:" + str + " ,col:" + name + " is Null");
                    throw new Exception();
                }
            }

            var t = map.MappedType;
            while (connection.Step() == SQLite3.Result.Row)
            {
                var obj = Activator.CreateInstance(t);
                for (var i = 0; i < n; i++)
                {
                    var col = cols[i];
                    var val = connection.ReadCol(i, col.ColumnType);
                    col.FieldInfo.SetValue(obj, val);
                }

                list.Add((T) obj);
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0}\n : {1}", str, e);
        }
        finally
        {
            connection.FinalizeStmt();
        }

        return list;
#else
        System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
        cmd.Connection = _connection;
        cmd.CommandText = query;

        //cmd.Prepare();
        Debug.Log("query:" + query);
        System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

        int n = reader.FieldCount;
        Debug.Log("FieldCount:" + n + " reader.StepCount:" + reader.StepCount);

        //cmd.

        List<T> list = new List<T>(n);

        try
        {
            var cols = new TableMapping.Column[n];
            for (int i = 0; i < cols.Length; i++)
            {
                var name = reader.GetName(i);
                cols[i] = map.FindColumn(name);
                //if (cols[i] == null)
                Debug.LogError("table:" + typeof(T).Name + " ,col:" + name + " " + cols[i].Name);
            }

            //for (int j=0;j<reader.StepCount;j++)
            while (reader.Read())
            {
                //Debug.Log("int read:"+ cols.Length);
                var obj = Activator.CreateInstance(map.MappedType);
                for (int i = 0; i < cols.Length; i++)
                {
                    //if (cols[i] == null)
                    //    continue;
                    var colType = reader.GetFieldType(i);
                    object val = reader.GetValue(i); ;//  _connection.ReadCol(i, colType, cols[i].ColumnType);
                                                      //Debug.Log("val:" + i + " " + val);
                    if (colType == typeof(double))
                    {
                        //val = 2.5f;//reader.GetFloat(i);
                        float vv = (float)((double)val);
                        cols[i].FieldInfo.SetValue(obj, vv);
                    }
                    else
                        cols[i].FieldInfo.SetValue(obj, val);

                }
                list.Add((T)obj);
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0}\n : {1}", typeof(T).Name, e);
        }
        finally
        {
            //_connection.Close();
        }
        return list;
#endif
    }

    /// <summary>
    /// Retrieves the mapping that is automatically generated for the given type.
    /// </summary>
    /// <param name="type">
    /// The type whose mapping to the database is returned.
    /// </param>         
    /// <param name="createFlags">
    /// Optional flags allowing implicit PK and indexes based on naming conventions
    /// </param>     
    /// <returns>
    /// The mapping represents the schema of the columns of the database and contains 
    /// methods to set and get properties of objects.
    /// </returns>
    public static TableMapping GetMapping(Type type)
    {
        if (_mappings == null)
        {
            _mappings = new Dictionary<string, TableMapping>();
        }

        TableMapping map;
        if (!_mappings.TryGetValue(type.FullName, out map))
        {
            map = new TableMapping(type);
            _mappings[type.FullName] = map;
        }

        return map;
    }

    public class TableMapping
    {
        public Type MappedType;

        public Column[] Columns;

        public TableMapping(Type type)
        {
            MappedType = type;
            var props = MappedType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            var l = props.Length;
            var cols = new Column[l];
            for (var i = 0; i < l; i++)
            {
                //if (p.CanWrite)
                {
                    cols[i] = new Column(props[i]);
                }
            }

            Columns = cols;
        }

        public Column FindColumn(string columnName)
        {
            int n = Columns.Length;
            for (int i = 0; i < n; ++i)
            {
                var column = Columns[i];
                if (column.Name == columnName)
                {
                    return column;
                }
            }

            return null;
        }

        public class Column
        {
            public FieldInfo FieldInfo;

            public string Name;

            public Type ColumnType;

            public Column(FieldInfo fieldInfo)
            {
                FieldInfo = fieldInfo;
                Name = fieldInfo.Name;
                //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the actual type instead
                ColumnType = Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType;
            }
        }
    }
}