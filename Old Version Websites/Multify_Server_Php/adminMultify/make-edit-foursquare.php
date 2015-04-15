<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 28.01.2015
 * Time: 12:15
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
    $sql = "delete from Multify where appID=$selected";
    mysqli_query($connection, $sql);
    $sql = "delete from FoursquareApps where id=$selected";
    mysqli_query($connection, $sql);
    mysqli_close($connection);
    header("Location: edit-remove-foursquare.php");
}
$sql = "select appName, clientID, clientSecret from FoursquareApps where id=$selected";
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
        <h1 style="margin-bottom: 50px;"><span class="label label-primary">Edit Foursquare Application</span></h1>
        <form class="formAlign" action="update_foursquare.php" method="post">
            <div class="input-group input-group-lg">
                <span class="input-group-addon" id="sizing-addon3">Application Name</span>
                <input type="text" name="appName" class="form-control" placeholder="Name" value="<?php echo $Results['appName'];?>" required/>
            </div>
            <div class="input-group input-group-lg" style="margin-top: 10px;">
                <span class="input-group-addon" id="sizing-addon3">Client ID</span></span>
                <input type="text" name="clientID" class="form-control" placeholder="Client ID" value="<?php echo $Results['clientID'];?>" required/>
            </div>
            <div class="input-group input-group-lg" style="margin-top: 10px;">
                <span class="input-group-addon" id="sizing-addon3">Client Secret</span>
                <input type="text" name="clientSecret" class="form-control" placeholder="Client Secret" value="<?php echo $Results['clientSecret'];?>" required/>
            </div>
            <input name="selected" type="hidden" value="<?php echo $selected; mysqli_close($connection)?>" />

            <button type="submit" class="btn btn-success formButton" value="Edit">Done</button>
        </form>

    </div>
    </body>
</html>