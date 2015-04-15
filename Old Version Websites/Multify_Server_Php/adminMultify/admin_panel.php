<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.12.2014
 * Time: 23:23
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
    <title>Multify | Admin Panel</title>
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
        <span class="glyphicon glyphicon-user" aria-hidden="true" style="color: #ffffff;"></span><span style="color: #ffffff; margin-left: 5px;">Welcome,<?php echo $Results['name'];?></span>
    </div>
    <div class="btn-group btn-group-justified" role="group" aria-label="">
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showCheckIn()"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> CheckIn Status</button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showAddNewVenue()"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Add New Venue</button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showEditRemove()"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Edit / Remove Venue</button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showAddDevice()"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Add Device </button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showEditDevice()"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Edit / Remove Device </button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showAddFoursquare()"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Add Foursquare App </button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showEditFoursquare()"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Edit / Remove Foursquare App </button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showLinkDevice()"><span class="glyphicon glyphicon-link" aria-hidden="true"></span> Link Venue - Device </button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" onclick="showMessages()" ><span class="glyphicon glyphicon-envelope" aria-hidden="true"></span> Messages</button>
        </div>
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-default" id="logout" ><span class="glyphicon glyphicon-off" aria-hidden="true"></span> Logout</button>
        </div>
    </div>
    <iframe id="checkin" class="tab current_tab" src="status.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="add" class="tab" src="add.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="edit" class="tab" src="edit-remove.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="addDevice" class="tab" src="add-device.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="editDevice" class="tab" src="editDevice.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="linkDevice" class="tab" src="link.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="messages" class="tab" src="messages.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="addFoursquare" class="tab" src="addFoursquareApp.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
    <iframe id="editFoursquare" class="tab" src="edit-remove-foursquare.php" width="100%" frameborder="0" scrolling="no" onload='resizeIframe(this);'></iframe>
</body>
<script>
    function showCheckIn(){
        $(".tab").removeClass("current_tab");
        $("#checkin").toggleClass("current_tab");
        var iframe = document.getElementById("checkin");
        iframe.src = "status.php";
    }
    function showAddNewVenue(){
        $(".tab").removeClass("current_tab");
        $("#add").toggleClass("current_tab");
        var iframe = document.getElementById("add");
        iframe.src = "add.php";
    }
    function showEditRemove(){
        $(".tab").removeClass("current_tab");
        $("#edit").toggleClass("current_tab");
        var iframe = document.getElementById("edit");
        iframe.src = "edit-remove.php";
    }
    function showAddDevice(){
        $(".tab").removeClass("current_tab");
        $("#addDevice").toggleClass("current_tab");
        var iframe = document.getElementById("addDevice");
        iframe.src = "add-device.php";
    }
    function showLinkDevice(){
        $(".tab").removeClass("current_tab");
        $("#linkDevice").toggleClass("current_tab");
        var iframe = document.getElementById("linkDevice");
        iframe.src = "link.php";
    }
    function showEditDevice(){
        $(".tab").removeClass("current_tab");
        $("#editDevice").toggleClass("current_tab");
        var iframe = document.getElementById("editDevice");
        iframe.src = "editDevice.php";
    }

    function showMessages(){
        $(".tab").removeClass("current_tab");
        $("#messages").toggleClass("current_tab");
        var iframe = document.getElementById("messages");
        iframe.src = "messages.php";
    }

    function showAddFoursquare(){
        $(".tab").removeClass("current_tab");
        $("#addFoursquare").toggleClass("current_tab");
        var iframe = document.getElementById("addFoursquare");
        iframe.src = "addFoursquareApp.php";
    }

    function showEditFoursquare(){
        $(".tab").removeClass("current_tab");
        $("#editFoursquare").toggleClass("current_tab");
        var iframe = document.getElementById("editFoursquare");
        iframe.src = "edit-remove-foursquare.php";
    }

    function resizeIframe(obj) {
        obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
    }
    $("#logout").click(function(){
        document.location = "logout.php";
    });
</script>
</html>

