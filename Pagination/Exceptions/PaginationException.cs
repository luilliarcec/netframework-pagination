using System;

namespace Luilliarcec.Pagination.Exceptions
{
    public class PaginationException : Exception
    {
        //
        // Resumen:
        //     Inicializa una nueva instancia de la clase System.Exception.
        public PaginationException() : base()
        {
        }

        //
        // Resumen:
        //     Inicializa una nueva instancia de la clase System.Exception con el mensaje de
        //     error especificado.
        //
        // Parámetros:
        //   message:
        //     Mensaje que describe el error.
        public PaginationException(string message) : base(message)
        {
        }
    }
}
