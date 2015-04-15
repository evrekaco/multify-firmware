<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 20:36
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Client where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
if(count($Results) < 1){
    header("Location: index.php");
}
?>
<!DOCTYPE html>
<html>
<head lang="en">
    <meta charset="UTF-8">
    <title>Multify | Client Panel</title>
    <!--    <script type="text/javascript" src="jquery-1.8.0.min.js"></script>-->
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
    <style type="text/css">
        input[type=text]
        {
            border: 1px solid #ccc;
            border-radius: 3px;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);
            width:200px;
            min-height: 28px;
            padding: 4px 20px 4px 8px;
            font-size: 12px;
            -moz-transition: all .2s linear;
            -webkit-transition: all .2s linear;
            transition: all .2s linear;
        }
        input[type=text]:focus
        {
            width: 400px;
            border-color: #51a7e8;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.1),0 0 5px rgba(81,167,232,0.5);
            outline: none;
        }
        input[type=password]
        {
            border: 1px solid #ccc;
            border-radius: 3px;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);
            width:200px;
            min-height: 28px;
            padding: 4px 20px 4px 8px;
            font-size: 12px;
            -moz-transition: all .2s linear;
            -webkit-transition: all .2s linear;
            transition: all .2s linear;
        }
        input[type=password]:focus
        {
            width: 400px;
            border-color: #51a7e8;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.1),0 0 5px rgba(81,167,232,0.5);
            outline: none;
        }
    </style>
    <script>
        $(function() {
            $( "#tabs" ).tabs();
        });
    </script>
</head>
<body>
<div id="header" style="background-color: #1a1a1a; padding: 20px;">
    <img src="img/multify_logo_original_white.png" style="width: auto; height: 75px; margin: 10px;"/>
    <span class="glyphicon glyphicon-user" aria-hidden="true" style="color: #ffffff;"></span><span style="color: #ffffff; margin-left: 5px;">Hoşgeldin,<?php echo $Results['name'];?></span>
</div>
<div class="btn-group btn-group-justified" role="group" aria-label="">
    <div class="btn-group" role="group">
        <button type="button" class="btn btn-default" onclick="showEdit()"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Profil</button>
    </div>
    <div class="btn-group" role="group">
        <button type="button" class="btn btn-default" onclick="showFix()"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Multify'ımı Doğrula</button>
    </div>
    <div class="btn-group" role="group">
        <button type="button" class="btn btn-default" id="logout" ><span class="glyphicon glyphicon-off" aria-hidden="true"></span> Çıkış</button>
    </div>
</div>
<iframe id="edit" class="tab current_tab" src="edit_profile.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
<iframe id="fix" class="tab" src="fix.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
</body>
<script>
    function showEdit(){
        $(".tab").removeClass("current_tab");
        $("#edit").toggleClass("current_tab");
        var iframe = document.getElementById("edit");
        iframe.src = "edit_profile.php";
    }
    function showFix(){
        $(".tab").removeClass("current_tab");
        $("#fix").toggleClass("current_tab");
        var iframe = document.getElementById("fix");
        iframe.src = "fix.php";
    }
    function resizeIframe(obj) {
        obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
    }
    $("#logout").click(function(){
        document.location = "logout.php";
    });
</script>
</html>
