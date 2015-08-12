using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

/*
UserSingleton 클래스는 현재 유저의 개인 정보 및 능력치 정보를 메모리 상에 들고 있는 싱글톤 클래스입니다.
서버로부터 /User/{유저아이디} API로 정보를 가져와서 여기에 저장합니다.
*/
public class UserSingleton : MonoBehaviour {

	// UserID 입니다. 서버 상에서 유저를 식별하는 고유번호입니다.
	public int UserID{
		get {
			return PlayerPrefs.GetInt("UserID");
		}
		set {
			PlayerPrefs.SetInt("UserID",value);
		}
	}
	
	// AccessToken은 서버 API에 접근하기 위한 API의 역할을 합니다.
	public string AccessToken{
		get {
			return PlayerPrefs.GetString("AccessToken");
		}
		set {
			PlayerPrefs.SetString("AccessToken",value);
		}
	}
	
	// 페이스북 아이디입니다. 페이스북의 고유번호입니다. App Scoped User ID입니다.
	public string FacebookID{
		get {
			return PlayerPrefs.GetString("FacebookID");
		}
		set {
			PlayerPrefs.SetString("FacebookID",value);
		}
	}
	
	// 페이스북에 인증할 수 있는 유저의 개인 비밀번호 키입니다.
	public string FacebookAccessToken{
		get {
			return PlayerPrefs.GetString("FacebookAccessToken");
		}
		set {
			PlayerPrefs.SetString("FacebookAccessToken",value);
		}
	}
	
	// 유저의 이름입니다. 기본으로 페이스북의 이름을 가져와 적용합니다.
	public string Name{
		get {
			return PlayerPrefs.GetString("Name");
		}
		set {
			PlayerPrefs.SetString("Name",value);
		}
	}
	
	// 페이스북의 프로필사진 주소입니다.
	public string FacebookPhotoURL{
		get {
			return PlayerPrefs.GetString("FacebookPhotoURL");
		}
		set {
			PlayerPrefs.SetString("FacebookPhotoURL",value);
		}
	}
	
	// 유저의 레벨, 경험치, 데미지, 체력, 방어력, 스피드, 데미지 레벨, 체력 레벨, 방어력 레벨, 스피드 레벨입니다. 
	// 다음 레벨까지 남은 경험치, 그리고 다음 레벨로 레벨업하기 위해 필요한 경험치 정보도 가지고 있습니다.
	public int 
		Level, Experience, 
		Damage, Health, Defense, Speed,
		DamageLevel, HealthLevel, DefenseLevel, SpeedLevel,
		Diamond, ExpAfterLastLevel, ExpForNextLevel;
	
	//싱글톤 객체를 설정하는 부분입니다.
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

	//유저의 정보를 서버로부터 받아와서 최신 정보로 업데이트 하는 함수입니다. 
	//콜백변수로, 로드가 완료되면 다시 호출한 스크립트로 로드가 완료되었다고 호출할 수 있습니다.
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