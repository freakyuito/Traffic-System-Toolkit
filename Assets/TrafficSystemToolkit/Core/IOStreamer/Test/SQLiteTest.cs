using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

namespace TrafficSystem.IOStreamer
{
	public class SQLiteTest : MonoBehaviour
	{
		void Start ()
		{  
			SqliteCore db = new SqliteCore ("data source=" + Application.streamingAssetsPath + "/" + "trafficsystem.db");

			//		db.ExecuteQuery ("DELETE FROM NODE_INFO");

			//		db.ExecuteQuery (string.Format ("INSERT INTO NODE_INFO VALUES ({0},{1},{2})",
			//			2, "'(xfdsfds)'", 24));

			//		SqliteDataReader sqReader = db.ExecuteQuery ("SELECT * FROM NODE_INFO");
			//		while (sqReader.Read ()) {
			//			Debug.Log (sqReader.GetInt32 (sqReader.GetOrdinal ("node_id")).ToString () + sqReader.GetString (sqReader.GetOrdinal ("node_pos"))
			//			+ sqReader.GetInt32 (sqReader.GetOrdinal ("parent_lane_id")));
			//		} 

			////		db.CreateTable ("momo", new string[]{ "name", "qq", "email", "blog" }, new string[]{ "text", "text", "text", "text" });
			////		db.InsertInfo ("momo", new string[]{ "'宣雨松'", "'289187120'", "'xuanyusong@gmail.com'", "'www.xuanyusong.com'"   });
			//		db.UpdateInfo ("momo", new string[]{ "name", "qq" }, new string[]{ "'xuanyusong'", "'11111111'" }, "email", "'xuanyusong@gmail.com'");
			//		db.Delete ("momo", new string[]{ "email", "email" }, new string[]{ "'xuanyusong@gmail.com'", "'000@gmail.com'" });
			//
			////		db.InsertInfo("momo", new string[]{ "'宣雨松'","'289187120'","'xuanyusong@gmail.com'","'www.xuanyusong.com'"   });
			//		db.InsertInfo ("momo", new string[]{ "'雨松MOMO'", "'289187120'", "'000@gmail.com'", "'www.xuanyusong.com'"   });
			//		db.InsertInfo ("momo", new string[]{ "'哇咔咔'", "'289187120'", "'111@gmail.com'", "'www.xuanyusong.com'"   });
			//
			//		db.Delete ("momo", new string[]{ "email", "email" }, new string[]{ "'xuanyusong@gmail.com'", "'000@gmail.com'" });
			//
			//		SqliteDataReader sqReader = db.SelectWhere ("momo", new string[]{ "name", "email" }, new string[]{ "qq" }, new string[]{ "=" }, new string[]{ "289187120" });
			//
			//		while (sqReader.Read ()) {
			//			Debug.Log (sqReader.GetString (sqReader.GetOrdinal ("name")) + sqReader.GetString (sqReader.GetOrdinal ("email")));
			//		} 

			db.CloseSqlConnection ();
		}
	}
}
