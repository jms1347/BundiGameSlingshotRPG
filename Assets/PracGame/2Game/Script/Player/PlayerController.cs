using UnityEngine;
namespace pyo
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // ĳ���� �̵� �ӵ�
        [SerializeField] private JoyStick joyStick; // ���̽�ƽ ����
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


                rigid.AddForce(moveDirection, ForceMode.Impulse); // ���� ũ�⸦ ����
            }
        }

        private void MoveCharacter()
        {
            Debug.Log("���콺��");
            float horizontalInput = joyStick.Horizontal();
            float verticalInput = joyStick.Vertical();

            // ĳ������ �̵� ������ ���
            moveDirection = new Vector3(-horizontalInput, 0, -verticalInput);



        }
    }

}
