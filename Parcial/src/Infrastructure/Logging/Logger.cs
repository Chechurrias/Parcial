using System;

namespace Infrastructure.Logging
{
    public static class Logger
    {
        private static bool _enabled = true;

        // Propiedad pública para controlar lectura y escritura de Enabled
        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

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
