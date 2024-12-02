<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

// Retrieve data from JSON payload
$student_id = $data->student_id;
$name = $data->name;
$lastname = $data->lastname;
$age = $data->age;
$email = $data->email;
$contactno = $data->contactno;
$course = $data->course;

// Prepare the SQL query to update the student record
$sql = "UPDATE student_list 
        SET name='$name', lastname='$lastname', age=$age, email='$email', contactno='$contactno', course='$course' 
        WHERE student_id=$student_id";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Student updated successfully"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>
