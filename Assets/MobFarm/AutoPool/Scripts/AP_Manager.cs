using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[HelpURL("http://mobfarmgames.weebly.com/ap_manager.html")]
public class AP_Manager : MonoBehaviour {

	public bool allowCreate = true;
	public bool allowModify = true;

	[Tooltip("장면이 중지되면 풀 사용량을 보여주는 보고서를 생성합니다.\n\n" +
	"시작 크기 - 장면을 시작할 때 풀의 크기입니다.\n\n" +
	"초기화 추가됨 - 런타임에 InitializeSpawn()에 의해 추가된 개체 수입니다.\n\n" +
	"Grow Objects - EmptyBehavior.Grow로 추가된 개체 수.\n\n" +
	"종료 크기 - 로그 보고 시점에 이 풀의 활성 및 비활성 총 개체입니다.\n\n" +
	"실패한 스폰 - 스폰을 반환하지 않은 Spawn() 요청의 수입니다.\n\n" +
	"재사용된 개체 - 정상적으로 디스폰되기 전에 개체가 재사용된 횟수입니다.\n\n" +
	"가장 많이 활성화된 개체 - 이 풀의 가장 많은 항목이 한 번에 활성화됩니다.")]
	public bool printAllLogsOnQuit;

	[HideInInspector] public Dictionary<GameObject, AP_Pool> poolRef;

	void Awake () {
		CheckDict();
	}

	void CheckDict() {
		if ( poolRef == null )
		{ 
			// 아직 사전이 생성되지 않았습니다
			poolRef = new Dictionary<GameObject, AP_Pool>();
		}
	}

	public bool InitializeSpawn ( GameObject obj, float addPool, int minPool, AP_enum.EmptyBehavior emptyBehavior, AP_enum.MaxEmptyBehavior maxEmptyBehavior, bool modBehavior ) { 
		if ( obj == null ) { return false; }
		CheckDict();
		bool result = false;
		bool tempModify = false;

		if ( poolRef.ContainsKey( obj ) == true && poolRef[obj] == null )
		{ 
			// 깨진 참조 확인
			poolRef.Remove( obj ); // remove it
		}
		if ( poolRef.ContainsKey( obj ) == true ) {
				result = true; // already have refrence
		} else {
			if ( MakePoolRef( obj ) == null ) { // ref not found
				if ( allowCreate == true ) {
					CreatePool( obj, 0, 0, emptyBehavior, maxEmptyBehavior );
					tempModify = true; // may modify a newly created pool
					result = true;
				} else {
					result = false;
				}
			} else {
				result = true; // ref was created
			}
		}

		if ( result == true ) { // hava a valid pool ref
			if ( allowModify == true || tempModify == true ) { // may modify a newly created pool
				if ( addPool > 0 || minPool > 0 ) {
					int size = poolRef[obj].poolBlock.size;
					int l1 = 0; int l2 = 0;
					if ( addPool >= 0 ) { // not negative
						if ( addPool < 1 ) { // is a percentage
							l2 = Mathf.RoundToInt( size * addPool );
						} else { // not a percentage
							l1 = Mathf.RoundToInt( addPool );
						}
					}
					int loop = 0;
					int a = size == 0 ? 0 : Mathf.Max( l1, l2 );
					if ( size < minPool ) { loop = minPool - size; }
					loop += a;
					for ( int i=0; i < loop; i++ ) {
						poolRef[obj].CreateObject( true );
					}
					poolRef[obj].poolBlock.maxSize = poolRef[obj].poolBlock.size * 2;
					if ( modBehavior == true ) {
						poolRef[obj].poolBlock.emptyBehavior = emptyBehavior;
						poolRef[obj].poolBlock.maxEmptyBehavior = maxEmptyBehavior;
					}
				}
			}
		}

		return result;
	}

	public GameObject Spawn ( GameObject obj, int? child, Vector3 pos, Quaternion rot, bool usePosRot ) {
		if ( obj == null ) { return null; } // object wasn't defined
		CheckDict();

		if ( poolRef.ContainsKey( obj ) == true ) { // reference already created
			if ( poolRef[obj] != null ) { // make sure pool still exsists
				return poolRef[obj].Spawn( child, pos, rot, usePosRot ); // create spawn
			} else { // pool no longer exsists
				poolRef.Remove( obj ); // remove reference
				return null;
			}
		} else { // ref not yet created
			AP_Pool childScript = MakePoolRef ( obj ); // create ref
			if ( childScript == null ) { // ref not found
				return null;
			} else {
				return childScript.Spawn( child, pos, rot, usePosRot ); // create spawn
			}
		}
	}

	AP_Pool MakePoolRef ( GameObject obj )
	{ 
		// 스크립트 참조 생성 및 반환 시도
		for ( int i=0; i < transform.childCount; i++ ) {
			AP_Pool childScript = transform.GetChild(i).GetComponent<AP_Pool>();
			if ( childScript && obj == childScript.poolBlock.prefab ) {
				poolRef.Add( obj, childScript );
				return childScript;
			}
		}
//		Debug.Log( obj.name + ": Tried to reference object pool, but no matching pool was found." );
		return null;
	}

	public int GetActiveCount ( GameObject prefab ) {
		if ( prefab == null ) { return 0; } // object wasn't defined
		AP_Pool childScript = null;
		if ( poolRef.ContainsKey( prefab ) == true ) { // reference already created
			childScript = poolRef[prefab];
		} else { // ref not yet created
			childScript = MakePoolRef ( prefab ); // create ref
		}
		if ( childScript == null ) { // pool not found
			return 0;
		} else {
			return childScript.poolBlock.size - childScript.pool.Count;
		}
	}

	public int GetAvailableCount ( GameObject prefab ) {
		if ( prefab == null ) { return 0; } // object wasn't defined
		AP_Pool childScript = null;
		if ( poolRef.ContainsKey( prefab ) == true ) { // reference already created
			childScript = poolRef[prefab];
		} else { // ref not yet created
			childScript = MakePoolRef ( prefab ); // create ref
		}
		if ( childScript == null ) { // pool not found
			return 0;
		} else {
			return childScript.pool.Count;
		}
	}

	public bool RemoveAll () {
		bool result = true;
		GameObject[] tempObj = new GameObject[poolRef.Count];
		int i = 0;
		foreach ( GameObject obj in poolRef.Keys ) {
			if ( poolRef[obj] != null ) {
				tempObj[i] = obj;
				i++;
			}
		}
		for ( int t=0; t < tempObj.Length; t++ ) {
			if ( tempObj[t] != null ) {
				if ( RemovePool( tempObj[t] ) == false ) { result = false; }
			}
		}
		return result;
	}

	public bool DespawnAll () {
		bool result = true;
		foreach ( GameObject obj in poolRef.Keys ) {
			if ( DespawnPool( obj ) == false ) { result = false; }
		}
		return result;
	}

	public bool RemovePool ( GameObject prefab ) {
		if ( prefab == null ) { return false; } // object wasn't defined
		bool result = false;
		AP_Pool childScript = null;
		if ( poolRef.ContainsKey( prefab ) == true ) { // reference already created
			childScript = poolRef[prefab];
		} else { // ref not yet created
			childScript = MakePoolRef ( prefab ); // create ref
		}
		if ( childScript == null ) { // pool not found
			return false;
		} else {
			result = DespawnPool( prefab );
			Destroy( childScript.gameObject );
			poolRef.Remove( prefab );
			return result;
		}
	}
		
	public bool DespawnPool ( GameObject prefab ) {
		if ( prefab == null ) { return false; } // object wasn't defined
		AP_Pool childScript = null;
		if ( poolRef.ContainsKey( prefab ) == true ) { // reference already created
			childScript = poolRef[prefab];
		} else { // ref not yet created
			childScript = MakePoolRef ( prefab ); // create ref
		}
		if ( childScript == null ) { // pool not found
			return false;
		} else {
			for ( int i=0; i < childScript.masterPool.Count; i++ ) {
				childScript.Despawn( childScript.masterPool[i].obj, childScript.masterPool[i].refScript ) ;
			}
			return true;
		}
	}

	public void CreatePool () {
		CreatePool ( null, 32, 64, AP_enum.EmptyBehavior.Grow, AP_enum.MaxEmptyBehavior.Fail );
	}
	public void CreatePool ( GameObject prefab, int size, int maxSize, AP_enum.EmptyBehavior emptyBehavior, AP_enum.MaxEmptyBehavior maxEmptyBehavior ) {
		GameObject obj = new GameObject("Object Pool");
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		AP_Pool script = obj.AddComponent<AP_Pool>();
		if ( Application.isPlaying == true ) {
			obj.name = prefab.name;
			script.poolBlock.size = size;
			script.poolBlock.maxSize = maxSize;
			script.poolBlock.emptyBehavior = emptyBehavior;
			script.poolBlock.maxEmptyBehavior = maxEmptyBehavior;
			script.poolBlock.prefab = prefab;
			if ( prefab ) { MakePoolRef( prefab ); }
		}
	}

	void OnApplicationQuit () { 
		if ( printAllLogsOnQuit == true ) {
			PrintAllLogs();
		}
	}

	public void PrintAllLogs () {
		foreach ( AP_Pool script in poolRef.Values ) {
			script.PrintLog();
		}
	}

}
