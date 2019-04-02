using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    //public ParticleSystem ps;
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
    public Text instructions;

    public AudioSource aud;
    public AudioClip audiowin;
    public AudioClip audiolose;
    public AudioClip audiopickup;
    public AudioClip audioexplosion;
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
        instructions.gameObject.SetActive(false);
        //ps.GetComponent<ParticleSystem>();

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
        else if (other.CompareTag("instructions"))
        {
            instructions.gameObject.SetActive(true);
            StartCoroutine("WaitForSec");
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            if (count > minimumPickupRequired)
            {
                SceneManager.LoadScene("Level 2");
                aud.clip = audiowin;
                aud.Play();
                win();
            }

            else
            {
                lose();
            }
        }
        else if (other.gameObject.CompareTag("Damage"))
        {
            aud.clip = audioexplosion;
            aud.Play();
            Detonate();
            Damage();
            // ps.enableEmission = true;
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

            aud.clip = audiolose;
            aud.Play();
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
            other.gameObject.SetActive(false);
        }
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        instructions.gameObject.SetActive(false);
    }
}

