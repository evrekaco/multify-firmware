<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 28.01.2015
 * Time: 11:47
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

            <div id="tabs" style="width: 50%; height: 400px; margin: -50px auto;">
                <center><h1 style="margin-bottom: 50px;"><span class="label label-primary">Add New Foursquare Application</span></h1></center>
                <form action="add4sqApp.php" method="post" class="formAlign" style="width: 80%; margin: 0 auto;">
                    <div class="input-group input-group-lg">
                        <span class="input-group-addon" id="sizing-addon3">Application Name</span>
                        <input type="text" name="appName" class="form-control" placeholder="Application Name" required/>
                    </div>
                    <div class="input-group input-group-lg" style="margin-top: 10px;">
                        <span class="input-group-addon" id="sizing-addon3">Client ID</span>
                        <input type="text" name="clientID" class="form-control" placeholder="Client ID" required/>
                    </div>
                    <div class="input-group input-group-lg" style="margin-top: 10px;">
                        <span class="input-group-addon" id="sizing-addon3">Client Secret</span>
                        <input type="text" name="clientSecret" class="form-control" placeholder="Client Secret" required/>
                    </div>
                    <input name="action" type="hidden" value="signup" />
                    <button type="submit" class="btn btn-success formButton" value="Add">Add</button>
                </form>

            </div>
        </body>
</html>