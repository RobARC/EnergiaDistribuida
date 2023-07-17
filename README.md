# Energia Distribuida Proyecto Backend

## Contexto
Un cliente de una determinada empresa de distribución de energía busca obtener información ejecutiva que le permita tomar decisiones certeras sobre el mantenimiento y/o evaluación de los trabajos realizados en diferentes tramos en la distribución de energía eléctrica. Para ello, nos permitió tener información desde 2.010 a 2.020 de las diferentes características de los tramos en un documento excel. El documento esta divido en tres pestañas que dan información por tramos de consumo, costos y perdidas.

## Prerequisitos
<b>Base de Datos</b>

Debido a que esta es una información inicial, se debe plantear una estructura de base de datos de acuerdo al documento, también, asumiendo la carga inicial de los 10 años de historia. Esto con el motivo que mas adelante se puedan ingresar los periodos de tiempo hasta la fecha actual. Los tramos no varían, ya que no se han previsto expansiones (hasta el momento, pero entre mas escalable, mejor).
<li>Motor de Base de Datos MySQL, MariaDB o MIcrosoft SQL Server preferiblemente, relacionales obligatorio.</li>
<li>La estructura de la tabla o tablas queda a disposición del desarrollador. Tener presente que este diseño debe ser escalable</li>
<li>El modo de carga del excel queda a libre disposición del desarrollador, pero se requiere que todos los datos se encuentren presente en la base de datos. </li>

## Bcckend

Debe ser desarrollado en .NET (Obligatorio).
<li>La estructura de las respuestas queda a libre disposición del desarrollador.</li>

## Requerimientos

<b>Request 1: Histórico Consumos por Tramos</b>
Se envía una fecha inicial y una final (yyyy-MM-dd) y se debe de responder con una historia por cada tramo, que contenga el consumo, perdidas y costo por el consumo. Esto permite dar una contextualización al cliente de como han estado los datos en ese periodo de tiempo.

<b>Request 2: Histórico Consumos por Cliente (Residencial, Comercial, Industrial)</b>
Se envía una fecha inicial y una final (yyyy-MM-dd) y se debe de responder con una historia por cada tipo de usuario, que contenga el tramo, consumo, perdidas y costo por el consumo. Esta historia permite ver cual tramo genera mayores perdidas para tomar decisiones.

<b>Request 3: Top 20 Peores Tramos/Cliente</b>
Se envía una fecha inicial y una final (yyyy-MM-dd) y se debe de responder con un listado con los tramos/cliente con las mayores pérdidas. Este busca saber quién genera las mayores pérdidas y así planear un mantenimiento correctivo/preventivo, según sea el caso.

## Entregables

Archivo SQL que evidencie la estructura y los datos almacenados
Proyecto Visual Studio 







