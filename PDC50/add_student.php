<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

// Retrieve the data from the JSON payload
$name = $data->name;
$lastname = $data->lastname;
$age = $data->age;
$email = $data->email;
$contactNo = $data->contactno;
$course = $data->course;

// Prepare the SQL query to insert the data into the student_list table
$sql = "INSERT INTO student_list (name, lastname, age, email, contactno, course) 
        VALUES ('$name', '$lastname', $age, '$email', '$contactNo', '$course')";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Student added successfully"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>