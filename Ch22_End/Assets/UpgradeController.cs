using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class UpgradeController : MonoBehaviour {

	public Text txtUpgradeStatus, txtHealth, txtDefense, txtDamage, txtSpeed;

	// Use this for initialization
	void Start () {
		NotificationCenter.Instance.Add(NotificationCenter.Subject.PlayerData,UpdatePlayerData);
		UpdatePlayerData();
	}

	void UpdatePlayerData()
	{
		txtUpgradeStatus.text = string.Format("{0}\n{1}\n{2}\n{3}",
		                                      UserSingleton.Instance.Health,
		                                      UserSingleton.Instance.Defense,
		                                      UserSingleton.Instance.Damage,
		                                      UserSingleton.Instance.Speed);

		txtHealth.text = string.Format("+{0}", UserSingleton.Instance.HealthLevel);
		txtDefense.text = string.Format("+{0}", UserSingleton.Instance.DefenseLevel);
		txtDamage.text = string.Format("+{0}", UserSingleton.Instance.DamageLevel);
		txtSpeed.text = string.Format("+{0}", UserSingleton.Instance.SpeedLevel);

	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
