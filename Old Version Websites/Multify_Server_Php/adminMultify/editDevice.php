<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 03:27
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

<div id="tabs" style="width: 50%; margin: -50px auto; height: auto">
    <div id="tabs-1" style="text-align: center; height: auto;">
        <center><h1 style="margin-bottom: 50px;"><span class="label label-primary">Edit / Remove Device</span></h1></center>
        <form class="formAlign" action="make-edit-device.php" method="post">
            <?php
            $sql = "Select did,dev_name,deviceID from Devices;";
            $strSQL = mysqli_query($connection,$sql);
            $i = 1;
            echo '<table class="table table-bordered" style="background-color: #ffffff; text-align: center;"><tr><th style="text-align: center;">#</th><th style="text-align: center;">Name</th><th style="text-align: center">Device ID</th><th style="text-align: center;">Select</th></tr>';
            while($Results = mysqli_fetch_array($strSQL)){
                echo "<tr><td>$i</td><td>".$Results['dev_name']."</td><td>".$Results['deviceID']."</td><td><input type='radio' name='selected' value='".$Results['did']."'/></td></tr>";
                $i++;
            }
            echo "</table>";
            ?>
<!--            <div style="float: right;">-->
                <button name="button_edit" type="submit" class="btn btn-success formButton" value="Edit">Edit</button>
                <button name="button_delete" type="submit" class="btn btn-danger formButton" value="Delete">Delete</button>
<!--            </div>-->
        </form>
    </div>

</div>
</body>
</html>