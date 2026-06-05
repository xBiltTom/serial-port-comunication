using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunicacionRedes
{
    class Trama
    {
        public const int TAMANIO_TRAMA = 1024;
        public const int TAMANIO_CABECERA = 5;
        public const byte BYTE_RELLENO = 64; //Se rellena la trama con @'s

        public byte[] crearTramaMensaje(string mensaje)
        {
            byte[] trama = new byte[TAMANIO_TRAMA];
            
            for (int i=0; i<TAMANIO_TRAMA; i++)
            {
                trama[i] = BYTE_RELLENO;
            }

            byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);

            if(mensajeBytes.Length > TAMANIO_TRAMA - TAMANIO_CABECERA)
            {
                throw new Exception("El mensaje es demasiado largo para una trama de 1024 bytes.");
            }

            string longitud = mensajeBytes.Length.ToString("D4");

            string cabecera = "M" + longitud;

            byte[] cabeceraBytes = Encoding.UTF8.GetBytes(cabecera);

            Array.Copy(cabeceraBytes, 0, trama, 0, TAMANIO_CABECERA);
            Array.Copy(mensajeBytes, 0, trama, TAMANIO_CABECERA, mensajeBytes.Length);

            return trama;
        }

        public string extraerMensaje(byte[] trama)
        {
            string tipoTrama = Encoding.UTF8.GetString(trama, 0, 1);
            if(tipoTrama != "M")
            {
                throw new Exception("La trama recibida no es de tipo mensaje.");
            }

            string longitudTexto = Encoding.UTF8.GetString(trama, 1, 4);
            int longitudMensaje = Convert.ToInt32(longitudTexto);
            string mensaje = Encoding.UTF8.GetString(trama, TAMANIO_CABECERA, longitudMensaje);
            return mensaje;
        }

    }
}
