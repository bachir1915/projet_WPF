<?php
$db_name = "bdpersonne";
$db_server = "127.0.0.1";
$db_user = "root";
$db_pass = "";

$db = new PDO(
    "mysql:host=$db_server;dbname=$db_name;charset=utf8",
    $db_user,
    $db_pass
);

$db->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

// CORS Headers to allow requests from the C# frontend and other clients
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");
?>
