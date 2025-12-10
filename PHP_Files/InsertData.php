<?php
header('Content-Type: application/json');

// Credenciales
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_DATABASE', 'pvz_héroes'); // Cambié a tu DB real

// Inicializamos el objeto final de respuesta HeroResponse
$response = [
    "success" => false,
    "message" => "Ocurrió un error desconocido.",
    "Hero" => []
];

// Leer JSON desde Unity
$data = json_decode(file_get_contents("php://input"), true);

// Campos del héroe
$pName = $data['Name'] ?? '';
$pBando = $data['Bando'] ?? 0;
$pClase1 = $data['Clase1'] ?? 0;
$pClase2 = $data['Clase2'] ?? 0;
$pDescripcion = $data['Descripcion'] ?? '';

// Crear conexión
$conn = new mysqli(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_DATABASE);

// Verificar la conexión
if ($conn->connect_error) {
    $response["message"] = "Error de conexión a la BD: " . $conn->connect_error;
    die(json_encode($response));
}

// Preparar la consulta SQL (CALL al Stored Procedure CREATE_HERO)
$sql = "CALL CREATE_HERO(?,?,?,?,?)";

// Usar Prepared Statements
$stmt = $conn->prepare($sql);

// Verificar si la preparación falló
if ($stmt === false) {
    $response["message"] = "Error al preparar la consulta: " . $conn->error;
} else {
    // Asignar parámetros y ejecutar
    $stmt->bind_param("siiis", $pName, $pBando, $pClase1, $pClase2, $pDescripcion);

    if ($stmt->execute()) {
        // Éxito en la inserción
        $response["success"] = true;
        $response["message"] = "Héroe '{$pName}' insertado correctamente.";
    } else {
        // Error de ejecución
        $response["message"] = "Error al insertar el héroe: " . $stmt->error;
    }

    // Cerrar statement
    $stmt->close();
}

// Cerrar conexión
$conn->close();

// Devolver el JSON final
echo json_encode($response);
?>
