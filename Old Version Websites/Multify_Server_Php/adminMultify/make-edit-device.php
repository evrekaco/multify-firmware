<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 03:32
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
if(isset($_POST['button_delete'])){
    $sql = "delete from Multify where did=$selected";
    mysqli_query($connection, $sql);
    $sql = "delete from Devices where did=$selected";
    mysqli_query($connection, $sql);
    mysqli_close($connection);
    header("Location: editDevice.php");
}
$sql = "select dev_name, deviceID from Devices where did=$selected";
$strSQL = mysqli_query($connection,$sql);
$Results = mysqli_fetch_array($strSQL);
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

<div id="tabs" style="width: 480px; margin: -50px auto;">
    <h1 style="margin-bottom: 50px;"><span class="label label-primary">Edit Device</span></h1>
    <form class="formAlign" action="update_device.php" method="post">
        <div class="input-group input-group-lg">
            <span class="input-group-addon" id="sizing-addon3">Name</span>
            <input type="text" name="name" class="form-control" placeholder="Name" value="<?php echo $Results['dev_name'];?>" required/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3"><span class="glyphicon glyphicon-tag" aria-hidden="true"></span></span>
            <input type="text" name="deviceID" class="form-control" placeholder="Device ID" value="<?php echo $Results['deviceID'];?>" required/>
        </div>
        <input name="selected" type="hidden" value="<?php echo $selected; mysqli_close($connection)?>" />

        <button type="submit" class="btn btn-success formButton" value="Edit">Done</button>
    </form>

</div>
</body>
</html>