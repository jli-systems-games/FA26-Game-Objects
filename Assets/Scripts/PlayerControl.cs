using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Animator characterAnimator;//This will reference the players Animator component.
   [SerializeField] CharacterController controller;
   [SerializeField] Vector3 playerVelocity;
   [SerializeField] bool groundedPlayer;
   [SerializeField] float playerSpeed;
   [SerializeField] float gravityValue;
   [SerializeField] GameObject activeChar;
   [SerializeField] float moveHorizontal;
   [SerializeField] float moveVertical;
   [SerializeField] float speed = 4;
   [SerializeField] float rotateSpeed = 2;
   [SerializeField] float jumpHeight = 1.2f;
   [SerializeField] bool isJumping;


    void Start()
    {
        characterAnimator = GetComponent<Animator>();// This will automatically grab the Animator component if it exists on the gameboject.
        playerSpeed = 4;
        gravityValue = -20;

    }


    void Update()
    {
        
       

        float moveHorizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;

        if( moveHorizontal == 0 && moveVertical == 0)
        {
            
            characterAnimator.SetInteger("AnimState", 0);
        }
        


        transform.position += new Vector3(moveHorizontal, 0, moveVertical);

        if (Input.GetKey(KeyCode.E) && groundedPlayer)
        {
             isJumping = true;
             activeChar.GetComponent<Animator>().Play("Jumping");
             playerVelocity.y += 10;
         }
    



        
    }
}
