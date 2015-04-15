<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 16:44
 */
$i = 0;
while(true){
    if($i >= 60/3){
        break;
    }
    else{
        exec("/usr/local/bin/php -q /home/multify/public_html/adminMultify/multify.php");
    }
    $i++;
    sleep(3);
}
