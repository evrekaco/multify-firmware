<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 02:41
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Admin where id=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.html");
}
$sql = "select cid,name from Client;";
$strSQL = mysqli_query($connection, $sql);
$sql = "select did,dev_name,deviceID from Devices;";
$strSQL2 = mysqli_query($connection, $sql);
$sql = "select id, appName from FoursquareApps where usageCount < 3;";
$strSQL3 = mysqli_query($connection, $sql);
?>
<!DOCTYPE html>
<html>
<head lang="en">
    <meta charset="UTF-8">
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="css/multi-select.css" media="screen" rel="stylesheet" type="text/css">
    <script src="js/jquery.multi-select.js" type="text/javascript"></script>
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

<div id="tabs" style="width: 50%; height: 100%; margin: -50px auto;">
    <center><h1 style="margin-bottom: 15px; width: 100%; "><span class="label label-primary">Link Venue - Device</span></h1></center>
    <form action="make-link.php" method="post">
        <div>
        <h3 style="text-align: center;"><b>Venues</b></h3>
        <select name="client" style="margin: 0 auto; display: table;">
            <?php
                while($Results = mysqli_fetch_array($strSQL)){
                    echo '<option value ="'.$Results['cid'].'">'.$Results['name'];
                }
            ?>
        </select>

        </div>
        <div>
            <h3 style="text-align: center;"><b>Foursquare Apps</b></h3>
            <select name="foursquareApps" style="margin: 0 auto; display: table;">
                <?php
                while($Results = mysqli_fetch_array($strSQL3)){
                    echo '<option value ="'.$Results['id'].'">'.$Results['appName'];
                }
                ?>
            </select>

        </div>
        <h3 style="text-align: center;"><b>Devices</b></h3>
        <select name="devices[]" multiple="multiple" id="my-select" style="width: 100%;">
            <?php
            while($Results = mysqli_fetch_array($strSQL2)){
                echo '<option value="'.$Results['did'].'">'.$Results['dev_name'].",".$Results['deviceID']."</option>";
            }
            ?>
        </select>
        <button type="submit" class="btn btn-success" value="Link" style="margin-left: 47%; margin-bottom: 20px;">Link</button>
    </form>

</div>
</body>
<script>
    $('#my-select').multiSelect({});
</script>
</html>