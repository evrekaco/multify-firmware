<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 25.01.2015
 * Time: 19:30
 */
 function countDigits( $str )
{
    return preg_match_all( "/[0-9]/", $str );
}
$dev_id = $_POST['dev_id'];
$value = $_POST['value'];
//$fsq = $_POST['fsq'];
$digitCount = strlen($value);
$numArray = array("0","1","2","3","4","5","6","7","8","9");
if($digitCount <= 6){
  $flag = 1;
  for($i = 0; $i < $digitCount; $i++){
    if(!in_array($value[$i],$numArray)){
        $flag = 0;
        echo "<script type='text/javascript'>
        window.alert('Geçersiz değer girdiniz!');
        window.location.href='fix.php';
        </script>";
        break;
    }
  }
  if($flag == 1){
    $access_token = "4cf127806204bc9c0599bf7451b3aba0653aa3d0";
    $function_name = "Changer";
    $url = "https://api.spark.io/v1/devices/$dev_id/$function_name";
    $ch = curl_init();
    curl_setopt($ch,CURLOPT_URL,$url);
    curl_setopt($ch,CURLOPT_POST,1);
    curl_setopt($ch,CURLOPT_POSTFIELDS,"access_token=$access_token&args=$value");
    curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
    $server_output = curl_exec($ch);
    curl_close($ch);
    echo "<script type='text/javascript'>
    window.alert('İşlem başarılı !');
    window.location.href='fix.php';
    </script>";
  }
  // $access_token = "d0f0188db952635198c57b060291c7c8b7bdd69f";
  // $function_name = "Changer";
  // $url = "https://api.spark.io/v1/devices/$dev_id/$function_name";
  // $ch = curl_init();
  // curl_setopt($ch,CURLOPT_URL,$url);
  // curl_setopt($ch,CURLOPT_POST,1);
  // curl_setopt($ch,CURLOPT_POSTFIELDS,"access_token=$access_token&args=$value");
  // curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
  // curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
  // curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
  // $server_output = curl_exec($ch);
  // curl_close($ch);
  // echo "<script type='text/javascript'>
  // window.alert('İşlem başarılı !');
  // window.location.href='fix.php';
  // </script>";
}
else{
  echo "<script type='text/javascript'>
  window.alert('Girilien sayı 6 basamaktan büyük !');
  window.location.href='fix.php';
  </script>";
}
// if (preg_match('/[A-Z]/', $value) || preg_match('/[a-z]/', $value) || ctype_space ($value))
// {
//   echo "<script type='text/javascript'>
//   window.alert('Geçersiz değer girdiniz!');
//   window.location.href='fix.php';
//   </script>";
// }
// else{
//   $digitCount = countDigits($value);
//   if($digitCount <= 6){
//     $access_token = "d0f0188db952635198c57b060291c7c8b7bdd69f";
//     $function_name = "Changer";
//     $url = "https://api.spark.io/v1/devices/$dev_id/$function_name";
//     $ch = curl_init();
//     curl_setopt($ch,CURLOPT_URL,$url);
//     curl_setopt($ch,CURLOPT_POST,1);
//     curl_setopt($ch,CURLOPT_POSTFIELDS,"access_token=$access_token&args=$value");
//     curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
//     curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
//     curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
//     $server_output = curl_exec($ch);
//     curl_close($ch);
//     echo "<script type='text/javascript'>
//     window.alert('İşlem başarılı !');
//     window.location.href='fix.php';
//     </script>";
//   }
//   else{
//     echo "<script type='text/javascript'>
//     window.alert('Girilien sayı 6 basamaktan büyük !');
//     window.location.href='fix.php';
//     </script>";
//   }
// }



//    header("Location: fix.php");
