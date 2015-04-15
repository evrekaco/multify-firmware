<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 20.12.2014
 * Time: 23:53
 */
include('db.php');
session_start();
$email = mysqli_real_escape_string($connection,$_POST['email']);
$password = mysqli_real_escape_string($connection,$_POST['password']);
$strSQL = mysqli_query($connection,"select name from Client where email='".$email."' and password='".md5($password)."'");
$Results = mysqli_fetch_array($strSQL);

if(count($Results)>=1)
{
    $sql = "select cid from Client where email ='".$email."'";
    $result = mysqli_query($connection, $sql);
    $row = mysqli_fetch_array($result);
    $id = $row['cid'];
    $message = $Results['name']." Girisi Basarili";
    $_SESSION['id'] = $id;
    echo "<script type='text/javascript'>
        window.alert('$message');
        window.location.href='client_panel.php';
      </script>";
}
else
{
    $message = "Giris Hatali";
    echo "<script type='text/javascript'>
        window.alert('$message');
        window.location.href='index.php';
      </script>";
}
