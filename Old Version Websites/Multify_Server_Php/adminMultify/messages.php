<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 08.01.2015
 * Time: 17:17
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
else{
    $sql = "select * from Subscribers";
    $strSQL = mysqli_query($connection, $sql);
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
        <center><h1 style="margin-bottom: 50px;"><span class="label label-primary">Messages</span></h1></center>
        <form class="formAlign" action="make-edit-device.php" method="post">
            <?php
                echo '<table class="table table-bordered" style="background-color: #ffffff; text-align: center;"><tr><th style="text-align: center;">From:</th><th style="text-align: center;">Message:</th><th style="text-align: center">Phone:</th></tr>';
                while($Results = mysqli_fetch_array($strSQL)){
                    echo "<tr><td>".$Results['name']."<".$Results['email']."-".$Results['venueName'].">"."</td><td>".$Results['message']."</td><td>".$Results['phone']."</td></tr>";
                }
                echo "</table>";
            ?>
            <!--            <div style="float: right;">-->
<!--            <button name="button_edit" type="submit" class="btn btn-success formButton" value="Edit">Edit</button>-->
<!--            <button name="button_delete" type="submit" class="btn btn-danger formButton" value="Delete">Delete</button>-->
            <!--            </div>-->
        </form>
    </div>

</div>
</body>
</html>