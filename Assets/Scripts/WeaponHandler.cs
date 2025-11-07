using UnityEngine; // Unity motorunun temel işlevlerine erişim sağlar (GameObject, Component, vb.).
using StarterAssets; // Starter Assets paketindeki karakter kontrolcüleri gibi sınıfları kullanmak için.
using UnityEngine.Animations.Rigging; // Unity'nin Animasyon Rigging sistemi (IK, vb.) bileşenlerine erişim sağlar.
using Unity.Cinemachine; // Cinemachine sanal kamera sistemine erişim sağlar.
using System.Collections; // Coroutine (eş zamanlı çalışan rutinler) kullanmak için gereklidir.
using TMPro; //  TextMeshPro UI kullanımı için eklendi

// Bu script, üçüncü şahıs nişancı oyunlarında silah tutma, nişan alma, atış yapma, kamera ve IK geçişlerini yönetir.
//Youtube "Thunder Dev" adlı kanalın "Unity Third Person Shooter Tutorial – Smooth Aiming & Shooting" isimli videosundan yardım alındı.
public class WeaponHandler : MonoBehaviour
{
    // === REFERANSLAR ===
    [Header("References")] // Inspector penceresinde başlık oluşturur.
    [SerializeField] private CinemachineThirdPersonFollow cm_camera; // Cinemachine kamerasının takip bileşenine referans. Nişan alma sırasında kamera pozisyonunu değiştirmek için kullanılır.
    private Animator anim; // Karakterin Animator bileşenine referans. Animasyonları (nişan alma, ateş etme) kontrol etmek için.
    private ThirdPersonController controller; // StarterAssets'ten gelen karakter hareket kontrolcüsüne referans. Nişan alırken yana yürüme (strafe) modunu etkinleştirmek için.

    // === ATIŞ AYARLARI ===
    [Header("Shooting")] // Inspector penceresinde başlık oluşturur.
    [SerializeField] private float fireRate = 0.09f; // Silahın atış hızı (iki atış arasındaki minimum bekleme süresi).
    [SerializeField] private float shootBlendTime = 0.075f; // Atış animasyonuna geçiş (crossfade) süresi.
    [SerializeField] private string shootStateName = "Fire_Rifle"; // Atış animasyonunun Animator'daki state adı.
    [SerializeField] private AudioClip shootSound; // Atış sırasında çalınacak ses klibi.
    [SerializeField] private ParticleSystem muzzleFlash; // Namlu alevi (muzzle flash) parçacık sistemi efekti.
    private bool canShoot = true; // Ateş edip edemeyeceğimizi kontrol eden bayrak (fireRate ile kontrol edilir).

    // === RELOAD (ŞARJÖR YENİLEME) ===
    [Header("Reload Settings")]
    [SerializeField] private int maxAmmo = 30;          // Şarjör kapasitesi
    private int currentAmmo;                            // Mevcut mermi
    [SerializeField] private float reloadTime = 2f;     // Şarjör yenileme süresi
    private bool isReloading = false;                   // Şarjör değiştiriliyor mu?
    [SerializeField] private string reloadStateName = "Reload"; // Animator’daki reload state adı

    // === NİŞAN ALMA (AIMING) AYARLARI ===
    [Header("Aiming")] // Inspector penceresinde başlık oluşturur.
    [SerializeField] private float cameraTransitionSpeed = 7f; // Kamera pozisyonu geçişlerinin yumuşaklık hızı.
    [SerializeField] private float ikTransitionSpeed = 10f; // IK ağırlığının geçiş yumuşaklık hızı.
    [SerializeField] private MultiAimConstraint aimIk; // Silahın hedefe bakmasını sağlayan MultiAimConstraint IK bileşenine referans.
    [Space(10)] // Inspector'da görsel boşluk bırakır.
    [SerializeField] private float aimVerticalArmLength = 0.2f; // Nişan alırken Cinemachine'in dikey kol uzunluğu.
    [SerializeField] private float aimCameraSide = 0.75f; // Nişan alırken kameranın yana kayma miktarı (omuz üstü nişan).
    [SerializeField] private float aimCameraDistance = 0.85f; // Nişan alırken kameranın karakterden uzaklığı.
    private float defaultVerticalArmLength; // Nişan almıyorken varsayılan dikey kol uzunluğu.
    private float defaultCameraSide; // Nişan almıyorken varsayılan yana kayma miktarı.
    private float defaultCameraDistance; // Nişan almıyorken varsayılan kamera uzaklığı.

    public bool Aiming { get; private set; } // Nişan alma durumunu tutan ve dışarıdan sadece okunabilen özellik.

    // === KULLANICI ARAYÜZÜ (UI) ===
    [Header("UI")] // Inspector penceresinde başlık oluşturur.
    [SerializeField] private GameObject crosshair; // Nişan imleci (crosshair) GameObject'ine referans.

    [SerializeField] private TextMeshProUGUI ammoText; // Mermi sayısını gösterecek UI elementi

    private void Start()
    {
        anim = GetComponent<Animator>(); // Script'in bulunduğu GameObject'teki Animator bileşenini alır.
        controller = GetComponent<ThirdPersonController>(); // ThirdPersonController bileşenini alır.

        // Cinemachine kamerasının varsayılan pozisyon ayarlarını kaydederek nişan alma bitince geri dönülmesini sağlar.
        defaultVerticalArmLength = cm_camera.VerticalArmLength;
        defaultCameraSide = cm_camera.CameraSide;
        defaultCameraDistance = cm_camera.CameraDistance;

        UpdateAmmoUI(); // Başlangıçta mermi sayısını ekrana yaz
    }

    // Her karede bir kez çağrılır. Oyun mantığının çoğunu içerir.
    private void Update()
    {
        // Reload sırasında hiçbir işlem yapılmaz (sadece yürüme/nişan izinli)
        if (isReloading)
        {
            anim.SetBool("Aiming", false);
            controller.Strafe = false;
            return;
        }

        // === GİRİŞ (INPUT) YÖNETİMİ ===
        Aiming = Input.GetButton("Fire2"); // "Fire2" (varsayılan sağ fare tuşu) basılı mı kontrol eder.
        bool shootInp = Input.GetButton("Fire1"); // "Fire1" (varsayılan sol fare tuşu) basılı mı kontrol eder.

        // === ANİMASYONLAR VE HAREKET ===
        anim.SetBool("Aiming", Aiming); // Animator'daki "Aiming" boolean parametresini günceller (nişan alma animasyonu).
        controller.Strafe = Aiming; // Nişan alıyorsa (Aiming true ise) karakteri yana yürüme (strafe) moduna geçirir.

        // === KAMERA AYARLAMA (SMOOTH TRANSITION) ===
        // Nişan alma durumuna (Aiming) göre hedeflenen kamera pozisyon değerlerini (aim veya default) belirler.
        float targetVerticalArmLength = Aiming ? aimVerticalArmLength : defaultVerticalArmLength;
        float targetSide = Aiming ? aimCameraSide : defaultCameraSide;
        float targetDistance = Aiming ? aimCameraDistance : defaultCameraDistance;

        // Cinemachine kamera parametrelerini, belirlenen hedefe doğru yumuşak bir geçişle (Mathf.Lerp) hareket ettirir.
        cm_camera.VerticalArmLength = Mathf.Lerp(cm_camera.VerticalArmLength, targetVerticalArmLength, cameraTransitionSpeed * Time.deltaTime);
        cm_camera.CameraSide = Mathf.Lerp(cm_camera.CameraSide, targetSide, cameraTransitionSpeed * Time.deltaTime);
        cm_camera.CameraDistance = Mathf.Lerp(cm_camera.CameraDistance, targetDistance, cameraTransitionSpeed * Time.deltaTime);

        // === KULLANICI ARAYÜZÜ (UI) ===
        crosshair.SetActive(Aiming); // Nişan alınıyorsa crosshair'ı gösterir, aksi halde gizler.

        // === TERS KİNEMATİK (IK) YÖNETİMİ ===
        // Nişan alınıyorsa IK ağırlığını 1'e, almıyorsa 0'a hedefler.
        float targetWeight = Aiming ? 1 : 0;
        // IK ağırlığını yumuşak bir şekilde hedeflenen değere doğru hareket ettirir.
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

        // === ATIŞ KONTROLÜ ===
        if (shootInp && Aiming) // Ateş etme tuşuna basıldıysa VE karakter nişan alıyorsa.
            Shoot(); // Atış fonksiyonunu çağırır.
    }

    // Silahın ateş etme mantığını içeren fonksiyon.
    private void Shoot()
    {
        if (!canShoot || currentAmmo <= 0 || isReloading)
            return;

        currentAmmo--; // her atışta 1 mermi azalır
        UpdateAmmoUI(); // Mermi sayısını ekranda güncelle

        AudioSource.PlayClipAtPoint(shootSound, transform.position); // Atış sesini karakterin pozisyonunda bir kerelik oynatır.
        muzzleFlash.Play(); // Namlu alevi efektini başlatır.
        anim.CrossFadeInFixedTime(shootStateName, shootBlendTime); // Atış animasyonunu belirlenen süre içinde oynatmaya başlar.
        StartCoroutine("ResetFireRate"); // Ateş etme bekleme süresini başlatan coroutine'i çağırır.

        //chatGPT den yardım alındı bu kısım için
        // === IŞIN FIRLATMA (RAYCAST) İLE HEDEF TESPİTİ ===
        // Kameranın ekran ortasından (crosshair'ın pozisyonu) bir ışın (ray) oluşturur.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Viewport'ta (0,0) alt sol, (1,1) sağ üsttür. (0.5, 0.5) tam ortadır.
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Raycast'i fırlatır. Eğer ışın bir cisme çarparsa ve mesafe 100f içindeyse:
        {
            Debug.Log("Hit Object: " + hit.collider.name + " | Tag: " + hit.collider.tag);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            Zombie zombi = hit.collider.GetComponentInParent<Zombie>(); // Çarptığı cismin collider'ında "Zombie" bileşeni var mı diye bakar.
            if (zombi != null)  // Eğer bir Zombi bileşeni bulunursa.
            {
                // Çarptığı collider'ın etiketi "ZombieHead" ise 100 hasar (kafa vuruşu), aksi takdirde 34 hasar belirler.
                int damage = hit.collider.CompareTag("ZombieHead") ? 100 : 34; // Kafaya ateş edildiğinde tek atışla zombi ölür onun dışındaki bölgelere daha az hasar verili
                zombi.TakeDamage(damage);  // Zombi'nin hasar alma fonksiyonunu çağırır.

                if (hit.collider.CompareTag("ZombieHead"))
                    Debug.Log("HEADSHOT!");
            }
        }
    }

    // Ateş etme bekleme süresini (fireRate) yöneten coroutine.
    private IEnumerator ResetFireRate()
    {
        canShoot = false; // Hemen yeni bir atışı engeller.
        yield return new WaitForSeconds(fireRate); // 'fireRate' saniye kadar bekler.
        canShoot = true; // Bekleme süresi bittikten sonra tekrar atış yapılmasına izin verir.
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
        UpdateAmmoUI(); // Reload sonrası mermi sayısını güncelle

        isReloading = false;
        canShoot = true;

        Debug.Log("Reload complete!");
    }

    // Yeni: Mermi sayısını UI'da gösteren fonksiyon
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo; // örnek: 29 / 30
    }
}
