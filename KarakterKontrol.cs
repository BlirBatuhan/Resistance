using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kütüphanem;
using UnityEngine.UI;

public class KarakterKontrol : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Image HealthBar;
    public GameObject GameManager;

    private CharacterController controller;
    private Vector3 velocity; // Hız ve yön bilgisi
    private bool isGrounded; // Zeminde mi kontrolü

    private Animator anim;
    private float inputX;
    private float inputY;
    private float currentSpeed;
    Animasyon animasyon = new Animasyon();
    public static float Saglık;
    float[] Sol_Yon_Parametreleri = { 0.12f, 0.34f, 0.63f, 1 };
    float[] Sag_Yon_Parametreleri = { 0.12f, 0.34f, 0.63f, 1 };
    float[] Egilme_Yon_Parametreleri = { 0.2f, 0.35f, 0.40f, 0.45f, 0.92f};

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Saglık = 100f;
    }

    public void SaglıkDurumu(float DarbeGucu)
    {
        Saglık -= DarbeGucu;
        Debug.Log("Sağlık: " + Saglık);
        HealthBar.fillAmount = Saglık / 100;
        if (Saglık <= 0)
        {
            //ölme canvası çıkacak
            GameManager.GetComponent<GameManager>().Kaybettin();
            Saglık = 0;
            Debug.Log("Öldünüz");
        }
    }

    private void LateUpdate()
    {
        // Zeminde mi kontrolü
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Zemine yapışmasını sağlamak için küçük bir değer
        }

        // Hareket girdisi
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        // 🟢 Geri yürüme kontrolü
        bool isWalkingBackward = inputY < 0; // Eğer S tuşuna basıyorsa

        // Koşma kontrolü (eğer eğilmiyorsa koşabilir)
        bool isRunning =  Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && inputY > 0;
       
        if(isWalkingBackward)
        {
            currentSpeed = walkSpeed * 0.75f; // Geri yürürken hızın %75'e düşmesi
        }
        else
        {
            currentSpeed = isRunning ? runSpeed : walkSpeed;
        }

        // Zıplama kontrolü
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Yerçekimi uygula
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime; // Havadayken yerçekimi
        }

        // Hareket vektörü oluştur
        Vector3 move = transform.right * inputX + transform.forward * inputY;

        // **Animasyon Kontrolleri**
        float animationSpeed = new Vector3(inputX, 0, inputY).magnitude;
        anim.SetFloat("speed", animationSpeed * (isRunning ? 1f : 0.5f)); // Koşma için hız çarpanı gerçekçi animasyon için

        if (Input.GetKey(KeyCode.W))
        {
            if (isRunning) // Koşma
                anim.SetFloat("speed", 1f);
            else // Yürüme
                anim.SetFloat("speed", 0.35f);
        }
        else
        {
            anim.SetFloat("speed", 0); // Durma
        }

        // **Sağa ve Sola Hareket Animasyonları**
        animasyon.Sol_Hareket(anim, isRunning, "Sol_hareket", animasyon.ParamtereOlustur(Sol_Yon_Parametreleri));
        animasyon.Sag_Hareket(anim, isRunning, "Sag_hareket", animasyon.ParamtereOlustur(Sag_Yon_Parametreleri));
        animasyon.Geri_Hareket(anim, "geriYürü");

        // **Eğilme animasyonu çağrılırken hız azaltılıyor**
        animasyon.Egilme_Hareket(anim, "Egilme", animasyon.ParamtereOlustur(Egilme_Yon_Parametreleri), ref currentSpeed);

        // Hareket uygula (yerçekimini ve hareket hızını içeren toplam vektör)
        controller.Move((move * currentSpeed + velocity) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            GameManager.GetComponent<GameManager>().Kazandin();
        }
    }
}
