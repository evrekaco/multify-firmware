<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 28.01.2015
 * Time: 12:20
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
$appName = $_POST['appName'];
$clientID = $_POST['clientID'];
$clientSecret = $_POST['clientSecret'];

$sql = "update FoursquareApps set appName='$appName', clientID='$clientID', clientSecret = '$clientSecret' where id=$selected";
//var_dump($sql);
//exit;
mysqli_query($connection,$sql);
mysqli_close($connection);
header("location: edit-remove-foursquare.php");