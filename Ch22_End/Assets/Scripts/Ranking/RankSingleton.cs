using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using System.Collections.Generic;
using System;

public class RankSingleton : MonoBehaviour {

	public Dictionary<int, JSONObject> TotalRank, FriendRank;

	//싱글톤 객체를 설정하는 부분입니다.
	static RankSingleton _instance;
	public static RankSingleton Instance {
		get {
			if( ! _instance ) {
				GameObject container = new GameObject("RankSingleton");
				_instance = container.AddComponent( typeof( RankSingleton ) ) as RankSingleton;

				DontDestroyOnLoad( container );
			}

			return _instance;
		}
	}


	public void LoadTotalRank(Action callback){

		HTTPClient.Instance.GET (Singleton.Instance.HOST + "/Rank/Total?Start=1&Count=50", delegate(WWW www) {
		
			string response = www.text;

			JSONObject obj = JSONObject.Parse(response);

			JSONArray arr = obj["Data"].Array;

			foreach(JSONValue item in arr){
				int rank = (int)item.Obj["Rank"].Number;
				TotalRank.Add(rank,item.Obj);
			}

			callback();
		});


	}

	public void LoadFriendRank(Action callback){



		HTTPClient.Instance.GET (Singleton.Instance.HOST + "/Rank/Friend", delegate(WWW www) {

			string response = www.text;

			JSONObject obj = JSONObject.Parse(response);

			JSONArray arr = obj["Data"].Array;

			foreach(JSONValue item in arr){
				int rank = (int)item.Obj["Rank"].Number;
				TotalRank.Add(rank,item.Obj);
			}

			callback();
		});


	}
}
