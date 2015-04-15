<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 20.12.2014
 * Time: 23:53
 */
include('db.php');
session_start();
$username = mysqli_real_escape_string($connection,$_POST['username']);
$password = mysqli_real_escape_string($connection,$_POST['password']);
$strSQL = mysqli_query($connection,"select name from Admin where username='".$username."' and password='".$password."'");
$Results = mysqli_fetch_array($strSQL);

if(count($Results)>=1)
{
    $sql = "select id from Admin where username ='".$username."'";
    $result = mysqli_query($connection, $sql);
    $row = mysqli_fetch_array($result);
    $id = $row['id'];
    $message = $Results['name']." Login Sucessfully!!";
    $_SESSION['id'] = $id;
    echo "<script type='text/javascript'>
        window.alert('$message');
        window.location.href='admin_panel.php';
      </script>";
}
else
{
    $message = "Invalid email or password!!";
    echo "<script type='text/javascript'>
        window.alert('$message');
        window.location.href='index.html';
      </script>";
}
