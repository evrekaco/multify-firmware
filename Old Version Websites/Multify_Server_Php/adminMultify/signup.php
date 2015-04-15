<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 20.12.2014
 * Time: 23:53
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
$name       = mysqli_real_escape_string($connection,$_POST['name']);
$email      = mysqli_real_escape_string($connection,$_POST['email']);
$password   = mysqli_real_escape_string($connection,$_POST['password']);
$foursquareVenueID = mysqli_real_escape_string($connection,$_POST['4sqID']);
$query = "SELECT code4sq FROM Client where code4sq='".$foursquareVenueID."';";
$result = mysqli_query($connection,$query);
$numResults = mysqli_num_rows($result);
if($numResults>=1){
    $message = $name." already exist!!";
}
else{
    $sql = "insert into Client(name,email,password,code4sq) values('".$name."','".$email."','".md5($password)."','".$foursquareVenueID."');";
    mysqli_query($connection, $sql);
    //mysqli_query();
    $message = "Signup Sucessfully!!";

}
mysqli_close($connection);
echo "<script type='text/javascript'>
    window.alert('$message');
    window.location.href='add.php';
  </script>";

