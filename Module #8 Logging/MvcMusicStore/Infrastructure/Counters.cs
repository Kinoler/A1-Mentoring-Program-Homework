using System.Collections.Generic;
using PerformanceCounterHelper;

namespace MvcMusicStore.Service
{
    [PerformanceCounterCategory(
        "Counter", 
        System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, 
        "None")]
    public enum Counters
    {
        [PerformanceCounter(
            "LogIn", 
            "Count of user which login to the site.", 
            System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        LogIn,

        [PerformanceCounter(
            "LogOut",
            "Count of user which logout from the site.",
            System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        LogOut,

        [PerformanceCounter(
            "Register",
            "Count of user which register in to the site.",
            System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        Register
    }
}
