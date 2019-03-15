using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public int lives = 3;
    public float speed;
    public float jumpForce = 2.0f;
    public float xGrowRate = 0.1F;
    public float yGrowRate = 0.1F;
    public float zGrowRate = 0.1F;
    public float playerMass = 1.0f;
    public float massGrowRate = 0.1f;
    public int minimumPickupRequired = 11;

    //bomb attributes
    
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    
    public Text countText;
    public Text winText;
    public Text loseText;
    public Text livesText;

    public AudioSource aud;
    public AudioClip audiowin;
    public AudioClip audiolose;
    public AudioClip audiopickup;
    private Rigidbody rb;
    private int count;
    private bool gameEnded = false;
    private Vector3 jump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        rb.mass = playerMass;
        SetCountText();
        aud = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void SetCountText()
    {
        if (count <= 1)
            countText.text = "Coin: " + count.ToString();
        else
            countText.text = "Coins: " + count.ToString();
    }

    void update()
    {

    }
    void FixedUpdate()
    {
        if (gameEnded) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            aud.clip = audiopickup;
            aud.Play();
            IncreaseCount();
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            if (count > minimumPickupRequired)
            {
                aud.clip = audiowin;
                aud.Play();
                win();
            }

            else
            {
                aud.clip = audiolose;
                aud.Play();
                lose();
            }
        }
        else if (other.gameObject.CompareTag("Damage"))
        {
            Detonate();
            Damage();
        }


        void IncreaseCount()
        {
            count = count + 1;

            transform.localScale += new Vector3(xGrowRate, yGrowRate, zGrowRate);
            rb.mass += massGrowRate;
        }

        void win()
        {

            winText.gameObject.SetActive(true);
            gameEnded = true;
            animator.SetBool("Win", true);
        }

        void lose()
        {
           
            loseText.gameObject.SetActive(true);
            
            gameEnded = true;
        }
     
        void Damage()
        {
            lives -=1;
            livesText.text = "Lives: " + lives;
            if (lives < 1)
            {
                animator.SetBool("Die", true);
                lose();
            }
        }
        void Detonate()
        {
            Vector3 explosionPosition = gameObject.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb2 = hit.GetComponent<Rigidbody>();
                if (rb2 != null)
                {
                    rb2.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
                }
            } 
        }
    }
}

