<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 02.01.2015
 * Time: 17:58
 */
include("db.php");
session_start();
$email      = mysqli_real_escape_string($connection,$_POST['email']);
$sql = "insert into Emails(email) values('".$email."');";
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