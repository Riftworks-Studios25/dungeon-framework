using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private Rigidbody2D rb;
    [SerializeField] public float baseSpeed = 5;
    private float moveSpeed;
    public float extraSpeed = 1f;
    private Animator anim;
    private SpriteRenderer sr;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        Move(moveAction.ReadValue<Vector2>());
    }

    void Move(Vector2 moveVector)
    {
        moveSpeed = baseSpeed;
        moveSpeed *= extraSpeed;

        int moveHor = 0;
        int moveVert = 0;
        anim.SetBool("movingRight", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingDown", false);
        anim.SetBool("movingUp", false);

        if (moveVector.x > 0f)
        {
            moveHor = 1;
            anim.SetBool("movingRight", true);
        }
        else if (moveVector.x < 0f)
        {
            moveHor = -1;
            anim.SetBool("movingLeft", true);
        }

        if (moveVector.y > 0f)
        {
            moveVert = 1;
            if (!anim.GetBool("movingLeft") && !anim.GetBool("movingRight"))
                {
                    anim.SetBool("movingUp", true);
                }
        }
        else if (moveVector.y < 0f)
        {
            moveVert = -1;
            if (!anim.GetBool("movingLeft") && !anim.GetBool("movingRight"))
                {
                    anim.SetBool("movingDown", true);
                }
        }
        float moveAmountHor = moveHor * moveSpeed;
        float moveAmountVert = moveVert * moveSpeed;


        // Diagonal speed normalization
        if (moveAmountHor != 0 && moveAmountVert != 0)
        {
            moveAmountHor *= 0.707f;
            moveAmountVert *= 0.707f;
        }
        
        // If the player isn't moving, stop the movement animation
        if (anim.GetBool("movingUp") == false && 
            anim.GetBool("movingDown") == false && 
            anim.GetBool("movingLeft") == false && 
            anim.GetBool("movingRight") == false)
            {
                anim.enabled = false;

                AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    AnimationClip clip = clipInfo[0].clip;

                    clip.SampleAnimation(anim.gameObject, 0f);

                }
            }
            else
            {
                anim.enabled = true;
            }

        rb.linearVelocity = new Vector2(moveAmountHor, moveAmountVert);
    }

    public void AddSpeed(float amount)
    {
        extraSpeed += amount;
    }
}
