<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 02.01.2015
 * Time: 03:49
 */
include("db.php");
session_start();
$venueName  = mysqli_real_escape_string($connection, $_POST['venueName']);
$name       = mysqli_real_escape_string($connection,$_POST['name']);
$email      = mysqli_real_escape_string($connection,$_POST['email']);
$phone      = mysqli_real_escape_string($connection,$_POST['phone']);
$message    = mysqli_real_escape_string($connection,$_POST['message']);
$sql = "insert into Subscribers(venueName, name, email, phone, message) values('".$venueName."','".$name."','".$email."','".$phone."','".$message."');";
mysqli_query($connection, $sql);
mysqli_close($connection);
if($_SESSION['lang'] == "tr"){
    echo    "<script type='text/javascript'>
            window.location.href='index.php';
        </script>";
}
else{
    echo    "<script type='text/javascript'>
            window.location.href='index_en.php';
        </script>";
}

