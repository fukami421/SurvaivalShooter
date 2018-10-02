using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turning();
        Animating(h, v);

    }

    //Playerを移動させる
    void Move(float h,float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
             
    }

    //Playerを回転させる
    void Turning()
    {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        int layerMask = LayerMask.GetMask("Floor");
        if (Physics.Raycast(camRay, out floorHit, camRayLength))
        {
            Debug.Log("当たったのは" + floorHit.collider.name);
            if (Physics.Raycast(camRay, out floorHit, camRayLength, layerMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                playerRigidbody.MoveRotation(newRotation);
                // Rayの可視化
                //Debug.DrawRay(camRay.origin, camRay.direction, Color.red,10);
            }
        }
    }

    void Animating(float h,float v)
    {
        bool walking = (h != 0 || v != 0);
        anim.SetBool("IsWalking", walking);
    }
}
