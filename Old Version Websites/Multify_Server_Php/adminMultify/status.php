<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 00:22
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Admin where id=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
if(count($Results) < 1){
    header("Location: index.html");
}
$sql = "select Client.cid, Client.name, Multify.did, Multify.4sqCheckInCount from Client JOIN Multify ON Multify.cid = Client.cid";
$strSQL = mysqli_query($connection,$sql);
$devSearch = "select Multify.did, Devices.did, Devices.dev_name from Multify JOIN Devices ON Multify.did = Devices.did";
$strSQL2 = mysqli_query($connection,$devSearch);
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

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="css/main.css" />
    <script>
        $(function() {
            $( "#tabs" ).tabs();
        });
    </script>

</head>

<body style="margin-top: 100px;">s2['dev_name']

    <div id="tabs" style="width: 50%; height: auto; margin: -50px auto;">
        <center><h1 style="margin-bottom: 50px;"><span class="label label-primary">CheckIn Status</span></h1></center>
            <?php
            $i = 1;
            echo '<table class="table table-bordered" style="background-color: #ffffff; text-align: center;"><tr><th style="text-align: center;">#</th><th style="text-align: center;">Venue Name</th><th style="text-align: center;">Device</th><th style="text-align: center;">CheckIn</th></tr>';
            while($Results = mysqli_fetch_array($strSQL)){
                $Results2 = mysqli_fetch_array($strSQL2);
                echo "<tr><td>$i</td><td>".$Results['name']."</td><td>".$Results2['dev_name']."</td><td>".$Results['4sqCheckInCount']."</td></tr>";
                $i++;
            }
            echo "</table>";
            ?>
    </div>
</body>
</html>
