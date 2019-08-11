using System;
using System.Data.Entity.Validation;


namespace mARkIt.Backend
{
    public static class Utils
    {
        public static void LogDbEntityValidationException(DbEntityValidationException ex)
        {
            Log("Logging the details of DbEntityValidationException:");

            foreach (var eve in ex.EntityValidationErrors)
            {
                Log($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");

                foreach (var ve in eve.ValidationErrors)
                {
                    Log($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }
            }
        }

        public static void LogException(Exception ex)
        {
            Log($"{ex.GetBaseException().GetType().Name}: {ex.GetBaseException().Message}");
            /*
            Exception currentEx = ex;

            Log($"Printing exception occured in {ex.Source}:");

            while(currentEx != null)
            {
                Log($"{ex.GetType().Name}: {Environment.NewLine}{ ex.Message}");
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Log("Inner exception found: ");
                }

                currentEx = currentEx.InnerException;
            } 
            */
        }

        public static void Log(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
    }
}