<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 11.01.2015
 * Time: 03:41
 */
function getDistance($src, $dest){
    //$url = "http://maps.googleapis.com/maps/api/directions/json?origin=".str_replace(' ', '+', $src)."&destination=".str_replace(' ', '+', $dest)."&sensor=false";
    $url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=".$src."&destinations=".$dest."&key=AIzaSyBIXRGOKcqcXv_1UPCEburEdriVcmQA0Os";
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_PROXYPORT, 3128);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
    $response = curl_exec($ch);
    curl_close($ch);
    $response_all = json_decode($response);
    $elements = $response_all->rows[0]->elements;
    $distances = array();
    for($i = 0; $i < count($elements); $i++){
        $distance = $elements[$i]->distance->value;
        $distances[$i] = $distance;
    }
    //$distance = $response_all->routes[0]->legs[0]->distance->value;
    return $distances;
}
$file = fopen("coordinates.txt","r") or die("Unable to open coordinates.txt");
$coordinateArray = array();
$i = 0;
while($line = fgets($file)){
    if($i == 80){
        break;
    }
    $coordinateArray[$i] = str_replace("\n","",$line);
    $i++;
}
$coordinates = array();
$counter = 0;
//for($i = 0; $i < count($coordinateArray); $i++){
//    $temp = array();
//    for($j = 0; $j < count($coordinateArray); $j++){
//        if($i == $j){
//            $temp[$j] = 0;
//        }
//        else{
//            $temp[$j] = getDistance($coordinateArray[$i],$coordinateArray[$j]);
//            usleep(500000);
//        }
//    }
//    $coordinates[$i] = $temp;
//}
$fromRow = $_GET['fromRow'];
$toRow = $_GET['toRow'];
$fromColumn = $_GET['fromColumn'];
$toColumn = $_GET['toColumn'];
$index = 0;
for($i = $fromRow; $i < $toRow; $i++){
    $origin = $coordinateArray[$i];
    $destination = "";
    for($j = $fromColumn; $j < $toColumn; $j++){
        if($j == count($coordinateArray)-1){
            $destination = $destination.$coordinateArray[$j];
        }
        else{
            $destination = $destination.$coordinateArray[$j]."|";
        }
    }
    $coordinates[$index] = getDistance($origin,$destination);
    //var_dump($destination);
    $index++;
}
$toWrite = fopen("matrix.txt", "w") or die("Unable to open file!!");
for($i = 0; $i < count($coordinates); $i++){
    for($j = 0; $j < count($coordinates[$i]); $j++){
        $counter++;
        if($j != count($coordinates[$i]) - 1){
            fwrite($toWrite,$coordinates[$i][$j]);
            fwrite($toWrite,"-");
        }
        else{
            fwrite($toWrite,$coordinates[$i][$j]);
            fwrite($toWrite,"-");            
            fwrite($toWrite,"\n");
        }

    }
}
//getDistance("40.167266,31.920403","40.166907,31.920091");
//$source_address = "40.170474,31.925930";
//$destination_address = "40.167600,31.920340";
//getDistance($source_address,$destination_address);

//var_dump($coordinates);

fclose($file);
fclose($toWrite);