using ConsoleAppJonas;
using System.Data;

DataAccessLayer dataAccess = new DataAccessLayer();

DataSet ds = dataAccess.ExecuteQueryDataSet("select * from GeoUserDescription", new DataSet(), "clientes");

Console.WriteLine("Hello, World!" + ds.Tables["clientes"].Rows[0]["Name"].ToString());

