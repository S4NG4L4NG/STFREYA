<?php
include 'db.php';

// Decode the JSON payload
$data = json_decode(file_get_contents("php://input"));

// Retrieve the data from the JSON payload
$student_id = $data->student_id;
$date = $data->date;
$status = $data->status;

// Check if a record already exists
$check_sql = "SELECT * FROM attendance WHERE student_id = $student_id AND date = '$date'";
$result = $conn->query($check_sql);

if ($result->num_rows > 0) {
    // If record exists, update it
    $update_sql = "UPDATE attendance SET status = '$status' WHERE student_id = $student_id AND date = '$date'";
    if ($conn->query($update_sql) === TRUE) {
        echo json_encode(["message" => "Attendance record updated successfully"]);
    } else {
        echo json_encode(["message" => "Error updating record: " . $conn->error]);
    }
} else {
    // If no record exists, insert a new one
    $insert_sql = "INSERT INTO attendance (student_id, date, status) 
                   VALUES ($student_id, '$date', '$status')";
    if ($conn->query($insert_sql) === TRUE) {
        echo json_encode(["message" => "Attendance record added successfully"]);
    } else {
        echo json_encode(["message" => "Error adding record: " . $conn->error]);
    }
}

$conn->close();
?>