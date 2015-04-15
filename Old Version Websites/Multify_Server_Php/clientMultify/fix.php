<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 21:10
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Client where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.php");
}
$sql = "select DISTINCT(4sqCheckInCount) as cnt from Multify Where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Result = mysqli_fetch_array($strSQL);
//$sql = "select did.4sqCheckInCount from Multify where cid=$id";
$sql = "Select Devices.did AS id, Devices.dev_name AS name, Devices.deviceID AS dev_id From Multify JOIN Devices ON Multify.did = Devices.did WHERE Multify.cid=$id;";
$strSQL = mysqli_query($connection,$sql);
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

</head>
<body style="margin-top: 100px;">

<div id="tabs" style="width: 50%; margin: -50px auto;">
    <h1 style="margin-bottom: 50px; text-align: center;"><span class="label label-primary">Multify'ımı Doğrula</span></h1>
    <div style="text-align: center;">
        <?php
            while($row = mysqli_fetch_array($strSQL)){
                echo "<button type=\"button\" class=\"btn btn-default\" data-container=\"body\" data-toggle=\"popover\" data-html=\"true\" data-placement=\"right\" data-content=\"<form id='fixform' action='correct.php' method='post'><label>Görünen: </label><input type='text' name='fsq' value='".$Result['cnt']."' hidden> <input type='text' name='dev_id' value='".$row['dev_id']."' hidden> <input id='val' type='text' name='value' required/><p><button type='submit' style='float: right;' class='btn btn-success formButton' value='Edit'>Doğrula</button></p></form>\">".$row['name']."</button>";
            }
        ?>
        <?php

        ?>
    </div>

</div>

<!-- Latest compiled and minified JavaScript -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
<script>
    $(function() {
        $( "#tabs" ).tabs();
    });
    $("[data-toggle=popover]").popover();
</script>
</body>
</html>