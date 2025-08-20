using UnityEngine;
namespace pyo
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 캐릭터 이동 속도
        [SerializeField] private JoyStick joyStick; // 조이스틱 참조
        private Rigidbody rigid;

        public bool isForce = false;
        public Vector3 moveDirection;
        void Start()
        {
            rigid = this.transform.GetChild(0).GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isForce = false;

            }
            if (Input.GetMouseButton(0))
            {
                MoveCharacter();

            }
            if (Input.GetMouseButtonUp(0))
            {
                isForce = true;

            }

            if (isForce)
            {


                rigid.AddForce(moveDirection, ForceMode.Impulse); // 힘의 크기를 조절
            }
        }

        private void MoveCharacter()
        {
            Debug.Log("마우스업");
            float horizontalInput = joyStick.Horizontal();
            float verticalInput = joyStick.Vertical();

            // 캐릭터의 이동 방향을 계산
            moveDirection = new Vector3(-horizontalInput, 0, -verticalInput);



        }
    }

}
