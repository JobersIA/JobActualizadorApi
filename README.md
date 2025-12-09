# 🔄 JobActualizadorApi

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**API de prueba para sistema de autoactualización**

[Características](#-características) •
[Instalación](#-instalación) •
[Endpoints](#-endpoints) •
[Configuración](#-configuración)

</div>

---

## 📋 Descripción

JobActualizadorApi es una API de prueba diseñada para desarrollar y probar el sistema de autoactualización que se utiliza en las aplicaciones móviles de Jobers (JobFichador, JobRutas, etc.).

Permite experimentar con el control de versiones y la distribución de actualizaciones sin afectar a las APIs en producción.

### Características principales

- 🔢 **Control de versiones**: Gestión de versiones por plataforma (Android/iOS)
- 📥 **Distribución de APKs**: Endpoint para descarga de archivos de actualización
- ⚠️ **Actualizaciones forzadas**: Soporte para versiones mínimas requeridas
- 📝 **Notas de versión**: Información sobre cambios en cada versión
- 🔓 **Sin autenticación**: API simplificada para pruebas (sin JWT)
- 📚 **Swagger UI**: Documentación interactiva incluida

---

## 🛠️ Tecnologías

- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - Infraestructura REST
- **Swashbuckle** - Documentación OpenAPI/Swagger
- **CORS habilitado** - Permite peticiones desde cualquier origen

---

## 📋 Requisitos previos

- .NET 8.0 SDK o superior
- Visual Studio 2022 o VS Code

---

## 🚀 Instalación

1. **Navega al directorio:**
```bash
cd JobActualizadorApi/JobActualizadorApi
```

2. **Restaura los paquetes:**
```bash
dotnet restore
```

3. **Ejecuta la aplicación:**
```bash
dotnet run
```

4. **Accede a Swagger UI:**
```
http://localhost:5000/swagger
```

---

## 📡 Endpoints

### Sistema

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api` | Health check |
| GET | `/api/version` | Información de versión de la API |

### Versiones de App

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Sistema/GetAppVersion/{plataforma}` | Obtener versión para Android/iOS |
| PUT | `/api/Sistema/UpdateAppVersion` | Actualizar versión de una plataforma |
| GET | `/api/Sistema/GetAllVersions` | Listar todas las versiones configuradas |

### Descargas

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Sistema/Download/{fileName}` | Descargar archivo APK |

---

## 📖 Ejemplos de uso

### Health Check

```http
GET /api
```

**Respuesta:**
```
JobActualizadorApi funcionando y lista para recibir solicitudes...
```

### Información de versión de la API

```http
GET /api/version
```

**Respuesta:**
```json
{
  "version": "1.0.0",
  "compatible_versions": ["1.0.0"],
  "min_client_version": "1.0.0",
  "framework": ".NET 8.0",
  "title": "JobActualizadorApi"
}
```

### Obtener versión de App para Android

```http
GET /api/Sistema/GetAppVersion/Android
```

**Respuesta:**
```json
{
  "versionActual": "1.0.1",
  "versionMinima": "1.0.0",
  "urlDescarga": "http://localhost:5000/api/Sistema/Download/JobActualizador101.apk",
  "notasVersion": "Primera version de prueba del sistema de autoactualizacion",
  "actualizacionForzada": false
}
```

### Actualizar versión

```http
PUT /api/Sistema/UpdateAppVersion
Content-Type: application/json

{
  "plataforma": "Android",
  "versionActual": "1.0.2",
  "versionMinima": "1.0.0",
  "urlDescarga": "http://localhost:5000/api/Sistema/Download/JobActualizador102.apk",
  "notasVersion": "Nueva version con mejoras",
  "actualizacionForzada": false,
  "activo": true,
  "fechaPublicacion": "2025-12-09T00:00:00"
}
```

**Respuesta:**
```json
{
  "success": true,
  "message": "Version actualizada correctamente"
}
```

### Listar todas las versiones

```http
GET /api/Sistema/GetAllVersions
```

**Respuesta:**
```json
{
  "Android": {
    "versionActual": "1.0.1",
    "versionMinima": "1.0.0",
    "urlDescarga": "http://localhost:5000/api/Sistema/Download/JobActualizador101.apk",
    "notasVersion": "Primera version de prueba",
    "actualizacionForzada": false
  },
  "iOS": {
    "versionActual": "1.0.1",
    "versionMinima": "1.0.0",
    "urlDescarga": "",
    "notasVersion": "Primera version de prueba",
    "actualizacionForzada": false
  }
}
```

### Descargar APK

```http
GET /api/Sistema/Download/JobActualizador101.apk
```

**Respuesta:** Archivo binario APK

---

## 📁 Estructura del Proyecto

```
JobActualizadorApi/
├── Controllers/
│   └── SistemaController.cs    # Controlador de versiones y descargas
├── Models/
│   ├── AppVersionInfo.cs       # Modelo de información de versión
│   └── UpdateAppVersionRequest.cs  # Request para actualizar versión
├── Downloads/                  # Carpeta para archivos APK
├── Program.cs                  # Configuración y startup
├── appsettings.json           # Configuración
└── README.md
```

---

## ⚙️ Configuración

### Puerto por defecto

La API escucha en el puerto **5000** por defecto. Puedes cambiarlo en `appsettings.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      }
    }
  }
}
```

### Agregar archivos APK

Coloca los archivos APK en la carpeta `Downloads/`:

```
JobActualizadorApi/
└── Downloads/
    ├── JobActualizador100.apk
    ├── JobActualizador101.apk
    └── JobActualizador102.apk
```

### Versiones iniciales

La API viene con versiones preconfiguradas en memoria:

| Plataforma | Versión Actual | Versión Mínima |
|------------|----------------|----------------|
| Android | 1.0.1 | 1.0.0 |
| iOS | 1.0.1 | 1.0.0 |

Puedes modificarlas usando el endpoint `PUT /api/Sistema/UpdateAppVersion`.

---

## 🎯 Casos de prueba

### Simular actualización disponible

1. Configura la app con versión `1.0.0` en `app.json`
2. La API devuelve versión `1.0.1`
3. La app detecta que hay actualización

### Simular actualización forzada

```http
PUT /api/Sistema/UpdateAppVersion
{
  "plataforma": "Android",
  "versionActual": "2.0.0",
  "versionMinima": "2.0.0",
  "urlDescarga": "...",
  "notasVersion": "Actualización obligatoria de seguridad",
  "actualizacionForzada": true
}
```

### Simular sin actualizaciones

Configura la app y el servidor con la misma versión.

---

## 🔒 Seguridad

Esta API es **solo para pruebas** y no incluye:
- Autenticación JWT
- Autorización por roles
- Validación de tokens
- HTTPS (usar proxy inverso en producción)

**No usar en producción sin implementar seguridad adicional.**

---

## 🧪 Testing con Swagger

1. Accede a `http://localhost:5000/swagger`
2. Explora los endpoints disponibles
3. Usa "Try it out" para probar cada endpoint
4. Verifica las respuestas en tiempo real

---

## ❓ Solución de problemas

### "Archivo no encontrado"
- Verificar que el APK existe en la carpeta `Downloads/`
- El nombre del archivo debe coincidir exactamente

### "Puerto en uso"
- Cambiar el puerto en `appsettings.json`
- O detener el proceso que usa el puerto 5000

### "CORS bloqueado"
- La API tiene CORS habilitado para todos los orígenes
- Si hay problemas, verificar la configuración en `Program.cs`

---

## 📧 Contacto

**Jobers y Asociados, S.L**
- Email: rsanfelix@jobers.net
- Teléfono: 626 99 09 26
- Web: [www.jobersweb.com](https://www.jobersweb.com/)

---

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - consulta el archivo [LICENSE](LICENSE) para más detalles.

Copyright (c) 2025 Jobers y Asociados, S.L. y Ramón San Félix Ramón

---

**Versión:** 1.0.0
**Framework:** .NET 8.0
**Última actualización:** 09-12-2025
