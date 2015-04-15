<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 03:57
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
$devices = $_POST['devices'];
$cid = $_POST['client'];
$appID = $_POST['foursquareApps'];
if(isset($devices)){
    $str = "";
    for($i = 0; $i < count($devices); $i++){
        if($i == count($devices)-1){
            $str .= $devices[$i];
        }
        else{
            $str .= $devices[$i].",";
        }
    }
    $sql = "Select * from Devices WHERE did IN ($str);";
    $strSQL = mysqli_query($connection, $sql);
    $sqlUpdateApp = "Select usageCount from FoursquareApps where id=$appID;";
    $strSQL2 = mysqli_query($connection, $sqlUpdateApp);
    $usageCountArray = mysqli_fetch_array($strSQL2);
    $usageCount = $usageCountArray['usageCount'];
    while($Results = mysqli_fetch_array($strSQL)){
        $sql = "Insert into Multify VALUES (".$Results['did'].",".$cid.",0,$appID);";
        mysqli_query($connection,$sql);
        $usageCount++;
        if($usageCount == 3){
            break;
        }
    }
    $sql = "Update FoursquareApps SET usageCount=$usageCount where id=$appID;";
    mysqli_query($connection, $sql);
    mysqli_close($connection);
    header("Location: link.php");
}