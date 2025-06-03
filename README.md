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

### 1. Documentación de la API con Swagger
![Documentación API Swagger](https://res-console.cloudinary.com/dhkmjpq1h/thumbnails/v1/image/upload/v1748988107/Y2FwX2FwaV9taDJ5YmU=/drilldown)

*Descripción: Documentación completa de la API REST mostrando todos los endpoints disponibles para la gestión de productos y transacciones, con ejemplos de request/response y posibilidad de probar los endpoints directamente desde la interfaz de Swagger.*

### 2. Pantalla de Login
![Pantalla de Login](https://res.cloudinary.com/dhkmjpq1h/image/upload/v1748988106/cap_2_wifsen.png)

*Descripción: Interface de autenticación del sistema con campos para usuario y contraseña, incluyendo validaciones y manejo de errores de acceso.*

### 3. Dashboard Principal
![Dashboard](https://res.cloudinary.com/dhkmjpq1h/image/upload/v1748988106/cap_3_yacoz7.png)

*Descripción: Panel principal del sistema que muestra un resumen general con estadísticas, gráficos y accesos rápidos a las principales funcionalidades del sistema.*

### 4. Gestión de Productos - Listado dinámico con paginación
![Gestión de Productos](https://res.cloudinary.com/dhkmjpq1h/image/upload/v1748988106/cap_4_qlgsjx.png)

*Descripción: Pantalla principal de gestión de productos que incluye:*
- *Listado dinámico de productos con paginación*
- *Controles para crear, editar y eliminar productos*
- *Funciones de búsqueda y filtrado*
- *Visualización de información detallada de cada producto*

### 5. Gestión de Transacciones - Listado dinámico con paginación
![Gestión de Transacciones](https://res.cloudinary.com/dhkmjpq1h/image/upload/v1748988107/cap_8_pjmnw8.png)

*Descripción: Pantalla de gestión de transacciones que incluye:*
- *Listado dinámico de transacciones con paginación*
- *Controles para crear, editar y eliminar transacciones*
- *Filtros dinámicos para búsqueda avanzada*
- *Visualización detallada de cada transacción con información del producto asociado*

### Funcionalidades Demostradas

Las evidencias mostradas comprueban la implementación completa de:

✅ **Listado dinámico de productos y transacciones con paginación**  
✅ **Pantalla para la creación de productos**  
✅ **Pantalla para la edición de productos**  
✅ **Pantalla para la creación de transacciones**  
✅ **Pantalla para la edición de transacciones**  
✅ **Pantalla de filtros dinámicos**  
✅ **Documentación completa de la API**  
✅ **Sistema de autenticación**  
✅ **Dashboard con información consolidada**

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
└── README.md
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
- La documentación completa de la API está disponible en Swagger UI
- Para producción, considere configurar variables de entorno apropiadas y usar HTTPS
- La base de datos se crea automáticamente al ejecutar las migraciones
- El sistema incluye autenticación y autorización implementadas
- Todas las operaciones CRUD están completamente funcionales con validaciones
