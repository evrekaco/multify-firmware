<?php
/**
 * Created by PhpStorm.
 * User: murat
 * Date: 26.12.2014
 * Time: 15:39
 */
include("vendor/autoload.php");
include("db.php");
$pathToCertificateFile = __DIR__."/vendor/haxx-se/curl/cacert.pem";
//var_dump($pathToCertificateFile);
$client = new \TheTwelve\Foursquare\HttpClient\CurlHttpClient($pathToCertificateFile);
$redirector = new \TheTwelve\Foursquare\Redirector\HeaderRedirector();
$factory = new \TheTwelve\Foursquare\ApiGatewayFactory($client, $redirector);
$sql = "Select Client.code4sq AS code4sq, Client.cid AS cid, FoursquareApps.clientID AS clientID, FoursquareApps.clientSecret AS clientSecret From Multify JOIN Client ON Multify.cid = Client.cid JOIN FoursquareApps ON Multify.appID = FoursquareApps.id;";
$strSQL = mysqli_query($connection, $sql);
// Required for most requests
//$factory->setClientCredentials('2TIDIJJHKVGNO3KKT1RTAWXXB14FXXGJJ4VJWVAWT4TBTXNB', 'ODP2GUHBVWLWPAIU1RZSN32AUILNXBEICFQQ3WLJ3WEKKSTU');//setClientCredentials(clientID, ClientSecret);
$factory->setEndpointUri('https://api.foursquare.com');

$factory->useVersion(2);
if($_GET['venue_id'] != ""){
    $venueid = $_GET['venue_id'];
    $gateway = $factory->getVenuesGateway();
    $venue = $gateway -> getVenue($venueid);
    $venueName = $venue->name;
    $venueCheckIns = $venue->stats->checkinsCount;
    echo "<script>window.alert('VenueName = $venueName , totalCheckIn = $venueCheckIns')</script>";
}
while($Result = mysqli_fetch_array($strSQL)){
    $venueid = $Result['code4sq'];
    $cid = $Result['cid'];
    $clientID = $Result['clientID'];
    $clientSecret = $Result['clientSecret'];


    $factory->setClientCredentials($clientID,$clientSecret);
    $gateway = $factory->getVenuesGateway();
    var_dump($gateway);
    $venue = $gateway -> getVenue($venueid);
    //var_dump($venue);
    $venueCheckIns = $venue->stats->checkinsCount;
    $sql = "Update Multify set 4sqCheckInCount=$venueCheckIns where cid=$cid;";
    mysqli_query($connection, $sql);
}

mysqli_close($connection);