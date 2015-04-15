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
$sql = "select * from Admin where id=$id";
$strSQL = mysqli_query($connection, $sql);
$Results = mysqli_fetch_array($strSQL);
$selected = $_POST['selected'];
if(count($Results) < 1){
    header("Location: index.html");
}
session_destroy();
header("Location:index.html");