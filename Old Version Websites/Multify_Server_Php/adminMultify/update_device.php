<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 03:35
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
$deviceID = $_POST['deviceID'];

$sql = "update Devices set dev_name='$name', deviceID='$deviceID' where did=$selected";
//var_dump($sql);
//exit;
mysqli_query($connection,$sql);
mysqli_close($connection);
header("location: editDevice.php");