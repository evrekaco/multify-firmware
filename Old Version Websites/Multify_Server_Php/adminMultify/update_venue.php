<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 02:20
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
$name = $_POST['name'];
$email = $_POST['email'];
$password = $_POST['password'];
$foursquare = $_POST['4sqID'];

$sql = "update Client set name='$name', email='$email', password='".md5($password)."', code4sq='$foursquare' where cid=$selected";
//var_dump($sql);
//exit;
mysqli_query($connection,$sql);
mysqli_close($connection);
header("location: edit-remove.php");