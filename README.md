# Sistema de Gestión de Productos y Transacciones

Este proyecto es un sistema completo para la gestión de productos y transacciones, compuesto por una API REST desarrollada en .NET 8.0 y una aplicación frontend en React.

## Requisitos

### Backend
- .NET 8.0 SDK
- SQL Server (LocalDB o instancia completa)
- Visual Studio 2022 o Visual Studio Code
- Entity Framework Core Tools

### Frontend
- Node.js (versión 16 o superior)
- npm o yarn
- Navegador web moderno (Chrome, Firefox, Edge)

### Herramientas de desarrollo
- Git
- Postman (opcional, para pruebas de API)

## Ejecución del Backend

1. **Clonar el repositorio**
   ```bash
   git clone [URL_DEL_REPOSITORIO]
   cd [NOMBRE_DEL_PROYECTO]/backend
   ```

2. **Restaurar paquetes NuGet**
   ```bash
   dotnet restore
   ```

3. **Configurar la cadena de conexión**
   - Abrir el archivo `appsettings.json`
   - Verificar y ajustar la cadena de conexión de la base de datos según su entorno

4. **Ejecutar migraciones**
   ```bash
   dotnet ef database update
   ```

5. **Ejecutar el proyecto**
   ```bash
   dotnet run
   ```

6. **Acceder a la documentación de la API**
   - URL: https://localhost:44344/swagger/index.html
   - La documentación Swagger estará disponible con todos los endpoints documentados

### Características del Backend
- **Arquitectura**: Patrón Repository
- **Documentación**: Swagger/OpenAPI
- **Mapeo de objetos**: AutoMapper
- **Migraciones**: Entity Framework Core
- **Framework**: ASP.NET Core Web API (.NET 8.0)

## Ejecución del Frontend

1. **Navegar al directorio del frontend**
   ```bash
   cd [NOMBRE_DEL_PROYECTO]/frontend
   ```

2. **Instalar dependencias**
   ```bash
   npm install
   ```
   o
   ```bash
   yarn install
   ```

3. **Configurar variables de entorno**
   - Crear un archivo `.env` en la raíz del proyecto frontend
   - Agregar la URL base del backend:
     ```
     VITE_API_BASE_URL=https://localhost:44344/api
     ```

4. **Ejecutar el proyecto en modo desarrollo**
   ```bash
   npm run dev
   ```
   o
   ```bash
   yarn dev
   ```

5. **Acceder a la aplicación**
   - URL: http://localhost:5173/

### Características del Frontend
- **Framework**: React
- **Arquitectura**: Atomic Design
- **Servidor de desarrollo**: Vite
- **Estilos**: CSS Modules/Styled Components (según implementación)

## Evidencias

### 1. Listado dinámico de productos y transacciones con paginación
![Listado de productos con paginación](./screenshots/productos-listado.png)
![Listado de transacciones con paginación](./screenshots/transacciones-listado.png)

*Descripción: Muestra la tabla de productos y transacciones con controles de paginación, permitiendo navegar entre diferentes páginas de resultados.*

### 2. Pantalla para la creación de productos
![Crear producto](./screenshots/producto-crear.png)

*Descripción: Formulario para crear nuevos productos con validación de campos y manejo de errores.*

### 3. Pantalla para la edición de productos
![Editar producto](./screenshots/producto-editar.png)

*Descripción: Formulario precargado con los datos del producto seleccionado para su modificación.*

### 4. Pantalla para la creación de transacciones
![Crear transacción](./screenshots/transaccion-crear.png)

*Descripción: Formulario para registrar nuevas transacciones con selección de productos y cálculos automáticos.*

### 5. Pantalla para la edición de transacciones
![Editar transacción](./screenshots/transaccion-editar.png)

*Descripción: Formulario de edición de transacciones existentes con todos los campos modificables.*

### 6. Pantalla de filtros dinámicos
![Filtros dinámicos](./screenshots/filtros-dinamicos.png)

*Descripción: Interface de filtrado que permite buscar y filtrar productos y transacciones por diferentes criterios.*

### 7. Pantalla para la consulta de información de un formulario (extra)
![Consulta de información](./screenshots/consulta-informacion.png)

*Descripción: Vista detallada que muestra información completa de un registro específico con todos sus campos y relaciones.*

## Estructura del Proyecto

```
proyecto/
├── backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Data/
│   └── Migrations/
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── atoms/
│   │   │   ├── molecules/
│   │   │   ├── organisms/
│   │   │   └── templates/
│   │   ├── pages/
│   │   ├── services/
│   │   └── utils/
│   └── public/
└── screenshots/
```

## Tecnologías Utilizadas

### Backend
- ASP.NET Core Web API (.NET 8.0)
- Entity Framework Core
- AutoMapper
- Swagger/OpenAPI
- SQL Server

### Frontend
- React
- Vite
- Axios (para llamadas HTTP)
- React Router (para navegación)

## Notas Adicionales

- Asegúrese de que el backend esté ejecutándose antes de iniciar el frontend
- Los puertos predeterminados son 44344 para el backend y 5173 para el frontend
- Para producción, considere configurar variables de entorno apropiadas y usar HTTPS
- La base de datos se crea automáticamente al ejecutar las migraciones
