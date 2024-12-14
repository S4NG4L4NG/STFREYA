<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'db.php';

$data = json_decode(file_get_contents("php://input"));
//file_put_contents("log.txt", print_r($data, true));
$student_id = $data->studentId;
$course = $data->course;
$yearLevel = $data->yearlevel;
$date = $data->date;

$sql = "INSERT INTO academic_history (student_id, course, yearlevel, date) VALUES ($student_id, '$course', '$yearLevel', '$date')";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Academic history record added successfully"]);
} else {
    echo json_encode(["message" => "Error adding record: " . $conn->error]);
}

$conn->close();
?>