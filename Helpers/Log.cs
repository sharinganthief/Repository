using System;
using log4net;
using log4net.Config;

namespace Helpers
{
    public static class Log
    {
        #region Logger Initialization

        private static ILog _logger;
        private static readonly log4net.Core.Level TRACELevelERROR = new log4net.Core.Level(40000, "ERROR");
        private static readonly log4net.Core.Level TRACELevelWARN = new log4net.Core.Level(30000, "WARN");
        private static readonly log4net.Core.Level TRACELevelTRACE = new log4net.Core.Level(10000, "TRACE");
        //private static readonly TaskQueue Queue = TaskQueue.Instance;


        static Log() { Initialize(); }

        private static void Initialize()
        {
            XmlConfigurator.Configure();
            LogManager.GetRepository().LevelMap.Add(TRACELevelERROR);
            LogManager.GetRepository().LevelMap.Add(TRACELevelWARN);
            LogManager.GetRepository().LevelMap.Add(TRACELevelTRACE);
            _logger = LogManager.GetLogger(typeof(Log));
        }

        #endregion

        #region Tracing

        //[Conditional("TRACE")]
        public static void Trace(string message, params object[] args)
        {
            Trace( message, string.Empty, null, args);
        }

        public static void Trace(string message, Exception ex, params object[] args)
        {
            Trace(message, string.Empty, ex, args);
        }

        public static void Trace(string message, string movieTitle, Exception ex, params object[] args)
        {
            ThreadContext.Properties["Title"] = movieTitle;
            Trace(LogCategory.LogTrace, message, ex, args);
        }

        //[Conditional("TRACE")]
        public static void Trace(LogCategory category, string message, Exception ex, params object[] args)
        {
            //Queue.Queue(delegate
            //{
            ThreadContext.Properties["Category"] = category;
            string messageToLog = string.Format(message, args);

            ConsoleLog(ConsoleColor.Gray, message, args);
            _logger.Logger.Log(typeof(Log), TRACELevelTRACE, messageToLog, ex);
            //});
        }



        #endregion

        #region Warn / Error

        public static void Warn(string message, params object[] args)
        {
            ThreadContext.Properties["Category"] = LogCategory.LogWarning;

            ConsoleLog(ConsoleColor.Yellow, message, args);
            _logger.Logger.Log(typeof(Log), TRACELevelWARN, string.Format(message, args), null);
            
        }

        public static void Error(Exception ex, params object[] args)
        {
            Error(ex.ToString(), args);
        }

        public static void Error(string message, params object[] args)
        {
            ThreadContext.Properties["Category"] = LogCategory.LogError;

            ConsoleLog(ConsoleColor.Red, message, args);
            _logger.Logger.Log(typeof(Log), TRACELevelERROR, string.Format(message, args), null);
        }

        #endregion

        #region Helpers

        private static void ConsoleLog(ConsoleColor color, string message, params object[] args)
        {
            try
            {
                ConsoleColor c = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine("    " + message, args);
                Console.ForegroundColor = c;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("{0}, InnerException = {1}",
                    ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : string.Empty));
            }
        }

        public static void WaitForLoggingComplete()
        {
            //Queue.WaitForProcessingComplete();
        }

        #endregion

        #region Tracing Archive

        //[Conditional("TRACE")]
        //public static void Trace<TRequest>(TRequest request, TraceStep traceStep) where TRequest : class, IRequest
        //{
        //    traceStep.TraceStepProcess = request.RequestContext.TraceStep.TraceStepProcess;

        //    // Set the current context's trace step to the value being passed in now
        //    if (request.RequestContext != null) request.RequestContext.TraceStep = traceStep;    

        //    // Bypass if tracing not enabled, but still set tracestep for context above for debugging when available
        //    if (!Configuration.Diagnostics.TracingEnabled) return;

        //    Queue.Queue(delegate
        //    {
        //        // Get trace step index for passed in trace step key by means of lookup table which is reflected above
        //        int reflectedTraceStepIndex = 0;
        //        Dictionary<string, int> traceStepsForType;
        //        if( TraceStepDefinitions.TryGetValue( request.RequestContext.TraceStep.TraceStepProcess, out traceStepsForType ) ) 
        //            if(!traceStepsForType.TryGetValue( traceStep.Key, out reflectedTraceStepIndex ))
        //            {
        //                traceStep.Key = "Unknown";
        //                reflectedTraceStepIndex = 0;
        //            }

        //        //Set insert parameters
        //        ThreadContext.Properties["SessionID"] = request.RequestContext == null ? string.Empty : request.RequestContext.SessionID.ToString();
        //        ThreadContext.Properties["RequestContextID"] = request.RequestContext == null ? string.Empty : request.RequestContext.RequestContextID.ToString();
        //        ThreadContext.Properties["MessageData"] = Serialization.Serialize( request );
        //        ThreadContext.Properties["MessageID"] = request.RequestContext == null ? string.Empty : request.RequestContext.MessageID.ToString();
        //        ThreadContext.Properties["TraceStep"] = reflectedTraceStepIndex;
        //        ThreadContext.Properties["TraceStepKey"] = traceStep.Key;
        //        ThreadContext.Properties["Category"] = LogCategory.RequestContextTrace;

        //        ConsoleLog(ConsoleColor.Gray, string.Format("Trace step - " + traceStep.Key));
        //        logger.Logger.Log(typeof(Log), traceLevelTRACE, string.Format("Trace step - " + traceStep.Key), null);

        //        //Reset for other log entries
        //        ThreadContext.Properties["SessionID"] = string.Empty;
        //        ThreadContext.Properties["RequestContextID"] = string.Empty;
        //        ThreadContext.Properties["MessageID"] = string.Empty;
        //        ThreadContext.Properties["TraceStep"] = null;
        //        ThreadContext.Properties["TraceStepKey"] = string.Empty;
        //        ThreadContext.Properties["MessageData"] = string.Empty;
        //        ThreadContext.Properties["Category"] = string.Empty;
        //    });
        //}
        //private const string ProcessingNamespace = "Sage.Cloud.Services.Processing";

        //private static Dictionary<Type, Dictionary<string, int>> _traceStepDefinitions;
        //private static Dictionary<Type, Dictionary<string, int>> TraceStepDefinitions
        //{
        //    get
        //    {
        //        if (_traceStepDefinitions == null)
        //        {
        //            // Populate using reflection ( using ITraceStep classes )
        //            Assembly assembly = Assembly.Load( ProcessingNamespace );
        //            string namespaceAsString = assembly.GetType( ProcessingNamespace + 
        //                ".TraceSteps.SubscriptionCreateNotificationSteps" ).Namespace;

        //            List<Type> processes = assembly.GetTypes().ToList().Where(t => 
        //                String.Equals(t.Namespace, namespaceAsString, StringComparison.Ordinal)).ToList();

        //            _traceStepDefinitions = processes.ToDictionary(
        //                process => process, 
        //                process => process.GetFields().Select((field, index) => 
        //                    new { field.Name, index }).ToDictionary(step => step.Name, step => step.index));
        //        }
        //        return _traceStepDefinitions;
        //    }
        //}

        #endregion
    }
}
