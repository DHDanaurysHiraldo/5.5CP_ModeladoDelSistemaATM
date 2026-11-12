# Mini ATM — Modelado POO (Caso práctico 5.5, Deitel)

**Materia:** Programación Orientada a Objetos I — Semana 5, Día 1
**Tipo:** Actividad individual · Análisis y programación de consola
**Tecnología:** C# · .NET 8 · Aplicación de consola

---

## 1. Objetivo

Producir un modelo inicial orientado a objetos de un cajero automático (ATM), identificando clases, estado, comportamiento y responsabilidades, y demostrar su viabilidad implementando dos de esas clases en C# (`Cuenta` y `Transaccion`), instanciándolas y usándolas desde `Program.cs`.

Este caso parte del **[Mini ATM V2](https://github.com/DHDanaurysHiraldo/MiniATM)**, desarrollado con programación estructurada, y da el siguiente paso: reconocer los **objetos** que participan en el dominio.

---

## 2. Análisis del dominio (RF-01 a RF-06)

### 2.1 Clases candidatas y justificación (RF-01, RF-02)

| Clase | ¿Por qué pertenece al modelo? |
| :--- | :--- |
| **ATM** | Es el controlador central: coordina al usuario, los dispositivos (pantalla, teclado, dispensador) y la cuenta durante una sesión. |
| **Cuenta** | Es el concepto financiero que el cliente viene a operar; almacena el saldo que se consulta, deposita o retira. |
| **Pantalla** | Todo cajero necesita un medio de salida para guiar al usuario y mostrarle resultados. |
| **Teclado** | Es el medio por el cual el usuario introduce opciones de menú y montos. |
| **DispensadorEfectivo** | Representa el componente físico que limita los retiros: aunque la cuenta tenga fondos, el cajero solo entrega el efectivo que posee. |
| **Transaccion** | Cada operación genera datos propios (tipo, monto y fecha) que deben registrarse y mostrarse como comprobante. |

### 2.2 Tabla consolidada (RF-03 a RF-06)

| Clase | Responsabilidad Principal | Justificación en el Modelo (RF-02) | Estado (Propiedades) | Comportamiento (Métodos) | Ejemplo de Objeto (Instancia) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **ATM** | Coordinar el flujo de interacción y las operaciones del cajero. | Actúa como el controlador central que conecta al usuario con los componentes del cajero y su cuenta. | `CuentaActual`, `CodigoCajero` | `MostrarMenu()`, `EjecutarOperacion()` | `cajeroPrincipal` (`CodigoCajero = "ATM-001"`, `CuentaActual = cuentaJuan`) |
| **Cuenta** | Mantener datos básicos y saldo de una cuenta. | Es indispensable para almacenar, validar y actualizar los fondos financieros del cliente. | `NumeroCuenta`, `Saldo`, `Activa` | `ConsultarSaldo()`, `Depositar()`, `Retirar()` | `cuentaJuan` (`NumeroCuenta = "10024589"`, `Saldo = 15500.50m`, `Activa = true`) |
| **Pantalla** | Presentar mensajes y resultados. | Permite la comunicación visual del sistema hacia el usuario para guiarlo en sus operaciones. | *Sin estado* (solo emite salidas visuales en tiempo real hacia la consola). | `MostrarMensaje()`, `MostrarSaldo()` | `pantallaPrincipal` (instancia de salida en consola) |
| **Teclado** | Capturar datos introducidos por el usuario. | Provee el medio de entrada para que el cliente ingrese opciones de menú y montos de dinero. | *Sin estado* (actúa como canal de entrada transitorio sin almacenar datos). | `LeerOpcion()`, `LeerMonto()` | `tecladoNumerico` (instancia de lectura por consola) |
| **DispensadorEfectivo** | Representar la disponibilidad y entrega de efectivo. | Modela el componente físico que controla y valida si hay billetes suficientes para un retiro. | `EfectivoDisponible`, `CapacidadMaxima` | `DispensarEfectivo()` | `dispensador` (`EfectivoDisponible = 50000.00m`, `CapacidadMaxima = 100000.00m`) |
| **Transaccion** | Representar los datos de una operación bancaria. | Permite registrar el historial y emitir comprobantes detallados de cada movimiento realizado. | `Tipo`, `Monto`, `Fecha` | `MostrarDetalle()` | `retiroHoy` (`Tipo = "Retiro"`, `Monto = 2000.00m`, `Fecha = DateTime.Now`) |

### 2.3 Clase vs. objeto (RN-06)

- **Clase:** el molde que define qué datos (propiedades) y qué acciones (métodos) tendrá cada objeto. Ejemplo: `Cuenta`.
- **Objeto:** una instancia concreta creada con `new`, con su propio estado. Ejemplo: `cuentaJuan` y `cuentaMaria` son dos objetos de la clase `Cuenta` con saldos independientes.

---

## 3. De programación estructurada a objetos (Mini ATM V2 → Caso 5.5)

En el Mini ATM V2 los **datos** de la cuenta eran variables sueltas en `Main` y las **reglas** vivían en una clase `static` que recibía el saldo por parámetro. En este caso, la clase `Cuenta` reúne ambos en un solo objeto.

| Mini ATM V2 (estructurado) | Caso 5.5 (objetos) |
| :--- | :--- |
| `string numeroCuenta` | `cuenta.NumeroCuenta` |
| `decimal saldoDisponible` | `cuenta.Saldo` |
| `bool estadoCuenta` + `CuentaEstaActiva()` | `cuenta.Activa` |
| `OperacionesBancarias.EsMontoValido(monto)` | `cuenta.EsMontoValido(monto)` |
| `OperacionesBancarias.TieneFondos(saldo, monto)` | `cuenta.TieneFondos(monto)` |
| `saldo = OperacionesBancarias.Depositar(saldo, monto)` | `cuenta.Depositar(monto)` |
| `saldo = OperacionesBancarias.Retirar(saldo, monto)` | `cuenta.Retirar(monto)` |
| `List<string>` con movimientos ya formateados | Objetos `Transaccion` con `Tipo`, `Monto` y `Fecha` separados |

### Decisiones de diseño

- **Métodos de instancia (no `static`):** los métodos de `Cuenta` usan el estado del propio objeto (`Saldo`, `Activa`), por lo que pertenecen a cada objeto y no a la clase.
- **`Depositar()` y `Retirar()` devuelven `bool`:** el objeto actualiza su propio saldo, así que el retorno se usa para indicar si la operación fue exitosa o rechazada. En V2, si un retiro no era válido se devolvía el mismo saldo y quien llamaba no podía distinguir el rechazo.
- **La regla de cuenta activa vive dentro de `Cuenta`:** en V2 esa validación dependía de `Program.cs`; ahora el objeto protege su propio estado sin importar desde dónde se le llame.
- **`decimal` para montos:** es el tipo adecuado para valores monetarios en C# (evita errores de redondeo de `double`).
- **`Fecha` como `DateTime`:** se guarda como fecha real y el formato se aplica solo al mostrarla.
- **`MostrarDetalle()` devuelve un `string`:** `Transaccion` prepara el texto, pero mostrarlo es responsabilidad de la capa de presentación (la clase `Pantalla` en el análisis).
- **Propiedades con `set` público y sin constructor:** se dejan así para cumplir RF-09 (asignar propiedades desde `Program.cs`). En una versión posterior, `Saldo` debería ser `private set` y modificarse solo mediante `Depositar()` y `Retirar()` (encapsulamiento y constructores, Día 2).

---

## 4. Alcance del prototipo

**Incluye**

- Análisis de las seis clases candidatas (tabla consolidada).
- Implementación en C# de dos clases: `Cuenta` y `Transaccion`.
- Creación de objetos con `new`, asignación y consulta de propiedades e invocación de métodos desde `Program.cs`.
- Salida por consola que evidencia el estado y el comportamiento de los objetos.

**Excluye** (BRD, sección 3 y RN-04/RN-05)

- Un cajero bancario completo y funcional (menú interactivo).
- Autenticación real, bases de datos y persistencia.
- Herencia, polimorfismo, interfaces y patrones avanzados.
- Las clases `ATM`, `Pantalla`, `Teclado` y `DispensadorEfectivo` quedan **solo a nivel de análisis**.

---

## 5. Implementación (RF-07 a RF-10)

### 5.1 Estructura del proyecto

```
5.5CP_ModeladoDelSistemaATM/
├── 5-5CP_ModeladoDelSistemaATM.csproj   # Proyecto de consola .NET 8
├── 5-5CP_ModeladoDelSistemaATM.slnx     # Solución
├── Cuenta.cs                            # Clase Cuenta
├── Transaccion.cs                       # Clase Transaccion
├── Program.cs                           # Crea y usa los objetos
└── README.md
```

### 5.2 Clases implementadas

**`Cuenta`**

| Miembro | Tipo | Descripción |
| :--- | :--- | :--- |
| `NumeroCuenta` | `string` | Identificador de la cuenta. |
| `Saldo` | `decimal` | Fondos disponibles. |
| `Activa` | `bool` | Indica si la cuenta puede operar. |
| `ConsultarSaldo()` | `decimal` | Devuelve el saldo actual. |
| `EsMontoValido(monto)` | `bool` | Verifica que el monto sea mayor que cero. |
| `TieneFondos(monto)` | `bool` | Verifica que el saldo cubra el monto. |
| `Depositar(monto)` | `bool` | Suma el monto si la cuenta está activa y el monto es válido. |
| `Retirar(monto)` | `bool` | Resta el monto si la cuenta está activa, el monto es válido y hay fondos. |

**`Transaccion`**

| Miembro | Tipo | Descripción |
| :--- | :--- | :--- |
| `Tipo` | `string` | Tipo de operación (Depósito, Retiro). |
| `Monto` | `decimal` | Monto de la operación. |
| `Fecha` | `DateTime` | Momento en que ocurrió. |
| `MostrarDetalle()` | `string` | Devuelve el comprobante formateado. |

### 5.3 Qué demuestra `Program.cs`

| Sección de la salida | Qué demuestra | Requisito |
| :--- | :--- | :--- |
| Creación de `cuentaJuan`, `cuentaMaria`, `depositoHoy`, `retiroHoy` | Objetos creados con `new` | RF-08 |
| 1) Estado inicial | Asignación y consulta de propiedades | RF-09 |
| 2) Operaciones sobre `cuentaJuan` | Depósito y retiro exitosos; rechazos por fondos insuficientes y monto inválido | RF-09, RF-10 |
| 3) Operación sobre `cuentaMaria` | Rechazo por cuenta inactiva | RF-09, RF-10 |
| 4) Propiedades de `retiroHoy` | Estado de un objeto `Transaccion` | RF-09, RF-10 |
| 5) Comprobantes | Comportamiento de `MostrarDetalle()` | RF-09, RF-10 |
| 6) Clase vs. objeto | Dos objetos de la misma clase con estados independientes | RN-06 |

---

## 6. Ejecución

### 6.1 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior.

```bash
dotnet --version
```

### 6.2 Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/DHDanaurysHiraldo/5.5CP_ModeladoDelSistemaATM.git

# 2. Entrar a la carpeta del proyecto
cd 5.5CP_ModeladoDelSistemaATM

# 3. Compilar
dotnet build

# 4. Ejecutar
dotnet run
```

También puede abrirse `5-5CP_ModeladoDelSistemaATM.slnx` en Visual Studio 2022 o superior y ejecutar con **F5**.

### 6.3 Salida esperada

```
=================================================
  CASO 5.5 — MODELADO ATM: CLASES Y OBJETOS
=================================================

1) ESTADO INICIAL DE LAS CUENTAS
-------------------------------------------------
cuentaJuan  -> No. 10024589 | Saldo: RD$ 15,500.50 | Activa: True
cuentaMaria -> No. 20031177 | Saldo: RD$ 3,000.00 | Activa: False

2) OPERACIONES SOBRE cuentaJuan
-------------------------------------------------
Depósito de RD$ 1,000.00  -> EXITOSO   | Saldo: RD$ 16,500.50
Retiro de RD$ 2,000.00    -> EXITOSO   | Saldo: RD$ 14,500.50
Retiro de RD$ 50,000.00   -> RECHAZADO: fondos insuficientes
Depósito de RD$ -500.00   -> RECHAZADO: el monto debe ser mayor que cero

3) OPERACIÓN SOBRE cuentaMaria (cuenta inactiva)
-------------------------------------------------
Depósito de RD$ 500.00    -> RECHAZADO: la cuenta no está activa | Saldo: RD$ 3,000.00

4) PROPIEDADES DEL OBJETO retiroHoy
-------------------------------------------------
Tipo:  Retiro
Monto: RD$ 2,000.00
Fecha: 12/11/2026

5) COMPROBANTES CON MostrarDetalle()
-------------------------------------------------
12/11/2026 22:31 | Depósito   | RD$   1,000.00
12/11/2026 22:31 | Retiro     | RD$   2,000.00

6) CLASE vs. OBJETO
-------------------------------------------------
Ambas cuentas salen de la misma clase Cuenta,
pero cada objeto mantiene su propio estado:
Saldo final cuentaJuan:  RD$ 14,500.50
Saldo final cuentaMaria: RD$ 3,000.00

Fin de la demostración.
```

> La fecha y la hora varían según el momento de ejecución. El separador de miles y decimales puede variar según la configuración regional del equipo.

---

## 7. Pruebas y criterios de aceptación

| Caso | Criterio | Evidencia | Estado |
| :--- | :--- | :--- | :---: |
| CP-01 | Se identifican seis clases y su propósito. | Sección 2.1 | ✅ |
| CP-02 | Cada clase tiene métodos y estado o justificación de ausencia de estado. | Sección 2.2 | ✅ |
| CP-03 | Se presentan dos ejemplos concretos de instancias. | Sección 2.2 (`cuentaJuan`, `retiroHoy`, entre otros) | ✅ |
| CP-04 | La tabla consolidada es completa y coherente. | Sección 2.2 | ✅ |
| CP-05 | El proyecto compila con al menos dos clases propias. | `Cuenta.cs`, `Transaccion.cs` | ✅ |
| CP-06 | `Program.cs` crea objetos con `new`. | `Program.cs` | ✅ |
| CP-07 | Se asignan propiedades y se invocan métodos. | `Program.cs`, secciones 1 a 5 de la salida | ✅ |
| CP-08 | La consola evidencia los resultados esperados. | Sección 6.3 | ✅ |
| CP-09 | README explica objetivo, alcance y ejecución. | Este documento | ✅ |
| CP-10 | El enlace de GitHub permite revisar los entregables. | [Repositorio](https://github.com/DHDanaurysHiraldo/5.5CP_ModeladoDelSistemaATM) | ✅ |

---

## 8. Matriz de trazabilidad

| Requerimientos | Evidencia | Pruebas |
| :--- | :--- | :--- |
| RF-01 a RF-06 | Sección 2 (análisis y tabla consolidada) | CP-01 a CP-04 |
| RF-07 a RF-10 | `Cuenta.cs`, `Transaccion.cs`, `Program.cs` y salida por consola | CP-05 a CP-08 |
| RF-11 a RF-12 | Este README y el repositorio GitHub | CP-09 a CP-10 |