<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));
$student_ids = $data->student_ids; // Array of IDs

if (!empty($student_ids)) {
    $ids = implode(',', array_map('intval', $student_ids));
    $sql = "DELETE FROM student_list WHERE student_id IN ($ids)";

    if ($conn->query($sql) === TRUE) {
        echo json_encode(["message" => "Students deleted successfully"]);
    } else {
        echo json_encode(["message" => "Error: " . $conn->error]);
    }
} else {
    echo json_encode(["message" => "No students selected"]);
}

$conn->close();
?>
