<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 16:55
 */
session_start();
$_SESSION['lang'] = "tr";
?>
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/html">
<head>
    <title>Multify</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css"-->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.css">
    <link rel="stylesheet" href="css/swiper.min.css">
    <link rel="stylesheet" href="css/style.css">

</head>
<script>
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r;
        i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date();
        a = s.createElement(o),
            m = s.getElementsByTagName(o)[0];
        a.async = 1;
        a.src = g;
        m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-59611764-1', 'auto');
    ga('send', 'pageview');

</script>
<body>
<!-- Google Tag Manager -->
<noscript>
    <iframe src="//www.googletagmanager.com/ns.html?id=GTM-5SNKMX"
            height="0" width="0" style="display:none;visibility:hidden"></iframe>
</noscript>
<script>(function (w, d, s, l, i) {
        w[l] = w[l] || [];
        w[l].push({
            'gtm.start': new Date().getTime(), event: 'gtm.js'
        });
        var f = d.getElementsByTagName(s)[0],
            j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : '';
        j.async = true;
        j.src =
            '//www.googletagmanager.com/gtm.js?id=' + i + dl;
        f.parentNode.insertBefore(j, f);
    })(window, document, 'script', 'dataLayer', 'GTM-5SNKMX');</script>
<!-- End Google Tag Manager -->
<nav class="navbar navbar-default navbar-fixed-top">
    <div class="container-fluid">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse"
                    data-target="#collapsing-section">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="#" id="home"><img id="multify-logo" alt="Brand" src="img/logo.png"/></a>

        </div>
        <div class="collapse navbar-collapse" id="collapsing-section">
            <ul class="nav navbar-nav home-nav-bar navbar-right">
                <li><a href="#" id="deneyim"><p class="banner">Multify Experience</p></a></li>
                <li><a href="#" id="how-it-works"><p class="banner">How it Works?</p></a></li>
                <li>
                    <button class="btn btn-xs btn-red navbar-btn home-button visible-lg visible-md visible-sm"
                            id="pre-purchase"><p class="banner">Pre-order</p></button>
                    <a href="#" id="pre-purchase" class="visible-xs"><p class="banner">Pre-order</p></a>
                </li>
                <li><a href="#" id="changeLang" onclick="clickLang()"><img src="img/tr.png" id="lang-logo"
                                                                           class="img-circle visible-lg visible-md visible-sm"
                                                                           height="31" width="31"/>

                        <p class="banner visible-xs">Türkçe Siteye Geçh</p></a></li>
                <li>
                    <button class="btn btn-xs btn-warning navbar-btn home-button visible-lg visible-md visible-sm"
                            id="login" onclick='window.location="clientMultify"; return false;'><p
                            class="banner">Log In</p></button>
                    <a href="clientMultify" id="login" class="visible-xs"><p class="banner">Log In</p></a>
                </li>
            </ul>
        </div>
    </div>
</nav>
<div class="container-fluid" id="slider">
    <div class="row">
        <!-- Swiper -->
        <div class="swiper-container">
            <div class="swiper-wrapper">
                <div class="swiper-slide"><img src='img/multify_new1.jpg' class="center-block img-responsive"></div>
                <div class="swiper-slide"><img src='img/multify_digital.png' class="center-block img-responsive"></div>

            </div>
            <!-- Add Pagination -->
            <div class="swiper-pagination"></div>
        </div>
    </div>
</div>

<div class="container">
    <div class="row" style="margin-top:25px; margin-bottom:25px">

        <h1 class="heading" id="toDeneyim"><b>Multify Experience</b></h1>

        <div class="col-lg-12" id="first-row">
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-5 vertical-align">
                    <img src="img/multify1.png" class="img-responsive center-block ">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-7 vertical-align">
                    <p class="description">Multify allows you to use your value in social media as a real time marketing tool by reflecting the value of your place in social media into our lives.</p>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-7 vertical-align">
                    <p class="description">Multify creates an interactive atmosphere among customers and places. By this way number of customers and value of your place increase.</p>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-5 vertical-align">
                    <img src="img/2.png" class="img-responsive center-block">
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-5 vertical-align">
                    <img src="img/multify_gorsel_orta.png" class="center-block img-responsive">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-7 vertical-align">
                    <p class="description">Multify is not only a real time marketing tool but also a fancy thing that everyone likes on the wall.</p>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-7 vertical-align">
                    <p class="description">It is not all!!! You can understand your customers better thanks to monthly analyze reports. This leads you to attract more and more customers than your competitors.</p>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-5 vertical-align">
                    <img src="img/4.png" class="img-responsive center-block">
                </div>
            </div>
        </div>
    </div>
</div>

<div class="container">
    <div class="row" id="how-it-works-row" style="margin-top:25px; margin-bottom:25px">
        <h1 class="heading" id="toWorks"><b>How It Works?</b></h1>

        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
            <div class="row">
                <img src="img/6.png" class="center-block img-responsive">
            </div>
            <div class="row">
                <p class="heading-minor"><b>POSITION MULTIFY IN YOUR PLACE</b></p>

                <div class="subheading2">
                    <p class="description">Either on the wall or on any desk is a great place for Multify!</p>
                </div>
            </div>
        </div>

        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
            <div class="row">
                <img src="img/5.png" class="center-block img-responsive">
            </div>
            <div class="row">
                <p class="heading-minor"><b>PLUG IN</b></p>
            </div>
        </div>

        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
            <div class="row">
                <img src="img/7.png" class="center-block img-responsive">
            </div>
            <div class="row">
                <p class="heading-minor"><b>CONNECT TO WI-FI</b></p>

                <div class="subheading2">
                    <p class="description">Wi-Fi Connection is required for data transfer.</p>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="container-fluid">
    <div class="row" style="margin-top:25px; margin-bottom:25px">
        <h1 class="heading" id="sizes" style="margin-bottom:50px"><b>TECHNICAL SPECIFICATIONS</b></h1>

        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 vertical-align2">
            <img src="img/multify_size2.png" class="center-block img-responsive"/>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 vertical-align2">
            <img src="img/digital_dimensions.png" class="center-block img-responsive"/>
        </div>

    </div>
</div>


<div class="container" style="margin-top:25px; margin-bottom:25px">
    <div class="row">

        <div class="subheading" id="toPurchase">

            <p class="text-center heading" style="margin-top:25px; margin-bottom:25px">Pre Order</p>

            <div class="center-block form-div">
                <form class="form-horizontal" action="sendMessage.php" method="post">
                    <div class="form-group" style="margin: 5px auto;">
                        <label for="venueName" class="col-sm-2 control-label">Company Name: </label>

                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="venueName" placeholder="Company Name"
                                   name="venueName" required/>
                        </div>
                    </div>

                    <div class="form-group" style="margin: 5px auto;">
                        <label for="name" class="col-sm-2 control-label">Name Surname: </label>

                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="name" placeholder="Name Surname" name="name"
                                   required/>
                        </div>
                    </div>

                    <div class="form-group" style="margin: 5px auto;">
                        <label for="email" class="col-sm-2 control-label">E-Mail</label>

                        <div class="col-sm-10">
                            <input type="email" class="form-control" id="email" placeholder="E-Mail" name="email"
                                   required/>
                        </div>
                    </div>
                    <div class="form-group" style="margin: 5px auto;">
                        <label for="phone" class="col-sm-2 control-label">Phone Number: </label>

                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="phone" placeholder="Phone Number" name="phone"
                                   required/>
                        </div>
                    </div>

                    <div class="form-group" style="margin: 5px auto;">
                        <label for="message" class="col-sm-2 control-label">Your Message: </label>

                        <div class="col-sm-10">
                            <textarea id="message" class="form-control" rows="3" name="message" required></textarea>
                        </div>
                    </div>
                    <div class="form-group" style="margin: 5px auto;">
                        <div class="col-sm-10">
                            <input type="checkbox" class="pull-right" name="sozlesme" id="sozlesme_id"
                                    required>
                            <label for="sozlesme_id" class="pull-right"> I agree the <a href="#" data-toggle="modal" data-target="#sozlesmeModal">Terms of Service</a></label>
                        </div>
                    </div>

                    <div class="form-group" class="pull-right" style="margin: 5px auto;">
                        <div class="col-sm-12">
                            <button type="submit" class="btn btn-red pull-right">Submit</button>
                        </div>
                    </div>
                </form>

            </div>


        </div>

    </div>
</div>

<div class="container" style="margin-bottom: 20px">
    <div class="row">
        <div class="subheading">
            <p class="text-center" style="margin-top: 50px;">Subscribe to get informed about Multify.</p>

            <div class="center-block form-div">
                <form class="form-horizontal" action="subscribe.php" method="post">
                    <div class="form-group" style="margin: 5px auto;">
                        <label for="email" class="col-sm-2 control-label">E-mail</label>

                        <div class="col-sm-10">
                            <input type="email" class="form-control" id="email" placeholder="E-mail" name="email"
                                   required/>
                        </div>
                    </div>
                    <div class="form-group" style="margin: 5px auto;">
                        <div class="col-sm-12">
                            <button type="submit" class="btn btn-red pull-right">Register</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>

<div class="container-fluid">
    <div class="row footer" style="padding-top: 30px">
        <div class="container">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                <div class="row" style="padding-left: 30px;padding-right: 30px;">
                    <img src="img/logo.png" id="footer-logo" class="center-block img-responsive"
                         style="padding: 15px; max-height: 128px;"/>
                </div>
                <div class="center-block" id="social-logos">
                    <p class="justify-image">
                        <a href="https://www.facebook.com/MultifyCo"><img src="img/Facebook-icon.png"/></a>
                        <a href="https://twitter.com/MultifyCo"><img src="img/Twitter-icon.png"/></a>
                        <a href="https://plus.google.com/102685943168451757272" rel="publisher"><img
                                src="img/GooglePlus-icon.png"/></a>
                    </p>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                <ul class="list-unstyled"
                <li>
                    <strong>Multify</strong>
                </li>
                <li>
                    <a href="#" data-toggle="modal" data-target="#sozlesmeModal">Terms of Service</a>
                </li>
                <li>
                    <a href="#">Kullanıcı Sözleşmesi</a>
                </li>
                </ul>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="sozlesmeModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
     aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span
                        aria-hidden="true">&times;</span></button>
                <h2 class="modal-title" id="myModalLabel">Terms of Service</h2>
            </div>
            <div class="modal-body">
<h3> Madde 1.  KONU </h3> 
<p> Multify’ın internet sitesinde (www.multify.co) elektronik ortamda özellikleri ve fiyatı belirtilen sözleşme konusu ürünlerin satışı ve yurt içinde alıcıya teslimi ile ilgili tarafların hak ve yükümlülükleri bu sözleşmenin konusunu oluşturmaktadır. </p>
<h3>Madde 2.  SATIŞ VE TESLİMAT </h3>
<p>2.1. Teslimat, satıcı firma tarafından kargo aracılığı ile müşterinin belirttiği teslimat adresine sipariş onaylandıktan sonra 30 iş günü içinde yapılacaktır. </p>
<p>2.2. Resmi tatiller ve dini/milli bayram günleri bu süreden sayılmaz. Teslimat sırasında kargo görevlisi aracılığıyla ürünü teslim aldım ibaresine atılan imza;  alıcı ürünü muayene edip, ürünü tam eksiksiz ve hasarsız olarak teslim aldığını kabul ettiğini gösterir. Kargo veya kuryeye herhangi bir nakliye ücreti ödenmeyecektir. </p>
<p>2.3. Ürünün kiralanması durumunda ürün için bir depozito ücreti alma hakkını EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ kendinde saklı tutar. Ürünün kiralama süreci bittiği zaman ürün satıcı firmaya geri gönderildiğinde ürünün üzerinde herhangi bir sorun olduğu durumda, satıcı firma satış sırasında almış olduğu kullanıcı hesap bilgileri üzerinden 500 (beş yüz) Türk Lirasına kadar bir ücret tahsisi yapabilir. </p>
<h3>Madde 3. ÜRÜN VE HİZMETİ</h3>
<p>3.1. Multify cihazları, EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ tarafından geliştirilmiş bir üründür. İşletmelerin sosyal medyadaki (Swarm) check-in sayılarını gerçek hayatta, mekanik sayacında veya dijital ekranında gösteren bir alettir.</p>
<p>3.2. Multify, işletmede bulunan kablosuz internet altyapısını kullanarak bulut tabanlı sisteme erişerek işletmenin swarm üzerindeki “check-in” sayısına ulaşır ve bulut tabanlı sistemden gelen komutları kablosuz olarak mekanik sayacına ve ekranına yansıtır.</p>
<p>3.3. Multify, Foursquare ve Swarm üzerinden gelen verileri depolayan, aynı zamanda üyelerden gelen yönergelere göre çeşitli algoritmaların kullanımı ile kullanıcılarına belli raporlar sunabilen bir üründür.</p>
<p>3.4. Multify adlı ürünün sorunsuz çalışabilmesi için işletmede gerekli kablosuz internet ağı ve elektrik bağlantısı olması gereklidir. İşletmede bulunan kablosuz internet ağının ise “Captive Portal” ismiyle anlatılan bir ağ olmaması gerekmektedir. “Captive Portal” (Doruknet Wispotter, TTNET Wifi) internete girmek istediğinizde karşınıza çıkan ve internete girebilmeniz için sayfaya kayıt olmanızı isteyen yapının ismidir. </p>
<p>3.5. Multify’ı kullanmadan önce Multify Cihazları ile birlikte gelen veya internet sitesinden temin edebileceğiniz kullanım kılavuzunu okuyup, tüm talimatları izleyin.</p>
<p>3.6. Multify’ın kullanımı için gerekli teknik gereksinimler www.multify.co internet sitesinin Sıkça Sorulan Sorular (S.S.S) bölümünde listelenmiştir. Üyelik ve satın alma işlemlerini tamamlamadan önce kullanıcının bu gereksinimleri karşılayıp karşılayamayacağını kontrol etmesi kendi sorumluluğundadır.</p>
<p>3.7. Multify’ın kullanımı için kullanıcıların www.multify.co internet sitesinden üyelik oluşturmaları ve tüm kullanıcıların tek tek üyelik sözleşmesinin koşullarını kabul etmeleri gerekmektedir. (Multify’ı kullanan alıcılar “kullanıcı” olarak anılacaktır.)</p>
<p>3.8. Multify güvenlik veya iyileştirme gerekliliklerinden Multify cihazlarında uzaktan yazılım güncellemesi yapabilir.</p>
<p>3.9. Ürün’ün işletmeye yerleştirilme hizmetleri, ürünün bir parçası değildir.</p>
<p>3.10. Müşteri ürünün işletmeye yerleştirilmesi konusunda hizmet satın almak istiyorsa bunu üretici veya satıcı firma ile görüşmelidir.</p>

<h3>Madde 4. – CAYMA HAKKI</h3>
<p>4.1. Alıcı ürünü teslim aldığı tarihten itibaren 7 (yedi) gün içerisinde; yazılı olarak ileterek herhangi bir gerekçe göstermeksizin ve cezai şart ödemeksizin sözleşmeden cayma hakkına sahiptir.</p>
<p>4.2. Cayma hakkının kullanıldığına dair bildirimin 7 (yedi) gün içerisinde EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ’ne yöneltilmiş olması yeterlidir. Cayma süresi içinde EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ sözleşmeye konu mal veya hizmet karşılığında alıcıdan herhangi bir isim altında ödeme yapmasını veya alıcıyı borç altına sokan herhangi bir belge vermesini isteyemez. Alıcı, cayma süresi içinde malın mutat kullanımı sebebiyle meydana gelen değişiklik ve bozulmalardan sorumlu değildir.</p>
<p>4.3. Teslim alınmış olan ürünün değerinin azalması veya iadeyi imkânsız kılan bir nedenin varlığı cayma hakkının kullanılmasına engel değildir. Ancak değer azalması veya iadenin imkânsızlaşması alıcı kusurundan kaynaklanıyorsa, alıcı EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ’ne ürünün değerini veya değerindeki azalmayı tazmin etmesi gerekir.</p>

<h3>Madde 5. – TARAFLARIN YÜKÜMLÜLÜKLERİ VE GENEL ŞARTLAR</h3>
<p>5.1. Alıcı satın aldığı ürünün teslimatının süresinde ve doğru şekilde yapılması için, ödeme işlemlerini internet sitesinde verilen bilgilere göre gerçekleştirerek teslimata ilişkin adres ve kişi bilgilerini doğru ve eksiksiz olarak bildirdiğini beyan ve kabul etmiştir.</p>
<p>5.2. Alıcı, www.multify.co internet sitesinde satışı yapılan ürüne ait yine www.multify.co internet sitesi’nde belirtilen temel özellikleri, tüm vergiler dahil satış fiyatı, ödeme şekli ve teslimata ilişkin ön bilgileri okuyup bilgi sahibi olduğunu ve elektronik ortamda bu hususları kabul ettiğine dair teyidi verdiğini beyan ve kabul eder.</p>
<p>5.3.  Ürünün kiralanmadığı ve direk satın alındığı durumda, satıcı ve üretici firma kullanıcıya 6 ay süreli bir garanti hizmeti sunmak ile yükümlüdür.</p>
<p>5.4. Alıcı, teslim edilecek adres ve kişi olarak kendisinden başka bir kişi veya kuruluşu göstermiş ise bu kişi veya kuruluşun teslimatı kabul etmemesinden EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ sorumlu tutulamaz.</p>
<p>5.5. EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ sözleşme konusu ürünün sağlam, eksiksiz, siparişte belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullanım kılavuzları ile teslim edilmesinden sorumludur.</p>
<p>5.6. EVREKA YAZILIM DONANIM DANIŞMANLIK EĞİTİM SANAYİ VE LİMİTED ŞİRKETİ, hava muhalefeti, genel grev, ulaşımın kesilmesi gibi mücbir sebeplerle ancak bunlarla sınırlı olmamak üzere sözleşme konusu ürünün tesliminin imkansızlaşması sonucu yükümlülüklerini yerine getiremiyorsa, bu durumu alıcıya bildirmek zorundadır.</p>
<p>5.7. Multify adlı ürünün kullanım süresi boyunca ürün üzerinde, üründen kaynaklanan yani kullanıcının kullanım hatasından kaynaklanmayan sorunların çözümü için, ürünü üretici firmaya kargo ile göndermek ile yükümlüdür. Kargo ücreti müşterinin sorumluluğu altındadır. Üretici firma, yukarda ifade edildiği gibi ürün üzerinde kullanım hatasından kaynaklanmayan bir sorun olduğu durumda, ürünü 30 takvim günü içerisinde tamir etmek veya değiştirmek ile sorumludur.</p>
<p>5.8. Alıcı satın aldığı ürün/ürünler, bedelinin eksiksiz ödenmiş olması kaydıyla, alıcının teslimatın yapılmasını istediği adrese sipariş onayından itibaren 30 (otuz) günlük yasal süresi içinde teslim edilir.</p>
 
<p>İşbu sözleşme sipariş verildiği tarihte akdedilmiştir. Alıcı sipariş vermeden önce www.multify.co internet sitesinde, gerekse sipariş verdikten sonra iş bu sözleşmeyi okuduğunu, içerdiği tüm bilgi ve koşulları kabul ettiğini kabul, beyan ve taahhüt etmiştir.</p>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<script src="https://code.jquery.com/jquery-1.11.2.min.js"></script>
<script src="bootstrap/js/bootstrap.min.js"></script>
<!-- Swiper JS -->
<script src="js/swiper.min.js"></script>

<script>
    $("#deneyim").click(function () {
        $("html,body").animate({
            scrollTop: $("#toDeneyim").offset().top - 100
        }, 2000);
    });
    $("#how-it-works").click(function () {
        $("html,body").animate({
            scrollTop: $("#toWorks").offset().top - 100
        }, 2000);
    });
    $("#pre-purchase").click(function () {
        $("html,body").animate({
            scrollTop: $("#toPurchase").offset().top - 100
        }, 2000);
    });
    $("#home").click(function () {
        $("html,body").animate({
            scrollTop: $("#banner").offset().top
        }, 2000);
    });
    function clickLang() {
        window.location = "index.php";
    }

    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        paginationClickable: true,
        loop: true,
        autoplay: 5000

    });

</script>
</body>

</html>
