using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class UserSingleton : MonoBehaviour {

	public int UserID{
		get {
			return PlayerPrefs.GetInt("UserID");
		}
		set {
			PlayerPrefs.SetInt("UserID",value);
			PlayerPrefs.Save();
		}
	}
	public string AccessToken{
		get {
			return PlayerPrefs.GetString("AccessToken");
		}
		set {
			PlayerPrefs.SetString("AccessToken",value);
			PlayerPrefs.Save();
		}
	}
	public string FacebookID{
		get {
			return PlayerPrefs.GetString("FacebookID");
		}
		set {
			PlayerPrefs.SetString("FacebookID",value);
			PlayerPrefs.Save();
		}
	}
	public string FacebookAccessToken{
		get {
			return PlayerPrefs.GetString("FacebookAccessToken");
		}
		set {
			PlayerPrefs.SetString("FacebookAccessToken",value);
			PlayerPrefs.Save();
		}
	}
	public string Name{
		get {
			return PlayerPrefs.GetString("Name");
		}
		set {
			PlayerPrefs.SetString("Name",value);
			PlayerPrefs.Save();
		}
	}
	public string FacebookPhotoURL{
		get {
			return PlayerPrefs.GetString("FacebookPhotoURL");
		}
		set {
			PlayerPrefs.SetString("FacebookPhotoURL",value);
			PlayerPrefs.Save();
		}
	}
	
	public int 
		Level, Experience, 
		Damage, Health, Defense, Speed,
		DamageLevel, HealthLevel, DefenseLevel, SpeedLevel,
		Diamond, ExpAfterLastLevel, ExpForNextLevel;
	
	//Singleton Member And Method
	static UserSingleton _instance;
	public static UserSingleton Instance {
		get {
			if( ! _instance ) {
				GameObject container = new GameObject("UserSingleton");
				_instance = container.AddComponent( typeof( UserSingleton ) ) as UserSingleton;

				DontDestroyOnLoad( container );
			}
			
			return _instance;
		}
	}

	public void Refresh(Action callback)
	{
		HTTPClient.Instance.GET(Singleton.Instance.HOST + "/User/"+UserSingleton.Instance.UserID,
		                        delegate(WWW www)
		                        {
			Debug.Log(www.text);
			JSONObject response = JSONObject.Parse(www.text);
			int ResultCode = (int)response["ResultCode"].Number;
			JSONObject data = response["Data"].Obj;
			UserSingleton.Instance.Level = (int)data["Level"].Number;
			UserSingleton.Instance.Experience = (int)data["Experience"].Number;
			UserSingleton.Instance.Damage = (int)data["Damage"].Number;
			UserSingleton.Instance.Health = (int)data["Health"].Number;
			UserSingleton.Instance.Defense = (int)data["Defense"].Number;
			UserSingleton.Instance.Speed = (int)data["Speed"].Number;
			UserSingleton.Instance.DamageLevel = (int)data["DamageLevel"].Number;
			UserSingleton.Instance.HealthLevel = (int)data["HealthLevel"].Number;
			UserSingleton.Instance.DefenseLevel = (int)data["DefenseLevel"].Number;
			UserSingleton.Instance.SpeedLevel = (int)data["SpeedLevel"].Number;
			UserSingleton.Instance.Diamond = (int)data["Diamond"].Number;
			UserSingleton.Instance.ExpForNextLevel = (int)data["ExpForNextLevel"].Number;
			UserSingleton.Instance.ExpAfterLastLevel = (int)data["ExpAfterLastLevel"].Number;
			
			callback();
		});
	}

}

