using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;

namespace TrafficSystem.IOStreamer
{
	[ExecuteInEditMode]
	public class BuilderNodeDAO : MonoBehaviour
	{
		public static BuilderNodeDAO DAO = null;

		public class BuilderNodeRecord
		{
			public int id;
			public string position;
			public string rotation;
			public int type;

			public BuilderNodeRecord (int _id, string _position, string _rotation, int _type)
			{
				id = _id;
				position = _position;
				rotation = _rotation;
				type = _type;
			}
		}

		public List<BuilderNodeRecord> builderNodeRecords = new List<BuilderNodeRecord> ();

		void OnEnable ()
		{
			DAO = this;
		}

		public void Save (string path)
		{
			SqliteCore db = new SqliteCore ("data source=" + path);

			db.ExecuteQuery ("DELETE FROM BUILDER_NODE");
		
			foreach (BuilderNodeRecord record in builderNodeRecords) {
				db.ExecuteQuery (string.Format ("INSERT INTO BUILDER_NODE VALUES ({0},{1},{2},{3})",
					record.id, "'" + record.position + "'", "'" + record.rotation + "'", record.type));
				Debug.Log (record.id + record.position + record.rotation);
			}
	
			db.CloseSqlConnection ();

		}

		public void Load (string path)
		{
			SqliteCore db = new SqliteCore ("data source=" + path);

			builderNodeRecords.Clear ();

			SqliteDataReader sqReader = db.ExecuteQuery ("SELECT * FROM BUILDER_NODE");
			while (sqReader.Read ()) {
				int _id = sqReader.GetInt32 (sqReader.GetOrdinal ("id"));
				string _position = sqReader.GetString (sqReader.GetOrdinal ("position"));
				string _rotation = sqReader.GetString (sqReader.GetOrdinal ("rotation"));
				int _type = sqReader.GetInt32 (sqReader.GetOrdinal ("type"));
				BuilderNodeRecord newRecord = new BuilderNodeRecord (_id, _position, _rotation, _type);
				builderNodeRecords.Add (newRecord);
			} 

			db.CloseSqlConnection ();
		}
	}
}

