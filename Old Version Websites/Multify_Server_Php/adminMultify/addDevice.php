<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 03:00
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
$name       = mysqli_real_escape_string($connection,$_POST['name']);
$deviceID      = mysqli_real_escape_string($connection,$_POST['deviceID']);
$query = "SELECT deviceID FROM Devices where deviceID='".$deviceID."';";
$result = mysqli_query($connection,$query);
$numResults = mysqli_num_rows($result);
if($numResults>=1){ // cihaz db de var mı?
    $message = $name." already exist!!";
}
else{
    $sql = "insert into Devices(dev_name,deviceID) values('".$name."','".$deviceID."');";
    mysqli_query($connection, $sql);
    //mysqli_query();
    $message = "Device is added sucessfully!!";

}
mysqli_close($connection);
echo "<script type='text/javascript'>
    window.alert('$message');
    window.location.href='add-device.php';
  </script>";
