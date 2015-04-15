<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 21:15
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Client where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
$sql1 = "select * from Multify where cid=$id";
$strSQL1 = mysqli_query($connection, $sql1);
$Results1 = mysqli_fetch_array($strSQL1);
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.php");
}
//$sql = "select name,code4sq,email from Client where cid=$selected";
//$strSQL = mysqli_query($connection,$sql);
//$Results = mysqli_fetch_array($strSQL);
?>
<!DOCTYPE html>
<html>
<head lang="en">
    <meta charset="UTF-8">
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap-theme.min.css">
    <link rel="stylesheet" href="css/main.css" />

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
    <script>
        $(function() {
            $( "#tabs" ).tabs();
        });
    </script>
</head>
<body style="margin-top: 100px;">

<div id="tabs" style="width: 50%; margin: -50px auto;">
    <h1 style="margin-bottom: 50px; text-align: center;"><span class="label label-primary">Profil</span></h1>
    <form action="update_client.php" class="formAlign" method="post">
        <div class="input-group input-group-lg">
            <span class="input-group-addon" id="sizing-addon3">İşletme Adı</span>
            <input type="text" name="name" class="form-control" value="<?php echo $Results['name']?>" disabled/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3">Kullanıcı Adı</span>
            <input type="text" name="email" class="form-control" value="<?php echo $Results['email']?>" disabled/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3">Check-in</span>
            <input type="text" name="checkin" class="form-control" value="<?php echo $Results1['4sqCheckInCount']?>" disabled/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3">Şifre</span>
            <input type="password" name="password" class="form-control" placeholder="Şifrenizi değiştirmek için yeni şifrenizi giriniz" required/>
        </div>
        <input name="selected" type="hidden" value="<?php echo $selected; mysqli_close($connection)?>" />

        <button type="submit" class="btn btn-success formButton" value="Edit">Uygula</button>
    </form>


</div>
</body>
</html>