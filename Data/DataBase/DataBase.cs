using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using Data.DataBase.Tables;

namespace Data.DataBase;

public class DataBase<T>
{
    private readonly string _connectionString;
    private readonly string _tableName;


    public DataBase()
    {
        _connectionString = "Data Source=db.db;Version=3";
        _tableName = GetTableName();
    }

    public List<T> GetAll()
    {
        var objects = new List<T>();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new SQLiteCommand($"SELECT * FROM {_tableName}",connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = ToObj(reader);
                        objects.Add(obj);
                    }
                }
            }
        }
        return objects;
    }

    public void Delete(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new SQLiteCommand($"DELETE FROM {_tableName} WHERE 'Id' = {id}",connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Insert(T obj)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var props = typeof(T).GetProperties();
            var colNames = string.Join(",", props.Select(prop => $"\"{prop.Name}\""));
            var values = string.Join(",", props.Select(prop => $"@{prop.Name}"));
            using (var cmd = new SQLiteCommand
                       ($"INSERT INTO {_tableName} ({colNames}) VALUES ({values})", connection))
            {
                foreach (var prop in props)
                {
                    var parameter = new SQLiteParameter($"@{prop.Name}", prop.GetValue(obj) ?? DBNull.Value);
                    cmd.Parameters.Add(parameter);
                }

                cmd.ExecuteNonQuery();
            }
        }
    }
    
    private T ToObj(IDataRecord reader)
    {
        var obj = Activator.CreateInstance<T>();
        var props = typeof(T).GetProperties();
        foreach (var prop in props)
        {
            var index = reader.GetOrdinal(prop.Name);
            if (reader.IsDBNull(index)) continue;
            var val = reader.GetValue(index);
            if (prop.PropertyType == typeof(int) && val is long)
            {
                prop.SetValue(obj, Convert.ToInt32(val));
            }
            else
            {
                prop.SetValue(obj, val);
            }
        }
        return obj;
    }

    private string GetTableName()
    {
        var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true)
            .FirstOrDefault() as TableAttribute;

        return tableAttribute?.Name ?? typeof(T).Name + "s";
    }
    
}