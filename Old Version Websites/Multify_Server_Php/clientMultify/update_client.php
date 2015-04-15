<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 22:55
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Client where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.php");
}
else{
    $pass = $_POST['password'];
    $sql = "Update Client set password='".md5($pass)."' where cid=$id";
    mysqli_query($connection,$sql);
    header("Location: edit_profile.php");
}
