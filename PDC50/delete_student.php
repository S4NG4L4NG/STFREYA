<?php
include 'db.php';

// Decode JSON input
$data = json_decode(file_get_contents("php://input"));

// Retrieve student_id from the JSON payload
$student_id = $data->student_id;

// Prepare SQL query to delete the record
$sql = "DELETE FROM student_list WHERE student_id=$student_id";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Student deleted successfully"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

// Close the database connection
$conn->close();
?>