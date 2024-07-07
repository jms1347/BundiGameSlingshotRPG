using UnityEngine;
using System.Collections;

[HelpURL("http://mobfarmgames.weebly.com/ap_reference.html")]
public class AP_Reference : MonoBehaviour {

	[Tooltip("Despawn()이 호출되면 이 개체는 비활성화되지만 개체 풀에서 사용할 수 있게 되기 전에 지연 시간을 기다립니다.")]
	public float delay;

	[HideInInspector] public AP_Pool poolScript; // stores the location of the object pool script for this object
	[HideInInspector] public float timeSpawned;

	public bool Despawn ( float del ) { // -1 will use delay specified in this script
		if ( del >= 0 ) { // override delay
			if ( poolScript ) {
				Invoke( "DoDespawn", del );
				gameObject.SetActive(false);
				return true;
			} else {
				return false;
			}
		} else {
			return DoDespawn();
		}
	}

	bool DoDespawn() {
		if ( poolScript ) {
			poolScript.Despawn( gameObject, this );
			return true;
		} else {
			return false;
		}
	}

}
