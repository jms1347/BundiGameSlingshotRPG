using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[HelpURL("http://mobfarmgames.weebly.com/ap_pool.html")]
public class AP_Pool : MonoBehaviour { 

	public PoolBlock poolBlock;

	[HideInInspector] public Stack<PoolItem> pool;
	[HideInInspector] public List<PoolItem> masterPool; // only used when using EmptyBehavior.ReuseOldest

	int addedObjects;
	int failedSpawns;
	int reusedObjects;
	int peakObjects;
	int origSize;
	int initSize;
	int dynamicSize;
	bool loaded;

	[System.Serializable]
	public class PoolBlock {
		[Tooltip("풀에 있는 개체의 초기 번호입니다.")]
		public int size = 32;
		[Tooltip("객체가 요청되고 풀이 비어 있을 때의 동작입니다.\n\n" +
"성장 - 최대 크기로 \n제한된 새 개체를 풀에 추가합니다.\n\n" +
"실패 - 개체가 생성되지 않습니다.\n\n" +
"가장 오래된 항목 재사용 - 가장 오래된 활성 개체를 재사용합니다.")] 
		// 큰 풀의 경우 성장보다 느리지만 작은 풀의 경우 더 빠릅니다.
		public AP_enum.EmptyBehavior emptyBehavior;
		[Tooltip("성장 동작을 사용할 때 풀이 성장할 수 있는 절대 최대 크기입니다.")]
		public int maxSize = 64; // EmptyBehavior Grow 모드와 함께 사용되는 풀 크기의 절대 제한
		[Tooltip("객체가 요청되고 풀이 비어 있고 풀의 최대 크기에 도달했을 때의 동작입니다.\n\n" +
"실패 - 개체가 생성되지 않습니다.\n\n" +
"가장 오래된 항목 재사용 - 가장 오래된 활성 개체를 재사용합니다.")]
		public AP_enum.MaxEmptyBehavior maxEmptyBehavior; // 풀이 최대 크기일 때의 모드
		[Tooltip("이 풀에 포함된 개체입니다.")]
		public GameObject prefab;
		[Tooltip("장면이 중지되면 풀 사용량을 보여주는 보고서 생성:\n\n" +
				"시작 크기 - 장면이 시작될 때 풀의 크기입니다.\n\n" +
				"End Size - 장면이 종료될 때 풀의 크기입니다.\n\n" +
				"추가된 개체 - 시작 크기를 초과하여 풀에 추가된 개체의 수입니다.\n\n" +
				"실패한 스폰 - 풀에서 사용할 수 있는 개체가 없기 때문에 스폰 횟수가 실패했습니다.\n\n" +
				"재사용된 개체 - 풀에 다시 추가되기 전에 재사용된 개체의 수입니다.\n\n" +
				"가장 많은 개체 활성 - 가장 많은 풀 개체가 동시에 활성 상태입니다.")]
		public bool printLogOnQuit;

		public PoolBlock ( int size, AP_enum.EmptyBehavior emptyBehavior, int maxSize, AP_enum.MaxEmptyBehavior maxEmptyBehavior, GameObject prefab, bool printLogOnQuit ) {
			this.size = size;
			this.emptyBehavior = emptyBehavior;
			this.maxSize = maxSize;
			this.maxEmptyBehavior = maxEmptyBehavior;
			this.prefab = prefab;
			this.printLogOnQuit = printLogOnQuit;
		}
	}

	[System.Serializable]
	public class PoolItem {
		public GameObject obj;
		public AP_Reference refScript;

		public PoolItem ( GameObject obj, AP_Reference refScript ) {
			this.obj = obj;
			this.refScript = refScript;
		}
	}

	void OnValidate () {
		if ( loaded == false )
		{ // 편집기 중에만 실행
			if ( poolBlock.maxSize <= poolBlock.size ) { poolBlock.maxSize = poolBlock.size * 2; } 
		}
	}

	void Awake () {
		loaded = true;

		// required to allow creation or modification of pools at runtime. (Timing of script creation and initialization can get wonkey)
		if ( poolBlock == null ) {
			poolBlock = new PoolBlock( 0, AP_enum.EmptyBehavior.Grow, 0, AP_enum.MaxEmptyBehavior.Fail, null, false );
		} else {
			poolBlock = new PoolBlock( poolBlock.size, poolBlock.emptyBehavior, poolBlock.maxSize, poolBlock.maxEmptyBehavior, poolBlock.prefab, poolBlock.printLogOnQuit );
		}
		pool = new Stack<PoolItem>();
		masterPool = new List<PoolItem>();

		origSize = Mathf.Max( 0, poolBlock.size); 
		poolBlock.size = 0;

		for ( int i=0; i < origSize; i++ ) {
			CreateObject( true );
		}
	}

	void Start () {
		Invoke( "StatInit", 0 ); // for logging after dynamic creation of pool objects from other scripts
	}

	void StatInit () { // for logging after dynamic creation of pool objects from other scripts
		initSize = poolBlock.size - origSize;
	}
		 
	public GameObject Spawn ()
	{ // 풀에서 직접 스폰을 호출하는 데 사용하고 편집기의 "Spawn" 버튼에서도 사용됩니다.
		return Spawn( null, Vector3.zero, Quaternion.identity, false );
	}
	public GameObject Spawn ( int? child )
	{ // 풀에서 직접 스폰을 호출하는 데 사용
		return Spawn( child, Vector3.zero, Quaternion.identity, false );
	}
	public GameObject Spawn ( Vector3 pos, Quaternion rot ) { // use to call spawn directly from the pool
		return Spawn( null, pos, rot, true );
	}
	public GameObject Spawn ( int? child, Vector3 pos, Quaternion rot ) { // use to call spawn directly from the pool
		return Spawn( child, pos, rot, true );
	}
	public GameObject Spawn ( int? child, Vector3 pos, Quaternion rot, bool usePosRot ) {
		GameObject obj = GetObject();
		if ( obj == null ) { return null; } // early out

		obj.SetActive(false); // 개체가 재사용되는 경우 항목을 재설정하고 개체가 이미 비활성화된 경우에는 효과가 없습니다.
		obj.transform.parent = null;
		obj.transform.position = usePosRot ? pos : transform.position;
		obj.transform.rotation = usePosRot ? rot : transform.rotation;
	
		obj.SetActive(true);

		if ( child != null && child < obj.transform.childCount )
		{ // 특정 자식 활성화
			obj.transform.GetChild( (int)child ).gameObject.SetActive(true); 
		}

		if ( peakObjects < poolBlock.size - pool.Count ) { peakObjects = poolBlock.size - pool.Count; } // for logging
		return obj;
	}

	public void Despawn ( GameObject obj, AP_Reference oprScript ) { // return an object back to this pool
		if ( obj.transform.parent == transform ) { return; } // already in pool
		obj.SetActive(false);
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		oprScript.CancelInvoke();
		pool.Push( new PoolItem( obj, oprScript ) );
	}

	public GameObject GetObject ()
	{ // 풀에서 개체 가져오기, 필요한 경우 개체 만들기 및 설정이 허용하는 경우
		GameObject result = null;
		if ( pool.Count == 0 ) {
			if ( poolBlock.emptyBehavior == AP_enum.EmptyBehavior.Fail ) { failedSpawns++; return null; }

			if ( poolBlock.emptyBehavior == AP_enum.EmptyBehavior.ReuseOldest ) {
				result = FindOldest();
				if ( result != null ) { reusedObjects++; }
			}

			if ( poolBlock.emptyBehavior == AP_enum.EmptyBehavior.Grow ) {
				if ( poolBlock.size >= poolBlock.maxSize ) {
					if ( poolBlock.maxEmptyBehavior == AP_enum.MaxEmptyBehavior.Fail ) { failedSpawns++; return null; }
					if ( poolBlock.maxEmptyBehavior == AP_enum.MaxEmptyBehavior.ReuseOldest ) {
						result = FindOldest();
						if ( result != null ) { reusedObjects++; }
					}
				} else {
					addedObjects++;
					return CreateObject();
				}
			}
		} else {
			pool.Peek().refScript.timeSpawned = Time.time;
			return pool.Pop().obj;
		}
		return result;
	}

	GameObject FindOldest ()
	{ // 또한 반환된 객체에 대해 timeSpawned를 설정합니다.
		GameObject result = null;
		int oldestIndex = 0;
		float oldestTime = Mathf.Infinity;
		if ( masterPool.Count > 0 ) {
			for ( int i = 0; i < masterPool.Count; i++ ) {
				if ( masterPool[i] == null || masterPool[i].obj == null ) { continue; } // make sure object still exsists
				if ( masterPool[i].refScript.timeSpawned < oldestTime ) { 
					oldestTime = masterPool[i].refScript.timeSpawned;
					result = masterPool[i].obj;
					oldestIndex = i;
				}
			}
			masterPool[ oldestIndex ].refScript.timeSpawned = Time.time;
		}
		return result;
	}

	public GameObject CreateObject () {
		return CreateObject ( false );
	}
	public GameObject CreateObject ( bool createInPool )
	{ // 풀에 아이템을 생성하지 않고 생성할 때 true
		GameObject obj = null;
		if ( poolBlock.prefab ) {
			obj = (GameObject) Instantiate( poolBlock.prefab, transform.position, transform.rotation );
			AP_Reference oprScript = obj.GetComponent<AP_Reference>();
			if ( oprScript == null ) { oprScript = obj.AddComponent<AP_Reference>(); }
			oprScript.poolScript = this;
			oprScript.timeSpawned = Time.time;
			masterPool.Add( new PoolItem( obj, oprScript ) );

			if ( createInPool == true ) {
				pool.Push( new PoolItem( obj, oprScript ) );
				obj.SetActive(false);
				obj.transform.parent = transform;
			}
			poolBlock.size++;
		}
		return obj;
	}

	public int GetActiveCount () {
		return poolBlock.size - pool.Count;
	}

	public int GetAvailableCount () {
		return pool.Count;
	}

	void OnApplicationQuit () { 
		if ( poolBlock.printLogOnQuit == true ) {
			PrintLog();
		}
	}

	public void PrintLog () {
		Debug.Log( transform.name + ":       Start Size: " + origSize + "    Init Added: " + initSize + "    Grow Objects: " + addedObjects + "    End Size: " + poolBlock.size + "\n" +
			"    Failed Spawns: " + failedSpawns + "    Reused Objects: " + reusedObjects + "     Most objects active at once: " + peakObjects );
	}

}
