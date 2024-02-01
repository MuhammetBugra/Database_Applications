CREATE DATABASE databaseApp;
USE databaseApp;

CREATE TABLE `Publisher`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `publisher_name` VARCHAR(100) NOT NULL
);
CREATE TABLE `Genre`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `genre_name` VARCHAR(50) NOT NULL
);
CREATE TABLE `OrderDetails`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `order_id` INT UNSIGNED NOT NULL,
    `book_id` INT UNSIGNED NOT NULL,
    `quantity` INT UNSIGNED NOT NULL,
    `unit_price` DECIMAL(8, 2) NOT NULL
);
CREATE TABLE `Customer`(
    `id` INT UNSIGNED NOT NULL PRIMARY KEY,
    `wallet` DECIMAL(8, 2) NOT NULL DEFAULT '0'
);
CREATE TABLE `Language`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `language_name` VARCHAR(50) NOT NULL
);
CREATE TABLE `Order`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `customer_id` INT UNSIGNED NOT NULL,
    `order_date` DATETIME NOT NULL,
    `total_amount` DECIMAL(8, 2) NOT NULL,
    `payment_method` INT NOT NULL,
    `shipping_address` INT UNSIGNED NOT NULL,
    `order_status` INT NOT NULL DEFAULT '1'
);
CREATE TABLE `Book`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `title` VARCHAR(100) NOT NULL,
    `author` INT UNSIGNED NOT NULL,
    `ISBN` VARCHAR(13) NOT NULL,
    `price` DECIMAL(8, 2) NOT NULL,
    `stock_quantity` INT UNSIGNED NOT NULL DEFAULT '10',
    `publisher` INT UNSIGNED NULL,
    `publication_date` DATE NULL,
    `number_of_sales` INT UNSIGNED NOT NULL DEFAULT '0',
    `number_of_page` INT UNSIGNED NULL,
    `description` TEXT NULL,
    `genre` INT UNSIGNED NULL,
    `language` INT UNSIGNED NULL,
    `image` BLOB(153600) NULL
);
ALTER TABLE
    `Book` ADD UNIQUE `book_isbn_unique`(`ISBN`);
CREATE TABLE `Cities`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `city_name` VARCHAR(255) NOT NULL
);
CREATE TABLE `Users`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `username` VARCHAR(255) NOT NULL,
    `first_name` VARCHAR(50) NOT NULL,
    `last_name` VARCHAR(50) NOT NULL,
    `gender` VARCHAR(6) NULL,
    `email` VARCHAR(255) NOT NULL,
    `phone` VARCHAR(100) NULL,
    `password_hash` VARCHAR(255) NOT NULL,
    `create_date` DATETIME NOT NULL,
    `last_login_date` DATETIME NULL,
	`reset_Token` VARCHAR(255) NULL,
	`reset_Token_Expires` DATETIME NULL,
    `is_manager` BIT NOT NULL DEFAULT 0
);
ALTER TABLE
    `Users` ADD UNIQUE `users_username_unique`(`username`);
ALTER TABLE
    `Users` ADD UNIQUE `users_email_unique`(`email`);
CREATE TABLE `Towns`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `city_id` INT UNSIGNED NOT NULL,
    `town_name` VARCHAR(50) NOT NULL
);
CREATE TABLE `Credit_Card`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` INT NOT NULL,
    `title` VARCHAR(50) NOT NULL,
    `cvc` VARCHAR(3) NOT NULL,
    `number` VARCHAR(20) NOT NULL,
    `credit_card_name` VARCHAR(100) NOT NULL,
    `expiration_date` DATETIME NOT NULL
);
ALTER TABLE
    `Credit_Card` ADD UNIQUE `credit_card_number_unique`(`number`);
CREATE TABLE `Districts`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `town_id` INT UNSIGNED NOT NULL,
    `district_name` VARCHAR(50) NOT NULL
);
CREATE TABLE `Address`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` INT UNSIGNED NOT NULL,
    `city_id` INT UNSIGNED NOT NULL,
    `town_id` INT UNSIGNED NOT NULL,
    `district_id` INT UNSIGNED NOT NULL,
    `postal_code` VARCHAR(10) NULL,
    `address_text` VARCHAR(255) NOT NULL
);
CREATE TABLE `Author`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `author_name` VARCHAR(50) NOT NULL
);
CREATE TABLE `Rating`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` INT UNSIGNED NOT NULL,
    `book_id` INT UNSIGNED NOT NULL,
    `rating` INT UNSIGNED NOT NULL
);
CREATE TABLE `Comment`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` INT UNSIGNED NOT NULL,
    `book_id` INT UNSIGNED NOT NULL,
    `comment` TEXT NOT NULL
);
CREATE TABLE `orderstatus`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `status_name` VARCHAR(10) NOT NULL
);
CREATE TABLE `paymentmethod`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `method_name` VARCHAR(15) NOT NULL
);

INSERT INTO `paymentmethod` (`id`, `method_name`)
VALUES
('1', 'cash'),
('2', 'credit_card');

INSERT INTO `orderstatus` (`id`, `status_name`)
VALUES
('1', 'Preparing'),
('2', 'Shipped'),
('3', 'Delivered');

INSERT INTO `Rating` (`user_id`, `book_id`, `rating`)
VALUES
(2, 1, 4), (2, 2, 3), (2, 3, 5), (2, 4, 2), (2, 5, 1), (2, 6, 4), (2, 7, 2), (2, 8, 3), (2, 9, 5), (2, 10, 1), (2, 11, 4), (2, 12, 3), (2, 13, 5), (2, 14, 2), (2, 15, 1), (2, 16, 4), (2, 17, 3), (2, 18, 5), (2, 19, 2), (2, 20, 3), (2, 21, 1), (2, 22, 4), (2, 23, 2), (2, 24, 5), (2, 25, 3), (2, 26, 4), (2, 27, 1), (2, 28, 5), (2, 29, 2), (2, 30, 3), (2, 31, 4), (2, 32, 2), (2, 33, 5), (2, 34, 1), (2, 35, 3), (2, 36, 4), (2, 37, 5), (2, 38, 2), (2, 39, 1), (2, 40, 3), (2, 41, 4), (2, 42, 5), (2, 43, 2), (2, 44, 1), (2, 45, 4), (2, 46, 2), (2, 47, 3), (2, 48, 5), (2, 49, 1), (2, 50, 3), (2, 51, 4), (2, 52, 2), (2, 53, 5), (2, 54, 1), (2, 55, 4), (2, 56, 3), (2, 57, 5), (2, 58, 2), (2, 59, 1), (2, 60, 4), (2, 61, 2), (2, 62, 3), (2, 63, 5), (2, 64, 1),
(4, 1, 5), (4, 2, 2), (4, 3, 4), (4, 4, 3), (4, 5, 1), (4, 6, 4), (4, 7, 5), (4, 8, 2), (4, 9, 3), (4, 10, 4), (4, 11, 2), (4, 12, 5), (4, 13, 1), (4, 14, 3), (4, 15, 4), (4, 16, 5), (4, 17, 2), (4, 18, 3), (4, 19, 4), (4, 20, 2), (4, 21, 5), (4, 22, 1), (4, 23, 3), (4, 24, 4), (4, 25, 2), (4, 26, 5), (4, 27, 1), (4, 28, 4), (4, 29, 3), (4, 30, 5), (4, 31, 2), (4, 32, 1), (4, 33, 4), (4, 34, 5), (4, 35, 3), (4, 36, 2), (4, 37, 4), (4, 38, 1), (4, 39, 3), (4, 40, 4), (4, 41, 5), (4, 42, 2), (4, 43, 1), (4, 44, 4), (4, 45, 3), (4, 46, 5), (4, 47, 2), (4, 48, 4), (4, 49, 1), (4, 50, 5), (4, 51, 3), (4, 52, 4), (4, 53, 2), (4, 54, 1), (4, 55, 4), (4, 56, 3), (4, 57, 5), (4, 58, 2), (4, 59, 1), (4, 60, 4), (4, 61, 5), (4, 62, 2), (4, 63, 1), (4, 64, 4);

INSERT INTO `author` (`id`, `author_name`)
VALUES
('1', 'Peyami Safa'),
('2', 'Cengiz Aytmatov'),
('3', 'Tarık Buğra'),
('4', 'Ziya Gökalp'),
('5', 'Cengiz Dağcı'),
('6', 'George Orwell'),
('7', 'Rene Descartes'),
('8', 'Victor Hugo'),
('9', 'Oğuz Atay'),
('10', 'Cemil Meriç'),
('11', 'John Steinback'),
('12', 'Yakup Kadri Karaosmanoğlu'),
('13', 'Doğan Cüceloğlu'),
('14', 'Halil İnalçık'),
('15', 'Mustafa Kutlu'),
('16', 'Marcus Aurelius'),
('17', 'Franz Kafka'),
('18', 'Lev N. Tolstoy'),
('19', 'Virginia Woolf');

INSERT INTO `genre` (`id`, `genre_name`)
VALUES
('1', 'Turkish Literature Novels'),
('2', 'Ottoman History Books'),
('3', 'Novel'),
('4', 'Narrative Books'),
('5', 'Story Books'),
('6', 'Fairy Tales'),
('7', 'Legend and Epic Books'),
('8', 'General Sociology Books'),
('9', 'Culture and Science Books'),
('10', 'Russian Literature'),
('11', 'Memoir Letter and Diary Books'),
('12', 'Thought Books'),
('13', 'General Philosophy Books'),
('14', 'Philosophical Movement Books'),
('15', 'Books in Foreign Languages'),
('16', 'World Children\'s Classics'),
('17', 'Biographical and Autobiographical Books'),
('18', 'Culture and Science Books'),
('19', 'American Literature'),
('20', 'Non-Governmental Organization Books'),
('21', 'General Psychology Books'),
('22', 'History Research and Review Books');

INSERT INTO `language` (`id`, `language_name`)
VALUES
('1', 'Turkish'),
('2', 'French'),
('3', 'English');

INSERT INTO `publisher` (`id`, `publisher_name`)
VALUES
('1', 'Ötüken Neşriyat'),
('2', 'Ketebe Yayınevi'),
('3', 'İletişim Yayınları'),
('4', 'Kapra Yayıncılık'),
('5', 'Karbon Kitaplar'),
('6', 'Can Yayınları'),
('7', 'Say Yayınları'),
('8', 'Sel Yayınları'),
('9', 'Kronik Kitap'),
('10', 'Remzi Kitabevi'),
('11', 'Türkiye İş Bankası Kültür Yayınları'),
('12', 'Dergah Yayınları');

INSERT INTO `book` (id, title, author, ISBN, price, stock_quantity, publisher, publication_date, number_of_sales, number_of_page, description, genre, language, image)
VALUES
(1, 'Dokuzuncu Hariciye Koğuşu', 1, '9786254085086', 70.00, 10, 1, '2023-02-16', 0, 112, 'Peyami Safa\'nın şaheserlerinden Dokuzuncu Hariciye Koğuşu, Türk edebiyatında “insan ruhunun derinliklerinde ve labi­rentlerinde dolaşan ilk roman” olması ve hasta bir insanı ve onun psikolojisini ele alması bakımından önemli bir yere sahiptir. Birçok araştırmacı ve yazar tarafından Türk edebiyatında bir ilk kabul edilen Dokuzuncu Hariciye Koğuşu, Tanpınar dediği gibi, “acının ve ıstırabın yegâne kitabı” olarak hem kemiyet hem de keyfiyet bakımından başka hiçbir eser olmasa da Türk romanının var olduğuna delil gösterilebilecek kudrette bir eserdir. Romanın genç kahramanı, ayağındaki rahatsızlıktan kurtulabilmek için sayısız doktora görünür ve en nihayetinde havadar bir ortamda, stresten uzak bir istirahat dönemi geçirmesi gerektiğine ikna edilir. Ancak, gerek akrabaları olan bir Paşa\'nın Erenköyü\'ndeki köşkünde misafir kaldığı dönemde, gerekse kendi evi ve hastaneye gidiş gelişlerinde şuurunu adeta bir facia atmosferinde yoğurur. Peyami Safa\'nın çocukluk ve gençlik dönemlerinden fazlasıyla izler taşıyan roman, hem umudu ve umutsuzluğu, hem de sevinci ve felaketi aynı sayfalara sığdırabilmiş olması bakımından insanın eşsiz bir tarifini sunuyor.', 1, 1, null),
(2, 'Fatih Harbiye', 1, '9786254085079', 70.00, 10, 1, '2022-01-11', 0, 128, 'Darülelhan’ın (Konservatuvarın) alaturka kısmında ud eğitimi alan Neriman, mensup olmakla iftihar ettiği Doğu kültürünü çok seven babası Faiz Bey’le on beş yaşından beri Fatih semtinde oturmaktadır. Yine bu semtte ta­nıştığı, babasına çok benzeyen ve Darülelhan’da kemençe eğitimi alan Şinasi ile yedi yıldır nişanlı­dır. Bütün mahalle, tahammül sınırlarını zorlayan bu nişanlılık ilişkisinin evlilikle bitmesini beklemektedir. Ancak Neriman’ın Darülelhan’da tanıştığı Macit, onun içinde yer etmiş Batılı bir hayat yaşama isteğini uyandırır. Neriman, Beyoğlu’nda, Harbiye’de yaşanan ışıltılı hayat tarzına imrenerek yaşadığı muhitten, evlerinden, babasın­dan, Şinasi’den ve hatta doğuyu temsil ettiğini düşündüğü kedisinden bile nefret etmeye başlar. Tramvay yoluyla birbirine bağlanan ama birbiriyle bağdaşması mümkün olmayan iki semt, Fatih ve Harbiye, aynı coğrafyada yaşanan bir kültür ve zihin geriliminin cepheleridir. Türk edebiyatının en üretken kalemi Peyami Safa, televizyon dizilerine de konu olan Fatih-Harbiye romanında toplumumuzun yaşadığı asrîleşme (çağdaşlaşma) sancılarına eşyalar, şahıslar, kurumlar ve mekânlar üzerinden ayna tutmaktadır.', 1, 1, null),
(3, 'Yalnızız', 1, '9786254085093', 170.00, 10, 1, '2023-03-15', 0, 413, 'Peyami Safa\'nın son romanı Yalnızız, engin ruh tahlilleri ve kendi türünde açtığı çığırla onu yalnızca Türk edebiyatının değil, Dünya edebiyatının zirvelerine taşımış şaheseridir. Peyami Safa\'nın diğer bütün romanlarında olduğu gibi Yalnızız romanında da doğu-batı, madde-mânâ, ruh-beden, idealizm-materyalizm gibi ikilemler üzerinde durularak, aynı evde yaşadıkları hâlde birbirlerinden oldukça farklı mizaç, düşünce ve insan ilişkilerine sahip aile fertleri üzerinden ruhunu arayan bir toplum resmedilir. Bireysel ve toplumsal kimliklerimiz arasında, bilhassa Batılılaşma hareketlerinden sonra ortaya çıkan uyumsuzluğun yarattığı sıkıntılar, kalabalıklar içinde milyonlarca “yalnız”ın peyda olmasına sebep olmuştur. Yalnızız; sıra dışı kurgusu ve bir üst kurmaca metin olarak romanda kendine yer bulan ütopya ülkesi Simeranya ile yarım asırdır Türk edebiyatının en çok okunan ve sevilen romanlarının başında geliyor.', 1, 1, null),
(4, 'Türk İnkılabına Bakışlar', 1, '9789754370010', 120.00, 10, 1, '2022-02-15', 0, 202, 'Türk İnkılâbı\'nın temellerini erken bir dönemde hiçbir tartışmaya mahal vermeyecek açıklıkla aşikâr kılan bir eserdir. "Türk İnkılâbına Bakışların iki özelliği vardır. Birincisi inkılâp öncesi fikir cereyanlarını en gerçek kaynaklarıyla ortaya koymaya çalışmış olmasıdır. Kitaptaki vesikalardan, Atatürk inkılâbının İkinci Meşrutiyette ortaya çıkan ve müdafaası yapılan Avrupalılaşma hareketinden aynen ilham aldığı görülür. Eserin ikinci özelliği, Türk İnkılâbının tarih felsefesi, medeniyetlerin mukayesesi, Şark (Doğu) ve Garp (Batı) mefhumlarının tahlili, İslâm Türk ve Batı düşünceleri arasındaki kaynakların müşterek oluşunu izah bakımından ilk deneme oluşudur.', 2, 1, null),
(5, 'Beyaz Gemi', 2, '9786257303880', 70.00, 10, 2, '2021-02-06', 0, 180, 'Cengiz Aytmatov’dan geçmişle geleceğin, hafızayla hayal gücünün, ayrılık ve kavuşmanın ustaca bir araya getirildiği, beyaz perdeye de uyarlanmış unutulmaz bir eser! Engin ve korkutucu bir ormanın kıyısında, iyi yürekli dedesinin himayesine terk edilmiş küçük bir çocuk; balık olmayı, Isık-Göl’ün sularında ağır ağır yüzen beyaz gemiye ulaşmayı düşler. Gemide babası ve tamamlanmış bir hayat onu bekliyordur. Dedesi Hamarat Momun ise yalnız torununa ormanın ve kimsesiz çocukların koruyucusu Boynuzlu Geyik Ana’nın masalını anlatır sabırla. Elbet ormanın kalbinden çıkıp gelecektir Boynuzlu Geyik Ana. İnsanın acımasız tabiatını tüm gerçekliği ile gözler önüne sererek. Beyaz Gemi; yalnızlık, kökler, düşler, dünler ve yarınlar üzerine çarpıcı bir hikâye…', 3, 1, null),
(6, 'Toprak Ana', 2, '9789754371543', 60.00, 10, 1, '2023-01-11', 0, 136, 'Cengiz Aytmatov, Toprak Ana romanında erkekleri askere alınan bozkırın ortasındaki bir Kırgız köyünde geride kalanların çektiği sıkıntıları anlatıyor. Eldeki yetersiz yiyeceğin muhtaç olandan başlanarak dağıtılması, dört gözle beklenen hasat zamanları, umutların hasat zamanına ertelenmesi, savaş yüzünden ürünün hemen hepsinin merkezden istenmesi, boşa çıkan umutlar, yine açlık, sefalet, bir yandan cepheden gelen ölüm haberleri, umutsuz bekleyişler, savaşın uzun sürmesi üzerine aşağı çekilen cepheye çağrılma yaşı, anaların evlatlarını bir bir askere göndermesi, ayrılıklar, gözyaşları... Yani tek kelimeyle ve bütün zulmetiyle; savaş. Cengiz Aytmatov, o her zamanki berrak ve akıcı üslûbuyla bizleri, adeta insanları öğütür gibi harcayan savaş düzeneğinin yarattığı trajedilerle sarsıyor.', 3, 1, null),
(7, 'Masallar ve Efsaneler', 2, '9786256495340', 220.00, 10, 2, '2023-08-04', 0, 120, 'Kırgızistan\’ın Şeker Köyü\’nde dünyaya gelen Cengiz Aytmatov, büyükannesi Ayimkan\’dan dinlediği türküler, ninniler, masallar ve efsanelerle büyür. Büyükannesi misafirliğe, yeni doğan bir bebeğin doğumunu kutlamaya, cenaze törenine, düğüne hemen hemen gittiği her yere Cengiz\’i de götürür. Aytmatov bu sayede yepyeni türküler, şiirler, masallar ve efsaneler duyar. Yazarın hayal gücünün sınırlarını genişleten bu ilk adımlar, her zaman onun aklında ve kalbinde yer alır, kalemine işler; zamanla dünya edebiyatının sayılı isimlerinden biri olur, eserleri 170’ten fazla dile çevrilir. Cengiz Aytmatov, Masallar ve Efsaneler’de çeşitli sebeplerle meydana gelen doğaüstü olayları büyük bir gerçekçilik ve kendine has üslubuyla anlatıyor; okuyucusunu zorlu savaş zamanlarına, mitik eski dönemlere, büyülü mekânlara, destansı anlatılara götürüyor. Yaşlı bir cadı tarafından hayvana dönüştürülen küçük kız Gabi\’nin, Kırgızların manevi atası olan Manas\’ın yardımıyla ailesine kavuşması ve ormana terk edilen üç kız kardeşe yoldaş olan bir hayvanla en zorlu zamanlarda bile iyiliğin bir şekilde karşımıza çıkabileceği aktarılıyor. Ayrıca büyüklerinin nasihatini dinlemediği için önce deve sonra kurt tarafından yutulan Parmak Çocuk’un verdiği mücadele, savaştan sonra sağ kalan yedi kişilik askeri bir ekibin içindeki ajanın çaresizliği bu masalları oluşturuyor. Yenisey Nehri kıyısında hem öksüz hem yetim kalan iki çocuğu kurtaran ceylan Maral Ana, kutsal Ala Geyik’in canını alan acımasız avcı Karagül, Nayman Ana\’nın uzun arayışlar sonrası bulduğu oğlu Mankurt ve sevdiği şair Muhtar’a kavuşamayan İyi Han\’ın kızı Akbara’nın efsaneleri okurunu heyecan dolu serüvenlere davet ediyor.', 4, 1, null),
(8, 'Gün Olur Asra Bedel', 2, '9789754370539', 135.00, 10, 1, '2020-12-04', 0, 413, 'Yürek paralayan, tüyler ürperten bir haykırış.... Geçmiş, bugün ve yarın; bilim-kurgu, gerçek ve efsane bir arada gözler önüne serilir... Derin ve temiz aşklar, efsane ve masallar, KGB\'nin acımasız uygulamaları, okuru heyecandan heyecana sürükler. Birbirinden ilginç ve sürükleyici konular ustalıkla bütünleştirilerek sunulur. \"Mankurt hikâyesi bu eserle kültürümüze mal edilir. Yedigey, ölen emektar arkadaşı Kazangap\'ın cenazesini mezarına götürürken, kendisinin ve milletinin geçmişini, acı-tatlı, düşündürücü yanlarıyla bir bir gözlerinin önünden geçirir. O gün \"asra bedel bir gün olur.', 3, 1, null),
(9, 'Osmancık', 3, '9789754370799', 195.00, 10, 1, '2023-02-21', 0, 376, '\"Osmanlı\'nın sırrı nedir\" sorusunun cevabını arayan yazarın Osmanlı kuruluş döneminin dinamiklerini ve felsefesini bugünkü dille inşa ettiği romandır. Duvarları süsleyen \"Ey Osmancık; beğsin. Bundan sonra öfke bize, uysallık sana; güceniklik bize, gönül alma sana; suçlama bizde, katlanma sende; bundan böyle, yanılgı bize, hoş görmek sana; aciz bize, yardım sana; geçimsizlikler, uyuşmazlıklar, anlaşmazlıklar, çatışmalar bize, adalet sana; kötü göz bize, şom ağız bize, haksız yorum bize, bağışlama sana. Ey Osmancık bundan böyle, bölmek bize, bütünlemek sana; üşengenlik bize, gayret sana; uyuşukluk bize, rahat bize, uyarmak şevklendirmek, gayretlendirmek sana\" gibi sözler bu kitabın eseridir.', 1, 1, null),
(10, 'Oğlumuz / Yarın Diye Bir Şey Yoktur', 3, '9789750525377', 167.00, 10, 3, '2020-06-12', 0, 280, 'Tarık Buğra, Kurtuluş Savaşı\’nı ve Türkiye Cumhuriyeti’nin kuruluş sorunsalını konu alan siyasal roman geleneğimizin Yakup Kadri Karaosmanoğlu ve Kemal Tahir ile birlikte önde gelen yazarlarından biri olmasının yanı sıra öykücülüğüyle de dikkat çeker. Öykülerinde çoğu zaman “sıradan” insanın başından geçenleri ya da geçmesi ihtimal dahilinde olanları kendine has bir duyuş ile anlatan Buğra, bazen bir hastalığın hüznünü, bazen bir aşkın tutkusunu, bazen de bir sohbetin neşesini kendimiz yaşıyormuşçasına içimizde hissettirir. Romanlarında olduğu gibi öykülerinde de taşrada olmayı, taşra insanıyla bir arada bulunmayı, sözün özü “taşranın ruhunu” anlatmayı ihmâl etmez. Tarık Buğra\’nın kaleme aldığı öykülerin ilk kısmını bir araya getiren bu kitap, daha önce Buğra\’yı sadece romanlarından bilen okurları “öykü de yazmış bir romancı” ile değil, her cümlesiyle başlı başına bir öykücüyle bir araya getirirken, aynı zamanda Buğra\’nın metinleriyle ilk kez karşılaşacak okurların Tarık Buğra edebiyatının büyük “giriş kapısını” aralamalarına bir imkân sağlıyor. “Buğra\’nın, hikâyeciliğini belirgin iki çizgi üzerinde geliştirerek dönemin edebi tartışmalarına teoriyle değil, pratikle yanıt verdiğini düşünebiliriz. Buğra öykücülüğünün bir çizgisi Proust ve Tanpınar\’la buluştuğu \‘zaman\’ çizgisidir. Bu elbette Bergson sonrası modernist yazının da çizgisidir. (...) Buğra öykücülüğünün başta sözünü ettiğim ikinci çizgisi hümanizmdir. Zamana ilişkin öykülerinde nasıl Tanpınar\’la aynı yerdeyse, insancıl ve insancı öykülerinde de Sait Faik çizgisindedir.” Jale Parla’nın Önsöz’ünden...', 5, 1, null),
(11, 'Bu Çağın Adı (Makaleler)', 3, '9786254082863', 220.00, 10, 1, '2022-03-16', 0, 347, '“Bu Çağın Adı”, aydınlarımızı, idârecilerimizi ve bütün akıl sâhiplerini düşünmeye sevkeden konuları içine almaktadır. Politik şarlatanlıklara karşı gerçekleri ve bağımsız kafayı savunan; kısacası şahsiyetli insanlara yakışan bir tavır ve üslûpla millet ve memleket meselelerine bakmayı gündeme getiren bu makalelerin, okuyanlara çok şey ifâde edeceği inancındayız.', 5, 1, null),
(12, 'Küçük Ağa / Toplu Eserleri 1', 3, '9789750501982', 199.00, 10, 3, '2022-02-16', 0, 477, 'Küçük Ağa, Kurtuluş Savaşı yıllarında, siyasal karar ve tartışma merkezlerinin uzağında, Kuvvacı/Millici denilen, ama ne oldukları, neyi temsil ettikleri pek bilinmeyen birilerinin açtığı savaşa katılıp katılmamanın vebalini tartarak bir karar verme durumunda kalan insanları anlatır. Asırlarır sadece \"halife-i ruyi zemin\"in, padişahın açtığı sancağın altında savaşılacağı bilgi ve inancıyla yaşamış taşra insanlarının, halife-padişah çağrısının yokluğunda ve işgal haberleri yayılırken yaşadıkları ikilemlerin, açmaz ve iç çalkantıların, kendileri ve kaderlerine sahip çıkma hakkında yeniden düşünmek zorunda kalışlarının hikayesidir. Tarık Buğra\'nın kendi deyişiyle Küçük Ağa, destanlara yakışır bir konuyu ele almasına rağmen, destan değil, gerçekliği anlatan bir romandır. İttihatçıların ve Kuvvacıların değil, inanç ve gelenek kalıtıyla başbaşa, ilk kez kendisi ve kendi adına geleceği için karar vermeye çalışan bir ahalinin \"kahraman\"ı olduu bir roman. Şimdilerde Küçük Ağa\'yı okumak, güncelliğini bir kez daha kazanmış bir öyküyü, sorunsalı yeniden okumak demektir.', 1, 1, null),
(13, 'Türk Medeniyeti Tarihi', 4, '9786257419321', 26.00, 10, 4, '2021-07-29', 0, 339, 'Ziya Gökalp, Türk Medeniyeti Tarihi adlı bu eserini ömrünün son yıllarında kaleme aldı. Gökalp\’ın ölümünden sonra yayımlanan bu eser, onun düşünce dünyasının bir özeti mahiyetindedir.  Gökalp bu eserinde, Türk medeniyeti tarihinin temel kavramlarını, Türk medeniyetinin devirlerini,  Türk kültürü ile Türk medeniyeti arasındaki farkları bilimsel disiplinden ödün vermeden inceler. Yazıldığı dönemin önemli tartışmalarından beslenen bu eserde, Türklerin tarih boyunca geliştirdikleri siyasal yapılardan askerî stratejilere, evlilik geleneklerinden yeme içme kültürüne dair birçok konu titizlikle ele alınır. Türk Medeniyeti Tarihi, neredeyse bir asır önce kaleme alınmış olmasına rağmen yazarının ileri sürdüğü düşünceler hâlâ önemini ve güncelliğini korumaktadır', 8, 1, null),
(14, 'Altın Işık', 4, '9786257751643', 20.00, 10, 4, '2021-01-13', 0, 200, 'Altın Işık, manzum, nesir masallar ve piyesle Türk millî kültürünü gelecek nesle aktaracak köklü bir eser. İçerisinde her yaştan insanın keyifle okuyacağı, Keloğlan masalları, peri masalları ve saray masallarını saklıyor. En eski Türk boyları, gelenek ve görenekleri hakkında detaylı bilgilerin yer aldığı masallarıyla Gökalp, okurunu Türk tarihinin ve kültürünün engin sayfalarında tarifsiz bir yolculuğa çıkarıyor.', 6, 1, null),
(15, 'Kızıl Elma', 4, '9786052194638', 66.00, 10, 5, '2018-06-13', 0, 148, 'En son ne zaman masal dinlediniz, ya da okudunuz?', 7, 1, null),
(16, 'Türk Töresi', 4, '9786052194522', 43.00, 10, 5, '2018-01-16', 0, 96, 'Türk kültürünün tarihini yazmak niyetiyle yola çıkan Ziya Gökalp, Türk Töresi kitabında Türk dini ve siyasi tarihinin adeta el değmemiş bilgilerini gün yüzüne çıkarıyor. Tarih felsefesi ve tarih sosyolojisi alanında Türkiye\’de ilk çalışmaları yapan Gökalp, eserinde en eski Türk efsanelerini ve menkıbelerini toplu bir şekilde okuyucusuna sunuyor. Günümüzde süregelen bazı geleneklerin ve konuşulan deyimlerin temelinin eski Türk mitolojisinde atıldığını gösteren yazar, güncelliğini koruyan eseriyle bu asrın kültür ve medeniyetine de ışık tutuyor.', 9, 1, null),
(17, 'Onlar Da İnsandı', 5, '9786254083044', 220.00, 10, 1, '2022-01-11', 0, 494, '\"Evet, onlar da insandır! Pavlenko\'lar, İvan\'lar, Kostyürk\'ler, Vasil Dimitroviç\'ler, Stepan\'lar, belki bunu gülünç görecekler; ama nasıl görürlerse görsünler, ben eserimiz tekrar sakin bir dua ile bitirmek istiyorum. Romanımı kapatırken: \"Tanrım!\" diyorum. \"Onlar da insan!\" Acı onlara! Kendileri gibi, başkalarının da insan olduklarına inandır onları!\" Ötekiler, o hayvan gibi sürülüp götürülenler... Onlar da insandı.\"', 10, 1, null),
(18, 'Üşüyen Sokak', 5, '9789754372076', 90.00, 10, 1, '2022-02-15', 0, 192, 'Cengiz Dağcı, Üşüyen Sokak’ta, İkinci Dünya Savaşı’nın bütün şiddetiyle devam ettiği günlerde Kırım’ın Almanlar tarafından işgalini anlatır. Roman; kötü yola düşmüş Almira’nın sokakta tesadüf ettiği ürkek ve ne yapacağını bilmeyen Enstitü öğrencisi genç Halûk’u alıp bir apartman dairesine götürmesini ve Halûk’un orada geçirdiği üç günü bizlere gösterir. Cengiz Dağcı diğer romanlarından farklı olarak, Üşüyen Sokak’ta iç sese ve bilinç akışına büyük bir ağırlık vererek kahramanı Halûk’un sürgün ve siyasî baskılarla bunaltılan Kırım Türk toplumu içinde kendisini tanımasını ve dünyayı anlamlandırmasını, zorlu kış şartlarının ve işgalci askerlerin ablukaya aldığı bir sokak başındaki apartman dairesinin penceresinden hikâye eder.', 10, 1, null),
(19, 'Hatıralarda', 5, '9786254083594', 170.00, 10, 1, '2022-06-29', 0, 292, 'Yazarın kendi kaleminden hatıraları... Bir bakıma roman kahramanlarıyla kendisi arasında kurulan ilişkiye açıklık getirir. Çocukluğunu, savaşa gidişini, esir düşüşünü, iltica edişini ve yazarlığının merhalelerini anlatır. Elli yıl boyunca gönlünü sevindiren, yüreğini acıtan, elinde kalem masa başında otururken onu ağlatan Kırım... Karısıyla tanışması ve karısının vedası... Türk okuyucusuyla kurduğu ilişki...', 11, 1, null),
(20, 'Yurdunu Kaybeden Adam', 5, '9786254082030', 110.00, 10, 1, '2021-10-11', 0, 256, 'Esirlikten kurtulan ama hürriyetin tadına varamayan Cengiz Dağcı\'yı anlatır. "Yurdunu kaybeden adam için hürriyetin bile bir manası kalmadığını şimdi anlıyorum. İçinde doğduğum, gülüp oynadığım yerlerde benim dilim konuşulmuyor artık. Bir zamanlar, o topraklarda dilimi konuşan insanların ne olduklarını da bilmiyorum. Son fırtına, ağacı devirdi. Bizler, uçurduğu birkaç yaprak, boşlukta yolunu şaşırmış, ümitsiz ve şaşkın, meçhul bir geleceğe doğru, yalpa vurup duruyoruz.', 10, 1, null),
(21, 'Bin Dokuz Yüz Seksen Dört - 1984', 6, '9786257300117', 20.00, 10, 4, '2021-01-05', 0, 296, 'Geçmişi kontrol eden, geleceği de kontrol eder: Şimdiyi kontrol eden, geçmişi de kontrol eder. Her şey 1984 yılında geçer. Birbiriyle mütemadiyen savaşan üç büyük gücün elinde bölünmüş bir dünya, mutlak güce sahip bir Parti, kapanması yasak tele-ekranlarla her hareketi denetleyen Düşünce Polisi, her şeyi izleyen Büyük Birader ve diğer tüm düşünce biçimlerini imkânsız hâle getirmek için oluşturulan “Yenidil”. Gerçek Bakanlığı’nın altındaki Arşiv Bölümü\’nün gözlerden ırak odalarında, Parti\’nin ihtiyaçlarına göre geçmişi yeniden yazan Winston Smith’in oyununda arka plan bu kâbustur işte. Herkesi dilediği gibi kontrol eden bu totaliter dünyaya karşı içinde isyan tohumları büyüyen Winston, hakikat ve özgürlüğe duyduğu özlemin yanında aşka da kayıtsız kalamayacaktır. Yirminci yüzyılın en çok okunan ve en etkili kitaplarının başında gelen George Orwell\’in distopik başyapıtı Bin Dokuz Yüz Seksen Dört, dönemler değişse de varlığını sürdüren totaliter dünya düzenine tutulmuş bir ayna olmayı sürdürüyor.', 3, 1, null),
(22, 'Animal Farm', 6, '9786257678322', 58.00, 10, 5, '2021-01-04', 0, 98, 'In this good-natured satire upon dictatorship, George Orwell makes use of the technique perfected by Swift in The Tale of A Tub. It is the history of a revolution that went wrong- and of the excellent: excuses that were forthcoming at every step for each perversion of the original doctrine. The animals on a farm drive out their master and take over and administer the farm for themselves. The experiment is entirely successful, except for the unfortunate fact that someone has to take the deposed farmer’s place. Leadership devolves almost automatically upon the pigs, who are on a higher intellectual level than the rest of the animals. Unhappily their character is not equal to their intelligence, and out of this fact springs the main development of the story. The last chapter brings a dramatic change, which, as soon as it has happened, is seen to have been inevitable from the start.', 3, 3, null),
(23, 'Papazın Kızı', 6, '9786257300650', 20.00, 10, 4, '2021-01-28', 0, 295, 'Papazın Kızı, kendine has kusurlarına rağmen zorluklarla dolu hayatını sızlanmaksızın sürdürmeye çabalayan iyi niyetli genç bir kızın, Dorothy’nin hikâyesini anlatıyor. Her insanın yaşantısında ister istemez yer eden inanç ile inançsızlığın, çare ile çaresizliğin, takva ile günahkârlığın ötesinde, dünyaya dair algımıza ve tepkilerimize şekil veren bambaşka bir içsel güdünün bulunma ihtimalini sorguluyor. Yoksul evlerin kokusu, boş midenin sancısı, toprak yolların tozu, sonu gelmeyen borçların ve bir günahın ağırlığı; Dorothy\’nin topladığı şerbetçiotlarından damlayan çiy kadar berrak ve ürpertici. George Orwell bu romanında ufkunuzu açacak zorlu bir deneyimi acısını çekmeden, ama gerçekliğini iliklerinizde hissederek edinmenizi ustalıkla sağlıyor.', 3, 1, null),
(24, 'Boğulmamak İçin', 6, '9789750726491', 82.00, 10, 6, '2023-08-14', 0, 256, '“Orwell’in ironik mizah anlayışı tazeliğini hiç yitirmiyor. Bu, kaçırılmaması gereken bir Orwell yapıtı.” The Observer Göbeğinin çapı giderek genişleyen ve evinin taksitlerini ödemekle uğraşan George Bowling kırk beş yaşında, evli ve çocuklu –ve yeni aldığı takma dişleriyle kasvetli hayatından çaresizce kurtulmak isteyen– bir sigorta pazarlamacısıdır.1939’da patlak verecek olan savaşın gelişini; yemek kuyruklarını, askerleri, gizli polisi ve zorbalığı görerek modern zamanlardan korkmaktadır.Böylece çocukluğunun dünyasına, huzur ve sükûn dolu bir yer olarak hatırladığı köyüne sığınmaya karar verir.Fakat köyünde aradığını bulabilecek mi, orası şüphelidir. “Çok komik olmanın yanında hayranlık uyandıracak kadar gerçekçi... Bin Dokuz Yüz Seksen Dört’ü burada nüve haliyle görebiliyoruz. Hayvan Çiftliği’ni de... Hem zengin bir okuma keyfi sunan hem de iki klasiğin tohumlarını birden barındıran romanlara kolay rastlanmaz.” John Carey, The Sunday Times', 3, 1, null),
(25, 'Metafizik Üzerine Düşünceler', 7, '9786257361897', 20.00, 10, 4, '2021-04-26', 0, 87, 'On yedinci yüzyıl rasyonalizminin temellerini atan filozof ve bilim insanı Descartes, Metafizik Üzerine Düşünceler’de felsefenin başlıca sorunlarını incelerken, “Kendisinden en ufak şüphe duyulmayan ve belirsiz olduklarına hükmettiğim şeyler dışında bir şey yoksa neyi bilebilirim?” sorusuna da yanıt arıyor. Duyularıyla algıladığı ve varlığından emin olduğu her şeyi unutmaya karar verdikten sonra, mutlak hakikate ulaşmak için her şeyden şüphe edilmesi gerektiğine dair inancını temellendirmeye çalışıyor. Ön yargılardan arındırdığı zihniyle Tanrı’nın varlığı, ruh ile bedenin ayrılığı, maddelerin özü gibi konularda felsefi bir sorgulamaya girişen filozofun bu ölümsüz klasiği, modern düşüncenin de sık sık atıfta bulunduğu değerli bir kaynak olarak hâlâ felsefe tartışmalarının odağında yer alıyor.', 12, 1, null),
(26, 'Yöntem Üzerine Konuşma', 7, '9786257361880', 20.00, 10, 4, '2021-04-26', 0, 64, 'Modern felsefenin kurucularından Descartes’ın “Düşünüyorum, öyleyse varım.” önermesini ilk defa ortaya attığı Yöntem Üzerine Konuşma, filozofun düşüncelerinin otobiyografik bir özeti mahiyetindedir. Dönemin skolastik anlayışından uzaklaşmaya çalışan Descartes, şüphecilik konusunu sistematik şekilde ele alarak selefleri ve çağdaşlarından ayrılarak felsefi düşüncede hiçbir şekilde şüphe duyulmayacak bir yöntem arayışına girmiştir. Bunun için o güne dek bildiği her şeyin yanlış olduğunu farz ettikten sonra, geçici bir ahlâk düsturu belirleyip bilim ile felsefeyi bütünleştirerek hakikatin bilgisine ulaşmayı arzuladığı yolu izlerken güvenebileceklerinin neden sadece kendi benliği ve Tanrı’nın mevcudiyeti olduğunu, kendi koyduğu ilkelere göre açıklamayı amaçlamıştır.', 12, 1, null),
(27, 'Felsefenin İlkeleri', 7, '9786050208726', 80.00, 10, 7, '2020-03-11', 0, 168, '“Descartes, modern felsefenin kurucusu sayılır. Çok yüksek felsefi bir yeteneği olan, yeni fizik ve gökbilime dayanan ilk kişi odur. Birçok skolastik yanı bulunmasına rağmen Descartes, kendinden önce gelenlerin kurduğu temelleri benimsememiş, yeniden ve eksiksiz bir felsefe yapısı kurmaya çalışmıştır. Descartes\’ın yapıtlarında Platon\’dan beri hiçbir filozofta bulunmayan bir tazelik vardır. Descartes, bir öğretmen olarak değil, bir araştırmacı ve bulduğunu aktarmaya meraklı bir kişi olarak kalem kullanmıştır. Üslubu rahat ve iddiasızdır. Öğrencilere değil, zeki insanlara seslenir. Doğrusu, modern felsefe için, öncüsünün bu kadar hayranlık verici edebi bir üsluba sahip olması büyük bir talihtir.”', 14, 1, null),
(28, 'Ahlak Üzerine Mektuplar', 7, '9786050209266', 80.00, 10, 7, '2022-11-23', 0, 112, 'Descartes’ın Tanrı, evren, çeşitli tutkular, ruh, beden, madde ve ruhun bu hayattan sonraki durumu vb. konulardaki görüşlerini açıkladığı çeşitli mektuplarından oluşan Ahlak Üzerine Mektuplar, başta Prenses Elisabeth olmak üzere, Büyükelçi ve Kraliçe Christine’e yazdığı mektuplardan oluşmaktadır. Descartes’ın özellikle Prenses Elisabeth’e yazdığı mektuplar dikkat çekicidir. Mektupların birinde Descartes ruh hakkındaki görüşünü şöyle açıklar: “Insan ruhunda iki şey vardır ki doğası hakkında edinebileceğimiz bütün bilgiler onlara bağlıdır; bunlardan biri düşünmesi, diğeri de bir bedenle birleşmiş olduğuna göre, bedene etkisi ve bedenden etkilenmesidir...”', 13, 1, null),
(29, 'Sefiller (5 volumes)', 8, '9786257300438', 100.00, 10, 4, '2021-01-22', 0, 1515, 'Batı edebiyatının en büyük klasiklerinden biri olan Sefiller, yaratıcı zekâ ile yetenek düzleminde büyük bir ustalığın örneğini sunarak, karakter portrelerinin çiziminde ve tarihsel, sosyo-kültürel gerçeğin titiz anlatımında bunu derinden hissettiriyor.  Roman,  saçma bir nedenle suçlanan Jean Valjean’ı,  sokak çocuğu Gavroche’u, kötülüğün cisim bulmuş örneği Thénardierleri, düzen ve disiplinin hasta ruhlu koruyucusu yalnız adam Javert’i, dinsel bir çilenin simgesi olan sokak kadını Fantine’i ve onun kızı melek Cosette’i dramatik bir gerçeklik içinde anlatmaktadır. Okur, bu karakterlerle birlikte 19. yüzyıl başındaki Fransa’ya doğru bir yolculuğa çıkacak ve Jean Valjean’ın peşinden Paris’in arka sokaklarına giderek yoksulluğun izbe mekânları içinde bir ışık arayacaktır.', 16, 1, null),
(30, 'Le Dernier Jour D’un Condamne', 8, '9786052194058', 58.00, 10, 5, '2018-04-10', 0, 131, '« Condamné à mort ! Voilà cinq semaines que j’habite avec cette pensée, toujours seul avec elle, toujours glacé de sa présence, toujours courbé sous son poids ! Dans un cachot, un homme s\'apprête à mourir. Pour tromper son intolérable attente, le condamné écrit : son vain espoir de la grâce, son dernier voyage en fourgon, sa peur d\'affronter la foule..., mais aussi ses souvenirs de promenades autour de Paris, le sourire de sa petite fille Marie. Bientôt, le condamné sans nom et sans visage se révèle un être de chair et de sang, si proche, en somme, de chacun de nous...»', 3, 2, null),
(31, 'Les Miserables I', 8, '9786057972729', 252.00, 10, 5, '2018-12-26', 0, 560, '« Ce livre, les Misérables, n\'est pas moins votre miroir que le nôtre. Certains hommes, certaines castes, se révoltent contre ce livre, je le comprends. Les miroirs, ces diseurs de vérités, sont haïs; cela ne les empêche pas d\'être utiles. Quant à moi, j\'ai écrit pour tous, avec un profond amour pour mon pays, mais sans me préoccuper de la France plus que d\'un autre peuple. A mesure que j\'avance dans la vie je me simplifie, et je deviens de plus en plus patriote de l\'humanité. Ceci est d\'ailleurs la tendance de notre temps et la loi de rayonnement de la révolution française; les livres, pour répondre à l\'élargissement croissant de la civilisation, doivent cesser d\'être exclusivement français, italiens, allemands, espagnols, anglais, et devenir européens; je dis plus, humains. De là une nouvelle logique de l\'art, et de certaines nécessités de composition qui modifient tout, même les conditions, jadis étroites, de goût et de langue, lesquelles doivent s\'élargir comme le reste. En France, certains critiques m\'ont reproché, à ma grande joie, d\'être en dehors de ce qu\'ils appellent le goût français; je voudrais que cet éloge fût mérité. En somme, je fais ce que je peux, je souffre de la souffrance universelle, et je tâche de la soulager, je n\'ai que les chétives forces d\'un homme, et je crie à tous: aidez-moi ! » -Victor Hugo', 3, 2, null),
(32, 'Bir İdam Mahkumunun Son Günü', 8, '9786257751223', 20.00, 10, 4, '2020-11-28', 0, 128, 'Bir İdam Mahkûmunun Son Günü, hayatının beş yılını darbeyle başa gelen Louis Bonaparte’a karşı çıktığı için sürgünde geçiren Victor Hugo’nun başkaldırı güncesi olarak okunabilir. Modern edebiyatın ilk monoloğu sayılan romanda Hugo, idam cezasının trajikomik yanını da gözler önüne seriyor. İdama mahkûm bir adamın altı haftaya yayılan güncesini okurken asıl suçlunun kim olduğuna karar veremeyeceksiniz. Cinayeti işleyen katil mi, idamı bir şölen gibi izlemek için can atan toplum mu? “Giyotin en acısız ölüm şekliymiş. Oysa bedensel acı, ruhsal acının yanında hiç kalır. Belki günü geldiğinde, zavallı bir insanın bu son sözleri, payına düşeni yapacaktır.” Hugo\’nun en meşhur kitaplarından Le Dernier Jour D\’un Condamne Fransızca aslından Türkçeye çevrildi', 15, 1, null),
(33, 'Tutunamayanlar', 9, '9789754700114', 279.00, 10, 3, '2023-08-08', 0, 724, 'Tutunamayanlar, Türk edebiyatının en önemli eserlerinden biridir. Berna Moran, Oğuz Atay\'ın bu ilk romanını "hem söyledikleri hem de söyleyiş biçimiyle bir başkaldırı" olarak niteler. Moran\'a göre "Oğuz Atay\'ın mizah gücü ve duyarlığı ve kullandığı teknik incelikler, Tutunamayanlar\'ı büyük bir yeteneğin ürünü yapmış, eserdeki bu yetkinlik Türk romanını çağdaş roman anlayışıyla aynı hizaya getirmiş ve ona çok şey kazandırmıştır. "Küçük burjuva dünyasını ve değerlerini zekice alaya alan Atay, "saldırısı tutunanların anlamayacağı, rededeceği türden bir romanla yazar."', 1, 1, null),
(34, 'Günlük', 9, '9789754700350', 178.00, 10, 3, '2021-10-15', 0, 302, 'Atay\'ın edebiyatla ilgili herkes için sürekli merak konusu olmuş günlüğünün bütünü. "Kimse dinlemiyorsa beni ya da istediğim gibi dinlemiyorsa günlük tutmaktan başka çare kalmıyor. Canım insanlar Sonunda bana bunu da yaptınız sözleriyle başlayan Günlük boyunca okur, yazarın ruh halini paylaşmakla kalmıyor. Oyunlarla Yaşayanların oluşum sürecini adım adım izliyor; bir edebiyat laboratuvarındaymış gibi.', 11, 1, null),
(35, 'Bir Bilim Adamının Romanı', 9, '9789754700671', 142.00, 10, 3, '2021-06-02', 0, 283, 'Ülkemizde pek benimsenmemiş bir dalda, biyografik roman türünde, Oğuz Atay\’ın, kendine özgü üslubu ve kurgusuyla, kendi hocası da olan Mustafa İnan\’ı anlatışı. Bir halk çocuğunun uluslararası ün sahibi bilim adamı oluşunun zorlu serüveni sergilenirken toplumsal eleştiri kalıplarının da zorlanışı. İnan’ın yaşamından kesitler veren fotoğraf albümüyle birlikte. 1934\’te İnebolu\’da doğdu. Ankara Maarif Koleji\’ni, İTÜ İnşaat Fakültesi\’ni bitirdi. 1960\’ta İDMMA İnşaat -Bölümü’nde öğretim üyesi olarak çalışmaya  başladı. Tutunamayanlar\’ı yayımlamasının (1971-1972) ardından, önemli  bir tartışmanın odağına yer aldı. TRT 1970 Roman Ödülü’nü kazanan Tutunamayanlar\’ı kısa bir süre sonra, 1973 yılında Tehlikeli Oyunlar  adlı ikinci romanı izledi. Hikâyelerini Korkuyu Beklerken başlığı altında  topladı. 1911-1967 arasında yaşamış hocası Prof. Mustafa İnan\’ın hayatını romanlaştırarak Bir Bilim Adamının Romanı\’nı yazdı. Oyunlarla Yaşayanlar adlı tiyatro eseri Devlet Tiyatrolarında sahnelendi. Atay 13 Aralık 1977\’de, büyük projesi \`Türkiye’nin Ruhu\`nu yazamadan hayata gözlerini yumdu', 17, 1, null),
(36, 'Korkuyu Beklerken', 9, '9789754701586', 114.00, 10, 3, '2022-01-14', 0, 202, '"Oğuz Atay\'ın hikayeleri, gündelik hayatı kavrayış derinliği anlatım zenginliği ve okuru alıp götürmedeki enerjileri bakımından romanlarından geri kalmıyor. Kitaba adını veren hikayenin "korkuyu beklerken" kendini evine hapseden kahramanı, Atay\'ın edebiyat güzergahındaki farklılığının en büyük kanıtlarından. Yazarın bu kitaptaki iki hikayeyle var ettiği "Beyaz Mantolu Adam"da öyle...', 1, 1, null),
(37, 'Sosyoloji Notları ve Konferansları', 10, '9789754703566', 229.00, 10, 3, '2021-11-02', 0, 411, 'Yazarın değil konuşan Cemil Meriç Sosyoloji Notları ve Konferanslar, Cemil Meriç\'in İstanbul Üniversitei Edebiyat Fakültesi Sosyoloji Bölümü\'nde 1965\'ten 1969\'a kadar anlattığı dersleri, verdiği birkaç konferansın metnini ve bazı sohbetlerinden alınan notları içeriyor. Donmuş bir müfredatı anlatan bir "hoca" değil, öğrencileriyle ve dinleyenleriyle birlikte sesli düşünen bir fikir adamı, Cemil Meriç. Bu sesli düşünmeler; Cemil Meriç\'in daha sonraki yıllarda yazdığı kitapların malzemesini, taslaklarını oluşturoyor. "Yazar"ın ve "hoca"nın düşüncesini olgunlaştırmasının izini sürmeyi sağlayan metinler okuyacaksınız. Sesli düşünmenin belki disiplinsiz, dağınık, bazen spekülatif, ama yaratıcı ve kimi zaman da yazılı olandan daha canlı evreni...', 8, 1, null),
(38, 'Kültürden İrfana', 10, '9789750511899', 270.00, 10, 3, '2017-04-21', 0, 493, 'Kültürden İrfana ile on iki ciltlik Cemil Meriç külliyatı tamamlanıyor. Mefhumlar ve meseleler konusunda düşüncenin en ücra köşelerini yoklayan, yalınkat bir bilgi yerine kapsamlı, incelikli bir bilginin peşine düşen Cemil Meriç, Kültürden İrfana’da okurunu önyargıların köleliği yerine düşüncenin yoldaşlığına çağırıyor. “Kültür, Batı’nın düşünce sefaletini belgeleyen kelimelerden biri:', 18, 1, null),
(39, 'Bu Ülke', '10', '9789754702811', '219.00', '10', '3', '2021-10-04', '0', '339', 'Meriç’in “aynı kaynaktan fışkırdılar” dediği eserler dizisinin önemli bir halkası. Bir çağın, bir ülkenin vicdanı olmak isteği Meriç’in bütün çabasına her zaman yön vermiştir: “Bu sayfalarda hayatımın bütünü, yani bütün sevgilerim, bütün kinlerim, bütün tecrübelerim var. Bana öyle geliyor ki, hayat denen mülakata bu kitabı yazmak için geldim; etimin eti, kemiğimin kemiği.” Bu Ülke, Meriç’in sürekli etrafında dolandığı Doğu-Batı sorunu yanında, sol-sağ kutuplaşmasına ve kalıplaşmasına ilişkin önemli tesbit ve aforizmalarını da içeriyor.', '18', '1', null),
(40, 'Mağaradakiler', '10', '9789754705997', '183.00', '10', '3', '2022-01-14', '0', '287', 'Aydın mı dersiniz, entelektüel mi dersiniz? İki kavrama farklı anlamlar mı yüklersiniz? Aydınlardan entellektüellerden çok şeyler mi beklersiniz, hiçbir şey beklemez misiniz? Öyle ya da böyle, kültürle derinlemesine alışveriş kaygınız arsa zaman eksenine düşünce mesaisi düşünebiliyorsanız bu kavramlar üzerine kafa yorarsınız bu sorulara cevap ararsınız ufuk ararsınız. Cemil Meriç\'in hakikatte içi de, dışı da bir mağarayı anlattığı kitap Mağaradakiler bir geniş ufuk kitabı.', '18', '1', null),
(41, 'Yukarı Mahalle', 11, '9789755709079', 90.00, 10, 8, '2018-01-29', 0, 192, 'Birinci Dünya Savaşı ve Büyük Buhran yıllarının boğucu atmosferinde yerleşik kalıpların dışına taşanların, gelecek kaygısı taşımayan ama bugünü de sonuna kadar yaşayanların, sistemin dışında kalmakta direnenlerin, beş parasız aylak takımının hikayesi Yukarı Mahalle. Sıra dışı ilişkileri, tuhaf alışkanlıkları, durduk yere çıkan kavgaları, renkli karakterleri ve hatta köpekleriyle dostluğun, dayanışmanın, fedakârlığın ama illa ki neşenin kol gezdiği bu sokaklarda yoksulluk bir üzüntü, işsizlik bir yoksunluk olmaktan çıkıyor. Küçük insanların hikayelerinden dev yapıtlar yaratan dünya edebiyatının usta kalemi John Steinbeck\’in Tatlı Perşembe ve Sardalye Sokağı\’yla oluşturduğu üçleme Yukarı Mahalle\’yle tamamlanıyor.', 3, 1, NULL),
(42, 'Al Midilli', 11, '9789750531392', 91.00, 10, 3, '2023-08-17', 0, 97, 'Steinbeck’in doğaya ve insana on yaşındaki bir çocuğun gözünden baktığı Al Midilli kendi edebi kariyerinde olduğu kadar Amerikan edebiyatında da bir dönüm noktası. Salinas Vadisi’ndeki bir çiftlikte anne-babası ve yardımcıları Billy Buck’la yaşayan Jody’nin tekdüze hayatı babasının hediye ettiği al bir midilliyle renklenir. Jody’nin henüz tay olan midilliye binebilmesi için hem tayın büyümesini beklemesi hem de onu eğitmesi gerekir. İnsan doğasının zayıflıklarını ve karmaşıklığını resmetme ustası Steinbeck, kendi çocukluk anılarından esinlenerek kaleme aldığı Al Midilli’de ergenliğin ıstıraplarını gözler önüne seriyor. “Al Midilli, Steinbeck’in kendi kişisel ve sıradan deneyimlerini ‘sanatın simyası’ aracılığıyla evrensel masallara dönüştürme konusundaki becerisini gösteriyor.” JOHN TIMMERMAN “Bir başyapıt … Çocukluğun yürek burkacak kadar gerçek bir tablosu.” CLIFTON FADIMAN', 3, 1, NULL),
(43, 'Fareler ve İnsanlar', 11, '9789755705859', 95.00, 10, 8, '2023-01-26', 0, 111, 'Fareler ve İnsanlar, birbirine zıt karakterdeki iki mevsimlik tarım işçisinin, zeki George Milton ve onun güçlü kuvvetli ama akli dengesi bozuk yoldaşı Lennie Small’un öyküsünü anlatır. Küçük bir toprak satın alıp insanca bir hayat yaşamanın hayalini kuran bu ikilinin öyküsünde dostluk ve dayanışma duygusu önemli bir yer tutar. Steinbeck insanın insanla ilişkisini anlatmakla kalmaz insanın doğayla ve toplumla kurduğu ilişkileri de konu eder bu destansı romanında. Kitabın ismine ilham veren Robert Burns şiirindeki gibi; “En iyi planları farelerin ve insanların / Sıkça ters gider…”', 19, 1, NULL),
(44, 'Cennetin Doğusu', 11, '9789750531200', 290.00, 10, 3, '2021-06-16', 0, 644, 'Cennetin Doğusu, 20. yüzyıl Amerikan edebiyatının en önemli temsilcilerinden Steinbeck’in iyilikle kötülüğün ezeli mücadelesini işlediği başyapıtı. Steinbeck, Amerikan İç Savaşı’ndan Birinci Dünya Savaşı’nın sonuna kadar uzanan hikâyede Kuzey Kaliforniya’daki Salinas Vadisi’nde kaderleri kesişen Hamilton ve Trask ailelerinin nesiller boyu izlerini sürerek hem Amerika’nın hem de insanlığın tarihini anlatıyor. Kendi ailesinden de izler taşıyan bu eserde iyiyle kötü, güçle zayıflık, aşkla nefret, güzellikle çirkinlik temaları üzerinden en kadim hikâyelerden Habil’le Kabil’i yeniden yorumluyor. Cennetin Doğusu, Nobel Edebiyat Ödüllü John Steinbeck’in “magnum opus”u. “Hep bu kitabı yazmak istedim, bu kitabı yazabilmek için çalıştım, bu kitabı yazabilmek için dua ettim.” JOHN STEINBECK “Vahşi doğanın gücüne sahip dokunaklı ve göz yaşartıcı bir gösteri.” CARL SANDBURG', 3, 1, NULL),
(45, 'Yaban', 12, '9789754700060', 125.00, 10, 3, '2019-10-04', 0, 215, 'Millî Mücadele sırasında Orta Anadolu’da bir köy. Tanzimat aydınının sosyo-psikolojik özelliklerinin uzantılarını taşıyan Ahmet Celal. Kendini kurtarıcı olarak gören, halkı eğitmeyi (ya da adam etmeyi) görev edinmiş, kafasındaki gerçekle yaşanan gerçeğin çatışması sonucu “yaban“laşan tipik aydın. Kendi dönemi içindeki gerçekçilik anlayışına uygun olarak yazılmış olan Yaban\'da Yakup Kadri, 1. Dünya Savaşı\'nın bitimiyle birlikte Sakarya Savaşı\'nın sonuna kadar olan sürede bir Anadolu köyünde, köylüleri, köyün durumunu, Milli Mücadeleye ilişkin tavırlarını bir aydının gözüyle verir. Yaban için "bu eser benliğimin çok derinliklerinden adeta kendi kendine sökülüp, koparak gelmiş bir şeydir" diyen yazar, bu romanda ortaya koyduğu birçok soruna daha sonra yazacağı Ankara\'da cevap bulmaya çalışacaktır. Yirminci yüzyılın ilk yarısında büyük bir üretkenlikle dergilere yazdığı şiir, öykü, makale ve eleştri türü yazılarla Türk edebiyatı sahnesine adımını atan Yakup Kadri Karaosmanoğlu, romanları, hikayeleri, denemeleri, oyunları ve anılarıyla, en önemli edeiyatçılarımız arasında yer alır. Üslup özellikleri bakımından Yakup Kadri\´nin 1910\´dan 1974\´e dek verdiği eserler Türkçe´nin geçirdiği bütün evreleri yansıtır. Eserlerinin konu ve fikir zenginliği de dil özelliklerinin çeşitliliğinden aşağı kalmaz. Yakup Kadri´nin Fransız edebiyatı etkisinde başlayan yazarlığı, 1920\´lerden sonra özgün bir sese kavuşarak siyasi ve sosyolojik konulara, tarihe, dönem çatışmalarına ve birey psikolojisi irdelemelerine yönelir. Fecr-i Ati´den yetişmiş ama bunu izleyen elli yıl boyunca toplumsal koşullar, tarihi süreçler ve bireysel portreleri romanın dokusuna işlemek için roman tekniğiyle de boğuşmuş bir yazar olan Karaosmanoğlu\´nun eserleri, hala tüketilmemiş ayrıntılarının tartışılıp incelenmesi gereken zengin bir "panoroma"dır.', 1, 1, NULL),
(46, 'Milli Savaş Hikayeleri', 12, '9789754700725', 130.00, 10, 3, '2021-02-08', 0, 188, 'Her biri başka bir yeri, başka kişileri, başka olayları konu edinen, bir yandan da sonu gelmez ve umutsuz bir arayışı dile getiren hikayeler. Güzel ve büyük yurdunu yitiren Hamdi, kocasını aramak için İstanbul\'a gelen Ödemişli zavallı bir kadın her şeyi allak bullak eden, "yurt"u "gurbet"e çeviren savaş ve geride kalanların hayatları.', 1, 1, NULL),
(47, 'Gençlik ve Edebiyat Hatıraları', 12, '9789754700565', 168.00, 10, 3, '2015-11-01', 0, 284, 'Yakup Kadri\'nin anı kitaplarının sonuncusu. Yazar edebiyatla, edebiyatçılarla yakın ilişkiler kurduğu 1908 ve sonrasına, gençlik yıllarına döner ve bu kez anılarını anlatırken, öbür anı kitaplarından farklı bir yol izler. Doğrudan kimi edebiyatçıları konu ederek onlara ilişkin anılarını aktarır, sözünü ettiği kişinin bir sözünü veya bir davranışını yorumlar. Kimi zaman da yargılamaktan kaçınmaz. Gençlik anılarını yazarken de hem yaşadığı yıllardan hem de anılarının döneminden bakar olaylara.', 17, 1, NULL),
(48, 'Ergenekon Milli Mücadele Yazıları', 12, '9789750507472', 165.00, 10, 3, '2010-03-11', 0, 272, 'Ergenekon adı altında topladığım yazılar(...) Millî Mücadele sıralarında, İkdam gazetesinde çıkmış makalelerden bir küçük kısımdır. (...) Günü gününe, üç yıllık bir devreyi dolduran yüzlerce makale içinden yalnız elli-altmış tanesini ayırıp bir kitap halinde yayımladım. (...) 1920 yılından 1923’e kadar Türk milletinin geçirdiği vicdan ve fikir buhranlarını, ancak bu gibi yazılarla hatırlamak kabildir. İşte Ergenekon’u teşkil eden parçaların yegâne kıymeti bu fikir ve vicdan krizlerini öbürlerinden daha bariz bir surette göstermesinden ibarettir. Bu itibar ile bu yazıları, o devrin mânevî manzaralarını, ateşten bir çizgi halinde aksettiren birer küçük aynaya benzetebiliriz.', 20, 1, NULL),
(49, 'İnsan İnsana', 13, '9786257631525', 140.00, 10, 9, '2021-11-18', 0, 312, 'BİR İNSANIN İLİŞKİLERİNİN NİTELİĞİ,O İNSANIN YAŞAMININ KALİTESİNİ BELİRLER. İnsan, ilişkileri içinde sürekli olarak “yeniden tanımlanan” bir varlıktır. İnsan ilişkilerinin temelini ise iletişim süreçleri oluşturur. İki insan birbirinin farkına vardığı anda iletişim başlar. Aynı sosyal ortam içinde yer alan kişilerin söyledikleri sözler ve hareketleri kadar, hareketsizlikleri, susmaları, beden duruşları ve yüz ifadeleri, hepsi anlamlı birer mesaj oluşturur. İyi bir dinleyici, iletişim kurduğu kişinin yalnız söylediklerini değil, yüzü, eli, kolu ve bedeniyle yaptıklarını da “duyar.” Bir aracın sürücüsü, yolda kendinden başka araç yokmuş gibi davranırsa, trafik kazası olur. Bir kişi konuşurken, karşısındakini nasıl etkilediğini düşünmeden, kendi bildiği yönde istediğini söylerse “iletişim kazası” ortaya çıkar. İlişkilerimizde, verdiğimiz mesajların sorumluluğunun bilincinde olmamız, iletişim kazalarını önler. Bu varsayım toplumsal düzeyde de geçerlidir. Kişi farkında olsun ya da olmasın, toplumla da sürekli ilişki içindedir. Bir toplumda “Herkes benim gibi düşünmelidir, benim düşünce tarzım en doğrusudur,” tutumu ağır basarsa, akılcı tartışmalar yerine duygusal çatışmalar ortaya çıkar. İnsan hayatını mercek altına alıp, insana dair her hikâyeden bir anlam çıkarabilen bilgeliğiyle değerli Doğan Cüceloğlu, kimliklerin ötesinde, canların temas içinde olduğu “insan insana” bir ilişkinin mümkün olduğunu bize hatırlatıyor. Kalıpları tekrarlamaktan kurtulabilmeniz, insan ilişkilerine anlamsal zenginliği ve derinliği getirebilmeniz için iletişim süreçlerini uygun ve etkili bir biçimde uygulamanıza yönelik bilgi ve becerileri sunuyor. İletişim sorunlarını çözmeden doyumlu bir yaşam sürdürmenin olanaksız olduğunun ve insanın isterse kendini değiştirip geliştirebileceğinin altını çiziyor.', 21, 1, NULL),
(50, 'Gerçek Özgürlük', 13, '9786257631594', 145.00, 10, 9, '2021-12-10', 0, 344, 'YAŞAMINDA KENDİSİ OLARAK VAR OLAMAMIŞ BİRİ DUYGU, DÜŞÜNCE VE DAVRANIŞLARIYLA YAŞAMI ÖZGÜRCE KUCAKLAYABİLİR Mİ? “Dünya bazen kapkaranlık gözükür, insan kendini yapayalnız ve değersiz görür, bu duygular da yaşamın bir parçası. Bence sizin, sizi anlayacak biriyle konuşmaya ihtiyacınız var.” Bu kitap, gençlik yıllarımı temsil eden üniversite öğrencisi Timur ile yaşlılık yıllarımı temsil eden emekli psikoloji profesörü Yakup Bey arasında geçen sohbetlerden oluşuyor. Sevdiği kızın kendisini önemsemediğini fark etmeyen Timur ona evlilik teklif eder. Sosyoekonomik düzeyi yüksek Nesrin kibarca, “Sen ben denk değiliz,” mesajını verir. Tesadüfen Timur\’la karşılaşan Yakup Bey gencin yüzünden hüznünü ve yalnızlığını anlar ve ona isterse Sahaflar Çarşısı\’ndaki kitapçı dükkânına gelebileceğini söyler. Buluşmaya ve sohbet etmeye başlarlar. Bu sohbet içinde Timur kendi anlam verme sistemini, değerler sistemini, ezikliğinin kaynağını, toplumla, yaşamla ilişkisinin temellerini keşfetmeye başlayacaktır. O karşılaşmadan sonra Yakup Bey\’le yaptığı sohbetlerde, kültür robotluğundan şahsiyet olmaya giden bir özgürlük yolculuğuna çıkacaktır. Bu kitapta, karşılıklı saygı içinde olan iki insanın; yaşamını, ilişkilerini, kendi anlam verme sistemini keşfedişi yer almaktadır. Sevgi mi özgürlüğe, özgürlük mü sevgiye götürür? İç yalnızlığı gözlerinden okunurken sevdiğine evlenme teklif eden, gerçek sevgiyi, gerçek aşkı sorgulayan üniversiteli bir gencin biz bilinci içinde adım adım Gerçek Özgürlük\’e varışının öyküsü. Kendi yolculuğumuzu yapmak için buradayız; bu yolculukta kendimiz olabilme cesaretini bulmamız kolay değildir ama kendimiz olmadan yaşamımızdaki hiçbir şey anlamını bulamaz.', 21, 1, NULL),
(51, 'Başarıya Götüren Aile', 13, '9789751411075', 95.00, 10, 10, '2019-10-30', 0, 143, 'Sınav Döneminde Anababalık Bu kitap, çocuğunun başarılı olması için, "Çok çalış oğlum/kızım," demenin ya da tüm maddi olanaklarını seferber etmenin ötesinde bir şeyler yapmak isteyen anababalara yol göstermek amacıyla yazıldı. Her anababa, okul başarısı için çocuğuna yardımcı olmak ister. Ama öğrenme sürecinin bilimsel temellerini kavramadan atılacak her adım, iyi niyetli de olsa, çocuğu engelleyebilir. Başarıya Götüren Aile, sınav döneminde çocuklarına destek olmak için doğru ve etkili yöntemler arayan tüm anababalara kılavuzluk edecek.', 21, 1, NULL),
(52, 'İçimizdeki Çocuk', 13, '9789751403643', 162.00, 10, 10, '2019-06-25', 0, 256, 'Bu kitap, içinde yetiştiğiniz ailenin ve yakın çevrenin sizin iç dünyanızı ve şimdiki duygu, düşünüş ve davranışınızı nasıl etkilediğini incelemektedir.', 21, 1, NULL),
(53, 'Milli Mücadele Tarihi', 14, '9786258431896', 125.00, 10, 9, '2022-10-18', 0, 208, 'Tarih alanında dünyanın tartışmasız en büyük isimlerinden biri olan Halil İnalcık\’ın kaleminden Millî Mücadele Tarihi. İnkılâp tarihini bir bütün olarak kavrayabilmek için 1908 yılındaki II. Meşrutiyet\’in ilanından başlanması gerektiğini söyleyen İnalcık, ilk olarak 1908-1918 arasındaki belli başlı gelişmeleri kuşbakışı bir perspektifle ele alıyor. Bu çerçevede imparatorluğu kurtarmak için Osmanlılık ve Türkçülük akımlarının gelişimini, İttihat ve Terakki\’nin iktidar sürecini, I. Dünya Savaşı\’ndaki gelişmeleri ve savaş bitiminde memleketin işgaline giden aşamaları irdeliyor. İzmir\’in işgali ve sonrasında Mustafa Kemal\’in Anadolu\’ya geçişi ile başlayan süreç Türk Kurtuluş Savaşı\’nın odağını oluşturmaktadır. Bu çerçevede İnalcık, millî iradenin hâkim olması için Erzurum ve Sivas kongreleri ile başlayan mücadelenin 23 Nisan 1920\’de Ankara’da Türkiye Büyük Millet Meclisi\’nin açılışı ile ivme kazandığını ve sonrasında Doğu, Güneydoğu ve Batı Anadolu’da Mustafa Kemal liderliğindeki Türk ordusunun elde ettiği başarılar ile saltanatın kaldırılması ile neticelendiğini ortaya koyuyor. İnönü Muharebeleri, Sakarya Muharebesi ve Büyük Taarruz\’dan başarılı bir netice elde eden Ankara Hükümeti\’nin 24 Temmuz 1923\’te imzalanan Lozan Antlaşması ile siyasi olarak mevcudiyetini dünya kamuoyuna tescil ettirdiğini gösteriyor. Millî Mücadele Tarihi, Halil İnalcık\’ın kaleminden 1908-1923 yılları arasında millî iradenin hâkim kılınmasının aşamalarını ve Mustafa Kemal Atatürk\’ün bu aşamalardaki etkin liderliğinin detaylı anlatımı.', 22, 1, NULL),
(54, 'İmparatorluktan Cumhuriyete', 14, '9789752430860', 125.00, 10, 9, '2020-03-05', 0, 240, 'Tarih alanında dünyanın tartışmasız en büyük isimlerinden biri olan Halil İnalcık\’ın, Osmanlı sosyal tarihi ve modern Türkiye\’nin ortaya çıkışıyla ilgili çalışmaları bir arada. Kitabın ilk bölümü, Osmanlı İmparatorluğu\’nun siyasi ve toplumsal sisteminin temelini oluşturan toprak meselesi, çift-hane uygulaması ve tahrir meselesini irdeliyor. Sonrasında Osmanlı tebaası gayrimüslim milletlere dair arşiv vesikalarını, Rum Ortodoks Kilisesi\’nin yetki alanını, Osmanlıların Sefarad Yahudilerine iskân hakkı vermesinin özel koşullarını, modern Avrupa\’nın gelişmesinde Türk etkisini ve sultanın siyaset alanındaki diğer güç odaklarıyla iktidar mücadelesinin dönüşümünü ele alıyor. Kitabın ikinci bölümü İmparatorluktan Cumhuriyete geçiş sürecine ışık tutuyor. Özellikle Avrupa ile Ortadoğu arasındaki Türkiye\’nin stratejik konumu ve 1924\’de Halifeliğin kaldırılması ve Atatürk inkılapları arasındaki ilişkiye dair incelemeleri, İnalcık\’ın modern Türkiye Cumhuriyeti tarihi araştırmalarında da ne denli önemli bir yer teşkil ettiğini gösteriyor. İmparatorluktan Cumhuriyete, hem meslekten tarihçiler hem de tarih meraklıları için bir başucu kaynağı.', 22, 1, NULL),
(55, 'Osmanlı’da Devlet, Hukuk ve Adalet', 14, '9786058301122', 125.00, 10, 9, '2020-02-18', 0, 288, '“Milletleri millet yapan tarihleri ve kültürleridir. Tarihsiz bir millet, kişiliğini kaybetmiş bireye benzer. Bu kitabı okuyanlar umuyoruz ki, Osmanlı Devlet-i ‘Aliyye’sinin (İmparatorluğun) birçok millet ve dini, altı yüz yıl nasıl bir arada tuttuğunu ve nasıl idare ettiğini öğrenmiş olacaklar.” Halil İnalcık. Altı asır boyunca egemenliğini devlet, hukuk, adalet anlayışıyla sağlayan Osmanlılar, iktidarlarını ise kanun ile ahlak dengesiyle ayakta tutmuşlardır. Batı kaynaklarında Osmanlı halkından herhangi bir kimsenin hükümdarı bile dava edebileceğinden övgüyle bahsedilmiştir. Öte yandan bürokratlar ise hükümdarın asli prensipleri ezip geçmesi karşısında onu tahtından edebilmişlerdir. Hem Osmanlıları "Devlet-i Aliyye-i Osmâniyye" yapan hem de "Devlet-i Ebed-müddet" sözünü slogan olmaktan kalıcı bir mekanizma haline getiren düşünce, din ve devletin selameti adına devlet-hukuk-adalet güçlerinin bir direnç unsuru olarak daima bir arada yaşamış ve yaşatılmış olmasıdır. Tarih yazıcılığında çığır açmış olan Halil İnalcık, Osmanlı\’da Devlet, Hukuk ve Adâlet kitabında devlet anlayışı, kanun rejimi, kanunların uygulanış biçimi ve adalet yöntemleri üzerine araştırmalarını bir araya getiriyor. Okuyucular kitabı bitirdiklerinde, Osmanlı Devleti’nin birçok millet ve dini altı asır nasıl bir arada tutup idare ettiğini en orijinal bilgiler eşliğinde öğrenmiş olacaklar.', 2, 1, NULL),
(56, 'Osmanlılar Fütuhat, İmparatorluk, Avrupa İle İlişkiler', 14, '9786254292828', 130.00, 10, 11, '2022-12-08', 0, 320, '“Osmanlı Devleti’nin doğuşu sorunu ile Osmanlı İmparatorluğu’nun kuruluşu sorunu birbirine karıştırılmamalıdır. Osmanlı İmparatorluğu’nun tarihi, Osmanlı Devleti’nin Anadolu ve Balkanlar’da nasıl ve hangi koşullar altında siyasi bir güç durumuna geldiğini inceler. Osmanlı Devleti’nin doğuşu sorunu ise Selçuklu-Bizans uc bölgesinde Osman Gazi’nin liderliği altında siyasi çekirdeğin nasıl ortaya çıktığını açıklamakla ilgilidir.” Çalışmalarıyla Osmanlı İmparatorluğu\’nun tarih yazımına siyaset, ekonomi, kültür ve medeniyet gibi pek çok açıdan özgün katkılar veren Halil İnalcık\’ın Osmanlılar/Fütuhat, İmparatorluk, Avrupa ile İlişkiler eseri, arşiv belgeleri ışığında imparatorluğun gelişimini, fetih yöntemlerini, devlet sistemini ve Hıristiyan Avrupa ile ilişkilerini incelediği makalelerinden oluşuyor. İnalcık\’ın Osmanlıların kuruluş süreci, klasik dönem Osmanlı tarihinde egemenlik kavramı, Osmanlı İmparatorluğu\’nda köle emeği, Avrupa\’ya verilen ticaret imtiyazları gibi pek çok başlıkta kaleme aldığı Osmanlı\’nın kuruluşu ve Avrupa ile ilişkileri konularındaki değerlendirmeleri bu eser aracılığıyla bir kez daha okurlarıyla buluşuyor.', 22, 1, NULL),
(57, 'İyiler Ölmez', 15, '9789759957667', 105.00, 10, 12, '2016-10-10', 0, 152, '“Kapı açıldı, biri içeri girdi. Onunla beraber yağmurun kokusu, fırtınanın ayazı… Kahveci Hacı Kadir uzun süpürgenin sapına dayanarak gelene baktı. Biraz ürperdi ama renk vermedi. Ne de olsa gecenin bir vakti. Saç baş birbirine karışmış, sırt çantası taşıyan bir garip adam. Üstelik sakallı. O yıllarda memlekette sırt çantası yoktu. Demek bu adam yaban ya da turist… Orada öylece gözlerini kısmış duruyor, dimdik Hacı\’ya bakıyor.”', 1, 1, NULL),
(58, 'Yoksulluk İçimizde', 15, '9789759953126', 85.00, 10, 12, '2016-01-01', 0, 104, 'Bedeni ve maddi hazlara bağlı bir mutluluk düşüncesini besleyip büyütüyoruz. Dünya muhabbetini sayısız teferruat ile zenginleştiriyoruz. Nefsin ihtirasları bizi her an değişik parıltılar yayan eşyaya doğru koşturuyor. Bu vahşi koşu modern dünyanın simgesidir. Bu kitap kalbi olanı, aşkı ve öteleri dile getirerek hayatın hakikatına işaret ediyor. İçimizdeki yoksulluğu farketmek için belki bir fırsattır bu.', 1, 1, NULL),
(59, 'Uzun Hikaye', 15, '9789759953331', 85.00, 10, 12, '2012-10-01', 0, 120, 'Ben o zamanlar on altı yaşındaydım, lise birde. İnce uzun bir oğlan. Saçlarım kirpi gibi dik duruyor; ne yana, ne geriye taranmıyor, beni deli ediyordu. Babam "inatsın inat... İnatçı adamın saçı yatmaz. Dedeme çekmişsin besbelli. Keşke annene benzeseydin" diyordu. Keşke... Annemin lepiska gibi yumuşacık, sarı saçları vardı. En çok o mavi gözlerini özlüyorum. "Benim oğlum okuyacak yüksek bir memur olacak" der, sonra da göz ucuyla babama bakardı.... Devamı kitapta.', 1, 1, NULL),
(60, 'Sır', 15, '9789759953003', 85.00, 10, 12, '2012-05-01', 0, 96, 'Tarihin çöp sepeti, Politik-vizyon, Her ne var alemde, Aramakla bulunmaz, Mürit, Satılık huzur, Cüz gülü', 1, 1, NULL),
(61, 'Meditations', 16, '9786257850216', 89.00, 10, 5, '2020-07-13', 0, 149, 'One of the world''s most famous and influential books, Meditations, by the Roman emperor Marcus Aurelius incorporates the stoic precepts he used to cope with his life as a warrior and administrator of an empire. Ascending to the imperial throne in A.D. 161, Aurelius found his reign beset by natural disasters and war. In the wake of these challenges, he set down a series of private reflections', 13, 3, null),
(62, 'Metamorphosis', 17, '9786059681230', 37.00, 10, 5, '2016-05-09', 0, 63, 'The Metamorphosis is a short novel by Franz Kafka, first published in 1915. It is often cited as one of the seminal works of fiction of the 20th century and universites across the Western world. The story begins with a traveling salesman, Gregor Samsa, waking to find himself transformed into an insect.', 15, 3, null),
(63, 'A Room of One''s Own', 19, '9786052194188', 66.00, 10, 5, '2018-04-28', 0, 66, 'A Room of One''s Own is an extraordinary, beautifully written, poetic little book. It''s based on two lectures on women and fiction that Woolf gave in Cambridge in 1928, and it''s quite unlike the other great feminist polemics - or in fact anything else at all. Woolf imagines for us, in a novelistic stream of consciousness, two days in which she wanders around "Oxbridge" and the British Museum, and browses through everything ever written about or by women. Why was there no female Shakespeare, she ponders? She imagines what life would have been like for a brilliant sister of Shakespeare - and finds the woman killing herself in her prime. -The Guardian', 3, 3, null),
(64, 'What Men Live By', 18, '9786059681728', 30.00, 10, 5, '2016-09-30', 0, 46, '“What Men Live By” is a short story written by Russian author Leo Tolstoy in 1885. It is one of the short stories included in his collection What Men Live By, and Other Tales, published in 1885.', 3, 3, null);

INSERT INTO credit_card (id, user_id, cvc, number, credit_card_name, title, expiration_date) 
VALUES
(1, '2', '356', '5214873652145632', 'elif balcı', 'Premium Card', '2024-03-01 00:00:00'),
(2, '4', '412', '6345872139563214', 'zeynep demir', 'Traveler''s Card', '2025-05-15 00:00:00'),
(3, '5', '789', '4821376592143659', 'fatma aydın', 'Business Card', '2023-12-31 00:00:00'),
(4, '7', '263', '5896321478541236', 'ayşe erbil', 'Shopping Card', '2024-07-20 00:00:00'),
(5, '8', '974', '3652147896321478', 'emre orhan', 'Reward Card', '2026-01-10 00:00:00'),
(6, '10', '658', '2147859632587412', 'sibel taş', 'Student Card', '2025-09-30 00:00:00'),
(7, '11', '325', '7854123698745632', 'leyla gül', 'Gold Card', '2023-11-05 00:00:00'),
(8, '13', '841', '9632587412365874', 'nuray demirci', 'Platinum Card', '2026-04-17 00:00:00'),
(9, '14', '569', '1478523696541236', 'burak çağlar', 'Silver Card', '2024-06-25 00:00:00'),
(10, '15', '478', '3214569876547896', 'selin can', 'Classic Card', '2025-08-09 00:00:00');

INSERT INTO customer (id, wallet) 
VALUES
(2, 35),
(4, 42),
(5, 78),
(7, 52),
(8, 13),
(10, 4),
(11, 26),
(13, 11),
(14, 100),
(15, 85);

INSERT INTO `Comment` (`user_id`, `book_id`, `comment`) 
VALUES
(2, 1, 'Great book! Really enjoyed the plot.'),
(4, 2, 'The characters are so well-developed.'),
(5, 3, 'Couldn''t put it down!'),
(7, 4, 'Amazing storytelling.'),
(8, 5, 'A must-read for any book lover.'),
(10, 6, 'Loved the twists and turns.'),
(11, 7, 'Couldn''t stop reading.'),
(13, 8, 'Incredible journey in this book.'),
(14, 9, 'Beautifully written, highly recommended.'),
(15, 10, 'Captivating from start to finish.'),
(2, 11, 'Well-written and engaging.'),
(4, 12, 'The author has a unique style.'),
(5, 13, 'Impressive story, kept me hooked.'),
(7, 14, 'Fascinating characters and setting.'),
(8, 15, 'Enjoyed every chapter.'),
(10, 16, 'Couldn''t get enough of it.'),
(11, 17, 'Brilliant plot and execution.'),
(13, 18, 'Highly thought-provoking.'),
(14, 19, 'An emotional rollercoaster.'),
(15, 20, 'Well-crafted and thoughtfully written.'),
(2, 21, 'Riveting story, kept me guessing.'),
(4, 22, 'Couldn''t predict the twists.'),
(5, 23, 'Compelling characters and dialogue.'),
(7, 24, 'Page-turner with a satisfying ending.'),
(8, 25, 'Well-researched and informative.'),
(10, 26, 'Thoroughly enjoyed every page.'),
(11, 27, 'Couldn''t recommend it more.'),
(13, 28, 'Masterfully crafted narrative.'),
(14, 29, 'Epic adventure, worth the read.'),
(15, 30, 'Engaging from the very first chapter.'),
(2, 31, 'Thought-provoking and profound.'),
(4, 32, 'A literary gem, beautifully written.'),
(5, 33, 'Captivating plot and character development.'),
(7, 34, 'An instant classic in my opinion.'),
(8, 35, 'Immersive world-building.'),
(10, 36, 'Kept me up late, couldn''t stop reading.'),
(11, 37, 'Impressive storytelling skills.'),
(13, 38, 'The author has a unique voice.'),
(14, 39, 'Emotionally resonant and powerful.'),
(15, 40, 'A must-read for any avid reader.'),
(2, 41, 'Fascinating read, highly recommended.'),
(4, 42, 'Unique perspective, couldn''t put it down.'),
(5, 43, 'Kept me on the edge of my seat.'),
(7, 44, 'A literary delight, beautifully written.'),
(8, 45, 'Compelling and thought-provoking.'),
(10, 46, 'An absolute page-turner.'),
(11, 47, 'Hooked from the first chapter.'),
(13, 48, 'Intriguing plot twists and turns.'),
(14, 49, 'Characters that stay with you long after.'),
(15, 50, 'A literary journey worth taking.'),
(2, 51, 'Engrossing narrative, well-paced.'),
(4, 52, 'Richly atmospheric and immersive.'),
(5, 53, 'A triumph of storytelling.'),
(7, 54, 'Compelling exploration of human nature.'),
(8, 55, 'Unforgettable characters and setting.'),
(10, 56, 'A gem in contemporary literature.'),
(11, 57, 'Beautifully explores the human condition.'),
(13, 58, 'A book that stays with you.'),
(14, 59, 'Eloquent prose and vivid descriptions.'),
(15, 60, 'A literary masterpiece, simply brilliant.'),
(8, 1, 'A compelling blend of mystery and drama.'),
(10, 2, 'Characters that come to life on the page.'),
(11, 3, 'An emotional rollercoaster of a story.'),
(13, 4, 'A literary gem, beautifully written.'),
(14, 5, 'Compelling narrative and well-drawn characters.'),
(15, 6, 'Captivating from start to finish.'),
(2, 7, 'A thought-provoking exploration of humanity.'),
(4, 8, 'Richly detailed and immersive world-building.'),
(5, 9, 'Couldn''t turn the pages fast enough.'),
(7, 10, 'A must-read for lovers of great literature.'),
(8, 11, 'Well-crafted and beautifully executed.'),
(10, 12, 'Characters you can''t help but root for.'),
(11, 13, 'An absolute page-turner.'),
(13, 14, 'Evocative and beautifully written.'),
(14, 15, 'A literary triumph in every sense.'),
(15, 16, 'An exploration of the human spirit.'),
(2, 17, 'An immersive literary journey.'),
(4, 18, 'Kept me on the edge of my seat.'),
(5, 19, 'Thought-provoking and captivating.'),
(7, 20, 'A compelling narrative that resonates.'),
(8, 21, 'Skillful storytelling at its best.'),
(10, 22, 'A captivating tale of adventure.'),
(11, 23, 'Characters that feel like old friends.'),
(13, 24, 'A literary masterpiece, a must-read.'),
(14, 25, 'Engaging from start to finish.'),
(15, 26, 'A wonderful blend of plot and emotion.'),
(2, 27, 'A beautifully crafted literary work.'),
(4, 28, 'Intriguing and thoughtfully written.'),
(5, 29, 'A page-turner with depth and substance.'),
(7, 30, 'A journey into the heart of storytelling.'),
(8, 31, 'An exploration of human nature.'),
(10, 32, 'An enthralling tale of love and loss.'),
(11, 33, 'Characters that linger in the mind.'),
(13, 34, 'A literary gem that stays with you.'),
(14, 35, 'A compelling and richly detailed world.'),
(15, 36, 'A literary adventure worth savoring.'),
(2, 37, 'A literary journey full of surprises.'),
(4, 38, 'Characters with depth and authenticity.'),
(5, 39, 'Immersive and emotionally resonant.'),
(7, 40, 'An exploration of the human experience.'),
(8, 41, 'A tapestry of words that captivates.'),
(10, 42, 'Intriguing plot twists and revelations.'),
(11, 43, 'A masterclass in storytelling.'),
(13, 44, 'Engaging and thought-provoking.'),
(14, 45, 'Characters that you connect with.'),
(15, 46, 'A literary gem that lingers.'),
(2, 47, 'A captivating tale of love and loss.'),
(4, 48, 'Skillfully written with a poetic touch.'),
(5, 49, 'An emotional rollercoaster of a journey.'),
(7, 50, 'A book that leaves a lasting impression.'),
(8, 51, 'Characters you care about deeply.'),
(10, 52, 'A rich and vibrant literary landscape.'),
(11, 53, 'Page-turning excitement with depth.'),
(13, 54, 'A journey into the heart of imagination.'),
(14, 55, 'Thoughtful and beautifully executed.'),
(15, 56, 'A symphony of words and emotions.'),
(2, 57, 'An enthralling narrative that captivates.'),
(4, 58, 'Characters that feel real and relatable.'),
(5, 59, 'A literary experience that stays with you.'),
(7, 60, 'Intricate plot woven with precision.'),
(8, 61, 'A tapestry of emotions and storytelling.'),
(10, 62, 'A journey into the realms of imagination.'),
(11, 63, 'Thoughtful exploration of human nature.'),
(13, 64, 'A captivating tale of discovery and growth.');


INSERT INTO users (username, first_name, last_name, gender, email, phone, password_hash, create_date, last_login_date, is_manager) 
VALUES 
('ahmetk', 'Ahmet', 'Kaya', 'male', 'ahmet.kaya@email.com', '+905301234567', 'pass1234', NOW(), NOW(), 1),
('elifb', 'Elif', 'Balcı', 'female', 'elif.balci@email.com', '+905351234568', '1234pass', NOW(), NOW(), 0),
('mehmety', 'Mehmet', 'Yılmaz', 'male', 'mehmet.yilmaz@email.com', '+905371234569', 'abcd1234', NOW(), NOW(), 1),
('zeynepd', 'Zeynep', 'Demir', 'female', 'zeynep.demir@email.com', '+905391234560', 'password1', NOW(), NOW(), 0),
('fatmaa', 'Fatma', 'Aydın', 'female', 'fatma.aydin@email.com', '+905301234561', 'mypassword', NOW(), NOW(), 0),
('kemalo', 'Kemal', 'Öztürk', 'male', 'kemal.ozturk@email.com', '+905321234562', 'pass9876', NOW(), NOW(), 1),
('aysee', 'Ayşe', 'Erbil', 'female', 'ayse.erbil@email.com', '+905341234563', 'simplepass', NOW(), NOW(), 0),
('emreo', 'Emre', 'Orhan', 'male', 'emre.orhan@email.com', '+905361234564', 'qwerty123', NOW(), NOW(), 0),
('sibelt', 'Sibel', 'Taş', 'female', 'sibel.tas@email.com', '+905381234565', 'letmein', NOW(), NOW(), 1),
('hakanc', 'Hakan', 'Çelik', 'male', 'hakan.celik@email.com', '+905301234566', '12345678', NOW(), NOW(), 0),
('leylag', 'Leyla', 'Gül', 'female', 'leyla.gul@email.com', '+905311234567', 'password2', NOW(), NOW(), 0),
('osmank', 'Osman', 'Kurt', 'male', 'osman.kurt@email.com', '+905331234568', 'iloveyou', NOW(), NOW(), 1),
('nurayd', 'Nuray', 'Demirci', 'female', 'nuray.demirci@email.com', '+905351234569', 'sunshine', NOW(), NOW(), 0),
('burakc', 'Burak', 'Çağlar', 'male', 'burak.caglar@email.com', '+905371234560', 'hello123', NOW(), NOW(), 0),
('selinc', 'Selin', 'Can', 'female', 'selin.can@email.com', '+905391234561', 'password3', NOW(), NOW(), 0);

INSERT INTO address (id, user_id, city_id, town_id, district_id, postal_code, address_text) 
VALUES
(1, 2, 1, 1, 1, '06140', 'Yıldıztepe Mahallesi, Gül Sokak, No: 4, Daire: 7, Altındağ, Ankara, 06140, Türkiye'),
(2, 4, 1, 2, 2, '06640', 'Bağcılar Mahallesi, Cengiz Sokak, No: 15, Daire: 17, Çankaya, Ankara, 06640, Türkiye'),
(3, 5, 1, 3, 3, '06170', 'Varlık Mahallesi, Sefa Sokak, No: 4, Daire: 5, Yenimahalle, Ankara, 06170, Türkiye'),
(4, 7, 1, 4, 4, '06310', 'Şenlik Mahallesi, Metalica Sokak, No: 7, Daire: 1, Keçiören, Ankara, 06310, Türkiye'),
(5, 8, 2, 5, 5, '34975', 'Burgazada Mahallesi, İstanbul Sokak, No: 4, Daire: 2, Adalar, İstanbul, 34975, Türkiye'),
(6, 10, 2, 6, 6, '34285', 'Atatürk Mahallesi, Enverpaşa Sokak, No: 15, Daire: 7, Arnavutköy, İstanbul, 34285, Türkiye'),
(7, 11, 2, 7, 7, '34874', 'Ambarlı Mahallesi, Atatürk Sokak, No: 23, Daire: 4, Avcılar, İstanbul, 34874, Türkiye'),
(8, 13, 3, 8, 8, '35192', 'Barış Mahallesi, Papatya Sokak, No: 2, Daire: 1, Buca, İzmir, 35192, Türkiye'),
(9, 14, 3, 9, 9, '35478', 'Vatan Mahallesi, Adem Sokak, No: 7, Daire: 9, Karabağlar, İzmir, 35478, Türkiye'),
(10, 15, 3, 10, 10, '35698', 'Barbaros Mahallesi, Miraç Sokak, No: 3, Daire: 1, Bornova, İzmir, 35698, Türkiye');

INSERT INTO cities (id, city_name) 
VALUES
(1, 'Ankara'),
(2, 'İstanbul'),
(3, 'İzmir');

INSERT INTO districts (id, town_id, district_name) 
VALUES
(1, 1, 'Yıldıztepe'),
(2, 2, 'Bağcılar'),
(3, 3, 'Varlık'),
(4, 4, 'Şenlik'),
(5, 5, 'Burgazada'),
(6, 6, 'Atatürk'),
(7, 7, 'Ambarlı'),
(8, 8, 'Barış'),
(9, 9, 'Vatan'),
(10, 10, 'Barbaros');

INSERT INTO towns (id, city_id, town_name) 
VALUES
(1, 1, 'Altındağ'),
(2, 1, 'Çankaya'),
(3, 1, 'Yenimahalle'),
(4, 1, 'Keçiören'),
(5, 2, 'Adalar'),
(6, 2, 'Arnavutköy'),
(7, 2, 'Avcılar'),
(8, 3, 'Buca'),
(9, 3, 'Karabağlar'),
(10, 3, 'Bornova');