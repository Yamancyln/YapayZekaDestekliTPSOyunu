# Yapay Zeka Destekli TPS Oyunu: **Son Kale - Zombi İstilası**

##  Proje Hakkında

"Son Kale: Zombi İstilası", oyuncunun zombilerle dolu bir kalede hayatta kalmaya çalıştığı bir TPS (Third Person Shooter) oyunudur. Unity oyun motoru ve C# dili kullanılarak geliştirilmiştir. Oyuncu karakteri nişan alabilir, ateş edebilir, çömelip ilerleyebilirken, zombiler ise FSM tabanlı bir yapay zekayla devriye gezer, oyuncuyu takip eder ve yakın mesafede sıkıştırdıklarında saldırarak hasar verir.


## Senaryo

Özel Kuvvetler ajanı Kenshin, zombi istilasına uğrayan bölgede tek başına kalmıştı. Ekibi kaybolmuştu ve elindeki tüfek, kalabalık sürülere karşı son savunmasıydı. Görev basitti: Telsizden gelen son koordinatlardaki Yeşil Bölge'ye, yani çalışan radyo istasyonuna ulaşmak. Burası, insanlık için yardım çağrısı yapabileceği son umut noktasıydı. Kenshin, bitmek bilmeyen zombi uğultusuna karşı çelik gibi iradesiyle, yıkılmış binaların arasından hedefine ilerledi. Tek bir amacı vardı: Ulaş ve Çağrı Yap.

##  Platform

- Unity 6000.0.60f1 LTS (URP tabanlı 3D proje)
- Hedef platform: PC (Windows)

##  Takım Üyeleri ve Katkı Alanları

### Karakter ve Kontroller
- Mixamo karakteri import edilerek avatar ayarları yapıldı.
- Yeni silah paketi eklenerek karakterin eline hizalandı.
- Mouse kontrollü kamera ve TPS bakış açısı sağlandı.
- Niişan alma, çömelme ve ateş etme gibi kontroller ve animasyonlar tanımlandı.

### Oyuncu Mekanikleri
- Şarjör sistemi ve mermi sayacı eklenerek HUD ile entegre edildi.
- Silah geri tepme animasyonu ve mermi bittiğinde yeniden doldurma fonksiyonu yazıldı.

### Düşman (Zombi) AI
- FSM (finite state machine) mantığıyla Idle, Patrol, Chase ve Attack state'leri kodlandı.
- NavMesh Agent ve NavMesh Surface ile zombilerin doğru yol takibi sağlandı.
- Zombiler, oyuncuya saldırdığında hasar verir hale getirildi.
- Headshot hasar sistemi yazıldı: kafa vuruşlarında tek vuruşta ölüm.

### Hasar ve Sağlık Sistemi
- Oyuncuya ve zombilere can barı (UI Slider) eklendi.
- Hasar aldıkça sağlık durumunu gösteren sistem entegre edildi.

### Oyun Sonu Mekanikleri
- Oyuncu öldüğünde veya tüm zombiler yok edildiğinde oyun biter.
- Oyun sonunda "Game Over" ekranı görünerek oyun durdurulur.

### Kullanılan Arayüzler
- Pause Menü tasarlandı ve oyun durduğunda devreye girer.
- Diğer menüler (Ana menü, Ayarlar) hazırlansa da finalde devre dışı bırakıldı.

###Kullanılan Teknik ve Mimari Yapılar
---

## 🛠️ Proje Geliştirme Mimarisi ve Teknikleri

Bu proje, Unity motorunun standart özelliklerini ve çeşitli programlama kalıplarını kullanarak bir Üçüncü Şahıs Nişancı (TPS) ve Düşman Yapay Zekası (AI) sistemi oluşturmuştur.

### 1. Yapısal Mimariler ve Kalıplar

#### A. Durum Makinesi (State Machine) Mimarisi
Düşman (Zombi) yapay zekası, Unity'nin `StateMachineBehaviour` sınıfı kullanılarak bir **Durum Makinesi** mimarisi ile tasarlanmıştır. Bu, düşman davranışlarının (Boşta Kalma, Devriye Gezme, Kovalama ve Saldırma) birbirinden net bir şekilde ayrılmasını sağlar.

* **`IdleState.cs` (Boşta Kalma):** Düşmanın hiçbir şey yapmadığı, ancak bir sayaç ile belirli bir süre sonra **Devriye Gezme**'ye geçmeye veya oyuncu menzile girerse hemen **Kovalama**'ya geçmeye karar verdiği durumdur.
* **`PatrolState.cs` (Devriye Gezme):** Düşmanın `NavMeshAgent` kullanarak rastgele belirlenen yol noktaları (`WayPoints`) arasında hareket ettiği durumdur. Süresi dolunca **Boşta Kalma**'ya döner veya oyuncuyu görünce **Kovalama**'ya geçer.
* **`ChaseState.cs` (Kovalama):** Düşmanın `NavMeshAgent` ile hızını artırarak doğrudan oyuncunun pozisyonuna ilerlediği durumdur. Oyuncu çok yaklaşırsa **Saldırma**'ya, çok uzaklaşırsa Devriye veya Boşta Kalma'ya geri döner.
* **`AttackState.cs` (Saldırma):** Düşmanın oyuncuya belirli bir mesafede durup, bir soğuma süresi (`attackCooldown`) ile hasar verdiği durumdur.

#### B. Component-Based Architecture (Bileşen Tabanlı Mimari)
Unity'nin doğal yapısı gereği, her bir işlevsellik bir `MonoBehaviour` bileşeni (script) içine yerleştirilmiştir.

* **`ThirdPersonController.cs`:** Oyuncu hareketi, kamera dönüşü ve kullanıcı girişi gibi temel karakter kontrol işlevlerini yönetir.
* **`Zombie.cs`:** Düşmanın sağlık yönetimi, hasar alma ve ölme animasyonları gibi düşmana özgü verileri tutar.
* **`PlayerHealth` (Kullanılan fakat kodu sunulmayan):** Oyuncunun canını yöneten, düşman saldırısı tarafından çağrılan bir bileşendir.

### 2. Yapay Zeka ve Navigasyon Teknikleri

* **NavMesh Sistemi:** `PatrolState` ve `ChaseState` script'lerinde `UnityEngine.AI.NavMeshAgent` bileşeni kullanılarak düşmanların oyun dünyasında dinamik olarak yol bulması ve engellerden kaçınması sağlanmıştır.
* **Hedefe Yönelme (LookAt):** `AttackState` içinde `animator.transform.LookAt(player);` komutu kullanılarak düşmanın saldırmadan önce doğrudan oyuncuya dönmesi sağlanmıştır.
* **Menzil Kontrolü:** Davranışlar arasındaki geçişler (`isChasing`, `isAttacking`, `isPatrolling` gibi Animator parametreleri) `Vector3.Distance` ile oyuncu ile düşman arasındaki mesafeye bağlı olarak tetiklenir (`chaseRange` = 7f, saldırı menzili $\approx$ 1.5f, kovalama bırakma menzili > 15).
* **Saldırı Soğuma Süresi (`Attack Cooldown`):** `AttackState` içinde `Time.time >= nextAttackTime` kontrolü ile düşmanın sürekli değil, belirlenen bir aralıkta (1.5 saniye) hasar vermesi sağlanmıştır.

### 3. Kontrol ve Girdiler

* **Unity Input System:** Oyuncu girdileri (`UnityEngine.InputSystem` ve `PlayerInput`) kullanılarak modern bir giriş sistemi entegrasyonu yapılmıştır.
* **Karakter Hareketi:** `ThirdPersonController` içinde zıplama, yer çekimi, yürüme, koşma, **yan yürüme (Strafe)** ve **eğilme (Crouching)** gibi karmaşık hareketler Character Controller bileşeni ile yönetilir.
* **Hız Yönetimi:** Hız değişimleri, daha yumuşak bir his sağlamak için `Mathf.Lerp` fonksiyonu ile yumuşak geçişli (organik) olarak uygulanmıştır.

### 4. Animasyon Yönetimi

* **Animator Parametreleri:** Animasyonlar arasındaki geçişler ve hareketler, `animator.SetBool`, `animator.SetFloat` ve `animator.SetTrigger` metotları ile yönetilir (`_animIDSpeed`, `isChasing`, `die` vb.).
* **Hareket Senkronizasyonu:** `ThirdPersonController` içinde, hareket hızı (`_speed`) doğrudan animasyon hızına (`_animIDSpeed`) yansıtılarak hareket ve animasyon senkronize edilir.

### 5. Yardımcı Teknikler

* **GameManager (Kullanılan fakat kodu sunulmayan):** `Zombie.cs` dosyasındaki `RegisterZombie` ve `UnregisterZombie` çağrıları, oyundaki zombi sayısını veya durumunu merkezileştirilmiş bir `GameManager` (Yönetici) sınıfı ile takip etme yönteminin kullanıldığını gösterir.
* **Harici Kaynak Kullanımı:** Kod bloklarının başında belirtildiği gibi, çeşitli işlevler için **GDTitans** ve **Thunder Dev** gibi YouTube kanallarındaki öğreticilerden faydalanılmıştır.

  

##  Literatür ve Kaynaklar
- Unity Asset Store: Starter Asset - Third Person Controller
- Mixamo.com: Karakter ve animasyonlar
- Thunder Dev YouTube: Niişan alma ve ateş etme tutorialları
- GDTitans YouTube: TPS oyun serisi
- Unity Forum ve ChatGPT desteği

##  Karşılaşılan Teknik Zorluklar
- Silahın karakterin eline doğru hizalanması
- Kafa collider'ları ile headshot hasar algısı
- Pause menü ve Game Over ekranlarının kontrolü
- Kamera kontrolü ile nişan alma hareketlerinin çakışmaması
- Zombilerin birbirinden bağımsız olarak devriye ve saldırı davranışları

##  Oyundaki Temel Mekanikler
- Yürüyüş, koşu, zıplama
- Niişan alma ve ateş etme
- Mermi-Şarjör sınırı
- Zombi AI: Patrol, Chase, Attack, Damage, Death
- Hasar alma ve can barı görseli
- Oyun bitirme durumu

##  Kazanımlar
Bu projeyle birlikte:
- Unity ve oyun geliştirme bilgisi edinildi.
- Oyun fiziği, animasyon kontrolü ve yapay zeka gibi konularda tecrübe kazanıldı.
- Kod düzenleme, versiyon kontrolü ve takımla çalışma becerileri geliştirildi.
- Problem çözme, kaynak arama ve dokümantasyon hazırlama pratiği yapıldı.

---

> Rapor, proje belgelerine ve ekip içi çalışmalara dayalı olarak Markdown formatında GitHub README yapısına uygun olacak şekilde hazırlanmıştır.

> Raporla birlikte oyuna ait ekran görüntüleri, Unity Editor görüntüleri ve oyun içi test kareleri aşağıda klasör yapısına eklenmelidir:

```plaintext
📁 README.md
📁 Assets/
📁 Screenshots/
   ├︎ g.jpg
   ├︎ g1.jpg
   ├︎ g2.jpg
   ├︎ g3.jpg
   ├︎ g4.jpg
   ├︎ g5.jpg
   ├︎ g6.jpg
   ├︎ g7.jpg
   ├︎ g8.jpg
   ├︎ g9.jpg
   ├︎ g10.jpg
   ├︎ g11.jpg
   ├︎ g12.jpg
   ├︎ g13.jpg
   ├︎ g14.jpg
   └︎ g15.jpg
```

##  Kurulum

1. Unity 6000.0.60f1 LTS kurulu olmalıdır.
2. Reponun klonlanması:
   ```bash
   git clone https://github.com/Yamancyln/YapayZekaDestekliTPSOyunu.git
   ```
3. Unity Hub ile proje açılmalıdır.

##  Lisans
Bu proje eğitim ve proje ödevi amaçlı geliştirilmiştir. Tüm haklar proje ekibine aittir.

---

Hazırlayanlar: **Yaman Ceylan, Ferhat Sezgin, Efe Aydın**

Tarih: Kasım 2025
