<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'db.php';

$student_id = $_GET['student_id'];

$sql = "SELECT * FROM academic_history WHERE student_id = $student_id ORDER BY date DESC";
$result = $conn->query($sql);

$history = [];
while ($row = $result->fetch_assoc()) {
    $history[] = $row;
}

echo json_encode($history);

$conn->close();
?>