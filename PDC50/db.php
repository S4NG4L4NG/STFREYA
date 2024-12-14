<?php
$servername = "localhost";
$username = "testuser1";
$password = "testuser";
$dbname = "st_freya";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    echo("Connection failed: " . $conn->connect_error);
} 

?>

