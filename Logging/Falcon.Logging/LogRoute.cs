namespace Falcon.Logging
{
    public enum LogRoute
    {
        /// <summary>
        /// re-log-api 
        /// </summary>
        Api,

        /// <summary>
        /// re-log-job
        /// </summary>
        Scanner,

        /// <summary>
        /// re-log-processor 
        /// </summary>
        Sorter,

        /// <summary>
        /// re-log-trace
        /// </summary>
        Trace,

        /// <summary>
        /// re-log-default
        /// </summary>
        Default,
    }
}