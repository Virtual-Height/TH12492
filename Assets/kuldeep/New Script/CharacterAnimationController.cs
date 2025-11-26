using System.Collections;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class CharacterAnimationController : MonoBehaviour
{
    Animator animator;
    ThirdPersonController thirdPersonController;

    public GameObject thali;
    public GameObject flowerParticle;
    public Transform floawerSocket;
    bool isAnimating;

    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint ik;

    bool isRinging = false;
    public Transform ringPos;

   // public Material playerMaterial;

    [Header("Player Materials")]
    public Material[] clothMaterials;   // assign in inspector


    [Header("Player Renderers")]
    public Renderer[] playerRenderers;   // assign both meshes here

    CheckpointManager checkpointManager;

    private void Awake()
    {
       // playerMaterial.color = Color.white;
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Start()
    {
        FindFirstObjectByType<UIManager>().UnhideCursor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            Debug.Log("SafeZone is call and player in water  ....");

            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

            StartCoroutine(HarHar());
            checkpointManager = other.transform.parent.GetComponent<CheckpointManager>();
        }

        else if (other.CompareTag("ClouthRemove"))
        {
            Debug.Log("ClouthRemove is call....");

            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

            StartCoroutine(ClouthRemove());
            checkpointManager = other.transform.parent.GetComponent<CheckpointManager>();
        }

        else if (other.CompareTag("Aarti&Flower"))
        {
            Debug.Log("Aarti & Flower is call....");  

            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

            FindFirstObjectByType<UIManager>().activityScreen.SetActive(true);

            SetupforActivity();
            checkpointManager = other.transform.parent.GetComponent<CheckpointManager>();
        }

    }

    private void Update()
    {
        if (isRinging)
        {
            ik.weight += 1f * Time.deltaTime * 1.5f;
        }
        else
        {
            ik.weight -= 1f * Time.deltaTime * 1.5f;
        }

        if (isAnimating)
        {
            return;
        }

    }

    IEnumerator HarHar()
    {
        FindFirstObjectByType<UIManager>().activityScreen.SetActive(false);

        GetComponent<Animator>().SetFloat("Speed", 0);
        isAnimating = true;

        animator.SetBool("Swimming", true);
        thirdPersonController.canMove = false;
        yield return new WaitForSeconds(12f);
        animator.SetBool("Swimming", false);
        yield return new WaitForSeconds(1.5f);

        isAnimating = false;



        checkpointManager.CompleteCurrentActivity();
        thirdPersonController.canMove = true;

        GhatInstructionManager.Instance.OnSafeZoneComplete();

    }

    IEnumerator ClouthRemove()
    {
        FindFirstObjectByType<UIManager>().activityScreen.SetActive(false);
        GetComponent<Animator>().SetFloat("Speed", 0);


        /*isAnimating = true;
        animator.SetBool("Swimming", true);*/

        //playerMaterial.color = Color.red;
        thirdPersonController.canMove = false;


        if (clothMaterials.Length > 0)
        {
            Material randomMat = clothMaterials[Random.Range(0, clothMaterials.Length)];

            foreach (Renderer r in playerRenderers)
            {
                r.material = randomMat;
            }
        }
        else
        {
            Debug.LogWarning("clothMaterials array is empty!");
        }


        yield return new WaitForSeconds(2f);

        //animator.SetBool("Swimming", false);
        //yield return new WaitForSeconds(1.5f);

        isAnimating = false;

        checkpointManager.CompleteCurrentActivity();
        thirdPersonController.canMove = true;

        GhatInstructionManager.Instance.OnClothRemoveComplete();

    }

    IEnumerator Arati()
    {
        FindFirstObjectByType<UIManager>().activityScreen.SetActive(false);

        GetComponent<Animator>().SetFloat("Speed", 0);
        isAnimating = true;

        thali.SetActive(true);
        animator.SetBool("Arati", true);
        thirdPersonController.canMove = false;
        yield return new WaitForSeconds(12f);
        animator.SetBool("Arati", false);
        yield return new WaitForSeconds(1.5f);
        thali.SetActive(false);

        //thirdPersonController.canMove = true;
        //isAnimating = false;

        FindFirstObjectByType<UIManager>().activityScreen.SetActive(true);
        GhatInstructionManager.Instance.OnFlowerArpanComplete();


    }

    IEnumerator FlowerArpan()
    {
        FindFirstObjectByType<UIManager>().activityScreen.SetActive(false);
        GetComponent<Animator>().SetFloat("Speed", 0);
        isAnimating = true;
        animator.SetTrigger("Flower");
        thirdPersonController.canMove = false;
        yield return new WaitForSeconds(10f);
        //thirdPersonController.canMove = true;
        //isAnimating = false;
        FindFirstObjectByType<UIManager>().activityScreen.SetActive(true);
        GhatInstructionManager.Instance.OnFlowerArpanComplete();
    }

    public void SetupforActivity()
    {
        GetComponent<Animator>().SetFloat("Speed", 0);
        thirdPersonController.canMove = false;
        isAnimating = true;
    }

    public void ActivityDone()
    {
        thirdPersonController.canMove = true;
        isAnimating = false;
        checkpointManager.CompleteCurrentActivity();
    }

    public void SpawnFlowerParticle()
    {
        GameObject particle = Instantiate(flowerParticle);

        particle.transform.position = floawerSocket.position;
        particle.transform.rotation = floawerSocket.rotation;

        Destroy(particle, 5f);
    }
    public void FlowerAarpanEvent()
    {
        StartCoroutine(FlowerArpan());
    }  

    public void AratiEvent()
    {
        StartCoroutine(Arati());
    }  

}