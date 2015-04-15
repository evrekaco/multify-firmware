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
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.html");
}
if(isset($_POST['button_delete'])){
    $sql = "delete from Multify where cid=$selected";
    mysqli_query($connection, $sql);
    $sql = "delete from Client where cid=$selected";
    mysqli_query($connection, $sql);
    mysqli_close($connection);
    header("Location: edit-remove.php");
}
$sql = "select name,code4sq,email from Client where cid=$selected";
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

<div id="tabs" style="width: 50%; margin: -50px auto;">
    <h1 style="margin-bottom: 50px; text-align: center;"><span class="label label-primary">Add New Venue</span></h1>
    <form action="update_venue.php" class="formAlign" method="post">
        <div class="input-group input-group-lg">
            <span class="input-group-addon" id="sizing-addon3">Name</span>
            <input type="text" name="name" class="form-control" value="<?php echo $Results['name']?>" required/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3">@</span>
            <input type="text" name="email" class="form-control" value="<?php echo $Results['email']?>" required/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon3">Password</span>
            <input type="password" name="password" class="form-control" placeholder="Password" required/>
        </div>
        <div class="input-group input-group-lg" style="margin-top: 10px;">
            <span class="input-group-addon" id="sizing-addon2"><img src="img/foursquare.png" style="height: 24px; width: 24px;"></span>
            <input type="text" name="4sqID" class="form-control" value="<?php echo $Results['code4sq']?>" required/>
        </div>
        <input name="selected" type="hidden" value="<?php echo $selected; mysqli_close($connection)?>" />

        <button type="submit" class="btn btn-success formButton" value="Edit">Done</button>
    </form>

</div>
</body>
</html>