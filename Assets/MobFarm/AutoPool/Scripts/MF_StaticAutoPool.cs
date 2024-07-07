using UnityEngine;
using System.Collections;

namespace AP_enum {
	public enum EmptyBehavior { Grow, Fail, ReuseOldest }
	public enum MaxEmptyBehavior { Fail, ReuseOldest }
}

[HelpURL("http://mobfarmgames.weebly.com/mf_staticautopool.html")]
public class MF_AutoPool {

	static AP_Manager opmScript;

	// 일찍 호출될 수 있으며 스폰을 생성하지 않지만 풀 참조를 생성하고 참조가 생성되었거나 이미 존재하는 경우 true를 반환합니다.
	// 특정 풀이 처음 생성되기 전에 풀 참조를 연결하려는 경우 사용합니다. (아마도 가장 까다로운 장면을 제외하고는 필요하지 않을 것입니다.)
	// 또한 런타임에 풀을 동적으로 생성하는 데 사용할 수 있습니다.
	public static bool InitializeSpawn ( GameObject prefab ) { 
		return InitializeSpawn ( prefab, 0f, 0 );
	}
	// 할당된 매개변수를 사용하여 런타임에 풀을 생성할 수 있습니다.
	// addPool이 < 1이면 기존 풀을 백분율로 늘리는 데 사용됩니다. 그렇지 않으면 가장 가까운 정수로 반올림하고 해당 금액만큼 증가합니다.
	// minPool은 해당 풀에 있어야 하는 최소 객체입니다. 현재 풀 + addPool < minPool이면 minPool이 사용됩니다.
	public static bool InitializeSpawn ( GameObject prefab, float addPool, int minPool ) { 
		return InitializeSpawn( prefab, addPool, minPool, AP_enum.EmptyBehavior.Grow, AP_enum.MaxEmptyBehavior.Fail, false ); 
	}
	public static bool InitializeSpawn ( GameObject prefab, float addPool, int minPool, AP_enum.EmptyBehavior emptyBehavior, AP_enum.MaxEmptyBehavior maxEmptyBehavior ) { 
		return InitializeSpawn( prefab, addPool, minPool, emptyBehavior, maxEmptyBehavior, true ); 
	}
	static bool InitializeSpawn ( GameObject prefab, float addPool, int minPool, AP_enum.EmptyBehavior emptyBehavior, AP_enum.MaxEmptyBehavior maxEmptyBehavior, bool modBehavior ) { 
		if ( prefab == null ) { return false; } // 개체가 정의되지 않았습니다

		if ( opmScript == null ) { // 개체 풀 관리자 스크립트를 아직 찾지 못했습니다.
			opmScript = Object.FindObjectOfType<AP_Manager>(); // 씬에서 찾기
			if ( opmScript == null ) { 
				Debug.Log( "No Object Pool Manager found in scene." ); 
				return false; 
			} // didn't find an object pool manager
		}
		// object pool 관리자를 찾았습니다.
		return opmScript.InitializeSpawn( prefab, addPool, minPool, emptyBehavior, maxEmptyBehavior, modBehavior ); 
	}

	// obj 프리팹의 스폰을 만드는 데 사용합니다. 생성된 개체를 반환합니다.
	public static GameObject Spawn ( GameObject prefab ) { 
		// spawns at the position and rotation of the pool
		return Spawn ( prefab, null, Vector3.zero, Quaternion.identity, false );
	}
	public static GameObject Spawn ( GameObject prefab, int? child ) {
		// 자식은 단일 개체가 여러 버전의 개체를 보유할 수 있도록 허용하고
		// 특정 자식만 활성화합니다.
		// null = 자식을 사용하지 않음
		return Spawn ( prefab, child, Vector3.zero, Quaternion.identity, false );
	}
	public static GameObject Spawn ( GameObject prefab, Vector3 pos, Quaternion rot )
	{ 
		// 특정 위치 및 회전 지정
		return Spawn ( prefab, null, pos, rot, true );
	}
	public static GameObject Spawn ( GameObject prefab, int? child, Vector3 pos, Quaternion rot ) {
		return Spawn ( prefab, child, pos, rot, true );
	}
	static GameObject Spawn ( GameObject prefab, int? child, Vector3 pos, Quaternion rot, bool usePosRot ) {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return null;
		} else { // 개체 풀 관리자를 찾았습니다.
			return opmScript.Spawn( prefab, child, pos, rot, usePosRot );
		} 
	}

	public static bool Despawn ( GameObject obj ) {
		if ( obj == null ) { return false; }
		return Despawn( obj.GetComponent<AP_Reference>(), -1f );
	}
	public static bool Despawn ( GameObject obj, float time ) {
		if ( obj == null ) { return false; }
		return Despawn( obj.GetComponent<AP_Reference>(), time );
	}
	public static bool Despawn ( AP_Reference script ) {
		return Despawn( script, -1f );
	} 
	public static bool Despawn ( AP_Reference script, float time ) {
		if ( script == null ) { return false; }
		return script.Despawn( time );
	}

	public static int GetActiveCount ( GameObject obj ) {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return 0;
		} else { 
			return opmScript.GetActiveCount( obj );
		}
	}

	public static int GetAvailableCount ( GameObject obj ) {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return 0;
		} else { 
			return opmScript.GetAvailableCount( obj );
		}
	}

	public static bool DespawnPool ( GameObject obj ) {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return false;
		} else { 
			return opmScript.DespawnPool( obj );
		}
	}

	public static bool DespawnAll () {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return false;
		} else { 
			return opmScript.DespawnAll();
		}
	}

	public static bool RemovePool ( GameObject obj ) {
		bool result = false;
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return false;
		} else { 
			result = opmScript.RemovePool( obj );
			if ( result == true ) { opmScript.poolRef.Remove( obj ); }
			return result;
		}
	}

	public static bool RemoveAll () {
		FindOPM();
		if ( opmScript == null ) { // 개체 풀 관리자를 찾지 못했습니다
			return false;
		} else { 
			return opmScript.RemoveAll();
		}
	}

	static void FindOPM () {
		if ( opmScript == null ) { // 개체 풀 관리자 스크립트를 아직 찾지 못했습니다.
			opmScript = Object.FindObjectOfType<AP_Manager>(); // 씬에서 찾기
		}
	}

}
