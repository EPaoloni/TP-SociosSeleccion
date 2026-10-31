# TP Principal – Socios de la Selección Argentina

Programación Avanzada II – UNLaM – 2026-2C

Sistema web (ASP.NET Core MVC, .NET 8, C#) para administrar los **socios de la Selección Argentina** (entidad principal)
y sus **inscripciones para ir a los partidos** (entidad de relación).

## Cómo ejecutarlo

Requisitos: **.NET 8 SDK** (o Visual Studio 2022 con la carga "Desarrollo de ASP.NET y web").

- **Visual Studio:** abrir `SociosSeleccion.sln`, elegir `SociosSeleccion.Web` como proyecto de inicio y presionar F5.
- **Consola:**
  ```bash
  dotnet run --project SociosSeleccion.Web
  ```

La base de datos es **SQLite** (`socios.db`), se crea sola la primera vez con datos de ejemplo
(4 socios, 5 partidos y 2 inscripciones). No hace falta instalar SQL Server.

**Usuarios de prueba:**

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `1234` | Administrador |
| `juan.perez` | `1234` | Socio (N° 1) |

Los demás socios de ejemplo todavía no tienen usuario: pueden crearlo desde "Crealo acá" en la pantalla de login
(por ejemplo N° 333, DNI 40555666).

> Si cambia la estructura de la base, borrar `SociosSeleccion.Web/socios.db` y se vuelve a crear al iniciar.

## Roles y pantallas

| Pantalla | Administrador | Socio |
|---|---|---|
| Socios (ABM + buscador + detalle) | ✔ | |
| Inscripciones (todas, anotar a cualquier socio) | ✔ | |
| **Usuarios**: lista de usuarios con los partidos a los que está inscripto cada uno | ✔ | |
| **Partidos**: lista con cupo, disponibles e inscriptos; detalle de cada partido | ✔ (ve los socios inscriptos) | ✔ (puede anotarse) |
| **Cargar y modificar partidos** | ✔ | |
| **Mis partidos**: sus inscripciones, anotarse y cancelar | | ✔ |
| Crear usuario (registro con N° de socio + DNI) | | ✔ (sin login) |

Si un socio intenta entrar a una pantalla del administrador, se muestra "Acceso denegado".

## Arquitectura en n capas

```
SociosSeleccion.Web        → Presentación (Controllers, Views, Layout, PartialViews, Login)
        │
SociosSeleccion.Negocio    → Servicios / reglas de negocio (SocioServicio, InscripcionServicio, UsuarioServicio)
        │
SociosSeleccion.Datos      → Acceso a datos (AppDbContext con EF Core + Repositorios)

SociosSeleccion.Entidades  → Entidades del negocio (Socio, Inscripcion, Partido, Usuario), usada por todas las capas
```

Las capas se comunican mediante interfaces y métodos que **envían y reciben objetos**
(ej.: `SociosController` → `ISocioServicio.Crear(Socio)` → `ISocioRepositorio.Agregar(Socio)`).
La presentación nunca accede directamente a la base de datos.

## Cumplimiento de la consigna

| Requisito | Dónde está |
|---|---|
| Alta de socios | `SociosController.Create` + `Views/Socios/Create.cshtml` |
| Modificación | `SociosController.Edit` + `Views/Socios/Edit.cshtml` |
| Baja | `SociosController.Delete` + `Views/Socios/Delete.cshtml` (confirmación) |
| Listado con buscador (≥ 2 campos) | `Views/Socios/Index.cshtml`: busca por **apellido/nombre**, **DNI** y **categoría** |
| Entidad de relación + alta asociada | `Inscripcion` (socio ↔ partido): `InscripcionesController.Create`, también desde el detalle del socio ("Anotar a un partido") |
| Login | `AccountController` + autenticación por cookies con roles (Administrador / Socio); todas las páginas requieren login (`AuthorizeFilter` global en `Program.cs`) y las de administración `[Authorize(Roles = "Administrador")]` |
| ≥ 10 campos | `Socio`: N° socio, nombre, apellido, DNI, fecha de nacimiento, email, teléfono, domicilio, provincia, categoría, fecha de alta (11) |
| Campo combo | **Provincia** y **Categoría** (y en la inscripción: socio, partido y sector) |
| Campo fecha validado | **Fecha de nacimiento** y **Fecha de alta**: `[DataType(Date)]` + atributo propio `[FechaValida]` (no futura, año mínimo) |
| Numérico 1 a 1000 | **N° de socio**: `[Range(1, 1000)]` |
| Todos obligatorios (data annotations) | `[Required]` en todos los campos de `Socio` e `Inscripcion` |
| Layout | `Views/Shared/_Layout.cshtml` |
| PartialView | `Views/Socios/_SocioForm.cshtml` (formulario compartido por alta y edición) `Views/Partidos/_PartidoForm.cshtml` (alta y edición de partidos) y `Views/Shared/_InscripcionesSocio.cshtml` (usada en el detalle del socio y en "Mis partidos") |

## Reglas de negocio (capa Negocio)

**Socios**
- El número de socio y el DNI no pueden repetirse.
- La fecha de alta no puede ser anterior a la fecha de nacimiento.
- La categoría **Menor** es solo para menores de 18 años, y los menores de 18 deben tener esa categoría.
- Al eliminar un socio se eliminan sus inscripciones.

**Partidos**
- La fecha tiene que ser posterior a hoy (y a no más de 5 años), y no puede haber dos partidos el mismo día.
- La competición se elige de una lista (combo) y el rival no puede ser Argentina.
- Al modificar, el cupo no puede quedar por debajo de las entradas ya reservadas.
- Un partido que ya se jugó no se puede modificar.

**Inscripciones a partidos**
- Solo se puede anotar a partidos que todavía no se jugaron.
- Un socio no puede anotarse dos veces al mismo partido.
- Se piden entre 1 y 4 entradas, y no se puede superar el cupo disponible del partido.
- Un socio solo puede cancelar sus propias inscripciones.

**Usuarios**
- Para crear su usuario, el socio debe ingresar un N° de socio y DNI que coincidan con un socio existente.
- Cada socio puede tener un solo usuario, y el nombre de usuario no se puede repetir.
- Las contraseñas se guardan hasheadas (PBKDF2), nunca en texto plano.
- Al eliminar un socio se elimina también su usuario.
