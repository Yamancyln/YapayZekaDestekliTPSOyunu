using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using Unity.Cinemachine;
using System.Collections;
using TMPro; // ✅ TextMeshPro UI kullanımı için eklendi

// Bu script, üçüncü şahıs nişancı oyunlarında silah tutma, nişan alma, atış yapma, şarjör yenileme,
// kamera ve IK geçişlerini yönetir.
public class WeaponHandler : MonoBehaviour
{
    // === REFERANSLAR ===
    [Header("References")]
    [SerializeField] private CinemachineThirdPersonFollow cm_camera;
    private Animator anim;
    private ThirdPersonController controller;

    // === ATIŞ AYARLARI ===
    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.09f;
    [SerializeField] private float shootBlendTime = 0.075f;
    [SerializeField] private string shootStateName = "Fire_Rifle";
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private ParticleSystem muzzleFlash;
    private bool canShoot = true;

    // === RELOAD (ŞARJÖR YENİLEME) ===
    [Header("Reload Settings")]
    [SerializeField] private int maxAmmo = 30;          // Şarjör kapasitesi
    private int currentAmmo;                            // Mevcut mermi
    [SerializeField] private float reloadTime = 2f;     // Şarjör yenileme süresi
    private bool isReloading = false;                   // Şarjör değiştiriliyor mu?
    [SerializeField] private string reloadStateName = "Reload"; // Animator’daki reload state adı

    // === NİŞAN ALMA (AIMING) AYARLARI ===
    [Header("Aiming")]
    [SerializeField] private float cameraTransitionSpeed = 7f;
    [SerializeField] private float ikTransitionSpeed = 10f;
    [SerializeField] private MultiAimConstraint aimIk;
    [Space(10)]
    [SerializeField] private float aimVerticalArmLength = 0.2f;
    [SerializeField] private float aimCameraSide = 0.75f;
    [SerializeField] private float aimCameraDistance = 0.85f;
    private float defaultVerticalArmLength;
    private float defaultCameraSide;
    private float defaultCameraDistance;

    public bool Aiming { get; private set; }

    // === UI ===
    [Header("UI")]
    [SerializeField] private GameObject crosshair;

    [SerializeField] private TextMeshProUGUI ammoText; // ✅ Mermi sayısını gösterecek UI elementi

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonController>();

        defaultVerticalArmLength = cm_camera.VerticalArmLength;
        defaultCameraSide = cm_camera.CameraSide;
        defaultCameraDistance = cm_camera.CameraDistance;

        currentAmmo = maxAmmo; // oyuna başlarken tam dolu
        UpdateAmmoUI(); // ✅ Başlangıçta mermi sayısını ekrana yaz
    }

    private void Update()
    {
        // Reload sırasında hiçbir işlem yapılmaz (sadece yürüme/nişan izinli)
        if (isReloading)
        {
            anim.SetBool("Aiming", false);
            controller.Strafe = false;
            return;
        }

        // === GİRİŞLER ===
        Aiming = Input.GetButton("Fire2");
        bool shootInp = Input.GetButton("Fire1");

        anim.SetBool("Aiming", Aiming);
        controller.Strafe = Aiming;

        // === KAMERA GEÇİŞLERİ ===
        float targetVerticalArmLength = Aiming ? aimVerticalArmLength : defaultVerticalArmLength;
        float targetSide = Aiming ? aimCameraSide : defaultCameraSide;
        float targetDistance = Aiming ? aimCameraDistance : defaultCameraDistance;

        cm_camera.VerticalArmLength = Mathf.Lerp(cm_camera.VerticalArmLength, targetVerticalArmLength, cameraTransitionSpeed * Time.deltaTime);
        cm_camera.CameraSide = Mathf.Lerp(cm_camera.CameraSide, targetSide, cameraTransitionSpeed * Time.deltaTime);
        cm_camera.CameraDistance = Mathf.Lerp(cm_camera.CameraDistance, targetDistance, cameraTransitionSpeed * Time.deltaTime);

        crosshair.SetActive(Aiming);

        float targetWeight = Aiming ? 1 : 0;
        aimIk.weight = Mathf.Lerp(aimIk.weight, targetWeight, ikTransitionSpeed * Time.deltaTime);

        // === RELOAD GİRİŞİ ===
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // === Mermi bittiyse otomatik reload ===
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // === ATEŞ ETME ===
        if (shootInp && Aiming)
            Shoot();
    }

    private void Shoot()
    {
        if (!canShoot || currentAmmo <= 0 || isReloading)
            return;

        currentAmmo--; // her atışta 1 mermi azalır
        UpdateAmmoUI(); // ✅ Mermi sayısını ekranda güncelle

        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        muzzleFlash.Play();
        anim.CrossFadeInFixedTime(shootStateName, shootBlendTime);
        StartCoroutine(ResetFireRate());

        // === RAYCAST ===
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log("Hit Object: " + hit.collider.name + " | Tag: " + hit.collider.tag);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            Zombie zombi = hit.collider.GetComponentInParent<Zombie>();
            if (zombi != null)
            {
                int damage = hit.collider.CompareTag("ZombieHead") ? 100 : 34;
                zombi.TakeDamage(damage);

                if (hit.collider.CompareTag("ZombieHead"))
                    Debug.Log("HEADSHOT!");
            }
        }
    }

    private IEnumerator ResetFireRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;

        Debug.Log("Reloading...");

        // Animator’a reload trigger gönder
        anim.CrossFadeInFixedTime(reloadStateName, 0.1f);

        yield return new WaitForSeconds(reloadTime); // reload süresi kadar bekle

        currentAmmo = maxAmmo;
        UpdateAmmoUI(); // ✅ Reload sonrası mermi sayısını güncelle

        isReloading = false;
        canShoot = true;

        Debug.Log("Reload complete!");
    }

    // ✅ Yeni: Mermi sayısını UI'da gösteren fonksiyon
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo; // örnek: 29 / 30
    }
}
