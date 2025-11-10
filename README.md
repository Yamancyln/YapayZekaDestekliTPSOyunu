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
