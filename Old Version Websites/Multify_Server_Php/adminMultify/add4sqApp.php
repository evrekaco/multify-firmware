<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 28.01.2015
 * Time: 11:53
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
$appName       = mysqli_real_escape_string($connection,$_POST['appName']);
$clientID      = mysqli_real_escape_string($connection,$_POST['clientID']);
$clientSecret   = mysqli_real_escape_string($connection,$_POST['clientSecret']);
$query = "SELECT clientID FROM FoursquareApps where clientID='".$clientID."';";
$result = mysqli_query($connection,$query);
$numResults = mysqli_num_rows($result);
if($numResults>=1){ //eklenen app db de var mı?
    $message = $appName." already exist!!";
}
else{
    $sql = "insert into FoursquareApps(appName,clientID,clientSecret,usageCount) values('".$appName."','".$clientID."','".$clientSecret."',0);";
    mysqli_query($connection, $sql);
    $message = "Foursquare Application added Sucessfully!!";

}
mysqli_close($connection);
echo "<script type='text/javascript'>
    window.alert('$message');
    window.location.href='addFoursquareApp.php';
  </script>";
