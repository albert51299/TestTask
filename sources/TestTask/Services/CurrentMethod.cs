using System.Diagnostics;
using System.Reflection;

namespace TestTask.Services {
    public static class CurrentMethod {
        public static string GetName() {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            return methodBase.DeclaringType.Name + "." + methodBase.Name;
        }
    }
}
