using System;

namespace Infrastructure.Logging
{
using System;

namespace Infrastructure.Logging
{
    public static class Logger
    {
        // Auto-implemented property con valor por defecto true
        public static bool Enabled { get; set; } = true;

        public static void Log(string message)
        {
            if (!Enabled) return;
            Console.WriteLine("[LOG] " + DateTime.Now + " - " + message);
        }

        public static void Try(Action a)
        {
            try
            {
                a();
            }
            catch (Exception ex)
            {
                // Se ignora explícitamente la excepción porque es no crítica
                // y no se quiere interrumpir la ejecución. Se registra el error para seguimiento.
                Log("Ignored exception in Try: " + ex.Message);
            }
        }
    }
}
