<?php
include 'db.php';

// Decode the JSON payload
$data = json_decode(file_get_contents("php://input"));

// Retrieve the data from the JSON payload
$name = $data->name;
$lastname = $data->lastname;
$age = $data->age;
$email = $data->email;
$contactNo = $data->contactno;
$course = $data->course;
$yearLevel = $data->yearlevel;
$gender = $data->gender; // Added gender

// Prepare the SQL query to insert the data into the student_list table
$sql = "INSERT INTO student_list (name, lastname, age, email, contactno, course, yearlevel, gender) 
        VALUES ('$name', '$lastname', $age, '$email', '$contactNo', '$course', '$yearLevel', '$gender')";

if ($conn->query($sql) === TRUE) {
    $student_id = $conn->insert_id; // Get the last inserted ID
    echo json_encode([
        "message" => "Student added successfully",
        "student_id" => $student_id
    ]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>
