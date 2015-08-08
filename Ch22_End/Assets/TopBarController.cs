using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopBarController : MonoBehaviour {

	public Text txtName, txtLevel, txtExp, txtDiamaon;
	public Slider sliderExp;

	// Use this for initialization
	void Start () {
		NotificationCenter.Instance.Add(NotificationCenter.Subject.PlayerData,UpdatePlayerData);
		UpdatePlayerData();
	}

	void UpdatePlayerData()
	{
		txtName.text = UserSingleton.Instance.Name;
		txtLevel.text = "Lv " + UserSingleton.Instance.Level.ToString();
		txtExp.text = UserSingleton.Instance.ExpAfterLastLevel.ToString() + " / " + UserSingleton.Instance.ExpForNextLevel.ToString();
		sliderExp.maxValue = UserSingleton.Instance.ExpForNextLevel;
		sliderExp.value = UserSingleton.Instance.ExpAfterLastLevel;
		txtDiamaon.text = UserSingleton.Instance.Diamond.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
