using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

namespace TrafficSystem.IOStreamer
{
	[ExecuteInEditMode]
	public class LinkInfoDAO : MonoBehaviour
	{
		public static LinkInfoDAO DAO = null;

		public class LinkInfoRecord
		{
			public int fromId;
			public int toId;
			public int distance;

			public LinkInfoRecord (int _from, int _to, int _dist)
			{
				fromId = _from;
				toId = _to;
				distance = _dist;
			}
		}

		public List<LinkInfoRecord> linkInfoRecords = new List<LinkInfoRecord> ();

		void OnEnable ()
		{
			DAO = this;
		}

		public void Save (string path)
		{
			SqliteCore db = new SqliteCore ("data source=" + path);

			db.ExecuteQuery ("DELETE FROM LINK_INFO");
		
			foreach (LinkInfoRecord record in linkInfoRecords) {
				db.ExecuteQuery (string.Format ("INSERT INTO LINK_INFO VALUES ({0},{1},{2})",
					record.fromId, record.toId, record.distance));

			}

			db.CloseSqlConnection ();
		}

		public void Load (string path)
		{
			SqliteCore db = new SqliteCore ("data source=" + path);

			linkInfoRecords.Clear ();

			SqliteDataReader sqReader = db.ExecuteQuery ("SELECT * FROM LINK_INFO");
			while (sqReader.Read ()) {
				int _from = sqReader.GetInt32 (sqReader.GetOrdinal ("from_id"));
				int _to = sqReader.GetInt32 (sqReader.GetOrdinal ("to_id"));
				int _distance = sqReader.GetInt32 (sqReader.GetOrdinal ("distance"));
				LinkInfoRecord newRecord = new LinkInfoRecord (_from, _to, _distance);
				linkInfoRecords.Add (newRecord);
			} 

			db.CloseSqlConnection ();
		}
	}
}
