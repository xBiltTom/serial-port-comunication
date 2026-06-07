using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunicacionRedes
{
    // Representa los datos extraidos de una trama recibida
    // Encapsula limpiamente el tipo, canal e información útil
    public struct Paquete
    {
        public char Tipo; // Ya sea tipo M: mensaje, I: inicio de archivo, A: archivo, F: fin de archivo
        public int ID;
        public byte[] Datos;
        public int LongitudDatos;
    }

    class Trama
    {
        public const int TAMANIO_TRAMA = 1024;
        public const int TAMANIO_CABECERA = 6; //Cambio de tamaño debido al nuevo campo para el id del archivo a enviar
        public const int TAMANIO_DATOS = TAMANIO_TRAMA - TAMANIO_CABECERA; // 1018 de datos debido a los 6 bytes ocupados por la cabecera
        public const byte BYTE_RELLENO = 64; //Se rellena la trama con @'s

        // Crea una trama de tipo mensaje con el formato: [M][longitud(4 dígitos)][mensaje][relleno]
        // ID es siempre 0 para mensajes, ya que no se necesita un identificador de archivo
        public byte[] crearTramaMensaje(string mensaje)
        {
            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            if (datos.Length > TAMANIO_DATOS)
            {
                throw new Exception($"Mensaje demasiado largo. Máximo {TAMANIO_DATOS} bytes");
            }

            return construirTrama('M', 0, datos, datos.Length);
        }

        // Crea una trama de tipo inicio de archivo con el formato: [I][ID(4 dígitos)][longitudNombre(2 dígitos)][nombreArchivo][relleno]
        // Transporta el nombre original del archivo a enviar, necesario para que el receptor pueda guardar el archivo con su nombre correcto
        public byte[] crearTramaInicio(int idCanal, string nombreArchivo)
        {
            byte[] datos = Encoding.UTF8.GetBytes(nombreArchivo);
            if(datos.Length > TAMANIO_DATOS)
            {
                throw new Exception("Nombre de archivo demasiado largo.");
            }

            return construirTrama('I', idCanal, datos, datos.Length);
        }

        // Crea una trama de tipo archivo con el formato: [A][ID(4 dígitos)][datosArchivo][relleno]       
        // Bytes validos indica cuántos bytes del chunk son realmente datos útiles, ya que el último fragmento de un archivo puede no llenar completamente la sección de datos de la trama
        public byte[] crearTramaFragmento(int idCanal, byte[] chunk, int bytesValidos)
        {
            if(bytesValidos > TAMANIO_DATOS)
            {
                throw new Exception($"Fragmento demasiado grande. Máximo {TAMANIO_DATOS} bytes");
            }
            return construirTrama('A', idCanal, chunk, bytesValidos);
        }

        // Crea una trama de tipo fin de archivo con el formato: [F][ID(4 dígitos)][relleno]
        // Sin datos útiles, solo indica que el archivo con el ID especificado ha terminado de enviarse
        public byte[] crearTramaFin(int idCanal)
        {
            return construirTrama('F',idCanal, new byte[0], 0);
        }

        public Paquete extraerPaquete(byte[] trama)
        {
            if(trama.Length != TAMANIO_TRAMA)
            {
                throw new Exception("La trama no tiene el tamaño correcto.");
            }

            // Byte 0 : tipo de trama (M, I, A, F)
            char tipo = (char)trama[0];

            // Byte 1 : ID (dígito ASCII 'O'- '5') 
            int id = trama[1] - '0'; // Convertir el byte del ID a un número entero (asumiendo que es un dígito ASCII)

            string longitudTexto = Encoding.UTF8.GetString(trama, 2, 4);
            int longitud = Convert.ToInt32(longitudTexto);

            byte[] datos = new byte[longitud]; 
            if(longitud > 0)
                Array.Copy(trama, TAMANIO_CABECERA, datos, 0, longitud);

            return new Paquete
            {
                Tipo = tipo,
                ID = id,
                Datos = datos,
                LongitudDatos = longitud
            };
        }


        // Construye el array d 1024 bytes con cabecera + datos + relleno.
        // Estructura: [Tipo(1 byte)][ID(1 byte)][Longitud(4 bytes)][Datos útiles][Relleno con '@']
        private byte[] construirTrama(char tipo, int idCanal, byte[] datos, int bytesValidos)
        {
            byte[] trama = new byte[TAMANIO_TRAMA];

            // Rellenar todo con '@' primero
            for (int i = 0; i < TAMANIO_TRAMA; i++)
                trama[i] = BYTE_RELLENO;

            // Cabecera: Tipo
            trama[0] = (byte)tipo;

            // Cabecera: ID (como carácter ASCII, ej. '0', '1' … '5')
            trama[1] = (byte)('0' + idCanal);

            // Cabecera: Longitud en 4 dígitos
            byte[] longBytes = Encoding.UTF8.GetBytes(bytesValidos.ToString("D4"));
            Array.Copy(longBytes, 0, trama, 2, 4);

            // Datos útiles a partir del byte 6
            if (bytesValidos > 0)
                Array.Copy(datos, 0, trama, TAMANIO_CABECERA, bytesValidos);

            return trama;
        }        

    }
}
