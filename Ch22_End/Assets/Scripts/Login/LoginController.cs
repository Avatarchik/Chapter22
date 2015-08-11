using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using System;
using Facebook;
using System.Collections.Generic;

public class LoginController : MonoBehaviour {

	public GameObject BtnFacebook;

	/*
	 * Login Process
	 * 
	 * 1) LoginInit()
	 * 2) LoginFacebook()
	 * 3) LoadDataFromFacebook() - Coroutine
	 * 4) LoginGameServer()
	 * 5) LoadDataFromGameServer() - Coroutine
	 * 6) LoadNextScene()
	 *
	 * */
	
	bool[] finished = new bool[5];

	void Start () {

		LoginInit();
		
		for(int i = 0; i < finished.Length;i++){
			finished[i] = false;
		}

	}

// LoginInit : 이미 로그인한 세션이 있으면 로그인 하거나 페이스북 로그인 버튼을 보여줍니다.
	void LoginInit()
	{
// 이미 유저아이디가 있거나 액세스 토큰이 있으면 자동으로 로그인합니다.
		if(UserSingleton.Instance.UserID != 0 
		&& UserSingleton.Instance.AccessToken != "") 
		{

			LoginFacebook();
		}else{ 
// 저장된 유저아이디가 없으면 새로 로그인합니다.
			BtnFacebook.SetActive(true);
		}
	}

// 화면 상의 페이스북 버튼을 누르면 호출되는 함
	public void LoginFacebook()
	{
// 페이스북 SDK를 초기화합니다. (페이스북 API 서버 접속)
		FB.Init(delegate {

			FB.ActivateApp();
// 페이스북 SDK로 로그인을 수행합니다.
// 유니티 에디터에서는 Access Token을 받아오는 팝업이 뜨지만
// 모바일에서는 잘 연동됩니다.
			FB.Login("email",delegate(FBResult result) { 

				Debug.Log("FB Login Result: " + result.Text);
/*
저자의 경우 오는 페이스북 로그인 결과

 {"is_logged_in":true,
 "user_id":"10204997009661738",
 "access_token":"~~~",
 "access_token_expires_at":"01/01/0001 00:00:00"}
*/

// 페이스북 로그인 결과를 JSON 파싱합니다.
				JSONObject obj = JSONObject.Parse(result.Text);
// 페이스북 로그인이 성공했는지 여부를 뜻하는 is_logged_in 변수 bool형
				bool is_logged_in = obj["is_logged_in"].Boolean;

// 페이스북 아이디를 UserSingleton에 저장합니다. 
// 이 변수는 게임이 껏다 켜져도 유지되도록 환경변수에 저장하도록 구현되있습니다.
				UserSingleton.Instance.FacebookID = obj["user_id"].Str;
// 페이스북 액세스 토큰을 UserSingleton에 저장힙니다.
// 이 변수 역시 게임이 껏다 켜져도 유지됩니다.
				UserSingleton.Instance.FacebookAccessToken = obj["access_token"].Str;

// 페이스북 로그인에 성공하면
				if(is_logged_in){

					StartCoroutine(LoadDataFromFacebook());


				}else{	
// 페이스북 로그인에 실패한 경우



				}



			});
		},delegate(bool isUnityShown) {

		},"");

	}

	public IEnumerator LoadDataFromFacebook()
	{
		FB.API("/me", HttpMethod.GET, delegate(FBResult meResult){
			Debug.Log(meResult.Text);

/*
저자의 경우 오는 페이스북 개인정보

{"id":"10204997009661738",
"first_name":"Chris",
"gender":"male",
"last_name":"Song",
"link":"https:\/\/www.facebook.com\/app_scoped_user_id\/10204997009661738\/",
"locale":"ko_KR",
"name":"Chris Song",
"timezone":9,
"updated_time":"2015-07-26T19:32:26+0000",
"verified":true}
			*/

			JSONObject meObj = JSONObject.Parse(meResult.Text);
			UserSingleton.Instance.Name = meObj["name"].Str;
			finished[0] = true;

		});

// 페이스북 프로필 사진 가져오기 

		FB.API("/me/picture?width=128&height=128&redirect=false", HttpMethod.GET, delegate(FBResult pictureResult){
			Debug.Log(pictureResult.Text);
			JSONObject pictureObj = JSONObject.Parse(pictureResult.Text);
			UserSingleton.Instance.FacebookPhotoURL = pictureObj["data"].Obj["url"].Str;

			finished[1] = true;

		});

// 페이스북 프로필 사진 가져오기 
		FB.API("/friends", HttpMethod.GET, delegate(FBResult friendResult){
			Debug.Log(friendResult.Text);

			finished[2] = true;
		});

		while(!finished[0] || !finished[1] || !finished[2]){
			yield return new WaitForSeconds(0.1f);
		}

		LoginGameServer();
	}

	public void LoginGameServer()
	{

// 페이스북 로그인 정보를 우리 게임 서버로 보내보겠습니다.
		JSONObject body = new JSONObject();
		body.Add("FacebookID", UserSingleton.Instance.FacebookID);
		body.Add("FacebookAccessToken", UserSingleton.Instance.FacebookAccessToken);
		body.Add("FacebookName", UserSingleton.Instance.Name);
		body.Add("FacebookPhotoURL", UserSingleton.Instance.FacebookPhotoURL);

		Debug.Log("Send To Server: " + body.ToString());
// 서버에 로그인 데이터를 전달합니다.
		HTTPClient.Instance.POST(Singleton.Instance.HOST + "/Login",
		                         body.ToString(),
		                         delegate(WWW www) 
		{
			Debug.Log(www.text);
			JSONObject response = JSONObject.Parse(www.text);
			
			int ResultCode = (int)response["ResultCode"].Number;
			if(ResultCode == 1 || ResultCode == 2)
			{
				JSONObject Data = response.GetObject("Data");
				UserSingleton.Instance.UserID = (int)Data["UserID"].Number;
				UserSingleton.Instance.AccessToken = Data["AccessToken"].Str;
				
				StartCoroutine(LoadDataFromGameServer());

			}else{ 
// 로그인 실패
				
			}

		});

	}

	public IEnumerator LoadDataFromGameServer()
	{

		UserSingleton.Instance.Refresh(delegate() {
			finished[3] = true;
		});

		while(!finished[3]){
			yield return new WaitForSeconds(0.1f);
		}
		LoadNextScene();
	}

	public void LoadNextScene()
	{
		Application.LoadLevel("Lobby");
	}
}
