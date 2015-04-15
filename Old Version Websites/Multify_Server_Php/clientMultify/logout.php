<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 05:09
 */
include('db.php');
session_start();
$id = $_SESSION['id'];
$sql = "select * from Client where cid=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
if(count($Results) < 1){
    header("Location: index.php");
}
session_destroy();
header("Location:index.php");