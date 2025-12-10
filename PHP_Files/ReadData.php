<?php
header('Content-Type: application/json');

// Credenciales
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_DATABASE', 'pvz_héroes');

// Respuesta inicial
$response = [
    "success" => false,
    "message" => "Ocurrió un error desconocido.",
    "Hero" => [],
    "Carta" => [],
    "Sobre" => []
];

// Leer JSON desde Unity
$data = json_decode(file_get_contents("php://input"), true);

// Recoger el campo Name de forma segura
$nombre_Heroe_a_buscar = trim($data["Name"] ?? "");
$nombre_Carta_a_buscar = trim($data["Name"] ?? "");
$nombre_Sobre_a_buscar = trim($data["Name"] ?? "");


// Conexión
$conn = new mysqli(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_DATABASE);

if ($conn->connect_error) {
    $response["message"] = "Error de conexión a la BD: " . $conn->connect_error;
    echo json_encode($response);
    exit();
}

//////////////////////////////////////////////////
//             CONSULTA HÉROES
//////////////////////////////////////////////////

if ($nombre_Heroe_a_buscar !== "") {

    $sql = "SELECT ID_Héroe, Nombre, ID_Bando, ID_Clase1, ID_Clase2, Descripción 
            FROM héroes 
            WHERE Nombre = ?";

    $stmt = $conn->prepare($sql);

    if ($stmt !== false) {

        $stmt->bind_param("s", $nombre_Heroe_a_buscar);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($row = $result->fetch_assoc()) {
            $response["Hero"][] = [
                "IDHeroe"     => (string) $row['ID_Héroe'],
                "Name"        => (string) $row['Nombre'],
                "Bando"       => (string) $row['ID_Bando'],
                "Clase1"      => (string) $row['ID_Clase1'],
                "Clase2"      => (string) $row['ID_Clase2'],
                "Descripcion" => (string) $row['Descripción']
            ];
        }

        $stmt->close();
    }
}

//////////////////////////////////////////////////
//             CONSULTA CARTAS
//////////////////////////////////////////////////

if ($nombre_Carta_a_buscar !== "") {

    $sql = "SELECT ID_Carta, Nombre, ID_Bando, ID_Clase, Tipo_Carta, ID_Coleccion, Rareza, Coste, Fuerza, Vida, Habilidad 
            FROM cartas 
            WHERE Nombre = ?";

    $stmt = $conn->prepare($sql);

    if ($stmt !== false) {

        $stmt->bind_param("s", $nombre_Carta_a_buscar);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($row = $result->fetch_assoc()) {
            $response["Carta"][] = [
                "IDCarta"    => (string) $row['ID_Carta'],
                "Name"       => (string) $row['Nombre'],
                "Bando"      => (string) $row['ID_Bando'],
                "Clase"      => (string) $row['ID_Clase'],
                "TipoCarta"  => (string) $row['Tipo_Carta'],
                "Coleccion"  => (string) $row['ID_Coleccion'],
                "Rareza"     => (string) $row['Rareza'],
                "Coste"      => (string) $row['Coste'],
                "Fuerza"     => (string) $row['Fuerza'],
                "Vida"       => (string) $row['Vida'],
                "Habilidad"  => (string) $row['Habilidad']
            ];
        }

        $stmt->close();
    }
}
//////////////////////////////////////////////////
//             CONSULTA SOBRES
//////////////////////////////////////////////////

if ($nombre_Sobre_a_buscar !== "") {

    $sql = "SELECT ID_Sobre, Nombre, Precio_Gemas, Precio_Soles, Descripción
            FROM sobres 
            WHERE Nombre = ?";

    $stmt = $conn->prepare($sql);

    if ($stmt !== false) {

        $stmt->bind_param("s", $nombre_Sobre_a_buscar);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($row = $result->fetch_assoc()) {
            $response["Sobre"][] = [
                "IDSobre"    => (string) $row['ID_Sobre'],
                "Name"       => (string) $row['Nombre'],
                "PrecioGemas"      => (string) $row['Precio_Gemas'],
                "PrecioSoles"      => (string) $row['Precio_Soles'],
                "Descripcion"  => (string) $row['Descripción'],
            ];
        }

        $stmt->close();
    }
}



//////////////////////////////////////////////////
//        MENSAJE FINAL SEGÚN RESULTADOS
//////////////////////////////////////////////////

if (!empty($response["Hero"]) || !empty($response["Carta"]) || !empty($response["Sobre"])) {
    $response["success"] = true;
    $response["message"] = "Datos encontrados.";
} else {
    $response["success"] = true;
    $response["message"] = "No se encontraron resultados para: " . $nombre_Heroe_a_buscar;
}


// Cerrar conexión
$conn->close();

// Enviar respuesta
echo json_encode($response);

?>
